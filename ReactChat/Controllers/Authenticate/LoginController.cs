using Microsoft.AspNetCore.Mvc;
using ReactChat.Application.Dtos.Authenticate;
using ReactChat.Application.Services.Login;

namespace ReactChat.Presentation.Controllers.Authenticate
{
    [Route("[Controller]")]
    public class LoginController(ILogger<LoginController> logger, LoginService loginService) : ControllerBase
    {
        private readonly ILogger<LoginController> _logger = logger;
        private readonly LoginService _loginService = loginService;

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = await _loginService.Authenticate(request.Username, request.Password);
            if (token == null)
                return Unauthorized("Invalid username or password");
            var refreshToken = _loginService.GenerateRefreshToken(request.Username);
            return Ok(new { token, refreshToken });
        }
        [HttpGet]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            return Ok(new { token = await _loginService.ValidateRefreshToken(refreshToken) });
        }
    }
}