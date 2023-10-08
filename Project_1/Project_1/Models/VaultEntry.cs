using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Project_1.Models
{
    internal class VaultEntry
    {
        public static string VaultCsvPath = Path.Combine("..", "..", "..", "resources", "vault.csv");
        public static string UserCsvPath = Path.Combine("..", "..", "..", "resources", "user.csv");

        public VaultEntry(string userid, string username, string password, string website)
        {
            Userid = userid;
            Username = username;
            EncryptClass encrypt = new EncryptClass(user.Email, password).Encrypt();
            this.password = encrypt.Value;
            Site = website;
        }

        [Name("userid")]
        public string? Userid { get; set; }
        [Name("username")]
        public string? Username { get; set; }
        [Ignore]
        public User? user
        {
            get
            {
                using StreamReader reader = new(UserCsvPath);
                using CsvReader csv = new(
                    reader, CultureInfo.InvariantCulture);
                return csv.GetRecords<User>()
                    .Where(el => el.Username == Userid)
                    .FirstOrDefault();
            }
        }
        [Name("password")]
        private string password;
        public string? Password {
            get {
                EncryptClass encrypt = new EncryptClass(user.Email, password).Decrypt();
                return encrypt.Value;
            }
        }
        [Name("website")]
        public string? Site { get; set; }

        override public string ToString()
        {
            return "Username: " + Userid +
                "\tPassword: " + Password +
                "\tSite: " + Site + "\n";
        }
    }
}
