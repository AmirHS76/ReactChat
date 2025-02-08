using ReactChat.Core.Entities.Message;

namespace ReactChat.Infrastructure.Repositories.Message
{
    public interface IMessageRepository : IGenericRepository<PrivateMessage>
    {
        Task<(IEnumerable<PrivateMessage> Messages, bool HasMore)> GetMessagesByUsernameAsync(string username, string targetUsername, int pageNum, CancellationToken cancellationToken);
    }
}
