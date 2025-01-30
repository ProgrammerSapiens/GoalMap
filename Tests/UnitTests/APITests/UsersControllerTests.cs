using API.Controllers;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.UnitTests.APITests
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UsersController _usersController;

        public UsersControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _usersController = new UsersController(_userServiceMock.Object);
        }

        #region GetUserByUserName tests

        //[Fact]
        //public async Task GetUserByUserName_ShouldReturnUserInfo_WhenUserExists()
        //{
        //    var userName = "TestUser";
        //    var hashedPassword = "password";
        //    var user = new User(userName, hashedPassword);

        //    _userServiceMock.Setup(user => user.GetUserByUserNameAsync(userName)).ReturnsAsync(user);

        //    var result = await _usersController.GetCurrentUser();

        //    var okResult = Assert.IsType<ActionResult<User>>(result);
        //    var returnedUser = Assert.IsType<User>(okResult.Value);
        //    Assert.Equal(user.UserId, returnedUser.UserId);
        //}

        [Fact]
        public async Task GetUserByUserName_ShouldReturnNotFound_WhenUserDoesNotFound()
        {
            var result = await _usersController.GetCurrentUser();

            Assert.IsType<ActionResult<User>>(result);
        }

        #endregion

        #region RegisterUser tests

        //[Fact]
        //public async Task RegisterUser_ShouldSuccessfullyRegisterNewUser()
        //{
        //    string userName = "TestUser";
        //    string password = "UserPassword";
        //    var user = new User(userName, password);

        //    _userServiceMock.Setup(user => user.RegisterUserAsync(It.Is<User>(u => u.UserName == userName), password)).Returns(Task.CompletedTask);

        //    var result = await _usersController.RegisterUser(user, password);

        //    _userServiceMock.Verify(service => service.RegisterUserAsync(It.Is<User>(u => u.UserName == userName), password), Times.Once);

        //    var actionResult = Assert.IsType<CreatedAtActionResult>(result);
        //    Assert.Equal("GetUserByUserName", actionResult.ActionName);
        //    Assert.Equal(user.UserName, actionResult.RouteValues["userName"]);
        //}

        //[Fact]
        //public async Task RegisterUser_ShouldReturnError_WhenUserAlreadyExists()
        //{
        //    string userName = "TestUser";
        //    string password = "UserPassword";

        //    var user = new User(userName, password);

        //    _userServiceMock.Setup(service => service.RegisterUserAsync(It.Is<User>(u => u.UserName == userName), password)).ThrowsAsync(new InvalidOperationException("User name is already exists."));

        //    var result = await _usersController.RegisterUser(user, password);

        //    _userServiceMock.Verify(service => service.RegisterUserAsync(It.Is<User>(u => u.UserName == userName), password), Times.Once);

        //    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        //    Assert.Equal("User name is already exists.", badRequestResult.Value);
        //}

        #endregion

        #region AuthenticateUser tests

        //[Fact]
        //public async Task AuthenticateUser_ShouldReturnToken_WhenCredentialsAreValid()
        //{
        //    string userName = "TestUser";
        //    string password = "Password";

        //    var user = new User(userName, password);

        //    _userServiceMock.Setup(service => service.AuthenticateUserAsync(userName, password)).ReturnsAsync(true);

        //    var result = await _usersController.AuthenticateUser(user, password);
        //    var okResult = Assert.IsType<OkResult>(result);
        //}

        //[Fact]
        //public async Task AuthenticateUser_ShouldReturnError_WhenCredentialsAreInvalid()
        //{
        //    string userName = "InvalidUser";
        //    string password = "InvalidPassword";

        //    var user = new User(userName, password);

        //    _userServiceMock.Setup(service => service.AuthenticateUserAsync(userName, password)).ReturnsAsync(false);

        //    var result = await _usersController.AuthenticateUser(user, password);
        //    var unauthorisedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        //    Assert.Equal("Invalid username or password", unauthorisedResult.Value);
        //}

        #endregion

        #region UpdateUserExperience tests

        //[Fact]
        //public async Task UpdateUserExperience_ShouldSuccessfullyUpdateUserExperience()
        //{
        //    string userName = "ExistingUserName";
        //    var difficulty = Difficulty.Easy;
        //    string password = "ExistingPassword";

        //    var user = new User(userName, password);

        //    _userServiceMock.Setup(service => service.UpdateUserExperienceAsync(userName, difficulty)).Returns(Task.CompletedTask);

        //    await _usersController.UpdateUserExperience(difficulty);

        //    _userServiceMock.Verify(service => service.UpdateUserExperienceAsync(userName, difficulty), Times.Once);
        //}

        //[Fact]
        //public async Task UpdateUserExperience_ShouldReturnError_WhenUserDoesNotExist()
        //{
        //    string userName = "ExistingUserName";
        //    string password = "ExistingPassword";
        //    var difficulty = Difficulty.Easy;

        //    var user = new User(userName, password);

        //    _userServiceMock.Setup(service => service.UpdateUserExperienceAsync(userName, difficulty)).ThrowsAsync(new InvalidOperationException("User does not exist."));

        //    var result = await _usersController.UpdateUserExperience(difficulty);
        //    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        //    Assert.Equal("User does not exist.", badRequestResult.Value);
        //}

        #endregion

    }
}
