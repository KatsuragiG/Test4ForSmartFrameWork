using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.DsiForSymbols;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.PositionData;
using AutomatedTests.Database.Positions;
using AutomatedTests.Database.Users;
using AutomatedTests.Enums.Chart;
using AutomatedTests.Enums.Dashboard;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Forms.Charts;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Forms.ResearchPages.StockAnalyzer;
using AutomatedTests.Forms;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.BrowserSteps;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutomatedTests.Enums.Tools.StockAnalyzer;
using TradeStops.Common.Enums;
using TradeStops.Common.Extensions;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._07_Invest._01_StockAnalyzer
{
    [TestClass]
    public class TC_1365_StockAnalyzer_CheckAllExpectedElementsAreShown : BaseTestUnitTests
    {
        private const int TestNumber = 1365;

        private int expectedTabsQuantity;
        private int symbolId;
        private readonly List<PositionsDBModel> positionsModels = new List<PositionsDBModel>();

        private string tickerToSearch;
        private string expectedAutocompleteLabelWording;
        private string samePositionDropdownWording;
        private string samePositionDropdownText;
        private string samePositionDropdownItem;
        private string expectedLatestCloseLabelWording;
        private string optionName;
        private string parentTickerWording;

        private bool isSamePositionsDropdownShown;
        private bool isIndicatorShown;
        private bool isHealthPillShown;
        private bool isTrendPillShown;
        private bool isTimingPillShown;
        private bool isLikeFolioPillShown;
        private bool isCarouselShown;
        private bool isStockRatingShown;
        private bool isAddPortfolioShown;
        private bool isCalculatePositionSize;
        private bool isTradeSummary;
        private bool isAdjustShown;
        private bool isGeneralStatisticsShown;
        private bool isFundamentalsShown;
        private bool isCorporateActionsShown;
        private bool isEarningsPerShareShown;
        private bool isMyOpportunitiesTabShown;
        private bool isNewsTabShown;
        private bool isOptionTabShown;
        private bool isCompanyTabShown;
        private bool isFinancialsTabShown;
        private bool isCoinProfileTabShown;
        private bool isInsightTabShown;
        private bool isEventsBlockShown;
        private bool isTimingBlockShown;

        private string currencySign;
        private string hdLatestPrice;
        private string expectedLatestCloseDollarChange;
        private string expectedLatestClosePercentChange;
        private string expectedSingOfLatestCloseChanging;
        private string expectedColorOfLatestCloseChanging;

        private readonly PositionDataQueries positionDataQueries = new PositionDataQueries();
        private readonly PositionsQueries positionsQueries = new PositionsQueries();
        private readonly SymbolsQueries symbolsQueries = new SymbolsQueries();
        private HDSymbolStatisticsModel hdSymbolStatisticsModel;
        private PositionAssetTypes assetType;

        [TestInitialize]
        public void TestInitialize()
        {
            var userProductSubscriptions = GetUserProductSubscriptions("userSubscription");

            var portfoliosModels = new List<PortfolioModel>();
            var portfoliosQuantity = GetTestDataAsInt("portfoliosQuantity");
            for (int i = 1; i <= portfoliosQuantity; i++)
            {
                portfoliosModels.Add(new PortfolioModel
                {
                    Name = StringUtility.RandomString(GetTestDataAsString($"PortfolioName{i}")),
                    Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>($"PortfolioType{i}"),
                    Currency = GetTestDataAsString("Currency")
                });
            }

            var positionsQuantity = GetTestDataAsInt("positionsQuantity");
            for (int i = 1; i <= positionsQuantity; i++)
            {
                positionsModels.Add(new PositionsDBModel
                {
                    Symbol = GetTestDataAsString($"Symbol{i}"),
                    TradeType = ((int)GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>($"TradeType{i}")).ToString(),
                    PurchaseDate = GetTestDataAsString($"EntryDate{i}"),
                    StatusType = $"{(int)AutotestPositionStatusTypes.Open}",
                    Shares = GetTestDataAsString($"Shares{i}"),
                    Notes = portfoliosModels[i - 1].Name
                });
            }

            expectedTabsQuantity = GetTestDataAsInt(nameof(expectedTabsQuantity));
            tickerToSearch = GetTestDataAsString(nameof(tickerToSearch));
            expectedAutocompleteLabelWording = GetTestDataAsString(nameof(expectedAutocompleteLabelWording));
            samePositionDropdownWording = GetTestDataAsString(nameof(samePositionDropdownWording));
            samePositionDropdownText = string.Format(GetTestDataAsString(nameof(samePositionDropdownText)), tickerToSearch);
            samePositionDropdownItem = GetTestDataAsString(nameof(samePositionDropdownItem));
            expectedLatestCloseLabelWording = GetTestDataAsString(nameof(expectedLatestCloseLabelWording));
            optionName = GetTestDataAsString(nameof(optionName));
            parentTickerWording = GetTestDataAsString(nameof(parentTickerWording));

            isSamePositionsDropdownShown = GetTestDataAsBool(nameof(isSamePositionsDropdownShown));
            isIndicatorShown = GetTestDataAsBool(nameof(isIndicatorShown));
            isHealthPillShown = GetTestDataAsBool(nameof(isHealthPillShown));
            isTrendPillShown = GetTestDataAsBool(nameof(isTrendPillShown));
            isTimingPillShown = GetTestDataAsBool(nameof(isTimingPillShown));
            isLikeFolioPillShown = GetTestDataAsBool(nameof(isLikeFolioPillShown));
            isCarouselShown = GetTestDataAsBool(nameof(isCarouselShown));
            isStockRatingShown = GetTestDataAsBool(nameof(isStockRatingShown));
            isAddPortfolioShown = GetTestDataAsBool(nameof(isAddPortfolioShown));
            isCalculatePositionSize = GetTestDataAsBool(nameof(isCalculatePositionSize));
            isTradeSummary = GetTestDataAsBool(nameof(isTradeSummary));
            isAdjustShown = GetTestDataAsBool(nameof(isAdjustShown));
            isGeneralStatisticsShown = GetTestDataAsBool(nameof(isGeneralStatisticsShown));
            isFundamentalsShown = GetTestDataAsBool(nameof(isFundamentalsShown));
            isCorporateActionsShown = GetTestDataAsBool(nameof(isCorporateActionsShown));
            isEarningsPerShareShown = GetTestDataAsBool(nameof(isEarningsPerShareShown));
            isMyOpportunitiesTabShown = GetTestDataAsBool(nameof(isMyOpportunitiesTabShown));
            isNewsTabShown = GetTestDataAsBool(nameof(isNewsTabShown));
            isOptionTabShown = GetTestDataAsBool(nameof(isOptionTabShown));
            isCompanyTabShown = GetTestDataAsBool(nameof(isCompanyTabShown));
            isFinancialsTabShown = GetTestDataAsBool(nameof(isFinancialsTabShown));
            isCoinProfileTabShown = GetTestDataAsBool(nameof(isCoinProfileTabShown));
            isInsightTabShown = GetTestDataAsBool(nameof(isInsightTabShown));
            isEventsBlockShown = GetTestDataAsBool(nameof(isEventsBlockShown));
            isTimingBlockShown = GetTestDataAsBool(nameof(isTimingBlockShown));

            currencySign = ((Currency)positionsQueries.SelectSymbolCurrencyBySymbol(tickerToSearch)).GetDescription();
            hdSymbolStatisticsModel = positionDataQueries.SelectSymbolStatisticsForSymbol(tickerToSearch);
            expectedLatestCloseDollarChange = $"{currencySign}{hdSymbolStatisticsModel.LatestCloseDollarChange()}";
            expectedLatestClosePercentChange = hdSymbolStatisticsModel.OneDayChangeInPercent;

            StringUtility.DetectLatestCloseChangingSingAndColor(expectedLatestClosePercentChange, out expectedSingOfLatestCloseChanging, out expectedColorOfLatestCloseChanging);

            symbolId = Parsing.ConvertToInt(positionsQueries.SelectSymbolIdNameUsingSymbol(tickerToSearch).SymbolId);
            var dbAssetType = positionsQueries.SelectAssetTypeNameBySymbolId(symbolId);
            assetType = dbAssetType.EqualsIgnoreCase(SymbolTypes.Option.ToString())
                ? PositionAssetTypes.Option
                : PositionAssetTypes.Stock;
            hdLatestPrice = Parsing.ConvertToDouble(positionDataQueries.SelectStockOrOptionData(assetType, tickerToSearch).TradeClose).ToString(CultureInfo.InvariantCulture).ToFractionalString();

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));
            var usermodel = UserModels.First();
            var isUserHasAccessToPortfolios = new UsersQueries().GetUserProductFeatures(usermodel.TradeSmithUserId).PortfoliosCount > 0;

            if (isUserHasAccessToPortfolios)
            {
                var portfoliosIds = new List<int>();
                foreach (var portfolioModel in portfoliosModels)
                {
                    portfoliosIds.Add(PortfoliosSetUp.AddManualPortfolio(usermodel.Email, portfolioModel));
                }

                PositionsAlertsSetUp.AddPositionViaDB(portfoliosIds.First(), positionsModels.First());
                PositionsAlertsSetUp.AddPositionViaDB(portfoliosIds.Last(), positionsModels.Last());
            }

            LoginSetUp.LogIn(usermodel);
            new MainMenuForm().SetSymbol(tickerToSearch);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1365$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("Test checks that all elements are existing on the Stock Analyzer page. https://tr.a1qa.com/index.php?/cases/view/22165304")]
        [TestCategory("StockAnalyzer")]
        public override void RunTest()
        {
            LogStep(1, "Check that Stock Analyzer page is displayed");
            var stockAnalyzerForm = new StockAnalyzerForm();
            stockAnalyzerForm.AssertIsOpen();

            LogStep(2, "Check that the description about ticker is displayed");
            Checker.CheckContains(expectedAutocompleteLabelWording, stockAnalyzerForm.GetAutocompleteLabelWording(),
                "Stock Analyzer Autocomplete Wording is not as expected");
            Checker.CheckEquals(tickerToSearch, stockAnalyzerForm.GetSymbolTreeSelectSingleValue(),
                "Ticker is not as expected");
            Checker.IsTrue(stockAnalyzerForm.IsMagnifyClassIconPresent(), $"Field with magnifying glass icon is not shown for {tickerToSearch}");
            CheckSamePositionsDropdown(stockAnalyzerForm);

            LogStep(3, "Check that the information about ticker price is displayed");
            Checker.CheckEquals(string.Format(expectedLatestCloseLabelWording, Parsing.ConvertToShortDateString(hdSymbolStatisticsModel.TradeDate)),
                stockAnalyzerForm.GetLatestCloseLabelWording(),
                $"Latest close label is not as expected for {tickerToSearch}");
            Checker.CheckEquals($"{currencySign}{hdLatestPrice}", stockAnalyzerForm.GetLatestClose().Replace(",", string.Empty),
                $"Latest close price is not as expected for {tickerToSearch}");
            CheckLatestCloseChanging(stockAnalyzerForm);

            LogStep(4, "Check ticker name");
            var expectedName = assetType == PositionAssetTypes.Option
                ? optionName
                : positionsQueries.SelectPositionNameByTickersForStocksWithPairTrades(tickerToSearch).ToUpperInvariant();
            Checker.CheckEquals(expectedName, stockAnalyzerForm.GetPositionName(), $"Name is not as expected for {tickerToSearch}");

            LogStep(5, "Check exchange name");

            if (assetType != PositionAssetTypes.Option)
            {
                Checker.CheckEquals(symbolsQueries.SelectDataFromHDSymbols(symbolId).ExchangeName,
                    stockAnalyzerForm.GetPositionExchangeName(), $"Exchange Name is not as expected for {tickerToSearch}");
            }

            LogStep(6, "Check Option name");
            CheckOptionName(stockAnalyzerForm);

            LogStep(7, "Check that Indicator block");
            var actualIndicatorPresence = stockAnalyzerForm.IsIndicatorLabelShown();
            Checker.CheckEquals(isIndicatorShown, actualIndicatorPresence, $"Indicator label is not as expected for {tickerToSearch}");
            if (actualIndicatorPresence && isIndicatorShown)
            {
                Checker.IsTrue(stockAnalyzerForm.Pills.IsPillPresent(PillsType.Vq), $"Vq pill visibility is not as expected for {tickerToSearch}");
                Checker.CheckEquals(isHealthPillShown, stockAnalyzerForm.Pills.IsPillPresent(PillsType.Health), $"Health pill visibility is not as expected for {tickerToSearch}");
                Checker.CheckEquals(isTrendPillShown, stockAnalyzerForm.Pills.IsPillPresent(PillsType.HealthTrend), $"Trend pill visibility is not as expected for {tickerToSearch}");
                Checker.CheckEquals(isTimingPillShown, stockAnalyzerForm.Pills.IsPillPresent(PillsType.Timing), $"Timing pill visibility is not as expected for {tickerToSearch}");
                Checker.CheckEquals(isLikeFolioPillShown, stockAnalyzerForm.Pills.IsPillPresent(PillsType.Likefolio), $"Likefolio pill visibility is not as expected for {tickerToSearch}");
            }

            LogStep(8, "Check that Carousel is displayed.");
            var actualCarouselPresence = stockAnalyzerForm.IsCarouselShown();
            Checker.CheckEquals(isCarouselShown, actualCarouselPresence, $"Carousel is not as expected for {tickerToSearch}");

            LogStep(9, "Click on carousel and press arrow button. Check that Stock Rating chart is displayed.");
            if (actualCarouselPresence)
            {
                stockAnalyzerForm.ClickCarouselArrow(CarouselActionArrows.Next);
                Checker.CheckEquals(isStockRatingShown, stockAnalyzerForm.Speedometer.IsExists(), $"Speedometer is not as expected for {tickerToSearch}");
            }

            LogStep(10, "Check that 'Add to Portfolio' button is shown.");
            Checker.CheckEquals(isAddPortfolioShown, stockAnalyzerForm.IsAdditionalActionsButtonPresent(StockAnalyzerAdditionalButtonTypes.AddToPortfolio),
                $"Add To Portfolio is not as expected for {tickerToSearch}");

            LogStep(11, "Check that 'Calculate Position Size' button is shown.");
            Checker.CheckEquals(isCalculatePositionSize, stockAnalyzerForm.IsAdditionalActionsButtonPresent(StockAnalyzerAdditionalButtonTypes.CalculatePositionSize),
                $"Calculate Position Size is not as expected for {tickerToSearch}");

            LogStep(12, "Check that 'Trade Summary' button is shown.");
            Checker.CheckEquals(isTradeSummary, stockAnalyzerForm.IsAdditionalActionsButtonPresent(StockAnalyzerAdditionalButtonTypes.TradeSummary),
                $"Trade Summary is not as expected for {tickerToSearch}");

            LogStep(13, "Check that expected tabs labels are displayed.");
            Checker.CheckEquals(expectedTabsQuantity, stockAnalyzerForm.GetTabsQuantity(),
                $"Tabs quantity is not as expected for {tickerToSearch}");

            LogStep(14, "Check that 'Stop Loss Analysis' tab open after clicking on label.");
            stockAnalyzerForm.ActivateTabWithoutChartWaiting(StockAnalyzerTabs.StopLossAnalysis);
            var stopLossAnalysisTabForm = new StopLossAnalysisTabForm();

            LogStep(15, "Check that adjust radiobox is shown according to {test data: isAdjustShown}.");
            Checker.CheckEquals(isAdjustShown, stopLossAnalysisTabForm.IsAdjustTypePresent(AdjustmentType.Adjusted),
                $"Adjusting is not as expected for {tickerToSearch}");

            LogStep(16, "Check that 'Statistic' tab open after clicking on label.");
            stockAnalyzerForm.ActivateTabWithoutChartWaiting(StockAnalyzerTabs.Statistics);
            var statisticTabForm = new StatisticTabForm();
            Checker.CheckEquals(isGeneralStatisticsShown, statisticTabForm.IsGeneralStatisticBlockPresent(),
                $"General Statistics is not as expected for {tickerToSearch}");
            Checker.CheckEquals(isFundamentalsShown, statisticTabForm.IsFundamentalsBlockPresent(),
                $"Fundamentals is not as expected for {tickerToSearch}");
            Checker.CheckEquals(isCorporateActionsShown, statisticTabForm.IsCorporateActionsBlockPresent(),
                $"Corporate Actions is not as expected for {tickerToSearch}");
            Checker.CheckEquals(isEarningsPerShareShown, statisticTabForm.IsEarningBlockPresent(),
                $"Earnings Per Share is not as expected for {tickerToSearch}");

            LogStep(17, "Check that 'My Opportunities' tab open after clicking on label.");
            var actualIsMyOpportunitiesTabShown = stockAnalyzerForm.IsTabPresent(StockAnalyzerTabs.MyOpportunities);
            Checker.CheckEquals(isMyOpportunitiesTabShown, actualIsMyOpportunitiesTabShown,
                $"My Opportunities tab is not as expected for {tickerToSearch}");
            if (actualIsMyOpportunitiesTabShown && isMyOpportunitiesTabShown)
            {
                stockAnalyzerForm.ActivateTabWithoutChartWaiting(StockAnalyzerTabs.MyOpportunities);
                new MyOpportunitiesTabForm().AssertIsOpen();
            }

            LogStep(18, "Check that 'News' tab open after clicking on label.");
            var actualIsNewsTabShown = stockAnalyzerForm.IsTabPresent(StockAnalyzerTabs.News);
            Checker.CheckEquals(isNewsTabShown, actualIsNewsTabShown,
                $"News tab is not as expected for {tickerToSearch}");
            if (actualIsNewsTabShown && isNewsTabShown)
            {
                stockAnalyzerForm.ActivateTabWithoutChartWaiting(StockAnalyzerTabs.News);
                new NewsTabForm().AssertIsOpen();
            }

            LogStep(19, "Check that 'Options' tab open after clicking on label.");
            var actualIsOptionTabShown = stockAnalyzerForm.IsTabPresent(StockAnalyzerTabs.Options);
            Checker.CheckEquals(isOptionTabShown, actualIsOptionTabShown,
                $"Options tab is not as expected for {tickerToSearch}");
            if (actualIsOptionTabShown && isOptionTabShown)
            {
                stockAnalyzerForm.ActivateTabWithoutChartWaiting(StockAnalyzerTabs.Options);
                new OptionsTabCommonForm().AssertIsOpen();
            }

            LogStep(20, "Check that 'Company Profile' tab open after clicking on label.");
            var actualIsCompanyTabShown = stockAnalyzerForm.IsTabPresent(StockAnalyzerTabs.CompanyProfile);
            Checker.CheckEquals(isCompanyTabShown, actualIsCompanyTabShown,
                $"Company Profile tab is not as expected for {tickerToSearch}");
            if (actualIsCompanyTabShown && isCompanyTabShown)
            {
                stockAnalyzerForm.ActivateTabWithoutChartWaiting(StockAnalyzerTabs.CompanyProfile);
                new CompanyProfileTabForm().AssertIsOpen();
            }

            LogStep(21, "Check that 'Financial' tab open after clicking on label.");
            var actualIsFinancialsTabShown = stockAnalyzerForm.IsTabPresent(StockAnalyzerTabs.Financial);
            Checker.CheckEquals(isFinancialsTabShown, actualIsFinancialsTabShown,
                $"Financial tab is not as expected for {tickerToSearch}");
            if (actualIsFinancialsTabShown && isFinancialsTabShown)
            {
                stockAnalyzerForm.ActivateTabWithoutChartWaiting(StockAnalyzerTabs.Financial);
                new FinancialsTabForm().AssertIsOpen();
            }

            LogStep(22, "Check that 'Coin profile' tab open after clicking on label.");
            var actualIsCoinProfileTabShown = stockAnalyzerForm.IsTabPresent(StockAnalyzerTabs.CoinProfile);
            Checker.CheckEquals(isCoinProfileTabShown, actualIsCoinProfileTabShown,
                $"Coin profile tab is not as expected for {tickerToSearch}");
            if (actualIsCoinProfileTabShown && isCoinProfileTabShown)
            {
                stockAnalyzerForm.ActivateTabWithoutChartWaiting(StockAnalyzerTabs.CoinProfile);
                new CoinProfileTabForm().AssertIsOpen();
            }

            LogStep(23, "Check that 'Insight' tab open after clicking on label.");
            var actualInsightTabShown = stockAnalyzerForm.IsTabPresent(StockAnalyzerTabs.Insights);
            Checker.CheckEquals(isInsightTabShown, actualInsightTabShown,
                $"Insight tab is not as expected for {tickerToSearch}");
            if (actualInsightTabShown && isInsightTabShown)
            {
                stockAnalyzerForm.ActivateTabWithoutChartWaiting(StockAnalyzerTabs.Insights);
                new InsightsTabForm().AssertIsOpen();
            }

            LogStep(24, "Check that 'Chart' tab open after clicking on label.");
            stockAnalyzerForm.ActivateTabWithoutChartWaiting(StockAnalyzerTabs.ChartSettings);
            var chartSettingsTabForm = new ChartSettingsTabForm();
            Checker.IsTrue(chartSettingsTabForm.IsOpenChartSettingsShown(), $"Open Chart Settings is not shown for {tickerToSearch}");

            LogStep(25, "Check than options of the chart is opened after clicking on 'Open Chart Setting' button.");
            chartSettingsTabForm.OpenChartSettings();
            Checker.IsTrue(chartSettingsTabForm.IsSectionLabelPresent(ChartCheckboxCategoryTypes.Lines.GetStringMapping()),
                $"Lines block is not present for {tickerToSearch}");
            Checker.CheckEquals(isEventsBlockShown,
                chartSettingsTabForm.IsSectionLabelPresent(ChartCheckboxCategoryTypes.Events.GetStringMapping()),
                $"Events block is unexpected on the page for {tickerToSearch}");
            Checker.CheckEquals(isTimingBlockShown,
                chartSettingsTabForm.IsSectionLabelPresent(ChartCheckboxCategoryTypes.Timing.GetStringMapping()),
                $"Timing block is unexpected on the page for {tickerToSearch}");
            Checker.IsTrue(chartSettingsTabForm.IsSectionLabelPresent(ChartCheckboxCategoryTypes.Charts.GetStringMapping()),
                $"Charts block is not present for {tickerToSearch}");
        }

        private void CheckOptionName(StockAnalyzerForm stockAnalyzerForm)
        {
            if (assetType == PositionAssetTypes.Option)
            {
                var parentSymbolModel = symbolsQueries.SelectParentStockModelByOptionSymbolId(symbolId.ToString());
                Checker.CheckEquals($"{parentTickerWording} {parentSymbolModel.SymbolName}",
                    stockAnalyzerForm.GetParentStockLabelWording().Replace("\r\n", " "),
                    $"Wording for parent ticker is not as expected for {tickerToSearch}");
                Checker.CheckEquals(parentSymbolModel.SymbolName,
                    stockAnalyzerForm.GetParentStockLinkText(), $"Wording for parent ticker link is not as expected for {tickerToSearch}");
                stockAnalyzerForm.ClickParentStockLInk();
                new BrowserSteps().CheckThatNewTabOpensPerformActionWithSwitchToNewTabBackAfterClosing(() =>
                    CheckParentStockAnalyzerPage(parentSymbolModel));
            }
            else
            {
                Checker.IsFalse(stockAnalyzerForm.IsParentStockLinkShown(), $"Link for parent ticker is shown for {tickerToSearch}");
            }
        }

        private void CheckLatestCloseChanging(StockAnalyzerForm stockAnalyzerForm)
        {
            if (stockAnalyzerForm.IsLatestCloseChangingShown())
            {
                var latestCloseChanging = stockAnalyzerForm.GetLatestCloseChanging().Replace(")", string.Empty).Trim().Split('(');
                Checker.CheckEquals(StringUtility.SetFormatFromSample(expectedLatestCloseDollarChange.DeleteMathSigns(), latestCloseChanging[0].DeleteMathSigns()),
                    StringUtility.ReplaceAllCurrencySigns(latestCloseChanging[0].DeleteMathSigns().Trim()),
                    $"Latest Close change $ value is not as expected for {tickerToSearch}");
                Checker.IsTrue(latestCloseChanging[0].Contains(expectedSingOfLatestCloseChanging),
                    $"Latest  change $ direction is not {expectedSingOfLatestCloseChanging} for {tickerToSearch}");
                Checker.CheckEquals(currencySign,
                    Constants.AllCurrenciesRegex.Match(latestCloseChanging[0].DeleteMathSigns()).Value,
                    $"Currency sign for Latest Close change $ value is not {expectedLatestCloseDollarChange}");
                var latestClosePercentValue = StringUtility.SetFormatFromSample(expectedLatestClosePercentChange.DeleteMathSigns(),
                    latestCloseChanging[1].DeleteMathSigns().Replace("%", string.Empty));
                Checker.CheckEquals(latestClosePercentValue, latestCloseChanging[1].DeleteMathSigns().Replace("%", string.Empty),
                    $"Latest Close change % value is not {expectedLatestClosePercentChange} for {tickerToSearch}");
                Checker.CheckContains(expectedColorOfLatestCloseChanging, stockAnalyzerForm.GetLatestCloseChangingColor(),
                    $"Latest Close change color is not as expected for {tickerToSearch}");
            }
        }

        private void CheckSamePositionsDropdown(StockAnalyzerForm stockAnalyzerForm)
        {
            var actualSamePositionsDropdownShown = stockAnalyzerForm.IsDropdownYouHaveSamePositionsPresent();
            Checker.CheckEquals(isSamePositionsDropdownShown, actualSamePositionsDropdownShown, $"Dropdown for same positions is not shown for {tickerToSearch}");
            if (isSamePositionsDropdownShown && actualSamePositionsDropdownShown)
            {
                Checker.CheckContains(samePositionDropdownWording, stockAnalyzerForm.GetYouHaveSamePositionsLabelWording(), "Wording for same positions label is not as expected");
                Checker.CheckContains(samePositionDropdownText, stockAnalyzerForm.GetDropdownYouHaveSamePositionsText(), "Wording for same positions dropdown is not as expected");
                var expectedDropdownItemModels = positionsModels.Where(t => t.Symbol.EqualsIgnoreCase(tickerToSearch)).ToList();
                var expectedDropdownItems = new List<string> { samePositionDropdownText };
                foreach (var expectedDropdownItemModel in expectedDropdownItemModels)
                {
                    expectedDropdownItems.Add(string.Format(samePositionDropdownItem, expectedDropdownItemModel.Shares, expectedDropdownItemModel.Notes));
                }
                var actualDropdownItems = stockAnalyzerForm.GetAllPositionsFromFromYouHaveSamePositionsDropdown();
                Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedDropdownItems, actualDropdownItems),
                    $"Same Positions Dropdown items are not as expected: {GetExpectedResultsString(expectedDropdownItems)}\r\n{GetActualResultsString(actualDropdownItems)}");
            }
        }

        private void CheckParentStockAnalyzerPage(SymbolModel symbolModel)
        {
            var stockAnalyzerForm = new StockAnalyzerForm();
            stockAnalyzerForm.AssertIsOpen();

            Checker.CheckEquals(symbolModel.Symbol, stockAnalyzerForm.GetSymbolTreeSelectSingleValue(),
                "Parent ticker is not as expected");
            Checker.CheckEquals(symbolModel.SymbolId, Constants.NumbersRegex.Match(Browser.GetDriver().Url).Value,
                "Parent ticker SymbolId is not as expected");
        }
    }
}