using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_1.Models
{
    internal class User
    {
        public static string VaultCsvPath = Path.Combine("..", "..", "..", "resources", "vault.csv");
        public static string UserCsvPath = Path.Combine("..", "..", "..", "resources", "user.csv");

        public User(string username, string password, string email, string firstname, string lastname)
        {
            Username = username;
            EncryptClass encrypt = new EncryptClass(Username, password).Encrypt();
            this.password = encrypt.Value;
            Email = email;
            Firstname = firstname;
            Lastname = lastname;
        }

        [Name("username")]
        public string? Username { get; set; }
        [Name("password")]
        public string password;
        public string? Password {
            get
            {
                EncryptClass encrypt = new EncryptClass(Username, password).Decrypt();
                return encrypt.Value;
            }
        }
        [Name("email")]
        public string? Email { get; set; }
        [Name("firstname")]
        public string? Firstname { get; set; }
        [Name("lastname")]
        public string? Lastname { get; set; }

        override public string ToString()
        {
            return "Username: " + Username + 
                "\tPassword: " + password +
                "\tEmail: " + Email + 
                "\tFistname: " + Firstname + 
                "\tLastname: " + Lastname;
        }
    }
}
