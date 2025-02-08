using ReactChat.Application.Dtos.MessageDto;

namespace ReactChat.Application.Interfaces.MessageHistory
{
    public interface IMessageService
    {
        Task<(IEnumerable<MessageDTO> Messages, bool HasMore)> GetMessagesByUsernameAsync(string username, string targetUsername, int pageNum, CancellationToken cancellationToken);
    }

}
