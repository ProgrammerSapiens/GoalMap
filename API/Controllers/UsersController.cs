using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Core.DTOs.User;
using AutoMapper;

namespace API.Controllers
{
    /// <summary>
    /// Controller for managing user-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userService">The user service instance.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the currently authenticated user.
        /// </summary>
        /// <returns>
        /// The user data if the user is authenticated and found, otherwise an appropriate HTTP response:
        /// - HTTP 401 Unauthorized if the user is not authenticated.
        /// - HTTP 404 Not Found if the user does not exist.
        /// </returns>
        /// <response code="200">Returns the current authenticated user's data.</response>
        /// <response code="401">Unauthorized if user authentication fails or user ID is invalid.</response>
        /// <response code="404">Not Found if the user is not found in the system.</response>
        [HttpGet("me")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var userId = GetUserId();
            if (Guid.Empty == userId) return Unauthorized("User ID is not authenticated or invalid.");

            var user = await _userService.GetUserByUserIdAsync(userId);
            if (user == null) return NotFound("User was not found.");

            return _mapper.Map<UserDto>(user);
        }

        /// <summary>
        /// Updates user information or experience level.
        /// </summary>
        /// <param name="updateUserDto">The user update data.</param>
        /// <returns>
        /// - HTTP 204 No Content if the update is successful.
        /// - HTTP 400 Bad Request if the update data is invalid or null.
        /// - HTTP 401 Unauthorized if the user is not authenticated.
        /// - HTTP 404 Not Found if the user does not exist.
        /// </returns>
        /// <response code="204">No Content if the user profile is updated successfully.</response>
        /// <response code="400">Bad Request if the user data is invalid or null.</response>
        /// <response code="401">Unauthorized if the user is not authenticated.</response>
        /// <response code="404">Not Found if the user does not exist.</response>
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UserUpdateDto? updateUserDto)
        {
            var userId = GetUserId();
            if (Guid.Empty == userId) return Unauthorized("User ID is not authenticated or invalid.");

            if (updateUserDto == null) return BadRequest("User data cannot be null.");

            var existingUser = await _userService.GetUserByUserIdAsync(userId);
            if (existingUser == null) return NotFound("User was not found.");

            _mapper.Map(updateUserDto, existingUser);
            await _userService.UpdateUserAsync(existingUser);

            return NoContent();
        }

        /// <summary>
        /// Extracts the authenticated user's ID from the current user's claims.
        /// </summary>
        /// <returns>
        /// The authenticated user's ID if present, otherwise an empty GUID.
        /// </returns>
        private Guid GetUserId()
        {
            return Guid.TryParse(User.Identity?.Name, out var parsedUserId) ? parsedUserId : new Guid();
        }
    }
}
