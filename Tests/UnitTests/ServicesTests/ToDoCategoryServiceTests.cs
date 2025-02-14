using Core.Services;
using Core.Models;
using Moq;
using Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Tests.UnitTests.ServicesTests
{
    public class ToDoCategoryServiceTests
    {
        private readonly Mock<IToDoCategoryRepository> _toDoCategoryRepositoryMock;
        private readonly Mock<ILogger<ToDoCategoryService>> _loggerMock;
        private readonly IToDoCategoryService _toDoCategoryService;

        public ToDoCategoryServiceTests()
        {
            _toDoCategoryRepositoryMock = new Mock<IToDoCategoryRepository>();
            _loggerMock = new Mock<ILogger<ToDoCategoryService>>();

            _toDoCategoryService = new ToDoCategoryService(_toDoCategoryRepositoryMock.Object, _loggerMock.Object);
        }

        #region GetToDoCategoryByCategoryIdAsync(Guid userId, Guid toDoCategoryId) tests

        [Fact]
        public async Task GetToDoCategoryByCategoryIdAsync_ShouldReturnCategory_WhenCategoryNameExists()
        {
            var toDoCategoryName = "Test Category";
            var userId = Guid.NewGuid();
            var expectedCategory = new ToDoCategory(userId, toDoCategoryName);
            var toDoCategoryId = expectedCategory.ToDoCategoryId;

            _toDoCategoryRepositoryMock.Setup(repo => repo.GetToDoCategoryByCategoryIdAsync(toDoCategoryId)).ReturnsAsync(expectedCategory);

            var result = await _toDoCategoryService.GetToDoCategoryByCategoryIdAsync(toDoCategoryId);

            Assert.NotNull(result);
            Assert.Equal(expectedCategory.ToDoCategoryName, result.ToDoCategoryName);
            Assert.Equal(expectedCategory.ToDoCategoryId, result.ToDoCategoryId);
            Assert.Equal(expectedCategory.UserId, result.UserId);
        }

        [Fact]
        public async Task GetToDoCategoryByCategoryIdAsync_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var toDoCategoryId = Guid.NewGuid();

            _toDoCategoryRepositoryMock.Setup(repo => repo.GetToDoCategoryByCategoryIdAsync(toDoCategoryId)).ReturnsAsync((ToDoCategory?)null);

            var result = await _toDoCategoryService.GetToDoCategoryByCategoryIdAsync(toDoCategoryId);
            Assert.Null(result);
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

            _toDoCategoryRepositoryMock.Setup(repo => repo.GetToDoCategoriesByUserIdAsync(userId)).ReturnsAsync(expectedCategories);

            var result = await _toDoCategoryService.GetToDoCategoriesByUserIdAsync(userId);

            Assert.NotNull(result);
            Assert.Equal(expectedCategories.Count, result.Count);
            Assert.All(result, category => Assert.Contains(expectedCategories, expC => expC.ToDoCategoryName == category.ToDoCategoryName));
        }

        [Fact]
        public async Task GetToDoCategoriesByUserIdAsync_ShouldReturnEmptyList_WhenNoCategoriesExist()
        {
            var userId = Guid.NewGuid();

            _toDoCategoryRepositoryMock.Setup(repo => repo.GetToDoCategoriesByUserIdAsync(userId)).ReturnsAsync(new List<ToDoCategory>());

            var result = await _toDoCategoryService.GetToDoCategoriesByUserIdAsync(userId);

            Assert.Empty(result);
        }

        #endregion

        #region AddToDoCategoryAsync(ToDoCategory toDoCategory) tests

        [Fact]
        public async Task AddToDoCategoryAsync_ShouldAddCategory_WhenCategoryIsValid()
        {
            var userId = Guid.NewGuid();
            string toDoCategoryName = "Test category";
            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            _toDoCategoryRepositoryMock.Setup(repo => repo.AddToDoCategoryAsync(It.IsAny<ToDoCategory>())).Returns(Task.CompletedTask);

            await _toDoCategoryService.AddToDoCategoryAsync(toDoCategory);

            _toDoCategoryRepositoryMock.Verify(repo => repo.AddToDoCategoryAsync(It.Is<ToDoCategory>(c => c.ToDoCategoryName == toDoCategoryName && c.UserId == userId)), Times.Once);
        }

        [Fact]
        public async Task AddToDoCategoryAsync_ShouldThrowException_WhenCategoryIsAlreadyExist()
        {
            var userId = Guid.NewGuid();
            string toDoCategoryName = "Test category";
            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            _toDoCategoryRepositoryMock.Setup(repo => repo.CategoryExistsByNameAsync(userId, toDoCategoryName)).ReturnsAsync(true);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _toDoCategoryService.AddToDoCategoryAsync(toDoCategory));

            _toDoCategoryRepositoryMock.Verify(repo => repo.CategoryExistsByNameAsync(userId, toDoCategory.ToDoCategoryName), Times.Once);
            Assert.Equal("Category with such name already exists.", exception.Message);
        }

        [Fact]
        public async Task AddToDoCategoryAsync_ShouldThrowException_WhenCategoryNameContainsDigits()
        {
            var userId = Guid.NewGuid();
            var toDoCategoryName = "Test1 category";

            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            _toDoCategoryRepositoryMock.Setup(repo => repo.CategoryExistsByNameAsync(userId, toDoCategoryName)).ReturnsAsync(false);

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _toDoCategoryService.AddToDoCategoryAsync(toDoCategory));

            _toDoCategoryRepositoryMock.Verify(repo => repo.CategoryExistsByNameAsync(userId, toDoCategory.ToDoCategoryName), Times.Once);
            Assert.Equal("Category name cannot contain digits.", exception.Message);
        }

        #endregion

        #region UpdateToDoCategoryAsync(ToDoCategory toDoCategory) tests

        [Fact]
        public async Task UpdateToDoCategoryAsync_ShouldUpdateCategory_WhenCategoryIsValid()
        {
            var userId = Guid.NewGuid();
            var oldToDoCategoryName = "Test category";
            var newToDoCategoryName = "New name";

            var existingToDoCategory = new ToDoCategory(userId, oldToDoCategoryName);
            var oldToDoCategoryId = existingToDoCategory.ToDoCategoryId;
            var newToDoCategory = new ToDoCategory(userId, newToDoCategoryName);
            var newToDoCategoryId = newToDoCategory.ToDoCategoryId;
            var toDoCategoryId = newToDoCategory.ToDoCategoryId;

            var oldCategoryNameCope = existingToDoCategory.ToDoCategoryName;

            _toDoCategoryRepositoryMock.Setup(repo => repo.GetToDoCategoryByCategoryIdAsync(toDoCategoryId)).ReturnsAsync(existingToDoCategory);
            _toDoCategoryRepositoryMock.Setup(repo => repo.CategoryExistsByNameAsync(userId, newToDoCategoryName)).ReturnsAsync(false);
            _toDoCategoryRepositoryMock.Setup(repo => repo.UpdateToDoCategoryAsync(It.IsAny<ToDoCategory>())).Returns(Task.CompletedTask);
            _toDoCategoryRepositoryMock.Setup(repo => repo.UpdateCategoryInToDosAsync(userId, oldToDoCategoryId, newToDoCategoryId)).Returns(Task.CompletedTask);

            await _toDoCategoryService.UpdateToDoCategoryAsync(newToDoCategory);

            _toDoCategoryRepositoryMock.Verify(repo => repo.GetToDoCategoryByCategoryIdAsync(toDoCategoryId), Times.Once());
            _toDoCategoryRepositoryMock.Verify(repo => repo.CategoryExistsByNameAsync(userId, newToDoCategoryName), Times.Once());
            _toDoCategoryRepositoryMock.Verify(repo => repo.UpdateToDoCategoryAsync(It.Is<ToDoCategory>(c => c.UserId == userId && c.ToDoCategoryName == newToDoCategoryName)), Times.Once());
            _toDoCategoryRepositoryMock.Verify(repo => repo.UpdateCategoryInToDosAsync(userId, oldToDoCategoryId, newToDoCategoryId), Times.Once());
        }

        [Fact]
        public async Task UpdateToDoCategoryAsync_ShouldThrowException_WhenCategoryDoesNotExist()
        {
            string toDoCategoryName = "Test Category";
            var userId = Guid.NewGuid();
            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            _toDoCategoryRepositoryMock.Setup(repo => repo.CategoryExistsByNameAsync(userId, toDoCategoryName)).ReturnsAsync(false);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _toDoCategoryService.UpdateToDoCategoryAsync(toDoCategory));

            Assert.Equal("Category was not found.", exception.Message);
        }

        [Theory]
        [InlineData("Habbit")]
        [InlineData("Other")]
        public async Task UpdateToDoCategoryAsync_ShouldThrowException_WhenCategoryIsDefault(string toDoCategoryName)
        {
            var userId = Guid.NewGuid();
            var existingToDoCategory = new ToDoCategory(userId, toDoCategoryName);
            var newToDoCategory = new ToDoCategory(userId, toDoCategoryName);

            _toDoCategoryRepositoryMock.Setup(repo => repo.GetToDoCategoryByCategoryIdAsync(newToDoCategory.ToDoCategoryId)).ReturnsAsync(existingToDoCategory);

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _toDoCategoryService.UpdateToDoCategoryAsync(newToDoCategory));

            Assert.Equal("You cannot update this category.", exception.Message);
            _toDoCategoryRepositoryMock.Verify(repo => repo.UpdateToDoCategoryAsync(newToDoCategory), Times.Never());
        }

        #endregion

        #region DeleteToDoCategoryAsync(Guid toDoCategoryId) tests

        [Fact]
        public async Task DeleteToDoCategoryAsync_ShouldDeleteCategory_WhenCategoryExists()
        {
            var toDoCategoryName = "Test category";
            var userId = Guid.NewGuid();
            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);
            var defaultToDoCategory = new ToDoCategory(userId, "Other");
            var toDoCategoryId = toDoCategory.ToDoCategoryId;
            var newToDoCategoryId = defaultToDoCategory.ToDoCategoryId;

            _toDoCategoryRepositoryMock.Setup(repo => repo.GetToDoCategoryByCategoryIdAsync(toDoCategoryId)).ReturnsAsync(toDoCategory);
            _toDoCategoryRepositoryMock.Setup(repo => repo.GetToDoCategoryByCategoryNameAsync("Other")).ReturnsAsync(defaultToDoCategory);
            _toDoCategoryRepositoryMock.Setup(repo => repo.DeleteToDoCategoryAsync(toDoCategoryId)).Returns(Task.CompletedTask);
            _toDoCategoryRepositoryMock.Setup(repo => repo.UpdateCategoryInToDosAsync(userId, toDoCategoryId, newToDoCategoryId)).Returns(Task.CompletedTask);

            await _toDoCategoryService.DeleteToDoCategoryAsync(toDoCategory.ToDoCategoryId);

            _toDoCategoryRepositoryMock.Verify(repo => repo.DeleteToDoCategoryAsync(toDoCategoryId), Times.Once);
            _toDoCategoryRepositoryMock.Verify(repo => repo.UpdateCategoryInToDosAsync(userId, toDoCategoryId, newToDoCategoryId), Times.Once);
        }

        [Theory]
        [InlineData("Habbit")]
        [InlineData("Other")]
        public async Task DeleteToDoCategoryAsync_ShouldThrowException_WhenCategoryIsDefault(string toDoCategoryName)
        {
            var userId = Guid.NewGuid();
            var existingToDoCategory = new ToDoCategory(userId, toDoCategoryName);
            var toDoCategoryId = existingToDoCategory.ToDoCategoryId;

            _toDoCategoryRepositoryMock.Setup(repo => repo.GetToDoCategoryByCategoryIdAsync(toDoCategoryId)).ReturnsAsync(existingToDoCategory);
            _toDoCategoryRepositoryMock.Setup(repo => repo.DeleteToDoCategoryAsync(toDoCategoryId)).Returns(Task.CompletedTask);

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _toDoCategoryService.DeleteToDoCategoryAsync(toDoCategoryId));

            Assert.Equal("You cannot delete this category.", exception.Message);
            _toDoCategoryRepositoryMock.Verify(repo => repo.DeleteToDoCategoryAsync(toDoCategoryId), Times.Never());
        }

        #endregion
    }
}
