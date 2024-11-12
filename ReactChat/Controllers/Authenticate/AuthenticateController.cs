using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReactChat.Application.Interfaces.Users;
using ReactChat.Core.Entities.Login;
using System.Security.Claims;

namespace ReactChat.Controllers.Authenticate
{
    [Route("[Controller]")]
    public class AuthenticateController : ControllerBase
    {
        IUserService _userService;
        public AuthenticateController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        [Authorize]
        public IActionResult Auth()
        {
            return Ok();
        }
        [HttpGet]
        [Authorize]
        [Route("Data")]
        public async Task<IActionResult> GetUserData()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            BaseUser? baseUser = await _userService.GetUserByUsernameAsync(username);
            return Ok(new { username, baseUser?.Email });
        }
    }
}
