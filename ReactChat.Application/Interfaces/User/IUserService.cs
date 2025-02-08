using ReactChat.Core.Entities.User;

namespace ReactChat.Application.Interfaces.User
{
    public interface IUserService
    {
        Task<BaseUser?> GetUserByUsernameAsync(string? username, CancellationToken cancellationToken = default);
        Task<bool> UpdateUserAsync(int Id, string username, string email, CancellationToken cancellationToken);
        Task<IEnumerable<BaseUser>?> GetAllUsersAsync(CancellationToken cancellationToken);
        Task<bool> AddNewUserAsync(string username, string password, string email, string role, CancellationToken cancellationToken);
        Task<bool> DeleteUserByID(int id, CancellationToken cancellationToken);
    }
}
