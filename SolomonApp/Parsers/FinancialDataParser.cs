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
        /// Calculate income statement ratios
        /// </summary>
        /// <param name="numeratorAccount"></param>
        /// <param name="denomonatorAccount"></param>
        /// <param name="incomeStatementList"></param>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, decimal>> CalcIncStatementFinancialRatio(string numeratorAccount,
                                                                    string denomonatorAccount,
                                                                    IncomeStatementList incomeStatementList)
        {
            // save company symbol
            var ticker = incomeStatementList.Symbol;

            // final results dict
            Dictionary<string, Dictionary<string, decimal>> finalResults = new Dictionary<string, Dictionary<string, decimal>>();

            // inner results dict for the ratio
            Dictionary<string, decimal> ratioResults = new Dictionary<string, decimal>();

            foreach(var statement in incomeStatementList.IncomeStatements)
            {
                // get fiscal date ending for the income statement
                string fiscalDateEnding = statement.FiscalDateEnding;

                // get values for calculation
                long numeratorDollars = GetLongAccountValue(statement, numeratorAccount);
                long denomonatorDollars = GetLongAccountValue(statement, denomonatorAccount);

                // TODO: fill in edge cases / error checks of where any of these values could be 0
                decimal ratio = CalcRatioResult(numeratorDollars, denomonatorDollars);

                // TODO: Consider currency edge cases as a part of broader data structure, here it doesn't matter as much considering its a %
                ratioResults.Add(fiscalDateEnding, ratio);

            }

            finalResults.Add(ticker, ratioResults);
            return finalResults;
        }
        #endregion

        #region "income statement helper methods"
        /// <summary>
        /// Calc the actual ratio to support the 5 year method
        /// CalcIncStatementFinancialRatio
        /// </summary>
        /// <param name="numeratorDollars"></param>
        /// <param name="denomonatorDollars"></param>
        /// <returns></returns>
        public decimal CalcRatioResult(long numeratorDollars, long denomonatorDollars)
        {
            // have to convert one of the two factors into a decimal, or else we'll get zero back
            // due to integer math not cooperating with decimal types
            decimal denomDecimal = Convert.ToDecimal(denomonatorDollars);
            decimal resultRatio = numeratorDollars / denomDecimal;
            return resultRatio;
        }

        /// <summary>
        /// Helper method to calc the GP for the 5 year GP trend method
        /// CalcGrossMarginPercentAllYrs
        /// </summary>
        /// <param name="grossProfitDollars"></param>
        /// <param name="totalRevenueDollars"></param>
        /// <returns></returns>
        #endregion

        #region "General helper methods"
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
        #endregion
    }
}
