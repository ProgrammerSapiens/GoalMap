using AutoMapper;
using Core.DTOs.ToDoCategory;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controller for managing ToDo categories.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoCategoriesController : ControllerBase
    {
        private readonly IToDoCategoryService _service;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoCategoriesController"/> class.
        /// </summary>
        /// <param name="toDoCategoryService">Service for ToDo category operations.</param>
        /// <param name="mapper">Mapper for mapping models to DTOs.</param>
        public ToDoCategoriesController(IToDoCategoryService toDoCategoryService, IMapper mapper)
        {
            _service = toDoCategoryService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets a ToDo category by its ID.
        /// </summary>
        /// <param name="toDoCategoryId">The unique identifier of the ToDo category.</param>
        /// <returns>The requested ToDo category.</returns>
        [HttpGet("{toDoCategoryId}")]
        public async Task<ActionResult<CategoryDto>> GetToDoCategoryByCategoryId(Guid toDoCategoryId)
        {
            if (Guid.Empty == toDoCategoryId) return BadRequest("Todo category id cannot be empty");

            var toDoCategory = await _service.GetToDoCategoryByCategoryIdAsync(toDoCategoryId);
            if (toDoCategory == null) return NotFound("Category was not found.");

            return _mapper.Map<CategoryDto>(toDoCategory);
        }

        /// <summary>
        /// Gets all ToDo categories for the authenticated user.
        /// </summary>
        /// <returns>A list of ToDo categories.</returns>
        [HttpGet("user")]
        public async Task<ActionResult<List<CategoryDto>>> GetToDoCategoriesByUserId()
        {
            var userId = GetUserId();
            if (Guid.Empty == userId) return Unauthorized("User ID is not authenticated or invalid.");

            var categories = await _service.GetToDoCategoriesByUserIdAsync(userId);
            if (categories.Count == 0) return new List<CategoryDto>();

            return _mapper.Map<List<CategoryDto>>(categories);
        }

        /// <summary>
        /// Adds a new ToDo category.
        /// </summary>
        /// <param name="categoryAddOrUpdateDto">The category data to be added.</param>
        /// <returns>The created category.</returns>
        [HttpPost]
        public async Task<IActionResult> AddToDoCategory([FromBody] CategoryAddOrUpdateDto? categoryAddOrUpdateDto)
        {
            var userId = GetUserId();
            if (Guid.Empty == userId) return Unauthorized("User ID is not authenticated or invalid.");

            if (categoryAddOrUpdateDto == null) return BadRequest("Category data cannot be null.");

            categoryAddOrUpdateDto.UserId = userId;
            var category = _mapper.Map<ToDoCategory>(categoryAddOrUpdateDto);
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
        public async Task<IActionResult> UpdateToDoCategory([FromBody] CategoryAddOrUpdateDto? categoryAddOrUpdateDto)
        {
            if (categoryAddOrUpdateDto == null) return BadRequest("Category data cannot be null.");

            if (Guid.Empty == categoryAddOrUpdateDto.ToDoCategoryId || string.IsNullOrEmpty(categoryAddOrUpdateDto.ToDoCategoryName))
            {
                return BadRequest("Category name cannot be empty.");
            }

            var existingCategory = await _service.GetToDoCategoryByCategoryIdAsync(categoryAddOrUpdateDto.ToDoCategoryId);
            if (existingCategory == null) return NotFound("Category was not found.");

            _mapper.Map(categoryAddOrUpdateDto, existingCategory);
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
            if (Guid.Empty == toDoCategoryId) return BadRequest("ToDo category id cannot be empty.");

            await _service.DeleteToDoCategoryAsync(toDoCategoryId);

            return NoContent();
        }

        /// <summary>
        /// Extracts the authenticated user's ID.
        /// </summary>
        private Guid GetUserId()
        {
            return Guid.TryParse(User.Identity?.Name, out var parsedUserId) ? parsedUserId : new Guid();
        }
    }
}
