using System;

namespace ReactChat.Core.Entities.User
{
    public class UserSession
    {
        public int? Id { get; set; }
        public string? UserId { get; set; }
        public string? UserAgent { get; set; }
        public string? IpAddress { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastActivity { get; set; }
        public bool? IsRevoked { get; set; }
    }
}
