using MediatR;
using ReactChat.Core.Entities.User;

namespace ReactChat.Application.Features.UserGroups.Queries.GetUserGroups
{
    public record GetUserGroupsQuery(string? UserName, int? GroupId) : IRequest<IEnumerable<UserGroup?>>;
}
