using System;
using System.Collections.Generic;
using System.Linq;
using SolomonApp.Models;
using SolomonApp.Parsers.Interfaces;

namespace SolomonApp.Parsers
{
    public class FinancialDataParser : IFinancialDataParser
    {
        #region "Scorecards"
        /// <summary>
        /// output format is {co ticker : {ratioName : {fiscalDate : Ratio ,fiscalDate : Ratio}) ratioName : {fiscalDate : Ratio}..}}}
        /// </summary>
        /// <param name="incomeStatementList"></param>
        /// <param name="finParser"></param>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, Dictionary<string, decimal>>> assembleScorecardOneCo(IncomeStatementList incomeStatementList,
                                                                            FinancialDataParser finParser)
        {
            // grab co ticker
            var ticker = incomeStatementList.Symbol;

            // final results
            Dictionary<string, Dictionary<string, Dictionary<string, decimal>>> incStatementScorecard = new Dictionary<string, Dictionary<string, Dictionary<string, decimal>>>();

            // Ratio Results - Stores the ratio name as the key and the results by fiscal date ending as the value inner dictionary
            Dictionary<string, Dictionary<string, decimal>> ratioResults = new Dictionary<string, Dictionary<string, decimal>>();

            // Accounts leveraged
            string grossProfitRaw = "GrossProfitRaw";
            string totalRevenueRaw = "TotalRevenueRaw";
            string sgaRaw = "SellingGeneralAndAdministrativeRaw";
            string interestExpenseRaw = "InterestExpenseRaw";
            string operatingIncomeRaw = "OperatingIncomeRaw";
            string incomeBeforeTaxRaw = "IncomeBeforeTaxRaw";
            string incomeTaxExpenseRaw = "IncomeTaxExpenseRaw";

            // GpResults
            var gpResults = finParser.CalcIncStatementFinancialRatio(grossProfitRaw, totalRevenueRaw, incomeStatementList);
            ratioResults.Add("gpRes", gpResults);

            // SgaResults
            var sgaResults = finParser.CalcIncStatementFinancialRatio(sgaRaw, grossProfitRaw, incomeStatementList);
            ratioResults.Add("sgaRes", sgaResults);

            // intExpResults
            var intExpResults = finParser.CalcIncStatementFinancialRatio(interestExpenseRaw, operatingIncomeRaw, incomeStatementList);
            ratioResults.Add("intExpRes", intExpResults);

            // tax expense test results
            // i.e. if tax exp reported is not 35% of income before taxes where is the extra
            // income coming from
            var taxExpResults = finParser.CalcIncStatementFinancialRatio(incomeTaxExpenseRaw, incomeBeforeTaxRaw, incomeStatementList);
            ratioResults.Add("taxExpRes", taxExpResults);


            // return final ratio results
            incStatementScorecard.Add(ticker, ratioResults);
            return incStatementScorecard;

        }

        #endregion
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
        public  Dictionary<string, decimal> CalcIncStatementFinancialRatio(string numeratorAccount,
                                                                    string denomonatorAccount,
                                                                    IncomeStatementList incomeStatementList)
        {

            // results dict for the ratio
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

            return ratioResults;
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

        /// <summary>
        /// Convert string property into decimal from deserialized string JSON
        /// </summary>
        /// <param name="finDataObject"></param>
        /// <param name="kpiName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get property value from financial models
        /// </summary>
        /// <param name="finDataObject"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public object GetPropertyValue(object finDataObject, string propName)
        {
            return finDataObject.GetType().GetProperty(propName).GetValue(finDataObject, null);
        }

        /// <summary>
        /// Format ratios into percent values for scorecard display
        /// </summary>
        /// <param name="numToConvert"></param>
        /// <returns></returns>
        public string FormatPercentValues(decimal numToConvert)
        {
            string valFormat = String.Format("Value: {0:P2}.", numToConvert);
            return valFormat; 
        }
        #endregion
    }
}
