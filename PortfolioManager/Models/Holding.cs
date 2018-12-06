namespace PortfolioManager.Models
{
    public class Holding
    {
        public string InvestmentName { get; set; }
        public string Symbol { get; set; }
        public decimal Shares { get; set; }
        public decimal SharePrice { get; set; }
        public decimal TotalValue { get; set; }
    }
}