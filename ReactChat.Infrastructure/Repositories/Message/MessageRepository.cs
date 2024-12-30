using Microsoft.EntityFrameworkCore;
using ReactChat.Core.Entities.Message;
using ReactChat.Infrastructure.Data.Context;

namespace ReactChat.Infrastructure.Repositories.Message
{
    public class MessageRepository(UserContext context) : GenericRepository<PrivateMessage>(context), IMessageRepository
    {
        private readonly UserContext _context = context;

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
