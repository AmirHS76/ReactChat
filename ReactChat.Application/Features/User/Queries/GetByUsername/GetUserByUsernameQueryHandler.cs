using MediatR;
using ReactChat.Core.Entities.User;
using ReactChat.Infrastructure.Data.UnitOfWork;

namespace ReactChat.Application.Features.User.Queries.GetByUsername
{
    public class GetUserByUsernameQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUserByUsernameQuery, BaseUser?>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<BaseUser?> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.UserRepository.GetUserByUsernameAsync(request.Username);
        }
    }
}
