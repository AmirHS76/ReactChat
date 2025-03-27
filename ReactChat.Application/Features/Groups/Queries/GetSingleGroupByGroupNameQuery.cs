using MediatR;
using ReactChat.Core.Entities.Chat.Group;

namespace ReactChat.Application.Features.Groups.Queries
{
    public record GetSingleGroupByGroupNameQuery(string? GroupName) : IRequest<ChatGroup?>;
}
