using Cryptography;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Project_1.Models
{
    internal class EncryptClass
    {
        public EncryptClass(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }
        public string Value { get; set; }

        public EncryptClass Encrypt()
        {
            using var hashing = SHA256.Create();
            byte[] keyHash = hashing.ComputeHash(Encoding.Unicode.GetBytes(Key));
            string key = Base64UrlEncoder.Encode(keyHash);
            string message = Base64UrlEncoder.Encode(Encoding.Unicode.GetBytes(Value));
            return new(Key, Fernet.Encrypt(key, message));
        }

        public EncryptClass Decrypt()
        {
            using var hashing = SHA256.Create();
            byte[] keyHash = hashing.ComputeHash(Encoding.Unicode.GetBytes(Key));
            string key = Base64UrlEncoder.Encode(keyHash);
            string encodedSecret = Fernet.Decrypt(key, Value);
            string message = Encoding.Unicode.GetString(Base64UrlEncoder.DecodeBytes(encodedSecret));
            return new(Key, message);
        }
    }
}
