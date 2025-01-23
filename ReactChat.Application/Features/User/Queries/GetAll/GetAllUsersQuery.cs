using MediatR;
using ReactChat.Core.Entities.User;

namespace ReactChat.Application.Features.User.Queries.GetAll
{
    public record GetAllUsersQuery() : IRequest<IEnumerable<BaseUser>>;
}
