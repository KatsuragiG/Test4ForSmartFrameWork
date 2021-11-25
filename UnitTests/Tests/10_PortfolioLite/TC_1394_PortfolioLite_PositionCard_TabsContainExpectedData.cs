using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.PositionData;
using AutomatedTests.Database.Positions;
using AutomatedTests.Database.PositionStats;
using AutomatedTests.Database.Users;
using AutomatedTests.Enums.Chart;
using AutomatedTests.Enums.PortfolioLite;
using AutomatedTests.Enums.PositionCard;
using AutomatedTests.Enums;
using AutomatedTests.Forms.PortfolioLite;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Models.PositionsModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.PortfolioLiteSteps;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._10_PortfolioLite
{
    [TestClass]
    public class TC_1394_PortfolioLite_PositionCard_TabsContainExpectedData : BaseTestUnitTests
    {
        private const int TestNumber = 1394;

        private PortfolioLitePositionModel positionModel;
        private List<string> statisticSectionsTitles;
        private readonly PositionsQueries positionsQueries = new PositionsQueries();
        private readonly PositionDataQueries positionDataQueries = new PositionDataQueries();

        private int symbolId;
        private int quantityOfAddedPositions;
        private bool isCorporateShown;
        private string corporateDescription;
        private string statisticsDescription;
        private string currencySign;
        private string expectedPortfolioPercent;

        [TestInitialize]
        public void TestInitialize()
        {
            quantityOfAddedPositions = GetTestDataAsInt(nameof(quantityOfAddedPositions));
            positionModel = new PortfolioLitePositionModel
            {
                Ticker = GetTestDataAsString("Symbol"),
                BuyDate = GetTestDataAsString("EntryDate"),
                Qty = GetTestDataAsString("Shares"),
                BuyPrice = GetTestDataAsString("EntryPrice"),
                IsLongType = GetTestDataAsBool("IsLongType"),
                Currency = GetTestDataParsedAsEnumFromStringMapping<Currency>("Currency")
            };

            currencySign = positionModel.Currency.GetDescription();
            isCorporateShown = GetTestDataAsBool(nameof(isCorporateShown));
            corporateDescription = GetTestDataAsString(nameof(corporateDescription));
            expectedPortfolioPercent = GetTestDataAsString(nameof(expectedPortfolioPercent));
            statisticsDescription = string.Format(GetTestDataAsString(nameof(statisticsDescription)),
                Parsing.ConvertToShortDateString(positionDataQueries.SelectLastTradeDate(positionModel.Ticker)));
            statisticSectionsTitles = GetTestDataValuesAsListByColumnName(nameof(statisticSectionsTitles));
            symbolId = Parsing.ConvertToInt(positionsQueries.SelectSymbolIdNameUsingSymbol(positionModel.Ticker).SymbolId);

            LogStep(0, "Preconditions. Create user with subscription to PortfolioLite");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.PortfolioLite));
            var fakeSnaid = StringUtility.RandomString(Constants.SnaidPattern);
            new UsersQueries().UpdateUserSnaid(fakeSnaid, UserModels.First().TradeSmithUserId);
            new PortfolioLiteNavigation().OpenPortfolioLiteWithUserGuid(UserModels.First());

            var portfolioLiteMainForm = new PortfolioLiteMainForm();
            portfolioLiteMainForm.ClickAddAPosition();
            portfolioLiteMainForm.AddPosition(positionModel);
            portfolioLiteMainForm.ExitFrame();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1394$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("PortfolioLite"), TestCategory("PortfolioCard")]
        [Description("Test checks that tabs contain expected data. https://tr.a1qa.com/index.php?/cases/view/21116196")]
        public override void RunTest()
        {
            LogStep(1, "Click Chart tab");
            var portfolioLiteMainForm = new PortfolioLiteMainForm();
            portfolioLiteMainForm.ClickPositionLinkInGridByNumber(quantityOfAddedPositions);
            var portfolioLiteCardForm = new PortfolioLiteCardForm();
            var portfolioLiteChartTabForm = portfolioLiteCardForm.ActivateTabGetForm<PortfolioLiteChartTabForm>(PortfolioLiteCardTabs.Chart);
            portfolioLiteChartTabForm.AssertIsOpen();

            LogStep(2, "Check that Chart tab contains expected data");
            portfolioLiteCardForm.Chart.SelectChartPeriod(ChartPeriod.ThreeYears);
            Checker.IsTrue(portfolioLiteCardForm.Chart.IsChartLinePresent(ChartLineTypes.EntryDate),
                "Entry Date line on the position card is not shown");
            var portfolioLiteSteps = new PortfolioLiteSteps();
            portfolioLiteSteps.CheckChartTabData(portfolioLiteCardForm.Chart);

            LogStep(3, "Click Statistics tab and check sections headers");
            var statisticTabForm = portfolioLiteCardForm.ActivateTabWithoutChartWaitingGetForm<StatisticTabForm>(PortfolioLiteCardTabs.Statistics);
            statisticTabForm.AssertIsOpen();
            var tabTitles = statisticTabForm.GetSectionTitles();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(statisticSectionsTitles, tabTitles),
                $"Statistics tab headers are not as expected: {GetExpectedResultsString(statisticSectionsTitles)}\r\n{ GetActualResultsString(tabTitles)}");

            LogStep(4, "Check that Statistics tab contains expected Corporate data");
            portfolioLiteSteps.CheckCorporateActionsData(statisticTabForm, positionModel.BuyDate, symbolId, corporateDescription, isCorporateShown);

            LogStep(5, "Check that Statistics tab contains expected general statistics data");
            portfolioLiteSteps.CheckGeneralStatisticData(statisticTabForm, positionModel.Ticker, currencySign, portfolioLiteCardForm.GetLatestClose(), statisticsDescription);

            LogStep(6, "Click Performance tab and check 10 labels for data");
            var performanceTabPositionCardForm = portfolioLiteCardForm.ActivateTabWithoutChartWaitingGetForm<PerformanceTabPositionCardForm>(PortfolioLiteCardTabs.Performance);
            performanceTabPositionCardForm.AssertIsOpen();
            Checker.CheckListsEquals(performanceTabPositionCardForm.expectedPerformanceStockLabels,
                performanceTabPositionCardForm.GetAllLabelTitles(),
                $"Labels for Performance tab fields are not as expected for {positionModel.Ticker}");

            LogStep(7, "Check that Performance tab contains expected data");
            CheckPerformanceData(portfolioLiteCardForm, performanceTabPositionCardForm);
        }

        private void CheckPerformanceData(PortfolioLiteCardForm portfolioLiteCardForm, PerformanceTabPositionCardForm performanceTabPositionCardForm)
        {
            var positionId = portfolioLiteCardForm.GetPositionIdFromElement();
            var positionData = positionsQueries.SelectAllPositionData(positionId);
            var positionStatisticData = new PositionStatsQueries().SelectPositionStatisticData(positionId);
            var expectedCostBasis = string.IsNullOrEmpty(positionData.CostBasis)
                ? Constants.NotAvailableAcronym
                : $"{currencySign}{positionData.CostBasis.ToFractionalString()}";
            Checker.CheckEquals(expectedCostBasis,
                performanceTabPositionCardForm.GetPerformanceTabFieldValue(PerformanceTabFieldTypes.CostBasis),
                $"Cost Basis value is not matched with DB for {positionModel.Ticker}");
            var expectedChangesSign = positionModel.IsLongType.HasValue && (bool)positionModel.IsLongType || Parsing.ConvertToDecimal(positionData.Value) == 0
                ? string.Empty
                : Constants.MinusSign;
            Checker.CheckEquals($"{expectedChangesSign}{currencySign}{positionData.Value.ToFractionalString()}",
                performanceTabPositionCardForm.GetPerformanceTabFieldValue(PerformanceTabFieldTypes.Value),
                $"Value is not matched with DB for {positionModel.Ticker}");
            Checker.CheckEquals(expectedPortfolioPercent,
                performanceTabPositionCardForm.GetPerformanceTabFieldValue(PerformanceTabFieldTypes.PortfolioPercent),
                $"Portfolio Percent value is not matched expectations for {positionModel.Ticker}");
            var expectedTotalDividends = string.IsNullOrEmpty(positionData.DividendsTotal)
                ? Constants.NotAvailableAcronym
                : $"{currencySign}{positionData.DividendsTotal.ToFractionalString()}";
            Checker.CheckEquals(expectedTotalDividends,
                performanceTabPositionCardForm.GetPerformanceTabFieldValue(PerformanceTabFieldTypes.TotalDividends),
                $"Total Dividends value is not matched with DB for {positionModel.Ticker}");
            var expectedGainPerShare = string.IsNullOrEmpty(positionData.GainPerShare)
                ? Constants.NotAvailableAcronym
                : $"{expectedChangesSign}{currencySign}{positionData.GainPerShare.DeleteMathSigns().ToFractionalString()}";
            Checker.CheckEquals(expectedGainPerShare,
                performanceTabPositionCardForm.GetPerformanceTabFieldValue(PerformanceTabFieldTypes.GainPerShare),
                $"Gain Per Share value is not matched with DB for {positionModel.Ticker}");
            expectedChangesSign = !string.IsNullOrEmpty(positionData.TotalGain) && Parsing.ConvertToDouble(positionData.TotalGain) < 0
                ? Constants.MinusSign
                : string.Empty;
            var expectedTotalGain = string.IsNullOrEmpty(positionData.TotalGain)
                ? $"{Constants.NotAvailableAcronym} ({Constants.NotAvailableAcronym})"
                : $"{expectedChangesSign}{currencySign}{positionData.TotalGain.DeleteMathSigns().ToFractionalString()} ({positionData.PercentGain.ToFractionalString()}%)";
            Checker.CheckEquals(expectedTotalGain,
                performanceTabPositionCardForm.GetPerformanceTabFieldValue(PerformanceTabFieldTypes.TotalGain),
                $"Total Gain is not matched with DB for {positionModel.Ticker}");
            var extremumDate = (bool)positionModel.IsLongType
                ? positionStatisticData.HighestCloseDate
                : positionStatisticData.LowestCloseDate;
            Checker.CheckEquals(Parsing.ConvertToShortDateString(extremumDate),
                performanceTabPositionCardForm.GetPerformanceTabFieldValue(PerformanceTabFieldTypes.MaxProfitableCloseDate),
                $"Max Profitable Close Date is not matched expectations for {positionModel.Ticker}");
            var extremumClose = (bool)positionModel.IsLongType
                ? positionStatisticData.HighestClosePrice
                : positionStatisticData.LowestClosePrice;
            Checker.CheckEquals($"{currencySign}{extremumClose.ToFractionalString()}",
                performanceTabPositionCardForm.GetPerformanceTabFieldValue(PerformanceTabFieldTypes.MaxProfitableClose),
                $"Max Profitable Close is not matched with DB for {positionModel.Ticker}");
            Checker.CheckEquals($"{positionData.PercentOffHLClose.ToFractionalString()}%",
                performanceTabPositionCardForm.GetPerformanceTabFieldValue(PerformanceTabFieldTypes.PercentOffHigh),
                $"% Off High value is not matched expectations for {positionModel.Ticker}");
            Checker.CheckEquals(positionData.HoldPeriod,
                performanceTabPositionCardForm.GetPerformanceTabFieldValue(PerformanceTabFieldTypes.DaysHeld).Replace(",", string.Empty),
                $"Days Held is not matched expectations for {positionModel.Ticker}");
        }
    }
}