using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Database.Timings;
using AutomatedTests.Enums.FilterEnums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.Timing;
using AutomatedTests.Enums;
using AutomatedTests.Forms.ResearchPages.StockAnalyzer;
using AutomatedTests.Forms.TimingCalendar;
using AutomatedTests.Forms;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System;
using TradeStops.Common.Enums;
using TradeStops.Common.Utils;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;
using WebdriverFramework.Framework.WebDriver;

namespace UnitTests.Tests._11_Calendar
{
    [TestClass]
    public class TC_1384_Calendar_CheckFiltersApplying : BaseTestUnitTests
    {
        private const int TestNumber = 1384;
        private const int TickersCountToOpenInStackAnalyzer = 3;

        private int stepNumber;
        private int quantityOfPortfolios;
        private bool isPaginationExpected;
        private CalendarOverviewPeriodTypes expectedOverviewPeriod;
        private List<string> sourcesToSelect;
        private List<string> expectedAssetTypes;
        private List<string> expectedHealthes;
        private List<string> expectedPeakValleyValues;
        private List<string> expectedConvictionLevels;
        private readonly TimingsQueries timingQueries = new TimingsQueries();

        [TestInitialize]
        public void TestInitialize()
        {
            var userProductSubscriptions = GetUserProductSubscriptions("userSubscription");

            quantityOfPortfolios = GetTestDataAsInt(nameof(quantityOfPortfolios));
            var portfoliosModels = new List<PortfolioDBModel>();
            for (int i = 1; i <= quantityOfPortfolios; i++)
            {
                portfoliosModels.Add(new PortfolioDBModel
                {
                    Name = GetTestDataAsString($"PortfolioName{i}"),
                    Type = $"{(int)GetTestDataParsedAsEnumFromStringMapping<PortfolioType>($"PortfolioType{i}")}",
                    CurrencyId = $"{(int)GetTestDataParsedAsEnumFromStringMapping<Currency>($"Currency{i}")}",
                });
            }
            expectedOverviewPeriod = GetTestDataParsedAsEnumFromStringMapping<CalendarOverviewPeriodTypes>(nameof(expectedOverviewPeriod));
            isPaginationExpected = GetTestDataAsBool(nameof(isPaginationExpected));

            var positionsQuantity = new List<int>();
            positionsQuantity = GetTestDataValuesAsListByColumnName(nameof(positionsQuantity)).Select(Parsing.ConvertToInt).ToList();
            sourcesToSelect = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(sourcesToSelect));
            expectedAssetTypes = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(expectedAssetTypes));
            expectedHealthes = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(expectedHealthes));
            expectedPeakValleyValues = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(expectedPeakValleyValues));
            expectedConvictionLevels = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(expectedConvictionLevels));

            LogStep(stepNumber++, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));

            for (int i = 0; i < quantityOfPortfolios; i++)
            {
                var portfolioId = PortfoliosSetUp.AddPortfolioViaDb(UserModels.First(), portfoliosModels[i]);

                var assetType = GetTestDataAsString($"assetTypePortfolio{i + 1}");
                var healthType = (HealthZoneTypes)GetTestDataAsInt($"healthPortfolio{i + 1}");
                var turnAreaPortfolio = (DbTimingTurnAreaTypes)GetTestDataAsInt($"turnAreaPortfolio{i + 1}");
                var levelPortfolio = (TimingTurnStrengthTypes)GetTestDataAsInt($"levelPortfolio{i + 1}");
                var timingScheduleTypes = GetTestDataParsedAsEnumFromStringMapping<TimingScheduleTypes>($"schedulePortfolio{i + 1}");

                var tickersCalendarModels = timingQueries.SelectTickersforTimingCalendar(positionsQuantity[i], healthType, turnAreaPortfolio, levelPortfolio, timingScheduleTypes, assetType);
                Checker.CheckEquals(positionsQuantity[i], tickersCalendarModels.Count,
                    $@"Unexpected positions quantity for Health = '{healthType}', Turn area = '{turnAreaPortfolio}', 
                    Asset = '{assetType}', Conviction = '{levelPortfolio}', scheduling = '{timingScheduleTypes}'");
                foreach (var tickerCalendarModels in tickersCalendarModels)
                {
                    var positionModel = new PositionsDBModel
                    {
                        Symbol = tickerCalendarModels.Symbol,
                        TradeType = $"{(int)PositionTradeTypes.Long}"
                    };
                    PositionsAlertsSetUp.AddPositionViaDB(portfolioId, positionModel);
                }
            }

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Calendar);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1384$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("https://tr.a1qa.com/index.php?/cases/view/21131068 The test checks that only adequate tickers are shown in grid after filtration")]
        [TestCategory("Smoke"), TestCategory("Calendar")]
        public override void RunTest()
        {
            LogStep(stepNumber++, "Check that default grid is not empty");
            var timingCalendarForm = new TimingCalendarForm();
            Checker.IsTrue(timingCalendarForm.IsResultGridPresent(), "Calendar grid is not shown");

            LogStep(stepNumber++, "Select required items in sources dropdown");
            timingCalendarForm.SelectSourcesMultipleByListOfText(sourcesToSelect);
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(sourcesToSelect, timingCalendarForm.GetSelectedItemsInSourceDropdown()),
                "Items in Source dropdown are not as expected");

            LogStep(stepNumber++, "Click Filters");
            timingCalendarForm.ClickFiltersButton();
            foreach (var calendarFilterType in EnumUtils.GetValues<TimingCalendarFilterTypes>())
            {
                Checker.IsTrue(timingCalendarForm.IsFilterExist(calendarFilterType),
                    $"{calendarFilterType.GetStringMapping()} filter is not shown");
            }

            DoStepsWithClearingAndSettingValuesInFilterDropdowns(TimingCalendarFilterTypes.AssetType, expectedAssetTypes);

            DoStepsWithClearingAndSettingValuesInFilterDropdowns(TimingCalendarFilterTypes.Health, expectedHealthes);

            DoStepsWithClearingAndSettingValuesInFilterDropdowns(TimingCalendarFilterTypes.PeakAndValley, expectedPeakValleyValues);

            DoStepsWithClearingAndSettingValuesInFilterDropdowns(TimingCalendarFilterTypes.ConvictionLevel, expectedConvictionLevels);

            LogStep(stepNumber++, "Click 3 or 6 month button (see test data)");
            timingCalendarForm.ClickPeriodButton(expectedOverviewPeriod);
            Checker.IsTrue(timingCalendarForm.IsPeriodButtonActive(expectedOverviewPeriod), "Overview button is not highlighted");

            LogStep(stepNumber++, "Click Apply filter");
            timingCalendarForm.ClickFiltersApplyingButton(FiltersApplyingButtonTypes.Apply);
            foreach (var calendarFilterType in EnumUtils.GetValues<TimingCalendarFilterTypes>())
            {
                Checker.IsFalse(timingCalendarForm.IsFilterExist(calendarFilterType),
                    $"{calendarFilterType.GetStringMapping()} filter is shown after applying");
            }
            Checker.IsTrue(timingCalendarForm.IsPeriodButtonActive(expectedOverviewPeriod), "Overview button is not highlighted after filtration");

            var isResultGridPresent = timingCalendarForm.IsResultGridPresent();
            Checker.IsTrue(isResultGridPresent, "Calendar grid is not shown after filtration");
            Checker.CheckEquals(isPaginationExpected, timingCalendarForm.IsPaginationExist(), "Pagination is not as expected after filtration");

            var resultsInGrd = timingCalendarForm.GetNumberOfRowsInCalendarGrid();
            if (isResultGridPresent)
            {
                Checker.IsTrue(resultsInGrd > 0, $"Calendar grid contains unexpected elements {resultsInGrd}");
            }

            var tickersInGrid = timingCalendarForm.GetTickersColumnValues().Select(t => t.Split('\r')[0]).ToList();
            var healthInGrid = timingCalendarForm.GetHealthColumnValues();
            var healthZoneTypes = healthInGrid.Select(t => Dictionaries.TypeOfHealthTypeReverse.GetValueByPartOfTheKeyOrDefault(t, defaultValue: HealthZoneTypes.NotAvailable)).ToList();
            var timingDirectionTypes = new List<TimingDirectionTypes>();
            var timingConvictionsLevels = new List<TimingConvictionLevelTypes>();
            var positionsQueries = new PositionsQueries();

            for (int i = 0; i < tickersInGrid.Count; i++)
            {
                var isTickerExist = !string.IsNullOrEmpty(tickersInGrid[i]);
                Checker.IsTrue(isTickerExist, $"Ticker's row {i + 1} does not contain symbol");
                if (isTickerExist)
                {
                    var symbolId = Parsing.ConvertToInt(positionsQueries.SelectSymbolIdNameUsingSymbol(tickersInGrid[i]).SymbolId);
                    var assetType = positionsQueries.SelectAssetTypeNameBySymbolId(symbolId);
                    if (assetType == PositionAssetTypes.Stock.ToString())
                    {
                        assetType += 's';
                    }
                    Checker.IsTrue(expectedAssetTypes.Contains(assetType), $"Ticker {tickersInGrid[i]} with assetType = {assetType} is shown unexpectedly");

                    var healthZoneDescription = Dictionaries.ShortHealthStatus[healthZoneTypes[i]];
                    Checker.IsTrue(expectedHealthes.Contains(healthZoneDescription),
                        $"Ticker {tickersInGrid[i]} with healthIco = {healthInGrid[i]} and health = {healthZoneTypes[i]} is shown unexpectedly");

                    var timingsSymbolModels = timingQueries.SelectNearestWithUpcomingTimingModelsBySymbolId(symbolId).Where(t => t.SerieType == (int)TimingSerieTypes.Composite).ToList();
                    var isExpectedAreaAndConvictionPresent = false;
                    foreach (var timingModel in timingsSymbolModels)
                    {
                        var actualDirectionType = timingModel.GetTimingDirectionTypes();
                        var actualConvictionLevel = (TimingConvictionLevelTypes)timingModel.TurnStrength;
                        if (expectedPeakValleyValues.Contains(actualDirectionType.GetStringMapping())
                            && expectedConvictionLevels.Contains(actualConvictionLevel.GetStringMapping()))
                        {
                            isExpectedAreaAndConvictionPresent = true;
                            timingDirectionTypes.Add(actualDirectionType);
                            timingConvictionsLevels.Add(actualConvictionLevel);
                            break;
                        }
                    }
                    if (!isExpectedAreaAndConvictionPresent)
                    {
                        Checker.Fail($"Ticker {tickersInGrid[i]} is shown unexpectedly for Area/Conviction");
                    }
                }
            }

            LogStep(stepNumber++, stepNumber++, "Select three symbols (or less if grid contains less than 3 items). " +
                "Click on the link on Ticker column for first ticker. Check that Health, Peak & Valley, Conviction level is matched expectations from filters");
            var tickersQuantityToStockAnalyzer = Math.Min(resultsInGrd, TickersCountToOpenInStackAnalyzer);
            var tickersOrdersToStockAnalyzer = Randoms.GetRandomNumbersInRange(1, resultsInGrd, tickersQuantityToStockAnalyzer);
            var driver = Browser.Instance.GetDriver();
            var currentWindow = driver.WindowHandles.Last();

            foreach (var tickerOrder in tickersOrdersToStockAnalyzer)
            {
                timingCalendarForm.ClickTickerLinkInGridByNumberForOpenInNewTab(tickerOrder);

                var newTabWindowHandle = driver.WindowHandles.Last();
                driver.SwitchTo().Window(newTabWindowHandle);
                var stockAnalyzerForm = new StockAnalyzerForm();

                var expectedTicker = tickersInGrid[tickerOrder - 1];
                Checker.CheckEquals(expectedTicker, stockAnalyzerForm.GetSymbolTreeSelectSingleValue(), "Tickers are not matched");

                Checker.CheckEquals(healthZoneTypes[tickerOrder - 1], stockAnalyzerForm.Pills.GetHealthPill().HealthZone,
                    $"HealthZone are not matched for {expectedTicker}");

                CloseCurrentBrowserTabViaDriverAndSwitchToWindow(currentWindow);
            }
        }

        private void DoStepsWithClearingAndSettingValuesInFilterDropdowns(TimingCalendarFilterTypes filter, List<string> expectedValues)
        {
            var timingCalendarForm = new TimingCalendarForm();
            LogStep(stepNumber++, $"Clear all items in {filter.GetStringMapping()} dropdown");
            timingCalendarForm.ClearDropdownForFilter(filter);
            Checker.CheckListsEquals(new List<string>(), timingCalendarForm.GetSelectedItemsInFilter(filter),
                $"Items in {filter.GetStringMapping()} dropdown are not as expected after clearing");
            Checker.IsFalse(timingCalendarForm.IsFiltersApplyingButtonActive(FiltersApplyingButtonTypes.Apply),
                $"Apply button is not disabled for {filter.GetStringMapping()}");

            LogStep(stepNumber++, $"Select All listed in test data items in {filter.GetStringMapping()} dropdown");
            timingCalendarForm.SelectItemsMultipleByListOfText(filter, expectedValues);
            var actualState = timingCalendarForm.GetSelectedItemsInFilter(filter);
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedValues, actualState),
                $"Items in {filter.GetStringMapping()} dropdown are not as expected after selection:\n {GetExpectedResultsString(expectedValues)}\r\n{GetActualResultsString(actualState)}");
            Checker.IsTrue(timingCalendarForm.IsFiltersApplyingButtonActive(FiltersApplyingButtonTypes.Apply),
                $"Apply button is disabled for {filter.GetStringMapping()}");
        }
    }
}