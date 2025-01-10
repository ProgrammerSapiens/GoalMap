using Core.Interfaces;
using Core.Models;

namespace Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher? _passwordHasher;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public UserService(IUserRepository repository, IPasswordHasher passwordHasher)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException("User name cannot be null or empty.");
            }

            var result = await _repository.GetUserByUserNameAsync(userName);

            if (result == null)
            {
                throw new InvalidOperationException("User does not exist.");
            }

            return result;
        }

        public async Task UpdateUserExperienceAsync(string userName, Difficulty taskDifficulty)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("User name cannot be null or empty.");
            }

            var user = await _repository.GetUserByUserNameAsync(userName);

            if (user == null)
            {
                throw new InvalidOperationException("User does not exist.");
            }

            int newUserExperience = user.Experience + (int)taskDifficulty;
            user.Experience = newUserExperience;

            await _repository.UpdateUserAsync(user);
        }

        public async Task<bool> AuthenticateUserAsync(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException();
            }

            var user = await _repository.GetUserByUserNameAsync(userName);

            if (user == null)
            {
                return false;
            }

            if (!await _passwordHasher.VerifyPasswordAsync(password, user.PasswordHash))
            {
                return false;
            }

            return true;
        }

        public async Task RegisterUserAsync(User user, string password)
        {
            if (await _repository.IsUserExistsAsync(user.UserName))
            {
                throw new InvalidOperationException("User name is already exists.");
            }

            string hashedPassword = await _passwordHasher.HashPasswordAsync(password);

            user.PasswordHash = hashedPassword;

            await _repository.AddUserAsync(user);
        }
    }
}
