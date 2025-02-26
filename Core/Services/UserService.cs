using Core.Interfaces;
using Core.Models;
using Microsoft.Extensions.Logging;

namespace Core.Services
{
    /// <summary>
    /// Provides user management functionalities such as authentication, registration, and experience updates.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<UserService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="repository">The repository for accessing user data.</param>
        /// <param name="passwordHasher">The password hasher for secure password management.</param>
        /// <exception cref="ArgumentNullException">Thrown if either repository or passwordHasher is null.</exception>
        public UserService(IUserRepository repository, IPasswordHasher passwordHasher, ILogger<UserService> logger)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>The user with the specified identifier, or null if not found.</returns>
        public async Task<User?> GetUserByUserIdAsync(Guid userId)
        {
            _logger.LogInformation($"GetUserByUserIdAsync({userId})");

            return await _repository.GetUserByUserIdAsync(userId);
        }

        /// <summary>
        /// Retrieves a user by their unique name.
        /// </summary>
        /// <param name="userName">The unique name of the user.</param>
        /// <returns>The user with the specified identifier, or null if not found.</returns>
        public async Task<User?> GetUserByUserNameAsync(string userName)
        {
            _logger.LogInformation($"GetUserByUserNameAsync({userName})");

            return await _repository.GetUserByUserNameAsync(userName);
        }

        /// <summary>
        /// Registers a new user with a hashed password.
        /// </summary>
        /// <param name="user">The user to register.</param>
        /// <param name="password">The password to be hashed and stored.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the username is already taken.</exception>
        public async Task RegisterUserAsync(User user, string password)
        {
            _logger.LogInformation($"RegisterUserAsync(User {user.UserName}, {password})");

            var existingUser = await _repository.GetUserByUserNameAsync(user.UserName);
            if (existingUser != null)
            {
                _logger.LogWarning("User name is already exists.");
                throw new InvalidOperationException("User name is already exists.");
            }

            user.PasswordHash = await _passwordHasher.HashPasswordAsync(password);

            await _repository.AddUserAsync(user);
            await CreateDefaultCategoriesAsync(user.UserId);
        }

        /// <summary>
        /// Authenticates a user by verifying their credentials.
        /// </summary>
        /// <param name="userName">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>True if authentication is successful; otherwise, false.</returns>
        public async Task<bool> AuthenticateUserAsync(string userName, string password)
        {
            _logger.LogInformation($"AuthenticateUserAsync({userName}, {password})");

            var user = await _repository.GetUserByUserNameAsync(userName);
            if (user == null)
            {
                _logger.LogInformation($"User {userName} was not found.");
                return false;
            }

            var passwordIsRight = await _passwordHasher.VerifyPasswordAsync(password, user.PasswordHash);
            if (!passwordIsRight)
            {
                _logger.LogInformation($"Password {password} is invalid");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Updates the user's information.
        /// </summary>
        /// <param name="user">The user with updated details.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateUserAsync(User user)
        {
            _logger.LogInformation($"UpdateUserAsync(User {user.UserName})");

            await _repository.UpdateUserAsync(user);
        }

        /// <summary>
        /// Creates default to-do categories for a newly registered user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task CreateDefaultCategoriesAsync(Guid userId)
        {
            var defaultToDoCategories = new List<ToDoCategory>()
            {
                new ToDoCategory(userId, "Habbit"),
                new ToDoCategory(userId, "Other")
            };

            await _repository.AddDefaultCategoriesAsync(defaultToDoCategories);
        }
    }
}
