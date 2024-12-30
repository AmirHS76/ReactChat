using ReactChat.Core.Entities.Message;

namespace ReactChat.Infrastructure.Repositories.Message
{
    public interface IMessageRepository : IGenericRepository<PrivateMessage>
    {
        Task<IEnumerable<PrivateMessage>?> GetMessagesByUsernameAsync(string username, string targetUsername);
    }
}
