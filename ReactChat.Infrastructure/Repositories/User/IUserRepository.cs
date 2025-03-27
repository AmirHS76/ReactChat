using ReactChat.Core.Entities.User;

namespace ReactChat.Infrastructure.Repositories.User
{
    public interface IUserRepository<T> : IGenericRepository<T> where T : class
    {
        Task<BaseUser?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken);
        Task<BaseUser?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
        Task<IEnumerable<UserGroup>> GetUserGroupAsync(string? userName, int? groupId);
    }
}
