using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReactChat.Application.Interfaces.User;
using ReactChat.Core.Entities.User;
using System.Security.Claims;

namespace ReactChat.Presentation.Controllers.Authenticate
{
    [Route("[Controller]")]
    public class AuthenticateController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpGet]
        [Authorize]
        public IActionResult Auth()
        {
            return Ok();
        }
        [HttpGet]
        [Authorize]
        [Route("Data")]
        public async Task<IActionResult> GetUserData(CancellationToken cancellationToken)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            BaseUser? baseUser = await _userService.GetUserByUsernameAsync(username, cancellationToken);
            string userEmail = baseUser?.Email ?? User.FindFirst(ClaimTypes.Email)?.Value ?? "";
            return Ok(new { username, Email = userEmail });
        }
    }
}
