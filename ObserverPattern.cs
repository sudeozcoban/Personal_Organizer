using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PersonalOrganizer
{
    // Observer (Gözlemci) arayüzü
    public interface IObserver
    {
        void Update(Reminder reminder);
    }

    // Subject (Konu) arayüzü
    public interface ISubject
    {
        void RegisterObserver(IObserver observer);
        void RemoveObserver(IObserver observer);
        void NotifyObservers(Reminder reminder);
    }

    // Reminder Manager sınıfı (Singleton tasarım deseni)
    public class ReminderManager : ISubject
    {
        private static ReminderManager _instance;
        private List<IObserver> _observers;
        private Guid _currentUserId;

        private ReminderManager()
        {
            _observers = new List<IObserver>();
        }

        public static ReminderManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ReminderManager();
                }
                return _instance;
            }
        }

        public void SetCurrentUser(Guid userId)
        {
            _currentUserId = userId;
        }

        public void RegisterObserver(IObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void RemoveObserver(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObservers(Reminder reminder)
        {
            if (reminder != null && reminder.UserId == _currentUserId)
            {
                foreach (var observer in _observers)
                {
                    observer.Update(reminder);
                }
            }
        }

        // Tüm hatırlatıcıları kontrol et
        public void CheckReminders(List<Reminder> reminders)
        {
            foreach (var reminder in reminders)
            {
                if (reminder.IsDue())
                {
                    NotifyObservers(reminder);
                }
            }
        }
    }

    // Form titretme ve bildirim sınıfı
    public static class WindowShaker
    {
        public static void ShakeWindow(Form form, string reminderTitle)
        {
            // Başlığı geçici olarak değiştir
            string originalTitle = form.Text;
            form.Text = $"HATIRLATICI: {reminderTitle}";

            // Başlığı sıfırlamak için zamanlayıcı oluştur
            var timer = new Timer();
            timer.Interval = 5000; // 5 saniye
            timer.Tick += (s, e) =>
            {
                form.Text = originalTitle;
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();

            // Hatırlatıcı hakkında mesaj kutusu göster
            MessageBox.Show(
                $"Hatırlatıcı: {reminderTitle}",
                "Hatırlatıcı Uyarısı",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}