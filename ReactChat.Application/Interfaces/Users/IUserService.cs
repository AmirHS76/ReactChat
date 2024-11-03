using ReactChat.Core.Entities.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactChat.Application.Interfaces.Users
{
    public interface IUserService
    {
        Task<BaseUser?> GetUserByUsernameAsync(string? username);
        Task<bool> UpdateUserAsync(string username, string email);
    }
}
