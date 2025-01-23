﻿using MediatR;
using ReactChat.Core.Entities.Message;

namespace ReactChat.Application.Features.Message.Commands
{
    public record CreateMessageCommand(PrivateMessage Message) : IRequest<bool>;
}
