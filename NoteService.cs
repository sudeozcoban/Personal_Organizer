using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace PersonalOrganizer
{
    // Basit bir not sınıfı oluşturuyoruz
    public class NoteItem
    {
        public int Id { get; set; }
        public string UserId { get; set; } // User.Id string olarak saklanacak
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class NoteCsvService
    {
        private List<NoteItem> _notes;
        private string _filePath;
        private int _nextId = 1;

        public NoteCsvService()
        {
            try
            {
                // Dosya adını atayalım
                string fileName = "notes.csv";

                // Dosya yolunu tam olarak belirleyelim
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                _filePath = Path.Combine(appDirectory, fileName);

                // Dosya yolunu konsola yazdıralım
                Debug.WriteLine($"Notlar dosya yolu: {_filePath}");

                // Eğer dosya yoksa başlık satırı ile oluştur
                if (!File.Exists(_filePath))
                {
                    Debug.WriteLine("Not dosyası bulunamadı, yeni oluşturuluyor.");
                    File.WriteAllText(_filePath, "Id;UserId;Content;CreatedAt;UpdatedAt" + Environment.NewLine);
                }

                // Notları yükle
                _notes = LoadNotesFromFile();

                // Yükleme detayları
                Debug.WriteLine($"{_notes.Count} not yüklendi.");

                // Mevcut ID'lerden sonraki ID'yi belirle
                if (_notes.Count > 0)
                {
                    _nextId = _notes.Max(n => n.Id) + 1;
                    Debug.WriteLine($"Sonraki ID: {_nextId}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"NoteCsvService constructor hatası: {ex.Message}");
                _notes = new List<NoteItem>();
            }
        }

        private List<NoteItem> LoadNotesFromFile()
        {
            List<NoteItem> notes = new List<NoteItem>();

            try
            {
                if (!File.Exists(_filePath))
                {
                    Debug.WriteLine($"Dosya bulunamadı: {_filePath}");
                    return notes;
                }

                string[] lines = File.ReadAllLines(_filePath);

                // İlk satır başlıklar, atlayalım
                for (int i = 1; i < lines.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(lines[i]))
                        continue;

                    string[] parts = lines[i].Split(';');
                    if (parts.Length >= 5)
                    {
                        try
                        {
                            NoteItem note = new NoteItem
                            {
                                Id = int.Parse(parts[0]),
                                UserId = parts[1], // UserId'yi string olarak saklıyoruz
                                Content = parts[2],
                                CreatedAt = DateTime.Parse(parts[3]),
                                UpdatedAt = DateTime.Parse(parts[4])
                            };

                            notes.Add(note);
                            Debug.WriteLine($"Not yüklendi: ID={note.Id}, İçerik={note.Content}");
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Satır işlenirken hata: {lines[i]}, Hata: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Notlar yüklenirken hata: {ex.Message}");
            }

            return notes;
        }

        private void SaveNotesToFile()
        {
            try
            {
                List<string> lines = new List<string>();
                lines.Add("Id;UserId;Content;CreatedAt;UpdatedAt");

                foreach (var note in _notes)
                {
                    // CSVHelper kullanmıyoruz, bu yüzden noktalı virgüller ve satır sonları kontrol edilmeli
                    string content = note.Content?.Replace(";", ",") ?? string.Empty;
                    string line = $"{note.Id};{note.UserId};{content};{note.CreatedAt};{note.UpdatedAt}";
                    lines.Add(line);
                }

                File.WriteAllLines(_filePath, lines);
                Debug.WriteLine($"{_notes.Count} not dosyaya kaydedildi.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Notlar kaydedilirken hata: {ex.Message}");
                throw;
            }
        }

        public List<NoteItem> GetAllNotes()
        {
            return _notes;
        }

        public List<NoteItem> GetNotesByUserId(string userId)
        {
            Debug.WriteLine($"GetNotesByUserId çağrıldı. UserId: {userId}");
            var userNotes = _notes.Where(n => n.UserId == userId).ToList();
            Debug.WriteLine($"{userId} kullanıcısı için {userNotes.Count} not bulundu.");
            return userNotes;
        }
        public void FixNotesUserIdIfMissing(string userId)
        {
            bool anyFixed = false;

            foreach (var note in _notes)
            {
                if (string.IsNullOrWhiteSpace(note.UserId))
                {
                    note.UserId = userId;
                    anyFixed = true;
                }
            }

            if (anyFixed)
            {
                SaveNotesToFile();
                Debug.WriteLine("Boş UserId'ler güncellendi.");
            }
        }


        public NoteItem GetNoteById(int id)
        {
            return _notes.FirstOrDefault(n => n.Id == id);
        }

        public void CreateNote(NoteItem note)
        {
            note.Id = _nextId++;
            note.CreatedAt = DateTime.Now;
            note.UpdatedAt = DateTime.Now;
            _notes.Add(note);
            SaveNotesToFile();

            Debug.WriteLine($"Yeni not eklendi: ID={note.Id}, İçerik={note.Content}");
        }

        public void UpdateNote(NoteItem note)
        {
            var existingNote = _notes.FirstOrDefault(n => n.Id == note.Id);
            if (existingNote == null)
            {
                throw new Exception("Not bulunamadı");
            }

            int index = _notes.FindIndex(n => n.Id == note.Id);
            note.UpdatedAt = DateTime.Now;
            _notes[index] = note;

            SaveNotesToFile();
            Debug.WriteLine($"Not güncellendi: ID={note.Id}, İçerik={note.Content}");
        }

        public void DeleteNote(int id)
        {
            var note = _notes.FirstOrDefault(n => n.Id == id);
            if (note == null)
            {
                throw new Exception("Not bulunamadı");
            }

            _notes.Remove(note);
            SaveNotesToFile();
            Debug.WriteLine($"Not silindi: ID={id}");
        }
    }
}