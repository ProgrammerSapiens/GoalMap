using Core.Models;

namespace Core.Interfaces
{
    /// <summary>
    /// Defines a contract for managing To-Do tasks, including CRUD operations and additional functionalities like checking task existence.
    /// </summary>
    public interface IToDoRepository
    {
        /// <summary>
        /// Retrieves a To-Do task by its unique identifier.
        /// </summary>
        /// <param name="toDoId">The unique identifier of the To-Do task.</param>
        /// <returns>The To-Do task corresponding to the provided identifier, or <c>null</c> if not found.</returns>
        Task<ToDo?> GetToDoByIdAsync(Guid toDoId);

        /// <summary>
        /// Retrieves a list of To-Do tasks for a specific user on a given date and time block.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="date">The date for which the To-Do tasks should be retrieved.</param>
        /// <param name="timeBlock">The time block associated with the To-Do tasks (e.g., morning, afternoon, evening).</param>
        /// <returns>A list of To-Do tasks that match the specified criteria.</returns>
        Task<List<ToDo>> GetToDosAsync(Guid userId, DateTime date, TimeBlock timeBlock);

        /// <summary>
        /// Retrieves a list of repeated ToDo items for a specific user. 
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A task representing the asynchronous operation, containing a list of repeated ToDo items.</returns>
        Task<List<ToDo>> GetRepeatedToDosAsync(Guid userId);

        /// <summary>
        /// Adds a new To-Do task to the repository.
        /// </summary>
        /// <param name="toDo">The To-Do task to be added.</param>
        Task AddToDoAsync(ToDo toDo);

        /// <summary>
        /// Updates an existing To-Do task in the repository.
        /// </summary>
        /// <param name="toDo">The To-Do task with updated information.</param>
        Task UpdateToDoAsync(ToDo toDo);

        /// <summary>
        /// Updates the experience points of a user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="experience">The amount of experience to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateUserExperienceAsync(Guid userId, int experience);

        /// <summary>
        /// Deletes a To-Do task from the repository by its unique identifier.
        /// </summary>
        /// <param name="toDoId">The unique identifier of the To-Do task to be deleted.</param>
        Task DeleteToDoAsync(Guid toDoId);

        /// <summary>
        /// Checks whether a To-Do task with the specified unique identifier exists in the repository.
        /// </summary>
        /// <param name="toDoId">The unique identifier of the To-Do task to check.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains a boolean indicating whether the To-Do task exists (<c>true</c>) or not (<c>false</c>).
        /// </returns>
        Task<bool> ToDoExistsAsync(Guid toDoId);
    }
}
