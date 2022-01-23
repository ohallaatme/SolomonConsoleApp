using System;
using System.Collections.Generic;
using SolomonApp.Models;
using SolomonApp.Parsers.Interfaces;

namespace SolomonApp.Parsers
{
    public class FinancialDataParser : IFinancialDataParser
    {
        #region "Income Statement KPIs"

        /*
         Even though some of these metrics are available in the company overview,
         its still necessary to have methods to calculate the metrics for trends
         because the overview is only as of the most recent earnings date
        
         */
        /// <summary>
        /// Get all of the historical GM %s for a company, 5 years is standard if available
        /// </summary>
        /// <param name="incomeStatementList"></param>
        /// <returns></returns>
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
                long grossProfitDollars = GetLongAccountValue(statement, "GrossProfitRaw");
                long totalRevenue = GetLongAccountValue(statement, "TotalRevenueRaw");

                // TODO: fill in edge cases / error checks of where any of these values could be 0
                decimal grossProfitPerc = CalcGmPerc(grossProfitDollars, totalRevenue);

                // TODO: Consider currency edge cases as a part of broader data structure, here it doesn't matter as much considering its a %
                gpResults.Add(fiscalDateEnding, grossProfitPerc);
            }
            finalResults.Add(ticker, gpResults);
            return finalResults;

        }

        /// <summary>
        /// Get all of the historical SGA as a % of GP KPIs for a company, 5 years is standard if available
        /// </summary>
        /// <param name="incomeStatementList"></param>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, decimal>> CalcSgaPercentAllYrs(IncomeStatementList incomeStatementList)
        {
            var ticker = incomeStatementList.Symbol;

            Dictionary<string, Dictionary<string, decimal>> finalResults = new Dictionary<string, Dictionary<string, decimal>>();

            Dictionary<string, decimal> sgaResults = new Dictionary<string, decimal>();

            foreach(var statement in incomeStatementList.IncomeStatements)
            {
                string fiscalDateEnding = statement.FiscalDateEnding;

                long grossProfitDollars = GetLongAccountValue(statement, "GrossProfitRaw");
                long sgaExpenseDollars = GetLongAccountValue(statement, "SellingGeneralAndAdministrativeRaw");

                decimal sgaPercent = CalcSgaPerc(sgaExpenseDollars, grossProfitDollars);

                sgaResults.Add(fiscalDateEnding, sgaPercent);
            }

            finalResults.Add(ticker, sgaResults);
            return finalResults;
        }

        #endregion

        #region "income statement helper methods"
        /// <summary>
        /// Helper method to calc the GP for the 5 year GP trend method
        /// CalcGrossMarginPercentAllYrs
        /// </summary>
        /// <param name="grossProfitDollars"></param>
        /// <param name="totalRevenueDollars"></param>
        /// <returns></returns>
        public decimal CalcGmPerc(long grossProfitDollars, long totalRevenueDollars)
        {
            // have to convert one of the two factors into a decimal, or else we'll get zero back
            // due to integer math not cooperating with decimal types
            decimal totalRevenueDec = Convert.ToDecimal(totalRevenueDollars);
            decimal grossProfitPerc = grossProfitDollars / totalRevenueDec;
            return grossProfitPerc;
        }

        public decimal CalcSgaPerc(long sgaExpenseDollars, long grossProfitDollars)
        {
            decimal grossProfitDec = Convert.ToDecimal(grossProfitDollars);
            decimal sgaPercent = sgaExpenseDollars / grossProfitDec;
            return sgaPercent;
        }

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

        #endregion
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
