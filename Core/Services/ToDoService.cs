using Core.Interfaces;
using Core.Models;

namespace Core.Services
{
    /// <summary>
    /// Service for managing To-Do tasks, providing functionalities for adding, updating, retrieving, and deleting tasks, as well as moving repeated tasks.
    /// </summary>
    public class ToDoService : IToDoService
    {
        private readonly IToDoRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoService"/> class.
        /// </summary>
        /// <param name="repository">The repository to interact with for data storage and retrieval.</param>
        public ToDoService(IToDoRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves a To-Do task by its unique identifier.
        /// </summary>
        /// <param name="toDoId">The unique identifier of the To-Do task.</param>
        /// <returns>The To-Do task corresponding to the provided identifier.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the To-Do task with the specified identifier does not exist.</exception>
        public async Task<ToDo?> GetToDoByIdAsync(Guid toDoId)
        {
            return await _repository.GetToDoByIdAsync(toDoId);
        }

        /// <summary>
        /// Retrieves a list of To-Do tasks for a specific user on a given date and time block.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="date">The date for which the To-Do tasks are retrieved.</param>
        /// <param name="timeBlock">The time block for which the To-Do tasks are retrieved (e.g., morning, afternoon, evening).</param>
        /// <returns>A list of To-Do tasks matching the given criteria.</returns>
        public async Task<List<ToDo>> GetToDosAsync(Guid userId, DateTime date, TimeBlock timeBlock)
        {
            return await _repository.GetToDosAsync(userId, date, timeBlock);
        }

        /// <summary>
        /// Adds a new To-Do task to the repository.
        /// </summary>
        /// <param name="toDo">The To-Do task to be added.</param>
        /// <exception cref="InvalidOperationException">Thrown when a To-Do task with the same ID already exists.</exception>
        public async Task AddToDoAsync(ToDo toDo)
        {
            if (await _repository.ToDoExistsAsync(toDo.ToDoId))
            {
                throw new InvalidOperationException("ToDo id already exists.");
            }

            await _repository.AddToDoAsync(toDo);
        }

        /// <summary>
        /// Updates an existing To-Do task in the repository.
        /// </summary>
        /// <param name="toDo">The To-Do task to be updated.</param>
        /// <exception cref="InvalidOperationException">Thrown when the To-Do task with the specified ID does not exist.</exception>
        public async Task UpdateToDoAsync(ToDo toDo)
        {
            if (!(await _repository.ToDoExistsAsync(toDo.ToDoId)))
            {
                throw new InvalidOperationException("Todo id does not exist.");
            }

            await _repository.UpdateToDoAsync(toDo);
        }

        /// <summary>
        /// Deletes a To-Do task from the repository by its unique identifier.
        /// </summary>
        /// <param name="toDoId">The unique identifier of the To-Do task to be deleted.</param>
        /// <exception cref="InvalidOperationException">Thrown when the To-Do task with the specified ID does not exist.</exception>
        public async Task DeleteToDoAsync(Guid toDoId)
        {
            await _repository.DeleteToDoAsync(toDoId);
        }

        /// <summary>
        /// Moves repeated To-Do tasks to the next appropriate date based on their repeat frequency.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose repeated To-Do tasks are to be moved.</param>
        /// <remarks>
        /// This method checks for To-Do tasks that are set to repeat (daily, weekly, monthly, yearly) and moves them to the next date based on their frequency.
        /// If no tasks are found for today, no action is taken.
        /// </remarks>
        public async Task MoveRepeatedToDosAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("User id cannot be empty.");
            }

            var todayToDos = await _repository.GetToDosAsync(userId, DateTime.Today, TimeBlock.Day);

            if (!todayToDos.Any())
            {
                return;
            }

            foreach (var todo in todayToDos)
            {
                todo.ToDoDate = todo.RepeatFrequency switch
                {
                    RepeatFrequency.Daily => todo.ToDoDate.AddDays(1),
                    RepeatFrequency.Weekly => todo.ToDoDate.AddDays(7),
                    RepeatFrequency.Monthly => todo.ToDoDate.AddMonths(1),
                    RepeatFrequency.Yearly => todo.ToDoDate.AddYears(1),
                    _ => todo.ToDoDate
                };

                await _repository.UpdateToDoAsync(todo);
            }

        }
    }
}
