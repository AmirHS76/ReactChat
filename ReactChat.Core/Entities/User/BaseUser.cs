using ReactChat.Core.Enums;

namespace ReactChat.Core.Entities.User
{
    public class BaseUser
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public UserRole UserRole { get; set; }
        public Accesses Accesses { get; set; } = Accesses.None;

    }
}