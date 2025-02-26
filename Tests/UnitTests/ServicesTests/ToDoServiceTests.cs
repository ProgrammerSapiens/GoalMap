using Core.Interfaces;
using Core.Models;
using Core.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.UnitTests.ServicesTests
{
    public class ToDoServiceTests
    {
        private readonly Mock<IToDoRepository> _toDoRepositoryMock;
        private readonly Mock<ILogger<ToDoService>> _loggerMock;
        private readonly IToDoService _toDoService;

        public ToDoServiceTests()
        {
            _toDoRepositoryMock = new Mock<IToDoRepository>();
            _loggerMock = new Mock<ILogger<ToDoService>>();

            _toDoService = new ToDoService(_toDoRepositoryMock.Object, _loggerMock.Object);
        }

        #region GetToDoByIdAsync(Guid toDoId) tests

        [Fact]
        public async Task GetTodoByIdAsync_ShouldReturnCorrectToDo_WhenToDoIdExists()
        {
            var expectedToDo = new ToDo("testDescription", TimeBlock.Day, Difficulty.Easy, DateTime.Today, Guid.NewGuid(), Guid.NewGuid(), null, DateTime.Today.AddDays(1), Guid.NewGuid(), RepeatFrequency.Daily);
            var toDoId = expectedToDo.ToDoId;

            _toDoRepositoryMock.Setup(repo => repo.GetToDoByIdAsync(toDoId)).ReturnsAsync(expectedToDo);

            var result = await _toDoService.GetToDoByIdAsync(toDoId);

            Assert.NotNull(result);
            Assert.Equal(expectedToDo.ToDoId, result.ToDoId);
            Assert.Equal(expectedToDo.Description, result.Description);
            _toDoRepositoryMock.Verify(repo => repo.GetToDoByIdAsync(toDoId), Times.Once);
        }

        [Fact]
        public async Task GetToDoByIdAsync_ShouldReturnNull_WhenToDoIdDoesNotExist()
        {
            var toDoId = Guid.NewGuid();

            _toDoRepositoryMock.Setup(repo => repo.GetToDoByIdAsync(toDoId)).ReturnsAsync((ToDo?)null);

            var result = await _toDoService.GetToDoByIdAsync(toDoId);
            Assert.Null(result);
        }

        #endregion

        #region GetToDosAsync(Guid userId, DateTime date, TimeBlock timeBlock) tests

        [Fact]
        public async Task GetToDosAsync_ShouldReturnToDosForGivenDate_WhenToDosExist()
        {
            var userId = Guid.NewGuid();
            DateTime date = DateTime.Today;

            var expectedToDos = new List<ToDo>
            {
                new ToDo("testDescription", TimeBlock.Day, Difficulty.Easy, date, Guid.NewGuid(), userId, null, DateTime.Today.AddDays(1), Guid.NewGuid(), RepeatFrequency.Daily),
                new ToDo("testDescription", TimeBlock.Day, Difficulty.Easy, date, Guid.NewGuid(), userId, null, DateTime.Today.AddDays(1), Guid.NewGuid(), RepeatFrequency.Daily)
            };

            _toDoRepositoryMock.Setup(repo => repo.GetToDosAsync(userId, date, TimeBlock.Day)).ReturnsAsync(expectedToDos);

            var result = await _toDoService.GetToDosAsync(userId, date, TimeBlock.Day);

            Assert.NotNull(result);
            Assert.Equal(expectedToDos.Count, result.Count);
            _toDoRepositoryMock.Verify(repo => repo.GetToDosAsync(userId, date, TimeBlock.Day), Times.Once);
        }

        [Fact]
        public async Task GetToDosAsync_ShouldReturnEmptyList_WhenNoToDosForDate()
        {
            var userId = Guid.NewGuid();
            DateTime date = DateTime.Today;

            _toDoRepositoryMock.Setup(repo => repo.GetToDosAsync(userId, date, TimeBlock.Day)).ReturnsAsync(new List<ToDo>());

            var result = await _toDoService.GetToDosAsync(userId, date, TimeBlock.Day);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region AddToDoAsync(ToDo toDo) tests

        [Fact]
        public async Task AddToDoAsync_ShouldAddToDoSuccessfully_WhenToDoIsValid()
        {
            string description = "testDescription";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Easy;
            DateTime toDoDate = DateTime.Today;
            Guid toDoCategoryId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            DateTime deadline = DateTime.Today.AddDays(1);
            var parentToDoId = Guid.NewGuid();
            RepeatFrequency repeatFrequency = RepeatFrequency.Daily;
            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId, null, deadline, parentToDoId, repeatFrequency);

            _toDoRepositoryMock.Setup(repo => repo.ToDoExistsAsync(toDo.ToDoId)).ReturnsAsync(false);
            _toDoRepositoryMock.Setup(repo => repo.AddToDoAsync(It.IsAny<ToDo>())).Returns(Task.CompletedTask);

            await _toDoService.AddToDoAsync(toDo);

            _toDoRepositoryMock.Verify(repo => repo.ToDoExistsAsync(toDo.ToDoId), Times.Once);
            _toDoRepositoryMock.Verify(repo => repo.AddToDoAsync(It.Is<ToDo>(t => t.ToDoId == toDo.ToDoId
            && t.Description == toDo.Description
            && t.TimeBlock == toDo.TimeBlock
            && t.Difficulty == toDo.Difficulty
            && t.ToDoDate == toDo.ToDoDate
            && t.ToDoCategoryId == toDo.ToDoCategoryId
            && t.UserId == toDo.UserId
            && t.Deadline == toDo.Deadline
            && t.ParentToDoId == toDo.ParentToDoId
            && t.RepeatFrequency == toDo.RepeatFrequency)), Times.Once);
        }

        [Fact]
        public async Task AddToDoAsync_ShouldThrowException_WhenToDoAlreadyExists()
        {
            string description = "testDescription";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Easy;
            DateTime toDoDate = DateTime.Today;
            Guid toDoCategoryId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            DateTime deadline = DateTime.Today.AddDays(1);
            var parentToDoId = Guid.NewGuid();
            RepeatFrequency repeatFrequency = RepeatFrequency.Daily;
            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId, null, deadline, parentToDoId, repeatFrequency);

            _toDoRepositoryMock.Setup(repo => repo.ToDoExistsAsync(toDo.ToDoId)).ReturnsAsync(true);
            _toDoRepositoryMock.Setup(repo => repo.AddToDoAsync(toDo)).Returns(Task.CompletedTask);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _toDoService.AddToDoAsync(toDo));

            Assert.Equal("ToDo id already exists.", exception.Message);
            _toDoRepositoryMock.Verify(repo => repo.ToDoExistsAsync(toDo.ToDoId), Times.Once);
            _toDoRepositoryMock.Verify(repo => repo.AddToDoAsync(toDo), Times.Never);
        }

        #endregion

        #region UpdateToDoAsync(ToDo toDo) tests

        [Fact]
        public async Task UpdateToDoAsync_ShouldUpdateTaskSuccessfully_WhenToDoIsValid()
        {
            string description = "testDescription";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Easy;
            DateTime toDoDate = DateTime.Today;
            var toDoCategoryId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            DateTime deadline = DateTime.Today.AddDays(1);
            var parentToDoId = Guid.NewGuid();
            RepeatFrequency repeatFrequency = RepeatFrequency.Daily;
            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId, null, deadline, parentToDoId, repeatFrequency);

            _toDoRepositoryMock.Setup(repo => repo.GetToDoByIdAsync(toDo.ToDoId)).ReturnsAsync(toDo);
            _toDoRepositoryMock.Setup(repo => repo.UpdateToDoAsync(It.IsAny<ToDo>())).Returns(Task.CompletedTask);

            await _toDoService.UpdateToDoAsync(toDo);

            _toDoRepositoryMock.Verify(repo => repo.GetToDoByIdAsync(toDo.ToDoId), Times.Once);
            _toDoRepositoryMock.Verify(repo => repo.UpdateToDoAsync(It.Is<ToDo>(t => t.ToDoId == toDo.ToDoId
            && t.Description == toDo.Description
            && t.TimeBlock == toDo.TimeBlock
            && t.Difficulty == toDo.Difficulty
            && t.ToDoDate == toDo.ToDoDate
            && t.ToDoCategoryId == toDo.ToDoCategoryId
            && t.UserId == toDo.UserId
            && t.Deadline == toDo.Deadline
            && t.ParentToDoId == toDo.ParentToDoId
            && t.RepeatFrequency == toDo.RepeatFrequency)), Times.Once);
        }

        [Fact]
        public async Task UpdateToDoAsync_ShouldThrowException_WhenToDoDoesNotExist()
        {
            string description = "testDescription";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Easy;
            DateTime toDoDate = DateTime.Today;
            var toDoCategoryId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            DateTime deadline = DateTime.Today.AddDays(1);
            var parentToDoId = Guid.NewGuid();
            RepeatFrequency repeatFrequency = RepeatFrequency.Daily;
            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId, null, deadline, parentToDoId, repeatFrequency);

            _toDoRepositoryMock.Setup(repo => repo.GetToDoByIdAsync(toDo.ToDoId)).ReturnsAsync((ToDo?)null);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _toDoService.UpdateToDoAsync(toDo));
            Assert.Equal("Todo id does not exist.", exception.Message);
        }

        [Fact]
        public async Task UpdateToDoAsync_ShouldThrowException_WhenToDoIsAlreadyCompleted()
        {
            string description = "testDescription";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Easy;
            DateTime toDoDate = DateTime.Today;
            var toDoCategoryId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            DateTime deadline = DateTime.Today.AddDays(1);
            var parentToDoId = Guid.NewGuid();
            RepeatFrequency repeatFrequency = RepeatFrequency.Daily;
            bool isCompleted = true;
            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId, null, deadline, parentToDoId, repeatFrequency);

            toDo.CompletionStatus = isCompleted;

            _toDoRepositoryMock.Setup(repo => repo.GetToDoByIdAsync(toDo.ToDoId)).ReturnsAsync(toDo);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _toDoService.UpdateToDoAsync(toDo));
            Assert.Equal("You cannot update completed todo", exception.Message);

        }

        #endregion

        #region DeleteToDoAsync(Guid toDoId) tests

        [Fact]
        public async Task DeleteToDoAsync_ShouldDeleteToDo_WhenToDoIdExists()
        {
            var toDoId = Guid.NewGuid();

            _toDoRepositoryMock.Setup(repo => repo.DeleteToDoAsync(toDoId)).Returns(Task.CompletedTask);

            await _toDoService.DeleteToDoAsync(toDoId);

            _toDoRepositoryMock.Verify(repo => repo.DeleteToDoAsync(toDoId), Times.Once);
        }

        #endregion

        #region MoveRepeatedToDosAsync(Guid userId) tests

        [Fact]
        public async Task MoveRepeatedToDosAsync_ShouldMoveToDosWithCorrectDates()
        {
            var userId = Guid.NewGuid();
            var originalDate = DateTime.Today;

            var repeatedToDos = new List<ToDo>
            {
                new ToDo("Daily Task", TimeBlock.Day, Difficulty.Easy, originalDate, Guid.NewGuid(), userId, null, null, Guid.Empty, RepeatFrequency.Daily),
                new ToDo("Weekly Task", TimeBlock.Day, Difficulty.Medium, originalDate, Guid.NewGuid(), userId, null, null, Guid.Empty, RepeatFrequency.Weekly),
                new ToDo("Monthly Task", TimeBlock.Day, Difficulty.Hard, originalDate, Guid.NewGuid(), userId, null, null, Guid.Empty, RepeatFrequency.Monthly),
                new ToDo("Yearly Task", TimeBlock.Day, Difficulty.Nightmare, originalDate, Guid.NewGuid(), userId, null, null, Guid.Empty, RepeatFrequency.Yearly),
                new ToDo("Not repeated task", TimeBlock.Day, Difficulty.Nightmare, originalDate, Guid.NewGuid(), userId, null, null, Guid.Empty, RepeatFrequency.None)
            };

            _toDoRepositoryMock.Setup(repo => repo.GetRepeatedToDosAsync(userId)).ReturnsAsync(repeatedToDos);
            _toDoRepositoryMock.Setup(repo => repo.AddToDoAsync(It.IsAny<ToDo>())).Returns(Task.CompletedTask);
            _toDoRepositoryMock.Setup(repo => repo.UpdateToDoAsync(It.IsAny<ToDo>())).Returns(Task.CompletedTask);

            await _toDoService.MoveRepeatedToDosAsync(userId);

            _toDoRepositoryMock.Verify(repo => repo.GetRepeatedToDosAsync(userId), Times.Once);

            _toDoRepositoryMock.Verify(repo => repo.AddToDoAsync(It.Is<ToDo>(t => t.RepeatFrequency == RepeatFrequency.Daily && t.ToDoDate == originalDate.AddDays(1))), Times.Once);
            _toDoRepositoryMock.Verify(repo => repo.AddToDoAsync(It.Is<ToDo>(t => t.RepeatFrequency == RepeatFrequency.Weekly && t.ToDoDate == originalDate.AddDays(7))), Times.Once);
            _toDoRepositoryMock.Verify(repo => repo.AddToDoAsync(It.Is<ToDo>(t => t.RepeatFrequency == RepeatFrequency.Monthly && t.ToDoDate == originalDate.AddMonths(1))), Times.Once);
            _toDoRepositoryMock.Verify(repo => repo.AddToDoAsync(It.Is<ToDo>(t => t.RepeatFrequency == RepeatFrequency.Yearly && t.ToDoDate == originalDate.AddYears(1))), Times.Once);
            _toDoRepositoryMock.Verify(repo => repo.AddToDoAsync(It.Is<ToDo>(t => t.RepeatFrequency == RepeatFrequency.None && t.ToDoDate == originalDate)), Times.Once);

            _toDoRepositoryMock.Verify(repo => repo.UpdateToDoAsync(It.Is<ToDo>(t => t.RepeatFrequency == RepeatFrequency.Daily && t.ToDoDate <= originalDate.AddDays(1))), Times.Once);
            _toDoRepositoryMock.Verify(repo => repo.UpdateToDoAsync(It.Is<ToDo>(t => t.RepeatFrequency == RepeatFrequency.Weekly && t.ToDoDate <= originalDate.AddDays(7))), Times.Once);
            _toDoRepositoryMock.Verify(repo => repo.UpdateToDoAsync(It.Is<ToDo>(t => t.RepeatFrequency == RepeatFrequency.Monthly && t.ToDoDate <= originalDate.AddMonths(1))), Times.Once);
            _toDoRepositoryMock.Verify(repo => repo.UpdateToDoAsync(It.Is<ToDo>(t => t.RepeatFrequency == RepeatFrequency.Yearly && t.ToDoDate <= originalDate.AddYears(1))), Times.Once);
            _toDoRepositoryMock.Verify(repo => repo.UpdateToDoAsync(It.Is<ToDo>(t => t.RepeatFrequency == RepeatFrequency.None && t.ToDoDate <= originalDate)), Times.Once);
        }

        #endregion
    }
}

