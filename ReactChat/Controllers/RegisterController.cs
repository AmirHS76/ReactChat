using Microsoft.AspNetCore.Mvc;
using ReactChat.Application.Interfaces.Register;
using ReactChat.Dtos;

namespace ReactChat.Controllers
{
    [Route("Auth/[Controller]")]
    public class RegisterController : ControllerBase
    {
        IRegisterService _registerService;
        public RegisterController(IRegisterService registerService)
        {
            _registerService = registerService;
        }
        public async Task<ActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return await _registerService.Register(registerDto.Username, registerDto.Password, registerDto.Email) ? Ok() : BadRequest();
        }
    }
}
