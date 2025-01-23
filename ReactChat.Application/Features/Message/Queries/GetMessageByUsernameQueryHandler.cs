using MediatR;
using ReactChat.Core.Entities.Message;
using ReactChat.Infrastructure.Data.UnitOfWork;

namespace ReactChat.Application.Features.Message.Queries
{
    public class GetMessageByUsernameQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetMessageByUsernameQuery, (IEnumerable<PrivateMessage> Messages, bool HasMore)>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<(IEnumerable<PrivateMessage> Messages, bool HasMore)> Handle(GetMessageByUsernameQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.MessageRepository.GetMessagesByUsernameAsync(request.username, request.targetUsername, request.pageNum);
        }
    }
}
