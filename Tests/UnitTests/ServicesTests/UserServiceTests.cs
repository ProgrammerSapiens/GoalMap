using Moq;
using Core.Interfaces;
using Core.Services;
using Core.Models;

namespace Tests.UnitTests.ServicesTests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly Mock<IPasswordHasher> passwordHasherMock;
        private readonly IUserService userService;

        public UserServiceTests()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            passwordHasherMock = new Mock<IPasswordHasher>();

            userService = new UserService(userRepositoryMock.Object, passwordHasherMock.Object);
        }

        #region GetUserByUserIdAsync(Guid userId) tests

        [Fact]
        public async Task GetUserByUserIdAsync_ShouldReturnUser_WhenUserIdExists()
        {
            var userId = Guid.NewGuid();
            var expectedUser = new User("TestUser")
            {
                ToDos = new List<ToDo>(),
                ToDoCategories = new List<ToDoCategory>()
            };

            userRepositoryMock.Setup(repo => repo.GetUserByUserIdAsync(userId)).ReturnsAsync(expectedUser);

            var result = await userService.GetUserByUserIdAsync(userId);

            Assert.NotNull(result);
            Assert.Equal(expectedUser.UserId, result.UserId);
            Assert.Equal(expectedUser.UserName, result.UserName);
            Assert.Equal(expectedUser.Experience, result.Experience);
            Assert.Empty(result.ToDos);
            Assert.Empty(result.ToDoCategories);
        }

        [Fact]
        public async Task GetUserByUserIdAsync_ShouldReturnNull_WhenUserIdDoesNotExist()
        {
            var nonExistingUserId = Guid.NewGuid();

            userRepositoryMock.Setup(repo => repo.GetUserByUserIdAsync(nonExistingUserId)).ReturnsAsync((User?)null);

            var result = await userService.GetUserByUserIdAsync(nonExistingUserId);
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

            userRepositoryMock.Setup(repo => repo.GetUserByUserNameAsync(userName)).ReturnsAsync((User?)null);
            passwordHasherMock.Setup(hasher => hasher.HashPasswordAsync(password)).ReturnsAsync(hashedPassword);
            userRepositoryMock.Setup(repo => repo.AddUserAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            await userService.RegisterUserAsync(user, password);

            userRepositoryMock.Verify(repo => repo.AddUserAsync(It.Is<User>(u => u.UserName == userName && u.PasswordHash == hashedPassword)), Times.Once);
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldThrowException_WhenUsernameAlreadyExists()
        {
            var userName = "existingUser";
            var password = "validPassword";

            var user = new User(userName);

            var existingUser = new User(userName);

            userRepositoryMock.Setup(repo => repo.GetUserByUserNameAsync(userName)).ReturnsAsync(user);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => userService.RegisterUserAsync(existingUser, password));
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

            userRepositoryMock.Setup(repo => repo.GetUserByUserNameAsync(userName)).ReturnsAsync(user);
            passwordHasherMock.Setup(repo => repo.VerifyPasswordAsync(password, hashedPassword)).ReturnsAsync(true);

            var result = await userService.AuthenticateUserAsync(userName, password);

            Assert.True(result);
            userRepositoryMock.Verify(repo => repo.GetUserByUserNameAsync(userName), Times.Once());
            passwordHasherMock.Verify(hasher => hasher.VerifyPasswordAsync(password, hashedPassword), Times.Once);
        }

        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnFalse_WhenUserNameIsInvalid()
        {
            var invalidUserName = "invalidUser";
            var password = "validPassword";

            userRepositoryMock.Setup(repo => repo.GetUserByUserNameAsync(invalidUserName)).ReturnsAsync((User?)null);

            var result = await userService.AuthenticateUserAsync(invalidUserName, password);

            Assert.False(result);
        }

        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnFalse_WhenPasswordIsInvalid()
        {
            var userName = "validUser";
            var invalidPassword = "invalidPassword";
            var passwordHash = "hashedPassword";

            var user = new User(userName) { PasswordHash = passwordHash };

            userRepositoryMock.Setup(repo => repo.GetUserByUserNameAsync(userName)).ReturnsAsync(user);
            passwordHasherMock.Setup(hasher => hasher.VerifyPasswordAsync(invalidPassword, passwordHash)).ReturnsAsync(false);

            var result = await userService.AuthenticateUserAsync(userName, invalidPassword);

            Assert.False(result);
        }

        #endregion

        #region UpdateUserAsync(User user) tests (Empty)



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

            userRepositoryMock.Setup(repo => repo.GetUserByUserIdAsync(userId)).ReturnsAsync(user);
            userRepositoryMock.Setup(repo => repo.UpdateUserAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            await userService.UpdateUserExperienceAsync(userId, difficulty);

            Assert.Equal(updatedExperience, user.Experience);
            userRepositoryMock.Verify(repo => repo.UpdateUserAsync(It.Is<User>(u => u.Experience == updatedExperience)), Times.Once);
        }

        [Fact]
        public async Task UpdateUserExperienceAsync_ShouldThrowException_WhenUserNameDoesNotExist()
        {
            var userId = Guid.NewGuid();

            userRepositoryMock.Setup(repo => repo.GetUserByUserIdAsync(userId)).ReturnsAsync((User?)null);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => userService.UpdateUserExperienceAsync(userId, Difficulty.Easy));

            Assert.Equal("User was not found.", exception.Message);
        }

        #endregion
    }
}
