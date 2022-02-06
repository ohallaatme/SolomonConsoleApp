using System;
using System.Collections.Generic;

namespace SolomonApp.Models.Results
{
    public class IncStatementScorecard
    {
        public string CoTicker { get; set; }
    
        public Dictionary<string, decimal> GrossProfitResults { get; set; }

        public Dictionary<string, decimal> SgaResults { get; set; }

        public Dictionary<string, decimal> IntExpResults { get; set; }

        public Dictionary<string, decimal> TaxExpResults { get; set; }
    }
}
