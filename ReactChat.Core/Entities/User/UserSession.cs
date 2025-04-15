using System;

namespace ReactChat.Core.Entities.User
{
    public class UserSession
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public string? UserAgent { get; set; }
        public required string IpAddress { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastActivity { get; set; }
        public bool IsRevoked { get; set; } = false;
    }
}
