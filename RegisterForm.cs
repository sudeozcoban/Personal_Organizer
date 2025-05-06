using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PersonalOrganizer
{
    public partial class RegisterForm : Form
    {
        private UserService _userService;

        public RegisterForm()
        {
            InitializeComponent();
            _userService = new UserService();

            // Telefon numarası için event handler ekle
            txtPhone.TextChanged += TxtPhone_TextChanged;

            // Telefon numarasına başlangıç değeri olarak +90 ekle
            txtPhone.Text = "+90";
        }

        private void TxtPhone_TextChanged(object sender, EventArgs e)
        {
            // Telefon numarasının her zaman +90 ile başlamasını sağla
            if (!txtPhone.Text.StartsWith("+90"))
            {
                txtPhone.Text = "+90";
                txtPhone.SelectionStart = txtPhone.Text.Length; // İmleci sona taşı
                return;
            }

            // +90 dışındaki kısmı al
            string number = txtPhone.Text.Substring(3);

            // Sadece rakam içerip içermediğini kontrol et
            string digitsOnly = new string(number.Where(char.IsDigit).ToArray());

            // Eğer rakam olmayan karakterler girilmişse, bunları temizle
            if (digitsOnly.Length != number.Length)
            {
                txtPhone.Text = "+90" + digitsOnly;
                txtPhone.SelectionStart = txtPhone.Text.Length; // İmleci sona taşı
            }

            // Rakam sayısı 10'dan fazlaysa, fazlalıkları kes
            if (digitsOnly.Length > 10)
            {
                txtPhone.Text = "+90" + digitsOnly.Substring(0, 10);
                txtPhone.SelectionStart = txtPhone.Text.Length; // İmleci sona taşı
            }
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
                return;
            try
            {
                // Telefon numarasını kaydetmeden önce format kontrolü yap
                string phoneNumber = txtPhone.Text.Trim();

                var user = new User
                {
                    Username = txtUsername.Text.Trim(),
                    Password = txtPassword.Text,
                    FirstName = txtFirstName.Text.Trim(),
                    LastName = txtLastName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Phone = phoneNumber,
                    Address = txtAddress.Text.Trim(),
                    UserType = UserType.Normal
                };
                _userService.CreateUser(user);
                MessageBox.Show("Kayıt işlemi başarılı. Artık giriş yapabilirsiniz.", "Başarılı",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kayıt işlemi başarısız: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateForm()
        {
            // Kullanıcı adı kontrolü (zorunlu)
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Kullanıcı adı zorunludur.", "Doğrulama Hatası",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return false;
            }
            // Şifre kontrolü (zorunlu, min uzunluk)
            if (string.IsNullOrWhiteSpace(txtPassword.Text) || txtPassword.Text.Length < 6)
            {
                MessageBox.Show("Şifre en az 6 karakter uzunluğunda olmalıdır.", "Doğrulama Hatası",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }
            // Şifre onayı kontrolü
            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Şifreler eşleşmiyor.", "Doğrulama Hatası",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmPassword.Focus();
                return false;
            }
            // Ad kontrolü (zorunlu)
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("Ad alanı zorunludur.", "Doğrulama Hatası",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFirstName.Focus();
                return false;
            }
            // Soyad kontrolü (zorunlu)
            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Soyad alanı zorunludur.", "Doğrulama Hatası",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLastName.Focus();
                return false;
            }
            // E-posta kontrolü (zorunlu, format)
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Geçerli bir e-posta adresi giriniz.", "Doğrulama Hatası",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            // Telefon kontrolü (10 rakam olmalı)
            string phoneDigits = txtPhone.Text.Substring(3); // +90 kısmını çıkar
            if (phoneDigits.Length != 10)
            {
                MessageBox.Show("Telefon numarası 10 rakamdan oluşmalıdır (Örnek: +90xxxxxxxxxx).", "Doğrulama Hatası",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}