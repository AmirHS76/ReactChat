using ReactChat.Core.Entities.Login;
using ReactChat.Infrastructure.Repositories.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactChat.Infrastructure.Data.Users
{
    public class UserUnitOfWork : IUserUnitOfWork
    {
        private readonly UserContext _context;
        private readonly IUserRepository _userRepository;

        public UserUnitOfWork(UserContext context)
        {
            _context = context;
            _userRepository = new UserRepository(_context);
        }

        public IUserRepository UserRepository => _userRepository;
        public async Task<BaseUser?> FindUserByUsernameAsync(string username)
        {
            return await _userRepository.GetUserByUsernameAsync(username);
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
