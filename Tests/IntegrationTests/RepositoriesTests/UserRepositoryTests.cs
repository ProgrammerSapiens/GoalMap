using Core.Models;
using Data.DBContext;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.IntegrationTests.RepositoriesTests
{
    public class UserRepositoryTests : IAsyncLifetime
    {
        private readonly DbContextOptions<AppDbContext> dbContextOptions;
        private readonly AppDbContext context;

        public UserRepositoryTests()
        {
            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            context = new AppDbContext(dbContextOptions);
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        #region GetUserByUserNameAsync(string userName) tests

        //[Fact]
        //public async Task GetUserByUserNameAsync_ShouldReturnUser_WhenUserExists()
        //{
        //    var userName = "TestUser";
        //    var password = "hashedPassword";
        //    var existingUser = new User(userName, password);

        //    context.Users.Add(existingUser);
        //    await context.SaveChangesAsync();

        //    var repository = new UserRepository(context);

        //    var result = await repository.GetUserByUserNameAsync(userName);

        //    Assert.NotNull(result);
        //    Assert.Equal(userName, result.UserName);
        //    Assert.Equal(password, result.PasswordHash);
        //    Assert.Equal(100, result.Experience);
        //}

        [Fact]
        public async Task GetUserByUserNameAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            var repository = new UserRepository(context);

            var result = await repository.GetUserByUserNameAsync("NonExistentUser");

            Assert.Null(result);
        }

        #endregion

        #region AddUserAsync(User user) tests

        //[Fact]
        //public async Task AddUserAsync_ShouldAddUser_WhenDataIsValid()
        //{
        //    var repository = new UserRepository(context);
        //    var newUser = new User("NewUser", "hashedPassword");

        //    await repository.AddUserAsync(newUser);

        //    var userInDb = await context.Users.FirstOrDefaultAsync(u => u.UserName == "NewUser");
        //    Assert.NotNull(userInDb);
        //    Assert.Equal("NewUser", userInDb.UserName);
        //    Assert.Equal("hashedPassword", userInDb.PasswordHash);
        //    Assert.Equal(0, userInDb.Experience);
        //}

        #endregion

        #region UpdateUserAsync(User user) tests

        //[Fact]
        //public async Task UpdateUserAsync_ShouldUpdateUser_WhenUserExists()
        //{
        //    var repository = new UserRepository(context);
        //    var existingUser = new User("ExistingUser", "hashedPassword");

        //    context.Users.Add(existingUser);
        //    await context.SaveChangesAsync();

        //    string newName = "NewUserName";
        //    existingUser.UserName = newName;
        //    await repository.UpdateUserAsync(existingUser);

        //    var userInDb = await context.Users.FirstOrDefaultAsync(u => u.UserName == newName);
        //    Assert.NotNull(userInDb);
        //    Assert.Equal(existingUser.UserId, userInDb.UserId);
        //    Assert.Equal(newName, userInDb.UserName);
        //    Assert.Equal(existingUser.PasswordHash, userInDb.PasswordHash);
        //    Assert.Equal(existingUser.Experience, userInDb.Experience);
        //}

        //[Fact]
        //public async Task UpdateUserAsync_ShouldThrowException_WhenUserDoesNotExist()
        //{
        //    var repository = new UserRepository(context);

        //    var nonExistentUser = new User("NonExistentUser", "hashedPassword");

        //    var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => repository.UpdateUserAsync(nonExistentUser));

        //    Assert.Equal("User not found.", exception.Message);
        //}

        #endregion

        #region UserExistsAsync(string username) tests

        //[Fact]
        //public async Task UserExists_ShouldReturnTrue_WhenUserExists()
        //{
        //    var repository = new UserRepository(context);
        //    var existingUser = new User("ExistingUser", "hashedPassword", 10);

        //    context.Users.Add(existingUser);
        //    await context.SaveChangesAsync();

        //    var result = await repository.UserExistsAsync(existingUser.UserName);
        //    Assert.True(result);

        //    var userInDb = await context.Users.FirstOrDefaultAsync(u => u.UserName == existingUser.UserName);
        //    Assert.NotNull(userInDb);
        //}

        //[Fact]
        //public async Task UserExists_ShouldReturnFalse_WhenUserDoesNotExist()
        //{
        //    var repository = new UserRepository(context);

        //    var result = await repository.UserExistsAsync("SomeUser");
        //    Assert.False(result);

        //    var userInDb = await context.Users.FirstOrDefaultAsync(u => u.UserName == "SomeUser");
        //    Assert.Null(userInDb);
        //}

        #endregion
    }
}
