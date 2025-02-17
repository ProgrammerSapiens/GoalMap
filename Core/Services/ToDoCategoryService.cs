using Core.Interfaces;
using Core.Models;
using Microsoft.Extensions.Logging;

namespace Core.Services
{
    /// <summary>
    /// Provides functionality for managing user ToDo categories.
    /// </summary>
    public class ToDoCategoryService : IToDoCategoryService
    {
        private readonly IToDoCategoryRepository _repository;
        private readonly ILogger<ToDoCategoryService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoCategoryService"/> class.
        /// </summary>
        /// <param name="repository">Repository for managing ToDo categories.</param>
        public ToDoCategoryService(IToDoCategoryRepository repository, ILogger<ToDoCategoryService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a ToDo category by its unique identifier.
        /// </summary>
        /// <param name="toDoCategoryId">The unique identifier of the ToDo category.</param>
        /// <returns>The requested ToDo category, or null if not found.</returns>
        public async Task<ToDoCategory?> GetToDoCategoryByCategoryIdAsync(Guid toDoCategoryId)
        {
            _logger.LogInformation($"GetToDoCategoryByCategoryIdAsync({toDoCategoryId})");

            return await _repository.GetToDoCategoryByCategoryIdAsync(toDoCategoryId);
        }

        /// <summary>
        /// Retrieves all ToDo categories associated with a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A list of the user's ToDo categories.</returns>
        public async Task<List<ToDoCategory>> GetToDoCategoriesByUserIdAsync(Guid userId)
        {
            _logger.LogInformation($"GetToDoCategoriesByUserIdAsync({userId})");

            return await _repository.GetToDoCategoriesByUserIdAsync(userId);
        }

        /// <summary>
        /// Adds a new ToDo category for a user.
        /// </summary>
        /// <param name="toDoCategory">The category to be added.</param>
        /// <exception cref="InvalidOperationException">Thrown if a category with the same name already exists.</exception>
        /// <exception cref="ArgumentException">Thrown if the category name contains digits.</exception>
        public async Task AddToDoCategoryAsync(ToDoCategory toDoCategory)
        {
            _logger.LogInformation($"AddToDoCategoryAsync(ToDoCategory {toDoCategory.ToDoCategoryName})");

            toDoCategory.ToDoCategoryName = GetNameWithACapitalLetterAndValidate(toDoCategory.ToDoCategoryName);

            if (await _repository.CategoryExistsByNameAsync(toDoCategory.UserId, toDoCategory.ToDoCategoryName))
            {
                _logger.LogWarning("Category with such name already exists.");
                throw new InvalidOperationException("Category with such name already exists.");
            }

            await _repository.AddToDoCategoryAsync(toDoCategory);
        }

        /// <summary>
        /// Updates an existing ToDo category.
        /// </summary>
        /// <param name="toDoCategory">The updated ToDo category object.</param>
        /// <exception cref="InvalidOperationException">Thrown if the category does not exist.</exception>
        /// <exception cref="ArgumentException">Thrown if attempting to update a default category.</exception>
        public async Task UpdateToDoCategoryAsync(ToDoCategory toDoCategory)
        {
            _logger.LogInformation($"UpdateToDoCategoryAsync(ToDoCategory {toDoCategory.ToDoCategoryName})");

            var existingToDoCategory = await _repository.GetToDoCategoryByCategoryIdAsync(toDoCategory.ToDoCategoryId);
            if (existingToDoCategory == null)
            {
                _logger.LogWarning("Category was not found.");
                throw new InvalidOperationException("Category was not found.");
            }

            toDoCategory.ToDoCategoryName = GetNameWithACapitalLetterAndValidate(toDoCategory.ToDoCategoryName);

            if (await _repository.CategoryExistsByNameAsync(toDoCategory.UserId, toDoCategory.ToDoCategoryName))
            {
                _logger.LogWarning("Category with such name already exists.");
                throw new InvalidOperationException("Category with such name already exists.");
            }

            await _repository.UpdateToDoCategoryAsync(toDoCategory);
            await _repository.UpdateCategoryInToDosAsync(toDoCategory.UserId, existingToDoCategory.ToDoCategoryId, toDoCategory.ToDoCategoryId);
        }

        /// <summary>
        /// Deletes a ToDo category and reassigns its tasks to the "Other" category.
        /// </summary>
        /// <param name="toDoCategoryId">The unique identifier of the ToDo category to delete.</param>
        /// <exception cref="InvalidOperationException">Thrown if the category does not exist.</exception>
        /// <exception cref="ArgumentException">Thrown if attempting to delete a protected category.</exception>
        public async Task DeleteToDoCategoryAsync(Guid toDoCategoryId)
        {
            _logger.LogInformation($"DeleteToDoCategoryAsync({toDoCategoryId})");

            var existingToDoCategory = await _repository.GetToDoCategoryByCategoryIdAsync(toDoCategoryId);
            if (existingToDoCategory == null)
            {
                _logger.LogWarning("ToDo category was not found.");
                throw new InvalidOperationException("ToDo category was not found.");
            }

            if (existingToDoCategory.ToDoCategoryName == "Habbit" || existingToDoCategory.ToDoCategoryName == "Other")
            {
                _logger.LogWarning("You cannot delete this category.");
                throw new ArgumentException("You cannot delete this category.");
            }

            var otherToDoCategory = await _repository.GetToDoCategoryByCategoryNameAsync("Other");
            if (otherToDoCategory == null)
            {
                _logger.LogError("Default todo category was not found.");
                throw new InvalidOperationException("ToDo category was not found.");
            }

            await _repository.DeleteToDoCategoryAsync(toDoCategoryId);
            await _repository.UpdateCategoryInToDosAsync(existingToDoCategory.UserId, existingToDoCategory.ToDoCategoryId, otherToDoCategory.ToDoCategoryId);
        }

        /// <summary>
        /// Capitalizes the first letter of a category name and converts the rest to lowercase.
        /// </summary>
        /// <param name="categoryName">The category name to format.</param>
        /// <returns>The formatted category name.</returns>
        private string GetNameWithACapitalLetterAndValidate(string categoryName)
        {
            string validatedCategoryName = char.ToUpper(categoryName[0]) + categoryName.Substring(1).ToLower();

            if (validatedCategoryName.Any(char.IsDigit))
            {
                _logger.LogWarning("Category name cannot contain digits.");
                throw new ArgumentException("Category name cannot contain digits.");
            }

            if (validatedCategoryName == "Habbit" || validatedCategoryName == "Other")
            {
                _logger.LogWarning("You cannot add/update this category.");
                throw new ArgumentException("You cannot add/update this category.");
            }

            return validatedCategoryName;
        }
    }
}
