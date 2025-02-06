using AutoMapper;
using Core.DTOs.ToDo;
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
        private readonly IMapper _mapper;

        public ToDosController(IToDoService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("{toDoId}")]
        public async Task<ActionResult<ToDoDto>> GetToDoById(Guid toDoId)
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

                var toDoDto = _mapper.Map<ToDoDto>(toDo);

                return toDoDto;
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<ToDoDto>>> GetToDos(ToDoGetByDateAndTimeBlockDto toDoGetByDateAndTimeBlockDto)
        {
            var userId = User.Identity?.Name;

            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
            {
                return Unauthorized("User ID is not authenticated or invalid.");
            }

            var date = toDoGetByDateAndTimeBlockDto.Date;
            var timeBlock = toDoGetByDateAndTimeBlockDto.TimeBlock;

            try
            {
                var toDos = await _service.GetToDosAsync(parsedUserId, date, timeBlock);

                if (toDos.Count == 0)
                {
                    return new List<ToDoDto>();
                }

                var toDosDto = _mapper.Map<List<ToDoDto>>(toDos);

                return toDosDto;
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToDo([FromBody] ToDoAddDto? toDoAddDto)
        {
            if (toDoAddDto == null)
            {
                return BadRequest("ToDo data cannot be null.");
            }

            try
            {
                var toDo = _mapper.Map<ToDo>(toDoAddDto);

                await _service.AddToDoAsync(toDo);
                return CreatedAtAction(nameof(GetToDoById), new { toDoId = toDo.ToDoId }, toDo);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
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
        public async Task<IActionResult> UpdateToDo([FromBody] ToDoUpdateDto? toDoUpdateDto)
        {
            if (toDoUpdateDto == null)
            {
                return BadRequest("ToDo data cannot be null.");
            }

            try
            {
                var existingToDo = await _service.GetToDoByIdAsync(toDoUpdateDto.ToDoId);

                if (existingToDo == null)
                {
                    return NotFound("ToDo was not found.");
                }

                _mapper.Map(toDoUpdateDto, existingToDo);

                await _service.UpdateToDoAsync(existingToDo);
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

        [HttpDelete("{toDoId}")]
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
