using ReactChat.Core.Entities.Login;
using ReactChat.Infrastructure.Data;
using ReactChat.Infrastructure.Data.Users;

namespace ReactChat.Application.Services
{
    public class LoginService
    {
        IUserUnitOfWork _userUnitOfWork;
        public LoginService(IUserUnitOfWork userUnitOfWork)
        {
            _userUnitOfWork = userUnitOfWork;
        }
        public async Task<bool> Authenticate(string username, string password)
        {
            BaseUser? user = await _userUnitOfWork.FindUserByUsernameAsync(username);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
                return true;
            return false;
        }
    }
}
