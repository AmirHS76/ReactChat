using MediatR;
using ReactChat.Core.Entities.User;
using ReactChat.Infrastructure.Data.UnitOfWork;

namespace ReactChat.Application.Features.User.Queries.GetByEmail
{
    public class GetUserByEmailQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUserByEmailQuery, BaseUser?>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<BaseUser?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.UserRepository.GetUserByEmailAsync(request.Email);
        }
    }
}
