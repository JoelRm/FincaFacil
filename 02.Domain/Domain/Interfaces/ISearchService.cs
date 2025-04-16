using Domain.Dtos;
using MiddlewareService.Domain.Dtos;
using MiddlewareService.Domain.Models.Request;
using MiddlewareService.Domain.Models.Response;

namespace MiddlewareService.Domain.Interfaces{
    public interface ISearchService{
        Task<ResponseBase<List<ProvinceDto>>> GetProvincesAsync(string token, FilterRequest request);
        Task<ResponseBase<List<BankDto>>> GetBanksAsync(string token, FilterRequest request);
        Task<ResponseBase<List<BankMovementDto>>> GetBankMovementAsync(string token, FilterRequest request);
    }
}