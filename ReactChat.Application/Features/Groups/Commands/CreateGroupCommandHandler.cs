using MediatR;
using ReactChat.Core.Entities.Chat.Group;
using ReactChat.Infrastructure.Data.UnitOfWork;

namespace ReactChat.Application.Features.Groups.Commands
{
    class CreateGroupCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateGroupCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<int> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.ChatRepository<ChatGroup>().AddAsync(request.ChatGroup, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return (await _unitOfWork.ChatRepository<ChatGroup>().GetSingleChatGroupByGroupNameAsync(request.ChatGroup.GroupName) ?? new ChatGroup()).Id;
        }
    }
}
