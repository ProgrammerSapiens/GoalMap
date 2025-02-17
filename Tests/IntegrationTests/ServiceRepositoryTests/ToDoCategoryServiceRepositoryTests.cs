using Core.Models;
using Core.Services;
using Data.DBContext;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.IntegrationTests.Service_RepositoriyTests
{
    public class ToDoCategoryServiceRepositoryTests : IAsyncLifetime
    {
        private readonly AppDbContext _context;
        private readonly ToDoCategoryRepository _toDoCategoryRepository;
        private readonly ToDoCategoryService _toDoCategoryService;
        private readonly Mock<ILogger<ToDoCategoryService>> _serviceLoggerMock;
        private readonly Mock<ILogger<ToDoCategoryRepository>> _repositoryLoggerMock;

        public ToDoCategoryServiceRepositoryTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _context = new AppDbContext(dbContextOptions);

            _serviceLoggerMock = new Mock<ILogger<ToDoCategoryService>>();
            _repositoryLoggerMock = new Mock<ILogger<ToDoCategoryRepository>>();

            _toDoCategoryRepository = new ToDoCategoryRepository(_context, _repositoryLoggerMock.Object);
            _toDoCategoryService = new ToDoCategoryService(_toDoCategoryRepository, _serviceLoggerMock.Object);
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
            var user = new User("TestUser");
            var userId = user.UserId;
            var expectedCategory = new ToDoCategory(userId, toDoCategoryName);
            var toDoCategoryId = expectedCategory.ToDoCategoryId;

            _context.ToDoCategories.Add(expectedCategory);
            await _context.SaveChangesAsync();

            var result = await _toDoCategoryService.GetToDoCategoryByCategoryIdAsync(toDoCategoryId);

            Assert.NotNull(result);
            Assert.Equal(expectedCategory.ToDoCategoryName, result.ToDoCategoryName);
            Assert.Equal(expectedCategory.ToDoCategoryId, result.ToDoCategoryId);
            Assert.Equal(expectedCategory.UserId, result.UserId);
        }

        [Fact]
        public async Task GetToDoCategoryByCategoryNameAsync_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            var toDoCategoryName = "Test Category";
            var user = new User("TestUser");
            var userId = user.UserId;
            var expectedCategory = new ToDoCategory(userId, toDoCategoryName);
            var toDoCategoryId = expectedCategory.ToDoCategoryId;

            var result = await _toDoCategoryService.GetToDoCategoryByCategoryIdAsync(toDoCategoryId);

            Assert.Null(result);
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

            var result = await _toDoCategoryService.GetToDoCategoriesByUserIdAsync(userId);

            Assert.NotNull(result);
            Assert.Equal(expectedCategories.Count, result.Count);
            Assert.All(result, category => Assert.Contains(expectedCategories, expC => expC.ToDoCategoryName == category.ToDoCategoryName));
        }

        [Fact]
        public async Task GetToDoCategoriesByUserIdAsync_ShouldReturnEmptyList_WhenNoCategoriesExist()
        {
            var userId = Guid.NewGuid();

            var result = await _toDoCategoryService.GetToDoCategoriesByUserIdAsync(userId);
            Assert.Empty(result);
        }

        #endregion

        #region AddToDoCategoryAsync(ToDoCategory toDoCategory) tests

        [Fact]
        public async Task AddToDoCategoryAsync_ShouldAddCategory_WhenCategoryIsValid()
        {
            var userId = Guid.NewGuid();
            string toDoCategoryName = "Test category";
            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            await _toDoCategoryService.AddToDoCategoryAsync(toDoCategory);

            var toDoCategoryInDb = await _context.ToDoCategories.FirstOrDefaultAsync(c => c.ToDoCategoryName == toDoCategoryName);

            Assert.NotNull(toDoCategoryInDb);
            Assert.Equal(userId, toDoCategoryInDb.UserId);
        }

        [Fact]
        public async Task AddToDoCategoryAsync_ShouldThrowException_WhenCategoryIsAlreadyExist()
        {
            var userId = Guid.NewGuid();
            string toDoCategoryName = "Test category";
            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            _context.ToDoCategories.Add(toDoCategory);
            await _context.SaveChangesAsync();

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _toDoCategoryService.AddToDoCategoryAsync(toDoCategory));

            Assert.Equal("Category with such name already exists.", exception.Message);
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

            var newToDoCategoryName = "Newname";
            toDoCategory.ToDoCategoryName = newToDoCategoryName;

            await _toDoCategoryService.UpdateToDoCategoryAsync(toDoCategory);

            var toDoCategoryInDb = await _context.ToDoCategories.FirstOrDefaultAsync(c => c.ToDoCategoryId == toDoCategoryId);

            Assert.NotNull(toDoCategoryInDb);
            Assert.Equal(userId, toDoCategoryInDb.UserId);
            Assert.Equal(newToDoCategoryName, toDoCategoryInDb.ToDoCategoryName);
        }

        [Fact]
        public async Task UpdateToDoCategoryAsync_ShouldThrowException_WhenCategoryDoesNotExist()
        {
            string toDoCategoryName = "Test category";
            var userId = Guid.NewGuid();
            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _toDoCategoryService.UpdateToDoCategoryAsync(toDoCategory));

            Assert.Equal("Category was not found.", exception.Message);
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

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _toDoCategoryService.UpdateToDoCategoryAsync(defaultCategory));

            Assert.Equal("You cannot add/update this category.", exception.Message);
        }

        #endregion

        #region DeleteToDoCategoryAsync(Guid toDoCategoryId) tests

        [Fact]
        public async Task DeleteToDoCategoryAsync_ShouldDeleteCategory_WhenCategoryExists()
        {
            var toDoCategoryName = "Test Category";
            var userId = Guid.NewGuid();
            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);
            var toDoCategoryId = toDoCategory.ToDoCategoryId;

            var defaultCategory = new ToDoCategory(userId, "Other");

            _context.ToDoCategories.Add(toDoCategory);
            _context.ToDoCategories.Add(defaultCategory);
            await _context.SaveChangesAsync();

            await _toDoCategoryService.DeleteToDoCategoryAsync(toDoCategoryId);

            var toDoCategoryInDb = await _context.ToDoCategories.FirstOrDefaultAsync(c => c.ToDoCategoryName == toDoCategoryName);

            Assert.Null(toDoCategoryInDb);
        }

        [Fact]
        public async Task DeleteToDoCategoryAsync_ShouldThrowException_WhenCategoryDoesNotExist()
        {
            var toDoCategoryId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _toDoCategoryService.DeleteToDoCategoryAsync(toDoCategoryId));

            Assert.Equal("ToDo category was not found.", exception.Message);
        }

        [Theory]
        [InlineData("Habbit")]
        [InlineData("Other")]
        public async Task DeleteToDoCategoryAsync_ShouldThrowException_WhenCategoryIsDefault(string toDoCategoryName)
        {
            var userId = Guid.NewGuid();

            var defaultCategory = new ToDoCategory(userId, toDoCategoryName);
            var toDoCategoryId = defaultCategory.ToDoCategoryId;

            _context.ToDoCategories.Add(defaultCategory);
            await _context.SaveChangesAsync();

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _toDoCategoryService.DeleteToDoCategoryAsync(toDoCategoryId));

            Assert.Equal("You cannot delete this category.", exception.Message);
        }

        #endregion
    }
}
