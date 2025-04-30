using MediatR;
using ReactChat.Core.Entities.User;
using ReactChat.Infrastructure.Data.UnitOfWork;
using System.Data.Entity;

namespace ReactChat.Application.Features.UserSessions.Queries
{
    public class GetUserSessionsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUserSessionsQuery, List<UserSession>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<List<UserSession>> Handle(GetUserSessionsQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_unitOfWork.UserRepository<UserSession>().GetQuery(request.UserSession).AsNoTracking().ToList());
        }
    }
}
