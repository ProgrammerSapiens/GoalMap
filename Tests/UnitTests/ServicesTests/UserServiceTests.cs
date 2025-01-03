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
        public async Task GetUserById_ShouldReturnUser_WhenUserIdExists()
        {
            var userId = Guid.NewGuid();
            var expectedUser = new User("TestUser", "hashed_password", 100)
            {
                Tasks = new List<ToDo>(),
                Categories = new List<TaskCategory>()
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(expectedUser);

            var userService = new UserService(userRepositoryMock.Object);

            var result = await userService.GetUserByIdAsync(userId);

            Assert.NotNull(result);
            Assert.Equal(expectedUser.Id, result.Id);
            Assert.Equal(expectedUser.UserName, result.UserName);
            Assert.Equal(expectedUser.PasswordHash, result.PasswordHash);
            Assert.Equal(expectedUser.Experience, result.Experience);
            Assert.Empty(result.Tasks);
            Assert.Empty(result.Categories);
        }

        [Fact]
        public async Task GetUserById_ShouldThrowException_WhenUserIdDoesNotExist()
        {
            var userId = Guid.NewGuid();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((User?)null);

            var userService = new UserService(userRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => userService.GetUserByIdAsync(userId));
            Assert.Equal("User not found.", exception.Message);
        }

        [Fact]
        public async Task GetUserById_ShouldThrowException_WhenUserIdIsEmpty()
        {
            var userId = Guid.Empty;

            var userRepositoryMock = new Mock<IUserRepository>();
            var userService = new UserService(userRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => userService.GetUserByIdAsync(userId));
            Assert.Equal("User ID cannot be empty.", exception.Message);
        }

        #endregion

        #region AuthenticateUserAsync(string userName, string password) tests

        [Fact]
        public async Task AuthenticateUser_ShouldReturnTrue_WhenCredentialsAreValid()
        {

        }

        [Fact]
        public async Task AuthenticateUser_ShouldReturnFalse_WhenUserNameIsInvalid()
        {

        }

        [Fact]
        public async Task AuthenticateUser_ShouldReturnFalse_WhenPasswordIsInvalid()
        {

        }

        [Fact]
        public async Task AuthenticateUser_ShouldThrowException_WhenInputsAreNullOrEmpty()
        {

        }

        #endregion

        #region RegisterUserAsync(User user) tests

        [Fact]
        public async Task RegisterUser_ShouldSucceed_WhenUserIsValid()
        {

        }

        [Fact]
        public async Task RegisterUser_ShouldThrowException_WhenUsernameAlreadyExists()
        {

        }

        [Fact]
        public async Task RegisterUser_ShouldThrowException_WhenUserIsNull()
        {

        }

        #endregion

        #region UpdateUserExperienceAsync(Guid userId, int experiencePoints) tests

        [Fact]
        public async Task UpdateUserExperience_ShouldUpdateexperience_WhenInputsAreValid()
        {

        }

        [Fact]
        public async Task UpdateUserExperience_ShouldThrowException_WhenUserIdDoesNotExist()
        {

        }

        [Fact]
        public async Task UpdateUserExperience_ShouldHandleNegativeExperiencePoints()
        {

        }

        #endregion
    }
}
