using Microsoft.AspNetCore.Mvc;
using ReactChat.Application.Services.User.Session;
using System.Security.Claims;

namespace ReactChat.Presentation.Controllers.User
{
    [Route("api/v1/[Controller]/")]
    public class SessionController(SessionService sessionService) : ControllerBase
    {
        private SessionService _sessionService = sessionService;

        [HttpGet]
        public async Task<IActionResult> GetUserSessions(CancellationToken cancellationToken = default)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException("User is invalid"));

            var result = await _sessionService.GetUserSessions(userId, cancellationToken);

            return result == null ? NotFound(userId) : Ok(result);
        }

        [HttpPatch("{sessionId}")]
        public async Task<IActionResult> RevokeSession(int sessionId, CancellationToken cancellationToken = default)
        {
            return await _sessionService.RevokeSession(sessionId, cancellationToken) ? Ok() : NotFound();
        }
    }
}
