using System.ComponentModel.DataAnnotations;

namespace ReactChat.Dtos.Authenticate
{
    public class LoginDto
    {
        [Required]
        public required string username { get; set; }
        [Required]
        public required string password { get; set; }
    }
}
