using MediatR;
using ReactChat.Core.Entities.User;

namespace ReactChat.Application.Features.UserGroups.Commands
{
    public record CreateUserGroupCommand(UserGroup UserGroup) : IRequest<bool>;
}
