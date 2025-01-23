namespace ReactChat.Application.Dtos.MessageDto
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public string Sender { get; set; } = default!;
        public string Recipient { get; set; } = default!;
        public string Content { get; set; } = default!;
        public DateTime Timestamp { get; set; }
    }

}
