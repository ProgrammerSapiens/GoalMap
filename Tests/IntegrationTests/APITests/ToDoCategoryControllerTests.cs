namespace Tests.IntegrationTests.APITests
{
    public class ToDoCategoryControllerTests : IClassFixture<CustomWebApplicationFactory<TestProgram>>
    {
        private readonly HttpClient _httpClient;

        public ToDoCategoryControllerTests(CustomWebApplicationFactory<TestProgram> factory)
        {
            _httpClient = factory.CreateClient();
        }

        #region GetToDoCategoryByCategoryName tests

        [Fact]
        public async Task GetToDoCategoryByCategoryName_ShouldReturnCategoryByName()
        {

        }

        [Fact]
        public async Task GetToDoCategoryByCategoryName_ShouldReturnEmptyCategory_WhenCategoryDoesNotExist()
        {

        }

        [Fact]
        public async Task GetToDoCategoryByCategoryName_ShouldReturnUnauthorized_WhenTokenIsMissing()
        {

        }

        #endregion

        #region GetToDoCategoriesByUserId tests

        [Fact]
        public async Task GetToDoCategoriesByUserId_ShouldReturnCategoriesForCurrentUser()
        {

        }

        [Fact]
        public async Task GetToDoCategoriesByUserId_ShouldReturnEmptyList_WhenNoCategoriesExist()
        {

        }

        [Fact]
        public async Task GetToDoCategoriesByUserId_ShouldReturnUnauthorized_WhenTokenIsMissing()
        {

        }

        #endregion

        #region AddToDoCategory tests

        [Fact]
        public async Task AddToDoCategory_ShouldSuccessfullyCreateCategoryForCurrentUser()
        {

        }

        [Fact]
        public async Task AddToDoCategory_ShouldReturnError_WhenCategoryAlreadyExists()
        {

        }

        [Fact]
        public async Task AddToDoCategory_ShouldReturnError_WhenUnauthorized()
        {

        }

        #endregion

        #region UpdateToDoCategory tests

        [Fact]
        public async Task UpdateToDoCategory_ShouldSuccessfullyUpdateCategoryName()
        {

        }

        [Fact]
        public async Task UpdateToDoCategory_ShouldReturnError_WhenTryingToUpdateAnotherUserCategory()
        {

        }

        [Fact]
        public async Task UpdateToDoCategory_ShouldReturnError_WhenCategoryNameAlreadyExists()
        {

        }

        #endregion

        #region DeleteToDoCategory tests

        [Fact]
        public async Task DeleteToDoCategory_ShouldSuccessfullyDeleteCategory()
        {

        }

        [Fact]
        public async Task DeleteToDoCategory_ShouldReturnError_WhenTryingToDeleteAnotherUserCategory()
        {

        }

        [Fact]
        public async Task DeleteToDoCategory_ShouldReturnError_WhenCategoryNotFound()
        {

        }

        #endregion
    }
}
