using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics; // Debug için

namespace PersonalOrganizer
{
    public partial class RemindersForm : Form
    {
        private User _currentUser;
        private ReminderService _reminderService;
        private Reminder _currentReminder;
        private List<Reminder> _reminderList;

        public RemindersForm(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _reminderService = new ReminderService();
            _reminderList = new List<Reminder>();

            // Button4 metnini ayarla
            button4.Text = "Hatırlatıcıları Göster";

            Debug.WriteLine($"RemindersForm oluşturuldu. Kullanıcı ID: {_currentUser.Id}");
        }

        private void RemindersForm_Load(object sender, EventArgs e)
        {
            try
            {
                Debug.WriteLine("RemindersForm_Load başladı");

                // Form kontrollerini hazırla
                comboBox1.Items.Clear();
                comboBox1.Items.Add("Toplantı");
                comboBox1.Items.Add("Görev");
                comboBox1.SelectedIndex = 0;

                dateTimePicker2.Format = DateTimePickerFormat.Time;
                dateTimePicker2.ShowUpDown = true;

                // Hatırlatıcıları yükle
                LoadReminders();

                // Mevcut hatırlatıcıları ReminderSubject'e bildir
                ReminderSubject.Instance.SetReminders(_reminderList);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"RemindersForm_Load hata: {ex.Message}");
                MessageBox.Show($"Form yüklenirken bir hata oluştu: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadReminders()
        {
            Debug.WriteLine("LoadReminders başladı");

            try
            {
                // ListBox'ı ve listeyi temizle
                listBox1.Items.Clear();
                _reminderList.Clear();

                // Kullanıcıya ait hatırlatıcıları al (Guid ile)
                _reminderList = _reminderService.GetRemindersByUserId(_currentUser.Id);
                Debug.WriteLine($"LoadReminders: {_reminderList.Count} hatırlatıcı bulundu");

                // ListBox'a ekle - sadece görüntülenecek metinleri
                foreach (var reminder in _reminderList)
                {
                    string displayText = $"{reminder.Type} - {reminder.Date.ToShortDateString()} {reminder.Time.ToShortTimeString()} - {reminder.Summary}";
                    listBox1.Items.Add(displayText);
                    Debug.WriteLine($"ListBox'a eklendi: {displayText}");
                }

                Debug.WriteLine($"ListBox eleman sayısı: {listBox1.Items.Count}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"HATA - LoadReminders: {ex.Message}");
                MessageBox.Show($"Hatırlatıcılar yüklenirken hata oluştu: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Debug.WriteLine("LoadReminders tamamlandı");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Verileri al
            string tur = comboBox1.SelectedItem.ToString();
            DateTime tarih = dateTimePicker1.Value.Date;
            DateTime saat = dateTimePicker2.Value;
            string ozet = textBox1.Text.Trim();
            string aciklama = textBox2.Text.Trim();

            // Validasyon
            if (string.IsNullOrEmpty(ozet))
            {
                MessageBox.Show("Özet alanı boş bırakılamaz.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Yeni hatırlatıcı oluştur
                Reminder newReminder = new Reminder
                {
                    UserId = _currentUser.Id, // Guid olarak
                    Type = tur,
                    Date = tarih,
                    Time = saat,
                    Summary = ozet,
                    Description = aciklama,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                // Servise kaydet
                _reminderService.CreateReminder(newReminder);

                // Observer pattern ile ReminderSubject'e bildirme
                ReminderSubject.Instance.AddReminder(newReminder);

                // ListBox'ı güncelle
                LoadReminders();

                // Formu temizle
                textBox1.Clear();
                textBox2.Clear();

                this.Text = ozet; // Başlıkta göster
                PencereyiTitret(); // Titreşim efekti
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hatırlatıcı eklenirken hata oluştu: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Seçili index var mı kontrol et
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen güncellenecek bir hatırlatıcı seçin.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Seçili indeksi al
                int selectedIndex = listBox1.SelectedIndex;

                // Gerçek Reminder nesnesini al
                if (selectedIndex >= 0 && selectedIndex < _reminderList.Count)
                {
                    Reminder reminder = _reminderList[selectedIndex];

                    // Verileri al
                    string tur = comboBox1.SelectedItem.ToString();
                    DateTime tarih = dateTimePicker1.Value.Date;
                    DateTime saat = dateTimePicker2.Value;
                    string ozet = textBox1.Text.Trim();
                    string aciklama = textBox2.Text.Trim();

                    // Validasyon
                    if (string.IsNullOrEmpty(ozet))
                    {
                        MessageBox.Show("Özet alanı boş bırakılamaz.", "Uyarı",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Hatırlatıcıyı güncelle
                    reminder.Type = tur;
                    reminder.Date = tarih;
                    reminder.Time = saat;
                    reminder.Summary = ozet;
                    reminder.Description = aciklama;
                    reminder.UpdatedAt = DateTime.Now;

                    // Servise kaydet
                    _reminderService.UpdateReminder(reminder);

                    // Observer pattern ile ReminderSubject'e bildirme
                    ReminderSubject.Instance.UpdateReminder(reminder);

                    // ListBox'ı güncelle
                    LoadReminders();

                    MessageBox.Show("Hatırlatıcı başarıyla güncellendi.", "Bilgi",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Text = ozet;
                    PencereyiTitret();
                }
                else
                {
                    MessageBox.Show("Seçili hatırlatıcı bulunamadı.", "Hata",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hatırlatıcı güncellenirken hata oluştu: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Seçili index var mı kontrol et
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen silinecek bir hatırlatıcı seçin.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Seçili indeksi al
                int selectedIndex = listBox1.SelectedIndex;

                // Gerçek Reminder nesnesini al
                if (selectedIndex >= 0 && selectedIndex < _reminderList.Count)
                {
                    Reminder reminder = _reminderList[selectedIndex];

                    // Silmeden önce onay al
                    if (MessageBox.Show("Seçilen hatırlatıcıyı silmek istediğinize emin misiniz?", "Onay",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        // Servisten sil
                        _reminderService.DeleteReminder(reminder.Id);

                        // Observer pattern ile ReminderSubject'e bildirme
                        ReminderSubject.Instance.RemoveReminder(reminder.Id);

                        // ListBox'ı güncelle
                        LoadReminders();

                        // Alanları temizle
                        textBox1.Clear();
                        textBox2.Clear();
                        _currentReminder = null;

                        MessageBox.Show("Hatırlatıcı başarıyla silindi.", "Bilgi",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Seçili hatırlatıcı bulunamadı.", "Hata",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hatırlatıcı silinirken hata oluştu: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hatırlatıcıları Göster butonu için Click olayı
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                LoadReminders();

                // Mevcut hatırlatıcıları ReminderSubject'e bildir
                ReminderSubject.Instance.SetReminders(_reminderList);

                MessageBox.Show("Hatırlatıcılar yenilendi.", "Bilgi",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hatırlatıcılar yüklenirken hata oluştu: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Seçim yapılmış mı kontrol et
                if (listBox1.SelectedIndex == -1) return;

                // Seçili indeksi al
                int selectedIndex = listBox1.SelectedIndex;

                // Gerçek Reminder nesnesini al
                if (selectedIndex >= 0 && selectedIndex < _reminderList.Count)
                {
                    Reminder reminder = _reminderList[selectedIndex];
                    _currentReminder = reminder; // Seçilen hatırlatıcıyı ata

                    // Form kontrollerini doldur
                    comboBox1.SelectedItem = reminder.Type;
                    dateTimePicker1.Value = reminder.Date;
                    dateTimePicker2.Value = reminder.Time;
                    textBox1.Text = reminder.Summary;
                    textBox2.Text = reminder.Description ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hatırlatıcı bilgileri yüklenirken hata oluştu: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void PencereyiTitret()
        {
            var orijinalKonum = this.Location;
            Random rnd = new Random();
            for (int i = 0; i < 10; i++)
            {
                this.Location = new System.Drawing.Point(orijinalKonum.X + rnd.Next(-5, 5), orijinalKonum.Y + rnd.Next(-5, 5));
                await Task.Delay(100);
            }
            this.Location = orijinalKonum;
        }
    }
}