using AutoMapper;
using Core.DTOs.User;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Core.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService, IJwtTokenService jwtTokenService, IMapper mapper, ILogger<AuthController> logger)
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="registerUserDto">The user registration data containing the username and password.</param>
        /// <returns>The created user data with a status of 201 Created if the registration is successful, 
        /// or a BadRequest response if the username or password is empty.</returns>
        /// <response code="201">User created successfully</response>
        /// <response code="400">Bad request (invalid username or password)</response>
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] UserRegAndAuthDto registerUserDto)
        {
            _logger.LogInformation("Register");

            if (string.IsNullOrWhiteSpace(registerUserDto.UserName) || string.IsNullOrEmpty(registerUserDto.Password))
            {
                _logger.LogWarning("The entered data is invalid.");
                return BadRequest("Username and password cannot be empty.");
            }

            var user = _mapper.Map<User>(registerUserDto);
            await _userService.RegisterUserAsync(user, registerUserDto.Password);

            var userDto = _mapper.Map<UserDto>(user);
            return CreatedAtAction(nameof(Register), new { userName = userDto.UserName }, userDto);
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        /// <param name="loginDto">The login data containing the username and password.</param>
        /// <returns>HTTP 200 OK with the token if authentication is successful, 
        /// or an Unauthorized response if the credentials are invalid or missing.</returns>
        /// <response code="200">Authentication successful, returns JWT token</response>
        /// <response code="400">Bad request (invalid username or password)</response>
        /// <response code="401">Unauthorized (invalid credentials)</response>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserRegAndAuthDto loginDto)
        {
            _logger.LogInformation("Login");

            if (string.IsNullOrWhiteSpace(loginDto.UserName) || string.IsNullOrWhiteSpace(loginDto.Password))
            {
                _logger.LogWarning("The entered data is invalid.");
                return BadRequest("Username or password cannot be empty.");
            }

            var user = await _userService.GetUserByUserNameAsync(loginDto.UserName);
            if (user == null || !await _userService.AuthenticateUserAsync(loginDto.UserName, loginDto.Password))
            {
                _logger.LogWarning("Invalid username or password.");
                return Unauthorized("Invalid username or password.");
            }

            var token = await _jwtTokenService.GenerateTokenAsync(user);
            return Ok(new { Token = token });
        }
    }
}
