using AutoMapper;
using ReactChat.Application.Dtos.MessageDto;
using ReactChat.Application.Interfaces.MessageHistory;
using ReactChat.Infrastructure.Data.UnitOfWork;

namespace ReactChat.Application.Services.MessageHistory
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MessageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<MessageDto> Messages, bool HasMore)> GetMessagesByUsernameAsync(string username, string targetUsername, int pageNum)
        {
            var result = await _unitOfWork.MessageRepository.GetMessagesByUsernameAsync(username, targetUsername, pageNum);
            var messages = _mapper.Map<IEnumerable<MessageDto>>(result.Messages);
            return (messages, result.HasMore);
        }
    }
}
