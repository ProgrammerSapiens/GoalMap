﻿using Core.Interfaces;
using Core.Models;
using Data.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    /// <summary>
    /// Repository for interacting with ToDoCategory entities in the database.
    /// Provides methods for adding, updating, deleting, and retrieving to-do categories.
    /// </summary>
    public class ToDoCategoryRepository : IToDoCategoryRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the repository with a given database context.
        /// </summary>
        /// <param name="context">The database context used to interact with the data store.</param>
        public ToDoCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a ToDoCategory by its name and associated user ID.
        /// </summary>
        /// <param name="toDoCategoryName">The name of the to-do category.</param>
        /// <param name="userId">The ID of the user who owns the category.</param>
        /// <returns>
        /// A ToDoCategory object if found, otherwise <c>null</c>.
        /// </returns>
        public async Task<ToDoCategory?> GetToDoCategoryByCategoryNameAsync(string toDoCategoryName, Guid userId)
        {
            return await _context.ToDoCategories.FirstOrDefaultAsync(c => c.UserId == userId && c.ToDoCategoryName == toDoCategoryName);
        }

        /// <summary>
        /// Retrieves a list of ToDoCategories associated with a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>
        /// A list of ToDoCategory objects associated with the given user ID.
        /// </returns>
        public async Task<List<ToDoCategory>> GetToDoCategoriesByUserIdAsync(Guid userId)
        {
            return await _context.ToDoCategories.Where(c => c.UserId == userId).ToListAsync();
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
            if (await CategoryExistsAsync(toDoCategory.ToDoCategoryName, toDoCategory.UserId))
            {
                throw new InvalidOperationException("Category with such name already exists.");
            }

            await _context.ToDoCategories.AddAsync(toDoCategory);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing ToDoCategory in the database.
        /// </summary>
        /// <param name="toDoCategory">The ToDoCategory object with updated information.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the category does not exist or if a category with the same name already exists for the user.
        /// </exception>
        public async Task UpdateToDoCategoryAsync(ToDoCategory toDoCategory)
        {
            var existingToDoCategory = await _context.ToDoCategories.FirstOrDefaultAsync(c => c.ToDoCategoryId == toDoCategory.ToDoCategoryId);

            if (existingToDoCategory == null)
            {
                throw new InvalidOperationException("Category does not exist.");
            }

            if (await CategoryExistsAsync(toDoCategory.ToDoCategoryName, toDoCategory.UserId))
            {
                throw new InvalidOperationException("Such todo name already exists.");
            }

            existingToDoCategory.ToDoCategoryName = toDoCategory.ToDoCategoryName;

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a ToDoCategory from the database by its name and user ID.
        /// </summary>
        /// <param name="toDoCategoryName">The name of the ToDoCategory to delete.</param>
        /// <param name="userId">The ID of the user who owns the category.</param>
        public async Task DeleteToDoCategoryAsync(string toDoCategoryName, Guid userId)
        {
            var toDoCategory = await _context.ToDoCategories.FirstOrDefaultAsync(c => c.UserId == userId && c.ToDoCategoryName == toDoCategoryName);

            if (toDoCategory != null)
            {
                _context.ToDoCategories.Remove(toDoCategory);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Checks if a ToDoCategory with the specified name already exists for the given user.
        /// </summary>
        /// <param name="toDoCategoryName">The name of the ToDoCategory.</param>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>
        /// <c>true</c> if the category exists, otherwise <c>false</c>.
        /// </returns>
        public async Task<bool> CategoryExistsAsync(string toDoCategoryName, Guid userId)
        {
            return await _context.ToDoCategories.AnyAsync(c => c.ToDoCategoryName == toDoCategoryName && c.UserId == userId);
        }
    }
}
