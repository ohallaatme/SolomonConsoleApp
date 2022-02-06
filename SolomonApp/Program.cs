using System;
using System.Reflection;
using SolomonApp.Data;
using SolomonApp.Parsers;
using System.Threading.Tasks;
using SolomonApp.Models;
using System.Collections.Generic;
using System.Linq;
using SolomonApp.Models.Results;

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

            string coTicker = "MSFT";

            // == Income Statement Initial Test ==
            var incomeStatements = await finDb.GetIncomeStatement(coTicker);

            Console.WriteLine("Income Statement Tests: ");

            FinancialDataParser finParser = new FinancialDataParser();

            IncStatementScorecard incScorecard = finParser.AssembleIncScorecardOneCo(incomeStatements);

            Console.WriteLine("Income Statement Scorecard for {0}", incScorecard.CoTicker);

            PrintIncomeStatementScorecard(incScorecard, finParser);

            Console.WriteLine("Balance Sheet Tests: ");

            var balanceSheets = await finDb.GetBalanceSheet(coTicker);

            BalSheetScorecard balShtScorecard = finParser.AssembleBsScorecardOneCo(balanceSheets,
                incomeStatements);

            Console.WriteLine("Balance Sheet Scorecard for {0}", balShtScorecard.CoTicker);

            PrintBalanceSheetScorecard(balShtScorecard, finParser);

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
