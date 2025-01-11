using Core.Interfaces;
using Core.Models;

namespace Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        public async Task<User?> GetUserByUserNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public async Task AddUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsUserExistsAsync(string username)
        {
            throw new NotImplementedException();
        }
    }
}
