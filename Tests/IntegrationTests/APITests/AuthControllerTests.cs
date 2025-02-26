using Core.DTOs.User;
using System.Net.Http.Json;
using System.Net;
using Data.DBContext;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using API;
using Core.Models;

namespace Tests.IntegrationTests.APITests
{
    public class AuthControllerTests : IAsyncLifetime
    {
        private HttpClient _client;
        private CustomWebApplicationFactory<Program> _factory;
        private AppDbContext _dbContext;
        private IServiceScope _scope;
        private readonly ITestOutputHelper _outputHelper;

        public AuthControllerTests(ITestOutputHelper outputHelper)
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

        #region Register([FromBody] UserRegAndAuthDto registerUserDto) tests

        [Fact]
        public async Task Register_ShouldReturnCreated_WhenUserIsRegistered()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/auth/register")
            {
                Content = JsonContent.Create(new UserRegAndAuthDto
                {
                    UserName = "new_user",
                    Password = "securepassword"
                })
            };

            var response = await _client.SendAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            _outputHelper.WriteLine($"Response body: {responseBody}");

            Assert.Contains("new_user", responseBody);
        }

        [Theory]
        [InlineData("", "password")]
        [InlineData("username", "")]
        [InlineData("", "")]
        public async Task Register_ShouldReturnBadRequest_WhenInvalidData(string username, string password)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/auth/register")
            {
                Content = JsonContent.Create(new UserRegAndAuthDto { UserName = username, Password = password })
            };

            var response = await _client.SendAsync(request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            _outputHelper.WriteLine($"Response body: {responseBody}");

            Assert.Contains("Username and password cannot be empty", responseBody);
        }

        #endregion

        #region Login([FromBody] UserRegAndAuthDto loginDto)

        [Fact]
        public async Task Login_ShouldReturnOk_WithToken_WhenCredentialsAreValid()
        {
            var password = "securepassword";
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            _outputHelper.WriteLine($"PasswordHash is {passwordHash}");

            var user = new User("valid_user")
            {
                PasswordHash = passwordHash
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/auth/login")
            {
                Content = JsonContent.Create(new UserRegAndAuthDto
                {
                    UserName = "valid_user",
                    Password = password
                })
            };

            var response = await _client.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            _outputHelper.WriteLine($"Response body: {responseBody}");

            Assert.Contains("token", responseBody);
        }

        [Theory]
        [InlineData("wrong_user", "securepassword")]
        [InlineData("valid_user", "wrongpassword")]
        [InlineData("wrong_user", "wrongpassword")]
        public async Task Login_ShouldReturnUnauthorized_WhenInvalidCredentials(string username, string password)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/auth/login")
            {
                Content = JsonContent.Create(new UserRegAndAuthDto { UserName = username, Password = password })
            };

            var response = await _client.SendAsync(request);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            _outputHelper.WriteLine($"Response body: {responseBody}");

            Assert.Contains("Invalid username or password", responseBody);
        }

        #endregion
    }
}
