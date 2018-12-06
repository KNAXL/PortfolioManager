namespace PortfolioManager.Models
{
    public class Allocation
    {
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string Category { get; set; }
        public decimal TargetPercent { get; set; }
        public string InvestmentName { get; set; }
        public string Symbol { get; set; }
    }
}