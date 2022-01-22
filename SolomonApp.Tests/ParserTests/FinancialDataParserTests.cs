using SolomonApp.Models;
using SolomonApp.Parsers;
using SolomonApp.Data;
using Xunit;

namespace SolomonApp.Tests.ParserTests
{
    public class FinancialDataParserTests
    {
        private readonly IncomeStatement _incomeStatement;
        private readonly BalanceSheet _balanceSheet;
        private readonly CashFlowStatement _cashFlowStatement;
        private readonly CompanyOverview _coOverview;

        private readonly FinancialDataParser _finDataParser;

        public FinancialDataParserTests()
        {
            // 1) arrange

            // == INCOME STATEMENT ==
            // create income statement object
            _incomeStatement = new IncomeStatement();

            // add values to test
            _incomeStatement.GrossProfitRaw = "9234567987675";
            _incomeStatement.InterestIncomeRaw = "None";
            _incomeStatement.NonInterestIncomeRaw = "-1234567";


            // == BALANCE SHEET ==
            _balanceSheet = new BalanceSheet();
            _balanceSheet.TotalAssetsRaw = "39165000000";
            _balanceSheet.TreasuryStockRaw = "169339000000";
            _balanceSheet.TotalNonCurrentLiabilitiesRaw = "-106679000000";
            _balanceSheet.ShortTermInvestmentsRaw = "None";


            // == CASH FLOW ==
            _cashFlowStatement = new CashFlowStatement();
            _cashFlowStatement.ProceedsFromRepurchaseOfEquityRaw = "-302000000";
            _cashFlowStatement.ProceedsFromOperatingActivitiesRaw = "3406000000";
            _cashFlowStatement.ProceedsFromIssuanceOfLongTermDebtAndCapitalSecuritiesNetRaw = "None";

            // == COMPANY OVERVIEW ==
            _coOverview = new CompanyOverview();
            _coOverview.PeRatioRaw = "25.4";
            _coOverview.PegRatioRaw = "-2.154";
            _coOverview.PriceToBookRatioRaw = "None";

            // parser
            _finDataParser = new FinancialDataParser();
        }

        // naming convention is {MethodUnderTest}_{InputParameters}_{ExpectedOutput}
        // adding the statement/data object tuype being tested
        [Theory]
        [InlineData("InterestIncomeRaw", 0)]
        [InlineData( "GrossProfitRaw", 9234567987675)]
        [InlineData("NonInterestIncomeRaw", -1234567)]
        public void GetLongAccountValue_Is_Input_Expected(string propName, long expected)
        {
            // 2) act
            var acctBalance = _finDataParser.GetLongAccountValue(_incomeStatement, propName);

            // 3) assert
            Assert.Equal(expected, acctBalance);
        }

        [Theory]
        [InlineData("TotalAssetsRaw", 39165000000)]
        [InlineData("TotalNonCurrentLiabilitiesRaw", -106679000000)]
        [InlineData("ShortTermInvestmentsRaw", 0)]
        public void GetLongAccountValue_Bs_Input_Expected(string propName, long expected)
        {
            var acctBalance = _finDataParser.GetLongAccountValue(_balanceSheet, propName);

            Assert.Equal(expected, acctBalance);
        }

        [Theory]
        [InlineData("ProceedsFromRepurchaseOfEquityRaw", -302000000)]
        [InlineData("ProceedsFromOperatingActivitiesRaw", 3406000000)]
        [InlineData("ProceedsFromIssuanceOfLongTermDebtAndCapitalSecuritiesNetRaw", 0)]
        public void GetLongAccountValue_Cf_Input_Expected(string propName, long expected)
        {
            var acctBalance = _finDataParser.GetLongAccountValue(_cashFlowStatement, propName);

            Assert.Equal(expected, acctBalance);
        }

        [Theory]
        [InlineData("PeRatioRaw", 25.4)]
        [InlineData("PegRatioRaw", -2.154)]
        [InlineData("PriceToBookRatioRaw", 0)]
        public void GetDecimalKpiValue_CoOverview_Input_Expected(string propName, decimal expected)
        {
            var kpiValue = _finDataParser.GetDecimalKpiValue(_coOverview, propName);

            Assert.Equal(expected, kpiValue);

        }
    }
}
