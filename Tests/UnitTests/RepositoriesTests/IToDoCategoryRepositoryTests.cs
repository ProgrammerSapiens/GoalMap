using Core.Interfaces;

namespace Tests.UnitTests.RepositoriesTests
{
    public class IToDoCategoryRepositoryTests
    {
        #region GetToDoCategoryByCategoryNameAsync(string toDoCategoryName, Guid userId) tests

        [Fact]
        public async Task GetToDoCategoryByCategoryNameAsync_ShouldReturnCategory_WhenCategoryExistsForUser()
        {

        }

        [Fact]
        public async Task GetToDoCategoryByCategoryNameAsync_ShouldReturnNull_WhenCategoryDoesNotExistForUser()
        {

        }

        #endregion

        #region GetToDoCategoriesByUserIdAsync(Guid userId) tests

        [Fact]
        public async Task GetToDoCategoryByCategoryNameAsync_ShouldReturnNull_WhenCategoryDoesNotExistForUser()
        {

        }

        [Fact]
        public async Task GetToDoCategoriesByUserIdAsync_ShouldReturnEmptyList_WhenNoCategoriesExistForUser()
        {

        }

        #endregion

        #region AddToDoCategoryAsync(ToDoCategory toDoCategory) tests

        [Fact]
        public async Task AddToDoCategoryAsync_ShouldAddCategory_WhenDataIsValid()
        {

        }

        [Fact]
        public async Task AddToDoCategoryAsync_ShouldThrowException_WhenCategoryNameAlreadyExistsForUser()
        {

        }

        #endregion

        #region UpdateToDoCategoryAsync(ToDoCategory toDoCategory) tests

        [Fact]
        public async Task UpdateToDoCategoryAsync_ShouldUpdateCategory_WhenCategoryExists()
        {

        }

        [Fact]
        public async Task UpdateToDoCategoryAsync_ShouldThrowException_WhenCategoryDoesNotExist()
        {

        }

        #endregion

        #region DeleteToDoCategoryAsync(string toDoCategoryName, Guid userId) tests

        [Fact]
        public async Task DeleteToDoCategoryAsync_ShouldDeleteCategory_WhenCategoryExistsForUser()
        {

        }

        [Fact]
        public async Task DeleteToDoCategoryAsync_ShouldDoNothing_WhenCategoryDoesNotExistForUser()
        {

        }

        #endregion

        #region IsCategoryExistsAsync(string toDoCategoryName, Guid userId) tests

        [Fact]
        public async Task IsCategoryExistsAsync_ShouldReturnTrue_WhenCategoryExistsForUser()
        {

        }

        [Fact]
        public async Task IsCategoryExistsAsync_ShouldReturnFalse_WhenCategoryDoesNotExistForUser()
        {

        }

        #endregion
    }
}
