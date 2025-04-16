namespace MiddlewareService.Domain.Entities
{
    public class BankMovement
    {
        public int MovementId { get; set; }
        public string? Date { get; set; }
        public string? Concept { get; set; }
        public decimal Amount { get; set; }
        public decimal Total { get; set; }
        public string? Icon { get; set; }
    }
}