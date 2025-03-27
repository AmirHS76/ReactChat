using ReactChat.Core.Entities.Chat.Message;

namespace ReactChat.Application.DTOs
{
    public class MessageResultDTO
    {
        public IEnumerable<PrivateMessage>? Messages { get; set; }
        public bool HasMore { get; set; }
    }
}
