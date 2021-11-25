using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Dividends;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.PositionCard;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Models;
using AutomatedTests.Models.PositionsModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._04_PositionsAndAlerts._01_Positions
{
    [TestClass]
    public class TC_0931_PositionsGrid_ValuesInDividendsColumnsAndPositionCardMatchTheExpectedForStocksWithOneSplit : BaseTestUnitTests
    {
        private const int TestNumber = 931;
        private const int MonthShiftForEntryDate = -3;

        private readonly DividendsQuantitySumModel dividendsModel = new DividendsQuantitySumModel();
        private string shares;
        private int positionId;
        private readonly List<string> columns = new List<string> { PositionsGridDataField.Dividends.GetStringMapping(), PositionsGridDataField.TotalDividends.GetStringMapping() };

        [TestInitialize]
        public void TestInitialize()
        {
            var portfolioModel = new PortfolioModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency")
            };
            var isLongTradeType = GetTestDataAsBool("tradeType");
            var adjustAlertsByDividends = GetTestDataAsBool("AdjustAlertsByDividends");
            shares = GetTestDataAsString(nameof(shares));
            var viewNameForAddedView = StringUtility.RandomString("View#######");
            var exchangeIds = GetTestDataAsString("ExchangeIds");
            var limitOfDividendsQuantity = GetTestDataAsString("LimitOfDividendsQuantity");
            var tradeDate = GetTestDataAsString("TradeDate");
            var numberOfSplits = GetTestDataAsInt("NumberOfSplits");

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsBasic));
            var portfolioIdManual = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            var dividendQueries = new DividendsQueries();
            var symbolId = dividendQueries.SelectPositionWithDefinedCurrencyBigDividendsCertainNumberOfSplitsNotDividendsBeforeDate(exchangeIds,
                limitOfDividendsQuantity, numberOfSplits, tradeDate);
            var tickerToOverride = GetTestDataAsString("tickerStock");
            var positionsQueries = new PositionsQueries();
            var ticker = string.IsNullOrEmpty(tickerToOverride) ? positionsQueries.SelectSymbolBySymbolId(symbolId) : tickerToOverride;
            symbolId = string.IsNullOrEmpty(tickerToOverride) ? symbolId : Parsing.ConvertToInt(positionsQueries.SelectSymbolIdNameUsingSymbol(tickerToOverride).SymbolId);
            var dateWithFirstDividend = DateTime.Parse(dividendQueries.SelectDateOfFirstDividensdForSymbol(symbolId));

            var dataForSplit = dividendQueries.SelectSplitDateCoeffForSymbol(symbolId);
            dividendsModel.SplitDate = dataForSplit.SplitDate;
            dividendsModel.SplitCoeff = dataForSplit.SplitCoeff;

            var dataForDividendsBeforeSplit = dividendQueries.SelectDividendSumQuantityBeforeSplit(dateWithFirstDividend.ToString(CultureInfo.InvariantCulture), dividendsModel.SplitDate, symbolId);
            dividendsModel.DividendsSumBeforeSplit = dataForDividendsBeforeSplit.DividendsSumBeforeSplit;
            dividendsModel.DividendsQuantityBeforeSplit = dataForDividendsBeforeSplit.DividendsQuantityBeforeSplit;

            var dataForDividendsAfterSplit = dividendQueries.SelectDividendSumQuantityAfterSplit(dividendsModel.SplitDate, symbolId);
            dividendsModel.DividendsSumAfterSplit = dataForDividendsAfterSplit.DividendsSumAfterSplit;
            dividendsModel.DividendsQuantityAfterSplit = dataForDividendsAfterSplit.DividendsQuantityAfterSplit;

            var positionModel = new AddPositionAdvancedModel
            {
                Ticker = ticker,
                IsLongTradeType = isLongTradeType,
                IsAdjustByDividends = adjustAlertsByDividends,
                EntryDate = dateWithFirstDividend.AddMonths(MonthShiftForEntryDate).ToShortDateString(),
                Shares = shares,
                AssetType = positionsQueries.SelectAssetTypeNameBySymbolId(symbolId).ParseAsEnumFromStringMapping<PositionAssetTypes>(),
                Portfolio = portfolioModel.Name
            };

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);
            positionId = PositionsAlertsSetUp.AddPositionFromAdvancedForm(portfolioIdManual, positionModel);

            new MainMenuNavigation().OpenPositionsGrid();
            new PositionsTabForm().AddANewViewWithCheckboxesMarked(viewNameForAddedView, columns);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_931$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PositionsGrid"), TestCategory("PositionCard")]
        [Description("https://tr.a1qa.com/index.php?/cases/view/19232210 The test checks that Values in Total Dividends and Dividends columns for open positions grid " +
            "and Position card match the expected for stocks with one split ")]
        public override void RunTest()
        {
            LogStep(1, "Make sure that dividend column has value matches with DividendsSumBeforeSplit*SplitCoeff + DividendsSumAfterSplit");
            var positionsTabForm = new PositionsTabForm();
            var dividendsData = positionsTabForm.GetPositionDataByPositionId(new List<PositionsGridDataField>
                {
                    PositionsGridDataField.Dividends, PositionsGridDataField.TotalDividends
                }, positionId);
            Checker.CheckEquals((dividendsModel.DividendsSumBeforeSplit * dividendsModel.SplitCoeff + dividendsModel.DividendsSumAfterSplit).ToString("#0.00"),
                dividendsData.Dividends.Replace(Constants.AllCurrenciesRegex.Match(dividendsData.Dividends.Replace("-", string.Empty)).Value, string.Empty).Replace("-", string.Empty),
                "Added position does not have value in a Dividend column matched with DividendsSumBeforeSplit*SplitCoeff + DividendsSumAfterSplit");

            LogStep(2, "Check that added positions have value in a Total Dividend column matched with own DividendSum values multiplyed on Shares and truncated to two decimals");
            var expectedTotalDividends = ((dividendsModel.DividendsSumBeforeSplit * dividendsModel.SplitCoeff + dividendsModel.DividendsSumAfterSplit) * Parsing.ConvertToDouble(shares))
                .ToString("#0.00");
            Checker.CheckEquals(expectedTotalDividends,
                dividendsData.TotalDividends.Replace(Constants.AllCurrenciesRegex.Match(dividendsData.TotalDividends
                    .Replace("-", string.Empty)).Value, string.Empty).Replace("-", string.Empty).Replace(",", string.Empty),
                "Added position does not have value in a Total Dividend column matched with own DividendSum values multiplied on Shares and truncated to two decimals");

            LogStep(3, "For position - click position link to open position card");
            positionsTabForm.ClickOnPositionLink(positionId);

            LogStep(4, "Sure that quantity of listed dividends *before split* matches DividendsQuantityBeforeSplit." +
                "Sure that quantity of listed dividends * after split * matches DividendsQuantityAfterSplit");
            var positionCardForm = new PositionCardForm();
            var statisticTabForm = positionCardForm.ActivateTabGetForm<StatisticTabForm>(PositionCardTabs.Statistics);
            var allCorporateActions = statisticTabForm.GetAllCorporateActions();
            var corporateActionsBeforeSplit = GetCorporateActionsBeforeSplit(allCorporateActions);
            var corporateActionsAfterSplit = GetCorporateActionsAfterSplit(allCorporateActions);
            Checker.CheckEquals(dividendsModel.DividendsQuantityBeforeSplit, corporateActionsBeforeSplit.Count,
                "Quantity of listed dividends *before split* does not match DividendsQuantityBeforeSplit");
            Checker.CheckEquals(dividendsModel.DividendsQuantityAfterSplit, corporateActionsAfterSplit.Count,
                "Quantity of listed dividends *after split* does not match DividendsQuantityAfterSplit");

            LogStep(5, "Sure Total Payouts per Share value matches DividendsSumBeforeSplit+DividendsSumAfterSplit");
            var performanceTabPositionCardForm = positionCardForm.ActivateTabWithoutChartWaitingGetForm<PerformanceTabPositionCardForm>(PositionCardTabs.Performance);
            Checker.CheckEquals($"{((Currency)Parsing.ConvertToInt(new PositionsQueries().SelectAllPositionData(positionId).CurrencyId)).GetDescription()}{expectedTotalDividends}",
                performanceTabPositionCardForm.GetPerformanceTabFieldValue(PerformanceTabFieldTypes.TotalDividends).Replace("-", string.Empty).Replace(",", string.Empty),
                "Total Dividends value does not match DividendsSumBeforeSplit+DividendsSumAfterSplit truncated to four decimals");
        }

        private List<CorporateActionsModel> GetCorporateActionsBeforeSplit(List<CorporateActionsModel> corporateActions)
        {
            return corporateActions.TakeWhile(t => !t.Type.Equals(CorporateActionsType.Split)).ToList();
        }

        private List<CorporateActionsModel> GetCorporateActionsAfterSplit(List<CorporateActionsModel> corporateActions)
        {
            var corporateActionsAfterSplit = new List<CorporateActionsModel>();
            for (int i = corporateActions.Count - 1; i >= 0; i--)
            {
                if (corporateActions[i].Type.Equals(CorporateActionsType.Split))
                {
                    break;
                }
                corporateActionsAfterSplit.Add(corporateActions[i]);
            }

            return corporateActionsAfterSplit;
        }
    }
}