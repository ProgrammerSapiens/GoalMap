using Moq;
using Core.Interfaces;
using Core.Services;
using Core.Models;

namespace Tests.UnitTests.ServicesTests
{
    public class UserServiceTests
    {
        #region GetUserByIdAsync(string userName) tests

        [Fact]
        public async Task GetUserByUserNameAsync_ShouldReturnUser_WhenUserNameExists()
        {
            var userName = "Test user";
            var expectedUser = new User("TestUser", "hashed_password", 100)
            {
                ToDos = new List<ToDo>(),
                ToDoCategories = new List<ToDoCategory>()
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.IsUserExistsAsync(userName)).ReturnsAsync(true);
            userRepositoryMock.Setup(repo => repo.GetUserByUserNameAsync(userName)).ReturnsAsync(expectedUser);

            var userService = new UserService(userRepositoryMock.Object);

            var result = await userService.GetUserByUserNameAsync(userName);

            Assert.NotNull(result);
            Assert.Equal(expectedUser.UserId, result.UserId);
            Assert.Equal(expectedUser.UserName, result.UserName);
            Assert.Equal(expectedUser.PasswordHash, result.PasswordHash);
            Assert.Equal(expectedUser.Experience, result.Experience);
            Assert.Empty(result.ToDos);
            Assert.Empty(result.ToDoCategories);
        }

        [Fact]
        public async Task GetUserByUserNameAsync_ShouldThrowException_WhenUserNameDoesNotExist()
        {
            var userName = "Test user";

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetUserByUserNameAsync(userName)).ReturnsAsync((User?)null);

            var userService = new UserService(userRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => userService.GetUserByUserNameAsync(userName));
            Assert.Equal("User does not exist.", exception.Message);
        }

        [Fact]
        public async Task GetUserByUserNameAsync_ShouldThrowException_WhenUserNameIsNullOrEmpty()
        {
            var userName = "";

            var userRepositoryMock = new Mock<IUserRepository>();
            var userService = new UserService(userRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => userService.GetUserByUserNameAsync(userName));

            Assert.Equal("User name cannot be null or empty.", exception.Message);
            userRepositoryMock.Verify(repo => repo.GetUserByUserNameAsync(userName), Times.Never);
        }

        #endregion

        #region UpdateUserExperienceAsync(string userName, Difficulty difficulty) tests

        [Theory]
        [InlineData(Difficulty.Easy)]
        [InlineData(Difficulty.Medium)]
        [InlineData(Difficulty.Hard)]
        [InlineData(Difficulty.Nightmare)]
        public async Task UpdateUserExperienceAsync_ShouldUpdateExperience_WhenInputsAreValid(Difficulty difficulty)
        {
            var userName = "userName";
            var password = "userPassword";
            int initialExperience = 5;
            int updatedExperience = initialExperience + (int)difficulty;

            var user = new User(userName, password, initialExperience);

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetUserByUserNameAsync(userName)).ReturnsAsync(user);
            userRepositoryMock.Setup(repo => repo.UpdateUserAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            var userService = new UserService(userRepositoryMock.Object);

            await userService.UpdateUserExperienceAsync(userName, difficulty);

            Assert.Equal(updatedExperience, user.Experience);
            userRepositoryMock.Verify(repo => repo.UpdateUserAsync(It.Is<User>(u => u.Experience == updatedExperience)), Times.Once);

        }

        [Fact]
        public async Task UpdateUserExperienceAsync_ShouldThrowException_WhenUserNameDoesNotExist()
        {
            var userName = "Non-exsistent test User";

            var userRepositoryMock = new Mock<IUserRepository>();
            var userService = new UserService(userRepositoryMock.Object);

            userRepositoryMock.Setup(repo => repo.IsUserExistsAsync(userName)).ReturnsAsync(false);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => userService.UpdateUserExperienceAsync(userName, Difficulty.Easy));

            Assert.Equal("User does not exist.", exception.Message);
        }

        [Fact]
        public async Task UpdateUserExperiecneAsync_ShouldThrowException_WhenUserNameIsNullOrEmpty()
        {
            var userName = "";

            var userRepositoryMock = new Mock<IUserRepository>();
            var userService = new UserService(userRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => userService.UpdateUserExperienceAsync(userName, Difficulty.Easy));

            Assert.Equal("User name cannot be null or empty.", exception.Message);
        }

        #endregion

        #region AuthenticateUserAsync(string userName, string password) tests

        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnTrue_WhenCredentialsAreValid()
        {
            string userName = "Test User";
            string password = "testPassword";
            string hashedPassword = "hashedTestPassword";

            var userRepositoryMock = new Mock<IUserRepository>();
            var passwordHasherMock = new Mock<IPasswordHasher>();

            var user = new User(userName, hashedPassword, 100);

            userRepositoryMock.Setup(repo => repo.GetUserByUserNameAsync(userName)).ReturnsAsync(user);

            passwordHasherMock.Setup(repo => repo.VerifyPasswordAsync(password, hashedPassword)).ReturnsAsync(true);

            var userService = new UserService(userRepositoryMock.Object, passwordHasherMock.Object);

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

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetUserByUserNameAsync(invalidUserName)).ReturnsAsync((User?)null);

            var userService = new UserService(userRepositoryMock.Object);

            var result = await userService.AuthenticateUserAsync(invalidUserName, password);

            Assert.False(result);
        }

        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnFalse_WhenPasswordIsInvalid()
        {
            var userName = "validUser";
            var invalidPassword = "invalidPassword";
            var passwordHash = "hashedPassword";

            var user = new User(userName, passwordHash, 100);

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetUserByUserNameAsync(userName)).ReturnsAsync(user);

            var passwordHasherMock = new Mock<IPasswordHasher>();
            passwordHasherMock.Setup(hasher => hasher.VerifyPasswordAsync(invalidPassword, passwordHash)).ReturnsAsync(false);

            var userService = new UserService(userRepositoryMock.Object, passwordHasherMock.Object);

            var result = await userService.AuthenticateUserAsync(userName, invalidPassword);

            Assert.False(result);
        }

        [Fact]
        public async Task AuthenticateUserAsync_ShouldThrowException_WhenInputsAreNullOrEmpty()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var passwordHasherMock = new Mock<IPasswordHasher>();

            var userService = new UserService(userRepositoryMock.Object, passwordHasherMock.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => userService.AuthenticateUserAsync(null, "password"));
            await Assert.ThrowsAsync<ArgumentNullException>(() => userService.AuthenticateUserAsync("username", null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => userService.AuthenticateUserAsync("", "password"));
            await Assert.ThrowsAsync<ArgumentNullException>(() => userService.AuthenticateUserAsync("username", null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => userService.AuthenticateUserAsync("username", ""));
            await Assert.ThrowsAsync<ArgumentNullException>(() => userService.AuthenticateUserAsync("", "password"));
        }

        #endregion

        #region RegisterUserAsync(User user) tests

        [Fact]
        public async Task RegisterUserAsync_ShouldSucceed_WhenUserIsValid()
        {
            var userName = "Test User";
            var password = "Test password";
            var hashedPassword = "hashedTestPassword";

            var newUser = new User(userName, password, 100);

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.IsUserExistsAsync(userName)).ReturnsAsync(false);
            userRepositoryMock.Setup(repo => repo.AddUserAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            var passwordHasherMock = new Mock<IPasswordHasher>();
            passwordHasherMock.Setup(hasher => hasher.HashPasswordAsync(password)).ReturnsAsync(hashedPassword);

            var userService = new UserService(userRepositoryMock.Object, passwordHasherMock.Object);

            await userService.RegisterUserAsync(newUser, password);

            userRepositoryMock.Verify(repo => repo.AddUserAsync(It.Is<User>(u => u.UserName == userName && u.PasswordHash == hashedPassword)), Times.Once);
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldThrowException_WhenUsernameAlreadyExists()
        {
            var userName = "existingUser";
            var password = "validPassword";

            var existingUser = new User(userName, password, 100);

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.IsUserExistsAsync(userName)).ReturnsAsync(true);

            var userService = new UserService(userRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => userService.RegisterUserAsync(existingUser, password));
            Assert.Equal("User name is already exists.", exception.Message);
        }

        #endregion
    }
}
