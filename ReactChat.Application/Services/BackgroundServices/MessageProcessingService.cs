using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReactChat.Application.Interfaces.MessageHub;
using System.Collections.Concurrent;

namespace ReactChat.Application.Services.BackgroundServices
{
    public class MessageProcessingService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentQueue<(string Sender, string Recipient, string Message)> _messageQueue = new ConcurrentQueue<(string, string, string)>();

        public MessageProcessingService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                while (_messageQueue.TryDequeue(out var message))
                {
                    await ProcessMessageAsync(message.Sender, message.Recipient, message.Message);
                }
                await Task.Delay(1000, stoppingToken);
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
