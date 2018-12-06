using System.Collections.Generic;
using System.IO;
using PortfolioManager.Models;

namespace PortfolioManager.Utils
{
    static class VanguardUtils
    {
        public static List<Account> Load(string filePath)
        {
            List<Account> accounts = new List<Account>();
            Account account = null;
            
            // Vanguard Acccounts Holding are at the top of the file seperated by one blank line each.
            //Account Number, Investment Name,Symbol,Shares,Share Price, Total Value
            const string holdingHeader = "Account Number,Investment Name,Symbol,Shares,Share Price,Total Value";
            const string transactionHeader =
                "Account Number,Trade Date,Settlement Date,Transaction Type,Transaction Description,Investment Name,Symbol,Shares,Share Price,Principal Amount,Commission Fees,Net Amount,Accrued Interest,Account Type";
            const string seperator = ",";
            string[] lines = File.ReadAllLines(filePath);
            int seperatorCount = 0;
            foreach (string line in lines)
            {
                if (line.StartsWith(holdingHeader))
                {
                    continue;
                }

                if (line.StartsWith(transactionHeader))
                {
                    break; // Done processing holdinsgs.
                }

                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                if (line.StartsWith(seperator))
                {
                    if (seperatorCount > 0)
                    {
                        // At the end
                        break;
                    }
                    else
                    {
                        account = null;
                        seperatorCount++;
                        continue;
                    }
                }

                // Parse the line.
                string[] values = line.Split(',');
                string accountNumber = values[0];
                string investmentName = values[1];
                string symbol = values[2];
                decimal shares = decimal.Parse(values[3]);
                decimal sharePrice = decimal.Parse(values[4]);
                decimal totalValue = decimal.Parse(values[5]);

                if (account == null || accountNumber != account.AccountNumber)
                {
                    account = new Account()
                    {
                        AccountNumber = accountNumber,
                        Holdings = new List<Holding>()
                    };

                    accounts.Add(account);
                }

                account.Holdings.Add(new Holding
                {
                    InvestmentName = investmentName,
                    Symbol = symbol,
                    Shares = shares,
                    SharePrice = sharePrice,
                    TotalValue = totalValue
                });

            }

            return accounts;
        }
    }
}