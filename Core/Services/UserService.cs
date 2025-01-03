using Core.Interfaces;
using Core.Models;

namespace Core.Services
{
    internal class UserService : IUserService
    {
        public void RegisterUser(User user)
        {
            throw new NotImplementedException();
        }

        public bool AuthenticateUser(string username, string password)
        {
            throw new NotImplementedException();
        }

        public User GetUserById(Guid userId)
        {
            throw new NotImplementedException();
        }

        public void UpdateUserExperience(Guid userId, int experiencePoints)
        {
            throw new NotImplementedException();
        }
    }
}
