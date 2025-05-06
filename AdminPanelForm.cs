using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PersonalOrganizer
{
    public partial class AdminPanelForm : Form
    {
        private User _currentUser;
        private UserService _userService;

        public AdminPanelForm()
        {
            InitializeComponent();
        }

        public AdminPanelForm(User currentUser, UserService userService)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _userService = userService;
        }

        private void AdminPanelForm_Load(object sender, EventArgs e)
        {
            if (_userService == null)
                _userService = new UserService();

            LoadUsers();
            SetupUserTypeComboBox();
        }

        private void LoadUsers()
        {
            lstUsers.Items.Clear();
            var allUsers = _userService.GetAllUsers();
            foreach (var u in allUsers)
            {
                if (_currentUser != null && u.Id == _currentUser.Id)
                    continue;

                var item = new ListViewItem($"{u.Username} - {u.FirstName} {u.LastName} ({u.UserType})");
                item.Tag = u;
                lstUsers.Items.Add(item);
            }
        }

        private void SetupUserTypeComboBox()
        {
            cmbUserType.Items.Clear();
            cmbUserType.Items.Add(UserType.Normal);
            cmbUserType.Items.Add(UserType.PartTime);
            cmbUserType.Items.Add(UserType.Admin);
            cmbUserType.SelectedIndex = 0;
        }

        private void lstUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool hasSelection = lstUsers.SelectedItems.Count > 0;
            btnDeleteUser.Enabled = hasSelection;
            btnEditUser.Enabled = hasSelection;
            btnResetPassword.Enabled = hasSelection;
            btnEditUser.Enabled = hasSelection;

            if (hasSelection)
            {
                var u = (User)lstUsers.SelectedItems[0].Tag;
                cmbUserType.SelectedItem = u.UserType;
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            using (var reg = new RegisterForm())
            {
                if (reg.ShowDialog() == DialogResult.OK)
                    LoadUsers();
            }
        }

        private void btnEditUser_Click(object sender, EventArgs e)
        {
            if (lstUsers.SelectedItems.Count == 0) return;
            var u = (User)lstUsers.SelectedItems[0].Tag;
            using (var prof = new ProfileForm(u, _userService))
            {
                if (prof.ShowDialog() == DialogResult.OK)
                    LoadUsers();
            }
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (lstUsers.SelectedItems.Count == 0) return;
            var u = (User)lstUsers.SelectedItems[0].Tag;
            if (MessageBox.Show($"{u.Username} silinsin mi?", "Onay",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            _userService.DeleteUser(u.Id);
            MessageBox.Show("Silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadUsers();
        }

        private void btnChangeUserType_Click(object sender, EventArgs e)
        {
            if (lstUsers.SelectedItems.Count == 0) return;
            var u = (User)lstUsers.SelectedItems[0].Tag;
            var newType = (UserType)cmbUserType.SelectedItem;
            if (u.UserType == newType) return;

            _userService.ChangeUserType(u.Id, newType);
            MessageBox.Show("Kullanıcı tipi güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadUsers();
        }

        private async void btnResetPassword_Click(object sender, EventArgs e)
        {
            if (lstUsers.SelectedItems.Count == 0)
            {
                MessageBox.Show("Lütfen bir kullanıcı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedUser = (User)lstUsers.SelectedItems[0].Tag;
            if (string.IsNullOrEmpty(selectedUser.Email))
            {
                MessageBox.Show("Seçilen kullanıcının e-posta adresi yok.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show(
                    $"{selectedUser.FirstName} {selectedUser.LastName} kullanıcısının şifresini sıfırlamak istediğinize emin misiniz?",
                    "Şifre Sıfırlama Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                != DialogResult.Yes) return;

            // Yeni şifre oluştur ve güncelle
            string newPassword = GenerateRandomPassword();
            selectedUser.Password = newPassword;
            _userService.UpdateUser(selectedUser);

            // Gönderen e-posta bilgilerini al
            using (var emailForm = new Form())
            {
                emailForm.Text = "E-posta Gönderen Bilgileri";
                emailForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                emailForm.StartPosition = FormStartPosition.CenterParent;
                emailForm.Size = new System.Drawing.Size(360, 180);
                emailForm.MaximizeBox = false;
                emailForm.MinimizeBox = false;

                var lblEmail = new Label { Text = "E-posta:", Left = 20, Top = 20, Width = 80 };
                var txtEmail = new TextBox { Left = 110, Top = 18, Width = 220 };
                var lblPass = new Label { Text = "Parola:", Left = 20, Top = 55, Width = 80 };
                var txtPass = new TextBox { Left = 110, Top = 53, Width = 220, PasswordChar = '*' };
                var btnOk = new Button { Text = "Tamam", Left = 110, Top = 95, Width = 100, DialogResult = DialogResult.OK };
                var btnCancel = new Button { Text = "İptal", Left = 230, Top = 95, Width = 100, DialogResult = DialogResult.Cancel };

                emailForm.Controls.AddRange(new Control[] { lblEmail, txtEmail, lblPass, txtPass, btnOk, btnCancel });

                if (emailForm.ShowDialog() == DialogResult.OK)
                {
                    var senderEmail = txtEmail.Text;
                    var senderPassword = txtPass.Text;

                    progressBarEmail.Visible = true;
                    try
                    {
                        string body = $@"
<html><body style='font-family:Arial,sans-serif;'>
  <p>Sayın {selectedUser.FirstName} {selectedUser.LastName},</p>
  <p>Şifreniz yönetici tarafından sıfırlandı.</p>
  <p><b>Yeni şifreniz: {newPassword}</b></p>
  <p>Lütfen giriş yaptıktan sonra şifrenizi değiştiriniz.</p>
  <p>İyi günler dileriz.</p>
</body></html>";

                        var msg = new MailMessage
                        {
                            From = new MailAddress(senderEmail, "Kişisel Organizör"),
                            Subject = "Şifre Sıfırlama",
                            Body = body,
                            IsBodyHtml = true
                        };
                        msg.To.Add(selectedUser.Email);

                        var client = new SmtpClient("smtp.gmail.com", 587)
                        {
                            EnableSsl = true,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(senderEmail, senderPassword),
                            Timeout = 10000
                        };

                        await Task.Run(() => client.Send(msg));

                        MessageBox.Show($"Şifre başarıyla gönderildi: {selectedUser.Email}", "Başarılı",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"E-posta gönderme hatası: {ex.Message}\nYeni şifre: {newPassword}",
                            "E-posta Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        progressBarEmail.Visible = false;
                    }
                }
                else
                {
                    MessageBox.Show($"Şifre sıfırlandı ancak e-posta gönderilemedi.\nYeni şifre: {newPassword}",
                        "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private string GenerateRandomPassword(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var rnd = new Random();
            return new string(Enumerable.Range(0, length)
                .Select(_ => chars[rnd.Next(chars.Length)]).ToArray());
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AdminPanelForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing &&
                MessageBox.Show("Yönetim panelinden çıkmak istediğinize emin misiniz?", "Onay",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
