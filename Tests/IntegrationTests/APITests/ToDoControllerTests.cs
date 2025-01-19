using Core.Models;
using Data.DBContext;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Tests.IntegrationTests.APITests
{
    public class ToDoControllerTests : IClassFixture<CustomWebApplicationFactory<TestProgram>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<TestProgram> _factory;

        public ToDoControllerTests(CustomWebApplicationFactory<TestProgram> factory)
        {
            _client = factory.CreateClient();
            _factory = factory;
        }

        #region GetToDoById tests

        [Fact]
        public async Task GetToDoById_ShouldReturnToDoById()
        {
            var description = "TestToDo";
            var timeBlock = TimeBlock.Day;
            var difficulty = Difficulty.Easy;
            var toDoDate = DateTime.Today;
            var toDoCategoryName = "SomeCategory";
            var userId = Guid.NewGuid();

            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryName, userId);

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.ToDos.Add(toDo);
                await dbContext.SaveChangesAsync();
            }

            var response = await _client.GetAsync($"/api/todos/{toDo.ToDoId}");
            response.EnsureSuccessStatusCode();

            var toDoResponse = await response.Content.ReadFromJsonAsync<ToDo>();
            Assert.NotNull(toDoResponse);
            Assert.Equal(toDo.ToDoId, toDoResponse.ToDoId);
        }

        [Fact]
        public async Task GetToDoById_ShouldReturnError_WhenToDoDoesNotExist()
        {
            var description = "TestToDo";
            var timeBlock = TimeBlock.Day;
            var difficulty = Difficulty.Easy;
            var toDoDate = DateTime.Today;
            var toDoCategoryName = "SomeCategory";
            var userId = Guid.NewGuid();

            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryName, userId);

            var response = await _client.GetAsync($"/api/todos/{toDo.ToDoId}");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var toDoInDb = await dbContext.ToDos.FirstOrDefaultAsync(t => t.ToDoId == toDo.ToDoId);

                Assert.Null(toDoInDb);
            }
        }

        #endregion

        #region GetToDos tests

        [Fact]
        public async Task GetToDos_ShouldReturnToDosForCurrentUser()
        {
            var description = "TestToDo";
            var timeBlock = TimeBlock.Day;
            var difficulty = Difficulty.Easy;
            var toDoDate = DateTime.Today;
            var toDoCategoryName = "SomeCategory";
            var userId = Guid.NewGuid();

            var toDos = new List<ToDo>()
            {
                new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryName, userId),
                new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryName, userId)
            };

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.ToDos.AddRange(toDos);
                await dbContext.SaveChangesAsync();
            }

            var response = await _client.GetAsync($"/api/todos/{userId}/{toDoDate}/{timeBlock}");
            response.EnsureSuccessStatusCode();

            var toDoResponse = await response.Content.ReadFromJsonAsync<List<ToDo>>();
            Assert.NotNull(toDoResponse);
            Assert.Equal(2, toDoResponse.Count);
            Assert.All(toDoResponse, t => Assert.Equal(description, t.Description));
        }

        [Fact]
        public async Task GetToDos_ShouldReturnEmptyToDo_WhenNoToDosExist()
        {
            var timeBlock = TimeBlock.Day;
            var toDoDate = DateTime.Today;
            var userId = Guid.NewGuid();

            var response = await _client.GetAsync($"/api/todos/{userId}/{toDoDate}/{timeBlock}");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion

        #region AddToDo tests

        [Fact]
        public async Task AddToDo_ShouldSuccessfullyCreateToDo()
        {
            var description = "TestToDo";
            var timeBlock = TimeBlock.Day;
            var difficulty = Difficulty.Easy;
            var toDoDate = DateTime.Today;
            var toDoCategoryName = "SomeCategory";
            var userId = Guid.NewGuid();

            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryName, userId);

            var content = new StringContent(JsonConvert.SerializeObject(toDo), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"/api/todos", content);
            response.EnsureSuccessStatusCode();

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var toDoInDb = await dbContext.ToDos.FirstOrDefaultAsync(t => t.ToDoId == toDo.ToDoId);

                Assert.NotNull(toDoInDb);
                Assert.Equal(description, toDoInDb.Description);
                Assert.Equal(timeBlock, toDoInDb.TimeBlock);
                Assert.Equal(difficulty, toDoInDb.Difficulty);
                Assert.Equal(toDoDate, toDoInDb.ToDoDate);
                Assert.Equal(toDoCategoryName, toDoInDb.ToDoCategoryName);
                Assert.Equal(userId, toDoInDb.UserId);
            }
        }

        #endregion

        #region UpdateToDo tests

        [Fact]
        public async Task UpdateToDo_ShouldSuccessfullyUpdateToDo()
        {
            var description = "TestToDo";
            var timeBlock = TimeBlock.Day;
            var difficulty = Difficulty.Easy;
            var toDoDate = DateTime.Today;
            var toDoCategoryName = "SomeCategory";
            var userId = Guid.NewGuid();

            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryName, userId);

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.ToDos.Add(toDo);
                await dbContext.SaveChangesAsync();
            }

            var newDescription = "NewDescription";
            toDo.Description = newDescription;

            var content = new StringContent(JsonConvert.SerializeObject(toDo), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"/api/todos", content);
            response.EnsureSuccessStatusCode();

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var toDoInDb = await dbContext.ToDos.FirstOrDefaultAsync(t => t.ToDoId == toDo.ToDoId);

                Assert.NotNull(toDoInDb);
                Assert.Equal(description, toDoInDb.Description);
                Assert.Equal(timeBlock, toDoInDb.TimeBlock);
                Assert.Equal(difficulty, toDoInDb.Difficulty);
                Assert.Equal(toDoDate, toDoInDb.ToDoDate);
                Assert.Equal(toDoCategoryName, toDoInDb.ToDoCategoryName);
                Assert.Equal(userId, toDoInDb.UserId);
            }
        }

        [Fact]
        public async Task UpdateToDo_ShouldReturnError_WnehToDoNotFound()
        {
            var description = "TestToDo";
            var timeBlock = TimeBlock.Day;
            var difficulty = Difficulty.Easy;
            var toDoDate = DateTime.Today;
            var toDoCategoryName = "SomeCategory";
            var userId = Guid.NewGuid();

            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryName, userId);

            var content = new StringContent(JsonConvert.SerializeObject(toDo), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"/api/todos", content);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var toDoInDb = await dbContext.ToDos.FirstOrDefaultAsync(t => t.ToDoId == toDo.ToDoId);

                Assert.Null(toDoInDb);
            }
        }

        #endregion

        #region DeleteToDo tests

        [Fact]
        public async Task DeleteToDo_ShouldSuccessfullyDeleteToDo()
        {
            var description = "TestToDo";
            var timeBlock = TimeBlock.Day;
            var difficulty = Difficulty.Easy;
            var toDoDate = DateTime.Today;
            var toDoCategoryName = "SomeCategory";
            var userId = Guid.NewGuid();

            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryName, userId);

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.ToDos.Add(toDo);
                await dbContext.SaveChangesAsync();
            }

            var response = await _client.DeleteAsync($"/api/todos/{toDo.ToDoId}");
            response.EnsureSuccessStatusCode();

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var toDoInDb = await dbContext.ToDos.FirstOrDefaultAsync(t => t.ToDoId == toDo.ToDoId);

                Assert.Null(toDoInDb);
            }
        }

        [Fact]
        public async Task DeleteToDo_ShouldReturnError_WhenToDoNotFound()
        {
            var description = "TestToDo";
            var timeBlock = TimeBlock.Day;
            var difficulty = Difficulty.Easy;
            var toDoDate = DateTime.Today;
            var toDoCategoryName = "SomeCategory";
            var userId = Guid.NewGuid();

            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryName, userId);

            var response = await _client.DeleteAsync($"/api/todos/{toDo.ToDoId}");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var toDoInDb = await dbContext.ToDos.FirstOrDefaultAsync(t => t.ToDoId == toDo.ToDoId);

                Assert.Null(toDoInDb);
            }
        }

        #endregion
    }
}
