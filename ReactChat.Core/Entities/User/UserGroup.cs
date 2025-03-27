using ReactChat.Core.Entities.Chat.Group;

namespace ReactChat.Core.Entities.User
{
    public class UserGroup
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public int GroupId { get; set; }
        public ChatGroup Group { get; set; } = null!;
    }
}
