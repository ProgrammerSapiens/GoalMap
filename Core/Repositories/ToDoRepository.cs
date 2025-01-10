using Core.Interfaces;
using Core.Models;

namespace Core.Repositories
{
   public class ToDoRepository : IToDoRepository
    {
        public async Task AddToDoAsync(ToDo toDo)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteToDoAsync(Guid toDoId)
        {
            throw new NotImplementedException();
        }

        public async Task<ToDo?> GetToDoByIdAsync(Guid toDoId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ToDo>> GetToDosAsync(Guid userId, DateTime date, TimeBlock timeBlock)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsToDoExistsAsync(Guid toDoId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateToDoAsync(ToDo toDo)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateToDosAsync(IEnumerable<ToDo> toDos)
        {
            throw new NotImplementedException();
        }
    }
}
