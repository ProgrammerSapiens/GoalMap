using Core.Interfaces;
using Core.Models;
using Data.DBContext;

namespace Data.Repositories
{
    public class ToDoCategoryRepository : IToDoCategoryRepository
    {
        private readonly AppDbContext _context;

        public ToDoCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ToDoCategory> GetToDoCategoryByCategoryNameAsync(string toDoCategoryName, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ToDoCategory>> GetToDoCategoriesByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task AddToDoCategoryAsync(ToDoCategory toDoCategory)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateToDoCategoryAsync(ToDoCategory toDoCategory)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteToDoCategoryAsync(string toDoCategoryName, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CategoryExistsAsync(string toDoCategoryName, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
