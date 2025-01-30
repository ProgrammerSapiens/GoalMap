using Core.Models;

namespace Core.Interfaces
{
    /// <summary>
    /// Provides methods for managing todo categories in the repository, including CRUD operations.
    /// </summary>
    public interface IToDoCategoryRepository
    {
        /// <summary>
        /// Retrieves a category by its name and associated user ID.
        /// </summary>
        /// <param name="toDoCategoryName">The name of the ToDo category.</param>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>The category associated with the specified name and user ID.</returns>
        Task<ToDoCategory?> GetToDoCategoryByCategoryIdAsync(Guid userId, Guid toDoCategoryId);

        /// <summary>
        /// Retrieves all categories associated with a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A list of categories associated with the specified user.</returns>
        Task<List<ToDoCategory>> GetToDoCategoriesByUserIdAsync(Guid userId);

        /// <summary>
        /// Adds a new category to the repository.
        /// </summary>
        /// <param name="toDoCategory">The category to be added.</param>
        Task AddToDoCategoryAsync(ToDoCategory toDoCategory);

        /// <summary>
        /// Updates an existing category in the repository.
        /// </summary>
        /// <param name="toDoCategory">The category to be updated with new information.</param>
        Task UpdateToDoCategoryAsync(ToDoCategory toDoCategory);

        Task UpdateCategoryInToDosAsync(Guid userId, string oldCategoryName, string newCategoryName);

        /// <summary>
        /// Deletes a category from the repository by its name and associated user ID.
        /// </summary>
        /// <param name="toDoCategoryName">The name of the ToDo category to be deleted.</param>
        /// <param name="userId">The unique identifier of the user.</param>
        Task DeleteToDoCategoryAsync(Guid userId, Guid toDoCategoryId);

        /// <summary>
        /// Checks if a category exists in the repository by its name and associated user ID.
        /// </summary>
        /// <param name="toDoCategoryName">The name of the ToDo category.</param>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>True if the category exists, otherwise false.</returns>
        Task<bool> CategoryExistsByNameAsync(Guid userId, string toDoCategoryName);
    }
}
