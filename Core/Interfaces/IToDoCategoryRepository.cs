using Core.Models;

namespace Core.Interfaces
{
    /// <summary>
    /// Provides methods for managing todo categories, including CRUD operations.
    /// </summary>
    internal interface IToDoCategoryRepository
    {
        /// <summary>
        /// Retrieves all categories associated with a specific user. 
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A list of categories associated with the user.</returns>
        Task<List<ToDoCategory>> GetToDoCategoryByUserIdAsync(int userId);

        /// <summary>
        /// Retrieves a category by its unique identifier.
        /// </summary>
        /// <param name="toDoCategoryId">The unique identifier of the category</param>
        /// <returns>The category with the specifier identifier.</returns>
        Task<ToDoCategory> GetToDoCategoryByIdAsync(int toDoCategoryId);

        /// <summary>
        /// Adds a new category to the repository.
        /// </summary>
        /// <param name="toDoCategory">The category to be added.</param>
        Task AddToDoCategoryAsync(ToDoCategory toDoCategory);

        /// <summary>
        /// Updates an existing category in the repository.
        /// </summary>
        /// <param name="toDoCategory">The category to be updated.</param>
        Task UpdateToDoCategoryAsync(ToDoCategory toDoCategory);

        /// <summary>
        /// Deletes a category from the repository by its unique identifier.
        /// </summary>
        /// <param name="toDoCategoryId">The unique identifier of the category</param>
        Task DeleteToDoCategoryAsync(int toDoCategoryId);
    }
}
