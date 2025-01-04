using Core.Models;

namespace Core.Interfaces
{
    /// <summary>
    /// Provides methods for managing todo categories, including CRUD operations.
    /// </summary>
    public interface IToDoCategoryService
    {
        /// <summary>
        /// Retrieves a category by its unique ID.
        /// </summary>
        /// <param name="toDoCategoryId">The unique identifier of the category.</param>
        /// <returns></returns>
        Task<ToDoCategory> GetToDoCategoryByIdAsync(Guid toDoCategoryId);

        /// <summary>
        /// Retrieves a list of categories associated with a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A list of categores belonging to the specified user.</returns>
        Task<List<ToDoCategory>> GetToDoCategoriesByUserAsync(Guid userId);

        /// <summary>
        /// Adds a new category to the system.
        /// </summary>
        /// <param name="toDoCategory">The category to be added.</param>
        Task AddToDoCategoryAsync(ToDoCategory toDoCategory);

        /// <summary>
        /// Updates an existing task category in the system.
        /// </summary>
        /// <param name="taskCategory">The task category with updated information.</param>
        Task UpdateToDoCategoryAsync(ToDoCategory toDoCategory);

        /// <summary>
        /// Deletes a task category by its unique identifier.
        /// </summary>
        /// <param name="taskCategoryId">The unique identifier of the task category to be deleted.</param>
        Task DeleteToDoCategoryAsync(Guid toDoCategoryId);
    }
}
