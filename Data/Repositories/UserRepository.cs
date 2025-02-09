﻿using Core.Interfaces;
using Core.Models;
using Data.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    /// <summary>
    /// Repository class for managing User entities in the database.
    /// Implements the <see cref="IUserRepository"/> interface.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class with the specified database context.
        /// </summary>
        /// <param name="context">The database context used to interact with the data store.</param>
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user if found, or null otherwise.</returns>
        public async Task<User?> GetUserByUserIdAsync(Guid userId)
        {
            return await _context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.UserId == userId);
        }

        /// <summary>
        /// Retrieves a user by their username.
        /// </summary>
        /// <param name="userName">The username of the user to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user if found, or null otherwise.</returns>
        public async Task<User?> GetUserByUserNameAsync(string userName)
        {
            return await _context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.UserName == userName);
        }

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        /// <param name="user">The user entity to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if a user with the same username already exists.</exception>
        public async Task AddUserAsync(User user)
        {
            var userExists = await GetUserByUserNameAsync(user.UserName);
            if (userExists != null)
            {
                throw new InvalidOperationException("Username already exists.");
            }

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing user in the database.
        /// </summary>
        /// <param name="user">The user entity with updated information.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the user does not exist in the database.</exception>
        public async Task UpdateUserAsync(User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId);

            if (existingUser == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            _context.Entry(existingUser).CurrentValues.SetValues(user);

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Adds default to-do categories to the database.
        /// </summary>
        /// <param name="defaultCategories">A list of default categories to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AddDefaultCategoriesAsync(List<ToDoCategory> defaultCategories)
        {
            await _context.ToDoCategories.AddRangeAsync(defaultCategories);
            await _context.SaveChangesAsync();
        }
    }
}
