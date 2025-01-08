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
        /// Retrieves a list of todos for a specific user and date.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="date">The date for which todos are to be retrieved.</param>
        /// <returns>A list of todos scheduled for the specified user on the given date.</returns>
        Task<List<ToDo>> GetToDosAsync(Guid userId, DateTime date, TimeBlock timeBlock);

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
        /// <param name="userId">The unique identifier of the user whose todos are being moved.</param>
        Task MoveRepeatedToDosAsync(Guid userId);
    }
}
