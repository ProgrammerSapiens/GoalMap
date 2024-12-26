using Core.Models;

namespace Core.Interfaces
{
    /// <summary>
    /// Provides methods for managing users, including CRUD operations. 
    /// </summary>
    internal interface IUserRepository
    {
        /// <summary>
        /// Retrieves a user by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>The user with the specified identifier.</returns>
        User GetById(int id);

        /// <summary>
        /// Retrieves a user by its unique username.
        /// </summary>
        /// <param name="userName">The unique username of the user.</param>
        /// <returns>The user with the specified username.</returns>
        User GetByUserName(string userName);

        /// <summary>
        /// Adds a new User to the repository.
        /// </summary>
        /// <param name="user">The user to be added.</param>
        void Add(User user);

        /// <summary>
        /// Updates an existing user in the repository.
        /// </summary>
        /// <param name="user">The user to be updated.</param>
        void Update(User user);

        /// <summary>
        /// Checks if the user with such a username exists.
        /// </summary>
        /// <param name="username">The unique username of the user.</param>
        /// <returns>
        /// <c>true</c> if the user exists, and <c>false</c> if its not.
        /// </returns>
        bool Exists(string username);
    }
}
