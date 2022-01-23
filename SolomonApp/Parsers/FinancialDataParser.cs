using System;
using System.Collections.Generic;
using SolomonApp.Models;
using SolomonApp.Parsers.Interfaces;

namespace SolomonApp.Parsers
{
    public class FinancialDataParser : IFinancialDataParser
    {
        // ==  Income Statement KPIs ==

        /*
         Even though some of these metrics are available in the company overview,
         its still necessary to have methods to calculate the metrics for trends
         because the overview is only as of the most recent earnings date
        
         */
        public Dictionary<string, Dictionary<string, decimal>> CalcGrossMarginPercentAllYrs(IncomeStatementList incomeStatementList)
        {
            // save company symbol
            var ticker = incomeStatementList.Symbol;

            // final results dict
            Dictionary<string, Dictionary<string, decimal>> finalResults = new Dictionary<string, Dictionary<string, decimal>>();

            // results dict to save the past 5 years of info (assuming 5 years of data exists)
            Dictionary<string, decimal> gpResults = new Dictionary<string, decimal>();

            foreach (var statement in incomeStatementList.IncomeStatements)
            {
                // get fiscal date ending for the income statement
                string fiscalDateEnding = statement.FiscalDateEnding;

                // get values for GP calc and calculate
                var grossProfitDollars = GetLongAccountValue(statement, "GrossProfitRaw");
                var totalRevenue = GetLongAccountValue(statement, "TotalRevenueRaw");

                // will get zero for final result without one of the operands being in decimal format
                var totalRevenueDec = Convert.ToDecimal(totalRevenue);
                // TODO: fill in edge cases / error checks of where any of these values could be 0
                var grossProfitPerc = grossProfitDollars / totalRevenueDec;

                // TODO: Consider currency edge cases as a part of broader data structure, here it doesn't matter as much considering its a %
                // TODO: consider decimal formatting
                gpResults.Add(fiscalDateEnding, grossProfitPerc);
            }
            finalResults.Add(ticker, gpResults);
            return finalResults;

        }
        // PICKUP 1.22.2022 add helper method for GP calc for an individual statement

        public long GetLongAccountValue(object finDataObject, string accountName)
        {
            string acctBalanceRaw = Convert.ToString(GetPropertyValue(finDataObject, accountName));

            if (acctBalanceRaw == "None")
            {
                // TODO: Consider if there needs to be error handling as this will
                // screw up some of the metrics, probably better to handle in 'parent'
                // KPI parser
                return 0;
            }

            long acctBalance = Convert.ToInt64(acctBalanceRaw);

            return acctBalance;

        }

        public decimal GetDecimalKpiValue(object finDataObject, string kpiName)
        {
            string kpiBalanceRaw = Convert.ToString(GetPropertyValue(finDataObject, kpiName));

            if (kpiBalanceRaw == "None")
            {
                return 0;
            }

            decimal kpiBalance = Convert.ToDecimal(kpiBalanceRaw);

            return kpiBalance;

        }

        // TODO: 
        public object GetPropertyValue(object finDataObject, string propName)
        {
            return finDataObject.GetType().GetProperty(propName).GetValue(finDataObject, null);
        }

        // helper method to eventually format the decimals into strings
        // with set decimal places and percent value
        public string FormatPercentValues(decimal numToConvert)
        {
            string valFormat = String.Format("Value: {0:P2}.", numToConvert);
            return valFormat; 
        }
    }
}
