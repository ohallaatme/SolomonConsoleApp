using System;
using System.Text.Json.Serialization;

namespace SolomonApp.Models
{
    public class IncomeStatement
    {
        [JsonPropertyName("fiscalDateEnding")]
        public string FiscalDateEnding { get; set; }

        [JsonPropertyName("reportedCurrency")]
        public string ReportedCurrency { get; set; }

        // pull the raw string data first, if there isn't a value we get
        // 'none' text so parser will convert data
        [JsonPropertyName("grossProfit")]
        public string GrossProfitRaw { get; set; }

        [JsonPropertyName("totalRevenue")]
        public string TotalRevenueRaw { get; set; }

        [JsonPropertyName("costOfRevenue")]
        public string CostOfRevenueRaw { get; set; }

        [JsonPropertyName("costofGoodsAndServicesSold")]
        public string CostOfGoodsAndServicesSoldRaw { get; set; }

        [JsonPropertyName("operatingIncome")]
        public string OperatingIncomeRaw { get; set; }

        [JsonPropertyName("sellingGeneralAndAdministrative")]
        public string SellingGeneralAndAdministrativeRaw { get; set; }

        [JsonPropertyName("researchAndDevelopment")]
        public string ResearchAndDevelopmentRaw { get; set; }

        [JsonPropertyName("operatingExpenses")]
        public string OperatingExpensesRaw { get; set; }
        
        [JsonPropertyName("investmentIncomeNet")]
        public string InvestmentIncomeNetRaw { get; set; }

        [JsonPropertyName("netInterestIncome")]
        public string NetInterestIncomeRaw { get; set; }

        [JsonPropertyName("interestIncome")]
        public string InterestIncomeRaw { get; set; }

        [JsonPropertyName("interestExpense")]
        public string InterestExpenseRaw { get; set; }

        [JsonPropertyName("nonInterestIncome")]
        public string NonInterestIncomeRaw { get; set; }

        [JsonPropertyName("otherNonOperatingIncome")]
        public string OtherNonOperatingIncomeRaw { get; set; }

        [JsonPropertyName("depreciation")]
        public string DepreciationRaw { get; set; }

        [JsonPropertyName("depreciationAndAmortization")]
        public string DepreciationAndAmortizationRaw { get; set; }

        [JsonPropertyName("incomeBeforeTax")]
        public string IncomeBeforeTaxRaw { get; set; }

        [JsonPropertyName("incomeTaxExpense")]
        public string IncomeTaxExpenseRaw { get; set; }

        [JsonPropertyName("interestAndDebtExpense")]
        public string InterestAndDebtExpenseRaw { get; set; }

        [JsonPropertyName("netIncomeFromContinuingOperations")]
        public string NetIncomeFromContinuingOperationsRaw { get; set; }

        [JsonPropertyName("comprehensiveIncomeNetOfTax")]
        public string ComprehensiveIncomeNetOfTaxRaw { get; set; }

        [JsonPropertyName("ebit")]
        public string EbitRaw { get; set; }

        [JsonPropertyName("ebita")]
        public string EbitaRaw { get; set; }

        [JsonPropertyName("netIncome")]
        public string NetIncomeRaw { get; set; }

    }
}
