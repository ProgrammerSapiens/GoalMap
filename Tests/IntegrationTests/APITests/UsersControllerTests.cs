//////////////////////////using Microsoft.Extensions.DependencyInjection;
//////////////////////////using Data.DBContext;
//////////////////////////using System.Net;
//////////////////////////using API;
//////////////////////////using Core.DTOs.User;
//////////////////////////using Core.Models;
//////////////////////////using System.Net.Http.Json;
//////////////////////////using Microsoft.Extensions.Logging;
//////////////////////////using Xunit.Abstractions;

//////////////////////////namespace Tests.IntegrationTests.APITests
//////////////////////////{
//////////////////////////    [Collection("NoParallelTests")]
//////////////////////////    public class UsersControllerTests /*: /*IAsyncLifetime*/*/
//////////////////////////    {
//////////////////////////        //private HttpClient _client;
//////////////////////////        //private CustomWebApplicationFactory<Program> _factory;
//////////////////////////        //private AppDbContext _dbContext;
//////////////////////////        //private IServiceScope _scope;
//////////////////////////        //private readonly ITestOutputHelper _outputHelper;

//////////////////////////        //public UsersControllerTests(ITestOutputHelper outputHelper)
//////////////////////////        //{
//////////////////////////        //    _outputHelper = outputHelper;
//////////////////////////        //}

//////////////////////////        //public async Task InitializeAsync()
//////////////////////////        //{
//////////////////////////        //    _factory = new CustomWebApplicationFactory<Program>();
//////////////////////////        //    _factory.SetOutputHelper(_outputHelper);
//////////////////////////        //    _client = _factory.CreateClient();

//////////////////////////        //    _scope = _factory.Services.CreateScope();
//////////////////////////        //    _dbContext = _scope.ServiceProvider.GetRequiredService<AppDbContext>();

//////////////////////////        //    _dbContext.Users.RemoveRange(_dbContext.Users);
//////////////////////////        //    await _dbContext.SaveChangesAsync();
//////////////////////////        //}

//////////////////////////        //public async Task DisposeAsync()
//////////////////////////        //{
//////////////////////////        //    await _dbContext.DisposeAsync();
//////////////////////////        //    _scope.Dispose();
//////////////////////////        //    _client.Dispose();
//////////////////////////        //    await _factory.DisposeAsync();
//////////////////////////        //}

//////////////////////////        #region GetCurrentUser() tests

//////////////////////////        //[Fact]
//////////////////////////        //public async Task GetCurrentUser_ShouldReturnUser_WhenAuthenticated()
//////////////////////////        //{
//////////////////////////        //    //var userId = Guid.NewGuid();
//////////////////////////        //    //var user = new User("Test User", userId);
//////////////////////////        //    //_dbContext.Users.Add(user);
//////////////////////////        //    //await _dbContext.SaveChangesAsync();

//////////////////////////        //    var response = await _client.GetAsync("/api/users/me");

//////////////////////////        //    response.EnsureSuccessStatusCode();
//////////////////////////        //    //var content = await response.Content.ReadFromJsonAsync<UserDto>;
//////////////////////////        //    //Assert.Contains("Test", content);
//////////////////////////        //}

//////////////////////////        //[Fact]
//////////////////////////        //public async Task GetCurrentUser_ShouldReturnUnauthorized_WhenNoUserId()
//////////////////////////        //{
//////////////////////////        //    var response = await _client.GetAsync("/api/users/me");

//////////////////////////        //    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//////////////////////////        //}

//////////////////////////        //#endregion

//////////////////////////        //#region UpdateUserProfile([FromBody] UserUpdateDto? updateUserDto) tests

//////////////////////////        //[Fact]
//////////////////////////        //public async Task UpdateUserProfile_ShouldReturnUnauthorized_WhenNoUserId()
//////////////////////////        //{
//////////////////////////        //    var updateUserDto = new UserUpdateDto { UserName = "Updated Name" };

//////////////////////////        //    var response = await _client.PutAsJsonAsync("/api/users/profile", updateUserDto);

//////////////////////////        //    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//////////////////////////        //}

//////////////////////////        //[Fact]
//////////////////////////        //public async Task UpdateUserProfile_ShouldReturnNotFound_WhenUserDoesNotExist()
//////////////////////////        //{
//////////////////////////        //    var updateUserDto = new UserUpdateDto { UserName = "Updated Name" };

//////////////////////////        //    var response = await _client.PutAsJsonAsync("/api/users/profile", updateUserDto);

//////////////////////////        //    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
//////////////////////////        //}

//////////////////////////        //[Fact]
//////////////////////////        //public async Task UpdateUserProfile_ShouldReturnNoContent_WhenSuccessfullyUpdated()
//////////////////////////        //{
//////////////////////////        //    var user = new User("OldName");
//////////////////////////        //    var userId = user.UserId;

//////////////////////////        //    _dbContext.Users.Add(user);
//////////////////////////        //    await _dbContext.SaveChangesAsync();

//////////////////////////        //    var updateUserDto = new UserUpdateDto { UserName = "Updated Name" };

//////////////////////////        //    var response = await _client.PutAsJsonAsync("/api/users/profile", updateUserDto);

//////////////////////////        //    Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

//////////////////////////        //    var updatedUser = await _dbContext.Users.FindAsync(userId);
//////////////////////////        //    Assert.Equal("Updated Name", updatedUser.UserName);
//////////////////////////        //}

//////////////////////////        //[Fact]
//////////////////////////        //public async Task UpdateUserProfile_ShouldReturnBadRequest_WhenDataIsInvalid()
//////////////////////////        //{
//////////////////////////        //    // var response = await _client.PutAsJsonAsync("/api/users/profile", null);

//////////////////////////        //    //Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
//////////////////////////        //}

//////////////////////////        //#endregion
//////////////////////////    }
//////////////////////////}
