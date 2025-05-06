using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PersonalOrganizer
{
    public partial class MainForm : Form, IReminderObserver
    {
        private User _currentUser;
        private UserService _userService;
        private NoteCsvService _noteService; // Eklendi
        private Timer _reminderCheckTimer;
        private ReminderService _reminderService;
        private List<Reminder> _activeReminders = new List<Reminder>();
        private NotifyIcon notifyIcon; // NotifyIcon burada tanımlanıyor

        public MainForm(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;

            // NotifyIcon oluştur
            CreateNotifyIcon();

            // Hatırlatıcı sistemini kur
            SetupReminderNotifications();
        }

        public MainForm(User user, UserService userService)
        {
            InitializeComponent();
            _currentUser = user;
            _userService = userService;

            // Not servisini oluştur
            _noteService = new NoteCsvService();

            // Gerekirse boş UserId'leri bu kullanıcıya ata
            _noteService.FixNotesUserIdIfMissing(_currentUser.Id.ToString());

            // Form başlığını güncelle
            this.Text = $"Kişisel Organizör - {_currentUser.FirstName} {_currentUser.LastName}";

            // NotifyIcon oluştur
            CreateNotifyIcon();

            // Hatırlatıcı sistemini kur
            SetupReminderNotifications();
        }

        private void CreateNotifyIcon()
        {
            // NotifyIcon'ı kod ile oluştur
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = this.Icon;
            notifyIcon.Visible = true;
            notifyIcon.Text = "Kişisel Organizatör";

            // Context menü oluştur
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Hatırlatıcıları Göster", null, NotifyIcon_ShowReminders);
            contextMenu.Items.Add("Kapat", null, NotifyIcon_Exit);
            notifyIcon.ContextMenuStrip = contextMenu;
        }

        private void SetupReminderNotifications()
        {
            // Reminder servisi oluştur
            _reminderService = new ReminderService();

            // Observer pattern için kaydol
            ReminderSubject.Instance.AddObserver(this);

            // Timer ayarla - her dakikada kontrol edecek
            _reminderCheckTimer = new Timer();
            _reminderCheckTimer.Interval = 60000; // 60 saniye
            _reminderCheckTimer.Tick += ReminderCheckTimer_Tick;
            _reminderCheckTimer.Start();

            // İlk hatırlatıcıları yükle
            LoadActiveReminders();
        }

        private void LoadActiveReminders()
        {
            if (_currentUser != null)
            {
                try
                {
                    var reminders = _reminderService.GetRemindersByUserId(_currentUser.Id);
                    ReminderSubject.Instance.SetReminders(reminders);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hatırlatıcılar yüklenirken hata oluştu: {ex.Message}", "Hata",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // IReminderObserver arayüzünden implemente edilen metot
        public void UpdateReminders(List<Reminder> reminders)
        {
            _activeReminders = reminders;
            UpdateNotifyIconTooltip();
        }

        private void UpdateNotifyIconTooltip()
        {
            if (_activeReminders == null || _activeReminders.Count == 0)
            {
                notifyIcon.Text = "Kişisel Organizatör - Aktif hatırlatıcı yok";
                return;
            }

            // Bugün ve yarına ait hatırlatıcılar
            var upcomingReminders = _activeReminders
                .Where(r => (r.Date.Date == DateTime.Today || r.Date.Date == DateTime.Today.AddDays(1)))
                .OrderBy(r => r.Date)
                .ThenBy(r => r.Time)
                .Take(3)
                .ToList();

            if (upcomingReminders.Count > 0)
            {
                string tooltipText = "Yaklaşan Hatırlatıcılar:\n";
                foreach (var reminder in upcomingReminders)
                {
                    tooltipText += $"• {reminder.Date.ToShortDateString()} {reminder.Time.ToShortTimeString()}: {reminder.Summary}\n";
                }

                // NotifyIcon metnini max 63 karakterle sınırlandır
                notifyIcon.Text = tooltipText.Length > 63 ? tooltipText.Substring(0, 60) + "..." : tooltipText;

                // Baloncuk bildirimi göster
                notifyIcon.ShowBalloonTip(5000, "Hatırlatıcılar", tooltipText, ToolTipIcon.Info);
            }
            else
            {
                notifyIcon.Text = "Kişisel Organizatör - Yaklaşan hatırlatıcı yok";
            }
        }

        private void ReminderCheckTimer_Tick(object sender, EventArgs e)
        {
            // Aktif hatırlatıcıları kontrol et
            CheckForDueReminders();
        }

        private void CheckForDueReminders()
        {
            if (_activeReminders == null || _activeReminders.Count == 0)
                return;

            DateTime now = DateTime.Now;
            var dueReminders = _activeReminders
                .Where(r => r.Date.Date == now.Date &&
                           Math.Abs((r.Time.TimeOfDay - now.TimeOfDay).TotalMinutes) < 1) // 1 dakika tolerans
                .ToList();

            foreach (var reminder in dueReminders)
            {
                // Bildirimi göster
                notifyIcon.ShowBalloonTip(10000, "Hatırlatıcı: " + reminder.Type,
                    $"{reminder.Summary}\n{reminder.Description}",
                    ToolTipIcon.Info);
            }
        }

        private void NotifyIcon_ShowReminders(object sender, EventArgs e)
        {
            // Hatırlatıcılar formunu aç
            foreach (Form form in MdiChildren)
            {
                if (form is RemindersForm)
                {
                    form.Activate();
                    return;
                }
            }

            RemindersForm remindersForm = new RemindersForm(_currentUser);
            remindersForm.MdiParent = this;
            remindersForm.Show();
        }

        private void NotifyIcon_Exit(object sender, EventArgs e)
        {
            // Uygulamadan çık
            Application.Exit();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Observer'ı kaldır
            ReminderSubject.Instance.RemoveObserver(this);

            // Timer'ı durdur
            if (_reminderCheckTimer != null)
            {
                _reminderCheckTimer.Stop();
                _reminderCheckTimer.Dispose();
            }

            // NotifyIcon'ı temizle
            if (notifyIcon != null)
            {
                notifyIcon.Visible = false;
                notifyIcon.Dispose();
            }

            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (!ValidationHelper.ConfirmExit())
                {
                    e.Cancel = true;
                    return;
                }
            }

            base.OnFormClosing(e);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Durum çubuğunu kullanıcı bilgisi ile güncelle
            lblUserInfo.Text = $"Giriş yapan: {_currentUser.FirstName} {_currentUser.LastName} ({_currentUser.UserType})";
            // Admin menüsünü kullanıcı tipine göre göster/gizle
            yönetimToolStripMenuItem.Visible = (_currentUser.UserType == UserType.Admin);
        }

        private void telefonRehberiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form form in MdiChildren)
            {
                if (form is PhoneBookForm)
                {
                    form.Activate();
                    return;
                }
            }
            PhoneBookForm phoneBookForm = new PhoneBookForm(_currentUser);
            phoneBookForm.MdiParent = this;
            phoneBookForm.Show();
        }

        private void çıkışToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ValidationHelper.ConfirmExit())
            {
                Application.Exit();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (!ValidationHelper.ConfirmExit())
                {
                    e.Cancel = true;
                }
            }
        }

        private void notlarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form form in MdiChildren)
            {
                if (form is NotesForm)
                {
                    form.Activate();
                    return;
                }
            }

            NotesForm notesForm = new NotesForm(_currentUser);
            notesForm.MdiParent = this;
            notesForm.Show();
        }

        private void hatırlatıcılarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form form in MdiChildren)
            {
                if (form is RemindersForm)
                {
                    form.Activate();
                    return;
                }
            }

            RemindersForm remindersForm = new RemindersForm(_currentUser);
            remindersForm.MdiParent = this;
            remindersForm.Show();
        }

        private void maaşHesaplayıcıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form form in MdiChildren)
            {
                if (form is SalaryCalculatorForm)
                {
                    form.Activate();
                    return;
                }
            }

            SalaryCalculatorForm salaryCalculatorForm = new SalaryCalculatorForm(_currentUser);
            salaryCalculatorForm.MdiParent = this;
            salaryCalculatorForm.Show();
        }

        private void profilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var profileForm = new ProfileForm(_currentUser, _userService))
            {
                if (profileForm.ShowDialog() == DialogResult.OK)
                {
                    this.Text = $"Kişisel Organizör - {_currentUser.FirstName} {_currentUser.LastName}";
                }
            }
        }

        private void userManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentUser.UserType != UserType.Admin)
            {
                MessageBox.Show("Bu bölüme erişim yetkiniz yok.", "Yetki Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var adminForm = new AdminPanelForm(_currentUser, _userService))
            {
                adminForm.ShowDialog();
            }
        }
    }
}