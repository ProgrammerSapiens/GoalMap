namespace Tests.UnitTests.ServicesTests
{
    public class ToDoCategoryServiceTests
    {
        #region GetToDoCategoryByIdAsync(Guid toDoCategoryId) tests

        [Fact]
        public async Task GetToDoCategoryByIdAsync_ShouldReturnCategory_WhenCategoryIdExists()
        {

        }

        [Fact]
        public async Task GetToDoCategoryByIdAsync_ShouldThrowException_WhenCategoryIdDoesNotExist()
        {

        }

        [Fact]
        public async Task GetToDoCategoryByIdAsync_ShouldThrowException_WhenCategoryIdIsEmpty()
        {

        }

        #endregion

        #region GetToDoCategoriesByUserAsync(Guid userId) tests

        [Fact]
        public async Task GetToDoCategoriesByUserAsync_ShouldReturnCategoryList_WhenCategoriesExist()
        {

        }

        [Fact]
        public async Task GetToDoCategoriesByUserAsync_ShouldReturnEmptyList_WhenNoCategoriesExist()
        {

        }

        [Fact]
        public async Task GetToDoCategoriesByUserAsync_ShouldThrowException_WhenUserIdDoesNotExist()
        {

        }

        #endregion

        #region AddToDoCategoryAsync(ToDoCategory toDoCategory) tests

        [Fact]
        public async Task AddToDoCategoryAsync_ShouldAddCategory_WhenCategoryIsValid()
        {

        }

        [Fact]
        public async Task AddToDoCategoryAsync_ShouldThrowException_WhenCategoryIsInvalid()
        {

        }

        [Fact]
        public async Task AddToDoCategoryAsync_ShouldThrowException_WhenCategoryIsNull()
        {

        }

        #endregion

        #region UpdateToDoCategoryAsync(ToDoCategory toDoCategory) tests

        [Fact]
        public async Task UpdateToDoCategoryAsync_ShouldUpdateCategory_WhenCategoryIsValid()
        {

        }

        [Fact]
        public async Task UpdateToDoCategoryAsync_ShouldThrowException_WhenCategoryDoesNotExist()
        {

        }

        [Fact]
        public async Task UpdateToDoCategoryAsync_ShouldThrowException_WhenCategoryIsNull()
        {

        }

        #endregion

        #region DeleteToDoCategoryAsync(Guid toDoCategoryId) tests

        [Fact]
        public async Task DeleteToDoCategoryAsync_ShouldDeleteCategory_WhenCategoryIdExists()
        {

        }

        [Fact]
        public async Task DeleteToDoCategoryAsync_ShouldThrowException_WhenCategoryIdDoesNotExist()
        {

        }

        [Fact]
        public async Task DeleteToDoCategoryAsync_ShouldThrowException_WhenCategoryIdIsEmpty()
        {

        }

        #endregion
    }
}
