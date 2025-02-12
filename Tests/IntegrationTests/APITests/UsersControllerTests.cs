using Microsoft.Extensions.DependencyInjection;
using Data.DBContext;
using System.Net;
using API;
using Core.DTOs.User;
using Core.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace Tests.IntegrationTests.APITests
{
    public class UsersControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly IServiceScopeFactory _scopeFactory;

        public UsersControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
        }

        #region GetCurrentUser() tests

        [Fact]
        public async Task GetCurrentUser_ShouldReturnUser_WhenAuthenticated()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var user = new User("TestUser");
                context.Users.Add(user);
                await context.SaveChangesAsync();
            }

            var response = await _client.GetAsync("/api/users/me");

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Test User", content);
        }

        [Fact]
        public async Task GetCurrentUser_ShouldReturnUnauthorized_WhenNoUserId()
        {
            var response = await _client.GetAsync("/api/users/me");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        #endregion

        #region UpdateUserProfile([FromBody] UserUpdateDto? updateUserDto) tests

        [Fact]
        public async Task UpdateUserProfile_ShouldReturnUnauthorized_WhenNoUserId()
        {
            var updateUserDto = new UserUpdateDto { UserName = "Updated Name" };

            var response = await _client.PutAsJsonAsync("/api/users/profile", updateUserDto);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task UpdateUserProfile_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            var updateUserDto = new UserUpdateDto { UserName = "Updated Name" };

            var response = await _client.PutAsJsonAsync("/api/users/profile", updateUserDto);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateUserProfile_ShouldReturnNoContent_WhenSuccessfullyUpdated()
        {
            var user = new User("OldName");
            var userId = user.UserId;

            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                context.Users.Add(user);
                await context.SaveChangesAsync();
            }

            var updateUserDto = new UserUpdateDto { UserName = "Updated Name" };

            var response = await _client.PutAsJsonAsync("/api/users/profile", updateUserDto);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var updatedUser = await context.Users.FindAsync(userId);
                Assert.Equal("Updated Name", updatedUser.UserName);
            }
        }

        //[Fact]
        //public async Task UpdateUserProfile_ShouldReturnBadRequest_WhenDataIsInvalid()
        //{
        //    var response = await _client.PutAsJsonAsync("/api/users/profile", null);

        //    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        //}

        #endregion
    }
}
