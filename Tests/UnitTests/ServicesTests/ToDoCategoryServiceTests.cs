using Core.Services;
using Core.Models;
using Moq;
using Core.Interfaces;

namespace Tests.UnitTests.ServicesTests
{
    public class ToDoCategoryServiceTests
    {
        #region GetToDoCategoryByIdAsync(Guid toDoCategoryId) tests

        [Fact]
        public async Task GetToDoCategoryByIdAsync_ShouldReturnCategory_WhenCategoryIdExists()
        {
            var toDoCategoryId = Guid.NewGuid();
            var expectedCategory = new ToDoCategory("Test Category", Guid.NewGuid())
            {
                User = new User("TestUser", "hashed_password", 100)
            };

            var toDoCategoryRepositoryMock = new Mock<IToDoCategoryRepository>();
            toDoCategoryRepositoryMock.Setup(repo => repo.GetToDoCategoryByIdAsync(toDoCategoryId)).ReturnsAsync(expectedCategory);

            var toDoCategoryService = new ToDoCategoryService(toDoCategoryRepositoryMock.Object);

            var result = await toDoCategoryService.GetToDoCategoryByIdAsync(toDoCategoryId);

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
        public async Task GetToDoCategoryByIdAsync_ShouldThrowException_WhenCategoryIdDoesNotExist()
        {
            var toDoCategoryId = Guid.NewGuid();

            var toDoCategoryRepositoryMock = new Mock<IToDoCategoryRepository>();
            toDoCategoryRepositoryMock.Setup(repo => repo.GetToDoCategoryByIdAsync(toDoCategoryId)).ReturnsAsync((ToDoCategory?)null);

            var toDoCategoryService = new ToDoCategoryService(toDoCategoryRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => toDoCategoryService.GetToDoCategoryByIdAsync(toDoCategoryId));
            Assert.Equal("The category was not found.", exception.Message);
        }

        [Fact]
        public async Task GetToDoCategoryByIdAsync_ShouldThrowException_WhenCategoryIdIsEmpty()
        {
            var categoryId = Guid.Empty;

            var toDoCategoryRepositoryMock = new Mock<IToDoCategoryRepository>();
            var toDoCategoryService = new ToDoCategoryService(toDoCategoryRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => toDoCategoryService.GetToDoCategoryByIdAsync(categoryId));
            Assert.Equal("The category Id cannot be empty", exception.Message);
        }

        #endregion

        #region GetToDoCategoriesByUserIdAsync(Guid userId) tests

        [Fact]
        public async Task GetToDoCategoriesByUserIdAsync_ShouldReturnCategoryList_WhenCategoriesExist()
        {
            var userId = Guid.NewGuid();
            var expectedCategories = new List<ToDoCategory>
            {
                new ToDoCategory("Test Category1", userId),
                new ToDoCategory("Test Category2", userId)
            };

            var toDoCategoryRepositoryMock = new Mock<IToDoCategoryRepository>();
            toDoCategoryRepositoryMock.Setup(repo => repo.GetToDoCategoriesByUserIdAsync(userId)).ReturnsAsync(expectedCategories);

            var toDoCategoryService = new ToDoCategoryService(toDoCategoryRepositoryMock.Object);

            var result = await toDoCategoryService.GetToDoCategoriesByUserIdAsync(userId);

            Assert.NotNull(result);
            Assert.Equal(expectedCategories.Count, result.Count);
            Assert.All(result, category => Assert.Contains(expectedCategories, expC => expC.ToDoCategoryName == category.ToDoCategoryName));
        }

        [Fact]
        public async Task GetToDoCategoriesByUserIdAsync_ShouldReturnEmptyList_WhenNoCategoriesExist()
        {
            var userId = Guid.NewGuid();

            var toDoCategoryRepositoryMock = new Mock<IToDoCategoryRepository>();
            toDoCategoryRepositoryMock.Setup(repo => repo.GetToDoCategoriesByUserIdAsync(userId)).ReturnsAsync(new List<ToDoCategory>());

            var toDoCategoryService = new ToDoCategoryService(toDoCategoryRepositoryMock.Object);

            var result = await toDoCategoryService.GetToDoCategoriesByUserIdAsync(userId);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetToDoCategoriesByUserIdAsync_ShouldThrowException_WhenUserIdDoesNotExist()
        {
            var userId = Guid.NewGuid();

            var toDoCategoryRepositoryMock = new Mock<IToDoCategoryRepository>();
            toDoCategoryRepositoryMock.Setup(repo => repo.GetToDoCategoriesByUserIdAsync(userId)).ReturnsAsync((List<ToDoCategory>?)null);

            var toDoCategoryService = new ToDoCategoryService(toDoCategoryRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => toDoCategoryService.GetToDoCategoriesByUserIdAsync(userId));

            Assert.Equal("The user Id does not exist.", exception.Message);
        }

        #endregion

        #region AddToDoCategoryAsync(ToDoCategory toDoCategory) tests

        [Fact]
        public async Task AddToDoCategoryAsync_ShouldAddCategory_WhenCategoryIsValid()
        {
            var userId = Guid.NewGuid();
            string toDoCategoryName = "Test Category";
            var toDoCategory = new ToDoCategory(toDoCategoryName, userId);

            var toDoCategoryRepositoryMock = new Mock<IToDoCategoryRepository>();
            toDoCategoryRepositoryMock.Setup(repo => repo.AddToDoCategoryAsync(It.IsAny<ToDoCategory>())).Returns(Task.CompletedTask);

            var toDoCategoryService = new ToDoCategoryService(toDoCategoryRepositoryMock.Object);

            await toDoCategoryService.AddToDoCategoryAsync(toDoCategory);

            toDoCategoryRepositoryMock.Verify(repo => repo.AddToDoCategoryAsync(It.Is<ToDoCategory>(c => c.ToDoCategoryName == toDoCategoryName && c.UserId == userId)), Times.Once);
        }

        [Fact]
        public async Task AddToDoCategoryAsync_ShouldThrowException_WhenCategoryIsInvalid()
        {
            string invalidCategoryName = string.Empty;
            Guid userId = Guid.NewGuid();
            var toDoCategory = new ToDoCategory(invalidCategoryName, userId);

            var toDoCategoryRepositoryMock = new Mock<IToDoCategoryRepository>();
            var toDoCategoryService = new ToDoCategoryService(toDoCategoryRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => toDoCategoryService.AddToDoCategoryAsync(toDoCategory));

            Assert.Equal("Category name cannot be null or empty.", exception.Message);

            toDoCategoryRepositoryMock.Verify(repo => repo.AddToDoCategoryAsync(It.IsAny<ToDoCategory>()), Times.Never);
        }

        [Fact]
        public async Task AddToDoCategoryAsync_ShouldThrowException_WhenCategoryIsNull()
        {
            var toDoCategoryRepositoryMock = new Mock<IToDoCategoryRepository>();
            var toDoCategoryService = new ToDoCategoryService(toDoCategoryRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => toDoCategoryService.AddToDoCategoryAsync(null));

            Assert.Equal("Category cannot be null. (Parameter 'toDoCategory')", exception.Message);
        }

        #endregion

        #region UpdateToDoCategoryAsync(ToDoCategory toDoCategory) tests

        [Fact]
        public async Task UpdateToDoCategoryAsync_ShouldUpdateCategory_WhenCategoryIsValid()
        {
            string toDoCategoryName = "Test Category";
            var userId = Guid.NewGuid();
            var toDoCategory = new ToDoCategory(toDoCategoryName, userId);

            var toDoCategoryRepositoryMock = new Mock<IToDoCategoryRepository>();
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
            var toDoCategory = new ToDoCategory(toDoCategoryName, userId);

            var toDoCategoryRepositoryMock = new Mock<IToDoCategoryRepository>();
            toDoCategoryRepositoryMock.Setup(repo => repo.GetToDoCategoryByIdAsync(It.IsAny<Guid>())).ReturnsAsync((ToDoCategory?)null);

            var toDoCategoryService = new ToDoCategoryService(toDoCategoryRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => toDoCategoryService.UpdateToDoCategoryAsync(toDoCategory));

            Assert.Equal("The category does not exist.", exception.Message);
        }

        [Fact]
        public async Task UpdateToDoCategoryAsync_ShouldThrowException_WhenCategoryIsNull()
        {
            var toDoCategoryRepositoryMock = new Mock<IToDoCategoryRepository>();
            var toDoCategoryService = new ToDoCategoryService(toDoCategoryRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => toDoCategoryService.UpdateToDoCategoryAsync(null));

            Assert.Equal("The category cannot be null. (Parameter 'toDoCategory')", exception.Message);
        }

        #endregion

        #region DeleteToDoCategoryAsync(Guid toDoCategoryId) tests

        [Fact]
        public async Task DeleteToDoCategoryAsync_ShouldDeleteCategory_WhenCategoryIdExists()
        {
            var toDoCategoryId = Guid.NewGuid();

            var toDoCategoryRepositoryMock = new Mock<IToDoCategoryRepository>();
            toDoCategoryRepositoryMock.Setup(repo => repo.DeleteToDoCategoryAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);

            var toDoCategoryService = new ToDoCategoryService(toDoCategoryRepositoryMock.Object);

            await toDoCategoryService.DeleteToDoCategoryAsync(toDoCategoryId);

            toDoCategoryRepositoryMock.Verify(repo => repo.DeleteToDoCategoryAsync(It.Is<Guid>(c => c == toDoCategoryId)), Times.Once);
        }

        [Fact]
        public async Task DeleteToDoCategoryAsync_ShouldThrowException_WhenCategoryIdDoesNotExist()
        {
            var toDoCategoryId = Guid.NewGuid();

            var toDoCategoryRepositoryMock = new Mock<IToDoCategoryRepository>();
            toDoCategoryRepositoryMock.Setup(repo => repo.GetToDoCategoryByIdAsync(It.IsAny<Guid>())).ReturnsAsync((ToDoCategory?)null);

            var toDoCategoryService = new ToDoCategoryService(toDoCategoryRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => toDoCategoryService.DeleteToDoCategoryAsync(toDoCategoryId));

            Assert.Equal("Category does not exist.", exception.Message);
        }

        [Fact]
        public async Task DeleteToDoCategoryAsync_ShouldThrowException_WhenCategoryIdIsEmpty()
        {
            var toDoCategoryRepositoryMock = new Mock<IToDoCategoryRepository>();
            var toDoCategoryService = new ToDoCategoryService(toDoCategoryRepositoryMock.Object);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => toDoCategoryService.DeleteToDoCategoryAsync(new Guid()));

            Assert.Equal("Category id cannot be null. (Parameter 'toDoCategoryId')", exception.Message);
        }

        #endregion
    }
}
