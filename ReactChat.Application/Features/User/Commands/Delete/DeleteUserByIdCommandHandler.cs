using MediatR;
using ReactChat.Infrastructure.Data.UnitOfWork;

namespace ReactChat.Application.Features.User.Commands.Delete
{
    public class DeleteUserByIdCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserByIdCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<bool> Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.UserRepository.DeleteAsync(request.Id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
