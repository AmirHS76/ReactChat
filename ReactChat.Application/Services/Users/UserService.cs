using ReactChat.Application.Interfaces.Users;
using ReactChat.Core.Entities.Login;
using ReactChat.Infrastructure.Data;

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
        public async Task<bool> UpdateUserAsync(string username, string email)
        {
            BaseUser? user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            if (user == null)
                return false;
            user.Email = email;
            await _unitOfWork.Repository<BaseUser>().UpdateAsync(user);
            return true;
        }
        public async Task<IEnumerable<BaseUser>> GetAllUsersAsync()
        {
            IEnumerable<BaseUser> allUsers = await _unitOfWork.Repository<BaseUser>().GetAllAsync();
            return allUsers;
        }
    }
}
