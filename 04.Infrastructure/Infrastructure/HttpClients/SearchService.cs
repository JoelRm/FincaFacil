using MiddlewareService.Domain.Interfaces;
using MiddlewareService.Infrastructure.Helpers;
using Microsoft.Extensions.Configuration;
using MiddlewareService.Domain.Models.Response;
using System.Text.Json;
using MiddlewareService.Domain.Models.Request;
using MiddlewareService.Domain.Dtos;
using Domain.Dtos;

namespace MiddlewareService.Infrastructure.HttpClients{
    public class SearchService : ISearchService{
        private readonly HttpClient _httpClient;
        private readonly string _url;
        public SearchService(HttpClient httpClient, IConfiguration configuration){
            _httpClient = httpClient;
            _url = configuration["Service:UrlBase"];
        }

        public async Task<ResponseBase<List<ProvinceDto>>> GetProvincesAsync(string token, FilterRequest request)
        {
            ResponseBase<List<ProvinceDto>>? result;

            var replacements = new Dictionary<string, object>
            {
                ["entity"] = Helper.modelProvince,
                ["field"]  = Helper.fieldProvince,
                ["value"]  = Helper.dataProvince,
                ["page"]   = request.Page,
                ["rows"]   = request.Rows
            };
            var response = await SendRequestAsync(replacements, token);
            Console.WriteLine(response);

            var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var provincesResponse = JsonSerializer.Deserialize<JsonElement>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var Data = provincesResponse.GetProperty("data").Deserialize<List<ProvinceDto>>(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            
            result = new ResponseBase<List<ProvinceDto>>
            {
                Code = ((int)response.StatusCode).ToString(),
                Message = response.StatusCode.ToString(),
                Data = Data
            };
            return result;
        }
        public async Task<ResponseBase<List<BankDto>>> GetBanksAsync(string token, FilterRequest request)
        {
            ResponseBase<List<BankDto>>? result;
            var replacements = new Dictionary<string, object>
            {
                ["entity"] = Helper.modelBank,
                ["field"]  = Helper.fieldBank,
                ["value"]  = Helper.dataBank,
                ["page"]   = request.Page,
                ["page"]   = request.Page,
                ["rows"]   = request.Rows
            };

             var response = await SendRequestAsync(replacements, token);
            Console.WriteLine(response);

            var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var provincesResponse = JsonSerializer.Deserialize<JsonElement>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var Data = provincesResponse.GetProperty("data").Deserialize<List<BankDto>>(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            
            result = new ResponseBase<List<BankDto>>
            {
                Code = ((int)response.StatusCode).ToString(),
                Message = response.StatusCode.ToString(),
                Data = Data
            };
            return result;
        }

        public async Task<ResponseBase<List<BankMovementDto>>> GetBankMovementAsync(string token, FilterRequest request)
        {
            ResponseBase<List<BankMovementDto>>? result;
             var replacements = new Dictionary<string, object>
             {
                ["entity"] = Helper.modelBankMovement,
                ["field"]  = Helper.modelBankMovement,
                ["value"]  = Helper.modelBankMovement,
                ["page"]   = request.Page,
                ["page"]   = request.Page,
                ["rows"]   = request.Rows
             };

            var response = await SendRequestAsync(replacements, token);
            Console.WriteLine(response);

            var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var provincesResponse = JsonSerializer.Deserialize<JsonElement>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var Data = provincesResponse.GetProperty("data").Deserialize<List<BankMovementDto>>(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            
            result = new ResponseBase<List<BankMovementDto>>
            {
                Code = ((int)response.StatusCode).ToString(),
                Message = response.StatusCode.ToString(),
                Data = Data
            };
            return result;

        }
        private async Task<HttpResponseMessage> SendRequestAsync(Dictionary<string, object> replacements, string token)
        {
            var jsonContent = HelperSearch.BuildJsonRequest(replacements, AppContext.BaseDirectory);
            var httpRequestMessage = HelperSearch.CreateHttpRequestMessage(_url, Helper.urlPuisearch, token, jsonContent);

            try
            {
                 var response = await _httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);
                 return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception during search: {ex.Message}", ex);
            }
        }
    }
}