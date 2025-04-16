using System.Text.Json.Serialization;

namespace Domain.Dtos
{
    public class BankMovementDto
    {
        [JsonPropertyName("movementid")]
        public int MovementId { get; set; }
        [JsonPropertyName("operationdate")]
        public string? Date { get; set; }
        [JsonPropertyName("movementconcept")]
        public string? Concept { get; set; }
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
        [JsonPropertyName("accountbalance")]
        public decimal Total { get; set; }
    }
}
