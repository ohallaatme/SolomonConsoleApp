using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SolomonApp.Models
{
    public class CashFlowList
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("annualReports")]
        public List<CashFlowStatement> CashFlowStatements { get; set; }

    }
}
