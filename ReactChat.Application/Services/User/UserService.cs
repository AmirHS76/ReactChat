using ReactChat.Application.Interfaces.Users;
using ReactChat.Core.Entities.Login;
using ReactChat.Core.Enums;
using ReactChat.Infrastructure.Data.UnitOfWork;
using System.Data;

namespace ReactChat.Application.Services.Users
{
    public class UserService : IUserService
    {
        IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<BaseUser?> GetUserByUsernameAsync(string? username)
        {
            if (username == null) return null;
            BaseUser? user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            return user;
        }
        public async Task<bool> UpdateUserAsync(int id, string username, string email)
        {
            BaseUser? user;
            if (id == 0)
                user = await GetUserByUsernameAsync(username);
            else
                user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
                return false;
            user.Username = username;
            user.Email = email;
            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<BaseUser>> GetAllUsersAsync()
        {
            IEnumerable<BaseUser> allUsers = await _unitOfWork.UserRepository.GetAllAsync();
            return allUsers;
        }
        public async Task<bool> AddNewUserAsync(string username, string password, string email, string role)
        {
            BaseUser? baseUser;
            baseUser = await GetUserByUsernameAsync(username);
            if (baseUser != null)
                return false;
            baseUser = CreateUser(username, password, email, role);
            await _unitOfWork.UserRepository.AddAsync(baseUser);
            return true;
        }
        private BaseUser CreateUser(string username, string password, string email,string role)
        {
            return new BaseUser
            {
                Username = username,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                Email = email,
                Role = (UserRole)Enum.Parse(typeof(UserRole), role)
            };
        }
    }
}
