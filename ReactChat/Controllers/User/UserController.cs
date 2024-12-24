using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReactChat.Application.Attributes;
using ReactChat.Application.Interfaces.MessageHistory;
using ReactChat.Application.Interfaces.Users;
using ReactChat.Core.Entities.Login;
using ReactChat.Dtos.Users;
using System.Security.Claims;

namespace ReactChat.Controllers.Users
{
    [Route("[Controller]/")]
    public class UserController : ControllerBase
    {
        IMapper _mapper;
        IUserService _userService;
        IMessageService _messageService;
        public UserController(IUserService userService, IMapper mapper, IMessageService messageService)
        {
            _mapper = mapper;
            _userService = userService;
            _messageService = messageService;
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
            IEnumerable<BaseUser>? result = await _userService.GetAllUsersAsync();
            List<UserDto> users = new List<UserDto>();
            users.AddRange(_mapper.Map<List<UserDto>>(result));

            return Ok(users);
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

        [Authorize]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserByID(id);
            return result ? Ok("User deleted successfully") : BadRequest("User not found");
        }

        [Authorize]
        [HttpGet]
        [Route("chatHistory")]
        public async Task<IActionResult> GetChatHistory(string targetUsername)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(username))
                return Unauthorized("User not found in token.");

            var messages = await _messageService.GetMessagesByUsernameAsync(username, targetUsername);

            if (messages == null || !messages.Any())
                return NotFound("No messages found.");

            return Ok(messages);
        }

    }
}
