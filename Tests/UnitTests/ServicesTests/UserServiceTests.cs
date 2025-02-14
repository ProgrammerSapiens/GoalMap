using Moq;
using Core.Interfaces;
using Core.Services;
using Core.Models;
using Microsoft.Extensions.Logging;

namespace Tests.UnitTests.ServicesTests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<ILogger<UserService>> _loggerMock;
        private readonly IUserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _loggerMock = new Mock<ILogger<UserService>>();

            _userService = new UserService(_userRepositoryMock.Object, _passwordHasherMock.Object, _loggerMock.Object);
        }

        #region GetUserByUserIdAsync(Guid userId) tests

        [Fact]
        public async Task GetUserByUserIdAsync_ShouldReturnUser_WhenUserIdExists()
        {
            var userId = Guid.NewGuid();
            var expectedUser = new User("TestUser");

            _userRepositoryMock.Setup(repo => repo.GetUserByUserIdAsync(userId)).ReturnsAsync(expectedUser);

            var result = await _userService.GetUserByUserIdAsync(userId);

            Assert.NotNull(result);
            Assert.Equal(expectedUser.UserId, result.UserId);
            Assert.Equal(expectedUser.UserName, result.UserName);
            Assert.Equal(expectedUser.Experience, result.Experience);
        }

        [Fact]
        public async Task GetUserByUserIdAsync_ShouldReturnNull_WhenUserIdDoesNotExist()
        {
            var nonExistingUserId = Guid.NewGuid();

            _userRepositoryMock.Setup(repo => repo.GetUserByUserIdAsync(nonExistingUserId)).ReturnsAsync((User?)null);

            var result = await _userService.GetUserByUserIdAsync(nonExistingUserId);
            Assert.Null(result);
        }

        #endregion

        #region  RegisterUserAsync(User user, string password) tests

        [Fact]
        public async Task RegisterUserAsync_ShouldSucceed_WhenUserIsValid()
        {
            var userName = "TestUser";
            var password = "Test password";
            var hashedPassword = "hashedTestPassword";

            var user = new User(userName);

            _userRepositoryMock.Setup(repo => repo.GetUserByUserNameAsync(userName)).ReturnsAsync((User?)null);
            _passwordHasherMock.Setup(hasher => hasher.HashPasswordAsync(password)).ReturnsAsync(hashedPassword);
            _userRepositoryMock.Setup(repo => repo.AddUserAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            await _userService.RegisterUserAsync(user, password);

            _userRepositoryMock.Verify(repo => repo.AddUserAsync(It.Is<User>(u => u.UserName == userName && u.PasswordHash == hashedPassword)), Times.Once);
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldThrowException_WhenUsernameAlreadyExists()
        {
            var userName = "existingUser";
            var password = "validPassword";

            var user = new User(userName);

            var existingUser = new User(userName);

            _userRepositoryMock.Setup(repo => repo.GetUserByUserNameAsync(userName)).ReturnsAsync(user);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.RegisterUserAsync(existingUser, password));
            Assert.Equal("User name is already exists.", exception.Message);
        }

        #endregion

        #region AuthenticateUserAsync(string userName, string password) tests

        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnTrue_WhenCredentialsAreValid()
        {
            var userName = "Test User";
            var password = "testPassword";
            var hashedPassword = "hashedTestPassword";

            var user = new User(userName) { PasswordHash = hashedPassword };

            _userRepositoryMock.Setup(repo => repo.GetUserByUserNameAsync(userName)).ReturnsAsync(user);
            _passwordHasherMock.Setup(repo => repo.VerifyPasswordAsync(password, hashedPassword)).ReturnsAsync(true);

            var result = await _userService.AuthenticateUserAsync(userName, password);

            Assert.True(result);
            _userRepositoryMock.Verify(repo => repo.GetUserByUserNameAsync(userName), Times.Once());
            _passwordHasherMock.Verify(hasher => hasher.VerifyPasswordAsync(password, hashedPassword), Times.Once);
        }

        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnFalse_WhenUserNameIsInvalid()
        {
            var invalidUserName = "invalidUser";
            var password = "validPassword";

            _userRepositoryMock.Setup(repo => repo.GetUserByUserNameAsync(invalidUserName)).ReturnsAsync((User?)null);

            var result = await _userService.AuthenticateUserAsync(invalidUserName, password);

            Assert.False(result);
        }

        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnFalse_WhenPasswordIsInvalid()
        {
            var userName = "validUser";
            var invalidPassword = "invalidPassword";
            var passwordHash = "hashedPassword";

            var user = new User(userName) { PasswordHash = passwordHash };

            _userRepositoryMock.Setup(repo => repo.GetUserByUserNameAsync(userName)).ReturnsAsync(user);
            _passwordHasherMock.Setup(hasher => hasher.VerifyPasswordAsync(invalidPassword, passwordHash)).ReturnsAsync(false);

            var result = await _userService.AuthenticateUserAsync(userName, invalidPassword);

            Assert.False(result);
        }

        #endregion

        #region UpdateUserAsync(User user) tests

        public async Task UpdateUserAsync_ShouldUpdate()
        {
            var user = new User("User");

            _userRepositoryMock.Setup(repo => repo.UpdateUserAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            _userRepositoryMock.Verify(repo => repo.UpdateUserAsync(user), Times.Once());
        }

        #endregion

        #region UpdateUserExperienceAsync(Guid userId, Difficulty taskDifficulty) tests

        [Theory]
        [InlineData(Difficulty.Easy)]
        [InlineData(Difficulty.Medium)]
        [InlineData(Difficulty.Hard)]
        [InlineData(Difficulty.Nightmare)]
        public async Task UpdateUserExperienceAsync_ShouldUpdateExperience_WhenInputsAreValid(Difficulty difficulty)
        {
            var userName = "TestUser";
            int initialExperience = 5;
            int updatedExperience = initialExperience + (int)difficulty;

            var user = new User(userName) { Experience = initialExperience };
            var userId = user.UserId;

            _userRepositoryMock.Setup(repo => repo.GetUserByUserIdAsync(userId)).ReturnsAsync(user);
            _userRepositoryMock.Setup(repo => repo.UpdateUserAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            await _userService.UpdateUserExperienceAsync(userId, difficulty);

            Assert.Equal(updatedExperience, user.Experience);
            _userRepositoryMock.Verify(repo => repo.UpdateUserAsync(It.Is<User>(u => u.Experience == updatedExperience)), Times.Once);
        }

        [Fact]
        public async Task UpdateUserExperienceAsync_ShouldThrowException_WhenUserNameDoesNotExist()
        {
            var userId = Guid.NewGuid();

            _userRepositoryMock.Setup(repo => repo.GetUserByUserIdAsync(userId)).ReturnsAsync((User?)null);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.UpdateUserExperienceAsync(userId, Difficulty.Easy));

            Assert.Equal("User was not found.", exception.Message);
        }

        #endregion
    }
}
