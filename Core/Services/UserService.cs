using Core.Interfaces;
using Core.Models;

namespace Core.Services
{
    /// <summary>
    /// Service for managing user operations such as authentication, registration, and experience updates.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IToDoCategoryService _toDoCategoryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class with a user repository and password hasher.
        /// </summary>
        /// <param name="repository">The repository for user data access.</param>
        /// <param name="passwordHasher">The password hasher for secure password operations.</param>
        public UserService(IUserRepository? repository, IPasswordHasher? passwordHasher, IToDoCategoryService? toDoCategoryService)
        {
            if (repository == null || passwordHasher == null || toDoCategoryService == null)
            {
                throw new ArgumentNullException("You cannot initialize sources with null.");
            }

            _repository = repository;
            _passwordHasher = passwordHasher;
            _toDoCategoryService = toDoCategoryService;
        }

        /// <summary>
        /// Retrieves a user by their username.
        /// </summary>
        /// <param name="userName">The username of the user to retrieve.</param>
        /// <returns>The user with the specified username.</returns>
        /// <exception cref="ArgumentException">Thrown when the username is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the user does not exist.</exception>
        public async Task<User> GetUserByUserNameAsync(string? userName)
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

        /// <summary>
        /// Updates the experience of a user based on the difficulty of a completed task.
        /// </summary>
        /// <param name="userName">The username of the user to update.</param>
        /// <param name="taskDifficulty">The difficulty of the task that was completed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">Thrown when the username is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the user does not exist.</exception>
        public async Task UpdateUserExperienceAsync(string? userName, Difficulty taskDifficulty)
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

        /// <summary>
        /// Authenticates a user by verifying their username and password.
        /// </summary>
        /// <param name="userName">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns><c>true</c> if authentication is successful; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the username or password is null or empty.</exception>
        public async Task<bool> AuthenticateUserAsync(string? userName, string? password)
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

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="user">The user to register.</param>
        /// <param name="password">The password for the user.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the username already exists.</exception>
        public async Task RegisterUserAsync(User user, string? password)
        {
            if (await _repository.UserExistsAsync(user.UserName))
            {
                throw new InvalidOperationException("User name is already exists.");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password cannot be null or empty");
            }

            string hashedPassword = await _passwordHasher.HashPasswordAsync(password);

            user.PasswordHash = hashedPassword;

            await _repository.AddUserAsync(user);

            await CreateDefaultCategoriesAsync(user.UserId);
        }

        /// <summary>
        /// Creates and adds two default to-do categories for the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user for whom the categories will be created.</param>
        /// <exception cref="InvalidOperationException">Thrown if the to-do category service was not provided in the constructor.</exception>
        /// <remarks>
        /// The method creates two categories named "Habbit" and "Other" and adds them via the <c>_toDoCategoryService</c>.
        /// Each category is associated with the specified <paramref name="userId"/>. If the to-do category service was not injected into the constructor,
        /// the method will throw an <see cref="InvalidOperationException"/>.
        /// </remarks>
        private async Task CreateDefaultCategoriesAsync(Guid userId)
        {
            var defaultToDoCategories = new List<ToDoCategory>()
            {
                new ToDoCategory(userId, "Habbit"),
                new ToDoCategory(userId, "Other")
            };

            await _toDoCategoryService.AddToDoCategoryAsync(defaultToDoCategories[0]);
            await _toDoCategoryService.AddToDoCategoryAsync(defaultToDoCategories[1]);
        }
    }
}
