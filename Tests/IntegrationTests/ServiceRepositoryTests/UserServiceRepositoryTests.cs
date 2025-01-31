using Core.Interfaces;
using Data.DBContext;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Core.Models;
using Core.Services;

namespace Tests.IntegrationTests.Service_RepositoriyTests
{
    public class UserServiceRepositoryTests : IAsyncLifetime
    {
        private readonly IUserRepository _userRepository;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly IUserService userService;
        private readonly AppDbContext _context;

        public UserServiceRepositoryTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _context = new AppDbContext(dbContextOptions);

            _userRepository = new UserRepository(_context);
            _passwordHasherMock = new Mock<IPasswordHasher>();

            userService = new UserService(_userRepository, _passwordHasherMock.Object);
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }

        #region GetUserByUserIdAsync(Guid userId) tests

        [Fact]
        public async Task GetUserByUserIdAsync_ShouldReturnUser_WhenUserExists()
        {
            var userName = "TestUser";

            var user = new User(userName);
            var userId = user.UserId;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = await userService.GetUserByUserIdAsync(userId);

            Assert.NotNull(result);
            Assert.Equal(userName, result.UserName);
            Assert.Equal(userId, result.UserId);
        }

        [Fact]
        public async Task GetUserByUserIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            var userName = "TestUser";

            var user = new User(userName);
            var userId = user.UserId;

            var result = await userService.GetUserByUserIdAsync(userId);
            Assert.Null(result);

            var userInDb = await _context.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            Assert.Null(userInDb);
        }

        #endregion

        #region RegisterUserAsync(User user, string password) tests

        [Fact]
        public async Task RegisterUserAsync_ShouldSucceed_WhenUserIsValid()
        {
            var userName = "TestUser";
            var password = "password";
            var hashedPassword = "hashedPassword";

            var user = new User(userName);
            var userId = user.UserId;

            _passwordHasherMock.Setup(hasher => hasher.HashPasswordAsync(password)).ReturnsAsync(hashedPassword);

            await userService.RegisterUserAsync(user, password);

            var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            Assert.NotNull(userInDb);
            Assert.Equal(hashedPassword, userInDb.PasswordHash);
            Assert.Equal(userId, userInDb.UserId);
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldCreateDefaultCategories_WhenUserIsValid()
        {
            var userName = "TestUser";
            var password = "password";
            var hashedPassword = "hashedPassword";

            var user = new User(userName);
            var userId = user.UserId;

            _passwordHasherMock.Setup(hasher => hasher.HashPasswordAsync(password)).ReturnsAsync(hashedPassword);

            await userService.RegisterUserAsync(user, password);

            var defaultToDoCategoriesInDb = await _context.ToDoCategories.Where(c => c.ToDoCategoryName == "Habbit" || c.ToDoCategoryName == "Other").ToListAsync();

            Assert.NotEmpty(defaultToDoCategoriesInDb);
            Assert.Equal(2, defaultToDoCategoriesInDb.Count);
            Assert.Contains(defaultToDoCategoriesInDb, c => c.ToDoCategoryName == "Habbit");
            Assert.Contains(defaultToDoCategoriesInDb, c => c.ToDoCategoryName == "Other");
            Assert.All(defaultToDoCategoriesInDb, category =>
            {
                Assert.Equal(userName, category.User.UserName);
                Assert.Equal(userId, category.User.UserId);
            });
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldThrowException_WhenUserAlreadyExists()
        {
            var userName = "TestUser";
            var password = "password";

            var user = new User(userName);
            var userId = user.UserId;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => userService.RegisterUserAsync(user, password));

            Assert.Equal("User name is already exists.", exception.Message);
        }

        #endregion

        //TODO: Write AuthenticateUserAsync after the implementation of the authentication project
        #region AuthenticateUserAsync(string? userName, string? password)

        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnTrue_WhenCredentialsAreValid()
        {

        }

        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnFalse_WhenCredentialsAreInvalid()
        {

        }

        #endregion

        #region UpdateUserAsync(User user)

        [Fact]
        public async Task UpdateUserAsync_ShouldSucceed_WhenUserIsValid()
        {
            var userName = "TestUser";
            var user = new User(userName);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var newUserName = "NewUserName";
            user.UserName = newUserName;

            await _userRepository.UpdateUserAsync(user);

            var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId);
            Assert.NotNull(userInDb);
            Assert.Equal(newUserName, userInDb.UserName);
        }

        #endregion

        #region UpdateUserExperienceAsync(string? userName, Difficulty taskDifficulty)

        [Fact]
        public async Task UpdateUserExperienceAsync_ShouldUpdateUser_WhenUserExists()
        {
            var userName = "TestUser";
            int experience = 10;
            int updatedExperience = experience + (int)Difficulty.Nightmare;

            var user = new User(userName) { Experience = experience };
            var userId = user.UserId;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await userService.UpdateUserExperienceAsync(userId, Difficulty.Nightmare);

            var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            Assert.NotNull(userInDb);
            Assert.Equal(userName, userInDb.UserName);
            Assert.Equal(updatedExperience, userInDb.Experience);
        }

        [Fact]
        public async Task UpdateUserExperienceAsync_ShouldThrowException_WhenUserDoesNotExist()
        {
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => userService.UpdateUserExperienceAsync(Guid.NewGuid(), Difficulty.Nightmare));

            Assert.Equal("User was not found.", exception.Message);
        }

        #endregion
    }
}
