using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortfolioManager.Models;
using PortfolioManager.Utils;

namespace PortfolioManager
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 3)
                {
                    Console.WriteLine("ERROR: Invalid Args");
                    Console.WriteLine("PortfolioManager.exe [TargetAllocationFile] [VanguardFile] [OutputFile]");
                    return;
                }

                string targetAllocationfile = args[0];
                if (!File.Exists(targetAllocationfile))
                {
                    Console.WriteLine("ERROR: Invalid TargetAllocationFile");
                    return;
                }

                string vanguardFile = args[1];
                if (!File.Exists(vanguardFile))
                {
                    Console.WriteLine("ERROR: Invalid VanguardFile");
                    return;
                }

                string outputFile = args[2];

                
                List<Allocation> allocations = TargetPortfolioUtils.Load(targetAllocationfile);
                List<Account> accounts = VanguardUtils.Load(vanguardFile);
                Dictionary<string, List<Reallocation>> reallocations = AllocationUtils.ReallocateAccounts(accounts,
                    allocations, outputFile);

                ReportUtils.Write(outputFile, accounts, reallocations);

                Console.WriteLine("Done");
            }
            catch (Exception e) 
            {
                Console.WriteLine("ERROR: {0}", e);
            }
        }
    }
}
