using Hangfire;
using ReactChat.Application.Services.BackgroundService;

namespace ReactChat.Presentation.Helpers.HubHelpers
{
    public class MessageHubHelper(MessageProcessingService messageProcessingService, IBackgroundJobClient backgroundJobClient) : IMessageHubHelper
    {
        private readonly MessageProcessingService _messageProcessingService = messageProcessingService;
        private readonly IBackgroundJobClient _backgroundJobClient = backgroundJobClient;
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
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
