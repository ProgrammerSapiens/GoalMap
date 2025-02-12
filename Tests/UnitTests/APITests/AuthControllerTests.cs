using API.Controllers;
using AutoMapper;
using Core.DTOs.User;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.UnitTests.APITests
{
    public class AuthControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IJwtTokenService> _jwtTokenServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<AuthController>> _loggerMock;
        private readonly AuthController _authController;

        public AuthControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _jwtTokenServiceMock = new Mock<IJwtTokenService>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<AuthController>>();
            _authController = new AuthController(_userServiceMock.Object, _jwtTokenServiceMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        #region Register([FromBody] UserRegAndAuthDto registerUserDto) tests

        [Fact]
        public async Task Register_WithValidData_ReturnsCreatedAtAction()
        {
            var registerDto = new UserRegAndAuthDto { UserName = "testUser", Password = "password123" };
            var user = new User(registerDto.UserName);
            var userDto = new UserDto { UserName = registerDto.UserName };

            _mapperMock.Setup(m => m.Map<User>(registerDto)).Returns(user);
            _userServiceMock.Setup(s => s.RegisterUserAsync(user, registerDto.Password)).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

            var result = await _authController.Register(registerDto);

            var createdAtAction = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedUserDto = Assert.IsType<UserDto>(createdAtAction.Value);
            Assert.NotNull(returnedUserDto);
            Assert.Equal(userDto.UserName, returnedUserDto.UserName);
            Assert.Equal(userDto.UserId, returnedUserDto.UserId);
        }

        [Fact]
        public async Task Register_WithEmptyUsernameOrPassword_ReturnsBadRequest()
        {
            var registerDto = new UserRegAndAuthDto { UserName = "", Password = "" };

            var result = await _authController.Register(registerDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Username and password cannot be empty.", badRequestResult.Value);
        }

        #endregion

        #region Login([FromBody] UserRegAndAuthDto loginDto) tests

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsOkWithToken()
        {
            var loginDto = new UserRegAndAuthDto { UserName = "testUser", Password = "password123" };
            var user = new User(loginDto.UserName);
            var token = "mocked-jwt-token";

            _userServiceMock.Setup(s => s.GetUserByUserNameAsync(loginDto.UserName)).ReturnsAsync(user);
            _userServiceMock.Setup(s => s.AuthenticateUserAsync(loginDto.UserName, loginDto.Password)).ReturnsAsync(true);
            _jwtTokenServiceMock.Setup(t => t.GenerateTokenAsync(user)).ReturnsAsync(token);

            var result = await _authController.Login(loginDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult);
            Assert.Equivalent(new { Token = token }, okResult.Value);
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
        {
            var loginDto = new UserRegAndAuthDto { UserName = "testUser", Password = "wrongPassword" };

            _userServiceMock.Setup(s => s.GetUserByUserNameAsync(loginDto.UserName)).ReturnsAsync((User)null);

            var result = await _authController.Login(loginDto);

            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid username or password.", unauthorizedResult.Value);
        }

        [Fact]
        public async Task Login_WithEmptyUsernameOrPassword_ReturnsBadRequest()
        {
            var loginDto = new UserRegAndAuthDto { UserName = "", Password = "" };

            var result = await _authController.Login(loginDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Username or password cannot be empty.", badRequestResult.Value);
        }

        #endregion
    }
}
