using MediatR;
using ReactChat.Core.Entities.Message;

namespace ReactChat.Application.Features.Message.Queries
{
    public record GetMessageByUsernameQuery(string username, string targetUsername, int pageNum) : IRequest<(IEnumerable<PrivateMessage> Messages, bool HasMore)>;
}
