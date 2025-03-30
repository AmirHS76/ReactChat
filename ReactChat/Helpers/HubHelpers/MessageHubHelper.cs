using Hangfire;
using MediatR;
using ReactChat.Application.Features.Groups.Commands;
using ReactChat.Application.Features.Groups.Queries;
using ReactChat.Application.Features.User.Queries.GetByUsername;
using ReactChat.Application.Features.UserGroups.Commands;
using ReactChat.Application.Features.UserGroups.Queries.GetUserGroups;
using ReactChat.Application.Services.BackgroundService;
using ReactChat.Core.Entities.Chat.Group;
using ReactChat.Core.Entities.User;
using ReactChat.Core.Enums;

namespace ReactChat.Presentation.Helpers.HubHelpers
{
    public class MessageHubHelper(MessageProcessingService messageProcessingService, IBackgroundJobClient backgroundJobClient, IMediator mediator) : IMessageHubHelper
    {
        private readonly MessageProcessingService _messageProcessingService = messageProcessingService;
        private readonly IBackgroundJobClient _backgroundJobClient = backgroundJobClient;
        private readonly IMediator _mediator = mediator;

        public void SaveMessageAsync(string sender, string recipient, string message)
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

        public async Task<bool> CreateGroupAsync(string groupName, List<string> members)
        {
            if ((await _mediator.Send(new GetSingleGroupByGroupNameQuery(groupName))) != null)
                return false;

            var newGroup = new ChatGroup { GroupName = groupName };
            int groupId = await _mediator.Send(new CreateGroupCommand(newGroup));

            await _mediator.Send(new CreateMultiUserGroupsCommand(groupId, members));
            return true;
        }

        public async Task<List<string>> GetGroupsAsync()
        {
            return (await _mediator.Send(new GetListGroupByGroupNameQuery(null)) ?? throw new NullReferenceException("No group found")).Select(g => g.GroupName).ToList();
        }

        public async Task AddUserToGroupAsync(string username, string groupName)
        {
            var group = await _mediator.Send(new GetSingleGroupByGroupNameQuery(groupName));
            if (group == null)
            {
                group = new ChatGroup { GroupName = groupName };
                await _mediator.Send(new CreateGroupCommand(group));
            }

            if (!(await _mediator.Send(new GetUserGroupsQuery(username, group.Id))).Any())
            {
                await _mediator.Send(new CreateUserGroupCommand(new UserGroup { Username = username, GroupId = group.Id }));
            }
        }

        public async Task<List<string?>?> GetUserGroupsAsync(string username)
        {
            return (await _mediator.Send(new GetUserGroupsQuery(username, null))).Select(ug => ug?.Group.GroupName).ToList();
        }
    }
}
