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
        public async Task<ToDoCategory> GetToDoCategoryByCategoryNameAsync(string toDoCategoryName, Guid userId)
        {
            if (!(await _repository.IsCategoryExistsAsync(toDoCategoryName, userId)))
            {
                throw new InvalidOperationException("Category does not exist.");
            }

            var result = await _repository.GetToDoCategoryByCategoryNameAsync(toDoCategoryName, userId);

            return result;
        }

        /// <summary>
        /// Retrieves all ToDo categories for the specified user.
        /// </summary>
        /// <param name="userId">The user Id.</param>
        /// <returns>A list of ToDo categories.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the user is not found or has no categories.</exception>
        public async Task<List<ToDoCategory>> GetToDoCategoriesByUserIdAsync(Guid userId)
        {
            var resultListOfToDoCategories = await _repository.GetToDoCategoriesByUserIdAsync(userId);

            if (resultListOfToDoCategories.Count == 0)
            {
                throw new InvalidOperationException("User Id does not exist or there is no categories.");
            }

            return resultListOfToDoCategories;
        }

        /// <summary>
        /// Adds a new ToDo category.
        /// </summary>
        /// <param name="toDoCategory">The ToDo category object.</param>
        /// <exception cref="InvalidOperationException">Thrown if a category with the same name already exists.</exception>
        public async Task AddToDoCategoryAsync(ToDoCategory toDoCategory)
        {
            if (await _repository.IsCategoryExistsAsync(toDoCategory.ToDoCategoryName, toDoCategory.UserId))
            {
                throw new InvalidOperationException("Сategory already exists.");
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
            if (!(await _repository.IsCategoryExistsAsync(toDoCategory.ToDoCategoryName, toDoCategory.UserId)))
            {
                throw new InvalidOperationException("Category does not exist.");
            }

            if (toDoCategory.ToDoCategoryName == "Habbit" || toDoCategory.ToDoCategoryName == "Other")
            {
                throw new ArgumentException("You cannot update this category.");
            }

            await _repository.UpdateToDoCategoryAsync(toDoCategory);
        }

        /// <summary>
        /// Deletes a ToDo category by its name and user Id.
        /// </summary>
        /// <param name="toDoCategoryName">The name of the ToDo category.</param>
        /// <param name="userId">The user Id.</param>
        /// <exception cref="InvalidOperationException">Thrown if the category does not exist.</exception>
        public async Task DeleteToDoCategoryAsync(string toDoCategoryName, Guid userId)
        {
            if (!(await _repository.IsCategoryExistsAsync(toDoCategoryName, userId)))
            {
                throw new InvalidOperationException("Category does not exist.");
            }

            if (toDoCategoryName == "Habbit" || toDoCategoryName == "Other")
            {
                throw new ArgumentException("You cannot delete this category.");
            }

            await _repository.DeleteToDoCategoryAsync(toDoCategoryName, userId);
        }
    }
}
