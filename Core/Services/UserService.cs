using Core.Interfaces;
using Core.Models;

namespace Core.Services
{
    /// <summary>
    /// Provides user management functionalities such as authentication, registration, and experience updates.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _passwordHasher;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="repository">The repository for accessing user data.</param>
        /// <param name="passwordHasher">The password hasher for secure password management.</param>
        /// <exception cref="ArgumentNullException">Thrown if either repository or passwordHasher is null.</exception>
        public UserService(IUserRepository repository, IPasswordHasher passwordHasher)
        {
            if (repository == null || passwordHasher == null)
            {
                throw new ArgumentNullException("You cannot initialize sources with null.");
            }

            _repository = repository;
            _passwordHasher = passwordHasher;
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>The user with the specified identifier, or null if not found.</returns>
        public async Task<User?> GetUserByUserIdAsync(Guid userId)
        {
            return await _repository.GetUserByUserIdAsync(userId);
        }

        /// <summary>
        /// Retrieves a user by their unique name.
        /// </summary>
        /// <param name="userName">The unique name of the user.</param>
        /// <returns>The user with the specified identifier, or null if not found.</returns>
        public async Task<User?> GetUserByUserNameAsync(string userName)
        {
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
            var existingUser = await _repository.GetUserByUserNameAsync(user.UserName);

            if (existingUser != null)
            {
                throw new InvalidOperationException("User name is already exists.");
            }

            string hashedPassword = await _passwordHasher.HashPasswordAsync(password);

            user.PasswordHash = hashedPassword;

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
            var user = await _repository.GetUserByUserNameAsync(userName);

            if (user == null)
            {
                return false;
            }

            var passwordIsRight = await _passwordHasher.VerifyPasswordAsync(password, user.PasswordHash);

            if (!passwordIsRight)
            {
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
            await _repository.UpdateUserAsync(user);
        }

        /// <summary>
        /// Updates the user's experience points based on task difficulty.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="taskDifficulty">The difficulty level of the completed task.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the user does not exist.</exception>
        public async Task UpdateUserExperienceAsync(Guid userId, Difficulty taskDifficulty)
        {
            var user = await _repository.GetUserByUserIdAsync(userId);

            if (user == null)
            {
                throw new InvalidOperationException("User was not found.");
            }

            user.Experience += (int)taskDifficulty;

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
