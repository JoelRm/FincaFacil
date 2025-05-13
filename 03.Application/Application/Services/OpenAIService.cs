using System.Text;
using Newtonsoft.Json;

namespace MiddlewareService.Application.Services{
    public class OpenAIService
{
    private static readonly HttpClient client = new HttpClient();

        public static async Task<string> ObtenerIconoConIA(string concepto, string apiKey)
        {
            var client = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(2)
            };
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var requestBody = new
            {
                model = "gpt-4",
                messages = new[]
                {
                new { role = "system", content = "Eres un asistente útil." },
                new { role = "user", content = $"Asocia un único ícono o emoji para la categoría bancaria '{concepto}'." }
            },
                max_tokens = 10,  // Reducir los tokens para limitar la respuesta
                temperature = 0.7
            };

            var jsonRequest = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            int maxRetries = 3;
            TimeSpan delayBetweenRetries = TimeSpan.FromSeconds(5);

            for (int attempt = 0; attempt < maxRetries; attempt++)
            {
                try
                {
                    var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);

                    // Comprobar si la respuesta fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();

                        // Parsear la respuesta JSON
                        var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseString);
                        var icono = jsonResponse.choices[0].message.content.ToString().Trim();

                        // Si el ícono es una lista de íconos, tomar solo el primero
                        if (icono.Contains(","))
                        {
                            var iconos = icono.Split(",");
                            icono = iconos[0].Trim();
                        }

                        // Si no se encuentra un ícono, asignar uno por defecto
                        return string.IsNullOrWhiteSpace(icono) ? "💼" : icono;
                    }
                    else
                    {
                        // Si la respuesta no es exitosa, lanzar una excepción
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        throw new HttpRequestException($"Error en la respuesta de la API: {errorResponse}");
                    }
                }
                catch (HttpRequestException ex) when (attempt < maxRetries - 1)
                {
                    // Manejo de excepciones para errores temporales (reintentos)
                    Console.WriteLine($"Intento {attempt + 1} fallido: {ex.Message}. Reintentando...");
                    await Task.Delay(delayBetweenRetries);
                }
                catch (Exception ex)
                {
                    // Manejo de otros errores
                    throw new Exception($"Error al obtener el ícono: {ex.Message}", ex);
                }
            }

            throw new Exception("Se agotaron los intentos de reintento.");
        }
    }
}