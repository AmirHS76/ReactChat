using ReactChat.Infrastructure.Repositories.Chat;
using ReactChat.Infrastructure.Repositories.Message;
using ReactChat.Infrastructure.Repositories.User;

namespace ReactChat.Infrastructure.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task SaveChangesAsync(CancellationToken cancellationToken);
        IUserRepository<T> UserRepository<T>() where T : class;
        IMessageRepository MessageRepository { get; }
        IChatRepository<T> ChatRepository<T>() where T : class;
    }
}
