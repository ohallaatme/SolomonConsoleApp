using System;
using System.Collections.Generic;
using System.Linq;
using SolomonApp.Models;
using SolomonApp.Models.Results;
using SolomonApp.Parsers.Interfaces;

namespace SolomonApp.Parsers
{
    public class FinancialDataParser : IFinancialDataParser
    {
        #region "Company Comparison Scorecards"

        public FinalCoComparisonScorecard CompanyComparison(List<IncomeStatementList> incomeStatementLists,
            List<BalanceSheetList> balanceSheetLists)
        {
            FinalCoComparisonScorecard finalScorecard = new FinalCoComparisonScorecard();

            // get all the co tickers to manage iteration between inc and bs lists for bs ratios

            List<string> coTickers = new List<string>();

            // Income statment ratios
            foreach (IncomeStatementList incStmtList in incomeStatementLists)
            {
                IncStatementScorecard incStmtScorecard = AssembleIncScorecardOneCo(incStmtList);

                coTickers.Add(incStmtScorecard.CoTicker);

                finalScorecard.IncomeStatementScorecards.Add(incStmtScorecard);
            }

            // Balance sheet ratios
            foreach (string co in coTickers)
            {
                var incStatementList = incomeStatementLists.Where(incomeStatementList => incomeStatementList.Symbol == co).FirstOrDefault();

                var balanceSheetList = balanceSheetLists.Where(balanceSheetList => balanceSheetList.Symbol == co).FirstOrDefault();

                BalSheetScorecard balSheetScorecard = AssembleBsScorecardOneCo(balanceSheetList, incStatementList);

                finalScorecard.BalSheetScorecards.Add(balSheetScorecard);

            }
            

            // PICKUP 2.5.2022 - Write program tests around assembling the final scorecards
            return finalScorecard;

        }


        #endregion
        #region "SingleCoScorecards"
        /// <summary>
        /// output format is {co ticker : {ratioName : {fiscalDate : Ratio ,fiscalDate : Ratio}) ratioName : {fiscalDate : Ratio}..}}}
        /// </summary>
        /// <param name="incomeStatementList"></param>
        /// <param name="finParser"></param>
        /// <returns></returns>
        public IncStatementScorecard AssembleIncScorecardOneCo(IncomeStatementList incomeStatementList)
        {
            // grab co ticker
            var ticker = incomeStatementList.Symbol;

            // final results
            IncStatementScorecard incStmtScorecard = new IncStatementScorecard();

            incStmtScorecard.CoTicker = ticker;

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
            incStmtScorecard.GrossProfitResults = CalcFinancialRatio(acctBalances[grossProfitRaw], acctBalances[totalRevenueRaw]);

            // SgaResults
            incStmtScorecard.SgaResults = CalcFinancialRatio(acctBalances[sgaRaw], acctBalances[operatingIncomeRaw]);

            // intExpResults
            incStmtScorecard.IntExpResults = CalcFinancialRatio(acctBalances[interestExpenseRaw], acctBalances[operatingIncomeRaw]);

            // tax expense test results
            // i.e. if tax exp reported is not 35% of income before taxes where is the extra
            // income coming from
            incStmtScorecard.TaxExpResults = CalcFinancialRatio(acctBalances[incomeTaxExpenseRaw], acctBalances[incomeBeforeTaxRaw]);

            // return final ratio results
            return incStmtScorecard;

        }


        // Note many balance sheet ratios require income statement accounts
        public BalSheetScorecard AssembleBsScorecardOneCo(BalanceSheetList balanceSheetList,
                                            IncomeStatementList incomeStatementList)
        {
            // grab co ticker
            var ticker = balanceSheetList.Symbol;

            // final results
            BalSheetScorecard balShtScorecard = new BalSheetScorecard();

            balShtScorecard.CoTicker = ticker;


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
            balShtScorecard.NetRecResults = CalcFinancialRatio(acctBalances[curNetReceivablesRaw], acctBalances[totalRevenueRaw]);

            // cash to debt results
            balShtScorecard.CashToDebtResults = CalcFinancialRatio(acctBalances[cashAndStInvestmentsRaw],
                totalDebtRawBalances);

            // inventory to net earnings, if both of these are on a corresponding rise its a sign of competitive advantage
            // (vs booming and busting every few years)
            balShtScorecard.InvToNetEarningsResults = CalcFinancialRatio(acctBalances[totalInvRaw],
                acctBalances[totalNetIncRaw]);

            // final res
            return balShtScorecard;
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
