using Core.Models;

namespace Core.Interfaces
{
    /// <summary>
    /// Provides methods for managing task categories, including CRUD operations.
    /// </summary>
    internal interface ITaskCategoryService
    {
        /// <summary>
        /// Retrieves a list of task categories associated with a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A list of task categores belonging to the specified user.</returns>
        List<TaskCategory> GetCategoriesByUser(int userId);

        /// <summary>
        /// Adds a new task category to the system.
        /// </summary>
        /// <param name="category">The task category to be added.</param>
        void AddCategory(TaskCategory category);

        /// <summary>
        /// Updates an existing task category in the system.
        /// </summary>
        /// <param name="category">The task category with updated information.</param>
        void UpdateCategory(TaskCategory category);

        /// <summary>
        /// Deletes a task category by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the task category to be deleted.</param>
        void DeleteCategory(int id);
    }
}
