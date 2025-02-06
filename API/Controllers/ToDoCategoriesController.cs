using AutoMapper;
using Core.DTOs.ToDoCategory;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoCategoriesController : ControllerBase
    {
        private readonly IToDoCategoryService _service;
        private readonly IMapper _mapper;

        public ToDoCategoriesController(IToDoCategoryService toDoCategoryService, IMapper mapper)
        {
            _service = toDoCategoryService;
            _mapper = mapper;
        }

        [HttpGet("{toDoCategoryId}")]
        public async Task<ActionResult<CategoryDto>> GetToDoCategoryByCategoryId(Guid toDoCategoryId)
        {
            var userId = User.Identity?.Name;

            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
            {
                return Unauthorized("User ID is not authenticated or invalid.");
            }
            if (Guid.Empty == toDoCategoryId)
            {
                return BadRequest("Todo category id cannot be empty");
            }

            try
            {
                var toDoCategory = await _service.GetToDoCategoryByCategoryIdAsync(parsedUserId, toDoCategoryId);

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

        [HttpPut]
        public async Task<IActionResult> UpdateToDoCategory([FromBody] CategoryAddOrUpdateDto? categoryAddOrUpdateDto)
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

            var categoryName = categoryAddOrUpdateDto.ToDoCategoryName;
            var categoryId = categoryAddOrUpdateDto.ToDoCategoryId;

            if (Guid.Empty == categoryId || string.IsNullOrEmpty(categoryName))
            {
                return BadRequest("Category name cannot be empty.");
            }

            try
            {
                var existingCategory = await _service.GetToDoCategoryByCategoryIdAsync(parsedUserId, categoryId);
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

        [HttpDelete("{toDoCategoryId}")]
        public async Task<IActionResult> DeleteToDoCategory(Guid toDoCategoryId)
        {
            var userId = User.Identity?.Name;

            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
            {
                return Unauthorized("User ID is not authenticated or invalid.");
            }

            if (Guid.Empty == toDoCategoryId)
            {
                return BadRequest("ToDo category id cannot be empty.");
            }

            try
            {
                await _service.DeleteToDoCategoryAsync(parsedUserId, toDoCategoryId);
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
