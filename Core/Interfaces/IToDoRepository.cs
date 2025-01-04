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
        Task<ToDo> GetToDoByIdAsync(int toDoId);

        /// <summary>
        /// Retrieves all todos associated with a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A list of todos associated with the user.</returns>
        Task<List<ToDo>> GetToDoByUserIdAsync(int userId);

        /// <summary>
        /// Retrieves all todos for a specific user on a given date.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="date">The date for which todos are retrieved.</param>
        /// <returns>A list of todos for the specified user on the given date.</returns>
        Task<List<ToDo>> GetToDoByDateAsync(int userId, DateTime date);

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
        Task DeleteToDoAsync(int toDoId);

        /// <summary>
        /// Moves all incomplete tasks for a specific user from one date to another.
        /// </summary>
        /// <param name="fromDate">The date from which incomplete todos are moved.</param>
        /// <param name="toDate">The date to which incomplete todos are moved.</param>
        /// <param name="userId">The unique identifier of the user whose todos are moved.</param>
        Task MoveIncompleteToDosAsync(DateTime fromDate, DateTime toDate, int userId);
    }
}
