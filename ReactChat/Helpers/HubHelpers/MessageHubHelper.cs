using Hangfire;
using ReactChat.Application.Interfaces.MessageHub;
using ReactChat.Application.Services.BackgroundServices;

namespace ReactChat.Helpers.HubHelpers
{
    public class MessageHubHelper : IMessageHubHelper
    {
        IMessageHubService _messageHubService;
        private readonly MessageProcessingService _messageProcessingService;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public MessageHubHelper(IMessageHubService messageHubService, MessageProcessingService messageProcessingService, IBackgroundJobClient backgroundJobClient)
        {
            _messageProcessingService = messageProcessingService;
            _messageHubService = messageHubService;
            _backgroundJobClient = backgroundJobClient;
        }
        public async Task SaveMessageAsync(string sender, string recipient, string message)
        {
            _backgroundJobClient.Enqueue(() => _messageProcessingService.EnqueueMessage(sender, recipient, message));
        }

        public string GetPrivateGroupName(string user1, string user2)
        {
            var sortedUsers = new[] { user1, user2 };
            Array.Sort(sortedUsers);
            return $"private_{sortedUsers[0]}_{sortedUsers[1]}";
        }
    }
}
