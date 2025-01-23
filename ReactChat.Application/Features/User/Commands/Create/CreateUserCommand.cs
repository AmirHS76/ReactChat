using MediatR;
using ReactChat.Core.Entities.User;

namespace ReactChat.Application.Features.User.Commands.Create
{
    public record CreateUserCommand(BaseUser User) : IRequest<bool>;
}
