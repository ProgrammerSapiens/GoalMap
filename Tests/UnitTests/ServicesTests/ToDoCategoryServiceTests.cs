using Core.Services;
using Core.Models;
using Moq;
using Core.Interfaces;

namespace Tests.UnitTests.ServicesTests
{
    public class ToDoCategoryServiceTests
    {
        private readonly Mock<IToDoCategoryRepository> toDoCategoryRepositoryMock;

        public ToDoCategoryServiceTests()
        {
            toDoCategoryRepositoryMock = new Mock<IToDoCategoryRepository>();
        }

        #region GetToDoCategoryByCategoryNameAsync(Guid userId, string toDoCategoryName) tests

        [Fact]
        public async Task GetToDoCategoryByCategoryNameAsync_ShouldReturnCategory_WhenCategoryNameExists()
        {
            var toDoCategoryName = "Test Category";
            var userId = Guid.NewGuid();
            var expectedCategory = new ToDoCategory(userId, toDoCategoryName)
            {
                User = new User("TestUser", "hashed_password", 100)
            };

            toDoCategoryRepositoryMock.Setup(repo => repo.IsCategoryExistsAsync(toDoCategoryName, userId)).ReturnsAsync(true);
            toDoCategoryRepositoryMock.Setup(repo => repo.GetToDoCategoryByCategoryNameAsync(toDoCategoryName, userId)).ReturnsAsync(expectedCategory);

            var toDoCategoryService = new ToDoCategoryService(toDoCategoryRepositoryMock.Object);

            var result = await toDoCategoryService.GetToDoCategoryByCategoryNameAsync(userId, toDoCategoryName);

            Assert.NotNull(result);
            Assert.Equal(expectedCategory.ToDoCategoryName, result.ToDoCategoryName);
            Assert.Equal(expectedCategory.ToDoCategoryId, result.ToDoCategoryId);
            Assert.Equal(expectedCategory.UserId, result.UserId);
            Assert.NotNull(result.User);
            Assert.Equal(expectedCategory.User.UserName, result.User.UserName);
            Assert.Equal(expectedCategory.User.PasswordHash, result.User.PasswordHash);
            Assert.Equal(expectedCategory.User.Experience, result.User.Experience);
        }

        [Fact]
        public async Task GetToDoCategoryByCategoryNameAsync_ShouldThrowException_WhenCategoryDoesNotExist()
        {
            var toDoCategoryName = "Test Category";
            var userId = Guid.NewGuid();

            toDoCategoryRepositoryMock.Setup(repo => repo.IsCategoryExistsAsync(toDoCategoryName, userId)).ReturnsAsync(false);

            var toDoCategoryService = new ToDoCategoryService(toDoCategoryRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => toDoCategoryService.GetToDoCategoryByCategoryNameAsync(userId, toDoCategoryName));

            Assert.Equal("Category does not exist.", exception.Message);
        }

        [Fact]
        public async Task GetToDoCategoryByCategoryNameAsync_ShouldThrowException_WhenUserIdIsEmpty()
        {
            var userId = new Guid();

            var toDoCategoryService = new ToDoCategoryService(toDoCategoryRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => toDoCategoryService.GetToDoCategoryByCategoryNameAsync(userId, "Test Category"));

            Assert.Equal("User id cannot be empty.", exception.Message);
        }

        #endregion

        #region GetToDoCategoriesByUserIdAsync(Guid userId) tests

        [Fact]
        public async Task GetToDoCategoriesByUserIdAsync_ShouldReturnCategoryList_WhenCategoriesExist()
        {
            var userId = Guid.NewGuid();
            var expectedCategories = new List<ToDoCategory>
            {
                new ToDoCategory(userId, "Test Category1"),
                new ToDoCategory(userId, "Test Category2")
            };

            toDoCategoryRepositoryMock.Setup(repo => repo.GetToDoCategoriesByUserIdAsync(userId)).ReturnsAsync(expectedCategories);

            var toDoCategoryService = new ToDoCategoryService(toDoCategoryRepositoryMock.Object);

            var result = await toDoCategoryService.GetToDoCategoriesByUserIdAsync(userId);

            Assert.NotNull(result);
            Assert.Equal(expectedCategories.Count, result.Count);
            Assert.All(result, category => Assert.Contains(expectedCategories, expC => expC.ToDoCategoryName == category.ToDoCategoryName));
        }

        [Fact]
        public async Task GetToDoCategoriesByUserIdAsync_ShouldThrowException_WhenUserIdDoesNotExistOrThereIsNoCategories()
        {
            var userId = Guid.NewGuid();

            toDoCategoryRepositoryMock.Setup(repo => repo.GetToDoCategoriesByUserIdAsync(userId)).ReturnsAsync(new List<ToDoCategory>());

            var toDoCategoryService = new ToDoCategoryService(toDoCategoryRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => toDoCategoryService.GetToDoCategoriesByUserIdAsync(userId));

            Assert.Equal("User Id does not exist or there is no categories.", exception.Message);
        }

        [Fact]
        public async Task GetToDoCategoriesByUserIdAsync_ShouldThrowException_WhenUserIdIsEmpty()
        {
            var userId = new Guid();

            var toDoCategoryService = new ToDoCategoryService(toDoCategoryRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => toDoCategoryService.GetToDoCategoriesByUserIdAsync(userId));

            Assert.Equal("User id cannot be empty.", exception.Message);
        }

        #endregion

        #region AddToDoCategoryAsync(ToDoCategory toDoCategory) tests

        [Fact]
        public async Task AddToDoCategoryAsync_ShouldAddCategory_WhenCategoryIsValid()
        {
            var userId = Guid.NewGuid();
            string toDoCategoryName = "Test Category";
            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            toDoCategoryRepositoryMock.Setup(repo => repo.AddToDoCategoryAsync(It.IsAny<ToDoCategory>())).Returns(Task.CompletedTask);

            var toDoCategoryService = new ToDoCategoryService(toDoCategoryRepositoryMock.Object);

            await toDoCategoryService.AddToDoCategoryAsync(toDoCategory);

            toDoCategoryRepositoryMock.Verify(repo => repo.AddToDoCategoryAsync(It.Is<ToDoCategory>(c => c.ToDoCategoryName == toDoCategoryName && c.UserId == userId)), Times.Once);
        }

        [Fact]
        public async Task AddToDoCategoryAsync_ShouldThrowException_WhenCategoryIsAlreadyExist()
        {
            var userId = Guid.NewGuid();
            string toDoCategoryName = "Test Category";
            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            toDoCategoryRepositoryMock.Setup(repo => repo.IsCategoryExistsAsync(toDoCategory.ToDoCategoryName, userId)).ReturnsAsync(true);

            var toDoCategoryService = new ToDoCategoryService(toDoCategoryRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => toDoCategoryService.AddToDoCategoryAsync(toDoCategory));

            toDoCategoryRepositoryMock.Verify(repo => repo.IsCategoryExistsAsync(toDoCategory.ToDoCategoryName, userId), Times.Once);
            Assert.Equal("Сategory already exists.", exception.Message);
        }

        #endregion

        #region UpdateToDoCategoryAsync(ToDoCategory toDoCategory) tests

        [Fact]
        public async Task UpdateToDoCategoryAsync_ShouldUpdateCategory_WhenCategoryIsValid()
        {
            string toDoCategoryName = "Test Category";
            var userId = Guid.NewGuid();
            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            toDoCategoryRepositoryMock.Setup(repo => repo.IsCategoryExistsAsync(toDoCategoryName, userId)).ReturnsAsync(true);
            toDoCategoryRepositoryMock.Setup(repo => repo.UpdateToDoCategoryAsync(It.IsAny<ToDoCategory>())).Returns(Task.CompletedTask);

            var toDoCategoryService = new ToDoCategoryService(toDoCategoryRepositoryMock.Object);

            await toDoCategoryService.UpdateToDoCategoryAsync(toDoCategory);

            toDoCategoryRepositoryMock.Verify(repo => repo.UpdateToDoCategoryAsync(It.Is<ToDoCategory>(c => c.UserId == userId && c.ToDoCategoryName == toDoCategoryName)), Times.Once());
        }

        [Fact]
        public async Task UpdateToDoCategoryAsync_ShouldThrowException_WhenCategoryDoesNotExist()
        {
            string toDoCategoryName = "Test Category";
            var userId = Guid.NewGuid();
            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            toDoCategoryRepositoryMock.Setup(repo => repo.IsCategoryExistsAsync(toDoCategoryName, userId)).ReturnsAsync(false);

            var toDoCategoryService = new ToDoCategoryService(toDoCategoryRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => toDoCategoryService.UpdateToDoCategoryAsync(toDoCategory));

            Assert.Equal("Category does not exist.", exception.Message);
        }

        [Theory]
        [InlineData("Habbit")]
        [InlineData("Other")]
        public async Task UpdateToDoCategoryAsync_ShouldThrowException_WhenCategoryIsDefault(string toDoCategoryName)
        {
            var userId = Guid.NewGuid();
            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            toDoCategoryRepositoryMock.Setup(repo => repo.IsCategoryExistsAsync(toDoCategoryName, userId)).ReturnsAsync(true);

            var toDoCategoryService = new ToDoCategoryService(toDoCategoryRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => toDoCategoryService.UpdateToDoCategoryAsync(toDoCategory));

            Assert.Equal("You cannot update this category.", exception.Message);
            toDoCategoryRepositoryMock.Verify(repo => repo.UpdateToDoCategoryAsync(toDoCategory), Times.Never());
        }

        #endregion

        #region DeleteToDoCategoryAsync(Guid toDoCategoryId) tests

        [Fact]
        public async Task DeleteToDoCategoryAsync_ShouldDeleteCategory_WhenCategoryIdExists()
        {
            var toDoCategoryName = "Test Category";
            var userId = Guid.NewGuid();

            toDoCategoryRepositoryMock.Setup(repo => repo.IsCategoryExistsAsync(toDoCategoryName, userId)).ReturnsAsync(true);
            toDoCategoryRepositoryMock.Setup(repo => repo.DeleteToDoCategoryAsync(toDoCategoryName, userId)).Returns(Task.CompletedTask);

            var toDoCategoryService = new ToDoCategoryService(toDoCategoryRepositoryMock.Object);

            await toDoCategoryService.DeleteToDoCategoryAsync(userId, toDoCategoryName);

            toDoCategoryRepositoryMock.Verify(repo => repo.DeleteToDoCategoryAsync(toDoCategoryName, userId), Times.Once);
        }

        [Fact]
        public async Task DeleteToDoCategoryAsync_ShouldThrowException_WhenCategoryIdDoesNotExist()
        {
            var toDoCategoryName = "Test Category";
            var userId = Guid.NewGuid();

            toDoCategoryRepositoryMock.Setup(repo => repo.IsCategoryExistsAsync(toDoCategoryName, userId)).ReturnsAsync(false);

            var toDoCategoryService = new ToDoCategoryService(toDoCategoryRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => toDoCategoryService.DeleteToDoCategoryAsync(userId, toDoCategoryName));

            Assert.Equal("Category does not exist.", exception.Message);
        }

        [Theory]
        [InlineData("Habbit")]
        [InlineData("Other")]
        public async Task DeleteToDoCategoryAsync_ShouldThrowException_WhenCategoryIsDefault(string toDoCategoryName)
        {
            var userId = Guid.NewGuid();

            toDoCategoryRepositoryMock.Setup(repo => repo.IsCategoryExistsAsync(toDoCategoryName, userId)).ReturnsAsync(true);
            toDoCategoryRepositoryMock.Setup(repo => repo.DeleteToDoCategoryAsync(toDoCategoryName, userId)).Returns(Task.CompletedTask);

            var toDoCategoryService = new ToDoCategoryService(toDoCategoryRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => toDoCategoryService.DeleteToDoCategoryAsync(userId, toDoCategoryName));

            Assert.Equal("You cannot delete this category.", exception.Message);
            toDoCategoryRepositoryMock.Verify(repo => repo.DeleteToDoCategoryAsync(toDoCategoryName, userId), Times.Never());
        }

        [Fact]
        public async Task DeleteToDoCategoryAsync_ShouldThrowException_WhenUserIdIsEmpty()
        {
            var userId = new Guid();

            var toDoCategoryService = new ToDoCategoryService(toDoCategoryRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => toDoCategoryService.DeleteToDoCategoryAsync(userId, "Test Category"));

            Assert.Equal("User id cannot be empty.", exception.Message);
        }
        #endregion
    }
}
