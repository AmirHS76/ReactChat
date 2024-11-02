using ReactChat.Core.Entities.Login;
using ReactChat.Infrastructure.Data;
using ReactChat.Infrastructure.Data.Users;
using ReactChat.Infrastructure.Repositories.Users;

namespace ReactChat.Application.Services.Login
{
    public class LoginService
    {
        IUnitOfWork _unitOfWork;
        public LoginService(IUnitOfWork userUnitOfWork)
        {
            _unitOfWork = userUnitOfWork;
        }
        public async Task<bool> Authenticate(string username, string password)
        {
            var userRepository = _unitOfWork.UserRepository;
            BaseUser? user = await userRepository.GetUserByUsernameAsync(username);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
                return true;
            return false;
        }
    }
}
