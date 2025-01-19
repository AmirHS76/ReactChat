using MediatR;
using ReactChat.Core.Entities.User;

namespace ReactChat.Application.Features.Queries
{
    public record GetAllUsersQuery() : IRequest<IEnumerable<BaseUser>>;
}
