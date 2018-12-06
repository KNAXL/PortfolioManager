using System.Dynamic;
using System.Security.Permissions;

namespace PortfolioManager.Models
{
    public class Reallocation : Allocation
    {
        public decimal CurrentPercentage { get; set; }

        public decimal CurrentValue { get; set; }

        public decimal TargetValue { get; set; }
        public string TradeAction { get; set; }
        public decimal TradeQuantity { get; set; }

        public decimal DiffPercent { get; set; }
        public decimal DiffValue{ get; set; }

        public Reallocation()
        {
            
        }

        public Reallocation(Allocation allocation)
        {
            AccountName = allocation.AccountName;
            AccountNumber = allocation.AccountNumber;
            Category = allocation.Category;
            InvestmentName = allocation.InvestmentName;
            Symbol = allocation.Symbol;
            TargetPercent = allocation.TargetPercent;
        }
    }
}