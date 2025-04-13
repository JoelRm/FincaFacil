namespace MiddlewareService.Domain.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> GetTokenAsync();
    }
}
