using Core.Models;

namespace Core.Interfaces
{
    /// <summary>
    /// Defines operations related to user management.
    /// </summary>
    internal interface IUserService
    {
        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>The user with the specified ID.</returns>
        User GetUserById(int id);

        /// <summary>
        /// Updates the experience points of a user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="experiencePoints">The amount of experience points to add.</param>
        void UpdateUserExperience(int userId, int experiencePoints);

        /// <summary>
        /// Authenticates a user with the specified credentials.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>
        /// <c>true</c> if the credentials are valid; otherwise, <c>false</c>.
        /// </returns>
        bool AuthenticateUser(string username, string password);

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="user">The user to register.</param>
        void RegisterUser(User user);
    }
}
