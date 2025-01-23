using ReactChat.Core.Entities.Message;

namespace ReactChat.Application.DTOs
{
    public class MessageResultDTO
    {
        public IEnumerable<PrivateMessage>? Messages { get; set; }
        public bool HasMore { get; set; }
    }
}
