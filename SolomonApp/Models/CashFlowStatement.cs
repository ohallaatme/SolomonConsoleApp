using System;
using System.Text.Json.Serialization;

namespace SolomonApp.Models
{
    public class CashFlowStatement
    {
        [JsonPropertyName("fiscalDateEnding")]
        public string FiscalDateEnding { get; set; }

        [JsonPropertyName("reportedCurrency")]
        public string ReportedCurrency { get; set; }

        [JsonPropertyName("operatingCashflow")]
        public string OperatingCashFlowRaw { get; set; }

        [JsonPropertyName("paymentsForOperatingActivities")]
        public string PaymentsForOperatingActivitiesRaw { get; set; }

        [JsonPropertyName("proceedsFromOperatingActivities")]
        public string ProceedsFromOperatingActivitiesRaw { get; set; }

        [JsonPropertyName("changeInOperatingLiabilities")]
        public string ChangeInOperatingLiabilitiesRaw { get; set; }

        [JsonPropertyName("changeInOperatingAssets")]
        public string ChangeInOperatingAssetsRaw { get; set; }

        [JsonPropertyName("depreciationDepletionAndAmortization")]
        public string DepreciationDepletionAndAmortizationRaw { get; set; }

        [JsonPropertyName("capitalExpenditures")]
        public string CapitalExpendituresRaw { get; set; }

        [JsonPropertyName("changeInReceivables")]
        public string ChangeInReceivablesRaw { get; set; }

        [JsonPropertyName("changeInInventory")]
        public string ChangeInInventoryRaw { get; set; }

        [JsonPropertyName("profitLoss")]
        public string ProfitLossRaw { get; set; }

        [JsonPropertyName("cashflowFromInvestment")]
        public string CashFlowFromInvestmentRaw { get; set; }

        [JsonPropertyName("cashflowFromFinancing")]
        public string CashFlowFromFinancingRaw { get; set; }

        [JsonPropertyName("proceedsFromRepaymentsOfShortTermDebt")]
        public string ProceedsFromRepaymentsOfShortTermDebt { get; set; }

        [JsonPropertyName("paymentsForRepurchaseOfCommonStock")]
        public string PaymentsForRepurchaseOfCommonStockRaw { get; set; }

        [JsonPropertyName("paymentsForRepurchaseOfEquity")]
        public string PaymentsForRepurchaseOfEquityRaw { get; set; }

        [JsonPropertyName("paymentsForRepurchaseOfPreferredStock")]
        public string PaymentsForRepurchaseOfPreferredStockRaw { get; set; }

        [JsonPropertyName("dividendPayout")]
        public string DividendPayoutRaw { get; set; }

        [JsonPropertyName("dividendPayoutCommonStock")]
        public string DividendPayoutCommonStockRaw { get; set; }

        [JsonPropertyName("dividendPayoutPreferredStock")]
        public string DividendPayoutPreferredStockRaw { get; set; }

        [JsonPropertyName("proceedsFromIssuanceOfCommonStock")]
        public string ProceedsFromIssuanceOfCommonStockRaw { get; set; }

        [JsonPropertyName("proceedsFromIssuanceOfLongTermDebtAndCapitalSecuritiesNet")]
        public string ProceedsFromIssuanceOfLongTermDebtAndCapitalSecuritiesNetRaw { get; set; }

        [JsonPropertyName("proceedsFromIssuanceOfPreferredStock")]
        public string ProceedsFromIssuanceOfPreferredStockRaw { get; set; }

        [JsonPropertyName("proceedsFromRepurchaseOfEquity")]
        public string ProceedsFromRepurchaseOfEquityRaw { get; set; }

        [JsonPropertyName("proceedsFromSaleOfTreasuryStock")]
        public string ProceedsFromSaleOfTreasuryStockRaw { get; set; }

        [JsonPropertyName("changeInCashAndCashEquivalents")]
        public string ChangeInCashAndCashEquivalentsRaw { get; set; }

        [JsonPropertyName("changeInExchangeRate")]
        public string ChangeInExchangeRateRaw { get; set; }

        [JsonPropertyName("netIncome")]
        public string NetIncomeRaw { get; set; }


    }
}
