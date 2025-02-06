using Microsoft.EntityFrameworkCore;
using Core.Models;

namespace Data.DBContext
{
    /// <summary>
    /// Represents the application's database context, providing access to database entities.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class with specified options.
        /// </summary>
        /// <param name="options">Configuration options for the database context.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>
        /// Gets or sets the Users table in the database.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the ToDos table in the database.
        /// </summary>
        public DbSet<ToDo> ToDos { get; set; }

        /// <summary>
        /// Gets or sets the ToDoCategories table in the database.
        /// </summary>
        public DbSet<ToDoCategory> ToDoCategories { get; set; }

        /// <summary>
        /// Configures the database options, enabling lazy loading proxies.
        /// </summary>
        /// <param name="optionsBuilder">The options builder used to configure the database context.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
