using MediatR;
using ReactChat.Core.Entities.User;
using ReactChat.Infrastructure.Data.UnitOfWork;

namespace ReactChat.Application.Features.UserSessions.Commands
{
    public class CreateUserSessionCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateUserSessionCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<bool> Handle(CreateUserSessionCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.UserRepository<UserSession>().AddAsync(request.UserSession, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
