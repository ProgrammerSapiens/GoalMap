using Core.Models;

namespace Core.Interfaces
{
    /// <summary>
    /// Defines a contract for managing To-Do tasks, including CRUD operations and additional functionalities like handling repeated tasks.
    /// </summary>
    public interface IToDoRepository
    {
        /// <summary>
        /// Retrieves a To-Do task by its unique identifier.
        /// </summary>
        /// <param name="toDoId">The unique identifier of the To-Do task.</param>
        /// <returns>The To-Do task corresponding to the provided identifier.</returns>
        Task<ToDo?> GetToDoByIdAsync(Guid toDoId);

        /// <summary>
        /// Retrieves a list of To-Do tasks for a specific user on a given date and time block.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="date">The date for which the To-Do tasks are retrieved.</param>
        /// <param name="timeBlock">The time block for which the To-Do tasks are retrieved (e.g., morning, afternoon, evening).</param>
        /// <returns>A list of To-Do tasks matching the given criteria.</returns>
        Task<List<ToDo>> GetToDosAsync(Guid userId, DateTime date, TimeBlock timeBlock);

        /// <summary>
        /// Adds a new To-Do task.
        /// </summary>
        /// <param name="toDo">The To-Do task to be added.</param>
        Task AddToDoAsync(ToDo toDo);

        /// <summary>
        /// Updates an existing To-Do task.
        /// </summary>
        /// <param name="toDo">The To-Do task to be updated.</param>
        Task UpdateToDoAsync(ToDo toDo);

        /// <summary>
        /// Updates multiple To-Do tasks in the repository.
        /// </summary>
        /// <param name="toDos">The collection of To-Do tasks to be updated.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// This method is used for batch updates of To-Do tasks. 
        /// Ensure that each task in the collection has a valid identifier and necessary updates before calling this method.
        /// </remarks>
        Task UpdateToDosAsync(IEnumerable<ToDo> toDos);

        /// <summary>
        /// Deletes a To-Do task by its unique identifier.
        /// </summary>
        /// <param name="toDoId">The unique identifier of the To-Do task to be deleted.</param>
        Task DeleteToDoAsync(Guid toDoId);

        /// <summary>
        /// Checks if a To-Do task with the specified unique identifier exists in the repository.
        /// </summary>
        /// <param name="toDoId">The unique identifier of the To-Do task to check.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains a boolean value indicating whether the To-Do task exists (`true`) or not (`false`).
        /// </returns>
        Task<bool> IsToDoExistsAsync(Guid toDoId);
    }
}
