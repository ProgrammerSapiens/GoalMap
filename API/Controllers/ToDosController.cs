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
            if (Guid.Empty == toDoId) return BadRequest("Todo id cannot be empty");

            var toDo = await _service.GetToDoByIdAsync(toDoId);
            if (toDo == null) return NotFound("ToDo was not found.");

            return _mapper.Map<ToDoDto>(toDo);
        }

        /// <summary>
        /// Retrieves all To-Dos for a given date and time block.
        /// </summary>
        /// <param name="toDoGetByDateAndTimeBlockDto">DTO containing date and time block information.</param>
        /// <returns>A list of To-Do items.</returns>
        [HttpGet]
        public async Task<ActionResult<List<ToDoDto>>> GetToDos(ToDoGetByDateAndTimeBlockDto toDoGetByDateAndTimeBlockDto)
        {
            var userId = Guid.TryParse(User.Identity?.Name, out var parsedUserId) ? parsedUserId : new Guid();
            if (Guid.Empty == userId) return Unauthorized("User ID is not authenticated or invalid.");

            var toDos = await _service.GetToDosAsync(parsedUserId, toDoGetByDateAndTimeBlockDto.Date, toDoGetByDateAndTimeBlockDto.TimeBlock);
            if (toDos.Count == 0) return new List<ToDoDto>();

            return _mapper.Map<List<ToDoDto>>(toDos);
        }

        /// <summary>
        /// Creates a new To-Do.
        /// </summary>
        /// <param name="toDoAddDto">DTO containing To-Do details.</param>
        /// <returns>The created To-Do item.</returns>
        [HttpPost]
        public async Task<IActionResult> AddToDo([FromBody] ToDoAddDto? toDoAddDto)
        {
            if (toDoAddDto == null) return BadRequest("ToDo data cannot be null.");

            var toDo = _mapper.Map<ToDo>(toDoAddDto);
            await _service.AddToDoAsync(toDo);

            return CreatedAtAction(nameof(GetToDoById), new { toDoId = toDo.ToDoId }, toDo);
        }

        /// <summary>
        /// Updates an existing To-Do.
        /// </summary>
        /// <param name="toDoUpdateDto">DTO containing updated To-Do details.</param>
        /// <returns>No content if the update is successful.</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateToDo([FromBody] ToDoUpdateDto? toDoUpdateDto)
        {
            if (toDoUpdateDto == null) return BadRequest("ToDo data cannot be null.");

            var existingToDo = await _service.GetToDoByIdAsync(toDoUpdateDto.ToDoId);
            if (existingToDo == null) return NotFound("ToDo was not found.");

            _mapper.Map(toDoUpdateDto, existingToDo);
            await _service.UpdateToDoAsync(existingToDo);

            return NoContent();
        }

        /// <summary>
        /// Deletes a To-Do by its ID.
        /// </summary>
        /// <param name="toDoId">The unique identifier of the To-Do.</param>
        /// <returns>No content if the deletion is successful.</returns>
        [HttpDelete("{toDoId}")]
        public async Task<IActionResult> DeleteToDo(Guid toDoId)
        {
            if (Guid.Empty == toDoId) return BadRequest("ToDo id cannot be empty.");

            await _service.DeleteToDoAsync(toDoId);

            return NoContent();
        }
    }
}
