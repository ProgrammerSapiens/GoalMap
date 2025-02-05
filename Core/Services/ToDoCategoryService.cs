using Core.Interfaces;
using Core.Models;

namespace Core.Services
{
    /// <summary>
    /// Service for managing user ToDo categories.
    /// Provides methods for adding, retrieving, updating, and deleting categories.
    /// </summary>
    public class ToDoCategoryService : IToDoCategoryService
    {
        private readonly IToDoCategoryRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoCategoryService"/> class.
        /// </summary>
        /// <param name="repository">The repository for working with ToDo categories.</param>
        public ToDoCategoryService(IToDoCategoryRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves a ToDo category by its name and user Id.
        /// </summary>
        /// <param name="toDoCategoryName">The name of the ToDo category.</param>
        /// <param name="userId">The user Id.</param>
        /// <returns>The ToDo category object.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the category does not exist.</exception>
        public async Task<ToDoCategory?> GetToDoCategoryByCategoryIdAsync(Guid userId, Guid toDoCategoryId)
        {
            return await _repository.GetToDoCategoryByCategoryIdAsync(userId, toDoCategoryId);
        }

        /// <summary>
        /// Retrieves all ToDo categories for the specified user.
        /// </summary>
        /// <param name="userId">The user Id.</param>
        /// <returns>A list of ToDo categories.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the user is not found or has no categories.</exception>
        public async Task<List<ToDoCategory>> GetToDoCategoriesByUserIdAsync(Guid userId)
        {
            return await _repository.GetToDoCategoriesByUserIdAsync(userId);
        }

        /// <summary>
        /// Adds a new ToDo category.
        /// </summary>
        /// <param name="toDoCategory">The ToDo category object.</param>
        /// <exception cref="InvalidOperationException">Thrown if a category with the same name already exists.</exception>
        public async Task AddToDoCategoryAsync(ToDoCategory toDoCategory)
        {
            toDoCategory.ToDoCategoryName = GetNameWithACapitalLetter(toDoCategory.ToDoCategoryName);

            if (await _repository.CategoryExistsByNameAsync(toDoCategory.UserId, toDoCategory.ToDoCategoryName))
            {
                throw new InvalidOperationException("Category with such name already exists.");
            }

            if (toDoCategory.ToDoCategoryName.Any(char.IsDigit))
            {
                throw new ArgumentException("Category name cannot contain digits.");
            }

            await _repository.AddToDoCategoryAsync(toDoCategory);
        }

        /// <summary>
        /// Updates an existing ToDo category.
        /// </summary>
        /// <param name="toDoCategory">The updated ToDo category object.</param>
        /// <exception cref="InvalidOperationException">Thrown if the category does not exist.</exception>
        public async Task UpdateToDoCategoryAsync(ToDoCategory toDoCategory)
        {
            var existingToDoCategory = await _repository.GetToDoCategoryByCategoryIdAsync(toDoCategory.UserId, toDoCategory.ToDoCategoryId);

            if (existingToDoCategory == null)
            {
                throw new InvalidOperationException("Category was not found.");
            }

            toDoCategory.ToDoCategoryName = GetNameWithACapitalLetter(toDoCategory.ToDoCategoryName);
            var oldCategoryName = existingToDoCategory.ToDoCategoryName;
            var newCategoryName = toDoCategory.ToDoCategoryName;

            if (await _repository.CategoryExistsByNameAsync(toDoCategory.UserId, newCategoryName))
            {
                throw new InvalidOperationException("Category with such name already exists.");
            }

            if (oldCategoryName == "Habbit" || oldCategoryName == "Other")
            {
                throw new ArgumentException("You cannot update this category.");
            }

            await _repository.UpdateToDoCategoryAsync(toDoCategory);
            await _repository.UpdateCategoryInToDosAsync(toDoCategory.UserId, oldCategoryName, newCategoryName);
        }

        /// <summary>
        /// Deletes a ToDo category by its name and user Id.
        /// </summary>
        /// <param name="toDoCategoryName">The name of the ToDo category.</param>
        /// <param name="userId">The user Id.</param>
        /// <exception cref="InvalidOperationException">Thrown if the category does not exist.</exception>
        public async Task DeleteToDoCategoryAsync(Guid userId, Guid toDoCategoryId)
        {
            var existingToDoCategory = await _repository.GetToDoCategoryByCategoryIdAsync(userId, toDoCategoryId);

            if (existingToDoCategory == null)
            {
                throw new InvalidOperationException("ToDo category was not found.");
            }

            var oldCategoryName = existingToDoCategory.ToDoCategoryName;
            var newCategoryName = "Other";

            if (oldCategoryName == "Habbit" || oldCategoryName == "Other")
            {
                throw new ArgumentException("You cannot delete this category.");
            }

            await _repository.DeleteToDoCategoryAsync(userId, toDoCategoryId);
            await _repository.UpdateCategoryInToDosAsync(userId, oldCategoryName, newCategoryName);
        }

        private string GetNameWithACapitalLetter(string categoryName)
        {
            return char.ToUpper(categoryName[0]) + categoryName.Substring(1).ToLower();
        }
    }
}
