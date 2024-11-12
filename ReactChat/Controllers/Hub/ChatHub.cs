using Microsoft.AspNetCore.SignalR;
using ReactChat.Helpers.HubHelpers;

namespace ReactChat.Controllers.Hub
{
    public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly IMessageHubHelper _messageHubHelper;

        public ChatHub(IMessageHubHelper messageHubHelper)
        {
            _messageHubHelper = messageHubHelper;
        }
        public async Task JoinPrivateChat(string username)
        {
            if (Context.User?.Identity?.Name == null)
                throw new ArgumentNullException("User not found");
            string groupName = _messageHubHelper.GetPrivateGroupName(Context.User.Identity.Name, username);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Caller.SendAsync("JoinedGroup", groupName);
        }

        private string GetPrivateGroupName(string user1, string user2)
        {
            var sortedUsers = new[] { user1, user2 };
            Array.Sort(sortedUsers);
            return $"private_{sortedUsers[0]}_{sortedUsers[1]}";
        }

        public async Task SendPrivateMessage(string recipientUsername, string message)
        {
            if (Context.User?.Identity?.Name == null)
                throw new ArgumentNullException("User not found");
            string senderUsername = Context.User.Identity.Name;
            string groupName = _messageHubHelper.GetPrivateGroupName(senderUsername, recipientUsername);

            await _messageHubHelper.SaveMessageAsync(senderUsername, recipientUsername, message);
            await Clients.Caller.SendAsync("ReceiveMessage", senderUsername, message, "sender");

            await Clients.GroupExcept(groupName, Context.ConnectionId)
                         .SendAsync("ReceiveMessage", senderUsername, message, "recipient");
        }


    }
}