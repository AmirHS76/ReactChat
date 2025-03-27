using MediatR;
using ReactChat.Core.Entities.User;
using ReactChat.Infrastructure.Data.UnitOfWork;

namespace ReactChat.Application.Features.UserGroups.Commands
{
    class CreateUserGroupCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateUserGroupCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<bool> Handle(CreateUserGroupCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.UserRepository<UserGroup>().AddAsync(request.UserGroup, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
