using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;

namespace YourForum.Core.Services
{
    public class DefaultPasswordService : IPasswordService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        }

        public bool VerifyPassword(string hashedPassword, string testPassword)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(testPassword, hashedPassword);
        }
    }
}
