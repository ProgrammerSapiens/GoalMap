using Core.Models;
using Task = System.Threading.Tasks.Task;

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
        /// <returns>The user with the specified ID.</returns>
        Task<User?> GetUserByIdAsync(Guid userId);

        /// <summary>
        /// Updates the experience points of a user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="experiencePoints">The amount of experience points to add.</param>
        Task UpdateUserExperienceAsync(Guid userId, int experiencePoints);

        /// <summary>
        /// Authenticates a user with the specified credentials.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>
        /// <c>true</c> if the credentials are valid; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> AuthenticateUserAsync(string username, string password);

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="user">The user to register.</param>
        Task<Guid> RegisterUserAsync(User user);
    }
}
