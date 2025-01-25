using Core.Interfaces;

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
