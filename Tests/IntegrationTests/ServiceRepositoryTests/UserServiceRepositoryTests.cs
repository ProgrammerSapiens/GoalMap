using Core.Interfaces;
using Data.DBContext;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Core.Models;
using Core.Services;
using Microsoft.Extensions.Logging;
using Authentication;

namespace Tests.IntegrationTests.Service_RepositoriyTests
{
    public class UserServiceRepositoryTests : IAsyncLifetime
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly Mock<ILogger<UserService>> _serviceLoggerMock;
        private readonly Mock<ILogger<UserRepository>> _repositoryLoggerMock;
        private readonly Mock<ILogger<PasswordHasher>> _passwordHasherLoggerMock;
        private readonly IUserService _userService;
        private readonly AppDbContext _context;

        public UserServiceRepositoryTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _context = new AppDbContext(dbContextOptions);

            _serviceLoggerMock = new Mock<ILogger<UserService>>();
            _repositoryLoggerMock = new Mock<ILogger<UserRepository>>();
            _passwordHasherLoggerMock = new Mock<ILogger<PasswordHasher>>();

            _userRepository = new UserRepository(_context, _repositoryLoggerMock.Object);
            _passwordHasher = new PasswordHasher(_passwordHasherLoggerMock.Object);
            _userService = new UserService(_userRepository, _passwordHasher, _serviceLoggerMock.Object);
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

            var result = await _userService.GetUserByUserIdAsync(userId);

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

            var result = await _userService.GetUserByUserIdAsync(userId);
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

            var user = new User(userName);
            var userId = user.UserId;

            await _userService.RegisterUserAsync(user, password);

            var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            Assert.NotNull(userInDb);
            Assert.Equal(userId, userInDb.UserId);
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldCreateDefaultCategories_WhenUserIsValid()
        {
            var userName = "TestUser";
            var password = "password";

            var user = new User(userName);
            var userId = user.UserId;

            await _userService.RegisterUserAsync(user, password);

            var defaultToDoCategoriesInDb = await _context.ToDoCategories.Where(c => c.ToDoCategoryName == "Habbit" || c.ToDoCategoryName == "Other").ToListAsync();

            Assert.NotEmpty(defaultToDoCategoriesInDb);
            Assert.Equal(2, defaultToDoCategoriesInDb.Count);
            Assert.Contains(defaultToDoCategoriesInDb, c => c.ToDoCategoryName == "Habbit");
            Assert.Contains(defaultToDoCategoriesInDb, c => c.ToDoCategoryName == "Other");
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

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.RegisterUserAsync(user, password));

            Assert.Equal("User name is already exists.", exception.Message);
        }

        #endregion

        #region AuthenticateUserAsync(string userName, string? password)

        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnTrue_WhenCredentialsAreValid()
        {
            var user = new User("TestUser");
            var password = "password";
            var hashedPassword = await _passwordHasher.HashPasswordAsync(password);

            user.PasswordHash = hashedPassword;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = await _userService.AuthenticateUserAsync(user.UserName, password);
            Assert.True(result);
        }

        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnFalse_WhenCredentialsAreInvalid()
        {
            var user = new User("TestUser");
            var password = "password";
            var invalidPassword = "InvalidPassword";
            var hashedPassword = await _passwordHasher.HashPasswordAsync(password);

            user.PasswordHash = hashedPassword;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = await _userService.AuthenticateUserAsync(user.UserName, invalidPassword);
            Assert.False(result);
        }

        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            var user = new User("TestUser");
            var password = "password";

            var result = await _userService.AuthenticateUserAsync(user.UserName, password);
            Assert.False(result);
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
    }
}
