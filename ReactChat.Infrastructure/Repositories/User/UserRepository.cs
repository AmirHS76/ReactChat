using Microsoft.EntityFrameworkCore;
using ReactChat.Core.Entities.User;
using ReactChat.Infrastructure.Data.Context;

namespace ReactChat.Infrastructure.Repositories.User
{
    public class UserRepository<T>(UserContext context) : GenericRepository<T>(context), IUserRepository<T> where T : class
    {
        private readonly UserContext _context = context;

        public async Task<BaseUser?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            return await _context.Set<BaseUser>().AsNoTracking().FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
        }

        public async Task<BaseUser?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Set<BaseUser>().AsNoTracking().FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<IEnumerable<UserGroup>> GetUserGroupAsync(string? userName, int? groupId)
        {
            var query = _context.Set<UserGroup>().AsNoTracking()
                .Include(ug => ug.Group).AsQueryable();

            if (userName is not null)
                query = query.Where(x => x.Username == userName);

            if (groupId is not null)
                query = query.Where(x => x.GroupId == groupId);

            var a = await query.ToListAsync();
            return await query.ToListAsync();
        }
    }
}
