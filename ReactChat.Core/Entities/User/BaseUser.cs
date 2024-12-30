using ReactChat.Core.Enums;

namespace ReactChat.Core.Entities.User
{
    public class BaseUser
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public UserRole Role { get; set; }
    }
}