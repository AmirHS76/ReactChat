using ReactChat.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactChat.Infrastructure.Data
{
    public interface IUnitOfWork : IDisposable
    {
        Task SaveChangesAsync();
    }
}
