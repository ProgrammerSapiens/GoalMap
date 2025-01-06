using Core.Interfaces;
using Core.Models;
using Core.Services;
using Moq;

namespace Tests.UnitTests.ServicesTests
{
    public class ToDoServiceTests
    {
        #region GetToDoByIdAsync(Guid toDoId) tests

        [Fact]
        public async Task GetTodoByIdAsync_ShouldReturnCorrectToDo_WhenToDoIdExists()
        {
            var expectedToDo = new ToDo("testDescription", TimeBlock.Day, Difficulty.Easy, DateTime.Today, Guid.NewGuid(), Guid.NewGuid(), DateTime.Today.AddDays(1), Guid.NewGuid(), RepeatFrequency.Daily);

            var toDoId = expectedToDo.ToDoId;

            var toDoRepositoryMock = new Mock<IToDoRepository>();
            toDoRepositoryMock.Setup(repo => repo.GetToDoByIdAsync(toDoId)).ReturnsAsync(expectedToDo);

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            var result = await toDoService.GetToDoByIdAsync(toDoId);

            Assert.NotNull(result);
            Assert.Equal(expectedToDo.ToDoId, result.ToDoId);
            Assert.Equal(expectedToDo.Description, result.Description);
            Assert.Equal(expectedToDo.TimeBlock, result.TimeBlock);
            Assert.Equal(expectedToDo.Difficulty, result.Difficulty);
            Assert.Equal(expectedToDo.ParentToDoId, result.ParentToDoId);
            Assert.Equal(expectedToDo.ToDoCategoryId, result.ToDoCategoryId);
            Assert.Equal(expectedToDo.UserId, result.UserId);
            Assert.Equal(expectedToDo.RepeatFrequency, result.RepeatFrequency);
            Assert.Equal(expectedToDo.ToDoDate, result.ToDoDate);
            Assert.Equal(expectedToDo.Deadline, result.Deadline);
        }

        [Fact]
        public async Task GetToDoByIdAsync_ShouldThrowException_WhenToDoIdDoesNotExist()
        {
            var toDoId = Guid.NewGuid();

            var toDoRepositoryMock = new Mock<IToDoRepository>();
            toDoRepositoryMock.Setup(repo => repo.GetToDoByIdAsync(toDoId)).ReturnsAsync((ToDo?)null);

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => toDoService.GetToDoByIdAsync(toDoId));

            Assert.Equal("The Todo was not found.", exception.Message);
        }

        [Fact]
        public async Task GetToDoByIdAsync_ShouldThrowException_WhenToDoIdIsEmpty()
        {
            var toDoId = new Guid();

            var toDoRepositoryMock = new Mock<IToDoRepository>();
            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => toDoService.GetToDoByIdAsync(toDoId));
            Assert.Equal("The todo Id cannot be empty.", exception.Message);
        }

        #endregion

        #region GetToDosByUserAsync(Guid userId) tests

        [Fact]
        public async Task GetToDosByUserIdAsync_ShouldReturnToDoList_WhenToDoExist()
        {
            var userId = Guid.NewGuid();

            var expectedToDos = new List<ToDo>()
            {
                new ToDo("testDescription", TimeBlock.Day, Difficulty.Easy, DateTime.Today, Guid.NewGuid(), Guid.NewGuid(), DateTime.Today.AddDays(1), Guid.NewGuid(), RepeatFrequency.Daily),
                new ToDo("testDescription2", TimeBlock.Day, Difficulty.Easy, DateTime.Today, Guid.NewGuid(), Guid.NewGuid(), DateTime.Today.AddDays(1), Guid.NewGuid(), RepeatFrequency.Daily)
            };

            var toDoRepositoryMock = new Mock<IToDoRepository>();
            toDoRepositoryMock.Setup(repo => repo.GetToDosByUserIdAsync(userId)).ReturnsAsync(expectedToDos);

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            var result = await toDoService.GetToDosByUserIdAsync(userId);

            Assert.NotNull(result);
            Assert.Equal(expectedToDos.Count, result.Count);

            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal(expectedToDos[i].ToDoId, result[i].ToDoId);
                Assert.Equal(expectedToDos[i].Description, result[i].Description);
                Assert.Equal(expectedToDos[i].TimeBlock, result[i].TimeBlock);
                Assert.Equal(expectedToDos[i].Difficulty, result[i].Difficulty);
                Assert.Equal(expectedToDos[i].Deadline, result[i].Deadline);
                Assert.Equal(expectedToDos[i].UserId, result[i].UserId);
                Assert.Equal(expectedToDos[i].RepeatFrequency, result[i].RepeatFrequency);
            }
        }

        [Fact]
        public async Task GetToDosByUserIdAsync_ShouldReturnEmptyList_WhenNoToDosExist()
        {
            var userId = Guid.NewGuid();

            var toDoRepositoryMock = new Mock<IToDoRepository>();
            toDoRepositoryMock.Setup(repo => repo.GetToDosByUserIdAsync(userId)).ReturnsAsync(new List<ToDo>());

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            var result = await toDoService.GetToDosByUserIdAsync(userId);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetToDosByUserIdAsync_ShouldThrowException_WhenUserIdDoesNotExist()
        {
            var userId = Guid.NewGuid();

            var toDoRepositoryMock = new Mock<IToDoRepository>();
            toDoRepositoryMock.Setup(repo => repo.GetToDosByUserIdAsync(userId)).ReturnsAsync(new List<ToDo>());

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => toDoService.GetToDosByUserIdAsync(userId));

            Assert.Equal("This id does not exist.", exception.Message);
        }

        #endregion

        #region GetTodosByDateAsync(Guid userId, DateTime date) tests

        [Fact]
        public async Task GetToDosByDateAsync_ShouldReturnToDosForGivenDate_WhenToDosExist()
        {
            var userId = Guid.NewGuid();
            DateTime date = DateTime.Today;

            var expectedToDos = new List<ToDo>()
            {
                new ToDo("testDescription", TimeBlock.Day, Difficulty.Easy, date, Guid.NewGuid(), Guid.NewGuid(), DateTime.Today.AddDays(1), Guid.NewGuid(), RepeatFrequency.Daily),
                new ToDo("testDescription2", TimeBlock.Day, Difficulty.Easy, date, Guid.NewGuid(), Guid.NewGuid(), DateTime.Today.AddDays(1), Guid.NewGuid(), RepeatFrequency.Daily)
            };

            var toDoRepositoryMock = new Mock<IToDoRepository>();
            toDoRepositoryMock.Setup(repo => repo.GetToDosByDateAsync(userId, date)).ReturnsAsync(expectedToDos);

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            var result = await toDoService.GetToDosByDateAsync(userId, date);

            Assert.NotNull(result);
            Assert.Equal(expectedToDos.Count, result.Count);

            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal(expectedToDos[i].ToDoId, result[i].ToDoId);
                Assert.Equal(expectedToDos[i].Description, result[i].Description);
                Assert.Equal(expectedToDos[i].TimeBlock, result[i].TimeBlock);
                Assert.Equal(expectedToDos[i].Difficulty, result[i].Difficulty);
                Assert.Equal(expectedToDos[i].Deadline, result[i].Deadline);
                Assert.Equal(expectedToDos[i].UserId, result[i].UserId);
                Assert.Equal(expectedToDos[i].RepeatFrequency, result[i].RepeatFrequency);
            }
        }

        [Fact]
        public async Task GetToDosByDateAsync_ShouldReturnEmptyList_WhenNoToDosForDate()
        {
            var userId = Guid.NewGuid();
            DateTime date = DateTime.Today;

            var toDoRepositoryMock = new Mock<IToDoRepository>();
            toDoRepositoryMock.Setup(repo => repo.GetToDosByDateAsync(userId, date)).ReturnsAsync(new List<ToDo>());

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            var result = await toDoService.GetToDosByDateAsync(userId, date);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetToDosByDateAsync_ShouldThrowException_WhenUserIdDoesNotExist()
        {
            var userId = Guid.NewGuid();
            DateTime date = DateTime.Today;

            var toDoRepositoryMock = new Mock<IToDoRepository>();
            toDoRepositoryMock.Setup(repo => repo.GetToDosByDateAsync(userId, date)).ReturnsAsync(new List<ToDo>());

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            var result = await toDoService.GetToDosByDateAsync(userId, date);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region GetToDosByTimeBlockAsync(Guid userId, TimeBlock timeBlock) tests

        [Fact]
        public async Task GetToDosByTimeBlockAsync_ShoulReturnToDosForGivenTimeBlock_WhenToDosExist()
        {
            var userId = Guid.NewGuid();
            TimeBlock timeBlock = TimeBlock.Day;

            var expectedToDos = new List<ToDo>()
            {
                new ToDo("testDescription", timeBlock, Difficulty.Easy, DateTime.Today, Guid.NewGuid(), Guid.NewGuid(), DateTime.Today.AddDays(1), Guid.NewGuid(), RepeatFrequency.Daily),
                new ToDo("testDescription2", timeBlock, Difficulty.Easy, DateTime.Today, Guid.NewGuid(), Guid.NewGuid(), DateTime.Today.AddDays(1), Guid.NewGuid(), RepeatFrequency.Daily)
            };

            var toDoRepositoryMock = new Mock<IToDoRepository>();
            toDoRepositoryMock.Setup(repo => repo.GetToDosByTimeBlockAsync(userId, timeBlock)).ReturnsAsync(expectedToDos);

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            var result = await toDoService.GetToDosByTimeBlockAsync(userId, timeBlock);

            Assert.NotNull(result);
            Assert.Equal(expectedToDos.Count, result.Count);

            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal(expectedToDos[i].ToDoId, result[i].ToDoId);
                Assert.Equal(expectedToDos[i].Description, result[i].Description);
                Assert.Equal(expectedToDos[i].TimeBlock, result[i].TimeBlock);
                Assert.Equal(expectedToDos[i].Difficulty, result[i].Difficulty);
                Assert.Equal(expectedToDos[i].Deadline, result[i].Deadline);
                Assert.Equal(expectedToDos[i].UserId, result[i].UserId);
                Assert.Equal(expectedToDos[i].RepeatFrequency, result[i].RepeatFrequency);
            }
        }

        [Fact]
        public async Task GetToDosByTimeBlockAsync_ShouldReturnEmptyList_WhenNoToDosForTimeBlock()
        {
            var userId = Guid.NewGuid();
            TimeBlock timeBlock = TimeBlock.Day;

            var toDoRepositoryMock = new Mock<IToDoRepository>();
            toDoRepositoryMock.Setup(repo => repo.GetToDosByTimeBlockAsync(userId, timeBlock)).ReturnsAsync(new List<ToDo>());

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            var result = await toDoService.GetToDosByTimeBlockAsync(userId, timeBlock);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetToDosByTimeBlockAsync_ShouldThrowException_WhenTimeBlockIsInvalid()
        {
            var userId = Guid.NewGuid();
            TimeBlock timeBlock = TimeBlock.Day;

            var toDoRepositoryMock = new Mock<IToDoRepository>();
            toDoRepositoryMock.Setup(repo => repo.GetToDosByTimeBlockAsync(userId, timeBlock)).ReturnsAsync(new List<ToDo>());

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            var result = await toDoService.GetToDosByTimeBlockAsync(userId, timeBlock);

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
            var toDoCategoryId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            DateTime deadline = DateTime.Today.AddDays(1);
            var parentToDoId = Guid.NewGuid();
            RepeatFrequency repeatFrequency = RepeatFrequency.Daily;
            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId, deadline, parentToDoId, repeatFrequency);

            var toDoRepositoryMock = new Mock<IToDoRepository>();
            toDoRepositoryMock.Setup(repo => repo.AddToDoAsync(It.IsAny<ToDo>())).Returns(Task.CompletedTask);

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            await toDoService.AddToDoAsync(toDo);

            toDoRepositoryMock.Verify(repo => repo.AddToDoAsync(It.Is<ToDo>(t => t.ToDoId == toDo.ToDoId
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
        public async Task AddToDoAsync_ShouldThrowException_WhenToDoIsInvalid()
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
            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId, deadline, parentToDoId, repeatFrequency);

            var toDoRepositoryMock = new Mock<IToDoRepository>();
            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => toDoService.AddToDoAsync(toDo));

            Assert.Equal("The todo description cannot be null or empty.", exception.Message);

            toDoRepositoryMock.Verify(repo => repo.AddToDoAsync(It.IsAny<ToDo>()), Times.Never);
        }

        [Fact]
        public async Task AddToDoAsync_ShouldThrowException_WhenToDoIdNull()
        {
            var toDoRepositoryMock = new Mock<IToDoRepository>();
            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => toDoService.AddToDoAsync(null));

            Assert.Equal("The todo cannot be null. (Parameter 'toDo')", exception.Message);
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
            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId, deadline, parentToDoId, repeatFrequency);

            var toDoRepositoryMock = new Mock<IToDoRepository>();
            toDoRepositoryMock.Setup(repo => repo.UpdateToDoAsync(It.IsAny<ToDo>())).Returns(Task.CompletedTask);

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            await toDoService.UpdateToDoAsync(toDo);

            toDoRepositoryMock.Verify(repo => repo.UpdateToDoAsync(It.Is<ToDo>(t => t.ToDoId == toDo.ToDoId
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
            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId, deadline, parentToDoId, repeatFrequency);

            var toDoRepositoryMock = new Mock<IToDoRepository>();
            toDoRepositoryMock.Setup(repo => repo.GetToDoByIdAsync(It.IsAny<Guid>())).ReturnsAsync((ToDo?)null);

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => toDoService.UpdateToDoAsync(toDo));

            Assert.Equal("The todo does not exist.", exception.Message);
        }

        [Fact]
        public async Task UpdateToDoAsync_ShouldThrowException_WhenToDoIsNull()
        {
            var toDoRepositoryMock = new Mock<IToDoRepository>();
            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => toDoService.UpdateToDoAsync(null));

            Assert.Equal("The todo cannot be null. (Parameter 'toDo')", exception.Message);
        }

        #endregion

        #region DeleteToDoAsync(Guid toDoId) tests

        [Fact]
        public async Task DeleteToDoAsync_ShouldDeleteToDo_WhenToDoIdExists()
        {
            var toDoId = Guid.NewGuid();

            var toDoRepositoryMock = new Mock<IToDoRepository>();
            toDoRepositoryMock.Setup(repo => repo.DeleteToDoAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            await toDoService.DeleteToDoAsync(toDoId);

            toDoRepositoryMock.Verify(repo => repo.DeleteToDoAsync(It.Is<Guid>(c => c == toDoId)), Times.Once);
        }

        [Fact]
        public async Task DeleteToDoAsync_ShouldThrowException_WhenToDoIdDoesNotExist()
        {
            var toDoId = Guid.NewGuid();

            var toDoRepositoryMock = new Mock<IToDoRepository>();
            toDoRepositoryMock.Setup(repo => repo.GetToDoByIdAsync(toDoId)).ReturnsAsync((ToDo?)null);

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => toDoService.DeleteToDoAsync(toDoId));

            Assert.Equal("The todo does not exist.", exception.Message);
        }

        [Fact]
        public async Task DeleteToDoAsync_ShouldThrowException_WhenToDoIdIsEmpty()
        {
            var toDoRepositoryMock = new Mock<IToDoRepository>();
            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => toDoService.DeleteToDoAsync(new Guid()));

            Assert.Equal("Todo id cannot be null. (Parameter 'toDoId')", exception.Message);
        }

        #endregion

        #region MoveRepeatedToDosAsync(RepeatFrequency repeatFrequency, Guid userId) tests

        [Theory]
        [InlineData(RepeatFrequency.Daily, 1)]
        [InlineData(RepeatFrequency.Weekly, 7)]
        [InlineData(RepeatFrequency.Monthly, 31)]
        [InlineData(RepeatFrequency.Yearly, 365)]
        public async Task MoveRepeatedToDosAsync_ShouldMoveToDos_WhenFrequencyAndUserIdAreValid(RepeatFrequency repeatFrequency, int days)
        {
            var userId = Guid.NewGuid();
            var originalDate = DateTime.Today;
            var newDate = originalDate.AddDays(days);

            var repeatedToDos = new List<ToDo>
            {
                new ToDo("Test 1", TimeBlock.Day, Difficulty.Easy, originalDate, Guid.NewGuid(), Guid.NewGuid(), originalDate.AddDays(2), userId, repeatFrequency),
                new ToDo("Test 2", TimeBlock.Day, Difficulty.Hard, originalDate, Guid.NewGuid(), Guid.NewGuid(), originalDate.AddDays(3), userId, repeatFrequency)
            };

            var toDoRepositoryMock = new Mock<IToDoRepository>();
            toDoRepositoryMock.Setup(repo => repo.MoveRepeatedToDosAsync(repeatFrequency, userId)).Returns(Task.CompletedTask);
            toDoRepositoryMock.Setup(repo => repo.UpdateToDoAsync(It.IsAny<ToDo>())).Returns(Task.CompletedTask);

            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            await toDoService.MoveRepeatedToDosAsync(repeatFrequency, userId);

            toDoRepositoryMock.Verify(repo => repo.MoveRepeatedToDosAsync(repeatFrequency, userId), Times.Once);

            foreach (var toDo in repeatedToDos)
            {
                toDoRepositoryMock.Verify(repo => repo.UpdateToDoAsync(It.Is<ToDo>(
                    t => t.ToDoId == toDo.ToDoId &&
                         t.ToDoDate == newDate)), Times.Once);
            }
        }

        [Fact]
        public async Task MoveRepeatedToDosAsync_ShouldHandleNoRepeatedToDos()
        {
            var invalidUserId = Guid.Empty;

            var toDoRepositoryMock = new Mock<IToDoRepository>();
            var toDoService = new ToDoService(toDoRepositoryMock.Object);

            var userIdException = await Assert.ThrowsAsync<ArgumentException>(() => toDoService.MoveRepeatedToDosAsync(RepeatFrequency.Daily, invalidUserId));
            Assert.Equal("The user ID cannot be empty.", userIdException.Message);
        }

        #endregion
    }
}

