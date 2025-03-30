using MediatR;

namespace ReactChat.Application.Features.UserGroups.Commands
{
    public record CreateMultiUserGroupsCommand(int GroupId, List<string> Users) : IRequest<bool>;

}
