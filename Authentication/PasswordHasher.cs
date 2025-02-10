using Core.Interfaces;

namespace Authentication
{
    public class PasswordHasher : IPasswordHasher
    {
        public Task<string> HashPasswordAsync(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return Task.FromResult(hashedPassword);
        }

        public Task<bool> VerifyPasswordAsync(string password, string hashedPassword)
        {
            bool isVerified = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            return Task.FromResult(isVerified);
        }
    }
}
