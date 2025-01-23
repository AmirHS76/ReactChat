using AutoMapper;
using MediatR;
using ReactChat.Application.Dtos.MessageDto;
using ReactChat.Application.Features.Message.Queries;
using ReactChat.Application.Interfaces.MessageHistory;

namespace ReactChat.Application.Services.MessageHistory
{
    public class MessageService(IMediator mediator, IMapper mapper) : IMessageService
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;

        public async Task<(IEnumerable<MessageDTO> Messages, bool HasMore)> GetMessagesByUsernameAsync(string username, string targetUsername, int pageNum)
        {
            var result = await _mediator.Send(new GetMessageByUsernameQuery(username, targetUsername, pageNum));
            var messages = _mapper.Map<IEnumerable<MessageDTO>>(result.Messages);
            return (messages, result.HasMore);
        }
    }
}
