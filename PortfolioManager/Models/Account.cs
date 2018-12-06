using System.Collections.Generic;

namespace PortfolioManager.Models
{
    public class Account
    {
        public string AccountNumber { get; set; }
        public List<Holding> Holdings { get; set; }
    }
}