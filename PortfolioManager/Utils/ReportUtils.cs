using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortfolioManager.Models;

namespace PortfolioManager.Utils
{
    public static class ReportUtils
    {
        public static void Write(string outputFile, List<Account> accounts, Dictionary<string, List<Reallocation>> reallocations)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Generated: {0} {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
            sb.AppendLine();
            sb.AppendLine();

            foreach (Account account in accounts)
            {
                sb.AppendFormat("Account: {0} ({1})", account.AccountNumber, reallocations[account.AccountNumber][0].AccountName);
                sb.AppendLine();
                sb.AppendLine(
                    "Category,InvestmentName,Symbol,TargetPercent,TargetValue,CurrentPercentage,CurrentValue,DiffPercent,DiffValue,TradeAction,TradeQuantity");

                foreach (Reallocation reallocation in reallocations[account.AccountNumber].OrderBy(r => r.InvestmentName))
                {
                    sb.AppendFormat("{0},{1},{2},{3:P2},{4:F2},{5:P2},{6:F2},{7:P2},{8:F2},{9},{10}", reallocation.Category,
                        reallocation.InvestmentName, reallocation.Symbol,
                        reallocation.TargetPercent, reallocation.TargetValue,
                        reallocation.CurrentPercentage, reallocation.CurrentValue,
                        reallocation.DiffPercent, reallocation.DiffValue,
                        reallocation.TradeAction, reallocation.TradeQuantity);
                    sb.AppendLine();
                }

                sb.AppendLine();
                sb.AppendFormat("{0},{1},{2},{3:P2},{4:F2},{5:P2},{6:F2},{7:P2},{8:F2},{9},{10}", "TOTAL",
                    "-","-",
                    reallocations[account.AccountNumber].Sum(r => r.TargetPercent),
                    reallocations[account.AccountNumber].Sum(r => r.TargetValue),
                    reallocations[account.AccountNumber].Sum(r => r.CurrentPercentage),
                    reallocations[account.AccountNumber].Sum(r => r.CurrentValue),
                    reallocations[account.AccountNumber].Sum(r => r.DiffPercent),
                    reallocations[account.AccountNumber].Sum(r => r.DiffValue),
                    "-", "-");
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine();
            }

            File.WriteAllText(outputFile, sb.ToString());
        }
    }
}
