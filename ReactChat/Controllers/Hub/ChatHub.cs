using Microsoft.AspNetCore.SignalR;
using ReactChat.Presentation.Helpers.HubHelpers;

namespace ReactChat.Presentation.Controllers.Hub
{
    public class ChatHub(IMessageHubHelper messageHubHelper) : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly IMessageHubHelper _messageHubHelper = messageHubHelper;

        public async Task JoinPrivateChat(string username, CancellationToken cancellationToken = default)
        {
            if (Context.User?.Identity?.Name == null)
                throw new ArgumentNullException(nameof(username), "User not found");
            string groupName = _messageHubHelper.GetPrivateGroupName(Context.User.Identity.Name, username);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Caller.SendAsync("JoinedGroup", groupName);
        }

        public async Task SendPrivateMessage(string recipientUsername, string message, CancellationToken cancellationToken = default)
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
    }
}