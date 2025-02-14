using Core.Interfaces;
using Core.Models;
using Data.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repositories
{
    /// <summary>
    /// Repository for managing ToDo entities in the database.
    /// Implements the <see cref="IToDoRepository"/> interface.
    /// </summary>
    public class ToDoRepository : IToDoRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ToDoRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoRepository"/> class.
        /// </summary>
        /// <param name="context">The application's database context.</param>
        public ToDoRepository(AppDbContext context, ILogger<ToDoRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a ToDo item by its unique identifier.
        /// </summary>
        /// <param name="toDoId">The unique identifier of the ToDo item.</param>
        /// <returns>A task representing the asynchronous operation, containing the ToDo item if found; otherwise, <c>null</c>.</returns>
        public async Task<ToDo?> GetToDoByIdAsync(Guid toDoId)
        {
            _logger.LogInformation($"GetToDoByIdAsync({toDoId})");

            return await _context.ToDos.AsNoTracking().SingleOrDefaultAsync(t => t.ToDoId == toDoId);
        }

        /// <summary>
        /// Retrieves a list of ToDo items for a specific user, date, and time block.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="date">The date of the ToDo items.</param>
        /// <param name="timeBlock">The time block of the ToDo items.</param>
        /// <returns>A task representing the asynchronous operation, containing a list of ToDo items matching the specified criteria.</returns>
        public async Task<List<ToDo>> GetToDosAsync(Guid userId, DateTime date, TimeBlock timeBlock)
        {
            _logger.LogInformation($"GetToDosAsync({userId}, {date}, {timeBlock})");

            return await _context.ToDos.AsNoTracking().Where(t => t.UserId == userId && t.ToDoDate.Date == date.Date && t.TimeBlock == timeBlock).ToListAsync();
        }

        /// <summary>
        /// Adds a new ToDo item to the database.
        /// </summary>
        /// <param name="toDo">The ToDo item to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if a ToDo item with the same Id already exists.</exception>
        public async Task AddToDoAsync(ToDo toDo)
        {
            _logger.LogInformation($"AddToDoAsync({toDo.Description})");

            if (await ToDoExistsAsync(toDo.ToDoId))
            {
                _logger.LogWarning($"ToDo {toDo.Description} already exists.");
                throw new InvalidOperationException("Todo already exists.");
            }

            await _context.ToDos.AddAsync(toDo);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing ToDo item in the database.
        /// </summary>
        /// <param name="toDo">The updated ToDo item.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the ToDo item does not exist.</exception>
        public async Task UpdateToDoAsync(ToDo toDo)
        {
            _logger.LogInformation($"UpdateToDoAsync({toDo.Description})");

            var existingToDo = await _context.ToDos.SingleOrDefaultAsync(t => t.ToDoId == toDo.ToDoId);

            if (existingToDo == null)
            {
                _logger.LogWarning($"ToDo {toDo.Description} does not exists.");
                throw new InvalidOperationException("ToDo with such an Id does not exists.");
            }

            _context.Entry(existingToDo).CurrentValues.SetValues(toDo);

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a ToDo item by its unique identifier.
        /// </summary>
        /// <param name="toDoId">The unique identifier of the ToDo item to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the ToDo item does not exist.</exception>
        public async Task DeleteToDoAsync(Guid toDoId)
        {
            _logger.LogInformation($"DeleteToDoAsync({toDoId})");

            var existingToDo = _context.ToDos.FirstOrDefault(t => t.ToDoId == toDoId);

            if (existingToDo == null)
            {
                _logger.LogWarning($"ToDo with id {toDoId} does not exist.");
                throw new InvalidOperationException("Todo id does not exist.");
            }

            _context.ToDos.Remove(existingToDo);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Checks if a ToDo item with the specified Id exists in the database.
        /// </summary>
        /// <param name="toDoId">The unique identifier of the ToDo item.</param>
        /// <returns>A task representing the asynchronous operation, returning <c>true</c> if the ToDo item exists; otherwise, <c>false</c>.</returns>
        public async Task<bool> ToDoExistsAsync(Guid toDoId)
        {
            _logger.LogInformation($"ToDoExistsAsync({toDoId})");

            return await _context.ToDos.AsNoTracking().AnyAsync(t => t.ToDoId == toDoId);
        }
    }
}
