using MediatR;
using ReactChat.Core.Entities.User;

namespace ReactChat.Application.Features.User.Queries.GetById
{
    public record GetUserByIdQuery(int Id) : IRequest<BaseUser?>;
}
