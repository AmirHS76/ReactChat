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
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            var userDtos = users.Select(user => new UserDto
            {
                email = user.Email ?? "",
                username = user.Username ?? ""
            }).ToList();

            return Ok(userDtos);
        }
        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("User not found in token.");
            }

            var user = await _userService.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(new
            {
                username = user.Username,
                email = user.Email
            });
        }
    }
}
