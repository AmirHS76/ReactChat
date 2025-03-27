using MediatR;
using ReactChat.Core.Entities.Chat.Group;

namespace ReactChat.Application.Features.Groups.Commands
{
    public record CreateGroupCommand(ChatGroup ChatGroup) : IRequest<bool>;
}
