using ReactChat.Application.Interfaces.MessageHub;
using ReactChat.Core.Entities.Messages;
using ReactChat.Infrastructure.Data.UnitOfWork;

namespace ReactChat.Application.Services.MessageHub
{
    public class MessageHubService : IMessageHubService
    {
        IUnitOfWork _unitOfWork;
        public MessageHubService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task SaveMessageAsync(string? sender, string? recipient, string? message)
        {
            PrivateMessage privateMessage = new PrivateMessage()
            {
                Message = message,
                ReceiverName = recipient,
                SenderName = sender,
                MessageDateTime = DateTime.Now.ToString()
            };
            await _unitOfWork.Repository<PrivateMessage>().AddAsync(privateMessage);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
