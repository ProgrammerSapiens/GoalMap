using Microsoft.EntityFrameworkCore;
using Core.Models;

namespace Data.DBContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<ToDo> ToDos { get; set; }
        public DbSet<ToDoCategory> ToDoCategories { get; set; }
    }
}
