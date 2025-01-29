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
        private readonly IToDoCategoryRepository _toDoCategoryRepository;
        private readonly IToDoCategoryService _toDoCategoryService;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly AppDbContext _context;

        public UserServiceRepositoryTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _context = new AppDbContext(dbContextOptions);

            _userRepository = new UserRepository(_context);
            _toDoCategoryRepository = new ToDoCategoryRepository(_context);
            _toDoCategoryService = new ToDoCategoryService(_toDoCategoryRepository);
            _passwordHasherMock = new Mock<IPasswordHasher>();
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

        #region GetUserByUserNameAsync(string? userName)

        [Fact]
        public async Task GetUserByUserNameAsync_ShouldReturnUser_WhenUserExists()
        {
            var userName = "TestUser";
            var hashedPassword = "hashedPassword";

            var user = new User(userName, hashedPassword);
            var userId = user.UserId;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userService = new UserService(_userRepository, _passwordHasherMock.Object, _toDoCategoryService);

            var result = await userService.GetUserByUserNameAsync(userName);

            Assert.NotNull(result);
            Assert.Equal(userName, result.UserName);
            Assert.Equal(hashedPassword, result.PasswordHash);
            Assert.Equal(userId, result.UserId);
        }

        #endregion

        #region UpdateUserExperienceAsync(string? userName, Difficulty taskDifficulty)

        [Fact]
        public async Task UpdateUserExperienceAsync_ShouldUpdateUser_WhenUserExists()
        {
            var userName = "TestUser";
            var hashedPassword = "hashedPassword";
            int experience = 10;
            int updatedExperience = experience + (int)Difficulty.Nightmare;

            var user = new User(userName, hashedPassword, experience);
            var userId = user.UserId;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userService = new UserService(_userRepository, _passwordHasherMock.Object, _toDoCategoryService);

            await userService.UpdateUserExperienceAsync(userName, Difficulty.Nightmare);

            var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            Assert.NotNull(userInDb);
            Assert.Equal(userName, userInDb.UserName);
            Assert.Equal(hashedPassword, userInDb.PasswordHash);
            Assert.Equal(updatedExperience, userInDb.Experience);
        }

        [Fact]
        public async Task UpdateUserExperienceAsync_ShouldThrowException_WhenUserDoesNotExist()
        {
            var userName = "TestUser";

            var userService = new UserService(_userRepository, _passwordHasherMock.Object, _toDoCategoryService);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => userService.UpdateUserExperienceAsync(userName, Difficulty.Nightmare));

            Assert.Equal("User does not exist.", exception.Message);
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

        #region RegisterUserAsync(User user, string? password)

        [Fact]
        public async Task RegisterUserAsync_ShouldSucceed_WhenUserIsValid()
        {
            var userName = "TestUser";
            var password = "password";
            var hashedPassword = "hashedPassword";

            var user = new User(userName, password);
            var userId = user.UserId;

            _passwordHasherMock.Setup(hasher => hasher.HashPasswordAsync(password)).ReturnsAsync(hashedPassword);

            var userService = new UserService(_userRepository, _passwordHasherMock.Object, _toDoCategoryService);

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

            var user = new User(userName, password);
            var userId = user.UserId;

            _passwordHasherMock.Setup(hasher => hasher.HashPasswordAsync(password)).ReturnsAsync(hashedPassword);

            var userService = new UserService(_userRepository, _passwordHasherMock.Object, _toDoCategoryService);

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

            var user = new User(userName, password);
            var userId = user.UserId;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userService = new UserService(_userRepository, _passwordHasherMock.Object, _toDoCategoryService);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => userService.RegisterUserAsync(user, password));

            Assert.Equal("User name is already exists.", exception.Message);
        }

        #endregion
    }
}
