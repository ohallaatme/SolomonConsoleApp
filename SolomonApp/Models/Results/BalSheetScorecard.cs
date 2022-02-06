using System;
using System.Collections.Generic;

namespace SolomonApp.Models.Results
{
    public class BalSheetScorecard
    {
        public string CoTicker { get; set; }

        public Dictionary<string, decimal> NetRecResults { get; set; }

        public Dictionary<string, decimal> CashToDebtResults { get; set; }

        public Dictionary<string, decimal> InvToNetEarningsResults { get; set; }
    }
}
