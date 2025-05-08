using System.ComponentModel.DataAnnotations;

namespace ReactChat.Application.Dtos.Authenticate
{
    public class LoginDTO
    {
        [Required]
        public required string Username { get; set; }
        [Required]
        public required string Password { get; set; }
        public string? Captcha { get; set; }
    }
}
