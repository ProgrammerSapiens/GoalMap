using Core.Models;
using Data.DBContext;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;
using API;

namespace Tests.IntegrationTests.APITests
{
    public class ToDoCategoryControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public ToDoCategoryControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _factory = factory;
        }

        #region GetToDoCategoryByCategoryName tests

        [Fact]
        public async Task GetToDoCategoryByCategoryName_ShouldReturnCategoryByName()
        {
            var userId = Guid.NewGuid();
            var toDoCategoryName = "TestCategory";

            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.ToDoCategories.Add(toDoCategory);
                await dbContext.SaveChangesAsync();
            }

            var response = await _client.GetAsync($"/api/todocategories/{toDoCategoryName}");

            response.EnsureSuccessStatusCode();

            var toDoCategoryResponse = await response.Content.ReadFromJsonAsync<ToDoCategory>();
            Assert.NotNull(toDoCategoryResponse);
            Assert.Equal(userId, toDoCategoryResponse.UserId);
            Assert.Equal(toDoCategoryName, toDoCategoryResponse.ToDoCategoryName);
        }

        [Fact]
        public async Task GetToDoCategoryByCategoryName_ShouldReturnOtherCategory_WhenCategoryDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var toDoCategoryName = "TestCategory";

            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            var response = await _client.GetAsync($"api/todocategories/{toDoCategoryName}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetToDoCategoryByCategoryName_ShouldReturnUnauthorized_WhenTokenIsMissing()
        {
            var toDoCategoryName = "TestCategory";

            var response = await _client.GetAsync($"api/todocategories/{toDoCategoryName}");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        #endregion

        #region GetToDoCategoriesByUserId tests

        [Fact]
        public async Task GetToDoCategoriesByUserId_ShouldReturnCategoriesForCurrentUser()
        {
            var userId = Guid.NewGuid();

            var categories = new List<ToDoCategory>
            {
                new ToDoCategory(userId, "Category1"),
                new ToDoCategory(userId, "Caetgory2")
            };

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.ToDoCategories.AddRange(categories);
                await dbContext.SaveChangesAsync();
            }

            var response = await _client.GetAsync($"/api/todocategories/{userId}");
            response.EnsureSuccessStatusCode();

            var toDoCategoryResponse = await response.Content.ReadFromJsonAsync<List<ToDoCategory>>();
            Assert.NotNull(toDoCategoryResponse);
            Assert.Equal(2, toDoCategoryResponse.Count);
            Assert.All(toDoCategoryResponse, category => Assert.Equal(userId, category.UserId));
        }

        [Fact]
        public async Task GetToDoCategoriesByUserId_ShouldReturnEmptyList_WhenNoCategoriesExist()
        {
            var userId = Guid.NewGuid();

            var response = await _client.GetAsync($"/api/todocategories/{userId}");
            response.EnsureSuccessStatusCode();

            var toDoCategoryResponse = await response.Content.ReadFromJsonAsync<List<ToDoCategory>>();
            Assert.NotNull(toDoCategoryResponse);
            Assert.Empty(toDoCategoryResponse);
        }

        [Fact]
        public async Task GetToDoCategoriesByUserId_ShouldReturnUnauthorized_WhenTokenIsMissing()
        {
            var userId = Guid.NewGuid();

            var response = await _client.GetAsync($"api/todocategories/{userId}");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        #endregion

        #region AddToDoCategory tests

        [Fact]
        public async Task AddToDoCategory_ShouldSuccessfullyCreateCategoryForCurrentUser()
        {
            var userId = Guid.NewGuid();
            var toDoCategory = new ToDoCategory(userId, "TestCategory");

            var content = new StringContent(JsonConvert.SerializeObject(toDoCategory), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/todocategories", content);
            response.EnsureSuccessStatusCode();

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var categoryInDb = await dbContext.ToDoCategories.FirstOrDefaultAsync(c => c.ToDoCategoryName == "TestCategory" && c.UserId == userId);

                Assert.NotNull(categoryInDb);
                Assert.Equal("TestCategory", categoryInDb.ToDoCategoryName);
                Assert.Equal(userId, categoryInDb.UserId);
            }
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

        [Fact]
        public async Task AddToDoCategory_ShouldReturnError_WhenUnauthorized()
        {
            var userId = Guid.NewGuid();
            var toDoCategory = new ToDoCategory(userId, "TestCategory");

            var content = new StringContent(JsonConvert.SerializeObject(toDoCategory), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/todocategories", content);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        #endregion

        #region UpdateToDoCategory tests

        [Fact]
        public async Task UpdateToDoCategory_ShouldSuccessfullyUpdateCategoryName()
        {
            var userId = Guid.NewGuid();
            var toDoCategory = new ToDoCategory(userId, "TestCategory");
            var toDoCategoryId = toDoCategory.ToDoCategoryId;

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.ToDoCategories.Add(toDoCategory);
                await dbContext.SaveChangesAsync();
            }

            var newToDoCategoryName = "NewName";

            toDoCategory.ToDoCategoryName = newToDoCategoryName;

            var content = new StringContent(JsonConvert.SerializeObject(toDoCategory), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync("/api/todocategories", content);
            response.EnsureSuccessStatusCode();

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var toDoCategoryInDb = await dbContext.ToDoCategories.FirstOrDefaultAsync(c => c.ToDoCategoryId == toDoCategoryId);

                Assert.NotNull(toDoCategoryInDb);
                Assert.Equal(newToDoCategoryName, toDoCategoryInDb.ToDoCategoryName);
            }
        }

        [Fact]
        public async Task UpdateToDoCategory_ShouldReturnError_WhenCategoryNameAlreadyExists()
        {
            var userId = Guid.NewGuid();

            var toDoCategory1 = new ToDoCategory(userId, "TestCategory");

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.ToDoCategories.Add(toDoCategory1);
                await dbContext.SaveChangesAsync();
            }

            var toDoCategory2 = new ToDoCategory(userId, "TestCategory");

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.ToDoCategories.Add(toDoCategory2);
                await dbContext.SaveChangesAsync();
            }

            toDoCategory1.ToDoCategoryName = "TestCategory";

            var content = new StringContent(JsonConvert.SerializeObject(toDoCategory1), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync("/api/todocategories", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion

        #region DeleteToDoCategory tests

        [Fact]
        public async Task DeleteToDoCategory_ShouldSuccessfullyDeleteCategory()
        {
            var userId = Guid.NewGuid();
            var toDoCategory = new ToDoCategory(userId, "TestCategory");
            var toDoCategoryId = toDoCategory.ToDoCategoryId;

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.ToDoCategories.Add(toDoCategory);
                await dbContext.SaveChangesAsync();
            }

            var content = new StringContent(JsonConvert.SerializeObject(toDoCategory), Encoding.UTF8, "application/json");

            var response = await _client.DeleteAsync($"/api/todocategory/{userId}/{toDoCategory.ToDoCategoryName}");

            response.EnsureSuccessStatusCode();

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var deletedCategory = await dbContext.ToDoCategories
                                                     .FirstOrDefaultAsync(c => c.ToDoCategoryId == toDoCategory.ToDoCategoryId);

                Assert.Null(deletedCategory);
            }
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
