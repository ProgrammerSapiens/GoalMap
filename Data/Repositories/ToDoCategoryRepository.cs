using Core.Interfaces;
using Core.Models;

namespace Data.Repositories
{
    public class ToDoCategoryRepository : IToDoCategoryRepository
    {
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

        public async Task<bool> IsCategoryExistsAsync(string toDoCategoryName, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
