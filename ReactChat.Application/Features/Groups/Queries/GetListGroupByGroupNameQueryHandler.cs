using MediatR;
using ReactChat.Core.Entities.Chat.Group;
using ReactChat.Infrastructure.Data.UnitOfWork;

namespace ReactChat.Application.Features.Groups.Queries
{
    public class GetListGroupByGroupNameQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetListGroupByGroupNameQuery, IEnumerable<ChatGroup?>?>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<IEnumerable<ChatGroup?>?> Handle(GetListGroupByGroupNameQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.ChatRepository<ChatGroup>().GetListChatGroupByGroupNameAsync(request.GroupName, cancellationToken);
        }
    }
}
