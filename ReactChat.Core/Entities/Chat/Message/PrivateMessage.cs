namespace ReactChat.Core.Entities.Chat.Message
{
    public class PrivateMessage
    {
        public int Id { get; set; }
        public string? SenderName { get; set; }
        public string? ReceiverName { get; set; }
        public string? Message { get; set; }
        public string? MessageDateTime { get; set; }
    }
}
