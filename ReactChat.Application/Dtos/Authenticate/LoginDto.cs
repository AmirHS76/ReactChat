using System.ComponentModel.DataAnnotations;

namespace ReactChat.Application.Dtos.Authenticate
{
    public class LoginDto
    {
        [Required]
        public required string Username { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
