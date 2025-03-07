using Microsoft.Extensions.DependencyInjection;
using Data.DBContext;
using System.Net;
using API;
using Core.DTOs.User;
using Core.Models;
using System.Net.Http.Json;
using Xunit.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Tests.IntegrationTests.APITests
{
    public class UsersControllerTests : IAsyncLifetime
    {
        private HttpClient _client;
        private CustomWebApplicationFactory<Program> _factory;
        private AppDbContext _dbContext;
        private IServiceScope _scope;
        private readonly ITestOutputHelper _outputHelper;

        public UsersControllerTests(ITestOutputHelper outputHelper)
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

            _dbContext.Users.RemoveRange(_dbContext.Users);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            await _dbContext.DisposeAsync();
            _scope.Dispose();
            _client.Dispose();
            await _factory.DisposeAsync();
        }

        #region GetCurrentUser() tests

        [Fact]
        public async Task GetCurrentUser_ShouldReturnUser_WhenAuthenticated()
        {
            var userId1 = Guid.NewGuid();
            var user1 = new User("UserOne", userId1);

            var userId2 = Guid.NewGuid();
            var user2 = new User("UserTwo", userId2);

            _dbContext.Users.AddRange(user1, user2);
            await _dbContext.SaveChangesAsync();

            var request1 = new HttpRequestMessage(HttpMethod.Get, "/api/users/me");
            request1.Headers.Add("X-Test-UserId", userId1.ToString());

            var response1 = await _client.SendAsync(request1);
            response1.EnsureSuccessStatusCode();
            var content1 = await response1.Content.ReadFromJsonAsync<UserDto>();

            Assert.NotNull(content1);
            Assert.Equal("UserOne", content1.UserName);

            var request2 = new HttpRequestMessage(HttpMethod.Get, "/api/users/me");
            request2.Headers.Add("X-Test-UserId", userId2.ToString());

            var response2 = await _client.SendAsync(request2);
            response2.EnsureSuccessStatusCode();
            var content2 = await response2.Content.ReadFromJsonAsync<UserDto>();

            Assert.NotNull(content2);
            Assert.Equal("UserTwo", content2.UserName);
        }

        [Fact]
        public async Task GetCurrentUser_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            var nonExistentUserId = Guid.NewGuid();

            _outputHelper.WriteLine($"Testing with non-existent user ID: {nonExistentUserId}");

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/users/me");
            request.Headers.Add("X-Test-UserId", nonExistentUserId.ToString());

            var response = await _client.SendAsync(request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            _outputHelper.WriteLine($"Response body: {responseBody}");

            Assert.Contains("User was not found", responseBody);
        }

        #endregion

        #region UpdateUserProfile([FromBody] UserUpdateDto? updateUserDto) tests

        [Fact]
        public async Task UpdateUserProfile_ShouldReturnNoContent_WhenSuccessfullyUpdated()
        {
            var user = new User("OldName");
            var userId = user.UserId;

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            _outputHelper.WriteLine($"Testing user update for existing user ID: {userId}");

            var request = new HttpRequestMessage(HttpMethod.Put, "/api/users/profile")
            {
                Content = JsonContent.Create(new UserUpdateDto { UserName = "Updated Name" })
            };

            request.Headers.Add("X-Test-UserId", userId.ToString());

            var response = await _client.SendAsync(request);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            _dbContext.ChangeTracker.Clear();
            var updatedUser = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId);

            Assert.NotNull(updatedUser);
            Assert.Equal("Updated Name", updatedUser.UserName);

            _outputHelper.WriteLine("User profile updated successfully.");
        }

        [Fact]
        public async Task UpdateUserProfile_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            var nonExistentUserId = Guid.NewGuid();

            _outputHelper.WriteLine($"Testing with non-existent user ID: {nonExistentUserId}");

            var request = new HttpRequestMessage(HttpMethod.Put, "/api/users/profile")
            {
                Content = JsonContent.Create(new UserUpdateDto { UserName = "Updated Name" })
            };

            request.Headers.Add("X-Test-UserId", nonExistentUserId.ToString());

            var response = await _client.SendAsync(request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            _outputHelper.WriteLine($"Response body: {responseBody}");

            Assert.Contains("User was not found", responseBody);
        }

        [Fact]
        public async Task UpdateUserProfile_ShouldReturnBadRequest_WhenDataIsInvalid()
        {
            _outputHelper.WriteLine("Testing UpdateUserProfile with null data.");

            var request = new HttpRequestMessage(HttpMethod.Put, "/api/users/profile")
            {
                Content = null
            };

            var response = await _client.SendAsync(request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            _outputHelper.WriteLine($"Response body: {responseBody}");

            Assert.Contains("User data cannot be null", responseBody);
        }

        #endregion
    }
}
