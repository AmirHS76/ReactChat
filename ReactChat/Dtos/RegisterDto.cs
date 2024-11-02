﻿using System.ComponentModel.DataAnnotations;

namespace ReactChat.Dtos
{
    public class RegisterDto
    {
        [Required]
        [StringLength(40)]
        public required string Username { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 8)]
        public required string Password { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
