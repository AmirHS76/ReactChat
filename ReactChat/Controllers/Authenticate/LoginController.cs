using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReactChat.Application.Services.Login;
using ReactChat.Dtos.Authenticate;

[Route("[Controller]")]
public class LoginController : ControllerBase
{
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
        _logger.LogInformation("User token = " + token);
        return token != null ? Ok(new { Token = token }) : Unauthorized("Invalid username or password");
    }
}