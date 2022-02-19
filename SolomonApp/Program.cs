using System;
using System.Reflection;
using SolomonApp.Data;
using SolomonApp.Parsers;
using System.Threading.Tasks;
using SolomonApp.Models;
using System.Collections.Generic;
using System.Linq;
using SolomonApp.Models.Results;
using SolomonApp.ExcelExport;
using System.Threading;

namespace SolomonApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string apiKey;
            Console.Write("Welcome to Solomon!");
            Console.WriteLine("Please enter your API key from Alpha Vantage: ");
            apiKey = Console.ReadLine();


            FinanceDb finDb = new FinanceDb(apiKey);

            string coTickerOne = "MSFT";
            string coTickerTwo = "AAPL";
            string coTickerThree = "NFLX";
            string coTickerFour = "FB";
            string coTickerFive = "AMZN";

            List<string> tickers = new List<string>
            {
                coTickerOne,
                coTickerTwo,
                coTickerThree,
                coTickerFour,
                coTickerFive
            };

            FinancialDataParser finParser = new FinancialDataParser();


            List<IncStatementScorecard> incScorecards = new List<IncStatementScorecard>();
            List<BalSheetScorecard> balShtScorecards = new List<BalSheetScorecard>();
            // Need to count the # of API requests and pause
            // when there have been 5 in less than a mn
            int apiReqs = 0;

            DateTime startTime = DateTime.Now;
            DateTime endTime;
            
            foreach (string ticker in tickers)
            {
                // inc statement scorecard
                var incomeStatements = await finDb.GetIncomeStatement(ticker);
                apiReqs++;

                IncStatementScorecard incScorecard = finParser.AssembleIncScorecardOneCo(incomeStatements);

                incScorecards.Add(incScorecard);

                // balance sheet scorecard
                var balanceSheets = await finDb.GetBalanceSheet(ticker);
                apiReqs++;

                BalSheetScorecard balShtScorecard = finParser.AssembleBsScorecardOneCo(balanceSheets,
                incomeStatements);

                balShtScorecards.Add(balShtScorecard);

                endTime = DateTime.Now;

                var delta = (endTime - startTime).TotalSeconds;

                /*
                if (apiReqs >= 4 && delta < 60)
                {
                    var msToSleep = Convert.ToInt32((60 - delta) * 1000);
                    await Task.Delay(msToSleep);
                }
                */
                await Task.Delay(30000);

            }


            XmlWriter xmlWriter = new XmlWriter();

            xmlWriter.WriteFinStatementScorecard(incScorecards, balShtScorecards);

        }

        public static void PrintIncomeStatementScorecard(IncStatementScorecard incScorecard,
            FinancialDataParser finParser)
        {
            Console.WriteLine("Gross Profit Results: ");

            foreach (var gp in incScorecard.GrossProfitResults)
            {
                string res = finParser.FormatPercentValues(gp.Value);
                Console.WriteLine("{0}: {1}", gp.Key, res);
            }

            Console.WriteLine("SG&A Expense as a Percent of Operating Income: ");

            foreach (var sga in incScorecard.SgaResults)
            {
                string res = finParser.FormatPercentValues(sga.Value);
                Console.WriteLine("{0}: {1}", sga.Key, res);
            }

            Console.WriteLine("Interest Expense as a % of Operating Income: ");

            foreach (var intexp in incScorecard.IntExpResults)
            {
                string res = finParser.FormatPercentValues(intexp.Value);
                Console.WriteLine("{0}: {1}", intexp.Key, res);
            }

            Console.WriteLine("Tax Expense as a % of Income Before Tax: ");

            foreach (var taxexp in incScorecard.TaxExpResults)
            {
                string res = finParser.FormatPercentValues(taxexp.Value);
                Console.WriteLine("{0}: {1}", taxexp.Key, res);

            }
        }

        public static void PrintBalanceSheetScorecard(BalSheetScorecard balShtScorecard,
            FinancialDataParser finParser)
        {
            Console.WriteLine("Net Receivables as a % of Gross Revenue: ");

            foreach (var netRecRes in balShtScorecard.NetRecResults)
            {
                string res = finParser.FormatPercentValues(netRecRes.Value);
                Console.WriteLine("{0}: {1}", netRecRes.Key, res);
            }

            Console.WriteLine("Cash to Debt Ratio: ");

            foreach (var cshDebtRes in balShtScorecard.CashToDebtResults)
            {
                string res = finParser.FormatPercentValues(cshDebtRes.Value);
                Console.WriteLine("{0}: {1}", cshDebtRes.Key, res);
            }

            Console.WriteLine("Inventory as a % of Net Income: ");

            foreach (var invRes in balShtScorecard.InvToNetEarningsResults)
            {
                string res = finParser.FormatPercentValues(invRes.Value);
                Console.WriteLine("{0}: {1}", invRes.Key, res);
            }
        }
    }
}
