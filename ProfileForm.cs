using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PersonalOrganizer
{
    public partial class ProfileForm : Form
    {
        private User _currentUser;
        private UserService _userService;
        private bool _formDataChanged = false;

        // Geri alma / yineleme için değişkenler
        private Stack<KeyValuePair<TextBox, string>> _undoStack = new Stack<KeyValuePair<TextBox, string>>();
        private Stack<KeyValuePair<TextBox, string>> _redoStack = new Stack<KeyValuePair<TextBox, string>>();
        private bool _isUndoRedo = false;
        private TextBox _lastFocusedTextBox = null;
        private string _lastText = string.Empty;

        // Boş yapıcı metot (Designer için)
        public ProfileForm()
        {
            InitializeComponent();
            this.KeyPreview = true;
            KeyPreview = true;
            this.KeyDown += ProfileForm_KeyDown;
        }

        // Parametreli yapıcı metot
        public ProfileForm(User currentUser, UserService userService)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _userService = userService;

            // Form kapanışını izle
            this.FormClosing += ProfileForm_FormClosing;

            // TextBox olaylarını izle
            InitializeTextControls();

            // CTRL-Z/Y için tuş olaylarını yakala
            this.KeyPreview = true;
            this.KeyDown += ProfileForm_KeyDown;
            this.Load += ProfileForm_Load;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // Button3'ün olayını oluştur
            button3.Click += RemovePhoto_Click;
        }

        private void InitializeTextControls()
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is TextBox)
                {
                    TextBox txt = (TextBox)ctrl;
                    // Değişiklik ve odak olaylarını bağla
                    txt.TextChanged += TextBox_TextChanged;
                    txt.Enter += TextBox_Enter;
                }
            }
        }

        private void TextBox_Enter(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;

            // Yeni bir metin kutusuna geçiş
            if (_lastFocusedTextBox != txt)
            {
                _lastFocusedTextBox = txt;
                _lastText = txt.Text;
            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            _formDataChanged = true;
            // Eğer undo/redo işleminden geliyorsa yığınları değiştirme
            if (_isUndoRedo)
                return;
            // Değişikliği kaydet
            if (_lastFocusedTextBox == txt)
            {
                if (_lastText != txt.Text)
                {
                    _undoStack.Push(new KeyValuePair<TextBox, string>(txt, _lastText));
                    _lastText = txt.Text;

                    // Yeni bir değişiklik yapıldığında redo stack'i temizle
                    _redoStack.Clear();
                }
            }
        }

        // Form yüklendiğinde çağrılır
        private void ProfileForm_Load(object sender, EventArgs e)
        {
            // Kullanıcı bilgilerini form kontrollerine yükle
            txtUsername.Text = _currentUser.Username;
            txtFirstName.Text = _currentUser.FirstName;
            txtLastName.Text = _currentUser.LastName;
            txtEmail.Text = _currentUser.Email;
            txtPhone.Text = _currentUser.Phone;
            txtAddress.Text = _currentUser.Address;

            // Kullanıcı tipini göster
            lblUserType.Text = _currentUser.UserType.ToString();

            // Admin olmayanların kullanıcı adı ve email'i değiştirilemesin
            if (_currentUser.UserType != UserType.Admin)
            {
                txtUsername.ReadOnly = true;
                txtEmail.ReadOnly = true;
            }

            // Kullanıcı fotoğrafını yükle (varsa)
            if (!string.IsNullOrEmpty(_currentUser.Photo))
            {
                try
                {
                    LoadImageFromBase64(_currentUser.Photo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fotoğraf yüklenirken hata oluştu: {ex.Message}", "Hata",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    picUserPhoto.Image = null;
                }
            }

            // Form yüklendikten sonra değişiklikleri sıfırla
            _formDataChanged = false;
        }

        // Resmi Base64'ten yükleyen metot
        private void LoadImageFromBase64(string base64String)
        {
            try
            {
                // Mevcut resmi temizle
                if (picUserPhoto.Image != null)
                {
                    picUserPhoto.Image.Dispose();
                    picUserPhoto.Image = null;
                }

                if (!string.IsNullOrEmpty(base64String))
                {
                    // Base64'ten resmi yükle
                    byte[] imageBytes = Convert.FromBase64String(base64String);
                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        picUserPhoto.Image = new Bitmap(ms);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fotoğraf yüklenirken hata oluştu: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Form kapanırken çağrılır
        private void ProfileForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Değişiklik yapıldıysa ve kaydedilmediyse uyarı göster
            if (_formDataChanged && this.DialogResult != DialogResult.OK)
            {
                DialogResult result = MessageBox.Show(
                    "Yaptığınız değişiklikler kaydedilmeyecek. Çıkmak istediğinize emin misiniz?",
                    "Çıkış Onayı",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        // Tuş basımlarını yakalar (CTRL+Z/Y için)
        private void ProfileForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Z)
            {
                button1.PerformClick(); // Geri al butonunu tetikle
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.Y)
            {
                button2.PerformClick(); // İleri al butonunu tetikle
                e.Handled = true;
            }
        }

        // Geri alma işlemini gerçekleştiren metot - Designer'da tanımlanan metot
        private void button1_Click(object sender, EventArgs e)
        {
            if (_undoStack.Count > 0)
            {
                try
                {
                    _isUndoRedo = true;
                    // Geri alınacak işlemi al
                    KeyValuePair<TextBox, string> undoItem = _undoStack.Pop();
                    TextBox txt = undoItem.Key;
                    string previousText = undoItem.Value;
                    // Şu anki durumu redo stack'e ekle
                    _redoStack.Push(new KeyValuePair<TextBox, string>(txt, txt.Text));
                    // Geri alma işlemini uygula
                    txt.Text = previousText;
                    _lastText = previousText;
                    // Odağı ayarla
                    _lastFocusedTextBox = txt;
                    txt.Focus();
                    txt.SelectionStart = txt.Text.Length;
                }
                finally
                {
                    _isUndoRedo = false;
                }
            }
        }

        // İleri alma işlemini gerçekleştiren metot - Designer'da tanımlanan metot
        private void button2_Click(object sender, EventArgs e)
        {
            if (_redoStack.Count > 0)
            {
                try
                {
                    _isUndoRedo = true;
                    // İleri alınacak işlemi al
                    KeyValuePair<TextBox, string> redoItem = _redoStack.Pop();
                    TextBox txt = redoItem.Key;
                    string nextText = redoItem.Value;
                    // Şu anki durumu undo stack'e ekle
                    _undoStack.Push(new KeyValuePair<TextBox, string>(txt, txt.Text));

                    // İleri alma işlemini uygula
                    txt.Text = nextText;
                    _lastText = nextText;
                    // Odağı ayarla
                    _lastFocusedTextBox = txt;
                    txt.Focus();
                    txt.SelectionStart = txt.Text.Length;
                }
                finally
                {
                    _isUndoRedo = false;
                }
            }
        }

        // Mevcut fotoğrafı silen metot
        private void RemovePhoto_Click(object sender, EventArgs e)
        {
            if (picUserPhoto.Image != null)
            {
                // Fotoğrafı kaldır
                picUserPhoto.Image.Dispose();
                picUserPhoto.Image = null;
                _formDataChanged = true;

                MessageBox.Show("Fotoğraf kaldırıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Kaldırılacak fotoğraf yok.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Kaydet butonuna tıklandığında
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Zorunlu alanları kontrol et
            if (string.IsNullOrEmpty(txtFirstName.Text) || string.IsNullOrEmpty(txtLastName.Text))
            {
                MessageBox.Show("Ad ve soyad alanları boş bırakılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Kullanıcı adı değiştiyse ve varsa hata ver
                if (txtUsername.Text != _currentUser.Username && _userService.UserExists(txtUsername.Text))
                {
                    MessageBox.Show("Bu kullanıcı adı zaten kullanılıyor.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Email değiştiyse ve varsa hata ver
                if (txtEmail.Text != _currentUser.Email && _userService.EmailExists(txtEmail.Text))
                {
                    MessageBox.Show("Bu e-posta adresi zaten kullanılıyor.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Kullanıcı bilgilerini güncelle
                _currentUser.Username = txtUsername.Text;
                _currentUser.FirstName = txtFirstName.Text;
                _currentUser.LastName = txtLastName.Text;
                _currentUser.Email = txtEmail.Text;
                _currentUser.Phone = txtPhone.Text;
                _currentUser.Address = txtAddress.Text;
                _currentUser.UpdatedAt = DateTime.Now;

                // Fotoğrafı güncelle (eğer varsa)
                if (picUserPhoto.Image != null)
                {
                    try
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            // Resmi daha verimli bir formatta kaydet
                            picUserPhoto.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            byte[] imageBytes = ms.ToArray();
                            _currentUser.Photo = Convert.ToBase64String(imageBytes);
                        }
                    }
                    catch (Exception imgEx)
                    {
                        // Sadece fotoğraf kaydedilirken hata oluşursa, kullanıcıyı bilgilendir
                        // ama diğer bilgilerin kaydedilmesine devam et
                        MessageBox.Show($"Fotoğraf kaydedilirken hata oluştu: {imgEx.Message}. Diğer bilgiler kaydedilecek.",
                            "Fotoğraf Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        _currentUser.Photo = null;
                    }
                }
                else
                {
                    // Kullanıcı fotoğrafı kaldırdıysa
                    _currentUser.Photo = null;
                }

                // Veritabanında güncelle...
                _userService.UpdateUser(_currentUser);
                MessageBox.Show("Profil bilgileriniz başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _formDataChanged = false;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Şifre değiştir butonuna tıklandığında
        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            // ChangePasswordForm'u tanımlamanız gerekiyor
            using (var passwordForm = new ChangePasswordForm(_currentUser, _userService))
            {
                passwordForm.ShowDialog();
            }
        }

        // İptal butonuna tıklandığında
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        // Fotoğraf seçme butonuna tıklandığında
        private void btnSelectPhoto_Click(object sender, EventArgs e)
        {
            // Eğer mevcut fotoğraf varsa önce silmesini iste
            if (picUserPhoto.Image != null)
            {
                MessageBox.Show("Önce mevcut fotoğrafı siliniz.",
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Dosya filtresini ayarla
            openFileDialog1.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Yeni resmi dosyadan yükle
                    using (Bitmap loadedImage = new Bitmap(openFileDialog1.FileName))
                    {
                        // PictureBox'a yeni bir bitmap kopyası olarak ayarla
                        picUserPhoto.Image = new Bitmap(loadedImage);
                        _formDataChanged = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Resim yüklenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}