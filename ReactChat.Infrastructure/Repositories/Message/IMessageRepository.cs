using ReactChat.Core.Entities.Messages;

namespace ReactChat.Infrastructure.Repositories.Message
{
    public interface IMessageRepository : IGenericRepository<PrivateMessage>
    {
        Task<IEnumerable<PrivateMessage>?> GetMessagesByUsernameAsync(string username, string targetUsername);
    }
}
