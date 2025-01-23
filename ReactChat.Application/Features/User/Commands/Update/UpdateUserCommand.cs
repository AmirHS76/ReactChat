using MediatR;
using ReactChat.Core.Entities.User;

namespace ReactChat.Application.Features.User.Commands.Update
{
    public record UpdateUserCommand(BaseUser User) : IRequest<bool>;
}
