using Microsoft.EntityFrameworkCore;
using ReactChat.Core.Entities.Login;
using ReactChat.Infrastructure.Data.Context;

namespace ReactChat.Infrastructure.Repositories.Users
{
    public class UserRepository : GenericRepository<BaseUser>, IUserRepository
    {
        private readonly UserContext _context;

        public UserRepository(UserContext context) : base(context)
        {
            _context = context;
        }

        public async Task<BaseUser?> GetUserByUsernameAsync(string username)
        {
            return await _context.Set<BaseUser>().FirstOrDefaultAsync(u => u.Username == username);
        }
        public async Task<BaseUser?> GetUserByEmailAsync(string email)
        {
            return await _context.Set<BaseUser>().FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
