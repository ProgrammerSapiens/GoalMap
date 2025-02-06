using Core.Interfaces;
using Core.Models;

namespace Core.Services
{
    /// <summary>
    /// Provides functionality for managing user ToDo categories.
    /// </summary>
    public class ToDoCategoryService : IToDoCategoryService
    {
        private readonly IToDoCategoryRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoCategoryService"/> class.
        /// </summary>
        /// <param name="repository">Repository for managing ToDo categories.</param>
        public ToDoCategoryService(IToDoCategoryRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves a ToDo category by its unique identifier.
        /// </summary>
        /// <param name="toDoCategoryId">The unique identifier of the ToDo category.</param>
        /// <returns>The requested ToDo category, or null if not found.</returns>
        public async Task<ToDoCategory?> GetToDoCategoryByCategoryIdAsync(Guid toDoCategoryId)
        {
            return await _repository.GetToDoCategoryByCategoryIdAsync(toDoCategoryId);
        }

        /// <summary>
        /// Retrieves all ToDo categories associated with a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A list of the user's ToDo categories.</returns>
        public async Task<List<ToDoCategory>> GetToDoCategoriesByUserIdAsync(Guid userId)
        {
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
        /// <exception cref="ArgumentException">Thrown if attempting to update a default category.</exception>
        public async Task UpdateToDoCategoryAsync(ToDoCategory toDoCategory)
        {
            var existingToDoCategory = await _repository.GetToDoCategoryByCategoryIdAsync(toDoCategory.ToDoCategoryId);

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
        /// Deletes a ToDo category and reassigns its tasks to the "Other" category.
        /// </summary>
        /// <param name="toDoCategoryId">The unique identifier of the ToDo category to delete.</param>
        /// <exception cref="InvalidOperationException">Thrown if the category does not exist.</exception>
        /// <exception cref="ArgumentException">Thrown if attempting to delete a protected category.</exception>
        public async Task DeleteToDoCategoryAsync(Guid toDoCategoryId)
        {
            var existingToDoCategory = await _repository.GetToDoCategoryByCategoryIdAsync(toDoCategoryId);

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

            await _repository.DeleteToDoCategoryAsync(toDoCategoryId);
            await _repository.UpdateCategoryInToDosAsync(existingToDoCategory.UserId, oldCategoryName, newCategoryName);
        }

        /// <summary>
        /// Capitalizes the first letter of a category name and converts the rest to lowercase.
        /// </summary>
        /// <param name="categoryName">The category name to format.</param>
        /// <returns>The formatted category name.</returns>
        private string GetNameWithACapitalLetter(string categoryName)
        {
            return char.ToUpper(categoryName[0]) + categoryName.Substring(1).ToLower();
        }
    }
}
