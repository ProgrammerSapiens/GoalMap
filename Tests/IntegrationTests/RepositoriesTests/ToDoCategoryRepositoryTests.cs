using Core.Interfaces;
using Core.Models;
using Data.DBContext;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.IntegrationTests.RepositoriesTests
{
    public class ToDoCategoryRepositoryTests : IAsyncLifetime
    {
        private readonly DbContextOptions<AppDbContext> dbContextOptions;
        private readonly AppDbContext context;

        public ToDoCategoryRepositoryTests()
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

        #region GetToDoCategoryByCategoryNameAsync(string toDoCategoryName, Guid userId) tests

        //[Fact]
        //public async Task GetToDoCategoryByCategoryNameAsync_ShouldReturnCategory_WhenCategoryExistsForUser()
        //{
        //    var userId = Guid.NewGuid();
        //    string categoryName = "TestCategory";
        //    var toDoCategory = new ToDoCategory(userId, categoryName);

        //    context.ToDoCategories.Add(toDoCategory);
        //    await context.SaveChangesAsync();

        //    var repository = new ToDoCategoryRepository(context);
        //    var result = await repository.GetToDoCategoryByCategoryIdAsync(userId, categoryName);

        //    Assert.NotNull(result);
        //    Assert.Equal(userId, result.UserId);
        //    Assert.Equal(categoryName, result.ToDoCategoryName);
        //    Assert.Equal(result.ToDoCategoryId, toDoCategory.ToDoCategoryId);
        //}

        //[Fact]
        //public async Task GetToDoCategoryByCategoryNameAsync_ShouldReturnNull_WhenCategoryDoesNotExistForUser()
        //{
        //    var userId = Guid.NewGuid();
        //    string categoryName = "TestCategory";
        //    var toDoCategory = new ToDoCategory(userId, categoryName);

        //    var repository = new ToDoCategoryRepository(context);
        //    var result = await repository.GetToDoCategoryByCategoryNameAsync(userId, categoryName);

        //    var toDoCategoryInDb = await context.ToDoCategories.FirstOrDefaultAsync(c => c.UserId == userId && c.ToDoCategoryName == categoryName);

        //    Assert.Null(result);
        //    Assert.Null(toDoCategoryInDb);
        //}

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

            context.ToDoCategories.AddRange(toDoCategories);
            await context.SaveChangesAsync();

            var repository = new ToDoCategoryRepository(context);
            var result = await repository.GetToDoCategoriesByUserIdAsync(userId);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, r => Assert.Equal(userId, r.UserId));
        }

        [Fact]
        public async Task GetToDoCategoriesByUserIdInRepositoryAsync_ShouldReturnEmptyList_WhenNoCategoriesExistForUser()
        {
            var userId = Guid.NewGuid();

            var repository = new ToDoCategoryRepository(context);
            var result = await repository.GetToDoCategoriesByUserIdAsync(userId);

            var toDoCategoriesInDb = await context.ToDoCategories.FirstOrDefaultAsync(c => c.UserId == userId);

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

            var repository = new ToDoCategoryRepository(context);
            await repository.AddToDoCategoryAsync(toDoCategory);

            var toDoCategoryInDb = await context.ToDoCategories.FirstOrDefaultAsync(c => c.ToDoCategoryName == toDoCategoryName && c.UserId == userId);

            Assert.NotNull(toDoCategoryInDb);
            Assert.Equal(userId, toDoCategoryInDb.UserId);
            Assert.Equal(toDoCategoryName, toDoCategoryInDb.ToDoCategoryName);
        }

        #endregion

        #region UpdateToDoCategoryAsync(ToDoCategory toDoCategory) tests

        [Fact]
        public async Task UpdateToDoCategoryAsync_ShouldUpdateCategory_WhenCategoryExists()
        {
            var userId = Guid.NewGuid();
            var toDoCategoryName = "TestCategory";
            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            context.ToDoCategories.Add(toDoCategory);
            await context.SaveChangesAsync();

            var newCategoryName = "NewTestCategory";
            toDoCategory.ToDoCategoryName = newCategoryName;

            var repository = new ToDoCategoryRepository(context);
            await repository.UpdateToDoCategoryAsync(toDoCategory);

            var toDoCategoryInDb = await context.ToDoCategories.FirstOrDefaultAsync(c => c.ToDoCategoryName == newCategoryName && c.UserId == userId);

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

            var repository = new ToDoCategoryRepository(context);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => repository.UpdateToDoCategoryAsync(toDoCategory));

            Assert.Equal("Category does not exist.", exception.Message);
        }

        #endregion

        #region DeleteToDoCategoryAsync(string toDoCategoryName, Guid userId) tests

        //[Fact]
        //public async Task DeleteToDoCategoryAsync_ShouldDeleteCategory_WhenCategoryExistsForUser()
        //{
        //    var userId = Guid.NewGuid();
        //    var toDoCategoryName = "TestCategory";
        //    var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

        //    context.ToDoCategories.Add(toDoCategory);
        //    await context.SaveChangesAsync();

        //    var repository = new ToDoCategoryRepository(context);
        //    await repository.DeleteToDoCategoryAsync(userId, toDoCategoryName);

        //    var toDoCategoryInDb = await context.ToDoCategories.FirstOrDefaultAsync(c => c.UserId == userId && c.ToDoCategoryName == toDoCategoryName);

        //    Assert.Null(toDoCategoryInDb);
        //}

        //[Fact]
        //public async Task DeleteToDoCategoryAsync_ShouldThrowException_WhenCategoryDoesNotExistForUser()
        //{
        //    var userId = Guid.NewGuid();
        //    var toDoCategoryName = "TestCategory";
        //    var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

        //    var repository = new ToDoCategoryRepository(context);

        //    var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => repository.DeleteToDoCategoryAsync(userId, toDoCategoryName));
        //    Assert.Equal("Category does not exist.", exception.Message);
        //}

        #endregion

        #region CategoryExistsAsync(string toDoCategoryName, Guid userId) tests

        [Fact]
        public async Task CategoryExistsAsync_ShouldReturnTrue_WhenCategoryExistsForUser()
        {
            var userId = Guid.NewGuid();
            var toDoCategoryName = "TestCategory";
            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            context.ToDoCategories.Add(toDoCategory);
            await context.SaveChangesAsync();

            var repository = new ToDoCategoryRepository(context);
            var result = await repository.CategoryExistsByNameAsync(userId, toDoCategoryName);

            var toDoCategoryInDb = await context.ToDoCategories.FirstOrDefaultAsync(c => c.UserId == userId && c.ToDoCategoryName == toDoCategoryName);

            Assert.True(result);
            Assert.NotNull(toDoCategoryInDb);
        }

        [Fact]
        public async Task CategoryExistsAsync_ShouldReturnFalse_WhenCategoryDoesNotExistForUser()
        {
            var userId = Guid.NewGuid();
            var toDoCategoryName = "TestCategory";

            var repository = new ToDoCategoryRepository(context);
            var result = await repository.CategoryExistsByNameAsync(userId, toDoCategoryName);

            var toDoCategoryInDb = await context.ToDoCategories.FirstOrDefaultAsync(c => c.UserId == userId && c.ToDoCategoryName == toDoCategoryName);

            Assert.False(result);
            Assert.Null(toDoCategoryInDb);
        }

        #endregion
    }
}
