using System;
using System.Collections.Generic;
using System.Linq;
using SolomonApp.Models;
using SolomonApp.Parsers.Interfaces;

namespace SolomonApp.Parsers
{
    public class FinancialDataParser : IFinancialDataParser
    {
        #region "SingleCoScorecards"
        /// <summary>
        /// output format is {co ticker : {ratioName : {fiscalDate : Ratio ,fiscalDate : Ratio}) ratioName : {fiscalDate : Ratio}..}}}
        /// </summary>
        /// <param name="incomeStatementList"></param>
        /// <param name="finParser"></param>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, Dictionary<string, decimal>>> AssembleIncScorecardOneCo(IncomeStatementList incomeStatementList)
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

            List<string> acctNames = new List<string>()
            {
                grossProfitRaw,
                totalRevenueRaw,
                sgaRaw,
                interestExpenseRaw,
                operatingIncomeRaw,
                incomeBeforeTaxRaw,
                incomeTaxExpenseRaw
            };

            // store account balances for the rolling 5 years available 
            Dictionary<string, Dictionary<string, long>> acctBalances = new Dictionary<string, Dictionary<string, long>>();

            // get account balances
            foreach(string acct in acctNames)
            {
                Dictionary<string, long> balances = GetAcctFiveYrBal(acctName: acct, incomeStatementList: incomeStatementList);
                acctBalances.Add(acct, balances);
            }


            // GpResults
            var gpResults = CalcFinancialRatio(acctBalances[grossProfitRaw], acctBalances[totalRevenueRaw]);
            ratioResults.Add("gpRes", gpResults);


            // SgaResults
            var sgaResults = CalcFinancialRatio(acctBalances[sgaRaw], acctBalances[operatingIncomeRaw]);
            ratioResults.Add("sgaRes", sgaResults);

            // intExpResults
            var intExpResults = CalcFinancialRatio(acctBalances[interestExpenseRaw], acctBalances[operatingIncomeRaw]);
            ratioResults.Add("intExpRes", intExpResults);

            // tax expense test results
            // i.e. if tax exp reported is not 35% of income before taxes where is the extra
            // income coming from
            var taxExpResults = CalcFinancialRatio(acctBalances[incomeTaxExpenseRaw], acctBalances[incomeBeforeTaxRaw]);
            ratioResults.Add("taxExpRes", taxExpResults);

            // return final ratio results
            incStatementScorecard.Add(ticker, ratioResults);
            return incStatementScorecard;

        }


        // Note many balance sheet ratios require income statement accounts
        public Dictionary<string, Dictionary<string, Dictionary<string, decimal>>> AssembleBsScorecardOneCo(BalanceSheetList balanceSheetList,
                                            IncomeStatementList incomeStatementList)
        {
            // grab co ticker
            var ticker = balanceSheetList.Symbol;

            // final results
            Dictionary<string, Dictionary<string, Dictionary<string, decimal>>> balanceSheetScorecard = new Dictionary<string, Dictionary<string, Dictionary<string, decimal>>>();

            // Ratio Results - Stores the ratio name as the key and the results by fiscal date ending as the value inner dictionary
            Dictionary<string, Dictionary<string, decimal>> ratioResults = new Dictionary<string, Dictionary<string, decimal>>();


            // == Accounts leveraged ==

            // -- Income Statement --
            string totalRevenueRaw = "TotalRevenueRaw";
            string totalNetIncRaw = "NetIncomeRaw";

            // -- Balance Sheet --

            // * Assets *
            string curNetReceivablesRaw = "CurrentNetReceivablesRaw";
            string cashAndStInvestmentsRaw = "CashAndShortTermInvestmentsRaw";
            string totalInvRaw = "InventoryRaw";
            

            // * Liabilities * 
            string shortTermDebt = "ShortTermDebtRaw";
            string longTermDebt = "LongTermDebtRaw";

            List<string> incStmtAcctNames = new List<string>()
            {
                totalRevenueRaw,
                totalNetIncRaw
            };

            List<string> balShtAcctNames = new List<string>()
            {
                curNetReceivablesRaw,
                cashAndStInvestmentsRaw,
                totalInvRaw,
                shortTermDebt,
                longTermDebt
            };

            Dictionary<string, Dictionary<string, long>> acctBalances = new Dictionary<string, Dictionary<string, long>>();

            foreach(var acct in incStmtAcctNames)
            {
                Dictionary<string, long> balances = GetAcctFiveYrBal(acctName: acct, incomeStatementList: incomeStatementList);
                acctBalances.Add(acct, balances);
            }

            foreach(var acct in balShtAcctNames)
            {
                Dictionary<string, long> balances = GetAcctFiveYrBal(acctName: acct, balanceSheetList: balanceSheetList);
                acctBalances.Add(acct, balances);
            }

            // need total debt, which isn't native to the original payload returned
            Dictionary<string, long> totalDebtRawBalances = SumTwoAccountBalances(acctBalances[shortTermDebt],
                acctBalances[longTermDebt]);


            // net receivables as a % of rev results
            Dictionary<string, decimal> recRevPercResults = CalcFinancialRatio(acctBalances[curNetReceivablesRaw], acctBalances[totalRevenueRaw]);
            ratioResults.Add("netRecResults", recRevPercResults);

            // cash to debt results
            Dictionary<string, decimal> cashToDebtResults = CalcFinancialRatio(acctBalances[cashAndStInvestmentsRaw],
                totalDebtRawBalances);
            ratioResults.Add("cashToDebtResults", cashToDebtResults);

            // inventory to net earnings, if both of these are on a corresponding rise its a sign of competitive advantage
            // (vs booming and busting every few years)
            Dictionary<string, decimal> invToNetEarningsResults = CalcFinancialRatio(acctBalances[totalInvRaw],
                acctBalances[totalNetIncRaw]);
            ratioResults.Add("invToNetEarningsResults", invToNetEarningsResults);

            // final res
            balanceSheetScorecard.Add(ticker, ratioResults);
            return balanceSheetScorecard;
        }
        #endregion


        #region "Scorecard Helper Methods"
        public Dictionary<string, decimal> CalcFinancialRatio(Dictionary<string, long> numAcctBalances,
            Dictionary<string, long> denomAcctBalances)
        {
            Dictionary<string, decimal> ratioResults = new Dictionary<string, decimal>();

            // get the years to loop through from one of the dicts,
            // doesn't matter which one because they are in the same company they should match
            var years = numAcctBalances.Keys;

            foreach (string yr in years)
            {
                long numDollars = numAcctBalances[yr];
                long denomDollars = denomAcctBalances[yr];

                decimal ratio = CalcRatioResult(numDollars, denomDollars);
                ratioResults.Add(yr, ratio);
            }

            return ratioResults;
        }

        public Dictionary<string, long> SumTwoAccountBalances(Dictionary<string, long> acctBalsOne,
            Dictionary<string, long> acctBalsTwo)
        {
            Dictionary<string, long> sumBalanceResults = new Dictionary<string, long>();

            // get the years, because this is for a single company doesn't matter which acct we
            // take it from
            var years = acctBalsOne.Keys;

            foreach(var yr in years)
            {
                long acctOneDollars = acctBalsOne[yr];
                long acctTwoDollars = acctBalsTwo[yr];

                long acctBalSum = acctOneDollars + acctTwoDollars;

                sumBalanceResults.Add(yr, acctBalSum);

            }

            return sumBalanceResults;

        }


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
        #endregion

        #region "Account trend methods"
        public Dictionary<string, long> GetAcctFiveYrBal(string acctName,
            IncomeStatementList incomeStatementList = null,
            BalanceSheetList balanceSheetList = null)
        {
            // guard clauses
            if (incomeStatementList == null && balanceSheetList == null)
            {
                throw new ArgumentNullException("Both the incomeStatementList and the balanceSheetList " +
                    "cannot be null");
            }

            if (incomeStatementList != null && balanceSheetList != null)
            {
                throw new ArgumentOutOfRangeException("Provide either an income statement or balance sheet list, " +
                    "can only process one statement at a time, not both");
            }

            Dictionary<string, long> acctBalResults = new Dictionary<string, long>();

            string fiscalDateEnding;
            long acctBal;

            if (incomeStatementList != null)
            {
                foreach (var statement in incomeStatementList.IncomeStatements)
                {
                    fiscalDateEnding = statement.FiscalDateEnding;
                    acctBal = GetLongAccountValue(statement, acctName);

                    acctBalResults.Add(fiscalDateEnding, acctBal);
                }
            }

            else if (balanceSheetList != null)
            {
                foreach (var statement in balanceSheetList.BalanceSheets)
                {
                    fiscalDateEnding = statement.FiscalDateEnding;
                    acctBal = GetLongAccountValue(statement, acctName);

                    acctBalResults.Add(fiscalDateEnding, acctBal);
                }
            }


            return acctBalResults;
        }

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
