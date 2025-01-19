using MediatR;
using ReactChat.Core.Entities.User;

namespace ReactChat.Application.Features.Queries
{
    public record GetUserByUsernameQuery(string Username) : IRequest<BaseUser?>;
}
