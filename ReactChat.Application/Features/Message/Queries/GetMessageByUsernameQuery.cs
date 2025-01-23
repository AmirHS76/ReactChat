using MediatR;
using ReactChat.Application.DTOs;

namespace ReactChat.Application.Features.Message.Queries
{
    public record GetMessageByUsernameQuery(string username, string targetUsername, int pageNum) : IRequest<MessageResultDTO>;
}
