namespace ReactChat.Application.Interfaces.Login
{
    internal interface ILoginService
    {
        Task<bool> Authenticate(string username, string password);
    }
}
