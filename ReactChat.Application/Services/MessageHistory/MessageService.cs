using AutoMapper;
using ReactChat.Application.Dtos.MessageDto;
using ReactChat.Application.Interfaces.MessageHistory;
using ReactChat.Infrastructure.Data.UnitOfWork;

namespace ReactChat.Application.Services.MessageHistory
{
    public class MessageService : IMessageService
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;
        public MessageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<MessageDto>> GetMessagesByUsernameAsync(string username, string targetUsername)
        {
            var result = await _unitOfWork.MessageRepository.GetMessagesByUsernameAsync(username, targetUsername);
            return _mapper.Map<IEnumerable<MessageDto>>(result);
        }
    }
}
