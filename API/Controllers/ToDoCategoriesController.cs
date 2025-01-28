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

        [HttpGet("{categoryName}")]
        public async Task<ActionResult<ToDoCategory>> GetToDoCategoryByCategoryName(Guid userId, string categoryName)
        {
            var toDoCategory = await _service.GetToDoCategoryByCategoryNameAsync(userId, categoryName);

            //TODO: Add returning Not Found

            return toDoCategory;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<ToDoCategory>>> GetToDoCategoriesByUserId(Guid userId)
        {
            var categories = await _service.GetToDoCategoriesByUserIdAsync(userId);

            return categories;
        }

        [HttpPost]
        public async Task<ActionResult<ToDoCategory>> AddToDoCategory([FromBody] ToDoCategory toDoCategory)
        {
            if (toDoCategory == null)
            {
                return BadRequest("Category data cannot be null.");
            }

            await _service.AddToDoCategoryAsync(toDoCategory);

            return CreatedAtAction(nameof(GetToDoCategoryByCategoryName), new { categoryName = toDoCategory.ToDoCategoryName }, toDoCategory);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateToDoCategory([FromBody] ToDoCategory toDoCategory)
        {
            if (toDoCategory == null)
            {
                return BadRequest("Category data cannot be null.");
            }

            await _service.UpdateToDoCategoryAsync(toDoCategory);

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteToDoCategory(Guid userId, string categoryName)
        {
            await _service.DeleteToDoCategoryAsync(userId, categoryName);

            return NoContent();
        }
    }
}
