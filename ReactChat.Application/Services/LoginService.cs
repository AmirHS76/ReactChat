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
            IEnumerable<BaseUser> user = await _userRepository.GetAllUsersAsync();
            BaseUser loginedUser = new BaseUser() { Username = username, Password = password };
            if (user.FirstOrDefault(x => x.Username == loginedUser.Username && x.Password == loginedUser.Password) != null)
            {
                return true;
            }
            return false;
        }
    }
}
