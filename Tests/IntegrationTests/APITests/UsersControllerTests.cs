using Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Data.DBContext;
using System.Net.Http.Json;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using API;
using System.Net.Http.Headers;

namespace Tests.IntegrationTests.APITests
{
    public class UsersControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public UsersControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        private async Task ResetDatabaseAsync()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Users.RemoveRange(dbContext.Users);
                await dbContext.SaveChangesAsync();
            }
        }

        #region GetUserByUserName tests

        [Fact]
        public async Task GetUserByUserName_ShouldReturnUserInfo_WhenUserExists()
        {
            await ResetDatabaseAsync();

            var userName = "testuser";
            var password = "password123";
            var newUser = new { Username = userName, Password = password };
            var content = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json");

            await _client.PostAsync("/api/auth/register", content);

            var loginResponse = await _client.PostAsync("/api/auth/login", content);
            var responseBody = await loginResponse.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseBody);
            var token = jsonResponse["token"];

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync($"/api/users/{userName}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetUserByUserName_ShouldReturnUnauthorized_WhenTokenIsMissing()
        {
            await ResetDatabaseAsync();

            var userName = "testuser";
            var response = await _client.GetAsync($"/api/users/{userName}");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        #endregion

        #region RegisterUser tests

        [Fact]
        public async Task RegisterUser_ShouldSuccessfullyRegisterNewUser()
        {
            await ResetDatabaseAsync();

            var userName = "testuser";
            var password = "password123";
            var newUser = new { Username = userName, Password = password };
            var content = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/auth/register", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task RegisterUser_ShouldReturnError_WhenUserAlreadyExists()
        {
            await ResetDatabaseAsync();

            var userName = "existinguser";
            var password = "password123";
            var existingUser = new { Username = userName, Password = password };
            var content = new StringContent(JsonConvert.SerializeObject(existingUser), Encoding.UTF8, "application/json");

            await _client.PostAsync("/api/auth/register", content);

            var response = await _client.PostAsync("/api/auth/register", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion

        #region AuthenticateUser tests

        [Fact]
        public async Task AuthenticateUser_ShouldReturnToken_WhenCredentialsAreValid()
        {
            await ResetDatabaseAsync();

            var userName = "validuser";
            var password = "password123";
            var newUser = new { Username = userName, Password = password };
            var content = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json");

            await _client.PostAsync("/api/auth/register", content);

            var loginResponse = await _client.PostAsync("/api/auth/login", content);
            var responseBody = await loginResponse.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseBody);

            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
            Assert.True(jsonResponse.ContainsKey("token"));
            Assert.False(string.IsNullOrEmpty(jsonResponse["token"]));
        }

        [Fact]
        public async Task AuthenticateUser_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
        {
            await ResetDatabaseAsync();

            var invalidUser = new { Username = "nonexistent", Password = "wrongpassword" };
            var content = new StringContent(JsonConvert.SerializeObject(invalidUser), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/auth/login", content);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        #endregion

        #region UpdateUser tests

        [Fact]
        public async Task UpdateUser_ShouldReturnError_WhenTryingToUpdateAnotherUser()
        {
            await ResetDatabaseAsync();

            var user1 = new { Username = "user1", Password = "password123" };
            var user2 = new { Username = "user2", Password = "password456" };
            var content1 = new StringContent(JsonConvert.SerializeObject(user1), Encoding.UTF8, "application/json");
            var content2 = new StringContent(JsonConvert.SerializeObject(user2), Encoding.UTF8, "application/json");

            await _client.PostAsync("/api/auth/register", content1);
            await _client.PostAsync("/api/auth/register", content2);

            var loginResponse = await _client.PostAsync("/api/auth/login", content1);
            var responseBody = await loginResponse.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseBody);
            var token = jsonResponse["token"];

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var updateContent = new StringContent(JsonConvert.SerializeObject(new { Username = "user2", Password = "newpass" }), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/api/users/user2", updateContent);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        #endregion
    }
}
