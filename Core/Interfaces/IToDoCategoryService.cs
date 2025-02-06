using Core.Models;

namespace Core.Interfaces
{
    /// <summary>
    /// Provides methods for managing To-Do categories, including CRUD operations.
    /// </summary>
    public interface IToDoCategoryService
    {
        /// <summary>
        /// Retrieves a To-Do category by its unique identifier.
        /// </summary>
        /// <param name="toDoCategoryId">The unique identifier of the To-Do category.</param>
        /// <returns>The To-Do category associated with the specified ID, or <c>null</c> if not found.</returns>
        Task<ToDoCategory?> GetToDoCategoryByCategoryIdAsync(Guid toDoCategoryId);

        /// <summary>
        /// Retrieves all To-Do categories associated with a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A list of To-Do categories belonging to the specified user.</returns>
        Task<List<ToDoCategory>> GetToDoCategoriesByUserIdAsync(Guid userId);

        /// <summary>
        /// Adds a new To-Do category.
        /// </summary>
        /// <param name="toDoCategory">The To-Do category to be added.</param>
        Task AddToDoCategoryAsync(ToDoCategory toDoCategory);

        /// <summary>
        /// Updates an existing To-Do category with new information.
        /// </summary>
        /// <param name="toDoCategory">The To-Do category containing updated details.</param>
        Task UpdateToDoCategoryAsync(ToDoCategory toDoCategory);

        /// <summary>
        /// Deletes a To-Do category by its unique identifier.
        /// </summary>
        /// <param name="toDoCategoryId">The unique identifier of the To-Do category to be deleted.</param>
        Task DeleteToDoCategoryAsync(Guid toDoCategoryId);
    }
}
