using AutoMapper;
using MediatR;
using ReactChat.Application.Constants;
using ReactChat.Application.Dtos.MessageDto;
using ReactChat.Application.DTOs;
using ReactChat.Application.Features.Message.Queries;
using ReactChat.Application.Interfaces.Cache;
using ReactChat.Application.Interfaces.MessageHistory;

namespace ReactChat.Application.Services.MessageHistory
{
    public class MessageService(IMediator mediator, IMapper mapper, ICacheService cacheService) : IMessageService
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly ICacheService _cacheService = cacheService;

        public async Task<(IEnumerable<MessageDto> Messages, bool HasMore)> GetMessagesByUsernameAsync(string username, string targetUsername, int pageNum)
        {
            var result = await _cacheService.GetAsync<MessageResultDTO>(CacheKeys.Messages + username + targetUsername + pageNum);
            if (result?.Messages == null)
            {
                result = await _mediator.Send(new GetMessageByUsernameQuery(username, targetUsername, pageNum));
                await _cacheService.SetAsync<MessageResultDTO>(CacheKeys.Messages + username + targetUsername + pageNum, result, TimeSpan.FromSeconds(600));
            }
            var messages = _mapper.Map<IEnumerable<MessageDTO>>(result.Messages);
            return (messages, result.HasMore);
        }
    }
}
