using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ReactChat.Application.Services.Login;
using ReactChat.Dtos.Authenticate;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("[Controller]")]
public class LoginController : ControllerBase
{
    private readonly string _refreshTokenSecretKey = "RefreshTokenSecretAmiRHS";
    private readonly ILogger<LoginController> _logger;
    LoginService _loginService;
    public LoginController(ILogger<LoginController> logger,LoginService loginService)
    {
        _logger = logger;
        _loginService = loginService;
    }
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var token = await _loginService.Authenticate(request.username, request.password);
        if (token == null)
            return Unauthorized("Invalid username or password");
        var refreshToken =  _loginService.GenerateRefreshToken(request.username);
        return Ok(new { token, refreshToken });
    }
    [HttpGet]
    public async Task<IActionResult> RefreshToken(string refreshToken)
    {
        return Ok(new {token = await _loginService.ValidateRefreshToken(refreshToken) });
    }
}