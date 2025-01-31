using Data.Repositories;
using Core.Models;
using Data.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Tests.IntegrationTests.RepositoriesTests
{
    public class ToDoRepositoryTests : IAsyncLifetime
    {
        private readonly AppDbContext _context;
        private readonly ToDoRepository _toDoRepository;

        public ToDoRepositoryTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _context = new AppDbContext(dbContextOptions);
            _toDoRepository = new ToDoRepository(_context);
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }

        #region GetToDoByIdAsync(Guid toDoId) tests

        [Fact]
        public async Task GetToDoByIdAsync_ShouldReturnToDo_WhenToDoExists()
        {
            var existingToDo = new ToDo("TestDescription", TimeBlock.Day, Difficulty.Easy, DateTime.Today, "Other", Guid.NewGuid());

            _context.ToDos.Add(existingToDo);
            await _context.SaveChangesAsync();

            var result = await _toDoRepository.GetToDoByIdAsync(existingToDo.ToDoId);
            Assert.NotNull(result);
            Assert.Equal(existingToDo.Description, result.Description);
            Assert.Equal(existingToDo.TimeBlock, result.TimeBlock);
            Assert.Equal(existingToDo.Difficulty, result.Difficulty);
            Assert.Equal(existingToDo.ToDoDate, result.ToDoDate);
            Assert.Equal(existingToDo.ParentToDoId, result.ParentToDoId);
            Assert.Equal(existingToDo.UserId, result.UserId);
        }

        [Fact]
        public async Task GetToDoByIdAsync_ShouldReturnNull_WhenToDoDoesNotExist()
        {
            var toDoId = Guid.NewGuid();

            var result = await _toDoRepository.GetToDoByIdAsync(toDoId);

            var toDoInDb = await _context.ToDos.FirstOrDefaultAsync(t => t.ToDoId == toDoId);

            Assert.Null(result);
            Assert.Null(toDoInDb);
        }

        #endregion

        #region GetToDosAsync(Guid userId, DateTime date, TimeBlock timeBlock) tests

        [Fact]
        public async Task GetToDosAsync_ShouldReturnToDos_WhenToDosExistForUserAndTimeBlock()
        {
            var userId = Guid.NewGuid();
            var date = DateTime.Today;
            TimeBlock timeBlock = TimeBlock.Day;
            var toDosList = new List<ToDo>()
            {
                new ToDo("TestDescription", timeBlock, Difficulty.Easy, date, "Other", userId),
                new ToDo("TestDescription2", timeBlock, Difficulty.Easy, date, "Other", userId),
                new ToDo("TestDescription3", timeBlock, Difficulty.Easy, date, "Other", Guid.NewGuid())
            };

            _context.ToDos.AddRange(toDosList);
            await _context.SaveChangesAsync();

            var result = await _toDoRepository.GetToDosAsync(userId, date, timeBlock);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, toDo =>
            {
                Assert.Equal(userId, toDo.UserId);
                Assert.Equal(date, toDo.ToDoDate);
                Assert.Equal(timeBlock, toDo.TimeBlock);
            });
        }

        [Fact]
        public async Task GetToDosAsync_ShouldReturnEmptyList_WhenNoToDosExistForUserAndTimeBlock()
        {
            var userId = Guid.NewGuid();
            var date = DateTime.Today;
            var timeBlock = TimeBlock.Day;

            var result = await _toDoRepository.GetToDosAsync(userId, date, timeBlock);

            var toDosInDb = _context.ToDos.Where(t => t.UserId == userId && t.ToDoDate == date && t.TimeBlock == timeBlock);

            Assert.Empty(result);
            Assert.Empty(toDosInDb);
        }

        #endregion

        #region AddToDoAsync(ToDo toDo) tests

        [Fact]
        public async Task AddToDoAsync_ShouldAddToDo_WhenDataIsValid()
        {
            var toDo = new ToDo("TestDescription", TimeBlock.Day, Difficulty.Easy, DateTime.Today, "Other", Guid.NewGuid());
            var toDoId = toDo.ToDoId;
            var userId = toDo.UserId;

            await _toDoRepository.AddToDoAsync(toDo);

            var toDoInDb = await _context.ToDos.FirstOrDefaultAsync(t => t.ToDoId == toDoId);

            Assert.NotNull(toDoInDb);
            Assert.Equal("TestDescription", toDoInDb.Description);
            Assert.Equal(TimeBlock.Day, toDoInDb.TimeBlock);
            Assert.Equal(Difficulty.Easy, toDoInDb.Difficulty);
            Assert.Equal(DateTime.Today, toDoInDb.ToDoDate);
            Assert.Equal(userId, toDoInDb.UserId);
        }

        [Fact]
        public async Task AddToDoAsync_ShouldThrowException_WhenToDoIdAlreadyExists()
        {
            var toDo = new ToDo("TestDescription", TimeBlock.Day, Difficulty.Easy, DateTime.Today, "Other", Guid.NewGuid());

            _context.ToDos.Add(toDo);
            await _context.SaveChangesAsync();

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _toDoRepository.AddToDoAsync(toDo));
            Assert.Equal("Todo already exists.", exception.Message);
        }

        #endregion

        #region UpdateToDoAsync(ToDo toDo) tests

        [Fact]
        public async Task UpdateToDoAsync_ShouldUpdateToDo_WhenToDoExists()
        {
            var toDo = new ToDo("TestDescription", TimeBlock.Day, Difficulty.Easy, DateTime.Today, "Other", Guid.NewGuid());
            var toDoId = toDo.ToDoId;

            _context.ToDos.Add(toDo);
            await _context.SaveChangesAsync();

            string newDescription = "NewTestDescription";
            toDo.Description = newDescription;

            await _toDoRepository.UpdateToDoAsync(toDo);

            var toDoInDb = await _context.ToDos.FirstOrDefaultAsync(t => t.ToDoId == toDoId);

            Assert.NotNull(toDoInDb);
            Assert.Equal(newDescription, toDoInDb.Description);
        }

        [Fact]
        public async Task UpdateToDoAsync_ShouldThrowException_WhenToDoDoesNotExist()
        {
            var toDo = new ToDo("TestDescription", TimeBlock.Day, Difficulty.Easy, DateTime.Today, "Other", Guid.NewGuid());
            var toDoId = toDo.ToDoId;

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _toDoRepository.UpdateToDoAsync(toDo));

            var toDoInDb = await _context.ToDos.FirstOrDefaultAsync(t => t.ToDoId == toDoId);

            Assert.Equal("ToDo with such an Id does not exists.", exception.Message);
            Assert.Null(toDoInDb);
        }

        #endregion

        #region DeleteToDoAsync(Guid toDoId) tests

        [Fact]
        public async Task DeleteToDoAsync_ShouldDeleteToDo_WhenToDoExists()
        {
            var toDo = new ToDo("TestDescription", TimeBlock.Day, Difficulty.Easy, DateTime.Today, "Other", Guid.NewGuid());
            var toDoId = toDo.ToDoId;

            _context.ToDos.Add(toDo);
            await _context.SaveChangesAsync();

            await _toDoRepository.DeleteToDoAsync(toDoId);

            var toDoInDb = await _context.ToDos.FirstOrDefaultAsync(t => t.ToDoId == toDoId);

            Assert.Null(toDoInDb);
        }

        [Fact]
        public async Task DeleteToDoAsync_ShouldDoNothing_WhenToDoDoesNotExist()
        {
            var toDo = new ToDo("TestDescription", TimeBlock.Day, Difficulty.Easy, DateTime.Today, "Other", Guid.NewGuid());
            var toDoId = toDo.ToDoId;

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _toDoRepository.DeleteToDoAsync(toDoId));
            Assert.Equal("Todo id does not exist.", exception.Message);
        }

        #endregion

        #region ToDoExistsAsync(Guid toDoId) tests

        [Fact]
        public async Task ToDoExistsAsync_ShouldReturnTrue_WhenToDoExists()
        {
            var toDo = new ToDo("TestDescription", TimeBlock.Day, Difficulty.Easy, DateTime.Today, "Other", Guid.NewGuid());
            var toDoId = toDo.ToDoId;

            _context.ToDos.Add(toDo);
            await _context.SaveChangesAsync();

            var result = await _toDoRepository.ToDoExistsAsync(toDoId);

            var toDoInDb = await _context.ToDos.FirstOrDefaultAsync(t => t.ToDoId == toDoId);

            Assert.True(result);
            Assert.NotNull(toDoInDb);
        }

        [Fact]
        public async Task ToDoExistsAsync_ShouldReturnFalse_WhenToDoDoesNotExist()
        {
            var toDo = new ToDo("TestDescription", TimeBlock.Day, Difficulty.Easy, DateTime.Today, "Other", Guid.NewGuid());
            var toDoId = toDo.ToDoId;

            var result = await _toDoRepository.ToDoExistsAsync(toDoId);

            var toDoInDb = await _context.ToDos.FirstOrDefaultAsync(t => t.ToDoId == toDoId);

            Assert.False(result);
            Assert.Null(toDoInDb);
        }

        #endregion
    }
}
