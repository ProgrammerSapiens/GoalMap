using Core.Interfaces;
using Core.Models;

namespace Core.Services
{
    internal class ToDoCategoryService : IToDoCategoryService
    {
        private readonly IToDoCategoryRepository _repository;

        public ToDoCategoryService(IToDoCategoryRepository repository)
        {
            _repository = repository;
        }

        public Task<ToDoCategory> GetToDoCategoryByIdAsync(Guid toDoCategoryId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ToDoCategory>> GetToDoCategoriesByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task AddToDoCategoryAsync(ToDoCategory? toDoCategory)
        {
            throw new NotImplementedException();
        }

        public Task UpdateToDoCategoryAsync(ToDoCategory? toDoCategory)
        {
            throw new NotImplementedException();
        }

        public Task DeleteToDoCategoryAsync(Guid toDoCategoryId)
        {
            throw new NotImplementedException();
        }
    }
}
