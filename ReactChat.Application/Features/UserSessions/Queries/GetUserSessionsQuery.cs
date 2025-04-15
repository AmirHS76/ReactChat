using MediatR;
using ReactChat.Core.Entities.User;

namespace ReactChat.Application.Features.UserSessions.Queries
{
    public record GetUserSessionsQuery(int UserId, string userIp) : IRequest<List<UserSession>>;

}
