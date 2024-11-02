using ReactChat.Core.Entities.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactChat.Infrastructure.Repositories.Users
{
    public interface IUserRepository : IGenericRepository<BaseUser>
    {
        Task<BaseUser?> GetUserByUsernameAsync(string username);
        Task<BaseUser?> GetUserByEmailAsync(string email);
    }
}
