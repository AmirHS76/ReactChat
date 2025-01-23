﻿using MediatR;
using ReactChat.Application.Features.User.Commands.Create;
using ReactChat.Application.Features.User.Queries.GetByEmail;
using ReactChat.Application.Features.User.Queries.GetByUsername;
using ReactChat.Application.Interfaces.Register;
using ReactChat.Core.Entities.User;

namespace ReactChat.Application.Services.Register
{
    public class RegisterService(IMediator mediator) : IRegisterService
    {
        private readonly IMediator _mediator = mediator;
        public async Task<bool> Register(string username, string password, string email)
        {
            if (await CheckIfUserExist(username, email))
                return false;

            var newUser = CreateUser(username, password, email);
            await _mediator.Send(new CreateUserCommand(newUser));

            return true;
        }

        private static BaseUser CreateUser(string username, string password, string email)
        {
            return new RegularUser
            {
                Username = username,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                Email = email,
                UserRole = Core.Enums.UserRole.Guest
            };
        }
        public async Task<bool> CheckIfUserExist(string username, string email)
        {
            return await _mediator.Send(new GetUserByUsernameQuery(username)) != null
                || await _mediator.Send(new GetUserByEmailQuery(email)) != null;
        }
    }
}
