using Core.Models;

namespace Core.Interfaces
{
    /// <summary>
    /// Provides methods for managing todos, including CRUD operations and task movement.
    /// </summary>
    public interface IToDoService
    {
        /// <summary>
        /// Retrieves a todo item by its unique identifier.
        /// </summary>
        /// <param name="toDoId">The unique identifier of the todo item.</param>
        /// <returns>The todo item if found; otherwise, <c>null</c>.</returns>
        Task<ToDo?> GetToDoByIdAsync(Guid toDoId);

        /// <summary>
        /// Retrieves a list of todos for a specific user on a given date and time block.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="date">The date for which todos should be retrieved.</param>
        /// <param name="timeBlock">The time block associated with the todos.</param>
        /// <returns>A list of todos scheduled for the specified user on the given date and time block.</returns>
        Task<List<ToDo>> GetToDosAsync(Guid userId, DateTime date, TimeBlock timeBlock);

        /// <summary>
        /// Adds a new todo item to the system.
        /// </summary>
        /// <param name="toDo">The todo item to be added.</param>
        Task AddToDoAsync(ToDo toDo);

        /// <summary>
        /// Updates the details of an existing todo item.
        /// </summary>
        /// <param name="toDo">The todo item with updated information.</param>
        Task UpdateToDoAsync(ToDo toDo);

        /// <summary>
        /// Deletes a todo item from the system by its unique identifier.
        /// </summary>
        /// <param name="toDoId">The unique identifier of the todo item to be deleted.</param>
        Task DeleteToDoAsync(Guid toDoId);

        /// <summary>
        /// Moves incomplete todos from one date to another for a specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose incomplete todos should be moved.</param>
        Task MoveRepeatedToDosAsync(Guid userId);
    }
}
