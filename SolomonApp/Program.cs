using System;
using System.Reflection;
using SolomonApp.Data;
using SolomonApp.Parsers;
using System.Threading.Tasks;
using SolomonApp.Models;

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

            Console.WriteLine("Income Statement Test: ");

            foreach (var statement in incomeStatements.IncomeStatements)
            {
                Console.WriteLine(statement.GrossProfitRaw);
            }

            Console.WriteLine("Kicking off GP analysis...");

            await GPResults(incomeStatements);

            /*
            // == Balance Sheet Initial Test ==
            var balanceSheets = await finDb.GetBalanceSheet(coTicker);

            Console.WriteLine("Balance Sheet Tests: ");

            foreach (var statement in balanceSheets.BalanceSheets)
            {
                Console.WriteLine(statement.TotalCurrentAssetsRaw);
            }

            


            // == Company Overview Test
            var companyOverview = await finDb.GetCompanyOverview(coTicker);

            Console.WriteLine("Company Overview Test One: ");

            Console.WriteLine(companyOverview.EbitdaRaw);

            var cashFlowStatements = await finDb.GetCashFlowStatement(coTicker);

            Console.WriteLine("Cash Flow Tests: ");

            foreach (var statement in cashFlowStatements.CashFlowStatements)
            {
                Console.WriteLine(statement.CashFlowFromFinancingRaw);
            }
            */
        }
        static async Task GPResults(IncomeStatementList incomeStatementList)
        {
            FinancialDataParser finParser = new FinancialDataParser();

            var gmResults = finParser.CalcGrossMarginPercentAllYrs(incomeStatementList);

            foreach(var item in gmResults)
            {
                Console.WriteLine("Results for {0}", item.Key);
                foreach(var innerItem in item.Value)
                {
                    var formattedGM = finParser.FormatPercentValues(innerItem.Value);
                    Console.WriteLine("Fiscal Period End Date: {0}", innerItem.Key);
                    Console.WriteLine("GM Percent: {0}", formattedGM);
                }
            }
        }
    }
}
