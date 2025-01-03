using Core.Interfaces;
using Core.Models;

namespace Core.Services
{
    internal class UserService : IUserService
    {
        public Task<User?> GetUserByIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task UpdateUserExperienceAsync(Guid userId, int experiencePoints)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AuthenticateUserAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> RegisterUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
