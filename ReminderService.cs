using System;
using System.Collections.Generic;
using System.Linq;
using System.IO; // Dosya işlemleri için
using System.Diagnostics; // Debug için
using System.Text;

namespace PersonalOrganizer
{
    public class ReminderService
    {
        private CsvHelper<Reminder> _csvHelper;
        private List<Reminder> _reminders;
        private int _nextId = 1;
        private string _filePath; // Dosya yolu için

        public ReminderService()
        {
            try
            {
                // Dosya adını atayalım
                string fileName = "reminders.csv";

                // Dosya yolunu tam olarak belirleyelim
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                _filePath = Path.Combine(appDirectory, fileName);

                // Dosya yolunu konsola yazdıralım
                Debug.WriteLine($"Hatırlatıcılar dosya yolu: {_filePath}");

                // Dosya mevcut değilse veya boşsa, yeni başlık satırı ile yeni dosya oluştur
                if (!File.Exists(_filePath) || new FileInfo(_filePath).Length == 0)
                {
                    Debug.WriteLine("CSV dosyası mevcut değil veya boş, yeni oluşturuluyor.");

                    // Sınıf özelliklerine göre başlık satırı oluştur
                    var properties = typeof(Reminder).GetProperties();
                    string headers = string.Join(";", properties.Select(p => p.Name));

                    // Yeni dosya oluştur
                    File.WriteAllText(_filePath, headers + Environment.NewLine, Encoding.UTF8);
                }

                // CsvHelper oluştur ve verileri yükle
                _csvHelper = new CsvHelper<Reminder>(fileName);
                _reminders = _csvHelper.ReadAll();

                // Yükleme detayları
                Debug.WriteLine($"{_reminders.Count} hatırlatıcı yüklendi.");

                // Her bir hatırlatıcıyı yazdıralım
                foreach (var reminder in _reminders)
                {
                    Debug.WriteLine($"- ID: {reminder.Id}, UserId: {reminder.UserId}, Özet: {reminder.Summary}");
                }

                // Mevcut ID'lerden sonraki ID'yi belirle
                if (_reminders.Count > 0)
                {
                    _nextId = _reminders.Max(r => r.Id) + 1;
                    Debug.WriteLine($"Sonraki ID: {_nextId}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ReminderService constructor hatası: {ex.Message}");
                // Başarısız olursa boş bir liste oluştur
                _reminders = new List<Reminder>();
            }
        }

        public List<Reminder> GetAllReminders()
        {
            return _reminders;
        }

        public List<Reminder> GetRemindersByUserId(Guid userId)
        {
            // UserId Guid olduğundan karşılaştırmayı string olarak yapalım
            string userIdString = userId.ToString();

            // Kullanıcı için hatırlatıcıları bul
            var userReminders = _reminders
                .Where(r => r.UserId.ToString().Equals(userIdString, StringComparison.OrdinalIgnoreCase))
                .ToList();

            Debug.WriteLine($"{userId} kullanıcısı için {userReminders.Count} hatırlatıcı bulundu.");

            // Her hatırlatıcıyı yazdır
            foreach (var reminder in userReminders)
            {
                Debug.WriteLine($"- ID: {reminder.Id}, Özet: {reminder.Summary}");
            }

            return userReminders;
        }

        public Reminder GetReminderById(int id)
        {
            return _reminders.FirstOrDefault(r => r.Id == id);
        }

        public void CreateReminder(Reminder reminder)
        {
            reminder.Id = _nextId++;
            reminder.CreatedAt = DateTime.Now;
            reminder.UpdatedAt = DateTime.Now;
            _reminders.Add(reminder);
            _csvHelper.Write(_reminders);

            Debug.WriteLine($"Yeni hatırlatıcı eklendi: ID={reminder.Id}, Özet={reminder.Summary}");
            Debug.WriteLine($"Toplam hatırlatıcı sayısı: {_reminders.Count}");
        }

        public void UpdateReminder(Reminder reminder)
        {
            var existingReminder = _reminders.FirstOrDefault(r => r.Id == reminder.Id);
            if (existingReminder == null)
            {
                throw new Exception("Hatırlatıcı bulunamadı");
            }

            int index = _reminders.FindIndex(r => r.Id == reminder.Id);
            reminder.UpdatedAt = DateTime.Now;
            _reminders[index] = reminder;

            _csvHelper.Write(_reminders);
            Debug.WriteLine($"Hatırlatıcı güncellendi: ID={reminder.Id}, Özet={reminder.Summary}");
        }

        public void DeleteReminder(int id)
        {
            var reminder = _reminders.FirstOrDefault(r => r.Id == id);
            if (reminder == null)
            {
                throw new Exception("Hatırlatıcı bulunamadı");
            }

            _reminders.Remove(reminder);
            _csvHelper.Write(_reminders);
            Debug.WriteLine($"Hatırlatıcı silindi: ID={id}");
            Debug.WriteLine($"Kalan hatırlatıcı sayısı: {_reminders.Count}");
        }
    }
}