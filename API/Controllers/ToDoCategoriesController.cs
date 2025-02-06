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
            if (Guid.Empty == toDoCategoryId)
            {
                return BadRequest("Todo category id cannot be empty");
            }

            try
            {
                var toDoCategory = await _service.GetToDoCategoryByCategoryIdAsync(toDoCategoryId);

                if (toDoCategory == null)
                {
                    return NotFound("Category was not found.");
                }

                var categoryDto = _mapper.Map<CategoryDto>(toDoCategory);

                return categoryDto;
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Gets all ToDo categories for the authenticated user.
        /// </summary>
        /// <returns>A list of ToDo categories.</returns>
        [HttpGet("user")]
        public async Task<ActionResult<List<CategoryDto>>> GetToDoCategoriesByUserId()
        {
            var userId = User.Identity?.Name;

            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
            {
                return Unauthorized("User ID is not authenticated or invalid.");
            }

            try
            {
                var categories = await _service.GetToDoCategoriesByUserIdAsync(parsedUserId);

                if (categories.Count == 0)
                {
                    return new List<CategoryDto>();
                }

                var categoriesDto = _mapper.Map<List<CategoryDto>>(categories);

                return categoriesDto;
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Adds a new ToDo category.
        /// </summary>
        /// <param name="categoryAddOrUpdateDto">The category data to be added.</param>
        /// <returns>The created category.</returns>
        [HttpPost]
        public async Task<IActionResult> AddToDoCategory([FromBody] CategoryAddOrUpdateDto? categoryAddOrUpdateDto)
        {
            var userId = User.Identity?.Name;

            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
            {
                return Unauthorized("User ID is not authenticated or invalid.");
            }

            if (categoryAddOrUpdateDto == null)
            {
                return BadRequest("Category data cannot be null.");
            }

            try
            {
                categoryAddOrUpdateDto.UserId = parsedUserId;
                var category = _mapper.Map<ToDoCategory>(categoryAddOrUpdateDto);
                await _service.AddToDoCategoryAsync(category);

                var categoryDto = _mapper.Map<CategoryDto>(category);
                return CreatedAtAction(nameof(GetToDoCategoryByCategoryId), new { toDoCategoryId = category.ToDoCategoryId }, categoryDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Updates an existing ToDo category.
        /// </summary>
        /// <param name="categoryAddOrUpdateDto">The updated category data.</param>
        /// <returns>No content if the update was successful.</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateToDoCategory([FromBody] CategoryAddOrUpdateDto? categoryAddOrUpdateDto)
        {
            if (categoryAddOrUpdateDto == null)
            {
                return BadRequest("Category data cannot be null.");
            }

            var categoryName = categoryAddOrUpdateDto.ToDoCategoryName;
            var categoryId = categoryAddOrUpdateDto.ToDoCategoryId;

            if (Guid.Empty == categoryId || string.IsNullOrEmpty(categoryName))
            {
                return BadRequest("Category name cannot be empty.");
            }

            try
            {
                var existingCategory = await _service.GetToDoCategoryByCategoryIdAsync(categoryId);
                if (existingCategory == null)
                {
                    return NotFound("Category was not found.");
                }

                _mapper.Map(categoryAddOrUpdateDto, existingCategory);

                await _service.UpdateToDoCategoryAsync(existingCategory);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server error.");
            }
        }

        /// <summary>
        /// Deletes a ToDo category by its ID.
        /// </summary>
        /// <param name="toDoCategoryId">The unique identifier of the category to be deleted.</param>
        /// <returns>No content if the deletion was successful.</returns>
        [HttpDelete("{toDoCategoryId}")]
        public async Task<IActionResult> DeleteToDoCategory(Guid toDoCategoryId)
        {
            if (Guid.Empty == toDoCategoryId)
            {
                return BadRequest("ToDo category id cannot be empty.");
            }

            try
            {
                await _service.DeleteToDoCategoryAsync(toDoCategoryId);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
