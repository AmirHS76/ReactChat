using ReactChat.Core.Entities.User;

namespace ReactChat.Infrastructure.Repositories.User
{
    public interface IUserRepository : IGenericRepository<BaseUser>
    {
        Task<BaseUser?> GetUserByUsernameAsync(string username);
        Task<BaseUser?> GetUserByEmailAsync(string email);
    }
}
