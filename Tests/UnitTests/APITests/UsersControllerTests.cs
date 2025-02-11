using API.Controllers;
using AutoMapper;
using Core.DTOs.User;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace Tests.UnitTests.APITests
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UsersController _usersController;

        public UsersControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _mapperMock = new Mock<IMapper>();
            _usersController = new UsersController(_userServiceMock.Object, _mapperMock.Object);
        }

        #region GetCurrentUser() tests

        [Fact]
        public async Task GetCurrentUser_ShouldReturnUserInfo_WhenUserExists()
        {
            var userName = "TestUser";
            var user = new User(userName);
            var userId = user.UserId;

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name,userId.ToString())
            ], "mock"));

            _usersController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userClaims }
            };

            _userServiceMock.Setup(service => service.GetUserByUserIdAsync(userId)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns((User user) => new UserDto { UserId = user.UserId, UserName = user.UserName, Experience = user.Experience, Level = user.Level });

            var result = await _usersController.GetCurrentUser();

            var actionResult = Assert.IsType<ActionResult<UserDto>>(result);
            var returnedUserDto = Assert.IsType<UserDto>(actionResult.Value);

            Assert.Equal(user.UserId, returnedUserDto.UserId);
            Assert.Equal(user.UserName, returnedUserDto.UserName);
            Assert.Equal(user.Experience, returnedUserDto.Experience);
            Assert.Equal(user.Level, returnedUserDto.Level);
        }

        [Fact]
        public async Task GetCurrentUser_ShouldReturnUnauthorized_WhenUserIdIsNotAuthenticated()
        {
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity());

            _usersController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userClaims }
            };

            var result = await _usersController.GetCurrentUser();

            var actionResult = Assert.IsType<ActionResult<UserDto>>(result);
            var objectResult = Assert.IsType<UnauthorizedObjectResult>(actionResult.Result);

            Assert.Equal("User ID is not authenticated or invalid.", objectResult.Value);
        }

        [Fact]
        public async Task GetCurrentUser_ShouldReturnNotFound_WhenUserDoesNotFound()
        {
            var user = new User("TestUser");
            var userId = user.UserId;

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name,userId.ToString())
            ], "mock"));

            _usersController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userClaims }
            };

            _userServiceMock.Setup(service => service.GetUserByUserIdAsync(userId)).ReturnsAsync((User?)null);

            var result = await _usersController.GetCurrentUser();

            var actionResult = Assert.IsType<ActionResult<UserDto>>(result);
            var objectResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);

            Assert.Equal("User was not found.", objectResult.Value);
        }

        #endregion

        #region UpdateUser([FromBody] UserUpdateDto updateUserDto) tests

        [Fact]
        public async Task UpdateUser_ShouldSuccessfullyUpdateUser()
        {
            var user = new User("TestUser");

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name,user.UserId.ToString())
            ], "mock"));

            _usersController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userClaims }
            };

            var userUpdateDto = new UserUpdateDto { UserName = "NewUserName" };
            user.UserName = userUpdateDto.UserName;

            _userServiceMock.Setup(service => service.GetUserByUserIdAsync(user.UserId)).ReturnsAsync(user);
            _userServiceMock.Setup(service => service.UpdateUserAsync(user)).Returns(Task.CompletedTask);

            var result = await _usersController.UpdateUserProfile(userUpdateDto);

            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnUnauthorized_WhenUserIdIsNotAuthenticated()
        {
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity());

            _usersController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userClaims }
            };

            var userUpdateDto = new UserUpdateDto { UserName = "TestUser" };

            var result = await _usersController.UpdateUserProfile(userUpdateDto);

            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("User ID is not authenticated or invalid.", unauthorizedResult.Value);

        }

        [Fact]
        public async Task UpdateUser_ShouldReturnBadRequest_WhenDataIsEmpty()
        {
            var user = new User("TestUser");

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name,user.UserId.ToString())
            ], "mock"));

            _usersController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userClaims }
            };

            var result = await _usersController.UpdateUserProfile(null);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User data cannot be null.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            var user = new User("TestUser");

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name,user.UserId.ToString())
            ], "mock"));

            _usersController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userClaims }
            };

            var userUpdateDto = new UserUpdateDto { UserName = "TestUser" };

            _userServiceMock.Setup(service => service.GetUserByUserIdAsync(user.UserId)).ReturnsAsync((User?)null);

            var result = await _usersController.UpdateUserProfile(userUpdateDto);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User was not found.", notFoundResult.Value);
        }

        #endregion

    }
}
