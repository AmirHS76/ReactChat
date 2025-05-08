using Microsoft.AspNetCore.Mvc;
using ReactChat.Application.Helpers.Captcha;
using ReactChat.Application.Services.Captcha;
using System.Runtime.Versioning;

namespace ReactChat.Presentation.Controllers.Authenticate
{
    [Route("api/v1/[controller]")]
    public class CaptchaController : Controller
    {
        private readonly CaptchaImageService _imageService;
        private readonly SessionCaptchaStore _store;

        public CaptchaController(CaptchaImageService imageService, SessionCaptchaStore store)
        {
            _imageService = imageService;
            _store = store;
        }

        [HttpGet]
        [SupportedOSPlatform("windows")]
        public IActionResult Image()
        {
            var text = CaptchaGenerator.GenerateRandomText(5);
            _store.Save(text);
            var imageBytes = _imageService.GenerateCaptchaImage(text);
            return Ok(Convert.ToBase64String(imageBytes));
        }
    }
}
