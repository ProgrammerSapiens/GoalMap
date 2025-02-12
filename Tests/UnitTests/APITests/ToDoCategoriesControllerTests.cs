using API.Controllers;
using AutoMapper;
using Core.DTOs.ToDoCategory;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace Tests.UnitTests.APITests
{
    public class ToDoCategoriesControllerTests
    {
        private readonly Mock<IToDoCategoryService> _mockService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<ToDoCategoriesController>> _loggerMock;
        private readonly ToDoCategoriesController _categoriesController;

        public ToDoCategoriesControllerTests()
        {
            _mockService = new Mock<IToDoCategoryService>();
            _mockMapper = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<ToDoCategoriesController>>();
            _categoriesController = new ToDoCategoriesController(_mockService.Object, _mockMapper.Object, _loggerMock.Object);
        }

        #region GetToDoCategoryByCategoryId(Guid toDoCategoryId)

        [Fact]
        public async Task GetToDoCategoryByCategoryId_ShouldReturnCategory_WhenCategoryExists()
        {
            var user = new User("TestUser");

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, user.UserId.ToString())
            ]));

            _categoriesController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userClaims }
            };

            var expectedCategoryName = "ExpectedCategory";
            var expectedCategory = new ToDoCategory(user.UserId, expectedCategoryName);
            var toDoCategoryId = expectedCategory.ToDoCategoryId;

            var categoryDto = new CategoryDto { ToDoCategoryId = toDoCategoryId, ToDoCategoryName = expectedCategoryName };

            _mockService.Setup(service => service.GetToDoCategoryByCategoryIdAsync(toDoCategoryId)).ReturnsAsync(expectedCategory);
            _mockMapper.Setup(mapper => mapper.Map<CategoryDto>(expectedCategory)).Returns(categoryDto);

            var result = await _categoriesController.GetToDoCategoryByCategoryId(toDoCategoryId);

            var actionResult = Assert.IsType<ActionResult<CategoryDto>>(result);
            var returnedCategoryDto = Assert.IsType<CategoryDto>(actionResult.Value);
            Assert.Equal(expectedCategory.ToDoCategoryId, returnedCategoryDto.ToDoCategoryId);
            Assert.Equal(expectedCategory.ToDoCategoryName, returnedCategoryDto.ToDoCategoryName);
        }

        [Fact]
        public async Task GetToDoCategoryByCategoryId_ShouldReturnBadRequest_WhenUserIdIsEmpty()
        {
            var user = new User("TestUser");

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, user.UserId.ToString())
            ]));

            _categoriesController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userClaims }
            };

            var result = await _categoriesController.GetToDoCategoryByCategoryId(new Guid());

            var actionResult = Assert.IsType<ActionResult<CategoryDto>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Todo category id cannot be empty", badRequestResult.Value);
        }

        [Fact]
        public async Task GetToDoCategoryByCategoryId_ShouldReturnNotFound_WhenCategoryDoesNotExist()
        {
            var user = new User("TestUser");

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, user.UserId.ToString())
            ]));

            _categoriesController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userClaims }
            };

            var expectedCategoryName = "ExpectedCategory";
            var expectedCategory = new ToDoCategory(user.UserId, expectedCategoryName);
            var toDoCategoryId = expectedCategory.ToDoCategoryId;

            _mockService.Setup(service => service.GetToDoCategoryByCategoryIdAsync(toDoCategoryId)).ReturnsAsync((ToDoCategory?)null);

            var result = await _categoriesController.GetToDoCategoryByCategoryId(toDoCategoryId);

            var actionResult = Assert.IsType<ActionResult<CategoryDto>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Category was not found.", notFoundResult.Value);
        }

        #endregion

        #region GetToDoCategoriesByUserId()

        [Fact]
        public async Task GetToDoCategoriesByUserId_ShouldReturnCategories_WhenUserHasCategories()
        {
            var user = new User("TestUser");

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, user.UserId.ToString())
            ]));

            _categoriesController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userClaims }
            };

            var expectedCategories = new List<ToDoCategory>
            {
                new ToDoCategory (user.UserId, "Work"),
                new ToDoCategory (user.UserId, "Personal")
            };

            var categoriesDto = new List<CategoryDto>
            {
                new CategoryDto { ToDoCategoryId = expectedCategories[0].ToDoCategoryId, ToDoCategoryName = "Work" },
                new CategoryDto { ToDoCategoryId = expectedCategories[1].ToDoCategoryId, ToDoCategoryName = "Personal" }
            };

            _mockService.Setup(service => service.GetToDoCategoriesByUserIdAsync(user.UserId)).ReturnsAsync(expectedCategories);
            _mockMapper.Setup(mapper => mapper.Map<List<CategoryDto>>(expectedCategories)).Returns(categoriesDto);

            var result = await _categoriesController.GetToDoCategoriesByUserId();

            var actionResult = Assert.IsType<ActionResult<List<CategoryDto>>>(result);
            var returnedCategoriesDto = Assert.IsType<List<CategoryDto>>(actionResult.Value);
            Assert.Equal(2, returnedCategoriesDto.Count);
        }

        [Fact]
        public async Task GetToDoCategoriesByUserId_ShouldReturnUnauthorized_WhenUserIsNotAuthenticated()
        {
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity());

            _categoriesController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userClaims }
            };

            var result = await _categoriesController.GetToDoCategoriesByUserId();

            var actionResult = Assert.IsType<ActionResult<List<CategoryDto>>>(result);
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(actionResult.Result);
            Assert.Equal("User ID is not authenticated or invalid.", unauthorizedResult.Value);
        }

        [Fact]
        public async Task GetToDoCategoriesByUserId_ShouldReturnEmptyList_WhenNoCategoriesExist()
        {
            var user = new User("TestUser");

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, user.UserId.ToString())
            ]));

            _categoriesController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userClaims }
            };

            _mockService.Setup(service => service.GetToDoCategoriesByUserIdAsync(user.UserId)).ReturnsAsync(new List<ToDoCategory>());

            var result = await _categoriesController.GetToDoCategoriesByUserId();

            var actionResult = Assert.IsType<ActionResult<List<CategoryDto>>>(result);
            var returnedCategoriesDto = Assert.IsType<List<CategoryDto>>(actionResult.Value);
            Assert.Empty(returnedCategoriesDto);
        }

        #endregion

        #region AddToDoCategory([FromBody] CategoryAddOrUpdateDto categoryAddOrUpdateDto)

        [Fact]
        public async Task AddToDoCategory_ShouldReturnCreatedAtAction_WhenCategoryIsValid()
        {
            var user = new User("TestUser");

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, user.UserId.ToString())
            ]));

            _categoriesController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userClaims }
            };

            var categoryAddOrUpdateDto = new CategoryAddOrUpdateDto { ToDoCategoryName = "TestCategory" };
            var newCategory = new ToDoCategory(user.UserId, categoryAddOrUpdateDto.ToDoCategoryName);
            var categoryDto = new CategoryDto { ToDoCategoryId = newCategory.ToDoCategoryId, ToDoCategoryName = newCategory.ToDoCategoryName };

            _mockMapper.Setup(mapper => mapper.Map<ToDoCategory>(categoryAddOrUpdateDto)).Returns(newCategory);
            _mockService.Setup(service => service.AddToDoCategoryAsync(newCategory)).Returns(Task.CompletedTask);
            _mockMapper.Setup(mapper => mapper.Map<CategoryDto>(newCategory)).Returns(categoryDto);

            var result = await _categoriesController.AddToDoCategory(categoryAddOrUpdateDto);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedCategoryDto = Assert.IsType<CategoryDto>(createdAtActionResult.Value);

            Assert.Equal(nameof(_categoriesController.GetToDoCategoryByCategoryId), createdAtActionResult.ActionName);
            Assert.Equal(newCategory.ToDoCategoryId, returnedCategoryDto.ToDoCategoryId);
            Assert.Equal(newCategory.ToDoCategoryName, returnedCategoryDto.ToDoCategoryName);
        }

        [Fact]
        public async Task AddToDoCategory_ShouldReturnUnauthorized_WhenUserIsNotAuthenticated()
        {
            var user = new User("TestUser");

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity());

            _categoriesController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userClaims }
            };

            var categoryAddOrUpdateDto = new CategoryAddOrUpdateDto { ToDoCategoryName = "TestCategory" };

            var result = await _categoriesController.AddToDoCategory(categoryAddOrUpdateDto);

            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("User ID is not authenticated or invalid.", unauthorizedResult.Value);
        }

        [Fact]
        public async Task AddToDoCategory_ShouldReturnBadRequest_WhenDataIsEmpty()
        {
            var user = new User("TestUser");

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, user.UserId.ToString())
            ]));

            _categoriesController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userClaims }
            };

            CategoryAddOrUpdateDto? categoryAddOrUpdateDto = null;

            var result = await _categoriesController.AddToDoCategory(categoryAddOrUpdateDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Category data cannot be null.", badRequestResult.Value);
        }

        #endregion

        #region UpdateToDoCategory([FromBody] CategoryAddOrUpdateDto categoryAddOrUpdateDto)

        [Fact]
        public async Task UpdateToDoCategory_ShouldReturnNoContent_WhenCategoryIsValid()
        {
            var user = new User("TestUser");

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, user.UserId.ToString())
            ]));

            _categoriesController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userClaims }
            };

            var categoryName = "TestCategory";
            var existingCategory = new ToDoCategory(user.UserId, categoryName);
            var updatedCategory = new ToDoCategory(user.UserId, "NewName");
            var categoryAddOrUpdateDto = new CategoryAddOrUpdateDto { UserId = user.UserId, ToDoCategoryId = existingCategory.ToDoCategoryId, ToDoCategoryName = "NewName" };

            _mockService.Setup(service => service.GetToDoCategoryByCategoryIdAsync(existingCategory.ToDoCategoryId)).ReturnsAsync(existingCategory);
            _mockMapper.Setup(mapper => mapper.Map(categoryAddOrUpdateDto, existingCategory)).Returns(updatedCategory);
            _mockService.Setup(service => service.UpdateToDoCategoryAsync(updatedCategory)).Returns(Task.CompletedTask);

            var result = await _categoriesController.UpdateToDoCategory(categoryAddOrUpdateDto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task AddToDoCategory_ShouldReturnBadRequest_WhenDataIsNull()
        {
            var user = new User("TestUser");

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, user.UserId.ToString())
            ]));

            _categoriesController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userClaims }
            };

            CategoryAddOrUpdateDto? categoryAddOrUpdateDto = null;

            var result = await _categoriesController.UpdateToDoCategory(categoryAddOrUpdateDto);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Category data cannot be null.", badRequestResult.Value);
        }

        [Fact]
        public async Task AddToDoCategory_ShouldReturnNotFound_WhenCategoryDoesNotExist()
        {
            var user = new User("TestUser");

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, user.UserId.ToString())
            ]));

            _categoriesController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userClaims }
            };

            var categoryName = "TestCategory";
            var existingCategory = new ToDoCategory(user.UserId, categoryName);
            var categoryAddOrUpdateDto = new CategoryAddOrUpdateDto { UserId = user.UserId, ToDoCategoryId = existingCategory.ToDoCategoryId, ToDoCategoryName = "NewName" };

            _mockService.Setup(service => service.GetToDoCategoryByCategoryIdAsync(existingCategory.ToDoCategoryId)).ReturnsAsync((ToDoCategory?)null);

            var result = await _categoriesController.UpdateToDoCategory(categoryAddOrUpdateDto);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Category was not found.", notFoundResult.Value);
        }

        #endregion

        #region DeleteToDoCategory(Guid toDoCategoryId)

        [Fact]
        public async Task DeleteToDoCategory_ShouldReturnNoContent_WhenCategoryIsDeleted()
        {
            var user = new User("TestUser");

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, user.UserId.ToString())
            ]));

            _categoriesController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userClaims }
            };

            var categoryName = "CategoryToDelete";
            var category = new ToDoCategory(user.UserId, categoryName);

            _mockService.Setup(service => service.DeleteToDoCategoryAsync(category.ToDoCategoryId)).Returns(Task.CompletedTask);

            var result = await _categoriesController.DeleteToDoCategory(category.ToDoCategoryId);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteToDoCategory_ShouldReturnBadRequest_WhenUserIdIsEmpty()
        {
            var user = new User("TestUser");

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, user.UserId.ToString())
            ]));

            _categoriesController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userClaims }
            };

            var result = await _categoriesController.DeleteToDoCategory(new Guid());

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("ToDo category id cannot be empty.", badRequest.Value);
        }

        #endregion
    }
}
