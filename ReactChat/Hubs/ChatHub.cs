using Microsoft.AspNetCore.SignalR;

namespace ReactChat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task JoinPrivateChat(string username)
        {
            string groupName = GetPrivateGroupName(Context.User.Identity.Name, username);

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
            string senderUsername = Context.User.Identity.Name;
            string groupName = GetPrivateGroupName(senderUsername, recipientUsername);

            await Clients.Caller.SendAsync("ReceiveMessage", senderUsername, message, "sender");

            await Clients.GroupExcept(groupName, Context.ConnectionId)
                         .SendAsync("ReceiveMessage", senderUsername, message, "recipient");
        }


    }
}