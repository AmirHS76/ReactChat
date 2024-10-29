namespace ReactChat.Application.Interfaces
{
    internal interface ILoginService
    {
        Task<bool> Authenticate(string username, string password);
    }
}
