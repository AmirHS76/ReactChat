namespace ReactChat.Application.Interfaces.MessageHub
{
    public interface IMessageHubService
    {
        Task SaveMessageAsync(string? sender, string? recipient, string? message);
    }
}
