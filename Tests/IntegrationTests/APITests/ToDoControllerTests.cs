namespace Tests.IntegrationTests.APITests
{
    public class ToDoControllerTests : IClassFixture<CustomWebApplicationFactory<TestProgram>>
    {
        private readonly HttpClient _httpClient;

        public ToDoControllerTests(CustomWebApplicationFactory<TestProgram> factory)
        {
            _httpClient = factory.CreateClient();
        }

        #region GetToDoById tests

        [Fact]
        public async Task GetToDoById_ShouldReturnToDoById()
        {

        }

        [Fact]
        public async Task GetToDoById_ShouldReturnEmptyToDo_WhenToDoDoesNotExist()
        {

        }

        [Fact]
        public async Task GetToDoById_ShouldReturnError_WhenTryingToGetFromAnotherUserCategory()
        {

        }

        #endregion

        #region GetToDos tests

        [Fact]
        public async Task GetToDos_ShouldReturnToDosForCurrentUser()
        {

        }

        [Fact]
        public async Task GetToDos_ShouldReturnToDosForSpecificCategory()
        {

        }

        [Fact]
        public async Task GetToDos_ShouldReturnEmptyToDo_WhenNoToDosExist()
        {

        }

        [Fact]
        public async Task GetToDos_ShouldReturnError_WhenTryingToGetFromAnotherUserCategory()
        {

        }

        #endregion

        #region AddToDo tests

        [Fact]
        public async Task AddToDo_ShouldSuccessfullyCreateToDo()
        {

        }

        [Fact]
        public async Task AddToDo_ShouldREturnError_WhenCategoryNotFound()
        {

        }

        [Fact]
        public async Task AddToDo_ShouldReturnError_WhenTryingToCreateInAnotherUserCategory()
        {

        }

        [Fact]
        public async Task AddToDo_ShouldReturnError_WhenInvalidDataIsProvided()
        {

        }

        #endregion

        #region UpdateToDo tests

        [Fact]
        public async Task UpdateToDo_ShouldSuccessfullyUpdateToDo()
        {

        }

        [Fact]
        public async Task UpdateToDo_ShouldReturnError_WhenTryingToUpdateAnotherUserToDo()
        {

        }

        [Fact]
        public async Task UpdateToDo_ShouldReturnError_WhenToDoNotFound()
        {

        }

        #endregion

        #region DeleteToDo tests

        [Fact]
        public async Task DeleteToDo_ShouldSuccessfullyDeleteToDo()
        {

        }

        [Fact]
        public async Task DeleteToDo_ShouldReturnError_WhenTryingToDeleteAnotherUserToDo()
        {

        }

        [Fact]
        public async Task DeleteToDo_ShouldReturnError_WhenToDoNotFound()
        {

        }

        #endregion
    }
}
