using ReactChat.Application.Interfaces.Register;
using ReactChat.Core.Entities.Login;
using ReactChat.Infrastructure.Data.UnitOfWork;
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
            if (await CheckIfUserExist(username, email))
                return false;

            var newUser = CreateUser(username, password, email);
            await _userRepository.AddAsync(newUser);

            return true;
        }

        private BaseUser CreateUser(string username, string password, string email)
        {
            return new BaseUser
            {
                Username = username,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                Email = email
            };
        }
        public async Task<bool> CheckIfUserExist(string username, string email)
        {
            return await _userRepository.GetUserByUsernameAsync(username) != null
                || await _userRepository.GetUserByEmailAsync(email) != null;
        }
    }
}
