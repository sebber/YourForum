using System;
using System.Collections.Generic;
using System.Text;

namespace YourForum.Core.Services
{
    public interface IPasswordService
    {
        string HashPassword(string password);

        bool VerifyPassword(string hashedPassword, string testPassword);
    }
}
