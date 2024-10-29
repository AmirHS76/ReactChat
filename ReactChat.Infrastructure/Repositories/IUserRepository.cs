using ReactChat.Core.Entities.Login;

namespace ReactChat.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<BaseUser?> GetUserByIdAsync(int id);
        Task<BaseUser?> GetUserByUsernameAsync(string username);
        Task<IEnumerable<BaseUser>> GetAllUsersAsync();
        Task CreateUserAsync(BaseUser user);
        Task UpdateUserAsync(BaseUser user);
        Task DeleteUserAsync(int id);
    }
}
