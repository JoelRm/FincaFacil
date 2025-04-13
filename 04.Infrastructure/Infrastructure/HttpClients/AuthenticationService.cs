using System.Text;
using System.Text.Json;
using MiddlewareService.Domain.Interfaces;
using MiddlewareService.Domain.Models.Request;

namespace MiddlewareService.Infrastructure.HttpClients
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;

        public AuthenticationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<string> GetTokenAsync()
        {
            var url = "https://pre-www.afinkia.com/afinkia-api/login/signin";

            LoginRequest request = new LoginRequest()
            {
                usr = "ffldev.admin",
                password = "24^lD5f%DG)5",
                persistent = false,
                client = "FFL"
            };

            var jsonContent = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            content.Headers.Add("Timezone", "GMT-5");

            // Crear un HttpRequestMessage
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };

            // Agregar el encabezado User-Agent a la solicitud
            httpRequestMessage.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");

            try
            {
                // Enviar la solicitud
                var response = await _httpClient.SendAsync(httpRequestMessage);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();

                    using (JsonDocument doc = JsonDocument.Parse(responseBody))
                    {
                        string token = doc.RootElement.GetProperty("jwt").GetString();
                        return token;
                    }
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error GetToken: {response.StatusCode} - {errorResponse}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception during GetToken: {ex.Message}", ex);
            }
        }

    }
}