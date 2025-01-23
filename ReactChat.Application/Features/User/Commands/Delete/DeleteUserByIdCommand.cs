using MediatR;

namespace ReactChat.Application.Features.User.Commands.Delete
{
    public record DeleteUserByIdCommand(int Id) : IRequest<bool>;
}
