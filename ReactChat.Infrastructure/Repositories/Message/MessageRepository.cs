using Microsoft.EntityFrameworkCore;
using ReactChat.Core.Entities.Messages;
using ReactChat.Infrastructure.Data.Context;

namespace ReactChat.Infrastructure.Repositories.Message
{
    public class MessageRepository : GenericRepository<PrivateMessage>, IMessageRepository
    {
        private readonly UserContext _context;

        public MessageRepository(UserContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PrivateMessage>?> GetMessagesByUsernameAsync(string username, string targetUsername)
        {
            if (_context?.PrivateMessages == null)
            {
                return null;
            }

            return await _context.PrivateMessages
                .Where(message => (message.SenderName == username && message.ReceiverName == targetUsername) ||
                message.SenderName == targetUsername && message.ReceiverName == username)
                .ToListAsync();
        }
    }
}
