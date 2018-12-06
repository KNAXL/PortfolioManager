using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortfolioManager.Models;

namespace PortfolioManager.Utils
{
    public static class AllocationUtils
    {
        public static Dictionary<string, List<Reallocation>> ReallocateAccounts(List<Account> accounts, List<Allocation> targetAllocations, string outputFile)
        {
            Dictionary<string, List<Reallocation>> accountAllocations = new Dictionary<string, List<Reallocation>>();

            foreach (Account account in accounts)
            {
                List<Allocation> accountAlloctions = targetAllocations.Where(a => a.AccountNumber == account.AccountNumber).ToList();
                accountAllocations[account.AccountNumber] = ReallocateAccount(account, accountAlloctions);
            }
            return accountAllocations;
            
        }

        public static List<Reallocation> ReallocateAccount(Account account, List<Allocation> accountTargetAllocations)
        {
            decimal accountTotalValue = account.Holdings.Sum(a => a.TotalValue);
            decimal totalTargetPercent = accountTargetAllocations.Sum(a => a.TargetPercent);
            if (totalTargetPercent != 1)
            {
                throw new Exception(string.Format("Account {0} allocations do not sum to 100%!, was {1:p} instead", account.AccountNumber, totalTargetPercent));
            }

            List<Reallocation> reallocations = new List<Reallocation>();

            // Add thet arget allocations
            foreach (Allocation allocation in accountTargetAllocations)
            {
                decimal targetValue = accountTotalValue*allocation.TargetPercent;

                Holding currentHolding = account.Holdings.FirstOrDefault(h => h.Symbol == allocation.Symbol);
                if (currentHolding == null)
                {
                    throw new Exception(string.Format("Account {0} does not hold {1}, cannot allocate without implementing annother data file for seperate prices.", totalTargetPercent));
                }

                Reallocation reallocation = new Reallocation(allocation);

                reallocation.CurrentValue = currentHolding.TotalValue;
                reallocation.CurrentPercentage = currentHolding.TotalValue / accountTotalValue;
                reallocation.TargetValue = targetValue;
                
                reallocation.DiffPercent = reallocation.CurrentPercentage - reallocation.TargetPercent;
                reallocation.DiffValue = reallocation.CurrentValue - reallocation.TargetValue;

                reallocation.TradeQuantity = -(int)reallocation.DiffValue / currentHolding.SharePrice;
                reallocation.TradeAction = reallocation.TradeQuantity > 0 ? "BUY" : "SELL";


                reallocations.Add(reallocation);
            }

            // Take the holdings not in the target allocations to 0.
            foreach (
                Holding holding in account.Holdings.Where(h => accountTargetAllocations.All(a => a.Symbol != h.Symbol)))
            {
                Reallocation reallocation = new Reallocation()
                {
                    AccountNumber = account.AccountNumber,
                    Category = "-",
                    InvestmentName = holding.InvestmentName,
                    Symbol = holding.Symbol,
                    TargetPercent = 0
                };

                reallocation.CurrentValue = holding.TotalValue;
                reallocation.CurrentPercentage = reallocation.CurrentValue / accountTotalValue;
                reallocation.TargetValue = 0;

                reallocation.DiffPercent = reallocation.CurrentPercentage - reallocation.TargetPercent;
                reallocation.DiffValue = reallocation.CurrentValue - reallocation.TargetValue;

                reallocation.TradeQuantity = -(int)reallocation.DiffValue / holding.SharePrice;
                reallocation.TradeAction = reallocation.TradeQuantity > 0 ? "BUY" : "SELL";

                reallocations.Add(reallocation);
            }


            return reallocations;
        } 
    }
}
