﻿using ReactChat.Core.Entities.Login;

namespace ReactChat.Application.Interfaces.Users
{
    public interface IUserService
    {
        Task<BaseUser?> GetUserByUsernameAsync(string? username);
        Task<bool> UpdateUserAsync(int Id, string username, string email);
        Task<IEnumerable<BaseUser>?> GetAllUsersAsync();
        Task<bool> AddNewUserAsync(string username, string password, string email, string role);
        Task<bool> DeleteUserByID(int id);
    }
}
