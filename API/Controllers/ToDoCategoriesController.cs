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

        public ToDoCategoriesController(IToDoCategoryService toDoCategoryService)
        {
            _service = toDoCategoryService;
        }

        [HttpGet("{toDoCategoryId}")]
        public async Task<ActionResult<ToDoCategory>> GetToDoCategoryByCategoryId(Guid toDoCategoryId)
        {
            var userId = User.Identity?.Name;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User id not authenticated.");
            }
            if (Guid.Empty == toDoCategoryId)
            {
                return BadRequest("User id or category name cannot be empty");
            }

            try
            {
                var toDoCategory = await _service.GetToDoCategoryByCategoryIdAsync(Guid.Parse(userId), toDoCategoryId);

                if (toDoCategory == null)
                {
                    return NotFound("Category not found.");
                }
                return toDoCategory;
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("user")]
        public async Task<ActionResult<List<ToDoCategory>>> GetToDoCategoriesByUserId()
        {
            var userId = User.Identity?.Name;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            try
            {
                var categories = await _service.GetToDoCategoriesByUserIdAsync(Guid.Parse(userId));

                if (categories.Count == 0)
                {
                    return new List<ToDoCategory>();
                }

                return categories;
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToDoCategory([FromBody] ToDoCategory toDoCategory)
        {
            if (toDoCategory == null)
            {
                return BadRequest("Category data cannot be null.");
            }

            try
            {
                await _service.AddToDoCategoryAsync(toDoCategory);

                return CreatedAtAction(nameof(GetToDoCategoryByCategoryId), new { categoryId = toDoCategory.ToDoCategoryId }, toDoCategory);
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
        public async Task<IActionResult> UpdateToDoCategory([FromBody] ToDoCategory toDoCategory)
        {
            if (toDoCategory == null)
            {
                return BadRequest("Category data cannot be null.");
            }

            try
            {
                await _service.UpdateToDoCategoryAsync(toDoCategory);
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

        [HttpDelete]
        public async Task<IActionResult> DeleteToDoCategory(Guid toDoCategoryId)
        {
            var userId = User.Identity?.Name;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }
            if (Guid.Empty == toDoCategoryId)
            {
                return BadRequest("ToDo category id cannot be empty.");
            }

            try
            {
                await _service.DeleteToDoCategoryAsync(Guid.Parse(userId), toDoCategoryId);
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
