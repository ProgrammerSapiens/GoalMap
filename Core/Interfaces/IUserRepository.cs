using Core.Models;

namespace Core.Interfaces
{
    /// <summary>
    /// Provides methods for managing users, including CRUD operations. 
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Retrieves a user by its unique username.
        /// </summary>
        /// <param name="userName">The unique username of the user.</param>
        /// <returns>The user with the specified username.</returns>
        Task<User?> GetUserByUserNameAsync(string userName);

        /// <summary>
        /// Adds a new User to the repository.
        /// </summary>
        /// <param name="user">The user to be added.</param>
        Task AddUserAsync(User user);

        /// <summary>
        /// Updates an existing user in the repository.
        /// </summary>
        /// <param name="user">The user to be updated.</param>
        Task UpdateUserAsync(User user);

        /// <summary>
        /// Checks if the user with such a username exists.
        /// </summary>
        /// <param name="username">The unique username of the user.</param>
        /// <returns>
        /// <c>true</c> if the user exists, and <c>false</c> if its not.
        /// </returns>
        Task<bool> IsUserExists(string username);
    }
}
