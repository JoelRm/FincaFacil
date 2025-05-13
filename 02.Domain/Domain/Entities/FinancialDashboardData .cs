namespace MiddlewareService.Domain.Entities{
    public class FinancialDashboardData {
        public List<AnnualExpense>? AnnualExpenses { get; set; }
        public List<SummaryItem>? SummaryData { get; set; }
        public List<ChartItem>? ChartData { get; set; }
        public List<ServicePayment>? ServicePayments { get; set; }
        public List<TotalData>? TotalsData { get; set; }
        public List<BankMovement>? BankMovements { get; set; }
    }

    public class AnnualExpense
    {
        public int Year { get; set; }
        public decimal Value { get; set; }
    }

    public class SummaryItem
    {
        public string? Title { get; set; }
        public string? Value { get; set; }
    }

    public class ChartItem
    {
        public string? Name { get; set; }
        public decimal Ingresos { get; set; }
        public decimal Egresos { get; set; }
    }

    public class ServicePayment
    {
        public string? Name { get; set; }
        public decimal Value { get; set; }
    }

    public class TotalData
    {
        public string? Name { get; set; }
        public decimal Value { get; set; }
    }

}