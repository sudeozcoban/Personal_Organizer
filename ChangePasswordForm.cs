using System;
using System.Windows.Forms;

namespace PersonalOrganizer
{
    public partial class ChangePasswordForm : Form
    {
        private User _currentUser;
        private UserService _userService;

        // Boş yapıcı metot
        public ChangePasswordForm()
        {
            InitializeComponent();
        }

        // Parametreli yapıcı metot
        public ChangePasswordForm(User currentUser, UserService userService)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _userService = userService;
        }

        // Değiştir butonuna tıklandığında
        private void btnChange_Click(object sender, EventArgs e)
        {
            // Mevcut şifreyi doğrula
            if (txtCurrentPassword.Text != _currentUser.Password)
            {
                MessageBox.Show("Mevcut şifreniz doğru değil.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Yeni şifrelerin eşleştiğini kontrol et
            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Yeni şifre ve onay şifresi eşleşmiyor.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Şifre uzunluğunu kontrol et
            if (txtNewPassword.Text.Length < 6)
            {
                MessageBox.Show("Şifre en az 6 karakter olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Şifreyi değiştir
                bool success = _userService.ChangePassword(_currentUser.Id, txtCurrentPassword.Text, txtNewPassword.Text);

                if (success)
                {
                    MessageBox.Show("Şifreniz başarıyla değiştirildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Şifre değiştirilemedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // İptal butonuna tıklandığında
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ChangePasswordForm_Load(object sender, EventArgs e)
        {

        }
    }
}