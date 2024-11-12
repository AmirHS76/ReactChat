using System.ComponentModel.DataAnnotations;

namespace ReactChat.Dtos
{
    public class RegisterDto
    {
        [Required]
        [StringLength(40)]
        public required string Username { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Password must be between 8 to 30 characters")]
        public required string Password { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
    }
}
