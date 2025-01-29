using Core.Interfaces;
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

            existingUser.Experience = user.Experience;

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Checks if a user with the given username exists in the database.
        /// </summary>
        /// <param name="userName">The username to check.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the user exists.</returns>
        public async Task<bool> UserExistsAsync(string userName)
        {
            return await _context.Users.AsNoTracking().AnyAsync(u => u.UserName == userName);
        }
    }
}
