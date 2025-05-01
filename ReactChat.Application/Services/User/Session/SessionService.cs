using MediatR;
using ReactChat.Application.Features.UserSessions.Commands;
using ReactChat.Application.Features.UserSessions.Queries;
using ReactChat.Core.Entities.User;

namespace ReactChat.Application.Services.User.Session
{
    public class SessionService(IMediator mediator)
    {
        private readonly IMediator _mediator = mediator;
        public async Task<List<UserSession>?> GetUserSessions(int userId, CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new GetUserSessionsQuery(new UserSession { UserId = userId.ToString() }), cancellationToken);
        }

        public async Task<bool> RevokeSession(int sessionId, CancellationToken cancellationToken = default)
        {
            var sessionToRevoke = (await _mediator.Send(new GetUserSessionsQuery(new UserSession { Id = sessionId }), cancellationToken)).FirstOrDefault();
            if (sessionToRevoke == null)
                return false;
            sessionToRevoke.IsRevoked = true;
            return await _mediator.Send(new UpdateUserSessionCommand(sessionToRevoke), cancellationToken);
        }

        public async Task<UserSession?> GetCurrentUserSession(int userId, string userIp, CancellationToken cancellationToken = default)
        {
            var userSession = await _mediator.Send(new GetUserSessionsQuery(new UserSession { UserId = userId.ToString(), IpAddress = userIp }), cancellationToken);
            return userSession.Where(x => !x.IsRevoked ?? false).FirstOrDefault();
        }

    }
}
