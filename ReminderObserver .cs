using System;
using System.Collections.Generic;

namespace PersonalOrganizer
{
    // Observer pattern için arayüz
    public interface IReminderObserver
    {
        void UpdateReminders(List<Reminder> reminders);
    }

    // Subject (Gözlenen) sınıfı
    public class ReminderSubject
    {
        private static ReminderSubject _instance;
        private readonly List<IReminderObserver> _observers = new List<IReminderObserver>();
        private readonly List<Reminder> _activeReminders = new List<Reminder>();

        // Singleton pattern
        public static ReminderSubject Instance
        {
            get { return _instance ?? (_instance = new ReminderSubject()); }
        }

        private ReminderSubject() { }

        public void AddObserver(IReminderObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void RemoveObserver(IReminderObserver observer)
        {
            _observers.Remove(observer);
        }

        public void SetReminders(List<Reminder> reminders)
        {
            _activeReminders.Clear();
            _activeReminders.AddRange(reminders);
            NotifyObservers();
        }

        public void AddReminder(Reminder reminder)
        {
            _activeReminders.Add(reminder);
            NotifyObservers();
        }

        public void UpdateReminder(Reminder reminder)
        {
            // Integer ID ile karşılaştırma
            int index = _activeReminders.FindIndex(r => r.Id == reminder.Id);
            if (index >= 0)
            {
                _activeReminders[index] = reminder;
                NotifyObservers();
            }
        }

        public void RemoveReminder(int reminderId)
        {
            // Integer ID ile karşılaştırma
            int index = _activeReminders.FindIndex(r => r.Id == reminderId);
            if (index >= 0)
            {
                _activeReminders.RemoveAt(index);
                NotifyObservers();
            }
        }

        private void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.UpdateReminders(_activeReminders);
            }
        }

        public List<Reminder> GetActiveReminders()
        {
            return new List<Reminder>(_activeReminders);
        }
    }
}