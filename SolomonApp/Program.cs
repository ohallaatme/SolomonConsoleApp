using System;
using System.Reflection;
using SolomonApp.Data;
using SolomonApp.Parsers;
using System.Threading.Tasks;

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


            /*
            // == Income Statement Initial Test ==
            var incomeStatements = await finDb.GetIncomeStatement(coTicker, apiKey);

            Console.WriteLine("Income Statement Test: ");

            foreach (var statement in incomeStatements.IncomeStatements)
            {
                Console.WriteLine(statement.GrossProfitRaw);
            }

            // == Balance Sheet Initial Test ==
            var balanceSheets = await finDb.GetBalanceSheet(coTicker, apiKey);

            Console.WriteLine("Balance Sheet Tests: ");

            foreach (var statement in balanceSheets.BalanceSheets)
            {
                Console.WriteLine(statement.TotalCurrentAssetsRaw);
            }

            */


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
        }
    }
}
