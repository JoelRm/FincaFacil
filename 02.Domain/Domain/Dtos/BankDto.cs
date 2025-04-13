using System.Text.Json.Serialization;

namespace Domain.Dtos
{
    public class BankDto
    {
        [JsonPropertyName("bankid")]
        public int Id { get; set; }
        [JsonPropertyName("disabled")]
        public int Disabled { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("code")]
        public string? Code { get; set; }
        [JsonPropertyName("bic")]
        public string? Bic { get; set; }
    }
}
