using System;
using System.Text.Json.Serialization;

namespace SolomonApp.Models
{
    public class BalanceSheet
    {
        [JsonPropertyName("fiscalDateEnding")]
        public string FiscalDateEnding { get; set; }

        [JsonPropertyName("reportedCurrency")]
        public string ReportedCurrency { get; set; }

        [JsonPropertyName("totalAssets")]
        public string TotalAssetsRaw { get; set; }

        [JsonPropertyName("totalCurrentAssets")]
        public string TotalCurrentAssetsRaw { get; set; }

        [JsonPropertyName("cashAndCashEquivalentsAtCarryingValue")]
        public string CashAndCashEquivalentsAtCarryingValueRaw { get; set; }

        [JsonPropertyName("cashAndShortTermInvestments")]
        public string CashAndShortTermInvestmentsRaw { get; set; }

        [JsonPropertyName("inventory")]
        public string InventoryRaw { get; set; }

        [JsonPropertyName("currentNetReceivables")]
        public string CurrentNetReceivablesRaw { get; set; }

        [JsonPropertyName("totalNonCurrentAssets")]
        public string TotalNonCurrentAssetsRaw { get; set; }

        [JsonPropertyName("propertyPlantEquipment")]
        public string PropertyPlantEquipmentRaw { get; set; }

        [JsonPropertyName("accumulatedDepreciationAmortizationPPE")]
        public string AccumulatedDepreciationAmortizationPPERaw { get; set; }

        [JsonPropertyName("intangibleAssets")]
        public string IntangibleAssetsRaw { get; set; }

        [JsonPropertyName("intangibleAssetsExcludingGoodwill")]
        public string IntangibleAssetsExcludingGoodwillRaw { get; set; }

        [JsonPropertyName("goodwill")]
        public string GoodwillRaw { get; set; }

        [JsonPropertyName("investments")]
        public string InvestmentsRaw { get; set; }

        [JsonPropertyName("longTermInvestments")]
        public string LongTermInvestmentsRaw { get; set; }

        [JsonPropertyName("shortTermInvestments")]
        public string ShortTermInvestmentsRaw { get; set; }

        [JsonPropertyName("otherCurrentAssets")]
        public string OtherCurrentAssetsRaw { get; set; }

        [JsonPropertyName("otherNonCurrrentAssets")]
        public string OtherCurrentNonAssetsRaw { get; set; }

        [JsonPropertyName("totalLiabilities")]
        public string TotalLiabilitiesRaw { get; set; }

        [JsonPropertyName("totalCurrentLiabilities")]
        public string TotalCurrentLiabilitiesRaw { get; set; }

        [JsonPropertyName("currentAccountsPayable")]
        public string CurrentAccountsPayableRaw { get; set; }

        [JsonPropertyName("deferredRevenue")]
        public string DeferredRevenueRaw { get; set; }

        [JsonPropertyName("currentDebt")]
        public string CurrentDebtRaw { get; set; }

        [JsonPropertyName("shortTermDebt")]
        public string ShortTermDebtRaw { get; set; }

        [JsonPropertyName("totalNonCurrentLiabilities")]
        public string TotalNonCurrentLiabilitiesRaw { get; set; }

        [JsonPropertyName("capitalLeaseObligations")]
        public string CapitalLeaseObligationsRaw { get; set; }

        [JsonPropertyName("longTermDebt")]
        public string LongTermDebtRaw { get; set; }

        [JsonPropertyName("currentLongTermDebt")]
        public string CurrentLongTermDebtRaw { get; set; }

        [JsonPropertyName("longTermDebtNoncurrent")]
        public string LongTermDebtNonCurrentRaw { get; set; }

        [JsonPropertyName("shortLongTermDebtTotal")]
        public string ShortLongTermDebtTotal { get; set; }

        [JsonPropertyName("otherCurrentLiabilities")]
        public string OtherCurrentLiabilitiesRaw { get; set; }

        [JsonPropertyName("otherNonCurrentLiabilities")]
        public string OtherNonCurrentLiabilitiesRaw { get; set; }

        [JsonPropertyName("totalShareholderEquity")]
        public string TotalShareHolderEquityRaw { get; set; }

        [JsonPropertyName("treasuryStock")]
        public string TreasuryStockRaw { get; set; }

        [JsonPropertyName("retainedEarnings")]
        public string RetainedEarningsRaw { get; set; }

        [JsonPropertyName("commonStock")]
        public string CommonStockRaw { get; set; }

        [JsonPropertyName("commonStockSharesOutstanding")]
        public string CommonStockSharesOutstandingRaw { get; set; }


    }
}
