using MediatR;
using ReactChat.Core.Entities.User;
using ReactChat.Infrastructure.Data.UnitOfWork;

namespace ReactChat.Application.Features.UserSessions.Commands
{
    public class UpdateUserSessionCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserSessionCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<bool> Handle(UpdateUserSessionCommand request, CancellationToken cancellationToken)
        {
            _unitOfWork.UserRepository<UserSession>().UpdateAsync(request.UserSession);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}