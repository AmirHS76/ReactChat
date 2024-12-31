﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ReactChat.Application.Dtos.User;
using ReactChat.Application.Interfaces.Register;

namespace ReactChat.Presentation.Controllers.User
{
    [Route("[Controller]")]
    public class RegisterController(IRegisterService registerService) : ControllerBase
    {
        private readonly IRegisterService _registerService = registerService;

        [HttpPost]
        public async Task<ActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(allErrors.Select(x => x.ErrorMessage));
            }
            return await _registerService.Register(registerDto.Username, registerDto.Password, registerDto.Email ?? "") ? Ok() : BadRequest();
        }
    }
}
