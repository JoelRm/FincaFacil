using System.Text.Json;

namespace MiddlewareService.Infrastructure.Helpers{
    public static class Helper{
        public static string urlPuisearch = "puisearch";
        public static string modelProvince  = "vlupmasterprovinces";
        public static string fieldProvince  = "countryid";
        public static string dataProvince   = "69";


        public static string modelBank  = "vlupmasterbank";
        public static string fieldBank  = "eq";
        public static string dataBank   = "0";

        public static string unnaxInsert = "unnaxbanktoken/insert";
        public static string unnaxLogin = "unnax/login?id=";
        public static string ApplyJsonTemplate(string template, Dictionary<string, object> replacements)
        {
            foreach (var kvp in replacements)
            {
                var placeholder = $"{{{{{kvp.Key}}}}}";

                // Si el valor es una cadena, simplemente usa ToString() para evitar las comillas extra.
                var serializedValue = kvp.Value is string ? kvp.Value.ToString() : JsonSerializer.Serialize(kvp.Value);

                template = template.Replace(placeholder, serializedValue);
            }

            return template;
        }
    }
}