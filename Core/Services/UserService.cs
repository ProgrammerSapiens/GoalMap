using Core.Interfaces;
using Core.Models;

namespace Core.Services
{
    internal class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public UserService(IUserRepository repository, IPasswordHasher passwordHasher)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
        }

        public Task<User?> GetUserByIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserExperienceAsync(Guid userId, int experiencePoints)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AuthenticateUserAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> RegisterUserAsync(User? user)
        {
            throw new NotImplementedException();
        }
    }
}
