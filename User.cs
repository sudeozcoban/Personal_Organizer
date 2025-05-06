using System;

namespace PersonalOrganizer
{
    public enum UserType
    {
        Normal,
        PartTime,
        Admin
    }

    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public UserType UserType { get; set; }
        public string Photo { get; set; } // Base64 encoded photo
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
       


        public User()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            UserType = UserType.Normal;
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}