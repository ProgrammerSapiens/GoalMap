using Core.Interfaces;
using Core.Models;
using Data.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repositories
{
    /// <summary>
    /// Repository for interacting with ToDoCategory entities in the database.
    /// Provides methods for adding, updating, deleting, and retrieving to-do categories.
    /// </summary>
    public class ToDoCategoryRepository : IToDoCategoryRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ToDoCategoryRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the repository with a given database context.
        /// </summary>
        /// <param name="context">The database context used to interact with the data store.</param>
        public ToDoCategoryRepository(AppDbContext context, ILogger<ToDoCategoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a ToDoCategory by its unique identifier.
        /// </summary>
        /// <param name="toDoCategoryId">The unique identifier of the to-do category.</param>
        /// <returns>A ToDoCategory object if found, otherwise <c>null</c>.</returns>
        public async Task<ToDoCategory?> GetToDoCategoryByCategoryIdAsync(Guid toDoCategoryId)
        {
            _logger.LogInformation($"GetToDoCategoryByCategoryIdAsync({toDoCategoryId})");

            return await _context.ToDoCategories.AsNoTracking().SingleOrDefaultAsync(c => c.ToDoCategoryId == toDoCategoryId);
        }

        /// <summary>
        /// Retrieves a ToDoCategory by its name.
        /// </summary>
        /// <param name="toDoCategoryName">The name of the to-do category.</param>
        /// <returns>A ToDoCategory object if found, otherwise <c>null</c>.</returns>
        public async Task<ToDoCategory?> GetToDoCategoryByCategoryNameAsync(string toDoCategoryName)
        {
            _logger.LogInformation($"GetToDoCategoryByCategoryNameAsync({toDoCategoryName})");

            return await _context.ToDoCategories.AsNoTracking().SingleOrDefaultAsync(c => c.ToDoCategoryName == toDoCategoryName);
        }

        /// <summary>
        /// Retrieves a list of ToDoCategories associated with a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of ToDoCategory objects associated with the given user ID.</returns>
        public async Task<List<ToDoCategory>> GetToDoCategoriesByUserIdAsync(Guid userId)
        {
            _logger.LogInformation($"GetToDoCategoriesByUserIdAsync({userId})");

            return await _context.ToDoCategories.AsNoTracking().Where(c => c.UserId == userId).ToListAsync();
        }

        /// <summary>
        /// Adds a new ToDoCategory to the database.
        /// </summary>
        /// <param name="toDoCategory">The ToDoCategory object to add.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if a category with the same name already exists for the user.
        /// </exception>
        public async Task AddToDoCategoryAsync(ToDoCategory toDoCategory)
        {
            _logger.LogInformation($"AddToDoCategoryAsync(ToDoCategory {toDoCategory.ToDoCategoryName})");

            if (await CategoryExistsByNameAsync(toDoCategory.UserId, toDoCategory.ToDoCategoryName))
            {
                _logger.LogWarning($"Todo category {toDoCategory.ToDoCategoryName} already exists.");
                throw new InvalidOperationException("Todo category already exists.");
            }

            await _context.ToDoCategories.AddAsync(toDoCategory);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing ToDoCategory in the database.
        /// </summary>
        /// <param name="toDoCategory">The ToDoCategory object with updated information.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the category does not exist.
        /// </exception>
        public async Task UpdateToDoCategoryAsync(ToDoCategory toDoCategory)
        {
            _logger.LogInformation($"UpdateToDoCategoryAsync(ToDoCategory {toDoCategory.ToDoCategoryName})");

            var existingToDoCategory = await _context.ToDoCategories.FirstOrDefaultAsync(c => c.ToDoCategoryId == toDoCategory.ToDoCategoryId);

            if (existingToDoCategory == null)
            {
                _logger.LogWarning($"Category {toDoCategory.ToDoCategoryName} does not exist.");
                throw new InvalidOperationException("Category does not exist.");
            }

            existingToDoCategory.ToDoCategoryName = toDoCategory.ToDoCategoryName;

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates the category name in all associated ToDo items for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="oldCategoryName">The old category name.</param>
        /// <param name="newCategoryName">The new category name.</param>
        public async Task UpdateCategoryInToDosAsync(Guid userId, Guid oldCategoryId, Guid newCategoryId)
        {
            _logger.LogInformation($"UpdateCategoryInToDosAsync({userId}, {oldCategoryId}, {newCategoryId})");

            var toDosToUpdate = await _context.ToDos.Where(t => t.UserId == userId && t.ToDoCategoryId == oldCategoryId).ToListAsync();

            foreach (var toDo in toDosToUpdate)
            {
                toDo.ToDoCategoryId = newCategoryId;
            }

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a ToDoCategory from the database by its unique identifier.
        /// </summary>
        /// <param name="toDoCategoryId">The unique identifier of the ToDoCategory to delete.</param>
        /// <exception cref="InvalidOperationException">Thrown if the category does not exist.</exception>
        public async Task DeleteToDoCategoryAsync(Guid toDoCategoryId)
        {
            _logger.LogInformation($"DeleteToDoCategoryAsync({toDoCategoryId})");

            var toDoCategory = await _context.ToDoCategories.SingleOrDefaultAsync(c => c.ToDoCategoryId == toDoCategoryId);

            if (toDoCategory == null)
            {
                _logger.LogWarning($"Category {toDoCategoryId} does not exist.");
                throw new InvalidOperationException("Category does not exist.");
            }

            _context.ToDoCategories.Remove(toDoCategory);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Checks if a ToDoCategory with the specified name already exists for the given user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="toDoCategoryName">The name of the ToDoCategory.</param>
        /// <returns><c>true</c> if the category exists, otherwise <c>false</c>.</returns>
        public async Task<bool> CategoryExistsByNameAsync(Guid userId, string toDoCategoryName)
        {
            _logger.LogInformation($"CategoryExistsByNameAsync({userId}, {toDoCategoryName})");

            return await _context.ToDoCategories.AsNoTracking().AnyAsync(c => c.ToDoCategoryName == toDoCategoryName && c.UserId == userId);
        }
    }
}
