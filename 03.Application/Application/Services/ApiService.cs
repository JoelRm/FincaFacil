using AutoMapper;
using MiddlewareService.Domain.Entities;
using MiddlewareService.Domain.Interfaces;
using MiddlewareService.Domain.Models.Request;
using MiddlewareService.Domain.Models.Response;

namespace MiddlewareService.Application.Services{
    public class ApiService{
        private readonly IAuthenticationService _authenticationService;
        private readonly ISearchService _searchService;
        private readonly IUnnaxService _unnaxService;
        private readonly IMapper _mapper;
        public ApiService(IAuthenticationService authenticationService, ISearchService searchService, IUnnaxService unnaxService, IMapper mapper){
            _authenticationService = authenticationService;
            _searchService = searchService;
            _unnaxService = unnaxService;
            _mapper = mapper;
        }

        public async Task<ResponseBase<List<Province>>> GetProvinces(FilterRequest request) 
        {
            var token = await _authenticationService.GetTokenAsync();
            var provinceDto = await _searchService.GetProvincesAsync(token, request);

            var mappedProvinces = _mapper.Map<List<Province>>(provinceDto.Data);

            var response = new ResponseBase<List<Province>>()
            {
                Code = provinceDto.Code,
                Message = provinceDto.Message,
                Data = mappedProvinces
            };
            return response;   
        }

        public async Task<ResponseBase<List<Bank>>> GetBanks(FilterRequest request) 
        {
            var token = await _authenticationService.GetTokenAsync();
            var bankDto = await _searchService.GetBanksAsync(token, request);

            var mappedBanks = _mapper.Map<List<Bank>>(bankDto.Data);

            var response = new ResponseBase<List<Bank>>()
            {
                Code = bankDto.Code,
                Message = bankDto.Message,
                Data = mappedBanks
            };

            return response;   
        }

        public async Task<ResponseBase<Unnax>> InsertBank(BankRequest request) 
        {
            ResponseBase<Unnax> response = new ResponseBase<Unnax>();
            var token = await _authenticationService.GetTokenAsync();
            var Id = await _unnaxService.Insert(token, request);
            response = await _unnaxService.LoginAsync(token,Id);
            return response;   
        }
    }
}