using System.ComponentModel.DataAnnotations;

namespace ReactChat.Dtos.Users
{
    public class UserDto
    {
        [Required]
        [StringLength(40)]
        public required string username { get; set; }
        [Required]
        [EmailAddress]
        public required string email { get; set; }
    }
}
