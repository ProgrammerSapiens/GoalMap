using Core.Interfaces;
using Core.Models;

namespace Core.Repositories
{
    internal class ToDoRepository : IToDoRepository
    {
        public Task AddToDoAsync(ToDo toDo)
        {
            throw new NotImplementedException();
        }

        public Task DeleteToDoAsync(Guid toDoId)
        {
            throw new NotImplementedException();
        }

        public Task<ToDo?> GetToDoByIdAsync(Guid toDoId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ToDo>> GetToDosAsync(Guid userId, DateTime date, TimeBlock timeBlock)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsToDoExistsAsync(Guid toDoId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateToDoAsync(ToDo toDo)
        {
            throw new NotImplementedException();
        }

        public Task UpdateToDosAsync(IEnumerable<ToDo> toDos)
        {
            throw new NotImplementedException();
        }
    }
}
