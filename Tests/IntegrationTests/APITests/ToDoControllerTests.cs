using Core.Models;
using Data.DBContext;
using Microsoft.Extensions.DependencyInjection;
using API;
using Xunit.Abstractions;
using Newtonsoft.Json;
using System.Net;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using Core.DTOs.ToDo;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Tests.IntegrationTests.APITests
{
    public class ToDoControllerTests : IAsyncLifetime
    {
        private HttpClient _client;
        private CustomWebApplicationFactory<Program> _factory;
        private AppDbContext _dbContext;
        private IServiceScope _scope;
        private readonly ITestOutputHelper _outputHelper;

        public ToDoControllerTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        public async Task InitializeAsync()
        {
            var dbName = Guid.NewGuid().ToString();

            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.Test.json").Build();

            _factory = new CustomWebApplicationFactory<Program>(dbName, _outputHelper, configuration);
            _client = _factory.CreateClient();

            _scope = _factory.Services.CreateScope();
            _dbContext = _scope.ServiceProvider.GetRequiredService<AppDbContext>();

            _dbContext.ToDos.RemoveRange(_dbContext.ToDos);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            await _dbContext.DisposeAsync();
            _scope.Dispose();
            _client.Dispose();
            await _factory.DisposeAsync();
        }

        #region GetToDoById tests

        [Fact]
        public async Task GetToDoById_ShouldReturnToDoById()
        {
            var description = "TestToDo";
            var timeBlock = TimeBlock.Day;
            var difficulty = Difficulty.Easy;
            var toDoDate = DateTime.Today;
            var toDoCategoryId = Guid.NewGuid();
            var userId = Guid.Parse("80a87a51-d544-4653-ae91-c6395e5fd8ce");

            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId);

            await _dbContext.ToDos.AddAsync(toDo);
            await _dbContext.SaveChangesAsync();

            var savedToDo = await _dbContext.ToDos.FindAsync(toDo.ToDoId);

            var response = await _client.GetAsync($"/api/todos/{toDo.ToDoId}");
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var toDoResponse = JsonConvert.DeserializeObject<ToDoDto>(jsonString);
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
            var toDoCategoryId = Guid.NewGuid();
            var userId = Guid.Parse("80a87a51-d544-4653-ae91-c6395e5fd8ce");

            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId);

            var response = await _client.GetAsync($"/api/todos/{toDo.ToDoId}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var toDoInDb = await _dbContext.ToDos.FirstOrDefaultAsync(t => t.ToDoId == toDo.ToDoId);
            Assert.Null(toDoInDb);
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
            var toDoCategoryId = Guid.NewGuid();
            var userId = Guid.Parse("80a87a51-d544-4653-ae91-c6395e5fd8ce");

            _outputHelper.WriteLine($"User id is {userId}");

            var toDos = new List<ToDo>()
            {
                new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId),
                new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId)
            };

            await _dbContext.ToDos.AddRangeAsync(toDos);
            await _dbContext.SaveChangesAsync();

            var toDoGetByDateAndTimeBlockDto = new ToDoGetByDateAndTimeBlockDto
            {
                Date = toDoDate,
                TimeBlock = timeBlock
            };

            var response = await _client.GetAsync($"/api/todos?date={toDoDate:yyyy-MM-dd}&timeBlock={timeBlock}");
            response.EnsureSuccessStatusCode();

            var toDoResponse = await response.Content.ReadFromJsonAsync<List<ToDoDto>>();

            Assert.NotNull(toDoResponse);
            Assert.Equal(2, toDoResponse.Count);
            Assert.All(toDoResponse, t => Assert.Equal(description, t.Description));
        }

        [Fact]
        public async Task GetToDos_ShouldReturnEmptyToDo_WhenNoToDosExist()
        {
            var timeBlock = TimeBlock.Day;
            var toDoDate = DateTime.Today;

            var response = await _client.GetAsync($"/api/todos?date={toDoDate:yyyy-MM-dd}&timeBlock={timeBlock}");
            response.EnsureSuccessStatusCode();

            var toDoResponse = await response.Content.ReadFromJsonAsync<List<ToDoDto>>();
            Assert.NotNull(toDoResponse);
            Assert.Empty(toDoResponse);
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
            var toDoCategoryId = Guid.NewGuid();
            var userId = Guid.Parse("80a87a51-d544-4653-ae91-c6395e5fd8ce");

            var toDoAddDto = new ToDoAddDto
            {
                Description = description,
                TimeBlock = timeBlock,
                Difficulty = difficulty,
                ToDoDate = toDoDate,
                ToDoCategoryId = toDoCategoryId,
                UserId = userId
            };

            var content = new StringContent(JsonConvert.SerializeObject(toDoAddDto), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"/api/todos", content);
            response.EnsureSuccessStatusCode();

            var createdToDo = await response.Content.ReadFromJsonAsync<ToDoDto>();

            Assert.NotNull(createdToDo);
            Assert.NotEqual(Guid.Empty, createdToDo.ToDoId);
            Assert.Equal(difficulty, createdToDo.Difficulty);
            Assert.Equal(toDoDate, createdToDo.ToDoDate);
            Assert.Equal(toDoCategoryId, createdToDo.ToDoCategoryId);
            Assert.Equal(userId, createdToDo.UserId);
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
            var toDoCategoryId = Guid.NewGuid();
            var userId = Guid.Parse("80a87a51-d544-4653-ae91-c6395e5fd8ce");

            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId);

            await _dbContext.ToDos.AddAsync(toDo);
            await _dbContext.SaveChangesAsync();

            var newDescription = "NewDescription";
            toDo.Description = newDescription;

            var content = new StringContent(JsonConvert.SerializeObject(toDo), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"/api/todos", content);
            response.EnsureSuccessStatusCode();

            var toDoInDb = await _dbContext.ToDos.FirstOrDefaultAsync(t => t.ToDoId == toDo.ToDoId);

            Assert.NotNull(toDoInDb);
            Assert.Equal(newDescription, toDoInDb.Description);
            Assert.Equal(timeBlock, toDoInDb.TimeBlock);
            Assert.Equal(difficulty, toDoInDb.Difficulty);
            Assert.Equal(toDoDate, toDoInDb.ToDoDate);
            Assert.Equal(toDoCategoryId, toDoInDb.ToDoCategoryId);
            Assert.Equal(userId, toDoInDb.UserId);
        }

        [Fact]
        public async Task UpdateToDo_ShouldReturnError_WnehToDoNotFound()
        {
            var description = "TestToDo";
            var timeBlock = TimeBlock.Day;
            var difficulty = Difficulty.Easy;
            var toDoDate = DateTime.Today;
            var toDoCategoryId = Guid.NewGuid();
            var userId = Guid.Parse("80a87a51-d544-4653-ae91-c6395e5fd8ce");

            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId);

            var content = new StringContent(JsonConvert.SerializeObject(toDo), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"/api/todos", content);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var toDoInDb = await _dbContext.ToDos.FirstOrDefaultAsync(t => t.ToDoId == toDo.ToDoId);
            Assert.Null(toDoInDb);
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
            var toDoCategoryId = Guid.NewGuid();
            var userId = Guid.Parse("80a87a51-d544-4653-ae91-c6395e5fd8ce");

            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId);

            await _dbContext.ToDos.AddAsync(toDo);
            await _dbContext.SaveChangesAsync();

            var response = await _client.DeleteAsync($"/api/todos/{toDo.ToDoId}");
            response.EnsureSuccessStatusCode();

            var toDoInDb = await _dbContext.ToDos.FirstOrDefaultAsync(t => t.ToDoId == toDo.ToDoId);
            Assert.Null(toDoInDb);
        }

        [Fact]
        public async Task DeleteToDo_ShouldReturnError_WhenToDoNotFound()
        {
            var description = "TestToDo";
            var timeBlock = TimeBlock.Day;
            var difficulty = Difficulty.Easy;
            var toDoDate = DateTime.Today;
            var toDoCategoryId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId);

            var response = await _client.DeleteAsync($"/api/todos/{toDo.ToDoId}");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var toDoInDb = await _dbContext.ToDos.FirstOrDefaultAsync(t => t.ToDoId == toDo.ToDoId);
            Assert.Null(toDoInDb);
        }

        #endregion
    }
}
