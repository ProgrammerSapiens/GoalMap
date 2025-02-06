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

        #region GetCurrentUser tests

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

        #region RegisterUser([FromBody] UserRegAndAuthDto registerUserDto) tests

        [Fact]
        public async Task RegisterUser_ShouldSuccessfullyRegisterNewUser()
        {
            var userRegisterDto = new UserRegAndAuthDto { UserName = "TestUser", Password = "Password" };
            var user = new User(userRegisterDto.UserName);
            var userDto = new UserDto { UserName = user.UserName, UserId = user.UserId, Experience = user.Experience, Level = user.Level };

            _mapperMock.Setup(m => m.Map<User>(userRegisterDto)).Returns(user);
            _mapperMock.Setup(m => m.Map<UserDto>(user)).Returns(userDto);
            _userServiceMock.Setup(service => service.RegisterUserAsync(user, userRegisterDto.Password)).Returns(Task.CompletedTask);

            var result = await _usersController.RegisterUser(userRegisterDto);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedUserDto = Assert.IsType<UserDto>(createdAtActionResult.Value);

            Assert.Equal(nameof(_usersController.GetCurrentUser), createdAtActionResult.ActionName);
            Assert.Equal(user.UserName, createdAtActionResult.RouteValues["userName"]);
            Assert.Equal(userDto.UserId, returnedUserDto.UserId);
            Assert.Equal(userDto.UserName, returnedUserDto.UserName);
            Assert.Equal(userDto.Experience, returnedUserDto.Experience);
            Assert.Equal(userDto.Level, returnedUserDto.Level);
        }

        [Fact]
        public async Task RegisterUser_ShouldReturnBadRequest_WhenDataIsNull()
        {
            var registerUserDto = new UserRegAndAuthDto { UserName = null, Password = null };
            _mapperMock.Setup(m => m.Map<User?>(registerUserDto)).Returns((User?)null);

            var result = await _usersController.RegisterUser(registerUserDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("User data or password cannot be null", badRequestResult.Value);
        }

        #endregion

        #region AuthenticateUser([FromBody] UserRegAndAuthDto authenticateUserDto) tests

        [Fact]
        public async Task AuthenticateUser_ShouldReturnToken_WhenCredentialsAreValid()
        {
            var userAuthenticateDto = new UserRegAndAuthDto { UserName = "TestUser", Password = "Password" };
            string userName = userAuthenticateDto.UserName;
            string password = userAuthenticateDto.Password;

            _userServiceMock.Setup(service => service.AuthenticateUserAsync(userName, password)).ReturnsAsync(true);

            var result = await _usersController.AuthenticateUser(userAuthenticateDto);
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task AuthenticateUser_ShouldReturnBadRequest_WhenDataIsNull()
        {
            var userAuthenticateDto = new UserRegAndAuthDto { UserName = null, Password = null };
            string? userName = userAuthenticateDto.UserName;
            string? password = userAuthenticateDto.Password;

            var result = await _usersController.AuthenticateUser(userAuthenticateDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Username or password cannot be empty.", badRequestResult.Value);
        }

        [Fact]
        public async Task AuthenticateUser_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
        {
            var userAuthenticateDto = new UserRegAndAuthDto { UserName = "InvalidUser", Password = "InvalidPassword" };
            string userName = userAuthenticateDto.UserName;
            string password = userAuthenticateDto.Password;

            _userServiceMock.Setup(service => service.AuthenticateUserAsync(userName, password)).ReturnsAsync(false);

            var result = await _usersController.AuthenticateUser(userAuthenticateDto);

            var unauthorisedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid username or password", unauthorisedResult.Value);
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

            var result = await _usersController.UpdateUser(userUpdateDto);

            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public async Task UpdateUser_ShouldSuccessfullyUpdateUserExperience()
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

            var userUpdateDto = new UserUpdateDto { Difficulty = Difficulty.Easy };

            _userServiceMock.Setup(service => service.UpdateUserExperienceAsync(user.UserId, userUpdateDto.Difficulty)).Returns(Task.CompletedTask);

            var result = await _usersController.UpdateUser(userUpdateDto);

            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);

            _userServiceMock.Verify(service => service.UpdateUserExperienceAsync(user.UserId, userUpdateDto.Difficulty), Times.Once);
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

            var result = await _usersController.UpdateUser(userUpdateDto);

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

            var userUpdateDto = new UserUpdateDto { Difficulty = Difficulty.None };

            var result = await _usersController.UpdateUser(userUpdateDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("UserName cannot be empty.", badRequestResult.Value);
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

            var result = await _usersController.UpdateUser(userUpdateDto);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User was not found.", notFoundResult.Value);
        }

        #endregion

    }
}
