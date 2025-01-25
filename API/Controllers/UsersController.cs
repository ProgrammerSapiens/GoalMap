using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserByUserName(string userName)
        {
            var user = await _userService.GetUserByUserNameAsync(userName);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
