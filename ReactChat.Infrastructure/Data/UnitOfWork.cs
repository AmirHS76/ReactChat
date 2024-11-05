using ReactChat.Core.Entities.Login;
using ReactChat.Infrastructure.Repositories;
using ReactChat.Infrastructure.Repositories.Users;
namespace ReactChat.Infrastructure.Data.Users
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserContext _context;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public UnitOfWork(UserContext context)
        {
            _context = context;
        }

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
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
