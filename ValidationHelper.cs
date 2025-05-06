using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PersonalOrganizer
{
    public static class ValidationHelper
    {
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            // Basit bir regex e-posta doğrulaması
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            // Şifre en az 6 karakter olmalı
            return password.Length >= 6;
        }

        public static bool ConfirmExit()
        {
            return MessageBox.Show("Gerçekten çıkmak istiyor musunuz?",
                "Çıkışı Onayla", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }
    }
}