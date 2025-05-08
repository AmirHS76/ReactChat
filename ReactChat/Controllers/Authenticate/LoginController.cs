using Microsoft.AspNetCore.Mvc;
using ReactChat.Application.Dtos.Authenticate;
using ReactChat.Application.Services.User.Login;
using ReactChat.Application.Services.User.Session;
using System.Security.Claims;

namespace ReactChat.Presentation.Controllers.Authenticate
{
    [Route("[Controller]")]
    public class LoginController(LoginService loginService, SessionService sessionService) : ControllerBase
    {
        private readonly LoginService _loginService = loginService;
        private readonly SessionService _sessionService = sessionService;

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = await _loginService.Authenticate(request, cancellationToken, HttpContext);
            if (token == null)
                return Unauthorized("Invalid username or password");
            var refreshToken = _loginService.GenerateRefreshToken(request.Username);
            return Ok(new { token, refreshToken });
        }

        [HttpGet]
        public async Task<IActionResult> RefreshToken(string refreshToken, CancellationToken cancellationToken)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var userIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var userSession = await _sessionService.GetCurrentUserSession(userId, userIp, cancellationToken);
            if (userSession?.IsRevoked ?? true)
                return BadRequest("Your session has been revoked.");

            return Ok(new { token = await _loginService.ValidateRefreshToken(refreshToken, cancellationToken) });
        }
    }
}