using Core.Models;
using Core.Services;
using Data.DBContext;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.IntegrationTests.Service_RepositoriyTests
{
    public class ToDoCategoryServiceRepositoryTests : IAsyncLifetime
    {
        private readonly AppDbContext _context;
        private readonly ToDoCategoryRepository _toDoCategoryRepository;

        public ToDoCategoryServiceRepositoryTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _context = new AppDbContext(dbContextOptions);

            _toDoCategoryRepository = new ToDoCategoryRepository(_context);
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

        #region GetToDoCategoryByCategoryNameAsync(Guid userId, string toDoCategoryName) tests

        [Fact]
        public async Task GetToDoCategoryByCategoryNameAsync_ShouldReturnCategory_WhenCategoryNameExists()
        {
            var toDoCategoryName = "Test Category";
            var user = new User("TestUser", "hashedPassoword");
            var userId = user.UserId;
            var expectedCategory = new ToDoCategory(userId, toDoCategoryName)
            {
                User = user
            };

            _context.ToDoCategories.Add(expectedCategory);
            await _context.SaveChangesAsync();

            var toDoCategoryService = new ToDoCategoryService(_toDoCategoryRepository);

            var result = await toDoCategoryService.GetToDoCategoryByCategoryNameAsync(userId, toDoCategoryName);

            Assert.NotNull(result);
            Assert.Equal(expectedCategory.ToDoCategoryName, result.ToDoCategoryName);
            Assert.Equal(expectedCategory.ToDoCategoryId, result.ToDoCategoryId);
            Assert.Equal(expectedCategory.UserId, result.UserId);
            Assert.NotNull(result.User);
            Assert.Equal(expectedCategory.User.UserName, result.User.UserName);
            Assert.Equal(expectedCategory.User.PasswordHash, result.User.PasswordHash);
            Assert.Equal(expectedCategory.User.Experience, result.User.Experience);
        }

        #endregion

        #region GetToDoCategoriesByUserIdAsync(Guid userId) tests

        [Fact]
        public async Task GetToDoCategoriesByUserIdAsync_ShouldReturnCategoryList_WhenCategoriesExist()
        {
            var userId = Guid.NewGuid();
            var expectedCategories = new List<ToDoCategory>
            {
                new ToDoCategory(userId, "Test Category1"),
                new ToDoCategory(userId, "Test Category2")
            };

            _context.ToDoCategories.AddRange(expectedCategories);
            await _context.SaveChangesAsync();

            var toDoCategoryService = new ToDoCategoryService(_toDoCategoryRepository);

            var result = await toDoCategoryService.GetToDoCategoriesByUserIdAsync(userId);

            Assert.NotNull(result);
            Assert.Equal(expectedCategories.Count, result.Count);
            Assert.All(result, category => Assert.Contains(expectedCategories, expC => expC.ToDoCategoryName == category.ToDoCategoryName));
        }

        #endregion

        #region AddToDoCategoryAsync(ToDoCategory toDoCategory) tests

        [Fact]
        public async Task AddToDoCategoryAsync_ShouldAddCategory_WhenCategoryIsValid()
        {
            var userId = Guid.NewGuid();
            string toDoCategoryName = "Test Category";
            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            var toDoCategoryService = new ToDoCategoryService(_toDoCategoryRepository);

            await toDoCategoryService.AddToDoCategoryAsync(toDoCategory);

            var toDoCategoryInDb = await _context.ToDoCategories.FirstOrDefaultAsync(c => c.ToDoCategoryName == toDoCategoryName);

            Assert.NotNull(toDoCategoryInDb);
            Assert.Equal(userId, toDoCategoryInDb.UserId);
        }

        [Fact]
        public async Task AddToDoCategoryAsync_ShouldThrowException_WhenCategoryIsAlreadyExist()
        {
            var userId = Guid.NewGuid();
            string toDoCategoryName = "Test Category";
            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            _context.ToDoCategories.Add(toDoCategory);
            await _context.SaveChangesAsync();

            var toDoCategoryService = new ToDoCategoryService(_toDoCategoryRepository);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => toDoCategoryService.AddToDoCategoryAsync(toDoCategory));

            Assert.Equal("Сategory already exists.", exception.Message);
        }

        #endregion

        #region UpdateToDoCategoryAsync(ToDoCategory toDoCategory) tests

        [Fact]
        public async Task UpdateToDoCategoryAsync_ShouldUpdateCategory_WhenCategoryIsValid()
        {
            string toDoCategoryName = "Test Category";
            var userId = Guid.NewGuid();
            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);
            var toDoCategoryId = toDoCategory.ToDoCategoryId;

            _context.ToDoCategories.Add(toDoCategory);
            await _context.SaveChangesAsync();

            var newToDoCategoryName = "NewName";
            toDoCategory.ToDoCategoryName = newToDoCategoryName;

            var toDoCategoryService = new ToDoCategoryService(_toDoCategoryRepository);

            await toDoCategoryService.UpdateToDoCategoryAsync(toDoCategory);

            var toDoCategoryInDb = await _context.ToDoCategories.FirstOrDefaultAsync(c => c.ToDoCategoryId == toDoCategoryId);

            Assert.NotNull(toDoCategoryInDb);
            Assert.Equal(userId, toDoCategoryInDb.UserId);
            Assert.Equal(newToDoCategoryName, toDoCategoryInDb.ToDoCategoryName);
        }

        [Fact]
        public async Task UpdateToDoCategoryAsync_ShouldThrowException_WhenCategoryDoesNotExist()
        {
            string toDoCategoryName = "Test Category";
            var userId = Guid.NewGuid();
            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            var toDoCategoryService = new ToDoCategoryService(_toDoCategoryRepository);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => toDoCategoryService.UpdateToDoCategoryAsync(toDoCategory));

            Assert.Equal("Category does not exist.", exception.Message);
        }

        [Theory]
        [InlineData("Habbit")]
        [InlineData("Other")]
        public async Task UpdateToDoCategoryAsync_ShouldThrowException_WhenCategoryIsDefault(string toDoCategoryName)
        {
            var userId = Guid.NewGuid();
            var defaultCategory = new ToDoCategory(userId, toDoCategoryName);

            _context.ToDoCategories.Add(defaultCategory);
            await _context.SaveChangesAsync();

            var toDoCategoryService = new ToDoCategoryService(_toDoCategoryRepository);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => toDoCategoryService.UpdateToDoCategoryAsync(defaultCategory));

            Assert.Equal("Category with such name already exists.", exception.Message);
        }

        #endregion

        #region DeleteToDoCategoryAsync(Guid toDoCategoryId) tests

        [Fact]
        public async Task DeleteToDoCategoryAsync_ShouldDeleteCategory_WhenCategoryExists()
        {
            var toDoCategoryName = "Test Category";
            var userId = Guid.NewGuid();
            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            _context.ToDoCategories.Add(toDoCategory);
            await _context.SaveChangesAsync();

            var toDoCategoryService = new ToDoCategoryService(_toDoCategoryRepository);

            await toDoCategoryService.DeleteToDoCategoryAsync(userId, toDoCategoryName);

            var toDoCategoryInDb = await _context.ToDoCategories.FirstOrDefaultAsync(c => c.ToDoCategoryName == toDoCategoryName);

            Assert.Null(toDoCategoryInDb);
        }

        [Fact]
        public async Task DeleteToDoCategoryAsync_ShouldThrowException_WhenCategoryDoesNotExist()
        {
            var toDoCategoryName = "Test Category";
            var userId = Guid.NewGuid();

            var toDoCategoryService = new ToDoCategoryService(_toDoCategoryRepository);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => toDoCategoryService.DeleteToDoCategoryAsync(userId, toDoCategoryName));

            Assert.Equal("Category does not exist.", exception.Message);
        }

        [Theory]
        [InlineData("Habbit")]
        [InlineData("Other")]
        public async Task DeleteToDoCategoryAsync_ShouldThrowException_WhenCategoryIsDefault(string toDoCategoryName)
        {
            var userId = Guid.NewGuid();

            var defaultCategory = new ToDoCategory(userId, toDoCategoryName);

            _context.ToDoCategories.Add(defaultCategory);
            await _context.SaveChangesAsync();

            var toDoCategoryService = new ToDoCategoryService(_toDoCategoryRepository);

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => toDoCategoryService.DeleteToDoCategoryAsync(userId, toDoCategoryName));

            Assert.Equal("You cannot delete this category.", exception.Message);
        }

        #endregion
    }
}
