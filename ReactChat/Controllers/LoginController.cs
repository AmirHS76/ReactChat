using Microsoft.AspNetCore.Mvc;
using ReactChat.Application.Services.Login;
using ReactChat.Dtos;

[Route("Auth/[controller]")]
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
        return await _loginService.Authenticate(request.username, request.password) ? Ok($"Login ok with username : {request.username}") 
            : Unauthorized("Invalid username or password");
    }
}