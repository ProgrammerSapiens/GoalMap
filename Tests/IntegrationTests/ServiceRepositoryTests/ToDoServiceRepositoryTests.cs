using Core.Interfaces;
using Core.Models;
using Core.Services;
using Data.DBContext;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.IntegrationTests.Service_RepositoriyTests
{
    public class ToDoServiceRepositoryTests : IAsyncLifetime
    {
        private readonly IToDoRepository _toDoRepository;
        private readonly AppDbContext _context;

        public ToDoServiceRepositoryTests()
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

        #region GetToDoByIdAsync(Guid toDoId)

        [Fact]
        public async Task GetTodoByIdAsync_ShouldReturnCorrectToDo_WhenToDoIdExists()
        {
            var toDo = new ToDo("testDescription", TimeBlock.Day, Difficulty.Easy, DateTime.Today, "Other", Guid.NewGuid(), DateTime.Today.AddDays(1), Guid.NewGuid(), RepeatFrequency.Daily);
            var toDoId = toDo.ToDoId;

            _context.ToDos.Add(toDo);
            await _context.SaveChangesAsync();

            var toDoService = new ToDoService(_toDoRepository);

            var result = await toDoService.GetToDoByIdAsync(toDoId);

            Assert.NotNull(result);
            Assert.Equal(toDo.ToDoId, result.ToDoId);
            Assert.Equal(toDo.Description, result.Description);
        }

        [Fact]
        public async Task GetToDoByIdAsync_ShouldThrowException_WhenToDoIdDoesNotExist()
        {
            var toDoId = Guid.NewGuid();

            var toDoService = new ToDoService(_toDoRepository);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => toDoService.GetToDoByIdAsync(toDoId));

            Assert.Equal("Todo id was not found.", exception.Message);
        }

        #endregion

        #region GetToDosAsync(Guid userId, DateTime date, TimeBlock timeBlock)

        [Fact]
        public async Task GetToDosAsync_ShouldReturnToDosForGivenDate_WhenToDosExist()
        {
            var userId = Guid.NewGuid();
            DateTime date = DateTime.Today;

            var expectedToDos = new List<ToDo>
            {
                new ToDo("testDescription", TimeBlock.Day, Difficulty.Easy, date, "Other", userId, DateTime.Today.AddDays(1), Guid.NewGuid(), RepeatFrequency.Daily),
                new ToDo("testDescription", TimeBlock.Day, Difficulty.Easy, date, "Other", userId, DateTime.Today.AddDays(1), Guid.NewGuid(), RepeatFrequency.Daily)
            };

            _context.ToDos.AddRange(expectedToDos);
            await _context.SaveChangesAsync();

            var toDoService = new ToDoService(_toDoRepository);

            var result = await toDoService.GetToDosAsync(userId, date, TimeBlock.Day);

            Assert.NotNull(result);
            Assert.Equal(expectedToDos.Count, result.Count);
        }

        [Fact]
        public async Task GetToDosAsync_ShouldReturnEmptyList_WhenNoToDosForDate()
        {
            var userId = Guid.NewGuid();
            DateTime date = DateTime.Today;

            var toDoService = new ToDoService(_toDoRepository);

            var result = await toDoService.GetToDosAsync(userId, date, TimeBlock.Day);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region AddToDoAsync(ToDo toDo)

        [Fact]
        public async Task AddToDoAsync_ShouldAddToDoSuccessfully_WhenToDoIsValid()
        {
            string description = "testDescription";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Easy;
            DateTime toDoDate = DateTime.Today;
            string toDoCategoryName = "Other";
            var userId = Guid.NewGuid();
            DateTime deadline = DateTime.Today.AddDays(1);
            var parentToDoId = Guid.NewGuid();
            RepeatFrequency repeatFrequency = RepeatFrequency.Daily;
            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryName, userId, deadline, parentToDoId, repeatFrequency);
            var toDoId = toDo.ToDoId;

            var toDoService = new ToDoService(_toDoRepository);

            await toDoService.AddToDoAsync(toDo);

            var toDoInDb = await _context.ToDos.FirstOrDefaultAsync(t => t.ToDoId == toDoId);

            Assert.NotNull(toDoInDb);
            Assert.Equal(description, toDoInDb.Description);
            Assert.Equal(timeBlock, toDoInDb.TimeBlock);
            Assert.Equal(difficulty, toDoInDb.Difficulty);
            Assert.Equal(toDoDate, toDoInDb.ToDoDate);
            Assert.Equal(toDoCategoryName, toDoInDb.ToDoCategoryName);
            Assert.Equal(userId, toDoInDb.UserId);
            Assert.Equal(deadline, toDoInDb.Deadline);
            Assert.Equal(parentToDoId, toDoInDb.ParentToDoId);
            Assert.Equal(repeatFrequency, toDoInDb.RepeatFrequency);
        }

        [Fact]
        public async Task AddToDoAsync_ShouldThrowException_WhenToDoAlreadyExists()
        {
            string description = "testDescription";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Easy;
            DateTime toDoDate = DateTime.Today;
            string toDoCategoryName = "Other";
            var userId = Guid.NewGuid();
            DateTime deadline = DateTime.Today.AddDays(1);
            var parentToDoId = Guid.NewGuid();
            RepeatFrequency repeatFrequency = RepeatFrequency.Daily;
            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryName, userId, deadline, parentToDoId, repeatFrequency);

            _context.ToDos.Add(toDo);
            await _context.SaveChangesAsync();

            var toDoService = new ToDoService(_toDoRepository);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => toDoService.AddToDoAsync(toDo));

            Assert.Equal("ToDo id already exists.", exception.Message);
        }

        #endregion

        #region UpdateToDoAsync(ToDo toDo)

        [Fact]
        public async Task UpdateToDoAsync_ShouldUpdateTaskSuccessfully_WhenToDoIsValid()
        {
            string description = "testDescription";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Easy;
            DateTime toDoDate = DateTime.Today;
            string toDoCategoryName = "Other";
            var userId = Guid.NewGuid();
            DateTime deadline = DateTime.Today.AddDays(1);
            var parentToDoId = Guid.NewGuid();
            RepeatFrequency repeatFrequency = RepeatFrequency.Daily;
            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryName, userId, deadline, parentToDoId, repeatFrequency);
            var toDoId = toDo.ToDoId;

            _context.ToDos.Add(toDo);
            await _context.SaveChangesAsync();

            var toDoService = new ToDoService(_toDoRepository);

            await toDoService.UpdateToDoAsync(toDo);

            var toDoInDb = await _context.ToDos.FirstOrDefaultAsync(t => t.ToDoId == toDoId);

            Assert.NotNull(toDoInDb);
            Assert.Equal(description, toDoInDb.Description);
            Assert.Equal(timeBlock, toDoInDb.TimeBlock);
            Assert.Equal(difficulty, toDoInDb.Difficulty);
            Assert.Equal(toDoDate, toDoInDb.ToDoDate);
            Assert.Equal(toDoCategoryName, toDoInDb.ToDoCategoryName);
            Assert.Equal(userId, toDoInDb.UserId);
            Assert.Equal(deadline, toDoInDb.Deadline);
            Assert.Equal(parentToDoId, toDoInDb.ParentToDoId);
            Assert.Equal(repeatFrequency, toDoInDb.RepeatFrequency);
        }

        [Fact]
        public async Task UpdateToDoAsync_ShouldThrowException_WhenToDoDoesNotExist()
        {
            string description = "testDescription";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Easy;
            DateTime toDoDate = DateTime.Today;
            string toDoCategoryName = "Other";
            var userId = Guid.NewGuid();
            DateTime deadline = DateTime.Today.AddDays(1);
            var parentToDoId = Guid.NewGuid();
            RepeatFrequency repeatFrequency = RepeatFrequency.Daily;
            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryName, userId, deadline, parentToDoId, repeatFrequency);

            var toDoService = new ToDoService(_toDoRepository);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => toDoService.UpdateToDoAsync(toDo));

            Assert.Equal("Todo id does not exist.", exception.Message);
        }

        #endregion

        #region DeleteToDoAsync(Guid toDoId)

        [Fact]
        public async Task DeleteToDoAsync_ShouldDeleteToDo_WhenToDoIdExists()
        {
            string description = "testDescription";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Easy;
            DateTime toDoDate = DateTime.Today;
            string toDoCategoryName = "Other";
            var userId = Guid.NewGuid();
            DateTime deadline = DateTime.Today.AddDays(1);
            var parentToDoId = Guid.NewGuid();
            RepeatFrequency repeatFrequency = RepeatFrequency.Daily;
            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryName, userId, deadline, parentToDoId, repeatFrequency);
            var toDoId = toDo.ToDoId;

            _context.ToDos.Add(toDo);
            await _context.SaveChangesAsync();

            var toDoService = new ToDoService(_toDoRepository);

            await toDoService.DeleteToDoAsync(toDoId);

            var toDoInDb = await _context.ToDos.FirstOrDefaultAsync(t => t.ToDoId == toDoId);

            Assert.Null(toDoInDb);
        }

        [Fact]
        public async Task DeleteToDoAsync_ShouldThrowException_WhenToDoIdDoesNotExist()
        {
            var toDoId = Guid.NewGuid();

            var toDoService = new ToDoService(_toDoRepository);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => toDoService.DeleteToDoAsync(toDoId));

            Assert.Equal("Todo id does not exist.", exception.Message);
        }

        #endregion

        #region MoveRepeatedToDosAsync(Guid userId)

        [Fact]
        public async Task MoveRepeatedToDosAsync_ShouldMoveToDosWithCorrectDates()
        {
            var userId = Guid.NewGuid();
            var originalDate = DateTime.Today;

            var repeatedToDos = new List<ToDo>
            {
                new ToDo("Daily Task", TimeBlock.Day, Difficulty.Easy, originalDate, "Other", userId, null, Guid.NewGuid(), RepeatFrequency.Daily),
                new ToDo("Weekly Task", TimeBlock.Day, Difficulty.Medium, originalDate, "Other", userId, null, Guid.NewGuid(), RepeatFrequency.Weekly),
                new ToDo("Monthly Task", TimeBlock.Day, Difficulty.Hard, originalDate, "Other", userId, null, Guid.NewGuid(), RepeatFrequency.Monthly),
                new ToDo("Yearly Task", TimeBlock.Day, Difficulty.Nightmare, originalDate, "Other", userId, null, Guid.NewGuid(), RepeatFrequency.Yearly),
                new ToDo("Not repeated task", TimeBlock.Day, Difficulty.Nightmare, originalDate, "Other", userId, null, Guid.NewGuid(), RepeatFrequency.None)
            };

            _context.ToDos.AddRange(repeatedToDos);
            await _context.SaveChangesAsync();

            var toDoService = new ToDoService(_toDoRepository);

            await toDoService.MoveRepeatedToDosAsync(userId);

            var toDoInDb = await _context.ToDos.FirstOrDefaultAsync(t => t.Description == "Daily Task");
            Assert.NotNull(toDoInDb);
            Assert.Equal(originalDate.AddDays(1), toDoInDb.ToDoDate);

            toDoInDb = await _context.ToDos.FirstOrDefaultAsync(t => t.Description == "Weekly Task");
            Assert.NotNull(toDoInDb);
            Assert.Equal(originalDate.AddDays(7), toDoInDb.ToDoDate);

            toDoInDb = await _context.ToDos.FirstOrDefaultAsync(t => t.Description == "Monthly Task");
            Assert.NotNull(toDoInDb);
            Assert.Equal(originalDate.AddMonths(1), toDoInDb.ToDoDate);

            toDoInDb = await _context.ToDos.FirstOrDefaultAsync(t => t.Description == "Yearly Task");
            Assert.NotNull(toDoInDb);
            Assert.Equal(originalDate.AddYears(1), toDoInDb.ToDoDate);

            toDoInDb = await _context.ToDos.FirstOrDefaultAsync(t => t.Description == "Not repeated task");
            Assert.NotNull(toDoInDb);
            Assert.Equal(originalDate, toDoInDb.ToDoDate);
        }

        #endregion
    }
}
