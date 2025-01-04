using Core.Interfaces;
using Core.Models;

namespace Core.Services
{
    internal class ToDoService : IToDoService
    {
        public Task AddToDoAsync(ToDo toDo)
        {
            throw new NotImplementedException();
        }

        public Task DeleteToDoAsync(Guid toDoId)
        {
            throw new NotImplementedException();
        }

        public Task<ToDo> GetToDoByIdAsync(Guid toDoId)
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

        public Task<List<ToDo>> GetToDosByUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task MoveIncompleteToDosAsync(DateTime fromDate, DateTime toDate, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task MoveRepeatedToDosAsync(RepeatFrequency repeatFrequency, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateToDoAsync(ToDo toDo)
        {
            throw new NotImplementedException();
        }
    }
}
