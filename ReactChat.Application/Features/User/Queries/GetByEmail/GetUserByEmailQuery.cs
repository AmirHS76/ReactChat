using MediatR;
using ReactChat.Core.Entities.User;

namespace ReactChat.Application.Features.User.Queries.GetByEmail
{
    public record GetUserByEmailQuery(string Email) : IRequest<BaseUser?>;
}
