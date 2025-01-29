using API.Controllers;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.UnitTests.APITests
{
    public class ToDosControllerTests
    {
        private readonly Mock<IToDoService> _toDoServiceMock;
        private readonly ToDosController _controller;

        public ToDosControllerTests()
        {
            _toDoServiceMock = new Mock<IToDoService>();
            _controller = new ToDosController(_toDoServiceMock.Object);
        }

        #region GetToDoById(Guid toDoId) tests

        [Fact]
        public async Task GetToDoById_ShouldReturnToDo_WhenExists()
        {
            var toDo = new ToDo("Description", TimeBlock.Day, Difficulty.Easy, DateTime.Today, "SomeCategory", Guid.NewGuid());
            var toDoId = toDo.ToDoId;

            _toDoServiceMock.Setup(s => s.GetToDoByIdAsync(toDoId)).ReturnsAsync(toDo);

            var result = await _controller.GetToDoById(toDoId);

            var actionResult = Assert.IsType<ActionResult<ToDo>>(result);
            Assert.Equal(toDo, actionResult.Value);
        }

        #endregion

        #region GetToDos(Guid userId, DateTime date, TimeBlock timeBlock) tests

        [Fact]
        public async Task GetToDos_ShouldReturnListOfToDos()
        {
            var userId = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var timeBlock = TimeBlock.Day;
            var toDos = new List<ToDo> { new ToDo("TestToDo", timeBlock, Difficulty.Easy, date, "SomeCategory", userId) };

            _toDoServiceMock.Setup(s => s.GetToDosAsync(userId, date, timeBlock)).ReturnsAsync(toDos);

            var result = await _controller.GetToDos(date, timeBlock);

            var actionResult = Assert.IsType<ActionResult<List<ToDo>>>(result);
            Assert.Equal(toDos, actionResult.Value);
        }

        #endregion

        #region AddToDo(ToDo toDo) tests

        [Fact]
        public async Task AddToDo_ShouldReturnCreatedAtAction_WhenValid()
        {
            var toDo = new ToDo("Description", TimeBlock.Day, Difficulty.Easy, DateTime.Today, "SomeCategory", Guid.NewGuid());

            _toDoServiceMock.Setup(s => s.AddToDoAsync(toDo)).Returns(Task.CompletedTask);

            var result = await _controller.AddToDo(toDo);

            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(ToDosController.GetToDoById), actionResult.ActionName);
        }

        [Fact]
        public async Task AddToDo_ShouldReturnBadRequest_WhenToDoIsNull()
        {
            var result = await _controller.AddToDo(null);

            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("ToDo data cannot be null.", actionResult.Value);
        }

        #endregion 

        #region UpdateToDo(ToDo toDo) tests

        [Fact]
        public async Task UpdateToDo_ShouldReturnNoContent_WhenValid()
        {
            var toDo = new ToDo("Description", TimeBlock.Day, Difficulty.Easy, DateTime.Today, "SomeCategory", Guid.NewGuid());

            _toDoServiceMock.Setup(s => s.UpdateToDoAsync(toDo)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateToDo(toDo);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateToDo_ShouldReturnBadRequest_WhenToDoIsNull()
        {
            var result = await _controller.UpdateToDo(null);

            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("ToDo data cannot be null.", actionResult.Value);
        }


        #endregion

        #region DeleteToDo(Guid toDoId) tests

        [Fact]
        public async Task DeleteToDo_ShouldReturnNoContent_WhenSuccessful()
        {
            var toDoId = Guid.NewGuid();

            _toDoServiceMock.Setup(s => s.DeleteToDoAsync(toDoId)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteToDo(toDoId);

            Assert.IsType<NoContentResult>(result);
        }

        #endregion
    }
}
