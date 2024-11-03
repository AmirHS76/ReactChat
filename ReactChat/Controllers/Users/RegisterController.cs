using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ReactChat.Application.Interfaces.Register;
using ReactChat.Dtos;

namespace ReactChat.Controllers.Users
{
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        IRegisterService _registerService;
        public RegisterController(IRegisterService registerService)
        {
            _registerService = registerService;
        }
        [HttpPost]
        public async Task<ActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(allErrors.Select(x => x.ErrorMessage));
            }
            return await _registerService.Register(registerDto.Username, registerDto.Password, registerDto.Email) ? Ok() : BadRequest();
        }
    }
}
