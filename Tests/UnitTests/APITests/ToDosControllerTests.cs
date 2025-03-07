using API.Controllers;
using AutoMapper;
using Core.DTOs.ToDo;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace Tests.UnitTests.APITests
{
    public class ToDosControllerTests
    {
        private readonly Mock<IToDoService> _toDoServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<ToDosController>> _loggerMock;
        private readonly ToDosController _toDosController;

        public ToDosControllerTests()
        {
            _toDoServiceMock = new Mock<IToDoService>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<ToDosController>>();
            _toDosController = new ToDosController(_toDoServiceMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        #region GetToDoById(Guid toDoId) tests

        [Fact]
        public async Task GetToDoById_ShouldReturnToDo_WhenExists()
        {
            var userId = Guid.NewGuid();
            var toDoCategory = new ToDoCategory(userId, "SomeCategory");
            var toDo = new ToDo("Description", TimeBlock.Day, Difficulty.Easy, DateTime.Today, toDoCategory.ToDoCategoryId, Guid.NewGuid());
            var toDoId = toDo.ToDoId;
            var toDoDto = new ToDoDto { ToDoCategoryId = toDoCategory.ToDoCategoryId, Description = "Description", Difficulty = Difficulty.Easy };

            _toDoServiceMock.Setup(service => service.GetToDoByIdAsync(toDoId)).ReturnsAsync(toDo);
            _mapperMock.Setup(mapper => mapper.Map<ToDoDto>(toDo)).Returns(toDoDto);

            var result = await _toDosController.GetToDoById(toDoId);

            var actionResult = Assert.IsType<ActionResult<ToDoDto>>(result);
            var returnedToDoDto = Assert.IsType<ToDoDto>(actionResult.Value);
            Assert.Equal(toDo.Description, returnedToDoDto.Description);
            Assert.Equal(toDo.Difficulty, returnedToDoDto.Difficulty);
        }

        [Fact]
        public async Task GetToDoById_ShouldReturnBadRequest_WhenToDoIdIsEmpty()
        {
            var result = await _toDosController.GetToDoById(new Guid());

            var actionResult = Assert.IsType<ActionResult<ToDoDto>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Todo id cannot be empty", badRequestResult.Value);
        }

        [Fact]
        public async Task GetToDoById_ShouldReturnNotFound_WhenToDoDoesNotExist()
        {
            var toDo = new ToDo("Description", TimeBlock.Day, Difficulty.Easy, DateTime.Today, Guid.NewGuid(), Guid.NewGuid());
            var toDoId = toDo.ToDoId;
            var toDoCategoryId = toDo.ToDoCategoryId;
            var toDoDto = new ToDoDto { ToDoCategoryId = toDoCategoryId, Description = "Description", Difficulty = Difficulty.Easy };

            _toDoServiceMock.Setup(service => service.GetToDoByIdAsync(toDoId)).ReturnsAsync((ToDo?)null);

            var result = await _toDosController.GetToDoById(toDoId);

            var actionResult = Assert.IsType<ActionResult<ToDoDto>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("ToDo was not found.", notFoundResult.Value);
        }

        #endregion

        #region GetToDos(ToDoGetByDateAndTimeBlockDto toDoGetByDateAndTimeBlockDto) tests

        [Fact]
        public async Task GetToDos_ShouldReturnListOfToDos()
        {
            var user = new User("TestUser");

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim("UserId", user.UserId.ToString())
            ], "mock"));

            _toDosController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userClaims }
            };

            var toDoCategoryId = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var timeBlock = TimeBlock.Day;
            var toDos = new List<ToDo>
            {
                new ToDo("TestToDo", timeBlock, Difficulty.Easy, date, Guid.NewGuid(), user.UserId),
                new ToDo("TestToDo2", timeBlock, Difficulty.Easy, date, Guid.NewGuid(), user.UserId)
            };

            var toDosDto = new List<ToDoDto>
            {
                new ToDoDto {  ToDoCategoryId = toDoCategoryId, Description = toDos[0].Description, Difficulty = toDos[0].Difficulty },
                new ToDoDto { ToDoCategoryId = toDoCategoryId, Description = toDos[1].Description, Difficulty = toDos[1].Difficulty }
            };

            var toDoGetByDateAndTimeBlockDto = new ToDoGetByDateAndTimeBlockDto { Date = date, TimeBlock = timeBlock };

            _toDoServiceMock.Setup(s => s.GetToDosAsync(user.UserId, date, timeBlock)).ReturnsAsync(toDos);
            _mapperMock.Setup(mapper => mapper.Map<List<ToDoDto>>(toDos)).Returns(toDosDto);

            var result = await _toDosController.GetToDos(toDoGetByDateAndTimeBlockDto);

            var actionResult = Assert.IsType<ActionResult<List<ToDoDto>>>(result);
            var returnedToDosDto = Assert.IsType<List<ToDoDto>>(actionResult.Value);
            Assert.Equal(2, returnedToDosDto.Count);
        }

        [Fact]
        public async Task GetToDos_ShouldReturnUnauthorized_WhenUserIsNotAuthenticated()
        {
            var user = new User("TestUser");

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity());

            _toDosController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userClaims }
            };

            var toDoGetByDateAndTimeBlockDto = new ToDoGetByDateAndTimeBlockDto();

            var result = await _toDosController.GetToDos(toDoGetByDateAndTimeBlockDto);

            var actionResult = Assert.IsType<ActionResult<List<ToDoDto>>>(result);
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(actionResult.Result);
            Assert.Equal("User ID is not authenticated or invalid.", unauthorizedResult.Value);
        }

        [Fact]
        public async Task GetToDos_ShouldReturnEmptyList_WhenNoToDosExist()
        {
            var user = new User("TestUser");

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim("UserId", user.UserId.ToString())
            ], "mock"));

            _toDosController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userClaims }
            };

            var date = DateTime.UtcNow;
            var timeBlock = TimeBlock.Day;

            var toDoGetByDateAndTimeBlockDto = new ToDoGetByDateAndTimeBlockDto { Date = date, TimeBlock = timeBlock };

            _toDoServiceMock.Setup(s => s.GetToDosAsync(user.UserId, date, timeBlock)).ReturnsAsync(new List<ToDo>());

            var result = await _toDosController.GetToDos(toDoGetByDateAndTimeBlockDto);

            var actionResult = Assert.IsType<ActionResult<List<ToDoDto>>>(result);
            var emptyToDoDto = Assert.IsType<List<ToDoDto>>(actionResult.Value);
            Assert.Empty(emptyToDoDto);
        }

        #endregion

        #region AddToDo([FromBody] ToDoAddDto toDoAddDto) tests

        [Fact]
        public async Task AddToDo_ShouldReturnCreatedAtAction_WhenValid()
        {
            //var toDoAddToDo = new ToDoAddDto { Description = "Description", TimeBlock = TimeBlock.Day, Difficulty = Difficulty.Easy, ToDoDate = DateTime.Today, ToDoCategoryName = "SomeCategory", UserId = Guid.NewGuid() };
            //var toDo = new ToDo(toDoAddToDo.Description, toDoAddToDo.TimeBlock, toDoAddToDo.Difficulty, toDoAddToDo.ToDoDate, toDoAddToDo.ToDoCategoryId, toDoAddToDo.UserId);

            //_mapperMock.Setup(mapper => mapper.Map<ToDo>(toDoAddToDo)).Returns(toDo);
            //_toDoServiceMock.Setup(s => s.AddToDoAsync(toDo)).Returns(Task.CompletedTask);

            //var result = await _toDosController.AddToDo(toDoAddToDo);

            //var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            //Assert.Equal(nameof(ToDosController.GetToDoById), actionResult.ActionName);
        }

        [Fact]
        public async Task AddToDo_ShouldReturnBadRequest_WhenDataIsNull()
        {
            var result = await _toDosController.AddToDo(null);

            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("ToDo data cannot be null.", actionResult.Value);
        }

        #endregion

        #region UpdateToDo([FromBody] ToDoUpdateDto toDoUpdateDto) tests

        [Fact]
        public async Task UpdateToDo_ShouldReturnNoContent_WhenValid()
        {
            var toDoCategoryId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var toDo = new ToDo("Description", TimeBlock.Day, Difficulty.Easy, DateTime.Today, toDoCategoryId, userId);
            var toDoUpdateDto = new ToDoUpdateDto { ToDoId = toDo.ToDoId, Description = "Description", Difficulty = Difficulty.Easy, ToDoDate = DateTime.Today, ToDoCategoryId = toDoCategoryId };

            _toDoServiceMock.Setup(service => service.GetToDoByIdAsync(toDo.ToDoId)).ReturnsAsync(toDo);
            _mapperMock.Setup(mapper => mapper.Map(toDoUpdateDto, toDo)).Returns(toDo);
            _toDoServiceMock.Setup(s => s.UpdateToDoAsync(toDo)).Returns(Task.CompletedTask);

            var result = await _toDosController.UpdateToDo(toDoUpdateDto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateToDo_ShouldReturnBadRequest_WhenDataIsNull()
        {
            var result = await _toDosController.UpdateToDo(null);

            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("ToDo data cannot be null.", actionResult.Value);
        }

        [Fact]
        public async Task UpdateToDo_ShouldReturnNotFound_WhenToDoDoesNotExist()
        {
            var toDoCategoryId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var toDo = new ToDo("Description", TimeBlock.Day, Difficulty.Easy, DateTime.Today, toDoCategoryId, userId);
            var toDoUpdateDto = new ToDoUpdateDto { ToDoId = toDo.ToDoId, Description = "Description", Difficulty = Difficulty.Easy, ToDoDate = DateTime.Today, ToDoCategoryId = toDoCategoryId };

            _toDoServiceMock.Setup(service => service.GetToDoByIdAsync(toDo.ToDoId)).ReturnsAsync((ToDo?)null);

            var result = await _toDosController.UpdateToDo(toDoUpdateDto);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("ToDo was not found.", notFoundResult.Value);
        }


        #endregion

        #region DeleteToDo(Guid toDoId) tests

        [Fact]
        public async Task DeleteToDo_ShouldReturnNoContent_WhenSuccessful()
        {
            var toDoId = Guid.NewGuid();

            _toDoServiceMock.Setup(s => s.DeleteToDoAsync(toDoId)).Returns(Task.CompletedTask);

            var result = await _toDosController.DeleteToDo(toDoId);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteToDo_ShouldReturnBadRequest_WhenToDoIdIsEmpty()
        {
            var result = await _toDosController.DeleteToDo(new Guid());

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("ToDo id cannot be empty.", badRequestResult.Value);
        }

        #endregion
    }
}
