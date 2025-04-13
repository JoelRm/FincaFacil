using System.Text;

namespace MiddlewareService.Infrastructure.Helpers{
    public static class HelperSearch{
        public static string BuildJsonRequest(Dictionary<string, object> replacements, string baseDirectory)
        {
            var fileName = "PuiSearchRequest.json";
            var relativePath = Path.Combine("JsonRequest", fileName);
            var fullPath = Path.Combine(baseDirectory, relativePath);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"No se encontró el archivo de plantilla: {fullPath}");

            var jsonTemplate = File.ReadAllText(fullPath);

            return Helper.ApplyJsonTemplate(jsonTemplate, replacements);
        }
        public static HttpRequestMessage CreateHttpRequestMessage(string urlBase, string endpoint, string token, string jsonContent)
        {
            var url = new Uri(new Uri(urlBase), endpoint);

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };

            httpRequestMessage.Headers.Add("Authorization", $"Bearer {token}");
            httpRequestMessage.Headers.Add("Timezone", "GMT-5");
            httpRequestMessage.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");

            return httpRequestMessage;
        }
    }
}