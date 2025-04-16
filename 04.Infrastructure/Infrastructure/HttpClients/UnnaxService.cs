using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using MiddlewareService.Domain.Entities;
using MiddlewareService.Domain.Interfaces;
using MiddlewareService.Domain.Models.Request;
using MiddlewareService.Domain.Models.Response;
using MiddlewareService.Infrastructure.Helpers;

namespace MiddlewareService.Infrastructure.HttpClients{
    public class UnnaxService : IUnnaxService{
        private readonly HttpClient _httpClient;
        private readonly string _url;
        public UnnaxService(HttpClient httpClient, IConfiguration configuration){
            _httpClient = httpClient;
            _url = configuration["Service:UrlBase"];
        }
        public async Task<int> Insert(string Token, BankRequest request)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Timezone", "GMT-5");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(_url + Helper.unnaxInsert, content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            // Deserializar solo el Id correctamente
            using (JsonDocument doc = JsonDocument.Parse(responseBody))
            {
                int Id = doc.RootElement.GetProperty("id").GetInt32();
                return Id;
            }
        }
        public async Task<ResponseBase<Unnax>> LoginAsync(string token, int Id)
        {
            _httpClient.DefaultRequestHeaders.Add("Timezone", "GMT-5");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(_url + Helper.unnaxLogin + Id);
            if (response.IsSuccessStatusCode)
            {
                var unnaxUrl = await response.Content.ReadAsStringAsync();
                return new ResponseBase<Unnax>(){
                     Code = ((int)response.StatusCode).ToString(),
                    Message = response.StatusCode.ToString(),
                    Data = new Unnax()
                    {
                        widget = unnaxUrl
                    }
                };
            }
            else
            {
                return new ResponseBase<Unnax>()
                {
                    Code = ((int)response.StatusCode).ToString(),
                    Message = response.StatusCode.ToString(),
                    Data = null
                };
            } 
        }
    }
}