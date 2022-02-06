using System;
using System.Collections.Generic;

namespace SolomonApp.Models.Results
{
    public class FinalCoComparisonScorecard
    {
        public List<IncStatementScorecard> IncomeStatementScorecards { get; set; }

        public List<BalSheetScorecard> BalSheetScorecards { get; set; }
    }
}
