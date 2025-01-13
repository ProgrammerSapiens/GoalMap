using Core.Interfaces;
using Core.Models;
using Data.DBContext;

namespace Data.Repositories
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly AppDbContext _context;

        public ToDoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ToDo?> GetToDoByIdAsync(Guid toDoId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ToDo>> GetToDosAsync(Guid userId, DateTime date, TimeBlock timeBlock)
        {
            throw new NotImplementedException();
        }

        public async Task AddToDoAsync(ToDo toDo)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateToDoAsync(ToDo toDo)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteToDoAsync(Guid toDoId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ToDoExistsAsync(Guid toDoId)
        {
            throw new NotImplementedException();
        }
    }
}
