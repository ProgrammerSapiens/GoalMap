using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Core.Models;
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
        /// <returns>The user data if found; otherwise, an appropriate HTTP response.</returns>
        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var userId = User.Identity?.Name;

            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
            {
                return Unauthorized("User ID is not authenticated or invalid.");
            }

            try
            {
                var user = await _userService.GetUserByUserIdAsync(parsedUserId);

                if (user == null)
                {
                    return NotFound("User was not found.");
                }

                var userDto = _mapper.Map<UserDto>(user);

                return userDto;
            }
            //Add logger through DI
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="registerUserDto">The user registration data.</param>
        /// <returns>The created user data.</returns>
        [HttpPost]
        public async Task<ActionResult<User>> RegisterUser([FromBody] UserRegAndAuthDto registerUserDto)
        {
            var user = _mapper.Map<User>(registerUserDto);
            var password = registerUserDto.Password;

            if (user == null || string.IsNullOrEmpty(password))
            {
                return BadRequest("User data or password cannot be null");
            }

            try
            {
                await _userService.RegisterUserAsync(user, password);

                var userDto = _mapper.Map<UserDto>(user);

                return CreatedAtAction(nameof(GetCurrentUser), new { userName = userDto.UserName }, userDto);
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
                return StatusCode(500, "Internal service error.");
            }
        }

        //TODO: Add returning Token from service
        /// <summary>
        /// Authenticates a user and returns a token.
        /// </summary>
        /// <param name="authenticateUserDto">The authentication request containing username and password.</param>
        /// <returns>HTTP 200 OK if successful, otherwise an appropriate HTTP response.</returns>
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateUser([FromBody] UserRegAndAuthDto authenticateUserDto)
        {
            var userName = authenticateUserDto.UserName;
            var password = authenticateUserDto.Password;

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                return BadRequest("Username or password cannot be empty.");
            }

            try
            {
                var authentication = await _userService.AuthenticateUserAsync(userName, password);

                if (!authentication)
                {
                    return Unauthorized("Invalid username or password");
                }

                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Updates user information or experience level.
        /// </summary>
        /// <param name="updateUserDto">The user update data.</param>
        /// <returns>HTTP 204 No Content if successful, otherwise an appropriate HTTP response.</returns>
        [HttpPut("experience")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto updateUserDto)
        {
            var userId = User.Identity?.Name;

            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
            {
                return Unauthorized("User ID is not authenticated or invalid.");
            }

            if (updateUserDto.Difficulty == Difficulty.None && string.IsNullOrEmpty(updateUserDto.UserName))
            {
                return BadRequest("UserName cannot be empty.");
            }

            try
            {
                if (updateUserDto.Difficulty == Difficulty.None)
                {
                    var existingUser = await _userService.GetUserByUserIdAsync(parsedUserId);

                    if (existingUser == null)
                    {
                        return NotFound("User was not found.");
                    }

                    _mapper.Map(updateUserDto, existingUser);

                    await _userService.UpdateUserAsync(existingUser);
                }
                else
                {
                    await _userService.UpdateUserExperienceAsync(parsedUserId, updateUserDto.Difficulty);
                }
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
