using ReactChat.Core.Entities.Login;
using ReactChat.Infrastructure.Repositories;
using ReactChat.Infrastructure.Repositories.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactChat.Infrastructure.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : class;
        Task SaveChangesAsync();
        IUserRepository UserRepository { get; }

    }
}
