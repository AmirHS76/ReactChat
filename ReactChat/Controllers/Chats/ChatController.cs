using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReactChat.Application.Services.ChatServices;

namespace ReactChat.Presentation.Controllers.Chats
{
    [Authorize]
    [Route("api/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ChatService _chatService;

        public ChatController(ChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet]
        public async Task<IActionResult> GetGroups()
        {
            return Ok(await _chatService.GetGroupsAsync());
        }

        [HttpPost("create-group")]
        public async Task<IActionResult> CreateGroup([FromBody] string groupName)
        {
            await _chatService.CreateGroupAsync(groupName);
            return Ok(new { message = "Group created successfully" });
        }

        [HttpPost]
        public async Task<IActionResult> JoinGroup([FromBody] string groupName)
        {
            string username = User.Identity?.Name!;
            bool success = await _chatService.AddUserToGroupAsync(username, groupName);
            if (!success) return BadRequest("Group not found");

            return Ok(new { message = $"Joined group {groupName}" });
        }
    }

}
