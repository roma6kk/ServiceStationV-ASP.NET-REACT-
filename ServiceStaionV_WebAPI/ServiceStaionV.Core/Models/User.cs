using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationV.Core.Models
{
    public class User
    {
        public const string ADMIN_ROLE = "admin";
        public const string USER_ROLE = "user";
        public const int MAX_NAME_LENGTH = 30;
        public const int MAX_EMAIL_LENGTH = 30;
        public const int MAX_PHONE_LENGTH = 12;
        private User( Guid id, string username, string email, string phonenumber, string passwordhash) 
        {
            Id = id;
            UserName = username;
            Email = email;
            PhoneNumber = phonenumber;
            PasswordHash = passwordhash;
            Role = USER_ROLE;
        }
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;   
        public string PhoneNumber { get; set; } = string.Empty; 
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public static User Create(Guid id, string username, string email, string phonenumber, string passwordhash)
        {
            return new User(id, username, email, phonenumber, passwordhash);
        }
    }
}
