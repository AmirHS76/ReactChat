using AutoMapper;
using ReactChat.Application.Dtos.MessageDto;
using ReactChat.Application.Interfaces.MessageHistory;
using ReactChat.Infrastructure.Data.UnitOfWork;

namespace ReactChat.Application.Services.MessageHistory
{
    public class MessageService(IUnitOfWork unitOfWork, IMapper mapper) : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<MessageDto>> GetMessagesByUsernameAsync(string username, string targetUsername)
        {
            var result = await _unitOfWork.MessageRepository.GetMessagesByUsernameAsync(username, targetUsername);
            return _mapper.Map<IEnumerable<MessageDto>>(result);
        }
    }
}
