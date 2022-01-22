using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace SolomonApp.Models
{
    public class BalanceSheetList
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }


        [JsonPropertyName("annualReports")]
        public List<BalanceSheet> BalanceSheets { get; set; }
    }
}
