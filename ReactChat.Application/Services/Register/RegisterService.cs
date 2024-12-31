using ReactChat.Application.Interfaces.Register;
using ReactChat.Core.Entities.User;
using ReactChat.Infrastructure.Data.UnitOfWork;

namespace ReactChat.Application.Services.Register
{
    public class RegisterService : IRegisterService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RegisterService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Register(string username, string password, string email)
        {
            if (await CheckIfUserExist(username, email))
                return false;

            var newUser = CreateUser(username, password, email);
            await _unitOfWork.UserRepository.AddAsync(newUser);

            return true;
        }

        private static BaseUser CreateUser(string username, string password, string email)
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
            return await _unitOfWork.UserRepository.GetUserByUsernameAsync(username) != null
                || await _unitOfWork.UserRepository.GetUserByEmailAsync(email) != null;
        }
    }
}
