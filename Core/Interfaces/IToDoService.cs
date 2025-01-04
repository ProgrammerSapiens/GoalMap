using Core.Models;

namespace Core.Interfaces
{
    /// <summary>
    /// Provides methods for managing todos, including CRUD operations and task movement.
    /// </summary>
    public interface IToDoService
    {
        /// <summary>
        /// Retrieves a todo by its unique identifier.
        /// </summary>
        /// <param name="toDoId">The unique identifier of the todo.</param>
        /// <returns>The todo with the specified identifier.</returns>
        Task<ToDo> GetToDoByIdAsync(Guid toDoId);

        /// <summary>
        /// Retrieves a list of todos associated with a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A list of todos assigned to the specified user.</returns>
        Task<List<ToDo>> GetToDosByUserAsync(Guid userId);

        /// <summary>
        /// Retrieves a list of todos for a specific user and date.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="date">The date for which todos are to be retrieved.</param>
        /// <returns>A list of todos scheduled for the specified user on the given date.</returns>
        Task<List<ToDo>> GetToDosByDateAsync(Guid userId, DateTime date);

        /// <summary>
        /// Retrieves a list of todos for a specific user and date.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="timeBlock">The time block to which the todo belongs.</param>
        /// <returns>A list of todos scheduled for the specified user on the given date.</returns>
        Task<List<ToDo>> GetToDosByTimeBlockAsync(Guid userId, TimeBlock timeBlock);

        /// <summary>
        /// Adds a new todo to the system.
        /// </summary>
        /// <param name="toDo">The todo to be added.</param>
        Task AddToDoAsync(ToDo toDo);

        /// <summary>
        /// Updates the details of an existing todo.
        /// </summary>
        /// <param name="toDo">The todo with updated information.</param>
        Task UpdateToDoAsync(ToDo toDo);

        /// <summary>
        /// Deletes a todo from the system by its unique identifier.
        /// </summary>
        /// <param name="toDoId">The unique identifier of the todo to be deleted.</param>
        Task DeleteToDoAsync(Guid toDoId);

        /// <summary>
        /// Moves incomplete todos from one date to another for a specified user.
        /// </summary>
        /// <param name="fromDate">The date from which todos will be moved.</param>
        /// <param name="toDate">The date to which todos will be moved.</param>
        /// <param name="userId">The unique identifier of the user whose todos are being moved.</param>
        Task MoveIncompleteToDosAsync(DateTime fromDate, DateTime toDate, Guid userId);

        /// <summary>
        /// Moves incomplete todos from one date to another for a specified user.
        /// </summary>
        /// <param name="repeatFrequency">The repeat frequency of the todo.</param>
        /// <param name="userId">The unique identifier of the user whose todos are being moved.</param>
        Task MoveRepeatedToDosAsync(RepeatFrequency repeatFrequency, Guid userId);
    }
}
