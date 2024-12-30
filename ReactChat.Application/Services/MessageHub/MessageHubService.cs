using ReactChat.Application.Interfaces.MessageHub;
using ReactChat.Core.Entities.Message;
using ReactChat.Infrastructure.Data.UnitOfWork;

namespace ReactChat.Application.Services.MessageHub
{
    public class MessageHubService(IUnitOfWork unitOfWork) : IMessageHubService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task SaveMessageAsync(string? sender, string? recipient, string? message)
        {
            PrivateMessage privateMessage = new()
            {
                Message = message,
                ReceiverName = recipient,
                SenderName = sender,
                MessageDateTime = DateTime.Now.ToString()
            };
            await _unitOfWork.MessageRepository.AddAsync(privateMessage);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
