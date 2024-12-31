namespace ReactChat.Presentation.Helpers.HubHelpers
{
    public interface IMessageHubHelper
    {
        Task SaveMessageAsync(string sender, string recipient, string message);
        string GetPrivateGroupName(string user1, string user2);
    }
}
