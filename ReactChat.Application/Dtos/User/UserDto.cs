using ReactChat.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ReactChat.Dtos.Users
{
    public class UserDto
    {
        public int? Id { get; set; }
        [Required]
        [StringLength(40)]
        public required string Username { get; set; }
        public string? Password { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        public string Role { get; set; } = UserRole.Guest.ToString();
    }
}
