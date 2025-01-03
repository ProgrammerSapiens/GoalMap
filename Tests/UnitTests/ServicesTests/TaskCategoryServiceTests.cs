namespace Tests.UnitTests.ServicesTests
{
    public class TaskCategoryServiceTests
    {
        #region GetTaskCategoryByIdAsync(Guid taskCategoryId) tests

        [Fact]
        public async Task GetTaskCategoryById_ShouldReturnCategory_WhenCategoryIdExists()
        {

        }

        [Fact]
        public async Task GetTaskCategoryById_ShouldThrowException_WhenCategoryIdDoesNotExist()
        {

        }

        [Fact]
        public async Task GetTaskCategoryById_ShouldThrowException_WhenCategoryIdIsEmpty()
        {

        }

        #endregion

        #region GetTaskCategoriesByUserAsync(Guid userId) tests

        [Fact]
        public async Task GetCategoriesByUser_ShouldReturnCategoryList_WhenCategoriesExist()
        {

        }

        [Fact]
        public async Task GetCategoriesByUser_ShouldReturnEmptyList_WhenNoCategoriesExist()
        {

        }

        [Fact]
        public async Task GetCategoriesByUser_ShouldThrowException_WhenUserIdDoesNotExist()
        {

        }

        #endregion

        #region AddTaskCategoryAsync(TaskCategory taskCategory) tests

        [Fact]
        public async Task AddTaskCategory_ShouldAddCategory_WhenCategoryIsValid()
        {

        }

        [Fact]
        public async Task AddTaskCategory_ShouldThrowException_WhenCategoryIsInvalid()
        {

        }

        [Fact]
        public async Task AddTaskCategory_ShouldThrowException_WhenCategoryIsNull()
        {

        }

        #endregion

        #region UpdateTaskCategoryAsync(TaskCategory taskCategory) tests

        [Fact]
        public async Task UpdateTaskCategory_ShouldUpdateCategory_WhenCategoryIsValid()
        {

        }

        [Fact]
        public async Task UpdateTaskCategory_ShouldThrowException_WhenCategoryDoesNotExist()
        {

        }

        [Fact]
        public async Task UpdateTaskCategory_ShouldThrowException_WhenCategoryIsNull()
        {

        }

        #endregion

        #region DeleteTaskCategory(Guid taskCategoryId) tests

        [Fact]
        public async Task DeleteTaskCategory_ShouldDeleteCategory_WhenCategoryIdExists()
        {

        }

        [Fact]
        public async Task DeleteTaskCategory_ShouldThrowException_WhenCategoryIdDoesNotExist()
        {

        }

        [Fact]
        public async Task DeleteTaskCategory_ShouldThrowException_WhenCategoryIdIsEmpty()
        {

        }

        #endregion
    }
}
