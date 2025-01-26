using Core.Interfaces;
using Core.Models;
using Core.Services;
using Moq;

namespace Tests.UnitTests.ServicesTests
{
    public class ToDoServiceTests
    {
        private readonly Mock<IToDoRepository> toDoRepositoryMock;

        public ToDoServiceTests()
        {
            toDoRepositoryMock = new Mock<IToDoRepository>();
        }

        #region GetToDoByIdAsync(Guid toDoId) tests

        [Fact]
        public async Task GetTodoByIdAsync_ShouldReturnCorrectToDo_WhenToDoIdExists()
        {
            var expectedToDo = new ToDo("testDescription", TimeBlock.Day, Difficulty.Easy, DateTime.Today, "Other", Guid.NewGuid(), DateTime.Today.AddDays(1), Guid.NewGuid(), RepeatFrequency.Daily);
            var toDoId = expectedToDo.ToDoId;

            toDoRepositoryMock.Setup(repo => repo.GetToDoByIdAsync(toDoId)).ReturnsAsync(expectedToDo);

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            var result = await toDoService.GetToDoByIdAsync(toDoId);

            Assert.NotNull(result);
            Assert.Equal(expectedToDo.ToDoId, result.ToDoId);
            Assert.Equal(expectedToDo.Description, result.Description);
            toDoRepositoryMock.Verify(repo => repo.GetToDoByIdAsync(toDoId), Times.Once);
        }

        [Fact]
        public async Task GetToDoByIdAsync_ShouldThrowException_WhenToDoIdDoesNotExist()
        {
            var toDoId = Guid.NewGuid();

            toDoRepositoryMock.Setup(repo => repo.GetToDoByIdAsync(toDoId)).ReturnsAsync((ToDo?)null);

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => toDoService.GetToDoByIdAsync(toDoId));

            Assert.Equal("Todo id was not found.", exception.Message);
        }

        [Fact]
        public async Task GetToDoByIdAsync_ShouldThrowException_WhenToDoIdIsEmpty()
        {
            var toDoId = new Guid();

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => toDoService.GetToDoByIdAsync(toDoId));

            Assert.Equal("ToDo id cannot be empty.", exception.Message);
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
                new ToDo("testDescription", TimeBlock.Day, Difficulty.Easy, date, "Other", userId, DateTime.Today.AddDays(1), Guid.NewGuid(), RepeatFrequency.Daily),
                new ToDo("testDescription", TimeBlock.Day, Difficulty.Easy, date, "Other", userId, DateTime.Today.AddDays(1), Guid.NewGuid(), RepeatFrequency.Daily)
            };

            toDoRepositoryMock.Setup(repo => repo.GetToDosAsync(userId, date, TimeBlock.Day)).ReturnsAsync(expectedToDos);

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            var result = await toDoService.GetToDosAsync(userId, date, TimeBlock.Day);

            Assert.NotNull(result);
            Assert.Equal(expectedToDos.Count, result.Count);
            toDoRepositoryMock.Verify(repo => repo.GetToDosAsync(userId, date, TimeBlock.Day), Times.Once);
        }

        [Fact]
        public async Task GetToDosAsync_ShouldReturnEmptyList_WhenNoToDosForDate()
        {
            var userId = Guid.NewGuid();
            DateTime date = DateTime.Today;

            toDoRepositoryMock.Setup(repo => repo.GetToDosAsync(userId, date, TimeBlock.Day)).ReturnsAsync(new List<ToDo>());

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            var result = await toDoService.GetToDosAsync(userId, date, TimeBlock.Day);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetToDosAsync_ShouldThrowException_WhenToDoIdIsEmpty()
        {
            var userId = new Guid();

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => toDoService.GetToDosAsync(userId, DateTime.Today, TimeBlock.Day));

            Assert.Equal("User id cannot be empty.", exception.Message);
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
            string toDoCategoryName = "Other";
            var userId = Guid.NewGuid();
            DateTime deadline = DateTime.Today.AddDays(1);
            var parentToDoId = Guid.NewGuid();
            RepeatFrequency repeatFrequency = RepeatFrequency.Daily;
            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryName, userId, deadline, parentToDoId, repeatFrequency);

            toDoRepositoryMock.Setup(repo => repo.ToDoExistsAsync(toDo.ToDoId)).ReturnsAsync(false);
            toDoRepositoryMock.Setup(repo => repo.AddToDoAsync(It.IsAny<ToDo>())).Returns(Task.CompletedTask);

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            await toDoService.AddToDoAsync(toDo);

            toDoRepositoryMock.Verify(repo => repo.ToDoExistsAsync(toDo.ToDoId), Times.Once);
            toDoRepositoryMock.Verify(repo => repo.AddToDoAsync(It.Is<ToDo>(t => t.ToDoId == toDo.ToDoId
            && t.Description == toDo.Description
            && t.TimeBlock == toDo.TimeBlock
            && t.Difficulty == toDo.Difficulty
            && t.ToDoDate == toDo.ToDoDate
            && t.ToDoCategoryName == toDo.ToDoCategoryName
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
            string toDoCategoryName = "Other";
            var userId = Guid.NewGuid();
            DateTime deadline = DateTime.Today.AddDays(1);
            var parentToDoId = Guid.NewGuid();
            RepeatFrequency repeatFrequency = RepeatFrequency.Daily;
            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryName, userId, deadline, parentToDoId, repeatFrequency);

            toDoRepositoryMock.Setup(repo => repo.ToDoExistsAsync(toDo.ToDoId)).ReturnsAsync(true);
            toDoRepositoryMock.Setup(repo => repo.AddToDoAsync(toDo)).Returns(Task.CompletedTask);

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => toDoService.AddToDoAsync(toDo));

            Assert.Equal("ToDo id already exists.", exception.Message);
            toDoRepositoryMock.Verify(repo => repo.ToDoExistsAsync(toDo.ToDoId), Times.Once);
            toDoRepositoryMock.Verify(repo => repo.AddToDoAsync(toDo), Times.Never);
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
            string toDoCategoryName = "Other";
            var userId = Guid.NewGuid();
            DateTime deadline = DateTime.Today.AddDays(1);
            var parentToDoId = Guid.NewGuid();
            RepeatFrequency repeatFrequency = RepeatFrequency.Daily;
            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryName, userId, deadline, parentToDoId, repeatFrequency);

            toDoRepositoryMock.Setup(repo => repo.ToDoExistsAsync(toDo.ToDoId)).ReturnsAsync(true);
            toDoRepositoryMock.Setup(repo => repo.UpdateToDoAsync(It.IsAny<ToDo>())).Returns(Task.CompletedTask);

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            await toDoService.UpdateToDoAsync(toDo);

            toDoRepositoryMock.Verify(repo => repo.ToDoExistsAsync(toDo.ToDoId), Times.Once);
            toDoRepositoryMock.Verify(repo => repo.UpdateToDoAsync(It.Is<ToDo>(t => t.ToDoId == toDo.ToDoId
            && t.Description == toDo.Description
            && t.TimeBlock == toDo.TimeBlock
            && t.Difficulty == toDo.Difficulty
            && t.ToDoDate == toDo.ToDoDate
            && t.ToDoCategoryName == toDo.ToDoCategoryName
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
            string toDoCategoryName = "Other";
            var userId = Guid.NewGuid();
            DateTime deadline = DateTime.Today.AddDays(1);
            var parentToDoId = Guid.NewGuid();
            RepeatFrequency repeatFrequency = RepeatFrequency.Daily;
            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryName, userId, deadline, parentToDoId, repeatFrequency);

            toDoRepositoryMock.Setup(repo => repo.ToDoExistsAsync(toDo.ToDoId)).ReturnsAsync(false);

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => toDoService.UpdateToDoAsync(toDo));

            Assert.Equal("Todo id does not exist.", exception.Message);
        }

        #endregion

        #region DeleteToDoAsync(Guid toDoId) tests

        [Fact]
        public async Task DeleteToDoAsync_ShouldDeleteToDo_WhenToDoIdExists()
        {
            var toDoId = Guid.NewGuid();

            toDoRepositoryMock.Setup(repo => repo.ToDoExistsAsync(toDoId)).ReturnsAsync(true);
            toDoRepositoryMock.Setup(repo => repo.DeleteToDoAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            await toDoService.DeleteToDoAsync(toDoId);

            toDoRepositoryMock.Verify(repo => repo.ToDoExistsAsync(toDoId), Times.Once);
            toDoRepositoryMock.Verify(repo => repo.DeleteToDoAsync(It.Is<Guid>(c => c == toDoId)), Times.Once);
        }

        [Fact]
        public async Task DeleteToDoAsync_ShouldThrowException_WhenToDoIdDoesNotExist()
        {
            var toDoId = Guid.NewGuid();

            toDoRepositoryMock.Setup(repo => repo.ToDoExistsAsync(toDoId)).ReturnsAsync(false);
            toDoRepositoryMock.Setup(repo => repo.DeleteToDoAsync(toDoId)).Returns(Task.CompletedTask);

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => toDoService.DeleteToDoAsync(toDoId));

            Assert.Equal("Todo id does not exist.", exception.Message);
            toDoRepositoryMock.Verify(repo => repo.DeleteToDoAsync(toDoId), Times.Never);
        }

        [Fact]
        public async Task DeleteToDoAsync_ShouldThrowException_WhenToDoIdIsEmpty()
        {
            var toDoId = new Guid();

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => toDoService.DeleteToDoAsync(toDoId));

            Assert.Equal("ToDo id cannot be empty.", exception.Message);
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
                new ToDo("Daily Task", TimeBlock.Day, Difficulty.Easy, originalDate, "Other", Guid.NewGuid(), null, userId, RepeatFrequency.Daily),
                new ToDo("Weekly Task", TimeBlock.Day, Difficulty.Medium, originalDate, "Other", Guid.NewGuid(), null, userId, RepeatFrequency.Weekly),
                new ToDo("Monthly Task", TimeBlock.Day, Difficulty.Hard, originalDate, "Other", Guid.NewGuid(), null, userId, RepeatFrequency.Monthly),
                new ToDo("Yearly Task", TimeBlock.Day, Difficulty.Nightmare, originalDate, "Other", Guid.NewGuid(), null, userId, RepeatFrequency.Yearly),
                new ToDo("Not repeated task", TimeBlock.Day, Difficulty.Nightmare, originalDate, "Other", Guid.NewGuid(), null, userId, RepeatFrequency.None)
            };

            toDoRepositoryMock.Setup(repo => repo.GetToDosAsync(userId, DateTime.Today, TimeBlock.Day)).ReturnsAsync(repeatedToDos);
            toDoRepositoryMock.Setup(repo => repo.UpdateToDoAsync(It.IsAny<ToDo>())).Returns(Task.CompletedTask);

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            await toDoService.MoveRepeatedToDosAsync(userId);

            toDoRepositoryMock.Verify(repo => repo.GetToDosAsync(userId, DateTime.Today, TimeBlock.Day), Times.Once);

            toDoRepositoryMock.Verify(repo => repo.UpdateToDoAsync(It.Is<ToDo>(t => t.RepeatFrequency == RepeatFrequency.Daily && t.ToDoDate == originalDate.AddDays(1))), Times.Once);

            toDoRepositoryMock.Verify(repo => repo.UpdateToDoAsync(It.Is<ToDo>(t => t.RepeatFrequency == RepeatFrequency.Weekly && t.ToDoDate == originalDate.AddDays(7))), Times.Once);

            toDoRepositoryMock.Verify(repo => repo.UpdateToDoAsync(It.Is<ToDo>(t => t.RepeatFrequency == RepeatFrequency.Monthly && t.ToDoDate == originalDate.AddMonths(1))), Times.Once);

            toDoRepositoryMock.Verify(repo => repo.UpdateToDoAsync(It.Is<ToDo>(t => t.RepeatFrequency == RepeatFrequency.Yearly && t.ToDoDate == originalDate.AddYears(1))), Times.Once);

            toDoRepositoryMock.Verify(repo => repo.UpdateToDoAsync(It.Is<ToDo>(t => t.RepeatFrequency == RepeatFrequency.None && t.ToDoDate == originalDate)), Times.Once);
        }

        [Fact]
        public async Task MoveRepeatedToDosAsync_ShouldThrowException_WhenToDoIdIsEmpty()
        {
            var userId = new Guid();

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => toDoService.MoveRepeatedToDosAsync(userId));

            Assert.Equal("User id cannot be empty.", exception.Message);
        }

        #endregion
    }
}

