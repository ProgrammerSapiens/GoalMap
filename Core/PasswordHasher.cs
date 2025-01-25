using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class PasswordHasher : IPasswordHasher
    {
        public Task<string> HashPasswordAsync(string password)
        {
            throw new NotImplementedException();
        }

        public Task<bool> VerifyPasswordAsync(string password, string hashedPassword)
        {
            throw new NotImplementedException();
        }
    }
}
