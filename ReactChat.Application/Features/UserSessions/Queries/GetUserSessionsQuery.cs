using MediatR;
using ReactChat.Core.Entities.User;

namespace ReactChat.Application.Features.UserSessions.Queries
{
    public record GetUserSessionsQuery(UserSession UserSession) : IRequest<List<UserSession>>;

}
