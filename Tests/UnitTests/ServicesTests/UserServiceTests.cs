using Moq;
using Core.Interfaces;
using Core.Services;
using Core.Models;

namespace Tests.UnitTests.ServicesTests
{
    public class UserServiceTests
    {
        #region GetUserByIdAsync(Guid userId) tests

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserIdExists()
        {
            var userId = Guid.NewGuid();
            var expectedUser = new User("TestUser", "hashed_password", 100)
            {
                ToDos = new List<ToDo>(),
                ToDoCategories = new List<ToDoCategory>()
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(expectedUser);

            var userService = new UserService(userRepositoryMock.Object);

            var result = await userService.GetUserByIdAsync(userId);

            Assert.NotNull(result);
            Assert.Equal(expectedUser.UserId, result.UserId);
            Assert.Equal(expectedUser.UserName, result.UserName);
            Assert.Equal(expectedUser.PasswordHash, result.PasswordHash);
            Assert.Equal(expectedUser.Experience, result.Experience);
            Assert.Empty(result.ToDos);
            Assert.Empty(result.ToDoCategories);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldThrowException_WhenUserIdDoesNotExist()
        {
            var userId = Guid.NewGuid();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync((User?)null);

            var userService = new UserService(userRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => userService.GetUserByIdAsync(userId));
            Assert.Equal("The user was not found.", exception.Message);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldThrowException_WhenUserIdIsEmpty()
        {
            var userId = Guid.Empty;

            var userRepositoryMock = new Mock<IUserRepository>();
            var userService = new UserService(userRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => userService.GetUserByIdAsync(userId));
            Assert.Equal("The user ID cannot be empty.", exception.Message);
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

            passwordHasherMock.Setup(repo => repo.VerifyPassword(password, hashedPassword)).Returns(true);

            var userService = new UserService(userRepositoryMock.Object, passwordHasherMock.Object);

            var result = await userService.AuthenticateUserAsync(userName, password);

            Assert.True(result);
            userRepositoryMock.Verify(repo => repo.GetUserByUserNameAsync(userName), Times.Once());
            passwordHasherMock.Verify(hasher => hasher.VerifyPassword(password, hashedPassword), Times.Once);
        }

        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnFalse_WhenUserNameIsInvalid()
        {
            var invalidUserName = "invalidUser";
            var password = "validPassword";
            var passwordHash = "hashedPassword";

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetUserByUserNameAsync(invalidUserName)).ReturnsAsync((User?)null);

            var passwordHasherMock = new Mock<IPasswordHasher>();

            var userService = new UserService(userRepositoryMock.Object, passwordHasherMock.Object);

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
            passwordHasherMock.Setup(hasher => hasher.VerifyPassword(invalidPassword, passwordHash)).Returns(false);

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
            userRepositoryMock.Setup(repo => repo.GetUserByUserNameAsync(userName)).ReturnsAsync((User?)null);
            userRepositoryMock.Setup(repo => repo.AddUserAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            var passwordHasherMock = new Mock<IPasswordHasher>();
            passwordHasherMock.Setup(hasher => hasher.HashPassword(password)).Returns(hashedPassword);

            var userService = new UserService(userRepositoryMock.Object, passwordHasherMock.Object);

            await userService.RegisterUserAsync(newUser);

            userRepositoryMock.Verify(repo => repo.AddUserAsync(It.Is<User>(u => u.UserName == userName && u.PasswordHash == hashedPassword)), Times.Once);
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldThrowException_WhenUsernameAlreadyExists()
        {
            var userName = "existingUser";
            var password = "validPassword";

            var existingUser = new User(userName, password, 100);

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetUserByUserNameAsync(userName)).ReturnsAsync(existingUser);

            var passwordHasherMock = new Mock<IPasswordHasher>();

            var userService = new UserService(userRepositoryMock.Object, passwordHasherMock.Object);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => userService.RegisterUserAsync(existingUser));
            Assert.Equal("The username is already exists.", exception.Message);
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldThrowException_WhenUserIsNull()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var passwordHasherMock = new Mock<IPasswordHasher>();

            var userService = new UserService(userRepositoryMock.Object, passwordHasherMock.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => userService.RegisterUserAsync(null));
        }

        #endregion

        #region UpdateUserExperienceAsync(Guid userId, int experiencePoints) tests

        [Fact]
        public async Task UpdateUserExperienceAsync_ShouldUpdateExperience_WhenInputsAreValid()
        {
            var userName = "userName";
            var password = "userPassword";
            int initialExperience = 5;
            int updatedExperience = 10;

            var user = new User(userName, password, initialExperience);

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetUserByUserNameAsync(It.IsAny<string>())).ReturnsAsync(user);
            userRepositoryMock.Setup(repo => repo.UpdateUserAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            var userService = new UserService(userRepositoryMock.Object);

            await userService.UpdateUserExperienceAsync(user.UserId, updatedExperience);

            Assert.Equal(updatedExperience, user.Experience);
            userRepositoryMock.Verify(repo => repo.UpdateUserAsync(It.Is<User>(u => u.Experience == updatedExperience)), Times.Once);

        }

        [Fact]
        public async Task UpdateUserExperienceAsync_ShouldThrowException_WhenUserIdDoesNotExist()
        {
            var nonExistentUserId = Guid.NewGuid();

            var userRepositoryMock = new Mock<IUserRepository>();
            var userService = new UserService(userRepositoryMock.Object);

            userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(nonExistentUserId)).ReturnsAsync((User?)null);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => userService.UpdateUserExperienceAsync(nonExistentUserId, 10));

            Assert.Equal("User does not exist.", exception.Message);
        }

        [Fact]
        public async Task UpdateUserExperienceAsync_ShouldHandleNegativeExperiencePoints()
        {
            var userId = Guid.NewGuid();
            var initialExperience = 10;
            var negativeExperience = -5;

            var user = new User("testUser", "hashedPassword", initialExperience);

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);
            userRepositoryMock.Setup(repo => repo.UpdateUserAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            var userService = new UserService(userRepositoryMock.Object);

            await userService.UpdateUserExperienceAsync(userId, negativeExperience);

            Assert.Equal(initialExperience, user.Experience);
        }

        #endregion
    }
}
