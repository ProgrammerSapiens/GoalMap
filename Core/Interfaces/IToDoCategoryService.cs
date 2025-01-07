using Core.Models;

namespace Core.Interfaces
{
    /// <summary>
    /// Provides methods for managing todo categories, including CRUD operations.
    /// </summary>
    public interface IToDoCategoryService
    {
        /// <summary>
        /// Retrieves a ToDo category by its name and associated user ID.
        /// </summary>
        /// <param name="toDoCategoryName">The name of the ToDo category.</param>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>The ToDo category associated with the specified name and user ID.</returns>
        Task<ToDoCategory> GetToDoCategoryByCategoryNameAsync(string toDoCategoryName, Guid userId);

        /// <summary>
        /// Retrieves a list of ToDo categories associated with a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A list of ToDo categories belonging to the specified user.</returns>
        Task<List<ToDoCategory>> GetToDoCategoriesByUserIdAsync(Guid userId);

        /// <summary>
        /// Adds a new ToDo category to the system.
        /// </summary>
        /// <param name="toDoCategory">The ToDo category to be added.</param>
        Task AddToDoCategoryAsync(ToDoCategory toDoCategory);

        /// <summary>
        /// Updates an existing ToDo category in the system with updated information.
        /// </summary>
        /// <param name="toDoCategory">The ToDo category with updated information.</param>
        Task UpdateToDoCategoryAsync(ToDoCategory toDoCategory);

        /// <summary>
        /// Deletes a ToDo category by its name and associated user ID.
        /// </summary>
        /// <param name="toDoCategoryName">The name of the ToDo category to be deleted.</param>
        /// <param name="userId">The unique identifier of the user.</param>
        Task DeleteToDoCategoryAsync(string toDoCategoryName, Guid userId);
    }
}
