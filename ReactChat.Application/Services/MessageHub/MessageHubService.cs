using MediatR;
using ReactChat.Application.Features.Message.Commands;
using ReactChat.Application.Interfaces.MessageHub;
using ReactChat.Core.Entities.Message;

namespace ReactChat.Application.Services.MessageHub
{
    public class MessageHubService(IMediator mediator) : IMessageHubService
    {
        private readonly IMediator _mediator = mediator;

        public async Task SaveMessageAsync(string? sender, string? recipient, string? message)
        {
            PrivateMessage privateMessage = new()
            {
                Message = message,
                ReceiverName = recipient,
                SenderName = sender,
                MessageDateTime = DateTime.Now.ToString()
            };
            await _mediator.Send(new CreateMessageCommand(privateMessage));
        }
    }
}
