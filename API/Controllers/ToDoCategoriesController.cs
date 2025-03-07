using AutoMapper;
using Core.DTOs.ToDoCategory;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controller for managing ToDo categories.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ToDoCategoriesController : ControllerBase
    {
        private readonly IToDoCategoryService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<ToDoCategoriesController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoCategoriesController"/> class.
        /// </summary>
        /// <param name="toDoCategoryService">Service for ToDo category operations.</param>
        /// <param name="mapper">Mapper for mapping models to DTOs.</param>
        public ToDoCategoriesController(IToDoCategoryService toDoCategoryService, IMapper mapper, ILogger<ToDoCategoriesController> logger)
        {
            _service = toDoCategoryService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Gets a ToDo category by its ID.
        /// </summary>
        /// <param name="toDoCategoryId">The unique identifier of the ToDo category.</param>
        /// <returns>The requested ToDo category.</returns>
        [HttpGet("{toDoCategoryId}")]
        public async Task<ActionResult<CategoryDto>> GetToDoCategoryByCategoryId(Guid toDoCategoryId)
        {
            _logger.LogInformation("GetToDoCategoryByCategoryId");

            if (Guid.Empty == toDoCategoryId)
            {
                _logger.LogWarning("Todo category id is invalid.");
                return BadRequest("Todo category id cannot be empty");
            }

            var toDoCategory = await _service.GetToDoCategoryByCategoryIdAsync(toDoCategoryId);
            if (toDoCategory == null)
            {
                _logger.LogError($"Category with id {toDoCategoryId} was not found.");
                return NotFound("Category was not found.");
            }

            return _mapper.Map<CategoryDto>(toDoCategory);
        }

        /// <summary>
        /// Gets all ToDo categories for the authenticated user.
        /// </summary>
        /// <returns>A list of ToDo categories.</returns>
        [HttpGet("user")]
        public async Task<ActionResult<List<CategoryDto>>> GetToDoCategoriesByUserId()
        {
            _logger.LogInformation("GetToDoCategoriesByUserId");

            var userId = GetUserId();
            if (Guid.Empty == userId)
            {
                _logger.LogWarning("User ID is invalid.");
                return Unauthorized("User ID is not authenticated or invalid.");
            }

            var categories = await _service.GetToDoCategoriesByUserIdAsync(userId);
            if (categories.Count == 0)
            {
                _logger.LogWarning("This user doesn't have any categories.");
                return new List<CategoryDto>();
            }

            return _mapper.Map<List<CategoryDto>>(categories);
        }

        /// <summary>
        /// Adds a new ToDo category.
        /// </summary>
        /// <param name="categoryAddDto">The category data to be added.</param>
        /// <returns>The created category.</returns>
        [HttpPost]
        public async Task<IActionResult> AddToDoCategory([FromBody] CategoryAddDto? categoryAddDto)
        {
            _logger.LogInformation("AddToDoCategory");

            var userId = GetUserId();
            if (Guid.Empty == userId)
            {
                _logger.LogWarning("User ID is invalid.");
                return Unauthorized("User ID is not authenticated or invalid.");
            }

            if (categoryAddDto == null || string.IsNullOrEmpty(categoryAddDto.ToDoCategoryName))
            {
                _logger.LogWarning("The entered data is null.");
                return BadRequest("Category data cannot be null.");
            }

            var category = new ToDoCategory(userId, categoryAddDto.ToDoCategoryName);
            await _service.AddToDoCategoryAsync(category);

            var categoryDto = _mapper.Map<CategoryDto>(category);

            return CreatedAtAction(nameof(GetToDoCategoryByCategoryId), new { toDoCategoryId = category.ToDoCategoryId }, categoryDto);
        }

        /// <summary>
        /// Updates an existing ToDo category.
        /// </summary>
        /// <param name="categoryAddOrUpdateDto">The updated category data.</param>
        /// <returns>No content if the update was successful.</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateToDoCategory([FromBody] CategoryUpdateDto? categoryUpdateDto)
        {
            _logger.LogInformation("UpdateToDoCategory");

            if (categoryUpdateDto == null)
            {
                _logger.LogWarning("The entered data is null");
                return BadRequest("Category data cannot be null.");
            }

            if (string.IsNullOrEmpty(categoryUpdateDto.ToDoCategoryName))
            {
                _logger.LogWarning("The entered data is invalid.");
                return BadRequest("Category name cannot be empty.");
            }

            var existingCategory = await _service.GetToDoCategoryByCategoryIdAsync(categoryUpdateDto.ToDoCategoryId);
            if (existingCategory == null)
            {
                _logger.LogError($"Category with id {categoryUpdateDto.ToDoCategoryId} was not found.");
                return NotFound("Category was not found.");
            }
            if (existingCategory.ToDoCategoryName == "Other" || existingCategory.ToDoCategoryName == "Habbit")
            {
                _logger.LogInformation($"You cannot update this category");
                return NoContent();
            }

            if (existingCategory.ToDoCategoryName == categoryUpdateDto.ToDoCategoryName)
                return NoContent();

            _mapper.Map(categoryUpdateDto, existingCategory);
            await _service.UpdateToDoCategoryAsync(existingCategory);

            return NoContent();
        }

        /// <summary>
        /// Deletes a ToDo category by its ID.
        /// </summary>
        /// <param name="toDoCategoryId">The unique identifier of the category to be deleted.</param>
        /// <returns>No content if the deletion was successful.</returns>
        [HttpDelete("{toDoCategoryId}")]
        public async Task<IActionResult> DeleteToDoCategory(Guid toDoCategoryId)
        {
            _logger.LogInformation("DeleteToDoCategory");

            if (Guid.Empty == toDoCategoryId)
            {
                _logger.LogWarning("ToDo id is invalid.");
                return BadRequest("ToDo category id cannot be empty.");
            }

            await _service.DeleteToDoCategoryAsync(toDoCategoryId);

            return NoContent();
        }

        /// <summary>
        /// Extracts the authenticated user's ID.
        /// </summary>
        private Guid GetUserId()
        {
            var userIdClaim = User.FindFirst("UserId");
            _logger.LogInformation($"userIdClaim {userIdClaim?.Value}");
            return userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId) ? userId : Guid.Empty;
        }
    }
}
