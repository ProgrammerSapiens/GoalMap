using Core.Interfaces;
using Core.Models;
using Data.DBContext;

namespace Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

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

        public async Task<bool> UserExistsAsync(string username)
        {
            throw new NotImplementedException();
        }
    }
}
