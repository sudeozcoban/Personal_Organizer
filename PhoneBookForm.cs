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
    public partial class PhoneBookForm : Form
    {
        private User _currentUser;
        private ContactService _contactService;
        private Contact _currentContact;
        private bool _isEditMode = false;
        private bool _isPhoneBeingProgrammaticallyUpdated = false;

        public PhoneBookForm(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _contactService = new ContactService();

            // Telefon TextBox'ı için event handler ekle
            txtPhone.TextChanged += TxtPhone_TextChanged;

            // Başlangıçta ayarları yap
            DisableContactForm();
            LoadContacts();
        }

        private void TxtPhone_TextChanged(object sender, EventArgs e)
        {
            // Eğer program tarafından değiştiriliyorsa işlem yapma
            if (_isPhoneBeingProgrammaticallyUpdated)
                return;

            _isPhoneBeingProgrammaticallyUpdated = true;

            // Telefon numarasının her zaman +90 ile başlamasını sağla
            if (!txtPhone.Text.StartsWith("+90"))
            {
                txtPhone.Text = "+90";
                txtPhone.SelectionStart = txtPhone.Text.Length; // İmleci sona taşı
                _isPhoneBeingProgrammaticallyUpdated = false;
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

            _isPhoneBeingProgrammaticallyUpdated = false;
        }

        private void LoadContacts()
        {
            lstContacts.Items.Clear();
            List<Contact> contacts = _contactService.GetContactsByUserId(_currentUser.Id);
            foreach (var contact in contacts)
            {
                ListViewItem item = new ListViewItem(contact.FirstName + " " + contact.LastName);
                item.SubItems.Add(contact.Phone);
                item.SubItems.Add(contact.Email);
                item.Tag = contact;
                lstContacts.Items.Add(item);
            }
            // Seçime göre butonları etkinleştir/devre dışı bırak
            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            bool hasSelection = lstContacts.SelectedItems.Count > 0;
            btnEdit.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
        }

        private void EnableContactForm()
        {
            groupContact.Enabled = true;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
        }

        private void DisableContactForm()
        {
            groupContact.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = true;
            ClearContactForm();
        }

        private void ClearContactForm()
        {
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;

            // Telefon alanını temizle ve +90 ekle
            _isPhoneBeingProgrammaticallyUpdated = true;
            txtPhone.Text = "+90";
            _isPhoneBeingProgrammaticallyUpdated = false;

            txtAddress.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtDescription.Text = string.Empty;
            _currentContact = null;
        }

        private void DisplayContact(Contact contact)
        {
            _currentContact = contact;

            if (contact != null)
            {
                txtFirstName.Text = contact.FirstName;
                txtLastName.Text = contact.LastName;

                // Telefon numarasını kontrol et ve gerekirse formatla
                _isPhoneBeingProgrammaticallyUpdated = true;
                string phone = contact.Phone;
                if (!string.IsNullOrEmpty(phone))
                {
                    if (!phone.StartsWith("+90"))
                    {
                        // Telefon +90 ile başlamıyorsa, ekle
                        txtPhone.Text = "+90" + phone;
                    }
                    else
                    {
                        txtPhone.Text = phone;
                    }
                }
                else
                {
                    txtPhone.Text = "+90";
                }
                _isPhoneBeingProgrammaticallyUpdated = false;

                txtAddress.Text = contact.Address;
                txtEmail.Text = contact.Email;
                txtDescription.Text = contact.Description;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateContactForm())
            {
                try
                {
                    if (_isEditMode && _currentContact != null)
                    {
                        // Mevcut kişiyi güncelle
                        _currentContact.FirstName = txtFirstName.Text.Trim();
                        _currentContact.LastName = txtLastName.Text.Trim();
                        _currentContact.Phone = txtPhone.Text.Trim();
                        _currentContact.Address = txtAddress.Text.Trim();
                        _currentContact.Email = txtEmail.Text.Trim();
                        _currentContact.Description = txtDescription.Text.Trim();
                        _currentContact.UpdatedAt = DateTime.Now;

                        _contactService.UpdateContact(_currentContact);
                        MessageBox.Show("Kişi başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Yeni kişi oluştur
                        Contact newContact = new Contact
                        {
                            UserId = _currentUser.Id,
                            FirstName = txtFirstName.Text.Trim(),
                            LastName = txtLastName.Text.Trim(),
                            Phone = txtPhone.Text.Trim(),
                            Address = txtAddress.Text.Trim(),
                            Email = txtEmail.Text.Trim(),
                            Description = txtDescription.Text.Trim()
                        };

                        _contactService.CreateContact(newContact);
                        MessageBox.Show("Yeni kişi başarıyla eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    // Listeyi ve arayüzü yenile
                    LoadContacts();
                    DisableContactForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Kişi kaydedilirken hata oluştu: {ex.Message}", "Hata",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DisableContactForm();
            if (lstContacts.SelectedItems.Count > 0)
            {
                Contact selectedContact = (Contact)lstContacts.SelectedItems[0].Tag;
                DisplayContact(selectedContact);
            }
        }

        private void lstContacts_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtonStates();

            if (lstContacts.SelectedItems.Count > 0)
            {
                Contact selectedContact = (Contact)lstContacts.SelectedItems[0].Tag;
                DisplayContact(selectedContact);
            }
            else
            {
                ClearContactForm();
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            _isEditMode = false;
            ClearContactForm();
            EnableContactForm();
            txtFirstName.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lstContacts.SelectedItems.Count > 0)
            {
                _isEditMode = true;
                EnableContactForm();
                txtFirstName.Focus();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstContacts.SelectedItems.Count > 0)
            {
                Contact contactToDelete = (Contact)lstContacts.SelectedItems[0].Tag;

                DialogResult result = MessageBox.Show(
                    $"'{contactToDelete.FirstName} {contactToDelete.LastName}' kişisini silmek istediğinize emin misiniz?",
                    "Silme Onayı",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        _contactService.DeleteContact(contactToDelete.Id);
                        LoadContacts();
                        ClearContactForm();
                        MessageBox.Show("Kişi başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Kişi silinirken hata oluştu: {ex.Message}", "Hata",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private bool ValidateContactForm()
        {
            // Ad zorunlu
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("Ad alanı zorunludur.", "Doğrulama Hatası",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFirstName.Focus();
                return false;
            }

            // Telefon zorunlu ve format kontrolü
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Telefon numarası zorunludur.", "Doğrulama Hatası",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return false;
            }

            // Telefon numarası +90 ile başlamalı ve sonrasında 10 rakam olmalı
            if (!txtPhone.Text.StartsWith("+90") || txtPhone.Text.Length != 13)
            {
                MessageBox.Show("Telefon numarası +90 ile başlamalı ve 10 rakam içermelidir. Örnek: +905xxxxxxxxx",
                    "Doğrulama Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return false;
            }

            // +90 sonrası 10 rakam kontrolü
            string phoneNumber = txtPhone.Text.Substring(3);
            if (phoneNumber.Length != 10 || !phoneNumber.All(char.IsDigit))
            {
                MessageBox.Show("Telefon numarası +90 sonrası 10 rakam içermelidir. Örnek: +905xxxxxxxxx",
                    "Doğrulama Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return false;
            }

            // E-posta formatı kontrolü
            if (!string.IsNullOrWhiteSpace(txtEmail.Text) && !ValidationHelper.IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Geçerli bir e-posta adresi giriniz.", "Doğrulama Hatası",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            return true;
        }
    }
}