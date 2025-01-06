using Core.Models;

namespace Core.Interfaces
{
    /// <summary>
    /// Provides methods for managing todos, including CRUD operations.
    /// </summary>
    internal interface IToDoRepository
    {
        /// <summary>
        /// Retrieves a todo by its unique identifier.
        /// </summary>
        /// <param name="toDoId">The unique identifier of the todo.</param>
        /// <returns>The todo with the specified identifier.</returns>
        Task<ToDo?> GetToDoByIdAsync(Guid toDoId);

        /// <summary>
        /// Retrieves all todos associated with a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A list of todos associated with the user.</returns>
        Task<List<ToDo>> GetToDosByUserIdAsync(Guid userId);

        /// <summary>
        /// Retrieves all todos for a specific user on a given date.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="date">The date for which todos are retrieved.</param>
        /// <returns>A list of todos for the specified user on the given date.</returns>
        Task<List<ToDo>> GetToDosByDateAsync(Guid userId, DateTime date);

        /// <summary>
        /// Retrieves all todos for a specific user by the given time block.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="timeBlock">The time block for which todos are retrieved.</param>
        /// <returns>A list of todos for the specified user by the given time block</returns>
        Task<List<ToDo>> GetToDosByTimeBlockAsync(Guid userId, TimeBlock timeBlock);

        /// <summary>
        /// Adds a new to to the repository.
        /// </summary>
        /// <param name="toDo">The todo to be added.</param>
        Task AddToDoAsync(ToDo toDo);

        /// <summary>
        /// Updates an existing todo in the repository.
        /// </summary>
        /// <param name="toDo">The todo to be updated.</param>
        Task UpdateToDoAsync(ToDo toDo);

        /// <summary>
        /// Deletes a todo from the repository by its unique identifier.
        /// </summary>
        /// <param name="toDoId">The unique identifier of the todo to delete.</param>
        Task DeleteToDoAsync(Guid toDoId);

        /// <summary>
        /// Moves all repeated todos for a specific user to a new time period based on the specified repetition frequency.
        /// </summary>
        /// <param name="repeatFrequency">The frequency with which the todos are repeated (e.g., Daily, Weekly, etc.). This determines how the todos will be moved.</param>
        /// <param name="userId">The unique identifier of the user whose repeated todos are to be moved.</param>
        /// <returns>A task that represets the asynchronous operation of moving the todos.</returns>
        Task MoveRepeatedToDosAsync(RepeatFrequency repeatFrequency, Guid userId);
    }
}
