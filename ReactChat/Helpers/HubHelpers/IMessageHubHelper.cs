using ReactChat.Core.Enums;

namespace ReactChat.Presentation.Helpers.HubHelpers
{
    public interface IMessageHubHelper
    {
        void SaveMessageAsync(string sender, string recipient, string message);
        string GetPrivateGroupName(string user1, string user2);
        Task<bool> CheckUserAccess(string userName, Accesses access);
    }
}
