using Core.Models;

namespace Core.Interfaces
{
    /// <summary>
    /// Defines operations related to user management.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>The user object if found; otherwise, <c>null</c>.</returns>
        Task<User?> GetUserByUserIdAsync(Guid userId);

        /// <summary>
        /// Retrieves a user by their unique name.
        /// </summary>
        /// <param name="userName">The unique name of the user.</param>
        /// <returns>The user object if found; otherwise, <c>null</c>.</returns>
        Task<User?> GetUserByUserNameAsync(string userName);

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="user">The user object to register.</param>
        /// <param name="password">The user's password.</param>
        Task RegisterUserAsync(User user, string password);

        /// <summary>
        /// Authenticates a user with the provided credentials.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>
        /// <c>true</c> if the credentials are valid; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> AuthenticateUserAsync(string username, string password);

        /// <summary>
        /// Updates the user's information.
        /// </summary>
        /// <param name="user">The user object containing updated data.</param>
        Task UpdateUserAsync(User user);

        /// <summary>
        /// Updates a user's experience points based on difficulty level.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="difficulty">The difficulty level affecting experience points.</param>
        Task UpdateUserExperienceAsync(Guid userId, Difficulty difficulty);
    }
}
