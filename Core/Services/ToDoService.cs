using Core.Interfaces;
using Core.Models;
using Microsoft.Extensions.Logging;

namespace Core.Services
{
    /// <summary>
    /// Provides functionality for managing To-Do tasks, including retrieval, creation, updating, deletion, and handling repeated tasks.
    /// </summary>
    public class ToDoService : IToDoService
    {
        private readonly IToDoRepository _repository;
        private readonly ILogger<ToDoService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoService"/> class with the specified repository.
        /// </summary>
        /// <param name="repository">The repository for To-Do data management.</param>
        public ToDoService(IToDoRepository repository, ILogger<ToDoService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a To-Do task by its unique identifier.
        /// </summary>
        /// <param name="toDoId">The unique identifier of the To-Do task.</param>
        /// <returns>The To-Do task if found; otherwise, null.</returns>
        public async Task<ToDo?> GetToDoByIdAsync(Guid toDoId)
        {
            _logger.LogInformation($"GetToDoByIdAsync({toDoId})");

            return await _repository.GetToDoByIdAsync(toDoId);
        }

        /// <summary>
        /// Retrieves a list of To-Do tasks for a specific user on a given date and time block.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="date">The date for which tasks are retrieved.</param>
        /// <param name="timeBlock">The time block of the day (e.g., morning, afternoon, evening).</param>
        /// <returns>A list of To-Do tasks matching the specified criteria.</returns>
        public async Task<List<ToDo>> GetToDosAsync(Guid userId, DateTime date, TimeBlock timeBlock)
        {
            _logger.LogInformation($"GetToDosAsync({userId}, {date}, {timeBlock})");

            return await _repository.GetToDosAsync(userId, date, timeBlock);
        }

        /// <summary>
        /// Adds a new To-Do task to the repository.
        /// </summary>
        /// <param name="toDo">The To-Do task to add.</param>
        /// <exception cref="InvalidOperationException">Thrown if a task with the same ID already exists.</exception>
        public async Task AddToDoAsync(ToDo toDo)
        {
            _logger.LogInformation($"AddToDoAsync(ToDo {toDo.Description})");

            if (await _repository.ToDoExistsAsync(toDo.ToDoId))
            {
                _logger.LogWarning("ToDo id already exists.");
                throw new InvalidOperationException("ToDo id already exists.");
            }

            await _repository.AddToDoAsync(toDo);
        }

        /// <summary>
        /// Updates an existing To-Do task.
        /// </summary>
        /// <param name="toDo">The To-Do task with updated details.</param>
        /// <exception cref="InvalidOperationException">Thrown if the task does not exist.</exception>
        public async Task UpdateToDoAsync(ToDo toDo)
        {
            _logger.LogInformation($"UpdateToDoAsync(ToDo {toDo.Description})");

            var existingToDo = await _repository.GetToDoByIdAsync(toDo.ToDoId);
            if (existingToDo == null)
            {
                _logger.LogWarning("ToDo id does not exist.");
                throw new InvalidOperationException("Todo id does not exist.");
            }

            if (existingToDo.CompletionStatus == true)
            {
                _logger.LogWarning("Todo is already completed");
                throw new InvalidOperationException("You cannot update completed todo");
            }

            if (toDo.CompletionStatus == true)
            {
                var experience = (int)toDo.Difficulty;
                await _repository.UpdateUserExperienceAsync(toDo.UserId, experience);
            }

            await _repository.UpdateToDoAsync(toDo);
        }

        /// <summary>
        /// Deletes a To-Do task by its unique identifier.
        /// </summary>
        /// <param name="toDoId">The unique identifier of the task to delete.</param>
        public async Task DeleteToDoAsync(Guid toDoId)
        {
            _logger.LogInformation($"DeleteToDoAsync({toDoId})");

            await _repository.DeleteToDoAsync(toDoId);
        }

        /// <summary>
        /// Moves repeated To-Do tasks to the next scheduled date based on their recurrence pattern.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose repeated tasks are updated.</param>
        /// <exception cref="ArgumentException">Thrown if the user ID is empty.</exception>
        public async Task MoveRepeatedToDosAsync(Guid userId)
        {
            _logger.LogInformation($"MoveRepeatedToDosAsync({userId})");

            var todayToDos = await _repository.GetRepeatedToDosAsync(userId);
            if (!todayToDos.Any())
            {
                _logger.LogInformation("There are no todos that repeated");
                return;
            }

            foreach (var todo in todayToDos)
            {
                if (todo.ToDoDate <= DateTime.Today)
                {
                    var nextDate = todo.RepeatFrequency switch
                    {
                        RepeatFrequency.Daily => todo.ToDoDate.AddDays(1),
                        RepeatFrequency.Weekly => todo.ToDoDate.AddDays(7),
                        RepeatFrequency.Monthly => todo.ToDoDate.AddMonths(1),
                        RepeatFrequency.Yearly => todo.ToDoDate.AddYears(1),
                        _ => todo.ToDoDate
                    };

                    var newToDo = new ToDo(todo.Description, todo.TimeBlock, todo.Difficulty, nextDate, todo.ToDoCategoryId, userId, Guid.NewGuid(), todo.Deadline, todo.ParentToDoId, todo.RepeatFrequency);
                    await _repository.AddToDoAsync(newToDo);
                    _logger.LogInformation($"Created new ToDo {newToDo.Description} for user {userId} on {nextDate}");

                    todo.Moved = true;
                    await _repository.UpdateToDoAsync(todo);
                    _logger.LogInformation($"Marked original ToDo as moved.");
                }
            }
        }
    }
}
