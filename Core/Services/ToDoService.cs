using Core.Interfaces;
using Core.Models;

namespace Core.Services
{
    internal class ToDoService : IToDoService
    {
        private readonly IToDoRepository _repository;

        public ToDoService(IToDoRepository repository)
        {
            _repository = repository;
        }

        public Task<ToDo> GetToDoByIdAsync(Guid toDoId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ToDo>> GetToDosByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ToDo>> GetToDosByDateAsync(Guid userId, DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<List<ToDo>> GetToDosByTimeBlockAsync(Guid userId, TimeBlock timeBlock)
        {
            throw new NotImplementedException();
        }

        public Task AddToDoAsync(ToDo? toDo)
        {
            throw new NotImplementedException();
        }

        public Task UpdateToDoAsync(ToDo? toDo)
        {
            throw new NotImplementedException();
        }

        public Task DeleteToDoAsync(Guid toDoId)
        {
            throw new NotImplementedException();
        }

        public Task MoveRepeatedToDosAsync(RepeatFrequency repeatFrequency, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
