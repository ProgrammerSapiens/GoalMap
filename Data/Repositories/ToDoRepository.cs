using Core.Interfaces;
using Core.Models;
using Data.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    /// <summary>
    /// Repository for managing ToDo entities in the database.
    /// Implements the <see cref="IToDoRepository"/> interface.
    /// </summary>
    public class ToDoRepository : IToDoRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoRepository"/> class.
        /// </summary>
        /// <param name="context">The application's database context.</param>
        public ToDoRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a ToDo item by its unique identifier.
        /// </summary>
        /// <param name="toDoId">The unique identifier of the ToDo item.</param>
        /// <returns>The ToDo item if found; otherwise, <c>null</c>.</returns>
        public async Task<ToDo?> GetToDoByIdAsync(Guid toDoId)
        {
            return await _context.ToDos.FirstOrDefaultAsync(t => t.ToDoId == toDoId);
        }

        /// <summary>
        /// Retrieves a list of ToDo items for a specific user, date, and time block.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="date">The date of the ToDo items.</param>
        /// <param name="timeBlock">The time block of the ToDo items.</param>
        /// <returns>A list of ToDo items matching the specified criteria.</returns>
        public async Task<List<ToDo>> GetToDosAsync(Guid userId, DateTime date, TimeBlock timeBlock)
        {
            return await _context.ToDos.Where(t => t.UserId == userId && t.ToDoDate.Date == date.Date && t.TimeBlock == timeBlock).ToListAsync();
        }

        /// <summary>
        /// Adds a new ToDo item to the database.
        /// </summary>
        /// <param name="toDo">The ToDo item to add.</param>
        /// <exception cref="InvalidOperationException">Thrown if a ToDo item with the same Id already exists.</exception>
        public async Task AddToDoAsync(ToDo toDo)
        {
            if (await ToDoExistsAsync(toDo.ToDoId))
            {
                throw new InvalidOperationException("ToDo with such an Id already exists.");
            }

            await _context.ToDos.AddAsync(toDo);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing ToDo item in the database.
        /// </summary>
        /// <param name="toDo">The updated ToDo item.</param>
        /// <exception cref="InvalidOperationException">Thrown if the ToDo item does not exist.</exception>
        public async Task UpdateToDoAsync(ToDo toDo)
        {
            var existingToDo = await _context.ToDos.SingleOrDefaultAsync(t => t.ToDoId == toDo.ToDoId);

            if (existingToDo == null)
            {
                throw new InvalidOperationException("ToDo with such an Id does not exists.");
            }

            existingToDo.Description = toDo.Description;
            existingToDo.Difficulty = toDo.Difficulty;
            existingToDo.Deadline = toDo.Deadline;
            existingToDo.ToDoDate = toDo.ToDoDate;
            existingToDo.CompletionStatus = toDo.CompletionStatus;
            existingToDo.RepeatFrequency = toDo.RepeatFrequency;
            existingToDo.ToDoCategoryName = toDo.ToDoCategoryName;
            existingToDo.UserId = toDo.UserId;

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a ToDo item by its unique identifier.
        /// </summary>
        /// <param name="toDoId">The unique identifier of the ToDo item to delete.</param>
        public async Task DeleteToDoAsync(Guid toDoId)
        {
            var existingToDo = _context.ToDos.FirstOrDefault(t => t.ToDoId == toDoId);

            if (existingToDo != null)
            {
                _context.ToDos.Remove(existingToDo);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Checks if a ToDo item with the specified Id exists in the database.
        /// </summary>
        /// <param name="toDoId">The unique identifier of the ToDo item.</param>
        /// <returns><c>true</c> if the ToDo item exists; otherwise, <c>false</c>.</returns>
        public async Task<bool> ToDoExistsAsync(Guid toDoId)
        {
            return await _context.ToDos.AnyAsync(t => t.ToDoId == toDoId);
        }
    }
}
