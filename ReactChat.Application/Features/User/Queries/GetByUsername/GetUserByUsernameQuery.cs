using MediatR;
using ReactChat.Core.Entities.User;

namespace ReactChat.Application.Features.User.Queries.GetByUsername
{
    public record GetUserByUsernameQuery(string Username) : IRequest<BaseUser?>;
}
