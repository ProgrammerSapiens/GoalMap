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
        private readonly IJwtTokenService _jwtTokenService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userService">The user service instance.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        public UsersController(IUserService userService, IMapper mapper, IJwtTokenService jwtTokenService)
        {
            _userService = userService;
            _mapper = mapper;
            _jwtTokenService = jwtTokenService;
        }

        /// <summary>
        /// Gets the currently authenticated user.
        /// </summary>
        /// <returns>The user data if found; otherwise, an appropriate HTTP response.</returns>
        [Authorize]
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
        /// Registers a new user.
        /// </summary>
        /// <param name="registerUserDto">The user registration data.</param>
        /// <returns>The created user data.</returns>
        [HttpPost]
        public async Task<ActionResult<User>> RegisterUser([FromBody] UserRegAndAuthDto registerUserDto)
        {
            if (string.IsNullOrEmpty(registerUserDto.Password) || string.IsNullOrEmpty(registerUserDto.UserName)) return BadRequest("User data or password cannot be null");

            var user = _mapper.Map<User>(registerUserDto);
            await _userService.RegisterUserAsync(user, registerUserDto.Password);

            var userDto = _mapper.Map<UserDto>(user);
            return CreatedAtAction(nameof(GetCurrentUser), new { userName = userDto.UserName }, userDto);
        }

        /// <summary>
        /// Authenticates a user and returns a token.
        /// </summary>
        /// <param name="authenticateUserDto">The authentication request containing username and password.</param>
        /// <returns>HTTP 200 OK if successful, otherwise an appropriate HTTP response.</returns>
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateUser([FromBody] UserRegAndAuthDto authenticateUserDto)
        {
            if (string.IsNullOrWhiteSpace(authenticateUserDto.UserName) || string.IsNullOrWhiteSpace(authenticateUserDto.Password))
            {
                return BadRequest("Username or password cannot be empty.");
            }

            var user = await _userService.GetUserByUserNameAsync(authenticateUserDto.UserName);
            if (user == null) return Unauthorized("Invalid username or password.");

            var isAuthenticated = await _userService.AuthenticateUserAsync(authenticateUserDto.UserName, authenticateUserDto.Password);
            if (!isAuthenticated) return Unauthorized("Invalid username or password");

            var token = await _jwtTokenService.GenerateTokenAsync(user);

            return Ok(new { Token = token });
        }

        /// <summary>
        /// Updates user information or experience level.
        /// </summary>
        /// <param name="updateUserDto">The user update data.</param>
        /// <returns>HTTP 204 No Content if successful, otherwise an appropriate HTTP response.</returns>
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
        /// Extracts the authenticated user's ID.
        /// </summary>
        private Guid GetUserId()
        {
            return Guid.TryParse(User.Identity?.Name, out var parsedUserId) ? parsedUserId : new Guid();
        }
    }
}
