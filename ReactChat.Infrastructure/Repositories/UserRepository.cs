using Microsoft.EntityFrameworkCore;
using ReactChat.Core.Entities.Login;
using ReactChat.Infrastructure.Data;

namespace ReactChat.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;

        public UserRepository(UserContext context)
        {
            _context = context;
        }

        public async Task<BaseUser?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<BaseUser?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        }
        public async Task<IEnumerable<BaseUser>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task CreateUserAsync(BaseUser user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(BaseUser user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await GetUserByIdAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
