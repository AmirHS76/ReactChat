using Microsoft.Extensions.DependencyInjection;
using ReactChat.Application.Interfaces.MessageHub;
using System.Collections.Concurrent;

namespace ReactChat.Application.Services.BackgroundServices
{
    public class MessageProcessingService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentQueue<(string Sender, string Recipient, string Message)> _messageQueue = new ConcurrentQueue<(string, string, string)>();

        public MessageProcessingService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void ProcessMessages()
        {
            while (_messageQueue.TryDequeue(out var message))
            {
                ProcessMessageAsync(message.Sender, message.Recipient, message.Message).GetAwaiter().GetResult();
            }
        }

        public void EnqueueMessage(string sender, string recipient, string message)
        {
            _messageQueue.Enqueue((sender, recipient, message));
        }

        private async Task ProcessMessageAsync(string sender, string recipient, string message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var messageHubService = scope.ServiceProvider.GetRequiredService<IMessageHubService>();
                await messageHubService.SaveMessageAsync(sender, recipient, message);
            }
        }
    }
}
