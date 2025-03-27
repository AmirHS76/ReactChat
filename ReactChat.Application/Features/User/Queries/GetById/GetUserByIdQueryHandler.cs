using MediatR;
using ReactChat.Core.Entities.User;
using ReactChat.Infrastructure.Data.UnitOfWork;

namespace ReactChat.Application.Features.User.Queries.GetById
{
    public class GetUserByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUserByIdQuery, BaseUser?>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<BaseUser?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.UserRepository<BaseUser>().GetByIdAsync(request.Id, cancellationToken);
        }
    }
}
