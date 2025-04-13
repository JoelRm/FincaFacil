using System.Text.Json.Serialization;

namespace MiddlewareService.Domain.Dtos{
    public class ProvinceDto{
        [JsonPropertyName("provinceid")]
        public int Id { get; set;}

        [JsonPropertyName("value")]
        public string? Description  { get; set;}
    }
}