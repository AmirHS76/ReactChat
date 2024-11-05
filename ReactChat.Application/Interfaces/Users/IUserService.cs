using ReactChat.Core.Entities.Login;

namespace ReactChat.Application.Interfaces.Users
{
    public interface IUserService
    {
        Task<BaseUser?> GetUserByUsernameAsync(string? username);
        Task<bool> UpdateUserAsync(string username, string email);
        Task<IEnumerable<BaseUser>> GetAllUsersAsync();
    }
}
