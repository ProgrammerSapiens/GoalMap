using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDosController : ControllerBase
    {
        private readonly IToDoService _service;

        public ToDosController(IToDoService service)
        {
            _service = service;
        }

        [HttpGet("{toDoId}")]
        public async Task<ActionResult<ToDo>> GetToDoById(Guid toDoId)
        {
            if (Guid.Empty == toDoId)
            {
                return BadRequest("Todo id cannot be empty");
            }

            try
            {
                var toDo = await _service.GetToDoByIdAsync(toDoId);

                if (toDo == null)
                {
                    return NotFound("ToDo was not found.");
                }

                return toDo;
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<ToDo>>> GetToDos(DateTime date, TimeBlock timeBlock)
        {
            var userId = User.Identity?.Name;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            try
            {
                var toDos = await _service.GetToDosAsync(Guid.Parse(userId), date, timeBlock);

                if (toDos.Count == 0)
                {
                    return new List<ToDo>();
                }

                return toDos;
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToDo([FromBody] ToDo? toDo)
        {
            if (toDo == null)
            {
                return BadRequest("ToDo data cannot be null.");
            }

            try
            {
                await _service.AddToDoAsync(toDo);
                return CreatedAtAction(nameof(GetToDoById), new { toDoDescription = toDo.Description }, toDo);
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
        public async Task<IActionResult> UpdateToDo([FromBody] ToDo? toDo)
        {
            if (toDo == null)
            {
                return BadRequest("ToDo data cannot be null.");
            }

            try
            {
                await _service.UpdateToDoAsync(toDo);
                return NoContent();
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

        [HttpDelete]
        public async Task<IActionResult> DeleteToDo(Guid toDoId)
        {
            if (Guid.Empty == toDoId)
            {
                return BadRequest("ToDo id cannot be empty.");
            }

            try
            {
                await _service.DeleteToDoAsync(toDoId);
                return NoContent();
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
    }
}
