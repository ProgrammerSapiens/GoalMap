using Core.Interfaces;
using Core.Models;

namespace Core.Repositories
{
    internal class ToDoCategoryRepository : IToDoCategoryRepository
    {
        public Task AddToDoCategoryAsync(ToDoCategory toDoCategory)
        {
            throw new NotImplementedException();
        }

        public Task DeleteToDoCategoryAsync(string toDoCategoryName, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ToDoCategory>> GetToDoCategoriesByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<ToDoCategory> GetToDoCategoryByCategoryNameAsync(string toDoCategoryName, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsCategoryExistsAsync(string toDoCategoryName, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateToDoCategoryAsync(ToDoCategory toDoCategory)
        {
            throw new NotImplementedException();
        }
    }
}
