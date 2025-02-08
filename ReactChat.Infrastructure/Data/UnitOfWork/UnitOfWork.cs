using ReactChat.Core.Entities.Message;
using ReactChat.Core.Entities.User;
using ReactChat.Infrastructure.Data.Context;
using ReactChat.Infrastructure.Repositories;
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
        public IUserRepository UserRepository
        {
            get
            {
                if (_repositories.TryGetValue(typeof(BaseUser), out var repository))
                {
                    return (IUserRepository)repository;
                }

                var newUserRepository = new UserRepository(_context);
                _repositories[typeof(BaseUser)] = newUserRepository;
                return newUserRepository;
            }
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
