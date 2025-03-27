using ReactChat.Core.Entities.Chat.Group;

namespace ReactChat.Infrastructure.Repositories.Chat
{
    public interface IChatRepository<T> : IGenericRepository<T> where T : class
    {
        public Task<ChatGroup?> GetSingleChatGroupByGroupNameAsync(string groupName, CancellationToken cancellationToken = default);
        public Task<List<ChatGroup>?> GetListChatGroupByGroupNameAsync(string groupName, CancellationToken cancellationToken = default);

    }
}
