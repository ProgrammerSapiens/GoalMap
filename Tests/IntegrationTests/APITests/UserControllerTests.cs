namespace Tests.IntegrationTests.APITests
{
    public class UserControllerTests
    {
        private readonly HttpClient _httpClient;

        public UserControllerTests(CustomWebApplicationFactory<TestProgram> factory)
        {
            _httpClient = factory.CreateClient();
        }

        #region GetUser tests

        [Fact]
        public async Task GetUser_ShouldReturnUserInfo_WhenUserExists()
        {

        }

        [Fact]
        public async Task GetUser_ShouldReturnError_WhenUserNotFound()
        {

        }

        [Fact]
        public async Task GetUser_ShouldReturnUnauthorized_WhenTokenIsMissing()
        {

        }

        #endregion

        #region RegisterUser tests

        [Fact]
        public async Task RegisterUser_ShouldSuccessfullyRegisterNewUser()
        {

        }

        [Fact]
        public async Task RegisterUser_ShouldReturnError_WhenUserAlreadyExists()
        {

        }

        [Fact]
        public async Task RegisterUser_ShouldReturnError_WhenPasswordIsMissing()
        {

        }

        [Fact]
        public async Task RegisterUser_ShouldReturnError_WhenInvalidDataIsProvided()
        {

        }

        #endregion

        #region AuthenticateUser tests

        [Fact]
        public async Task AuthenticateUser_ShouldReturnToken_WhenCredentialsAreValid()
        {

        }

        [Fact]
        public async Task AuthenticateUser_ShouldReturnError_WhenCredentialsAreInvalid()
        {

        }

        #endregion

        #region UpdateUser tests

        [Fact]
        public async Task UpdateUser_ShouldSuccessfullyUpdateUserData()
        {

        }

        [Fact]
        public async Task UpdateUser_ShouldReturnError_WhenTryingToUpdateAnotherUser()
        {

        }

        [Fact]
        public async Task UpdateUser_ShouldReturnError_WhenInvalidDataIsProvided()
        {

        }

        #endregion
    }
}
