using MediatR;
using ReactChat.Application.Constants;
using ReactChat.Application.Interfaces.Cache;
using ReactChat.Core.Entities.User;
using ReactChat.Infrastructure.Data.UnitOfWork;

namespace ReactChat.Application.Features.User.Commands.Create
{
    public class CreateUserCommandHandler(IUnitOfWork unitOfWork, ICacheService cacheService) : IRequestHandler<CreateUserCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICacheService _cacheService = cacheService;

        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.UserRepository<BaseUser>().AddAsync(request.User, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _cacheService.RemoveAsync(CacheKeys.AllUsers);
            return true;
        }
    }
}
