using Microsoft.EntityFrameworkCore;
using ReactChat.Core.Entities.Chat.Group;
using ReactChat.Infrastructure.Data.Context;

namespace ReactChat.Infrastructure.Repositories.Chat
{
    public class ChatRepository<T>(UserContext context) : GenericRepository<T>(context), IChatRepository<T> where T : class
    {
        private readonly UserContext _context = context;
        public async Task<ChatGroup?> GetSingleChatGroupByGroupNameAsync(string? groupName, CancellationToken cancellationToken = default)
        {
            if (groupName == null)
                return await _context.ChatGroups.FirstOrDefaultAsync(cancellationToken);
            return await _context.ChatGroups.Where(x => x.GroupName == groupName).FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<List<ChatGroup>?> GetListChatGroupByGroupNameAsync(string? groupName, CancellationToken cancellationToken = default)
        {
            if (groupName == null)
                return await _context.ChatGroups.ToListAsync(cancellationToken);
            return await _context.ChatGroups.Where(x => x.GroupName == groupName).ToListAsync(cancellationToken);
        }
    }
}
