using MiddlewareService.Domain.Entities;
using MiddlewareService.Domain.Models.Request;
using MiddlewareService.Domain.Models.Response;

namespace MiddlewareService.Domain.Interfaces{
    public interface IUnnaxService
    {
        Task<int> Insert(string Token, BankRequest request);
        Task<ResponseBase<Unnax>> LoginAsync(string token, int Id);
    }
}