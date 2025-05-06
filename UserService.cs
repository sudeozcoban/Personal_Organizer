using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonalOrganizer
{
    public class UserService : IDisposable
    {
        private CsvHelper<User> _csvHelper;
        private List<User> _users;

        public UserService()
        {
            _csvHelper = new CsvHelper<User>("users.csv");
            _users = _csvHelper.ReadAll();

            // Kullanıcı listesi null ise yeni liste oluştur
            if (_users == null)
            {
                _users = new List<User>();
                _csvHelper.Write(_users);
            }
        }

        public List<User> GetAllUsers()
        {
            try
            {
                // Dosyayı her seferinde yeniden oku
                _users = _csvHelper.ReadAll();

                // Null kontrolü
                if (_users == null)
                {
                    _users = new List<User>();
                }

                return _users;
            }
            catch (Exception ex)
            {
                throw new Exception($"Kullanıcılar okunamadı: {ex.Message}");
            }
        }

        public User GetUserById(Guid id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public User GetUserByUsername(string username)
        {
            return _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public bool AuthenticateUser(string username, string password, out User user)
        {
            // Kullanıcıları her seferinde yeniden oku
            _users = _csvHelper.ReadAll();

            user = _users.FirstOrDefault(u =>
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                u.Password == password);
            return user != null;
        }

        public void CreateUser(User user)
        {
            // Kullanıcı listesini her seferinde yeniden oku
            _users = _csvHelper.ReadAll();

            if (_users == null)
            {
                _users = new List<User>();
            }

            // Validate username is unique
            if (_users.Any(u => u.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception("Kullanıcı adı zaten mevcut");
            }

            // Eğer bu ilk kullanıcı ise, admin yetkisi ver
            if (_users.Count == 0)
            {
                user.UserType = UserType.Admin;
            }
            else
            {
                // Diğer kullanıcılar normal kullanıcı olsun
                user.UserType = UserType.Normal;
            }

            _users.Add(user);
            _csvHelper.Write(_users);
        }

        public void UpdateUser(User user)
        {
            var existingUser = _users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser == null)
            {
                throw new Exception("Kullanıcı bulunamadı");
            }

            // If changing username, validate it's unique
            if (!existingUser.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase) &&
                _users.Any(u => u.Id != user.Id && u.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception("Kullanıcı adı zaten mevcut");
            }

            // Update user properties
            int index = _users.FindIndex(u => u.Id == user.Id);
            user.UpdatedAt = DateTime.Now;
            _users[index] = user;

            _csvHelper.Write(_users);
        }

        public void DeleteUser(Guid id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                throw new Exception("Kullanıcı bulunamadı");
            }

            _users.Remove(user);
            _csvHelper.Write(_users);
        }

        public void ChangeUserType(Guid id, UserType newType)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                throw new Exception("Kullanıcı bulunamadı");
            }

            user.UserType = newType;
            user.UpdatedAt = DateTime.Now;

            _csvHelper.Write(_users);
        }

        public bool ResetPassword(Guid id, string newPassword)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                throw new Exception("Kullanıcı bulunamadı");
            }

            user.Password = newPassword;
            user.UpdatedAt = DateTime.Now;

            _csvHelper.Write(_users);
            return true;
        }

        public bool ChangePassword(Guid id, string currentPassword, string newPassword)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                throw new Exception("Kullanıcı bulunamadı");
            }

            if (user.Password != currentPassword)
            {
                return false;
            }

            user.Password = newPassword;
            user.UpdatedAt = DateTime.Now;

            _csvHelper.Write(_users);
            return true;
        }

        public void Dispose()
        {
            // Ensure changes are written
            _csvHelper.Write(_users);
        }

        public bool UserExists(string username)
        {
            return _users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public bool EmailExists(string email)
        {
            return _users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public bool IsFirstUser()
        {
            return _users.Count == 0; // Hiç kullanıcı yoksa true döner
        }
    }
}