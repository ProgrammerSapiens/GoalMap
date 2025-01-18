using Core.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Data.DBContext;
using System.Net.Http.Json;
using System.Net;
using Newtonsoft.Json;
using System.Text;

namespace Tests.IntegrationTests.APITests
{
    public class UserControllerTests : IClassFixture<CustomWebApplicationFactory<TestProgram>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<TestProgram> _factory;

        public UserControllerTests(CustomWebApplicationFactory<TestProgram> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        #region GetUserByUserName tests

        [Fact]
        public async Task GetUserByUserName_ShouldReturnUserInfo_WhenUserExists()
        {
            var user = new User("TestUser", "hashedPassword", 0);

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();
            }

            var response = await _client.GetAsync($"/api/users/{user.UserName}");

            response.EnsureSuccessStatusCode();

            var userResponce = await response.Content.ReadFromJsonAsync<User>();

            Assert.NotNull(userResponce);
            Assert.Equal(user.UserId, userResponce.UserId);
            Assert.Equal(user.UserName, userResponce.UserName);
        }

        [Fact]
        public async Task GetUserByUserName_ShouldReturnNotFound_WhenUserDoesNotFound()
        {
            string nonExistentUserName = "Some user";

            var response = await _client.GetAsync($"/api/users/{nonExistentUserName}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetUserByUserName_ShouldReturnUnauthorized_WhenTokenIsMissing()
        {
            var userName = "TestUser";

            var response = await _client.GetAsync($"/api/users/{userName}");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        #endregion

        #region RegisterUser tests

        [Fact]
        public async Task RegisterUser_ShouldSuccessfullyRegisterNewUser()
        {
            string userName = "TestUser";
            string password = "UserPassword";

            var user = new User(userName, password);

            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"/api/users", content);

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task RegisterUser_ShouldReturnError_WhenUserAlreadyExists()
        {
            string userName = "TestUser";
            string password = "UserPassword";

            var user = new User(userName, password);

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Users.Add(user);

                await dbContext.SaveChangesAsync();
            }

            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"/api/users", content);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("User already exists", responseContent);
        }

        [Fact]
        public async Task RegisterUser_ShouldReturnError_WhenPasswordIsMissing()
        {
            string userName = "TestUser";
            string password = "";

            var user = new User(userName, password);

            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"/api/users", content);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("Password is required", responseContent);
        }

        [Fact]
        public async Task RegisterUser_ShouldReturnError_WhenInvalidDataIsProvided()
        {
            string userName = "";
            string password = "password";

            var user = new User(userName, password);

            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/users", content);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("Username is required", responseContent);
        }

        #endregion

        #region AuthenticateUser tests

        [Fact]
        public async Task AuthenticateUser_ShouldReturnToken_WhenCredentialsAreValid()
        {
            string userName = "TestUser";
            string password = "Password";

            var user = new User(userName, password);

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();
            }

            var credentials = new { UserName = userName, Password = password };

            var content = new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/authenticate", content);

            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.Contains("token", responseBody);
        }

        [Fact]
        public async Task AuthenticateUser_ShouldReturnError_WhenCredentialsAreInvalid()
        {
            string userName = "InvalidUser";
            string password = "InvalidPassword";

            var credentials = new { UserName = userName, Password = password };

            var content = new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/authenticate", content);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.Contains("error", responseBody);
        }

        #endregion

        #region UpdateUser tests

        [Fact]
        public async Task UpdateUser_ShouldSuccessfullyUpdateUserData()
        {
            string userName = "ExistingUserName";
            string password = "ExistingPassword";

            var user = new User(userName, password);

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();
            }

            string newUserName = "NewUserName";
            string newPassword = "NewPassword";

            user.UserName = newUserName;
            user.PasswordHash = newPassword;

            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"/api/users/{user.UserId}", content);

            response.EnsureSuccessStatusCode();

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var updatedUser = await dbContext.Users.FindAsync(user.UserId);

                Assert.NotNull(updatedUser);
                Assert.Equal(newPassword, updatedUser.PasswordHash);
                Assert.Equal(newUserName, updatedUser.UserName);
            }
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnError_WhenTryingToUpdateAnotherUser()
        {
            string userName1 = "User1";
            string password1 = "Password1";

            var user1 = new User(userName1, password1);

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Users.Add(user1);
                await dbContext.SaveChangesAsync();
            }

            string userName2 = "User2";
            string password2 = "Password2";

            var user2 = new User(userName2, password2);

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Users.Add(user2);
                await dbContext.SaveChangesAsync();
            }

            user2.UserName = "NewUserNameForUser2";
            user2.PasswordHash = "NewPasswordForUser2";

            var content = new StringContent(JsonConvert.SerializeObject(user2), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"/api/users/{user2.UserId}", content);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnError_WhenInvalidDataIsProvided()
        {
            string userName = "ExistingUserName";
            string password = "ExistingPassword";

            var user = new User(userName, password);

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();
            }

            string invalidUserName = "";
            string invalidPassword = "";

            user.UserName = invalidUserName;
            user.PasswordHash = invalidPassword;

            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"/api/users/{user.UserId}", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion
    }
}
