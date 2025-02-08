using ReactChat.Infrastructure.Repositories.Message;
using ReactChat.Infrastructure.Repositories.User;

namespace ReactChat.Infrastructure.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task SaveChangesAsync(CancellationToken cancellationToken);
        IUserRepository UserRepository { get; }
        IMessageRepository MessageRepository { get; }

    }
}
