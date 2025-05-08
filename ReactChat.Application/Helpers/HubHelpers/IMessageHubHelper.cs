using ReactChat.Core.Enums;

namespace ReactChat.Presentation.Helpers.HubHelpers
{
    public interface IMessageHubHelper
    {
        void SaveMessageAsync(string sender, string recipient, string message);
        string GetPrivateGroupName(string user1, string user2);
        Task<bool> CheckUserAccess(string userName, Accesses access);
        Task AddUserToGroupAsync(string username, string groupName);
        Task<List<string?>?> GetUserGroupsAsync(string username);
        Task<bool> CreateGroupAsync(string groupName, List<string> members);
    }
}
