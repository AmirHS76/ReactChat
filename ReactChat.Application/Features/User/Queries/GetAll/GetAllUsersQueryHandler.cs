using MediatR;
using ReactChat.Core.Entities.User;
using ReactChat.Infrastructure.Data.UnitOfWork;

namespace ReactChat.Application.Features.User.Queries.GetAll
{
    public class GetAllUsersQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllUsersQuery, IEnumerable<BaseUser>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<BaseUser>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.UserRepository.GetAllAsync(cancellationToken);
        }
    }
}
