using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReactChat.Application.Dtos.User;
using ReactChat.Application.Services.MessageHistory;
using ReactChat.Application.Services.User;
using ReactChat.Core.Entities.User;
using ReactChat.Presentation.Helpers.Attributes;
using System.Security.Claims;
using static ReactChat.Core.Enums.Accesses;

namespace ReactChat.Presentation.Controllers.User
{
    [Route("[Controller]/")]
    public class UserController(UserService userService, IMapper mapper, MessageService messageService) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly UserService _userService = userService;
        private readonly MessageService _messageService = messageService;

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("User not found in token.");
            }

            var user = await _userService.GetUserByUsernameAsync(username, cancellationToken);

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
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
        {
            IEnumerable<BaseUser>? result = await _userService.GetAllUsersAsync(cancellationToken);

            List<UserDTO> users = _mapper.Map<List<UserDTO>>(result);

            return Ok(users);
        }

        [Authorize]
        [HttpGet]
        [Route("getRole")]
        public async Task<IActionResult> GetUserRole(CancellationToken cancellationToken)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _userService.GetUserByUsernameAsync(username, cancellationToken);
            if (user == null)
            {
                if (User.FindFirst(ClaimTypes.Role) != null)
                    return Ok(User.FindFirst(ClaimTypes.Role)?.Value);
                return BadRequest();
            }
            return Ok(new { role = user.UserRole.ToString() });
        }

        [Authorize]
        [HttpGet]
        [Route("getAccesses")]
        public async Task<IActionResult> GetUserAccesses(CancellationToken cancellationToken)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _userService.GetUserByUsernameAsync(username, cancellationToken);
            if (user == null)
            {
                return BadRequest();
            }
            return Ok(new { accesses = user.Accesses.ToString() });
        }

        [CustomAuthorize(CanAddUser)]
        [HttpPost]
        public async Task<IActionResult> AddNewUser([FromBody] UserDTO user, CancellationToken cancellationToken)
        {
            if (user.Password == null)
            {
                throw new ArgumentNullException(nameof(user), "Password was null");
            }
            return await _userService.AddNewUserAsync(user.Username, user.Password, user.Email, user.Role, cancellationToken) ? Ok(user) : BadRequest();
        }

        [CustomAuthorize(CanUpdateUser)]
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserDTO user, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return await _userService.UpdateUserAsync(user.Id ?? 0, user.Username, user.Email, cancellationToken) ? Ok(user) : BadRequest(ModelState);
        }

        [Authorize]
        [HttpPatch]
        [Route("{email}")]
        public async Task<IActionResult> UpdateEmail(string email, CancellationToken cancellationToken)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            if (username == null)
                return Unauthorized();
            var user = await _userService.GetUserByUsernameAsync(username, cancellationToken);
            if (user == null)
                return NotFound();
            return await _userService.UpdateUserAsync(user.Id, username, email, cancellationToken) ? Ok() : BadRequest();
        }

        [CustomAuthorize(CanRemoveUser)]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUser(int id, CancellationToken cancellationToken)
        {
            var result = await _userService.DeleteUserByID(id, cancellationToken);
            return result ? Ok("User deleted successfully") : BadRequest("User not found");
        }

        [Authorize]
        [HttpGet]
        [Route("chatHistory")]
        public async Task<IActionResult> GetChatHistory(string targetUsername, int pageNum, CancellationToken cancellationToken)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(username))
                return Unauthorized("User not found in token.");

            var (messages, hasMore) = await _messageService.GetMessagesByUsernameAsync(username, targetUsername, pageNum, cancellationToken);

            if (messages == null || !messages.Any())
                return Ok();

            return Ok(new { messages, hasMore });
        }
    }
}
