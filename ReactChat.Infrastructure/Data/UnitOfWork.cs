﻿using ReactChat.Infrastructure.Data.Users;
using ReactChat.Infrastructure.Repositories.Users;
using System;
using System.Threading.Tasks;

namespace ReactChat.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserContext _context;
        private readonly IUserRepository _userRepository;

        public UnitOfWork(UserContext context)
        {
            _context = context;
            _userRepository = new UserRepository(_context);
        }

        public IUserRepository UserRepository => _userRepository;

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}