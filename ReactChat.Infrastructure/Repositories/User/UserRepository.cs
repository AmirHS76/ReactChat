using Microsoft.EntityFrameworkCore;
using ReactChat.Core.Entities.User;
using ReactChat.Infrastructure.Data.Context;

namespace ReactChat.Infrastructure.Repositories.User
{
    public class UserRepository(UserContext context) : GenericRepository<BaseUser>(context), IUserRepository
    {
        private readonly UserContext _context = context;

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
