using AutoMapper;
using Core.DTOs.ToDo;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controller for managing To-Do tasks.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ToDosController : ControllerBase
    {
        private readonly IToDoService _service;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDosController"/> class.
        /// </summary>
        /// <param name="service">Service for handling To-Do operations.</param>
        /// <param name="mapper">AutoMapper instance for DTO mapping.</param>
        public ToDosController(IToDoService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets a To-Do by its ID.
        /// </summary>
        /// <param name="toDoId">The unique identifier of the To-Do.</param>
        /// <returns>The requested To-Do item.</returns>
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

        /// <summary>
        /// Retrieves all To-Dos for a given date and time block.
        /// </summary>
        /// <param name="toDoGetByDateAndTimeBlockDto">DTO containing date and time block information.</param>
        /// <returns>A list of To-Do items.</returns>
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

        /// <summary>
        /// Creates a new To-Do.
        /// </summary>
        /// <param name="toDoAddDto">DTO containing To-Do details.</param>
        /// <returns>The created To-Do item.</returns>
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

        /// <summary>
        /// Updates an existing To-Do.
        /// </summary>
        /// <param name="toDoUpdateDto">DTO containing updated To-Do details.</param>
        /// <returns>No content if the update is successful.</returns>
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

        /// <summary>
        /// Deletes a To-Do by its ID.
        /// </summary>
        /// <param name="toDoId">The unique identifier of the To-Do.</param>
        /// <returns>No content if the deletion is successful.</returns>
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
