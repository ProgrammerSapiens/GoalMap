using Core.Models;
using Data.DBContext;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.IntegrationTests.RepositoriesTests
{
    public class ToDoCategoryRepositoryTests : IAsyncLifetime
    {
        private readonly AppDbContext _context;
        private readonly ToDoCategoryRepository _categoryRepository;

        public ToDoCategoryRepositoryTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _context = new AppDbContext(dbContextOptions);
            _categoryRepository = new ToDoCategoryRepository(_context);
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

        #region GetToDoCategoryByCategoryIdAsync(Guid userId, Guid toDoCategoryId) tests

        [Fact]
        public async Task GetToDoCategoryByCategoryIdAsync_ShouldReturnCategory_WhenCategoryExistsForUser()
        {
            var userId = Guid.NewGuid();
            string categoryName = "TestCategory";
            var toDoCategory = new ToDoCategory(userId, categoryName);
            var toDoCategoryId = toDoCategory.ToDoCategoryId;

            _context.ToDoCategories.Add(toDoCategory);
            await _context.SaveChangesAsync();

            var result = await _categoryRepository.GetToDoCategoryByCategoryIdAsync(toDoCategoryId);

            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal(categoryName, result.ToDoCategoryName);
            Assert.Equal(result.ToDoCategoryId, toDoCategory.ToDoCategoryId);
        }

        [Fact]
        public async Task GetToDoCategoryByCategoryIdAsync_ShouldReturnNull_WhenCategoryDoesNotExistForUser()
        {
            var userId = Guid.NewGuid();
            string categoryName = "TestCategory";
            var toDoCategory = new ToDoCategory(userId, categoryName);
            var toDoCategoryId = toDoCategory.ToDoCategoryId;

            var result = await _categoryRepository.GetToDoCategoryByCategoryIdAsync(toDoCategoryId);

            var toDoCategoryInDb = await _context.ToDoCategories.FirstOrDefaultAsync(c => c.UserId == userId && c.ToDoCategoryName == categoryName);

            Assert.Null(result);
            Assert.Null(toDoCategoryInDb);
        }

        #endregion

        #region GetToDoCategoriesByUserIdAsync(Guid userId) tests

        [Fact]
        public async Task GetToDoCategoriesByUserIdAsync_ShouldReturnCategories_WhenCategoriesExistForUser()
        {
            var userId = Guid.NewGuid();

            var toDoCategories = new List<ToDoCategory>()
                {
                    new ToDoCategory(userId, "TestCategory"),
                    new ToDoCategory(userId, "TestCategory2"),
                    new ToDoCategory(Guid.NewGuid(),"TestCategory3")
                };

            _context.ToDoCategories.AddRange(toDoCategories);
            await _context.SaveChangesAsync();

            var result = await _categoryRepository.GetToDoCategoriesByUserIdAsync(userId);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, r => Assert.Equal(userId, r.UserId));
        }

        [Fact]
        public async Task GetToDoCategoriesByUserIdInRepositoryAsync_ShouldReturnEmptyList_WhenNoCategoriesExistForUser()
        {
            var userId = Guid.NewGuid();

            var result = await _categoryRepository.GetToDoCategoriesByUserIdAsync(userId);

            var toDoCategoriesInDb = await _context.ToDoCategories.FirstOrDefaultAsync(c => c.UserId == userId);

            Assert.Empty(result);
            Assert.Null(toDoCategoriesInDb);
        }

        #endregion

        #region AddToDoCategoryAsync(ToDoCategory toDoCategory) tests

        [Fact]
        public async Task AddToDoCategoryAsync_ShouldAddCategory_WhenDataIsValid()
        {
            var userId = Guid.NewGuid();
            var toDoCategoryName = "TestCategoryName";
            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            await _categoryRepository.AddToDoCategoryAsync(toDoCategory);

            var toDoCategoryInDb = await _context.ToDoCategories.FirstOrDefaultAsync(c => c.ToDoCategoryName == toDoCategoryName && c.UserId == userId);

            Assert.NotNull(toDoCategoryInDb);
            Assert.Equal(userId, toDoCategoryInDb.UserId);
            Assert.Equal(toDoCategoryName, toDoCategoryInDb.ToDoCategoryName);
        }

        [Fact]
        public async Task AddToDoCategoryAsync_ShouldThrowException_WhenCategoryExists()
        {
            var userId = Guid.NewGuid();
            var toDoCategoryName = "TestCategoryName";
            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            _context.ToDoCategories.Add(toDoCategory);
            await _context.SaveChangesAsync();

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _categoryRepository.AddToDoCategoryAsync(toDoCategory));
            Assert.Equal("Todo category already exists.", exception.Message);
        }

        #endregion

        #region UpdateToDoCategoryAsync(ToDoCategory toDoCategory) tests

        [Fact]
        public async Task UpdateToDoCategoryAsync_ShouldUpdateCategory_WhenCategoryExists()
        {
            var userId = Guid.NewGuid();
            var toDoCategoryName = "TestCategory";
            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            _context.ToDoCategories.Add(toDoCategory);
            await _context.SaveChangesAsync();

            var newCategoryName = "NewTestCategory";
            toDoCategory.ToDoCategoryName = newCategoryName;

            await _categoryRepository.UpdateToDoCategoryAsync(toDoCategory);

            var toDoCategoryInDb = await _context.ToDoCategories.FirstOrDefaultAsync(c => c.ToDoCategoryName == newCategoryName && c.UserId == userId);

            Assert.NotNull(toDoCategoryInDb);
            Assert.Equal(userId, toDoCategoryInDb.UserId);
            Assert.Equal(newCategoryName, toDoCategoryInDb.ToDoCategoryName);
        }

        [Fact]
        public async Task UpdateToDoCategoryAsync_ShouldThrowException_WhenCategoryDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var toDoCategoryName = "TestCategory";
            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _categoryRepository.UpdateToDoCategoryAsync(toDoCategory));

            Assert.Equal("Category does not exist.", exception.Message);
        }

        #endregion

        #region UpdateCategoryInToDosAsync(Guid userId, string oldCategoryName, string newCategoryName) tests

        [Fact]
        public async Task UpdateCategoryInToDosAsync_ShouldUpdateCategoryInToDos_WhenCategoryUpdates()
        {
            var userId = Guid.NewGuid();
            var oldCategoryId = Guid.NewGuid();
            var newCategoryId = Guid.NewGuid();
            var otherCategoryId = Guid.NewGuid();

            var toDos = new List<ToDo>()
            {
                new ToDo("TestToDo",TimeBlock.Day,Difficulty.Easy,DateTime.Today,oldCategoryId,userId),
                new ToDo("TestToDo2",TimeBlock.Week,Difficulty.Hard,DateTime.Today,oldCategoryId,userId),
                new ToDo("TestToDo3",TimeBlock.Month,Difficulty.Medium,DateTime.Today,otherCategoryId,userId)
            };

            _context.ToDos.AddRange(toDos);
            await _context.SaveChangesAsync();

            await _categoryRepository.UpdateCategoryInToDosAsync(userId, oldCategoryId, newCategoryId);

            var toDosInDb = await _context.ToDos.Where(t => t.UserId == userId && t.ToDoCategoryId == newCategoryId).ToListAsync();
            Assert.NotEmpty(toDosInDb);
            Assert.Equal(2, toDosInDb.Count);

            var toDoInDb = await _context.ToDos.FirstOrDefaultAsync(t => t.UserId == userId && t.ToDoCategoryId == otherCategoryId);
            Assert.NotNull(toDoInDb);
            Assert.Equal("TestToDo3", toDoInDb.Description);
        }

        #endregion

        #region  DeleteToDoCategoryAsync(Guid userId, Guid toDoCategoryId) tests

        [Fact]
        public async Task DeleteToDoCategoryAsync_ShouldDeleteCategory_WhenCategoryExistsForUser()
        {
            var userId = Guid.NewGuid();
            var toDoCategoryName = "TestCategory";
            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);
            var toDoCategoryId = toDoCategory.ToDoCategoryId;

            _context.ToDoCategories.Add(toDoCategory);
            await _context.SaveChangesAsync();

            await _categoryRepository.DeleteToDoCategoryAsync(toDoCategoryId);

            var toDoCategoryInDb = await _context.ToDoCategories.FirstOrDefaultAsync(c => c.ToDoCategoryId == toDoCategoryId);

            Assert.Null(toDoCategoryInDb);
        }

        [Fact]
        public async Task DeleteToDoCategoryAsync_ShouldThrowException_WhenCategoryDoesNotExistForUser()
        {
            var userId = Guid.NewGuid();
            var toDoCategoryName = "TestCategory";
            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);
            var toDoCategoryId = toDoCategory.ToDoCategoryId;

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _categoryRepository.DeleteToDoCategoryAsync(toDoCategoryId));
            Assert.Equal("Category does not exist.", exception.Message);
        }

        #endregion

        #region CategoryExistsAsync(string toDoCategoryName, Guid userId) tests

        [Fact]
        public async Task CategoryExistsAsync_ShouldReturnTrue_WhenCategoryExistsForUser()
        {
            var userId = Guid.NewGuid();
            var toDoCategoryName = "TestCategory";
            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            _context.ToDoCategories.Add(toDoCategory);
            await _context.SaveChangesAsync();

            var result = await _categoryRepository.CategoryExistsByNameAsync(userId, toDoCategoryName);

            var toDoCategoryInDb = await _context.ToDoCategories.FirstOrDefaultAsync(c => c.UserId == userId && c.ToDoCategoryName == toDoCategoryName);

            Assert.True(result);
            Assert.NotNull(toDoCategoryInDb);
        }

        [Fact]
        public async Task CategoryExistsAsync_ShouldReturnFalse_WhenCategoryDoesNotExistForUser()
        {
            var userId = Guid.NewGuid();
            var toDoCategoryName = "TestCategory";

            var result = await _categoryRepository.CategoryExistsByNameAsync(userId, toDoCategoryName);

            var toDoCategoryInDb = await _context.ToDoCategories.FirstOrDefaultAsync(c => c.UserId == userId && c.ToDoCategoryName == toDoCategoryName);

            Assert.False(result);
            Assert.Null(toDoCategoryInDb);
        }

        #endregion
    }
}
