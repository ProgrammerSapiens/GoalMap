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
            var userId = User.Identity?.Name;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            try
            {
                var user = await _userService.GetUserByUserIdAsync(Guid.Parse(userId));

                if (user == null)
                {
                    return NotFound("User was not found.");
                }

                return user;
            }
            //Add logger through DI
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

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
        public async Task<IActionResult> AuthenticateUser([FromQuery] string? userName, [FromQuery] string? password)
        {
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

        [HttpPut("experience")]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            var userId = User.Identity?.Name;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            try
            {
                await _userService.UpdateUserAsync(user);
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
