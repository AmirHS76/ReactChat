using MediatR;
using ReactChat.Application.DTOs;
using ReactChat.Infrastructure.Data.UnitOfWork;

namespace ReactChat.Application.Features.Message.Queries
{
    public class GetMessageByUsernameQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetMessageByUsernameQuery, MessageResultDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<MessageResultDTO> Handle(GetMessageByUsernameQuery request, CancellationToken cancellationToken)
        {
            var (Messages, HasMore) = await _unitOfWork.MessageRepository.GetMessagesByUsernameAsync(request.username, request.targetUsername, request.pageNum, cancellationToken);
            return new MessageResultDTO()
            {
                Messages = Messages,
                HasMore = HasMore
            };
        }
    }
}
