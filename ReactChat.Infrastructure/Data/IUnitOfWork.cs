using ReactChat.Infrastructure.Repositories;
using ReactChat.Infrastructure.Repositories.Users;

namespace ReactChat.Infrastructure.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : class;
        Task SaveChangesAsync();
        IUserRepository UserRepository { get; }

    }
}
