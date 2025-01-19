using MediatR;
using ReactChat.Core.Entities.User;
using ReactChat.Infrastructure.Data.UnitOfWork;

namespace ReactChat.Application.Features.Queries
{
    public class GetUserByUsernameQueryHandler : IRequestHandler<GetUserByUsernameQuery, BaseUser?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserByUsernameQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseUser?> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.UserRepository.GetUserByUsernameAsync(request.Username);
        }
    }
}
