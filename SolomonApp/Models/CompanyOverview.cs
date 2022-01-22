using System;
using System.Text.Json.Serialization;

namespace SolomonApp.Models
{
    public class CompanyOverview
    {

        [JsonPropertyName("Symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("AssetType")]
        public string AssetType { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }

        // CIK = Central Index Key, maps to ticker
        [JsonPropertyName("CIK")]
        public string Cik { get; set; }

        [JsonPropertyName("Exchange")]
        public string Exchange { get; set; }

        [JsonPropertyName("Currency")]
        public string Currency { get; set; }

        [JsonPropertyName("Country")]
        public string Country { get; set; }

        [JsonPropertyName("Sector")]
        public string Sector { get; set; }

        [JsonPropertyName("Industry")]
        public string Industry { get; set; }

        [JsonPropertyName("Address")]
        public string Address { get; set; }

        [JsonPropertyName("FiscalYearEnd")]
        public string FiscalYearEnd { get; set; }

        [JsonPropertyName("LatestQuarter")]
        public string LatestQuarter { get; set; }

        [JsonPropertyName("MarketCapitalization")]
        public string MarketCapitalizationRaw { get; set; }

        [JsonPropertyName("EBITDA")]
        public string EbitdaRaw { get; set; }

        [JsonPropertyName("PERatio")]
        public string PeRatioRaw { get; set; }

        [JsonPropertyName("PEGRatio")]
        public string PegRatioRaw { get; set; }

        // companies total assets less liabilities, should a company dissolve
        // the book value is what would be paid out to shareholders
        // should the company liquidate. Equivilant to 'carrying value' on the balance sheet
        [JsonPropertyName("BookValue")]
        public string BookValueRaw { get; set; }

        [JsonPropertyName("DividendPerShare")]
        public string DividendPerShareRaw { get; set; }

        [JsonPropertyName("DividendYield")]
        public string DividendYieldRaw { get; set; }

        [JsonPropertyName("EPS")]
        public string EpsRaw { get; set; }

        [JsonPropertyName("RevenuePerShareTTM")]
        public string RevenuePerShareTrailingTwelveMonthsRaw { get; set; }

        [JsonPropertyName("ProfitMargin")]
        public string ProfitMarginRaw { get; set; }

        [JsonPropertyName("OperatingMarginTTM")]
        public string OperatingMarginTrailingTwelveMonthsRaw { get; set; }

        [JsonPropertyName("ReturnOnAssetsTTM")]
        public string ReturnOnAssetsTrailingTwelveMonthsRaw { get; set; }

        [JsonPropertyName("ReturnOnEquityTTM")]
        public string ReturnOnEquityTrailingTwelveMonthsRaw { get; set; }

        [JsonPropertyName("RevenueTTM")]
        public string RevenueTrailingTwelveMonthsRaw { get; set; }

        [JsonPropertyName("GrossProfitTTM")]
        public string GrossProfitTrailingTwelveMonthsRaw { get; set; }

        [JsonPropertyName("DilutedEPSTTM")]
        public string DilutedEpsTrailingTwelveMonthsRaw { get; set; }

        [JsonPropertyName("QuarterlyEarningsGrowthYOY")]
        public string QuarterlyEarningsGrowthYearOverYearRaw { get; set; }

        [JsonPropertyName("QuarterlyRevenueGrowthYOY")]
        public string QuarterlyRevenueGrowthYearOverYearRaw { get; set; }

        // TODO: Incorporate metric on how accurate analyst estimates are
        // learn more about where this comes from...
        // log financial metrics against price to see how fluctuations affect the price
        // long term could this be leveraged for predictive model?
        [JsonPropertyName("AnalystTargetPrice")]
        public string AnalystTargetPriceRaw { get; set; }

        [JsonPropertyName("TrailingPE")]
        public string TrailingPriceEarningsRaw { get; set; }

        [JsonPropertyName("ForwardPE")]
        public string ForwardPriceEarningsRaw { get; set; }

        [JsonPropertyName("PriceToSalesRatioTTM")]
        public string PriceToSalesRatioTrailingTwelveMonthsRaw { get; set; }

        // TODO: send alerts when watch list stocks are under 1.00
        [JsonPropertyName("PriceToBookRatio")]
        public string PriceToBookRatioRaw { get; set; }

        // EV is 'Enterprise Value', and attempts to reflect the market value of a firm
        // calc'd by Market cap + Debt + Minority Interest + Preferred Shares - Cash & Cash Equivilant
        // great for M&A 
        [JsonPropertyName("EVToRevenue")]
        public string EnterpriseValueToRevenueRaw { get; set; }

        // EBITDA = Earnings Before Interest Tax Depreciation Amortization
        [JsonPropertyName("EVToEBITDA")]
        public string EnterpriseValueToEbitaRaw { get; set; }

        // Risk
        [JsonPropertyName("Beta")]
        public string BetaRaw { get; set; }

        [JsonPropertyName("52WeekHigh")]
        public string FiftyTwoWeekHighRaw { get; set; }

        [JsonPropertyName("52WeekLow")]
        public string FiftyTwoWeekLowRaw { get; set; }

        [JsonPropertyName("50DayMovingAverage")]
        public string FiftyDayMovingAverageRaw { get; set; }

        [JsonPropertyName("200DayMovingAverage")]
        public string TwoHundredDayMovingAverageRaw { get; set; }

        [JsonPropertyName("SharesOutstanding")]
        public string SharesOutstandingRaw { get; set; }

        [JsonPropertyName("DividendDate")]
        public string DividendDate { get; set; }

        [JsonPropertyName("ExDividendDate")]
        public string ExDividendDate { get; set; }


    }
}
