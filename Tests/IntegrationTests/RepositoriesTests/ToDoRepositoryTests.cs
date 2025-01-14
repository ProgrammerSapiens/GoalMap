using Core.Interfaces;
using Data.Repositories;
using Core.Models;
using Data.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Tests.IntegrationTests.RepositoriesTests
{
    public class ToDoRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> dbContextOptions;
        private readonly AppDbContext context;

        public ToDoRepositoryTests()
        {
            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;

            context = new AppDbContext(dbContextOptions);
        }

        #region GetToDoByIdAsync(Guid toDoId) tests

        [Fact]
        public async Task GetToDoByIdAsync_ShouldReturnToDo_WhenToDoExists()
        {
            var existingToDo = new ToDo("TestDescription", TimeBlock.Day, Difficulty.Easy, DateTime.Today, Guid.NewGuid(), Guid.NewGuid());

            context.ToDos.Add(existingToDo);
            await context.SaveChangesAsync();

            var toDoRepository = new ToDoRepository(context);

            var result = await toDoRepository.GetToDoByIdAsync(existingToDo.ToDoId);
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

            var repository = new ToDoRepository(context);

            var result = await repository.GetToDoByIdAsync(toDoId);

            var toDoInDb = await context.ToDos.FirstOrDefaultAsync(t => t.ToDoId == toDoId);

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
                    new ToDo("TestDescription", timeBlock, Difficulty.Easy, date, Guid.NewGuid(), userId),
                    new ToDo("TestDescription2", timeBlock, Difficulty.Easy, date, Guid.NewGuid(), userId),
                    new ToDo("TestDescription3", timeBlock, Difficulty.Easy, date, Guid.NewGuid(), Guid.NewGuid())
                };

            context.ToDos.AddRange(toDosList);
            await context.SaveChangesAsync();

            var repository = new ToDoRepository(context);

            var result = await repository.GetToDosAsync(userId, date, timeBlock);

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

            var repository = new ToDoRepository(context);

            var result = await repository.GetToDosAsync(userId, date, timeBlock);

            var toDosInDb = context.ToDos.Where(t => t.UserId == userId && t.ToDoDate == date && t.TimeBlock == timeBlock);

            Assert.Empty(result);
            Assert.Empty(toDosInDb);
        }

        #endregion

        #region AddToDoAsync(ToDo toDo) tests

        [Fact]
        public async Task AddToDoAsync_ShouldAddToDo_WhenDataIsValid()
        {
            var repository = new ToDoRepository(context);
            var toDo = new ToDo("TestDescription", TimeBlock.Day, Difficulty.Easy, DateTime.Today, Guid.NewGuid(), Guid.NewGuid());
            var toDoId = toDo.ToDoId;
            var userId = toDo.UserId;

            await repository.AddToDoAsync(toDo);

            var toDoInDb = await context.ToDos.FirstOrDefaultAsync(t => t.ToDoId == toDoId);

            Assert.NotNull(toDoInDb);
            Assert.Equal("TestDescription", toDoInDb.Description);
            Assert.Equal(TimeBlock.Day, toDoInDb.TimeBlock);
            Assert.Equal(Difficulty.Easy, toDoInDb.Difficulty);
            Assert.Equal(DateTime.Today, toDoInDb.ToDoDate);
            Assert.Equal(userId, toDoInDb.UserId);

            Assert.Single(context.ToDos);
        }

        [Fact]
        public async Task AddToDoAsync_ShouldThrowException_WhenToDoIdAlreadyExists()
        {
            var toDo = new ToDo("TestDescription", TimeBlock.Day, Difficulty.Easy, DateTime.Today, Guid.NewGuid(), Guid.NewGuid());

            context.ToDos.Add(toDo);
            await context.SaveChangesAsync();

            var repository = new ToDoRepository(context);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => repository.AddToDoAsync(toDo));
            Assert.Equal("ToDo with such an Id already exists.", exception.Message);
        }

        #endregion

        #region UpdateToDoAsync(ToDo toDo) tests

        [Fact]
        public async Task UpdateToDoAsync_ShouldUpdateToDo_WhenToDoExists()
        {
            var toDo = new ToDo("TestDescription", TimeBlock.Day, Difficulty.Easy, DateTime.Today, Guid.NewGuid(), Guid.NewGuid());
            var toDoId = toDo.ToDoId;

            context.ToDos.Add(toDo);
            await context.SaveChangesAsync();

            string newDescription = "NewTestDescription";
            toDo.Description = newDescription;

            var repository = new ToDoRepository(context);
            await repository.UpdateToDoAsync(toDo);

            var toDoInDb = await context.ToDos.FirstOrDefaultAsync(t => t.ToDoId == toDoId);

            Assert.NotNull(toDoInDb);
            Assert.Equal(newDescription, toDoInDb.Description);
        }

        [Fact]
        public async Task UpdateToDoAsync_ShouldThrowException_WhenToDoDoesNotExist()
        {
            var toDo = new ToDo("TestDescription", TimeBlock.Day, Difficulty.Easy, DateTime.Today, Guid.NewGuid(), Guid.NewGuid());
            var toDoId = toDo.ToDoId;

            var repository = new ToDoRepository(context);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => repository.UpdateToDoAsync(toDo));

            var toDoInDb = await context.ToDos.FirstOrDefaultAsync(t => t.ToDoId == toDoId);

            Assert.Equal("ToDo with such an Id does not exists.", exception.Message);
            Assert.Null(toDoInDb);
        }

        #endregion

        #region DeleteToDoAsync(Guid toDoId) tests

        [Fact]
        public async Task DeleteToDoAsync_ShouldDeleteToDo_WhenToDoExists()
        {
            var toDo = new ToDo("TestDescription", TimeBlock.Day, Difficulty.Easy, DateTime.Today, Guid.NewGuid(), Guid.NewGuid());
            var toDoId = toDo.ToDoId;

            context.ToDos.Add(toDo);
            await context.SaveChangesAsync();

            var repository = new ToDoRepository(context);
            await repository.DeleteToDoAsync(toDoId);

            var toDoInDb = await context.ToDos.FirstOrDefaultAsync(t => t.ToDoId == toDoId);

            Assert.Null(toDoInDb);
        }

        [Fact]
        public async Task DeleteToDoAsync_ShouldDoNothing_WhenToDoDoesNotExist()
        {
            var toDo = new ToDo("TestDescription", TimeBlock.Day, Difficulty.Easy, DateTime.Today, Guid.NewGuid(), Guid.NewGuid());
            var toDoId = toDo.ToDoId;

            var repository = new ToDoRepository(context);
            await repository.DeleteToDoAsync(toDoId);

            var toDoInDb = await context.ToDos.FirstOrDefaultAsync(t => t.ToDoId == toDoId);

            Assert.Null(toDoInDb);
        }

        #endregion

        #region ToDoExistsAsync(Guid toDoId) tests

        [Fact]
        public async Task ToDoExistsAsync_ShouldReturnTrue_WhenToDoExists()
        {
            var toDo = new ToDo("TestDescription", TimeBlock.Day, Difficulty.Easy, DateTime.Today, Guid.NewGuid(), Guid.NewGuid());
            var toDoId = toDo.ToDoId;

            context.ToDos.Add(toDo);
            await context.SaveChangesAsync();

            var repository = new ToDoRepository(context);
            var result = await repository.ToDoExistsAsync(toDoId);

            var toDoInDb = await context.ToDos.FirstOrDefaultAsync(t => t.ToDoId == toDoId);

            Assert.True(result);
            Assert.NotNull(toDoInDb);
        }

        [Fact]
        public async Task ToDoExistsAsync_ShouldReturnFalse_WhenToDoDoesNotExist()
        {
            var toDo = new ToDo("TestDescription", TimeBlock.Day, Difficulty.Easy, DateTime.Today, Guid.NewGuid(), Guid.NewGuid());
            var toDoId = toDo.ToDoId;

            var repository = new ToDoRepository(context);
            var result = await repository.ToDoExistsAsync(toDoId);

            var toDoInDb = await context.ToDos.FirstOrDefaultAsync(t => t.ToDoId == toDoId);

            Assert.False(result);
            Assert.Null(toDoInDb);
        }

        #endregion
    }
}
