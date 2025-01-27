using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Core.Models;

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

        [HttpGet("{userName}")]
        public async Task<IActionResult> GetUserByUserName(string userName)
        {
            var user = await _userService.GetUserByUserNameAsync(userName);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        //TODO: Add returning user if registration is success
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] User user, [FromBody] string? password)
        {
            if (user == null)
            {
                return BadRequest("User data cannot be null");
            }

            try
            {
                await _userService.RegisterUserAsync(user, password);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return CreatedAtAction(nameof(GetUserByUserName), new { userName = user.UserName }, user);
        }

        //TODO: Add returning Token from service
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateUser([FromBody] User user)
        {
            var authentication = await _userService.AuthenticateUserAsync(user.UserName, user.PasswordHash);

            if (!authentication)
            {
                return Unauthorized("Invalid username or password");
            }

            return Ok();
        }

        [HttpPut("experience/{userName}")]
        public async Task<IActionResult> UpdateUserExperience(string userName, [FromBody] Difficulty difficulty)
        {
            try
            {
                await _userService.UpdateUserExperienceAsync(userName, difficulty);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }
    }
}
