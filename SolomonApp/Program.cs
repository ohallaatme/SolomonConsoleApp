using System;
using System.Reflection;
using SolomonApp.Data;
using SolomonApp.Parsers;
using System.Threading.Tasks;
using SolomonApp.Models;
using System.Collections.Generic;
using System.Linq;

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

            await IncScorecardResults(incomeStatements, finParser);
            

        }
        static async Task IncScorecardResults(IncomeStatementList incomeStatementList,
                                    FinancialDataParser finParser)
        {
            var IncScorecard = finParser.assembleIncScorecardOneCo(incomeStatementList, finParser);
            LoopThroughIncResults(IncScorecard, finParser);
        }   

        static void LoopThroughIncResults(Dictionary<string, Dictionary<string, Dictionary<string, decimal>>> results, FinancialDataParser finParser)
        {
            var ticker = results.Keys.First();
            Console.WriteLine("Income Statement Results for: {0}", ticker);

            var innerRes = results[ticker];

            
            var gpRes = innerRes["gpRes"];
            var sgaRes = innerRes["sgaRes"];
            var intExpRes = innerRes["intExpRes"];
            var taxExpRes = innerRes["taxExpRes"];
            WriteIndividualKPIResults("Gross Profit %", gpRes, finParser);
            WriteIndividualKPIResults("SGA Expense as % of Gross Profit", sgaRes, finParser);
            WriteIndividualKPIResults("int Expense as % of Operating Income", intExpRes, finParser);
            WriteIndividualKPIResults("Tax Expense as % of Inc Before Taxes", taxExpRes, finParser);
        }

        static void WriteIndividualKPIResults(string resType, Dictionary<string, decimal> results,
            FinancialDataParser finParser)
        {
            Console.WriteLine(resType);

            foreach(var item in results)
            {
                var formRes = finParser.FormatPercentValues(item.Value);
                Console.WriteLine("{0} : {1} ", item.Key, formRes);
            }

        }
    }
}
