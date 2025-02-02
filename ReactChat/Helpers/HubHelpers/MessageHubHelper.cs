using Hangfire;
using MediatR;
using ReactChat.Application.Features.User.Queries.GetByUsername;
using ReactChat.Application.Services.BackgroundService;
using ReactChat.Core.Enums;

namespace ReactChat.Presentation.Helpers.HubHelpers
{
    public class MessageHubHelper(MessageProcessingService messageProcessingService, IBackgroundJobClient backgroundJobClient, IMediator mediator) : IMessageHubHelper
    {
        private readonly MessageProcessingService _messageProcessingService = messageProcessingService;
        private readonly IBackgroundJobClient _backgroundJobClient = backgroundJobClient;
        private readonly IMediator _mediator = mediator;
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

        public async Task<bool> CheckUserAccess(string userName, Accesses access)
        {
            var user = await _mediator.Send(new GetUserByUsernameQuery(userName));
            if (user is not null && user.HasAccess(access))
            {
                return true;
            }
            return false;
        }
    }
}
