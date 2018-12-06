using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortfolioManager.Models;

namespace PortfolioManager.Utils
{
    public static class TargetPortfolioUtils
    {
        public static List<Allocation> Load(string filePath)
        {
            List<Allocation> allocations = new List<Allocation>();

            // AccountName, AccountNumber, Category,TargetPercent,InvestmentName,InvestmentSymbol

            string[] lines = File.ReadAllLines(filePath);
            for(int i = 1; i < lines.Length; ++i)
            {
                string[] tokens = lines[i].Split(',');

                if (tokens.Length == 0 || string.IsNullOrEmpty(tokens[0]))
                {
                    continue;
                }

                Allocation allocation = new Allocation();
                allocation.AccountName = tokens[0];
                allocation.AccountNumber = tokens[1];
                allocation.Category = tokens[2];
                allocation.TargetPercent = decimal.Parse(tokens[3].Replace("%", ""))/(decimal)100.0;
                allocation.InvestmentName = tokens[4];
                allocation.Symbol = tokens[5];

                allocations.Add(allocation);
            }

            return allocations;
        }
    }
}
