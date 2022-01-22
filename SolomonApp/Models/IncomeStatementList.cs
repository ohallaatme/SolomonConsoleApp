using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SolomonApp.Models
{
    public class IncomeStatementList
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("annualReports")]
        public List<IncomeStatement> IncomeStatements { get; set; }
    }
}
