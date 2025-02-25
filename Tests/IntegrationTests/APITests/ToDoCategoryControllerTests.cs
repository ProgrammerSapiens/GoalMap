using Core.Models;
using Data.DBContext;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;
using API;
using Xunit.Abstractions;
using Core.DTOs.ToDoCategory;

namespace Tests.IntegrationTests.APITests
{
    public class ToDoCategoryControllerTests : IAsyncLifetime
    {
        private HttpClient _client;
        private CustomWebApplicationFactory<Program> _factory;
        private AppDbContext _dbContext;
        private IServiceScope _scope;
        private readonly ITestOutputHelper _outputHelper;

        public ToDoCategoryControllerTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        public async Task InitializeAsync()
        {
            var dbName = Guid.NewGuid().ToString();

            _factory = new CustomWebApplicationFactory<Program>(dbName, _outputHelper);
            _client = _factory.CreateClient();

            _scope = _factory.Services.CreateScope();
            _dbContext = _scope.ServiceProvider.GetRequiredService<AppDbContext>();

            _dbContext.ToDoCategories.RemoveRange(_dbContext.ToDoCategories);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            await _dbContext.DisposeAsync();
            _scope.Dispose();
            _client.Dispose();
            await _factory.DisposeAsync();
        }

        #region GetToDoCategoryByCategoryName tests

        [Fact]
        public async Task GetToDoCategoryByCategoryName_ShouldReturnCategoryByName()
        {
            var userId = Guid.Parse("80a87a51-d544-4653-ae91-c6395e5fd8ce");
            var toDoCategoryName = "TestCategory";

            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            _dbContext.ToDoCategories.Add(toDoCategory);
            await _dbContext.SaveChangesAsync();

            var response = await _client.GetAsync($"/api/todocategories/{toDoCategory.ToDoCategoryId}");

            response.EnsureSuccessStatusCode();

            var toDoCategoryResponse = await response.Content.ReadFromJsonAsync<CategoryDto>();
            Assert.NotNull(toDoCategoryResponse);
            Assert.Equal(userId, toDoCategoryResponse.UserId);
            Assert.Equal(toDoCategoryName, toDoCategoryResponse.ToDoCategoryName);
        }

        [Fact]
        public async Task GetToDoCategoryByCategoryName_ShouldReturnOtherCategory_WhenCategoryDoesNotExist()
        {
            var toDoCategoryName = "TestCategory";

            var response = await _client.GetAsync($"api/todocategories/{toDoCategoryName}");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion

        #region GetToDoCategoriesByUserId tests

        [Fact]
        public async Task GetToDoCategoriesByUserId_ShouldReturnCategoriesForCurrentUser()
        {
            var userId = Guid.Parse("80a87a51-d544-4653-ae91-c6395e5fd8ce");

            var categories = new List<ToDoCategory>
            {
                new ToDoCategory(userId, "Category1"),
                new ToDoCategory(userId, "Category2")
            };

            _dbContext.ToDoCategories.AddRange(categories);
            await _dbContext.SaveChangesAsync();

            var response = await _client.GetAsync($"/api/todocategories/user");
            response.EnsureSuccessStatusCode();

            var toDoCategoryResponse = await response.Content.ReadFromJsonAsync<List<CategoryDto>>();
            Assert.NotNull(toDoCategoryResponse);
            Assert.Equal(2, toDoCategoryResponse.Count);
            Assert.All(toDoCategoryResponse, category => Assert.Equal(userId, category.UserId));
        }

        [Fact]
        public async Task GetToDoCategoriesByUserId_ShouldReturnEmptyList_WhenNoCategoriesExist()
        {
            var response = await _client.GetAsync($"/api/todocategories/user");
            response.EnsureSuccessStatusCode();

            var toDoCategoryResponse = await response.Content.ReadFromJsonAsync<List<ToDoCategory>>();
            Assert.NotNull(toDoCategoryResponse);
            Assert.Empty(toDoCategoryResponse);
        }

        #endregion

        #region AddToDoCategory tests

        [Fact]
        public async Task AddToDoCategory_ShouldSuccessfullyCreateCategoryForCurrentUser()
        {
            var userId = Guid.Parse("80a87a51-d544-4653-ae91-c6395e5fd8ce");
            var toDoCategory = new ToDoCategory(userId, "Test Category");

            var categoryAddOrUpdateDto = new CategoryAddOrUpdateDto { ToDoCategoryId = toDoCategory.ToDoCategoryId, ToDoCategoryName = "Test Category", UserId = userId };

            var content = new StringContent(JsonConvert.SerializeObject(categoryAddOrUpdateDto), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/todocategories", content);
            response.EnsureSuccessStatusCode();

            var categoryInDb = await _dbContext.ToDoCategories.FirstOrDefaultAsync(c => c.ToDoCategoryName == "Test category" && c.UserId == userId);

            Assert.NotNull(categoryInDb);
            Assert.Equal("Test category", categoryInDb.ToDoCategoryName);
            Assert.Equal(userId, categoryInDb.UserId);
        }

        [Fact]
        public async Task AddToDoCategory_ShouldReturnError_WhenCategoryAlreadyExists()
        {
            var userId = Guid.NewGuid();
            var toDoCategory = new ToDoCategory(userId, "TestCategory");

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.ToDoCategories.Add(toDoCategory);
                await dbContext.SaveChangesAsync();
            }

            var content = new StringContent(JsonConvert.SerializeObject(toDoCategory), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/todocategories", content);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion

        #region UpdateToDoCategory tests

        [Fact]
        public async Task UpdateToDoCategory_ShouldSuccessfullyUpdateCategoryName()
        {
            var userId = Guid.Parse("80a87a51-d544-4653-ae91-c6395e5fd8ce");
            var toDoCategory = new ToDoCategory(userId, "TestCategory");
            var toDoCategoryId = toDoCategory.ToDoCategoryId;

            var categoryAddOrUpdateDto = new CategoryAddOrUpdateDto { ToDoCategoryId = toDoCategoryId, ToDoCategoryName = "TestCategory", UserId = userId };

            _dbContext.ToDoCategories.Add(toDoCategory);
            await _dbContext.SaveChangesAsync();

            var newToDoCategoryName = "Newname";

            toDoCategory.ToDoCategoryName = newToDoCategoryName;

            var content = new StringContent(JsonConvert.SerializeObject(categoryAddOrUpdateDto), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync("/api/todocategories", content);
            response.EnsureSuccessStatusCode();

            var toDoCategoryInDb = await _dbContext.ToDoCategories.FirstOrDefaultAsync(c => c.ToDoCategoryId == toDoCategoryId);

            Assert.NotNull(toDoCategoryInDb);
            Assert.Equal(newToDoCategoryName, toDoCategoryInDb.ToDoCategoryName);
        }

        [Fact]
        public async Task UpdateToDoCategory_ShouldReturnError_WhenCategoryNameAlreadyExists()
        {
            var userId = Guid.Parse("80a87a51-d544-4653-ae91-c6395e5fd8ce");

            var toDoCategory1 = new ToDoCategory(userId, "Testcategory");
            _dbContext.ToDoCategories.Add(toDoCategory1);
            await _dbContext.SaveChangesAsync();

            var categoryAddOrUpdateDto = new CategoryAddOrUpdateDto { ToDoCategoryId = toDoCategory1.ToDoCategoryId, ToDoCategoryName = "TestCategory", UserId = userId };

            var content = new StringContent(JsonConvert.SerializeObject(categoryAddOrUpdateDto), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync("/api/todocategories", content);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion

        #region DeleteToDoCategory tests

        [Fact]
        public async Task DeleteToDoCategory_ShouldSuccessfullyDeleteCategory()
        {
            var userId = Guid.Parse("80a87a51-d544-4653-ae91-c6395e5fd8ce");

            var otherToDoCategory = new ToDoCategory(userId, "Other");
            _dbContext.ToDoCategories.Add(otherToDoCategory);
            await _dbContext.SaveChangesAsync();

            var toDoCategory = new ToDoCategory(userId, "Testcategory");
            var toDoCategoryId = toDoCategory.ToDoCategoryId;

            _dbContext.ToDoCategories.Add(toDoCategory);
            await _dbContext.SaveChangesAsync();

            var content = new StringContent(JsonConvert.SerializeObject(toDoCategory), Encoding.UTF8, "application/json");

            var response = await _client.DeleteAsync($"/api/todocategories/{toDoCategoryId}");
            response.EnsureSuccessStatusCode();

            var deletedCategory = await _dbContext.ToDoCategories.FirstOrDefaultAsync(c => c.ToDoCategoryId == toDoCategory.ToDoCategoryId);
            Assert.Null(deletedCategory);
        }

        [Fact]
        public async Task DeleteToDoCategory_ShouldReturnError_WhenCategoryNotFound()
        {
            var userId = Guid.NewGuid();
            var nonExistentCategoryName = "NonExistentCategory";

            var response = await _client.DeleteAsync($"/api/todocategories/{userId}/{nonExistentCategoryName}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion
    }
}
