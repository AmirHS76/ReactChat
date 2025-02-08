using MediatR;
using ReactChat.Infrastructure.Data.UnitOfWork;

namespace ReactChat.Application.Features.Message.Commands
{
    public class CreateMessageCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateMessageCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<bool> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.MessageRepository.AddAsync(request.Message, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
