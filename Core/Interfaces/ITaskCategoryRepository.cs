using Core.Models;

namespace Core.Interfaces
{
    /// <summary>
    /// Provides methods for managing task categories, including CRUD operations.
    /// </summary>
    internal interface ITaskCategoryRepository
    {
        /// <summary>
        /// Retrieves all categories associated with a specific user. 
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A list of categories associated with the user.</returns>
        List<TaskCategory> GetByUserId(int userId);

        /// <summary>
        /// Retrieves a category by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the category</param>
        /// <returns>The category with the specifier identifier.</returns>
        TaskCategory GetById(int id);

        /// <summary>
        /// Adds a new category to the repository.
        /// </summary>
        /// <param name="category">The category to be added.</param>
        void Add(TaskCategory category);

        /// <summary>
        /// Updates an existing category in the repository.
        /// </summary>
        /// <param name="category">The category to be updated.</param>
        void Update(TaskCategory category);

        /// <summary>
        /// Deletes a category from the repository by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the category</param>
        void Delete(int id);
    }
}
