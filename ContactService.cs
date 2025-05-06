using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonalOrganizer
{
    public class ContactService : IDisposable
    {
        private CsvHelper<Contact> _csvHelper;
        private List<Contact> _contacts;

        public ContactService()
        {
            _csvHelper = new CsvHelper<Contact>("contacts.csv");
            _contacts = _csvHelper.ReadAll();
        }

        public List<Contact> GetAllContacts()
        {
            return _contacts;
        }

        public List<Contact> GetContactsByUserId(Guid userId)
        {
            return _contacts.Where(c => c.UserId == userId).ToList();
        }

        public Contact GetContactById(Guid id)
        {
            return _contacts.FirstOrDefault(c => c.Id == id);
        }

        public void CreateContact(Contact contact)
        {
            _contacts.Add(contact);
            _csvHelper.Write(_contacts);
        }

        public void UpdateContact(Contact contact)
        {
            var existingContact = _contacts.FirstOrDefault(c => c.Id == contact.Id);
            if (existingContact == null)
            {
                throw new Exception("Kişi bulunamadı");
            }

            int index = _contacts.FindIndex(c => c.Id == contact.Id);
            contact.UpdatedAt = DateTime.Now;
            _contacts[index] = contact;

            _csvHelper.Write(_contacts);
        }

        public void DeleteContact(Guid id)
        {
            var contact = _contacts.FirstOrDefault(c => c.Id == id);
            if (contact == null)
            {
                throw new Exception("Kişi bulunamadı");
            }

            _contacts.Remove(contact);
            _csvHelper.Write(_contacts);
        }

        public void Dispose()
        {
            // Değişikliklerin yazıldığından emin ol
            _csvHelper.Write(_contacts);
        }
    }
}