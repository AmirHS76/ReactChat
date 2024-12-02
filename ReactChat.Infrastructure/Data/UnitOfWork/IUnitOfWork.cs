using ReactChat.Infrastructure.Repositories;
using ReactChat.Infrastructure.Repositories.Message;
using ReactChat.Infrastructure.Repositories.Users;

namespace ReactChat.Infrastructure.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task SaveChangesAsync();
        IUserRepository UserRepository { get; }
        IMessageRepository MessageRepository { get; }

    }
}
