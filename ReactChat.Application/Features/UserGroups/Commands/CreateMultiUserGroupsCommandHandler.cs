using MediatR;
using ReactChat.Core.Entities.User;
using ReactChat.Infrastructure.Data.UnitOfWork;

namespace ReactChat.Application.Features.UserGroups.Commands
{
    class CreateMultiUserGroupsCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateMultiUserGroupsCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<bool> Handle(CreateMultiUserGroupsCommand request, CancellationToken cancellationToken)
        {
            foreach (string user in request.Users)
            {
                await _unitOfWork.UserRepository<UserGroup>().AddAsync(new UserGroup { Username = user, GroupId = request.GroupId }, cancellationToken);
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
