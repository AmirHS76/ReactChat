using ReactChat.Application.Dtos.MessageDto;

namespace ReactChat.Application.Interfaces.MessageHistory
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageDto>> GetMessagesByUsernameAsync(string username, string targetUsername);
    }

}
