using ReactChat.Core.Entities.Login;
using ReactChat.Infrastructure.Repositories.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactChat.Infrastructure.Data.Users
{
    public interface IUserUnitOfWork : IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        Task<BaseUser?> FindUserByUsernameAsync(string username);
    }
}
