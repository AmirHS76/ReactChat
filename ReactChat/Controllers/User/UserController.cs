using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReactChat.Application.Attributes;
using ReactChat.Application.Interfaces.Users;
using ReactChat.Dtos.Users;
using System.Security.Claims;

namespace ReactChat.Controllers.Users
{
    [Route("[Controller]/")]
    public class UserController : ControllerBase
    {
        IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [Authorize]
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
        [Authorize]
        [HttpGet]
        [Route("getAll")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            //todo : Automapper
            var userDtos = users.Select(user => new UserDto
            {
                Id = user.Id,
                Email = user.Email ?? "",
                Username = user.Username ?? "",
                Role = user.Role.ToString()
            }).ToList();

            return Ok(userDtos);
        }
        [Authorize]
        [HttpGet]
        [Route("getRole")]
        public async Task<IActionResult> GetUserRole()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null)
                return NotFound();
            return Ok(new { role = user.Role.ToString() });
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddNewUser([FromBody] UserDto user)
        {
            return await _userService.AddNewUserAsync(user.Username, user.Password, user.Email, user.Role) ? Ok(user) : BadRequest();
        }
        [CustomAuthorize("Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return (await _userService.UpdateUserAsync(user.Id ?? 0, user.Username, user.Email)) ? Ok(user) : BadRequest(ModelState);
        }
        [Authorize]
        [HttpPatch]
        [Route("{email}")]
        public async Task<IActionResult> UpdateEmail(string email)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            if (username == null)
                return Unauthorized();
            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null)
                return NotFound();
            return await _userService.UpdateUserAsync(user.Id, username, email) ? Ok() : BadRequest();
        }
    }
}
