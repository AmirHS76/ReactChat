using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReactChat.Application.Interfaces.User;
using ReactChat.Core.Entities.User;
using System.Security.Claims;

namespace ReactChat.Presentation.Controllers.Authenticate
{
    [ApiController]
    [Route("api/v{version:apiVersion}/Authenticate")]
    [ApiVersion("1.0")]
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
        [Route("test")]
        public IActionResult Test()
        {
            return Ok("V1");
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
    [ApiController]
    [Route("api/v{version:apiVersion}/Authenticate")]
    [ApiVersion("2.0")]
    public class AuthenticateV2Controller(IUserService userService) : ControllerBase
    {
        [HttpGet]
        [Route("test")]
        public IActionResult Test()
        {
            return Ok("V2");
        }
    }
}
