﻿using MediatR;
using ReactChat.Infrastructure.Data.UnitOfWork;

namespace ReactChat.Application.Features.User.Commands.Update;

public class UpdateUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        _unitOfWork.UserRepository.UpdateAsync(request.User);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}