using MediatR;
using ReactChat.Core.Enums;

namespace ReactChat.Application.Features.Commands
{
    public record CreateUserCommand(string Username, string Password, string Email, UserRole UserRole) : IRequest<bool>;
}
