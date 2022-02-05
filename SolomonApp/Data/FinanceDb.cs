using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using SolomonApp.Models;

namespace SolomonApp.Data
{
    public class FinanceDb
    {
        private static readonly HttpClient client = new HttpClient();

        private static readonly string apiURI = "https://www.alphavantage.co/query?";

        private string apiKey;

        public FinanceDb(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public async Task<CompanyOverview> GetCompanyOverview(string coTicker)
        {
            string function = "OVERVIEW";

            var paramsInfo = FormatParameters(function, coTicker);

            string reqUrl = GenerateRequestURL(paramsInfo);

            var streamTask = client.GetStreamAsync(reqUrl);

            var overview = await JsonSerializer.DeserializeAsync<CompanyOverview>(await streamTask);

            return overview;
        }

        /// <summary>
        /// Get the 5 most recent income statements for a company
        /// </summary>
        /// <param name="coTicker"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public async Task<IncomeStatementList> GetIncomeStatement(string coTicker)
        {

            string function = "INCOME_STATEMENT";

            var paramsInfo = FormatParameters(function, coTicker);

            string reqUrl = GenerateRequestURL(paramsInfo);

            var streamTask = client.GetStreamAsync(reqUrl);

            var incomeStatements = await JsonSerializer.DeserializeAsync<IncomeStatementList>(await streamTask);

            return incomeStatements; 

        }

        /// <summary>
        /// Get the top 5 most recent balance sheets for a company 
        /// </summary>
        /// <param name="coTicker"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public async Task<BalanceSheetList> GetBalanceSheet(string coTicker)
        {
            string function = "BALANCE_SHEET";

            var paramsInfo = FormatParameters(function, coTicker);

            // get the request URL
            string reqUrl = GenerateRequestURL(paramsInfo);

            var streamTask = client.GetStreamAsync(reqUrl);

            var balanceSheets = await JsonSerializer.DeserializeAsync<BalanceSheetList>(await streamTask);

            return balanceSheets;


        }

        // TODO: Create sections within this class's methods

        /// <summary>
        /// Get the top 5 most recent cash flow statements for a company
        /// </summary>
        /// <param name="coTicker"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public async Task<CashFlowList> GetCashFlowStatement(string coTicker)
        {
            string function = "CASH_FLOW";

            var paramsInfo = FormatParameters(function, coTicker);

            // get the request URL
            string reqUrl = GenerateRequestURL(paramsInfo);

            var streamTask = client.GetStreamAsync(reqUrl);

            var cashFlowStatements = await JsonSerializer.DeserializeAsync<CashFlowList>(await streamTask);

            return cashFlowStatements;


        }

        /// <summary>
        /// Each API endpoint from AlphaVantage requires a function, company ticker and the API key
        /// This method adds these to the parameter dictionary so it can be passed to the GenerateRequestURL
        /// helper method to create the final GET request URL to the correct endpoint
        /// </summary>
        /// <param name="function"></param>
        /// <param name="coTicker"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        private Dictionary<string, string> FormatParameters(string function, string coTicker)
        {
            Dictionary<string, string> paramsInfo = new Dictionary<string, string>();
            paramsInfo.Add("function", function);
            paramsInfo.Add("symbol", coTicker);
            paramsInfo.Add("apikey", apiKey);

            return paramsInfo;

        }

        /// <summary>
        /// Helper method for request tasks that assembles the URL with the given
        /// params and the URI in the class
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        private string GenerateRequestURL(Dictionary<string, string> paramsInfo)
        {

            string reqUrl = apiURI;

            // get count of dict so we don't add an uncessary '&' symbol at the end
            // of the last param/value pair
            int paramCount = paramsInfo.Count;

            // track where we are at in the iteration
            int counter = 1;

            foreach (var dict in paramsInfo)
            {
                reqUrl += dict.Key;
                reqUrl += "=";
                reqUrl += dict.Value;

                if (counter == paramCount)
                {
                    // we don't want an unecessary add of the '&',
                    // this means we are done with the iteration
                    continue;
                }

                reqUrl += "&";

                // add to counter
                counter++;

            }

            return reqUrl;
        }
    }
}
