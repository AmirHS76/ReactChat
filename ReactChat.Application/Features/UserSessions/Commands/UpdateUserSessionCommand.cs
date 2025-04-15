using MediatR;
using ReactChat.Core.Entities.User;

namespace ReactChat.Application.Features.UserSessions.Commands
{
    public record UpdateUserSessionCommand(UserSession UserSession) : IRequest<bool>;
}
