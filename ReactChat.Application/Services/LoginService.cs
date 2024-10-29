using ReactChat.Core.Entities.Login;
using ReactChat.Infrastructure.Repositories;

namespace ReactChat.Application.Services
{
    public class LoginService
    {
        IUserRepository _userRepository;
        public LoginService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<bool> Authenticate(string username, string password)
        {
            BaseUser? user = await _userRepository.GetUserByUsernameAsync(username);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
                return true;
            return false;
        }
    }
}
