using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactChat.Application.Interfaces.Register
{
    public interface IRegisterService
    {
        Task<bool> Register(string username, string password, string email);
    }
}
