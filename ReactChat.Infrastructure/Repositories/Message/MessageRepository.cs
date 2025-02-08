using Microsoft.EntityFrameworkCore;
using ReactChat.Core.Entities.Message;
using ReactChat.Infrastructure.Data.Context;

namespace ReactChat.Infrastructure.Repositories.Message
{
    public class MessageRepository(UserContext context) : GenericRepository<PrivateMessage>(context), IMessageRepository
    {
        private readonly UserContext _context = context;
        private static readonly int pageSize = 10;
        public async Task<(IEnumerable<PrivateMessage> Messages, bool HasMore)> GetMessagesByUsernameAsync(string username, string targetUsername, int pageNum, CancellationToken cancellationToken)
        {
            if (_context?.PrivateMessages == null)
            {
                return (Enumerable.Empty<PrivateMessage>(), false);
            }

            var messages = await _context.PrivateMessages
                .AsNoTracking()
                .Where(message => (message.SenderName == username && message.ReceiverName == targetUsername) ||
                                  (message.SenderName == targetUsername && message.ReceiverName == username))
                .OrderByDescending(x => x.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize + 1) // Fetch one extra message to check if there are more
                .ToListAsync(cancellationToken);

            bool hasMore = messages.Count > pageSize;
            if (hasMore)
            {
                messages.RemoveAt(pageSize); // Remove the extra message
            }

            return (messages, hasMore);
        }
    }
}
