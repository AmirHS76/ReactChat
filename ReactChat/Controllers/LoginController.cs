using Microsoft.AspNetCore.Mvc;
using ReactChat.Application.Services.Login;
using ReactChat.Dtos;

[Route("Login/[controller]")]
public class LoginController : ControllerBase
{
    LoginService _loginService;
    public LoginController(LoginService loginService)
    {
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
        return token != null ? Ok(new { Token = token }) : Unauthorized("Invalid username or password");
    }
}