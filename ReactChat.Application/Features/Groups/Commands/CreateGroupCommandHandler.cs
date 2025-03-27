using MediatR;
using ReactChat.Core.Entities.Chat.Group;
using ReactChat.Infrastructure.Data.UnitOfWork;

namespace ReactChat.Application.Features.Groups.Commands
{
    class CreateGroupCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateGroupCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<bool> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.ChatRepository<ChatGroup>().AddAsync(request.ChatGroup, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
