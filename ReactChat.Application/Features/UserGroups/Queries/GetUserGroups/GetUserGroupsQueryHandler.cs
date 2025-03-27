using MediatR;
using ReactChat.Core.Entities.User;
using ReactChat.Infrastructure.Data.UnitOfWork;

namespace ReactChat.Application.Features.UserGroups.Queries.GetUserGroups
{
    class GetUserGroupsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUserGroupsQuery, IEnumerable<UserGroup?>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<UserGroup?>> Handle(GetUserGroupsQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.UserRepository<BaseUser>().GetUserGroupAsync(request.UserName, request.GroupId);
        }
    }
}
