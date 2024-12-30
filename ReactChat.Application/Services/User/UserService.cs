using ReactChat.Application.Interfaces.Cache;
using ReactChat.Application.Interfaces.User;
using ReactChat.Core.Entities.User;
using ReactChat.Core.Enums;
using ReactChat.Infrastructure.Data.UnitOfWork;

namespace ReactChat.Application.Services.User
{
    public class UserService(IUnitOfWork unitOfWork, ICacheService cacheService) : IUserService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICacheService _cacheService = cacheService;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(5);

        public async Task<BaseUser?> GetUserByUsernameAsync(string? username)
        {
            if (username == null) return null;
            BaseUser? user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            return user;
        }

        public async Task<bool> UpdateUserAsync(int id, string username, string email)
        {
            BaseUser? user = id == 0 ? await GetUserByUsernameAsync(username) : await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
                return false;
            user.Username = username;
            user.Email = email;
            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<BaseUser>?> GetAllUsersAsync()
        {
            var cacheKey = "AllUsers";
            var cachesUsers = await _cacheService.GetAsync<IEnumerable<BaseUser>>(cacheKey);
            if (cachesUsers != null)
                return cachesUsers;

            IEnumerable<BaseUser> allUsers = await _unitOfWork.UserRepository.GetAllAsync();
            if (allUsers != null)
                await _cacheService.SetAsync(cacheKey, allUsers, _cacheExpiration);

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

        private static BaseUser CreateUser(string username, string password, string email, string role)
        {
            return new BaseUser
            {
                Username = username,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                Email = email,
                Role = (UserRole)Enum.Parse(typeof(UserRole), role)
            };
        }

        public async Task<bool> DeleteUserByID(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
                return false;
            await _unitOfWork.UserRepository.DeleteAsync(id);
            return true;
        }
    }
}
