using MediatR;
using ReactChat.Core.Entities.User;
using ReactChat.Infrastructure.Data.UnitOfWork;

namespace ReactChat.Application.Features.UserSessions.Queries
{
    public class GetUserSessionsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUserSessionsQuery, List<UserSession>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<List<UserSession>> Handle(GetUserSessionsQuery request, CancellationToken cancellationToken)
        {
            if (request.userIp != null && request.UserId != 0)
                return (await _unitOfWork.UserRepository<UserSession>().GetAllAsync(x => x.Id == request.UserId
                && x.IpAddress == request.userIp, cancellationToken)).ToList();
            return (await _unitOfWork.UserRepository<UserSession>().GetAllAsync(null, cancellationToken)).ToList();
        }
    }
}
