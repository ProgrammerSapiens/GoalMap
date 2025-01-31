using Core.Models;
using Data.DBContext;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.IntegrationTests.RepositoriesTests
{
    public class UserRepositoryTests : IAsyncLifetime
    {
        private readonly AppDbContext _context;
        private readonly UserRepository _userRepository;

        public UserRepositoryTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _context = new AppDbContext(dbContextOptions);
            _userRepository = new UserRepository(_context);
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
            var existingUser = new User(userName);
            var userId = existingUser.UserId;

            _context.Users.Add(existingUser);
            await _context.SaveChangesAsync();

            var result = await _userRepository.GetUserByUserIdAsync(userId);

            Assert.NotNull(result);
            Assert.Equal(userName, result.UserName);
        }

        [Fact]
        public async Task GetUserByUserIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            var result = await _userRepository.GetUserByUserIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        #endregion

        #region GetUserByUserNameAsync(string userName) tests

        [Fact]
        public async Task GetUserByUserNameAsync_ShouldReturnUser_WhenUserExists()
        {
            var userName = "TestUser";
            var existingUser = new User(userName);

            _context.Users.Add(existingUser);
            await _context.SaveChangesAsync();

            var result = await _userRepository.GetUserByUserNameAsync(userName);

            Assert.NotNull(result);
            Assert.Equal(userName, result.UserName);
        }

        [Fact]
        public async Task GetUserByUserNameAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            var result = await _userRepository.GetUserByUserNameAsync("Non-existent user");

            Assert.Null(result);
        }

        #endregion

        #region AddUserAsync(User user) tests

        [Fact]
        public async Task AddUserAsync_ShouldAddUser_WhenDataIsValid()
        {
            var newUser = new User("NewUser");

            await _userRepository.AddUserAsync(newUser);

            var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.UserName == "NewUser");
            Assert.NotNull(userInDb);
        }

        [Fact]
        public async Task AddUserAsync_ShouldThrowException_WhenUserNameAlreadyExists()
        {
            var newUser = new User("User");

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _userRepository.AddUserAsync(newUser));
            Assert.Equal("Username already exists.", exception.Message);
        }

        #endregion

        #region UpdateUserAsync(User user) tests

        [Fact]
        public async Task UpdateUserAsync_ShouldUpdateUser_WhenUserExists()
        {
            var existingUser = new User("ExistingUser");

            _context.Users.Add(existingUser);
            await _context.SaveChangesAsync();

            string newName = "NewUserName";
            existingUser.UserName = newName;
            await _userRepository.UpdateUserAsync(existingUser);

            var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.UserName == newName);
            Assert.NotNull(userInDb);
            Assert.Equal(existingUser.UserId, userInDb.UserId);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldThrowException_WhenUserDoesNotExist()
        {
            var nonExistentUser = new User("NonExistentUser");

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _userRepository.UpdateUserAsync(nonExistentUser));

            Assert.Equal("User not found.", exception.Message);
        }

        #endregion

        #region AddDefaultCategoriesAsync(List<ToDoCategory> defaultCategories) tests

        [Fact]
        public async Task AddDefaultCategoriesAsync_ShouldUpdateCategories_WhenUserUpdates()
        {
            var userId = Guid.NewGuid();

            var toDoCategories = new List<ToDoCategory>()
            {
                new ToDoCategory(userId,"Habbit"),
                new ToDoCategory(userId,"Other")
            };

            await _userRepository.AddDefaultCategoriesAsync(toDoCategories);

            var toDoCategoriesInDb = await _context.ToDoCategories.Where(c => c.UserId == userId).ToListAsync();
            Assert.NotNull(toDoCategoriesInDb);
            Assert.Contains(toDoCategoriesInDb, category => category.ToDoCategoryName == "Habbit" || category.ToDoCategoryName == "Other");
        }

        #endregion
    }
}
