using API.Controllers;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.UnitTests.APITests
{
    public class ToDoCategoriesControllerTests
    {
        private readonly Mock<IToDoCategoryService> _mockService;
        private readonly ToDoCategoriesController _controller;

        public ToDoCategoriesControllerTests()
        {
            _mockService = new Mock<IToDoCategoryService>();
            _controller = new ToDoCategoriesController(_mockService.Object);
        }

        #region GetToDoCategoryByCategoryName(Guid userId, string categoryName)

        [Fact]
        public async Task GetToDoCategoryByCategoryName_ReturnsCategory_WhenCategoryExists()
        {
            var userId = Guid.NewGuid();
            var toDoCategoryName = "TestCategory";
            var expectedCategory = new ToDoCategory(userId, toDoCategoryName);

            _mockService.Setup(service => service.GetToDoCategoryByCategoryNameAsync(userId, toDoCategoryName)).ReturnsAsync(expectedCategory);

            var result = await _controller.GetToDoCategoryByCategoryName(toDoCategoryName);

            var actionResult = Assert.IsType<ActionResult<ToDoCategory>>(result);
            Assert.Equal(expectedCategory, actionResult.Value);
        }

        #endregion

        #region GetToDoCategoriesByUserId(Guid userId)

        [Fact]
        public async Task GetToDoCategoriesByUserId_ReturnsCategories_WhenUserHasCategories()
        {
            var userId = Guid.NewGuid();
            var expectedCategories = new List<ToDoCategory>
            {
                new ToDoCategory (userId, "Work"),
                new ToDoCategory (userId, "Personal")
            };

            _mockService.Setup(service => service.GetToDoCategoriesByUserIdAsync(userId)).ReturnsAsync(expectedCategories);

            var result = await _controller.GetToDoCategoriesByUserId();

            var actionResult = Assert.IsType<ActionResult<List<ToDoCategory>>>(result);
            Assert.Equal(expectedCategories, actionResult.Value);
        }

        #endregion

        #region AddToDoCategory([FromBody] ToDoCategory toDoCategory)

        [Fact]
        public async Task AddToDoCategory_ReturnsCreatedAtAction_WhenCategoryIsValid()
        {
            var newCategory = new ToDoCategory(Guid.NewGuid(), "Category");

            _mockService.Setup(service => service.AddToDoCategoryAsync(newCategory)).Returns(Task.CompletedTask);

            var result = await _controller.AddToDoCategory(newCategory);

            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(ToDoCategoriesController.GetToDoCategoryByCategoryName), actionResult.ActionName);
            Assert.Equal(newCategory, actionResult.Value);
        }

        #endregion

        #region UpdateToDoCategory([FromBody] ToDoCategory toDoCategory)

        [Fact]
        public async Task UpdateToDoCategory_ReturnsNoContent_WhenCategoryIsValid()
        {
            var updatedCategory = new ToDoCategory(Guid.NewGuid(), "SomeCategory");

            _mockService.Setup(service => service.UpdateToDoCategoryAsync(updatedCategory)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateToDoCategory(updatedCategory);

            Assert.IsType<NoContentResult>(result);
        }

        #endregion

        #region DeleteToDoCategory(Guid userId, string categoryName)

        [Fact]
        public async Task DeleteToDoCategory_ReturnsNoContent_WhenCategoryIsDeleted()
        {
            var userId = Guid.NewGuid();
            var categoryName = "CategoryToDelete";

            _mockService.Setup(service => service.DeleteToDoCategoryAsync(userId, categoryName)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteToDoCategory(categoryName);

            Assert.IsType<NoContentResult>(result);
        }

        #endregion
    }
}
