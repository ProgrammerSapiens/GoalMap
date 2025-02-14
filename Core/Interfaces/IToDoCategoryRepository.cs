using Core.Models;

namespace Core.Interfaces
{
    /// <summary>
    /// Provides methods for managing To-Do categories in the repository, including CRUD operations.
    /// </summary>
    public interface IToDoCategoryRepository
    {
        /// <summary>
        /// Retrieves a To-Do category by its unique identifier.
        /// </summary>
        /// <param name="toDoCategoryId">The unique identifier of the To-Do category.</param>
        /// <returns>The To-Do category associated with the specified ID, or <c>null</c> if not found.</returns>
        Task<ToDoCategory?> GetToDoCategoryByCategoryIdAsync(Guid toDoCategoryId);

        /// <summary>
        /// Retrieves a To-Do category by its name.
        /// </summary>
        /// <param name="toDoCategoryName">The unique identifier of the To-Do category.</param>
        /// <returns>The To-Do category associated with the specified name, or <c>null</c> if not found.</returns>
        Task<ToDoCategory?> GetToDoCategoryByCategoryNameAsync(string toDoCategoryName);

        /// <summary>
        /// Retrieves all To-Do categories associated with a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A list of To-Do categories belonging to the specified user.</returns>
        Task<List<ToDoCategory>> GetToDoCategoriesByUserIdAsync(Guid userId);

        /// <summary>
        /// Adds a new To-Do category to the repository.
        /// </summary>
        /// <param name="toDoCategory">The To-Do category to be added.</param>
        Task AddToDoCategoryAsync(ToDoCategory toDoCategory);

        /// <summary>
        /// Updates an existing To-Do category in the repository.
        /// </summary>
        /// <param name="toDoCategory">The To-Do category containing updated details.</param>
        Task UpdateToDoCategoryAsync(ToDoCategory toDoCategory);

        /// <summary>
        /// Updates the category name in all associated To-Do tasks.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="oldCategoryId">The current id of the category.</param>
        /// <param name="newCategoryId">The new id to replace the old one.</param>
        Task UpdateCategoryInToDosAsync(Guid userId, Guid oldCategoryId, Guid newCategoryId);

        /// <summary>
        /// Deletes a To-Do category from the repository by its unique identifier.
        /// </summary>
        /// <param name="toDoCategoryId">The unique identifier of the To-Do category to be deleted.</param>
        Task DeleteToDoCategoryAsync(Guid toDoCategoryId);

        /// <summary>
        /// Checks if a To-Do category exists in the repository by its name and associated user ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="toDoCategoryName">The name of the To-Do category.</param>
        /// <returns><c>true</c> if the category exists; otherwise, <c>false</c>.</returns>
        Task<bool> CategoryExistsByNameAsync(Guid userId, string toDoCategoryName);
    }
}
