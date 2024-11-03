using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReactChat.Application.Interfaces.Users;
using ReactChat.Application.Services.Users;
using ReactChat.Dtos.Users;
using System.Security.Claims;

namespace ReactChat.Controllers.Users
{
    [Authorize]
    [Route("User/[Action]")]
    public class UserController : ControllerBase
    {
        IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        [Route("{email}")]
        public async Task<IActionResult> UpdateEmail(string email)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            if (username == null)
                return Unauthorized();
            return await _userService.UpdateUserAsync(username, email) ? Ok() : BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserDto user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return (await _userService.UpdateUserAsync(user.username,user.email)) ? Ok(user) : BadRequest(ModelState);
        }
    }
}
