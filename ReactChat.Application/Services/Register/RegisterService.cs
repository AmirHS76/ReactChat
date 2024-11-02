using ReactChat.Application.Interfaces.Register;
using ReactChat.Core.Entities.Login;
using ReactChat.Infrastructure.Data;
using ReactChat.Infrastructure.Data.Users;
using ReactChat.Infrastructure.Repositories.Users;

namespace ReactChat.Application.Services.Register
{
    public class RegisterService : IRegisterService
    {
        IUnitOfWork _unitOfWork;
        IUserRepository _userRepository;
        public RegisterService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userRepository = _unitOfWork.UserRepository;
        }
        public async Task<bool> Register(string username, string password, string email)
        {
            if(!await CheckIfUserExist(username))
                return false;
            var hashedPass  = BCrypt.Net.BCrypt.HashPassword(password);
            BaseUser newUser = new BaseUser() { Username = username, Password = hashedPass };
            await _userRepository.AddAsync(newUser);
            return true;
        }
        public async Task<bool> CheckIfUserExist(string username)
        {
            BaseUser? existingUser = await _userRepository.GetUserByUsernameAsync(username);
            if (existingUser != null) return false;
            return true;
        }
    }
}
