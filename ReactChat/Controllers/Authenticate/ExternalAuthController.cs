using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using ReactChat.Application.Services.Login;

namespace ReactChat.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalAuthController(IConfiguration configuration, LoginService loginService) : ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly LoginService _loginService = loginService;

        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action("GoogleResponse", "ExternalAuth");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (!result.Succeeded)
                return BadRequest();

            var (token, refreshToken) = _loginService.GenerateTokensForGoogleUser(result.Principal ?? throw new UnauthorizedAccessException("Claims was null")); // Assuming GenerateToken method exists

            var frontEndUrl = _configuration["FrontEnd:Url"];
            return Redirect($"{frontEndUrl}/google-callback?token={token}&refreshToken={refreshToken}");
        }
    }
}