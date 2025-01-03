using Core.Models;

namespace Core.Interfaces
{
    /// <summary>
    /// Provides methods for managing task categories, including CRUD operations.
    /// </summary>
    internal interface ITaskCategoryService
    {
        /// <summary>
        /// Retrieves a task category by its unique ID.
        /// </summary>
        /// <param name="taskCategoryId">The unique identifier of the task category.</param>
        /// <returns></returns>
        TaskCategory GetTaskCategoryById(Guid taskCategoryId);

        /// <summary>
        /// Retrieves a list of task categories associated with a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A list of task categores belonging to the specified user.</returns>
        List<TaskCategory> GetCategoriesByUser(Guid userId);

        /// <summary>
        /// Adds a new task category to the system.
        /// </summary>
        /// <param name="taskCategory">The task category to be added.</param>
        void AddCategory(TaskCategory taskCategory);

        /// <summary>
        /// Updates an existing task category in the system.
        /// </summary>
        /// <param name="taskCategory">The task category with updated information.</param>
        void UpdateCategory(TaskCategory taskCategory);

        /// <summary>
        /// Deletes a task category by its unique identifier.
        /// </summary>
        /// <param name="taskCategoryId">The unique identifier of the task category to be deleted.</param>
        void DeleteCategory(Guid taskCategoryId);
    }
}
