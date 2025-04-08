using Microsoft.AspNetCore.SignalR;
using ReactChat.Presentation.Helpers.HubHelpers;

namespace ReactChat.Presentation.Controllers.Hub
{
    public class ChatHub(IMessageHubHelper messageHubHelper) : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly IMessageHubHelper _messageHubHelper = messageHubHelper;

        public async Task JoinPrivateChat(string username)
        {
            if (Context.User?.Identity?.Name == null)
                throw new ArgumentNullException(nameof(username), "User not found");
            string groupName = _messageHubHelper.GetPrivateGroupName(Context.User.Identity.Name, username);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Caller.SendAsync("JoinedGroup", groupName);
        }

        public async Task SendPrivateMessage(string recipientUsername, string message)
        {
            if (Context.User?.Identity?.Name == null)
                throw new ArgumentNullException(nameof(recipientUsername), "User not found");
            string senderUsername = Context.User.Identity.Name;
            if (!await _messageHubHelper.CheckUserAccess(senderUsername, Core.Enums.Accesses.CanSendMessage))
                throw new UnauthorizedAccessException("You don't have access to send message");
            string groupName = _messageHubHelper.GetPrivateGroupName(senderUsername, recipientUsername);

            _messageHubHelper.SaveMessageAsync(senderUsername, recipientUsername, message);
            await Clients.Caller.SendAsync("ReceiveMessage", senderUsername, message, "sender");

            await Clients.GroupExcept(groupName, Context.ConnectionId)
                         .SendAsync("ReceiveMessage", senderUsername, message, "recipient");
        }

        public async Task<bool> CreateGroup(string groupName, List<string> members)
        {
            if (string.IsNullOrWhiteSpace(groupName))
                throw new ArgumentException("Group name cannot be empty");

            if (members == null || !members.Any())
                throw new ArgumentException("Group must have at least one member");

            bool success = await _messageHubHelper.CreateGroupAsync(groupName, members);

            if (success)
                await Clients.All.SendAsync("GroupCreated", groupName, members);

            return success;
        }

        public async Task JoinGroup(string groupName)
        {
            var username = Context.User?.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                throw new UnauthorizedAccessException("User not found");

            await _messageHubHelper.AddUserToGroupAsync(username, groupName);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Caller.SendAsync("JoinedGroup", groupName);
        }

        public async Task SendGroupMessage(string groupName, string message)
        {
            var username = Context.User?.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                throw new UnauthorizedAccessException("User not found");

            _messageHubHelper.SaveMessageAsync(username, groupName, message);

            await Clients.Group(groupName).SendAsync("ReceiveMessage", username, message, null);
        }

        public async Task<List<string?>?> GetUserGroups()
        {
            var username = Context.User?.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                throw new UnauthorizedAccessException("User not found");

            return await _messageHubHelper.GetUserGroupsAsync(username);
        }
    }
}