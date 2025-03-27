using Microsoft.EntityFrameworkCore;
using ReactChat.Core.Entities.Chat.Group;
using ReactChat.Core.Entities.User;
using ReactChat.Infrastructure.Data.Context;

namespace ReactChat.Application.Services.ChatServices
{
    public class ChatService
    {
        private readonly UserContext _context;

        public ChatService(UserContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetGroupsAsync()
        {
            return await _context.ChatGroups.Select(g => g.GroupName).ToListAsync();
        }

        public async Task CreateGroupAsync(string groupName)
        {
            if (!await _context.ChatGroups.AnyAsync(g => g.GroupName == groupName))
            {
                _context.ChatGroups.Add(new ChatGroup { GroupName = groupName });
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> AddUserToGroupAsync(string username, string groupName)
        {
            var group = await _context.ChatGroups.FirstOrDefaultAsync(g => g.GroupName == groupName);
            if (group == null) return false;

            if (!await _context.UserGroups.AnyAsync(ug => ug.Username == username && ug.GroupId == group.Id))
            {
                _context.UserGroups.Add(new UserGroup { Username = username, GroupId = group.Id });
                await _context.SaveChangesAsync();
            }

            return true;
        }
    }

}
