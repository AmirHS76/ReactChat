using ReactChat.Core.Entities.Chat.Message;
using ReactChat.Infrastructure.Data.Context;
using ReactChat.Infrastructure.Repositories;
using ReactChat.Infrastructure.Repositories.Chat;
using ReactChat.Infrastructure.Repositories.Message;
using ReactChat.Infrastructure.Repositories.User;
namespace ReactChat.Infrastructure.Data.UnitOfWork
{
    public class UnitOfWork(UserContext context) : IUnitOfWork
    {
        private readonly UserContext _context = context;
        private readonly Dictionary<Type, object> _repositories = [];

        public IGenericRepository<T> Repository<T>() where T : class
        {
            if (_repositories.TryGetValue(typeof(T), out var repository))
                return (IGenericRepository<T>)repository;

            var newRepository = new GenericRepository<T>(_context);
            _repositories[typeof(T)] = newRepository;
            return newRepository;
        }
        public IUserRepository<T> UserRepository<T>() where T : class
        {
            return new UserRepository<T>(_context);
        }
        public IMessageRepository MessageRepository
        {
            get
            {
                if (_repositories.TryGetValue(typeof(PrivateMessage), out var repository))
                {
                    return (IMessageRepository)repository;
                }

                var newMessageRepository = new MessageRepository(_context);
                _repositories[typeof(PrivateMessage)] = newMessageRepository;
                return newMessageRepository;
            }
        }
        public IChatRepository<T> ChatRepository<T>() where T : class
        {
            return new ChatRepository<T>(_context);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _context.Dispose();
        }
    }
}
