using Core.Models;

namespace Core.Interfaces
{
    /// <summary>
    /// Provides methods for managing users, including CRUD operations.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>The user object if found; otherwise, <c>null</c>.</returns>
        Task<User?> GetUserByUserIdAsync(Guid userId);

        /// <summary>
        /// Retrieves a user by their unique username.
        /// </summary>
        /// <param name="userName">The unique username of the user.</param>
        /// <returns>The user object if found; otherwise, <c>null</c>.</returns>
        Task<User?> GetUserByUserNameAsync(string userName);

        /// <summary>
        /// Adds a new user to the repository.
        /// </summary>
        /// <param name="user">The user object to be added.</param>
        Task AddUserAsync(User user);

        /// <summary>
        /// Adds a list of default categories associated with a user.
        /// </summary>
        /// <param name="categories">The list of default ToDo categories.</param>
        Task AddDefaultCategoriesAsync(List<ToDoCategory> categories);

        /// <summary>
        /// Updates an existing user in the repository.
        /// </summary>
        /// <param name="user">The user object containing updated data.</param>
        Task UpdateUserAsync(User user);
    }
}
