using MediatR;
using ReactChat.Application.Constants;
using ReactChat.Application.Interfaces.Cache;
using ReactChat.Infrastructure.Data.UnitOfWork;

namespace ReactChat.Application.Features.User.Commands.Create
{
    public class CreateUserCommandHandler(IUnitOfWork unitOfWork, ICacheService cacheService) : IRequestHandler<CreateUserCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICacheService _cacheService = cacheService;

        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.UserRepository.AddAsync(request.User);
            await _unitOfWork.SaveChangesAsync();
            await _cacheService.RemoveAsync(CacheKeys.AllUsers);
            return true;
        }
    }
}
