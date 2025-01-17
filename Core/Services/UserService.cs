using Core.Interfaces;
using Core.Models;
using Core.Services;

namespace Core.Services
{
    /// <summary>
    /// Service for managing user operations such as authentication, registration, and experience updates.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher? _passwordHasher;
        private readonly ToDoCategoryService _toDoCategoryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class with a user repository.
        /// </summary>
        /// <param name="repository">The repository for user data access.</param>
        public UserService(IUserRepository repository, ToDoCategoryService toDoCategoryService)
        {
            _repository = repository;
            _toDoCategoryService = toDoCategoryService;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class with a user repository and password hasher.
        /// </summary>
        /// <param name="repository">The repository for user data access.</param>
        /// <param name="passwordHasher">The password hasher for secure password operations.</param>
        public UserService(IUserRepository repository, IPasswordHasher passwordHasher, ToDoCategoryService toDoCategoryService)
        {
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

            string hashedPassword = await _passwordHasher.HashPasswordAsync(password);

            user.PasswordHash = hashedPassword;

            await _repository.AddUserAsync(user);

            var defaultToDoCategories = new List<ToDoCategory>()
            {
                new ToDoCategory(user.UserId, "Habbit"),
                new ToDoCategory(user.UserId, "Other")
            };
            await _toDoCategoryService.AddToDoCategoryAsync(defaultToDoCategories[0]);
            await _toDoCategoryService.AddToDoCategoryAsync(defaultToDoCategories[1]);
        }
    }
}
