using System;

namespace PersonalOrganizer
{
    public class Reminder
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Hatırlatıcının zamanının gelip gelmediğini kontrol eder
        /// </summary>
        /// <returns>Hatırlatıcı zamanı geldiyse true, aksi halde false</returns>
        public bool IsDue()
        {
            // Bugünün tarihini ve şu anki saati al
            DateTime now = DateTime.Now;
            DateTime today = DateTime.Today;

            // Hatırlatıcının tam zamanını oluştur (tarih + saat)
            DateTime reminderDateTime = Date.Date.Add(Time.TimeOfDay);

            // Hatırlatıcı zamanı geçmiş veya şu anda ise true döndür
            return reminderDateTime <= now;
        }

        public override string ToString()
        {
            return $"{Type} - {Date.ToShortDateString()} {Time.ToShortTimeString()} - {Summary}";
        }
    }
}