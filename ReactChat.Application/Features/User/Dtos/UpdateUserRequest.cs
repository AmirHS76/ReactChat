namespace ReactChat.Application.Features.User.Dtos
{
    public class UpdateUserRequest
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Role { get; set; }
        public bool IsDisabled { get; set; } = false;
    }
}