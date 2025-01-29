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
        public async Task<ToDoCategory?> GetToDoCategoryByCategoryNameAsync(Guid userId, string toDoCategoryName)
        {
            return await _repository.GetToDoCategoryByCategoryNameAsync(userId, toDoCategoryName);
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

        //TODO: Write test to check if the filters are working correctly.
        /// <summary>
        /// Adds a new ToDo category.
        /// </summary>
        /// <param name="toDoCategory">The ToDo category object.</param>
        /// <exception cref="InvalidOperationException">Thrown if a category with the same name already exists.</exception>
        public async Task AddToDoCategoryAsync(ToDoCategory toDoCategory)
        {
            var nameWithACapitalLetter = toDoCategory.ToDoCategoryName;
            toDoCategory.ToDoCategoryName = char.ToUpper(nameWithACapitalLetter[0]) + nameWithACapitalLetter.Substring(1).ToLower();

            if (await _repository.CategoryExistsByNameAsync(toDoCategory.UserId, toDoCategory.ToDoCategoryName))
            {
                throw new InvalidOperationException("Сategory already exists.");
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
            var nameWithACapitalLetter = toDoCategory.ToDoCategoryName;
            toDoCategory.ToDoCategoryName = char.ToUpper(nameWithACapitalLetter[0]) + nameWithACapitalLetter.Substring(1).ToLower();

            if (!(await _repository.CategoryExistsByCategoryIdAsync(toDoCategory.ToDoCategoryId)))
            {
                throw new InvalidOperationException("Category does not exist.");
            }

            if (await _repository.CategoryExistsByNameAsync(toDoCategory.UserId, toDoCategory.ToDoCategoryName))
            {
                throw new InvalidOperationException("Category with such name already exists.");
            }

            if (toDoCategory.ToDoCategoryName == "Habbit" || toDoCategory.ToDoCategoryName == "Other")
            {
                throw new ArgumentException("You cannot update this category.");
            }
     
            await _repository.UpdateToDoCategoryAsync(toDoCategory);
            //await _repository.UpdateToDosCategoryAsync(toDoCategory.UserId,)
        }

        /// <summary>
        /// Deletes a ToDo category by its name and user Id.
        /// </summary>
        /// <param name="toDoCategoryName">The name of the ToDo category.</param>
        /// <param name="userId">The user Id.</param>
        /// <exception cref="InvalidOperationException">Thrown if the category does not exist.</exception>
        public async Task DeleteToDoCategoryAsync(Guid userId, string toDoCategoryName)
        {
            if (toDoCategoryName == "Habbit" || toDoCategoryName == "Other")
            {
                throw new ArgumentException("You cannot delete this category.");
            }

            await _repository.DeleteToDoCategoryAsync(userId, toDoCategoryName);
        }
    }
}
