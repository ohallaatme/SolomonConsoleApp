using System;
using System.Reflection;
using SolomonApp.Data;
using SolomonApp.Parsers;
using System.Threading.Tasks;
using SolomonApp.Models;
using System.Collections.Generic;

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



            FinancialDataParser finParser = new FinancialDataParser();

            await IncRatioResults("GrossProfitRaw", "TotalRevenueRaw", "Gross Profit Percent",
                incomeStatements, finParser);

            await IncRatioResults("SellingGeneralAndAdministrativeRaw", "GrossProfitRaw",
                "SG&A Expense as a % of Gross Profit", incomeStatements, finParser);

        }
        static async Task IncRatioResults(string numAcct, string denomAcct,
                                    string kpiType,
                                    IncomeStatementList incomeStatementList,
                                    FinancialDataParser finParser)
        {
            var results = finParser.CalcIncStatementFinancialRatio(numAcct, denomAcct,
                                    incomeStatementList);

            LoopThroughPercResults(kpiType, results, finParser);
        }

        static void LoopThroughPercResults(string kpiType,
                    Dictionary<string, Dictionary<string, decimal>> resultsDict,
                    FinancialDataParser finParser)
        {
            foreach(var item in resultsDict)
            {
                Console.WriteLine("{0} results for {1}", kpiType, item.Key);

                foreach(var innerItem in item.Value)
                {
                    var formattedRes = finParser.FormatPercentValues(innerItem.Value);
                    Console.WriteLine("Fiscal Period End Date: {0}", innerItem.Key);
                    Console.WriteLine("{0}: {1}", kpiType, formattedRes);
                }
            }
        }
    }
}
