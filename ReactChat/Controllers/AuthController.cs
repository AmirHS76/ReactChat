using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReactChat.Controllers
{
    [Route("Auth")]
    public class AuthController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ValidateAction()
        {
            await Task.Delay(10);
            return Ok();
        }
    }
}
