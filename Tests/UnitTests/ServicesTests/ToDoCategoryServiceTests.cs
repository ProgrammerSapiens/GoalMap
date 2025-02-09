﻿using Core.Services;
using Core.Models;
using Moq;
using Core.Interfaces;

namespace Tests.UnitTests.ServicesTests
{
    public class ToDoCategoryServiceTests
    {
        private readonly Mock<IToDoCategoryRepository> toDoCategoryRepositoryMock;
        private readonly IToDoCategoryService toDoCategoryService;

        public ToDoCategoryServiceTests()
        {
            toDoCategoryRepositoryMock = new Mock<IToDoCategoryRepository>();

            toDoCategoryService = new ToDoCategoryService(toDoCategoryRepositoryMock.Object);
        }

        #region GetToDoCategoryByCategoryIdAsync(Guid userId, Guid toDoCategoryId) tests

        [Fact]
        public async Task GetToDoCategoryByCategoryIdAsync_ShouldReturnCategory_WhenCategoryNameExists()
        {
            var toDoCategoryName = "Test Category";
            var userId = Guid.NewGuid();
            var expectedCategory = new ToDoCategory(userId, toDoCategoryName)
            {
                User = new User("TestUser")
            };
            var toDoCategoryId = expectedCategory.ToDoCategoryId;

            toDoCategoryRepositoryMock.Setup(repo => repo.GetToDoCategoryByCategoryIdAsync(toDoCategoryId)).ReturnsAsync(expectedCategory);

            var result = await toDoCategoryService.GetToDoCategoryByCategoryIdAsync(toDoCategoryId);

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
        public async Task GetToDoCategoryByCategoryIdAsync_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var toDoCategoryId = Guid.NewGuid();

            toDoCategoryRepositoryMock.Setup(repo => repo.GetToDoCategoryByCategoryIdAsync(toDoCategoryId)).ReturnsAsync((ToDoCategory?)null);

            var result = await toDoCategoryService.GetToDoCategoryByCategoryIdAsync(toDoCategoryId);
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

            toDoCategoryRepositoryMock.Setup(repo => repo.GetToDoCategoriesByUserIdAsync(userId)).ReturnsAsync(expectedCategories);

            var result = await toDoCategoryService.GetToDoCategoriesByUserIdAsync(userId);

            Assert.NotNull(result);
            Assert.Equal(expectedCategories.Count, result.Count);
            Assert.All(result, category => Assert.Contains(expectedCategories, expC => expC.ToDoCategoryName == category.ToDoCategoryName));
        }

        [Fact]
        public async Task GetToDoCategoriesByUserIdAsync_ShouldReturnEmptyList_WhenNoCategoriesExist()
        {
            var userId = Guid.NewGuid();

            toDoCategoryRepositoryMock.Setup(repo => repo.GetToDoCategoriesByUserIdAsync(userId)).ReturnsAsync(new List<ToDoCategory>());

            var result = await toDoCategoryService.GetToDoCategoriesByUserIdAsync(userId);

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

            toDoCategoryRepositoryMock.Setup(repo => repo.AddToDoCategoryAsync(It.IsAny<ToDoCategory>())).Returns(Task.CompletedTask);

            await toDoCategoryService.AddToDoCategoryAsync(toDoCategory);

            toDoCategoryRepositoryMock.Verify(repo => repo.AddToDoCategoryAsync(It.Is<ToDoCategory>(c => c.ToDoCategoryName == toDoCategoryName && c.UserId == userId)), Times.Once);
        }

        [Fact]
        public async Task AddToDoCategoryAsync_ShouldThrowException_WhenCategoryIsAlreadyExist()
        {
            var userId = Guid.NewGuid();
            string toDoCategoryName = "Test category";
            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            toDoCategoryRepositoryMock.Setup(repo => repo.CategoryExistsByNameAsync(userId, toDoCategoryName)).ReturnsAsync(true);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => toDoCategoryService.AddToDoCategoryAsync(toDoCategory));

            toDoCategoryRepositoryMock.Verify(repo => repo.CategoryExistsByNameAsync(userId, toDoCategory.ToDoCategoryName), Times.Once);
            Assert.Equal("Category with such name already exists.", exception.Message);
        }

        [Fact]
        public async Task AddToDoCategoryAsync_ShouldThrowException_WhenCategoryNameContainsDigits()
        {
            var userId = Guid.NewGuid();
            var toDoCategoryName = "Test1 category";

            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            toDoCategoryRepositoryMock.Setup(repo => repo.CategoryExistsByNameAsync(userId, toDoCategoryName)).ReturnsAsync(false);

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => toDoCategoryService.AddToDoCategoryAsync(toDoCategory));

            toDoCategoryRepositoryMock.Verify(repo => repo.CategoryExistsByNameAsync(userId, toDoCategory.ToDoCategoryName), Times.Once);
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
            var newToDoCategory = new ToDoCategory(userId, newToDoCategoryName);
            var toDoCategoryId = newToDoCategory.ToDoCategoryId;

            var oldCategoryNameCope = existingToDoCategory.ToDoCategoryName;

            toDoCategoryRepositoryMock.Setup(repo => repo.GetToDoCategoryByCategoryIdAsync(toDoCategoryId)).ReturnsAsync(existingToDoCategory);
            toDoCategoryRepositoryMock.Setup(repo => repo.CategoryExistsByNameAsync(userId, newToDoCategoryName)).ReturnsAsync(false);
            toDoCategoryRepositoryMock.Setup(repo => repo.UpdateToDoCategoryAsync(It.IsAny<ToDoCategory>())).Returns(Task.CompletedTask);
            toDoCategoryRepositoryMock.Setup(repo => repo.UpdateCategoryInToDosAsync(userId, oldToDoCategoryName, newToDoCategoryName)).Returns(Task.CompletedTask);

            await toDoCategoryService.UpdateToDoCategoryAsync(newToDoCategory);

            toDoCategoryRepositoryMock.Verify(repo => repo.GetToDoCategoryByCategoryIdAsync(toDoCategoryId), Times.Once());
            toDoCategoryRepositoryMock.Verify(repo => repo.CategoryExistsByNameAsync(userId, newToDoCategoryName), Times.Once());
            toDoCategoryRepositoryMock.Verify(repo => repo.UpdateToDoCategoryAsync(It.Is<ToDoCategory>(c => c.UserId == userId && c.ToDoCategoryName == newToDoCategoryName)), Times.Once());
            toDoCategoryRepositoryMock.Verify(repo => repo.UpdateCategoryInToDosAsync(userId, oldToDoCategoryName, newToDoCategoryName), Times.Once());
        }

        [Fact]
        public async Task UpdateToDoCategoryAsync_ShouldThrowException_WhenCategoryDoesNotExist()
        {
            string toDoCategoryName = "Test Category";
            var userId = Guid.NewGuid();
            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            toDoCategoryRepositoryMock.Setup(repo => repo.CategoryExistsByNameAsync(userId, toDoCategoryName)).ReturnsAsync(false);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => toDoCategoryService.UpdateToDoCategoryAsync(toDoCategory));

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

            toDoCategoryRepositoryMock.Setup(repo => repo.GetToDoCategoryByCategoryIdAsync(newToDoCategory.ToDoCategoryId)).ReturnsAsync(existingToDoCategory);

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => toDoCategoryService.UpdateToDoCategoryAsync(newToDoCategory));

            Assert.Equal("You cannot update this category.", exception.Message);
            toDoCategoryRepositoryMock.Verify(repo => repo.UpdateToDoCategoryAsync(newToDoCategory), Times.Never());
        }

        #endregion

        #region DeleteToDoCategoryAsync(Guid toDoCategoryId) tests

        [Fact]
        public async Task DeleteToDoCategoryAsync_ShouldDeleteCategory_WhenCategoryExists()
        {
            var toDoCategoryName = "Test category";
            var userId = Guid.NewGuid();
            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);
            var toDoCategoryId = toDoCategory.ToDoCategoryId;

            toDoCategoryRepositoryMock.Setup(repo => repo.GetToDoCategoryByCategoryIdAsync(toDoCategoryId)).ReturnsAsync(toDoCategory);
            toDoCategoryRepositoryMock.Setup(repo => repo.DeleteToDoCategoryAsync(toDoCategoryId)).Returns(Task.CompletedTask);
            toDoCategoryRepositoryMock.Setup(repo => repo.UpdateCategoryInToDosAsync(userId, toDoCategoryName, "Other")).Returns(Task.CompletedTask);

            await toDoCategoryService.DeleteToDoCategoryAsync(toDoCategory.ToDoCategoryId);

            toDoCategoryRepositoryMock.Verify(repo => repo.DeleteToDoCategoryAsync(toDoCategoryId), Times.Once);
            toDoCategoryRepositoryMock.Verify(repo => repo.UpdateCategoryInToDosAsync(userId, toDoCategoryName, "Other"), Times.Once);
        }

        [Theory]
        [InlineData("Habbit")]
        [InlineData("Other")]
        public async Task DeleteToDoCategoryAsync_ShouldThrowException_WhenCategoryIsDefault(string toDoCategoryName)
        {
            var userId = Guid.NewGuid();
            var existingToDoCategory = new ToDoCategory(userId, toDoCategoryName);
            var toDoCategoryId = existingToDoCategory.ToDoCategoryId;

            toDoCategoryRepositoryMock.Setup(repo => repo.GetToDoCategoryByCategoryIdAsync(toDoCategoryId)).ReturnsAsync(existingToDoCategory);
            toDoCategoryRepositoryMock.Setup(repo => repo.DeleteToDoCategoryAsync(toDoCategoryId)).Returns(Task.CompletedTask);

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => toDoCategoryService.DeleteToDoCategoryAsync(toDoCategoryId));

            Assert.Equal("You cannot delete this category.", exception.Message);
            toDoCategoryRepositoryMock.Verify(repo => repo.DeleteToDoCategoryAsync(toDoCategoryId), Times.Never());
        }

        #endregion
    }
}
