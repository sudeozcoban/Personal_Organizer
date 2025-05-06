using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace PersonalOrganizer
{
    public partial class NotesForm : Form
    {
        private User _currentUser;
        private NoteCsvService _noteService;
        private NoteItem _currentNote;
        private List<NoteItem> _noteList;

        // Designer için default constructor
        public NotesForm()
        {
            InitializeComponent();
        }

        // User parametreli constructor
        public NotesForm(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _noteService = new NoteCsvService();
            _noteList = new List<NoteItem>();

            // Buton metinleri
            

            Debug.WriteLine($"NotesForm oluşturuldu. Kullanıcı ID: {_currentUser.Id}");
        }

        private void NotesForm_Load(object sender, EventArgs e)
        {
            try
            {
                Debug.WriteLine("NotesForm_Load başladı");

                // Form başlığını kullanıcıya göre ayarla
                this.Text = $"Notlar - {_currentUser.FirstName} {_currentUser.LastName}";

                // CheckedListBox özelliklerini ayarla
                checkedListBox1.CheckOnClick = true; // Tıklandığında otomatik işaretle
                checkedListBox1.SelectionMode = SelectionMode.One; // Tek öğe seçilebilir (içerik gösterme için)

                // Not yükleme işlemini başta yapmıyoruz, kullanıcı butona tıklayarak yapacak
                // LoadNotes();

                // Alternatif olarak form açılırken yüklemek isterseniz:
                LoadNotes();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"NotesForm_Load hata: {ex.Message}");
                MessageBox.Show($"Form yüklenirken bir hata oluştu: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadNotes()
        {
            Debug.WriteLine("LoadNotes başladı");

            try
            {
                // CheckedListBox'ı ve listeyi temizle
                checkedListBox1.Items.Clear();
                _noteList.Clear();

                // Kullanıcıya ait notları al - UserId'yi string olarak aktarıyoruz
                _noteList = _noteService.GetNotesByUserId(_currentUser.Id.ToString());
                Debug.WriteLine($"LoadNotes: {_noteList.Count} not bulundu");

                // CheckedListBox'a ekle
                foreach (var note in _noteList)
                {
                    string displayText = note.Content;
                    checkedListBox1.Items.Add(displayText, false); // false: başlangıçta işaretlenmemiş
                    Debug.WriteLine($"CheckedListBox'a eklendi: {displayText}");
                }

                Debug.WriteLine($"CheckedListBox eleman sayısı: {checkedListBox1.Items.Count}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"HATA - LoadNotes: {ex.Message}");
                MessageBox.Show($"Notlar yüklenirken hata oluştu: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Debug.WriteLine("LoadNotes tamamlandı");
        }

        private void button1_Click(object sender, EventArgs e) // Ekle
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Not içeriği boş olamaz.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Yeni not oluştur - UserId'yi string olarak sakla
                NoteItem newNote = new NoteItem
                {
                    UserId = _currentUser.Id.ToString(),
                    Content = textBox1.Text.Trim()
                };

                // Servise kaydet
                _noteService.CreateNote(newNote);

                // CheckedListBox'ı güncelle
                LoadNotes();

                // Formu temizle
                textBox1.Clear();

                MessageBox.Show("Not başarıyla eklendi.", "Bilgi",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Not eklenirken hata oluştu: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e) // Güncelle
        {
            if (checkedListBox1.SelectedIndex == -1 || string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Lütfen güncellenecek bir not seçin ve içerik girin.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Seçili indeksi al
                int selectedIndex = checkedListBox1.SelectedIndex;

                // Gerçek Note nesnesini al
                if (selectedIndex >= 0 && selectedIndex < _noteList.Count)
                {
                    NoteItem note = _noteList[selectedIndex];

                    // İçeriği güncelle
                    note.Content = textBox1.Text.Trim();

                    // Servise kaydet
                    _noteService.UpdateNote(note);

                    // CheckedListBox'ı güncelle
                    LoadNotes();

                    // Formu temizle
                    textBox1.Clear();

                    MessageBox.Show("Not başarıyla güncellendi.", "Bilgi",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Seçili not bulunamadı.", "Hata",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Not güncellenirken hata oluştu: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e) // Seçilenleri Sil
        {
            try
            {
                // İşaretli öğeleri bul
                List<int> checkedIndices = new List<int>();
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.GetItemChecked(i))
                    {
                        checkedIndices.Add(i);
                    }
                }

                // Seçili öğe yoksa uyarı ver
                if (checkedIndices.Count == 0)
                {
                    MessageBox.Show("Lütfen silinecek notları işaretleyin.", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Silmeden önce onay al
                string message = checkedIndices.Count == 1
                    ? "Seçilen notu silmek istediğinize emin misiniz?"
                    : $"Seçilen {checkedIndices.Count} notu silmek istediğinize emin misiniz?";

                if (MessageBox.Show(message, "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // En yüksek indeksten başlayarak sil (indeksler değişmesin diye)
                    checkedIndices.Sort();
                    checkedIndices.Reverse();

                    // Seçili notları sil
                    foreach (int index in checkedIndices)
                    {
                        if (index >= 0 && index < _noteList.Count)
                        {
                            NoteItem note = _noteList[index];
                            _noteService.DeleteNote(note.Id);
                        }
                    }

                    // CheckedListBox'ı güncelle
                    LoadNotes();

                    // Formu temizle
                    textBox1.Clear();

                    string resultMessage = checkedIndices.Count == 1
                        ? "Not başarıyla silindi."
                        : $"{checkedIndices.Count} not başarıyla silindi.";

                    MessageBox.Show(resultMessage, "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Not silinirken hata oluştu: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Notları Göster butonu için Click olayı
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                LoadNotes();
                MessageBox.Show("Notlar yenilendi.", "Bilgi",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Notlar yüklenirken hata oluştu: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Seçim yapılmış mı kontrol et
                if (checkedListBox1.SelectedIndex == -1) return;

                // Seçili indeksi al
                int selectedIndex = checkedListBox1.SelectedIndex;

                // Gerçek Note nesnesini al
                if (selectedIndex >= 0 && selectedIndex < _noteList.Count)
                {
                    NoteItem note = _noteList[selectedIndex];
                    _currentNote = note; // Seçilen notu ata

                    // TextBox'a içeriği yükle
                    textBox1.Text = note.Content;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Not bilgileri yüklenirken hata oluştu: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}