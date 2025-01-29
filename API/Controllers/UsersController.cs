using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        //TODO: Add DTO models
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<User>> GetCurrentUser()
        {
            var userName = User.Identity?.Name;

            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized("User is not authenticated.");
            }

            try
            {
                var user = await _userService.GetUserByUserNameAsync(userName);

                if (user == null)
                {
                    return NotFound();
                }

                return user;
            }
            //Add logger through DI
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        //TODO: Add returning user if registration is success
        [HttpPost]
        public async Task<ActionResult<User>> RegisterUser([FromBody] User user, [FromQuery] string? password)
        {
            if (user == null || string.IsNullOrEmpty(password))
            {
                return BadRequest("User data or password cannot be null");
            }

            try
            {
                await _userService.RegisterUserAsync(user, password);
                return CreatedAtAction(nameof(GetCurrentUser), new { userName = user.UserName }, user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //TODO: Add returning Token from service
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateUser([FromBody] User user, [FromQuery] string? password)
        {
            if (string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(password))
            {
                return BadRequest("Username or password cannot be empty.");
            }

            try
            {
                var authentication = await _userService.AuthenticateUserAsync(user.UserName, user.PasswordHash);

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

        [HttpPut("experience")]
        public async Task<IActionResult> UpdateUserExperience([FromBody] Difficulty difficulty)
        {
            var userName = User.Identity?.Name;

            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized("User is not authenticated.");
            }

            try
            {
                await _userService.UpdateUserExperienceAsync(userName, difficulty);
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
