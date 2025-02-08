namespace ReactChat.Application.Interfaces.Register
{
    public interface IRegisterService
    {
        Task<bool> Register(string username, string password, string email, CancellationToken cancellationToken);
    }
}
