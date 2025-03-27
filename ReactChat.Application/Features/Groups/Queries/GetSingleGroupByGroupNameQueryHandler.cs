using MediatR;
using ReactChat.Core.Entities.Chat.Group;
using ReactChat.Infrastructure.Data.UnitOfWork;

namespace ReactChat.Application.Features.Groups.Queries
{
    public class GetSingleGroupByGroupNameQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetSingleGroupByGroupNameQuery, ChatGroup?>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<ChatGroup?> Handle(GetSingleGroupByGroupNameQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.ChatRepository<ChatGroup>().GetSingleChatGroupByGroupNameAsync(request.GroupName);
        }
    }
}
