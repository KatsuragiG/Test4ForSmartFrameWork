using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.DsiForSymbols;
using AutomatedTests.Database.Options;
using AutomatedTests.Database.Positions;
using AutomatedTests.Elements;
using AutomatedTests.Enums.Dashboard;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.Sorting;
using AutomatedTests.Enums;
using AutomatedTests.Forms.Popups;
using AutomatedTests.Forms;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.TableElementSteps;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Utils;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;
using TradeStops.Common.Extensions;

namespace UnitTests.Tests._02_Dashboard._04_MyOpportunities
{
    [TestClass]
    public class TC_1299_Dashboard_MyOpportunities_CheckExpectedBehaviorOfWidget : BaseTestUnitTests
    {
        private const int TestNumber = 1299;

        private const WidgetTypes Widget = WidgetTypes.MyOpportunities;
        private const string ColumnNameForDirection = "DefaultSortingDirection";
        private const string ColumnNameForSorting = "DefaultSortingColumn";
        private const string ColumnNameForColumns = "Columns";
        private const string ColumnNameForRowsQuantity = "Rows";

        private List<DashboardMyOpportunitiesTab> allPossibleTabs;
        private List<DashboardMyOpportunitiesTab> tabsAvailableForSubscription;
        private Dictionary<DashboardMyOpportunitiesTab, List<GeneralTablesHeaders>> expectedWidgetHeaders;
        private int step;
        private Dictionary<DashboardMyOpportunitiesTab, GeneralTablesHeaders> headerSortedAsDefault;
        private Dictionary<DashboardMyOpportunitiesTab, SortingStatus> directionSortedAsDefault;
        private Dictionary<DashboardMyOpportunitiesTab, int> rowsQuantity;
        private readonly List<string> expectedStrategiesItems = new List<string>();
        private readonly List<string> expectedOptionsStrategiesItems = new List<string>();
        private string expectedHintText;
        private string expectedContentInfoText;
        private bool isUserHasAccessToCrypto;

        [TestInitialize]
        public void TestInitialize()
        {
            var userProductSubscriptions = GetUserProductSubscriptions("userSubscription");

            allPossibleTabs = EnumUtils.GetValues<DashboardMyOpportunitiesTab>().ToList();
            tabsAvailableForSubscription = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(tabsAvailableForSubscription))
                .Select(t => t.ParseAsEnumFromStringMapping<DashboardMyOpportunitiesTab>()).ToList();
            expectedStrategiesItems.AddRange(Constants.AvailableStrategyTypes.Select(strategy => strategy.GetStringMapping()));
            expectedOptionsStrategiesItems.AddRange(Constants.AvailableOptionsStrategyTypes.Select(strategy => strategy.GetStringMapping()));

            headerSortedAsDefault = InitializeDefaultSortingDictionary();
            directionSortedAsDefault = InitializeDirectionSortingDictionary();
            expectedWidgetHeaders = InitializeColumnsDictionary();
            rowsQuantity = InitializeRowsDictionary();
            expectedHintText = GetTestDataAsString(nameof(expectedHintText));
            expectedContentInfoText = GetTestDataAsString(nameof(expectedContentInfoText));
            isUserHasAccessToCrypto = GetTestDataAsBool(nameof(isUserHasAccessToCrypto));

            LogStep(step++, "Preconditions: Login. Open Dashboard page");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
        }

        private Dictionary<DashboardMyOpportunitiesTab, GeneralTablesHeaders> InitializeDefaultSortingDictionary()
        {
            var testData = new Dictionary<DashboardMyOpportunitiesTab, GeneralTablesHeaders>();
            foreach (var tab in allPossibleTabs)
            {
                testData[tab] = GetTestDataParsedAsEnumFromStringMapping<GeneralTablesHeaders>($"{tab}{ColumnNameForSorting}");
            }

            return testData;
        }
        private Dictionary<DashboardMyOpportunitiesTab, SortingStatus> InitializeDirectionSortingDictionary()
        {
            var testData = new Dictionary<DashboardMyOpportunitiesTab, SortingStatus>();
            foreach (var tab in allPossibleTabs)
            {
                testData[tab] = GetTestDataParsedAsEnumFromStringMapping<SortingStatus>($"{tab}{ColumnNameForDirection}");
            }

            return testData;
        }
        private Dictionary<DashboardMyOpportunitiesTab, List<GeneralTablesHeaders>> InitializeColumnsDictionary()
        {
            var testData = new Dictionary<DashboardMyOpportunitiesTab, List<GeneralTablesHeaders>>();
            foreach (var tab in allPossibleTabs)
            {
                testData[tab] = GetTestDataValuesAsListByColumnNameAndRemoveEmpty($"{tab}{ColumnNameForColumns}").Select(t => t.ParseAsEnumFromStringMapping<GeneralTablesHeaders>()).ToList();
            }

            return testData;
        }
        private Dictionary<DashboardMyOpportunitiesTab, int> InitializeRowsDictionary()
        {
            var testData = new Dictionary<DashboardMyOpportunitiesTab, int>();
            foreach (var tab in allPossibleTabs)
            {
                testData[tab] = GetTestDataAsInt($"{tab}{ColumnNameForRowsQuantity}");
            }

            return testData;
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1299$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Dashboard"), TestCategory("DashboardNewslettersRecentRecommendations"), TestCategory("Sorting")]
        [Description("Test check the expected behavior of the grid: https://tr.a1qa.com/index.php?/cases/view/19234154")]
        public override void RunTest()
        {
            LogStep(step++, "Check that name of the widget is displayed");
            var dashboardWidget = new WidgetForm(Widget);

            Checker.CheckEquals(Widget.GetStringMapping(), dashboardWidget.GetWidgetHeaderText(),
                "Dashboard widget name is not as expected");

            LogStep(step++, "Check that tabs are shown according to user subscription");
            var isWidgetShown = new Dictionary<DashboardMyOpportunitiesTab, bool>();
            foreach (var tab in allPossibleTabs)
            {
                isWidgetShown[tab] = dashboardWidget.IsWidgetContentTabExist(tab);
                Checker.CheckEquals(tabsAvailableForSubscription.Contains(tab), isWidgetShown[tab],
                    $"Widget tab {tab} visibility is not as expected");
            }

            LogStep(step++, "if Invest Strategies widget is shown. Check that 4 columns are presented in the widget");
            if (isWidgetShown[DashboardMyOpportunitiesTab.InvestStrategies])
            {
                dashboardWidget.ClickWidgetContentTab(DashboardMyOpportunitiesTab.InvestStrategies);
                var dashboardWidgetTable = dashboardWidget.WidgetTable;
                var headerNames = dashboardWidgetTable.GetTableColumnNames();
                var expectedHeaderNames = expectedWidgetHeaders[DashboardMyOpportunitiesTab.InvestStrategies].Select(header => header.GetStringMapping()).ToList();
                Checker.IsTrue(headerNames.Count > 0 || rowsQuantity[DashboardMyOpportunitiesTab.InvestStrategies] == 0,
                    $"Table {DashboardMyOpportunitiesTab.InvestStrategies.GetStringMapping()} is not shown");

                LogStep(step++, "Check the default sorting");
                if (headerNames.Count > 0)
                {
                    var tickerTextValues = CheckHeadersAndTickersAndGetTickers(dashboardWidgetTable, headerNames, expectedHeaderNames, DashboardMyOpportunitiesTab.InvestStrategies);

                    LogStep(step++, "Check that health status is green and matched adjusted long for ticker");
                    CheckHealthColumn(dashboardWidgetTable, tickerTextValues);

                    LogStep(step++, "Check that vq% value matched for ticker");
                    CheckVqColumn(dashboardWidgetTable, tickerTextValues);

                    CheckStrategiesColumnAndPopup(dashboardWidget, dashboardWidgetTable, tickerTextValues);

                    LogStep(step, "Set 'DESC' sorting status for column 'Ticker' and check sort");
                    new EmbeddedTableElementSteps(dashboardWidgetTable).CheckSortInColumnsForAscDescSortingStatuses(step++, new List<GeneralTablesHeaders> { GeneralTablesHeaders.Ticker });
                }
            }

            LogStep(step++, "if Trade Strategies widget is shown. Check that 4 columns are presented in the widget");
            if (isWidgetShown[DashboardMyOpportunitiesTab.TradeStrategies])
            {
                dashboardWidget.ClickWidgetContentTab(DashboardMyOpportunitiesTab.TradeStrategies);
                var dashboardWidgetTable = dashboardWidget.WidgetTable;
                var headerNames = dashboardWidgetTable.GetTableColumnNames();
                var expectedHeaderNames = expectedWidgetHeaders[DashboardMyOpportunitiesTab.TradeStrategies].Select(header => header.GetStringMapping()).ToList();
                Checker.IsTrue(headerNames.Count > 0 || rowsQuantity[DashboardMyOpportunitiesTab.TradeStrategies] == 0,
                    $"Table {DashboardMyOpportunitiesTab.TradeStrategies.GetStringMapping()} is not shown");

                LogStep(step++, "Check the default sorting");
                if (headerNames.Count > 0)
                {
                    var tickerTextValues = CheckHeadersAndTickersAndGetTickers(dashboardWidgetTable, headerNames, expectedHeaderNames, DashboardMyOpportunitiesTab.TradeStrategies);

                    LogStep(step++, "Check that POP and ROI matched for ticker");
                    CheckPopAndRoiColumns(dashboardWidgetTable, tickerTextValues);

                    LogStep(step++, "Check that wording in Strategies contains option strategy name");
                    CheckOptionStrategies(dashboardWidgetTable);

                    LogStep(step, "Set 'DESC' sorting status for column 'Ticker' and check sort");
                    new EmbeddedTableElementSteps(dashboardWidgetTable).CheckSortInColumnsForAscDescSortingStatuses(step++, new List<GeneralTablesHeaders> { GeneralTablesHeaders.Ticker });
                }
            }

            LogStep(step++, "if My Gurus widget is shown. Check that 4 columns are presented in the widget");
            if (isWidgetShown[DashboardMyOpportunitiesTab.MyGurus])
            {
                dashboardWidget.ClickWidgetContentTab(DashboardMyOpportunitiesTab.MyGurus);
                var dashboardWidgetTable = dashboardWidget.WidgetTable;
                var headerNames = dashboardWidgetTable.GetTableColumnNames();
                var expectedHeaderNames = expectedWidgetHeaders[DashboardMyOpportunitiesTab.MyGurus].Select(header => header.GetStringMapping()).ToList();
                Checker.IsTrue(headerNames.Count > 0 || rowsQuantity[DashboardMyOpportunitiesTab.MyGurus] == 0,
                    $"Table {DashboardMyOpportunitiesTab.MyGurus.GetStringMapping()} is not shown");

                LogStep(step++, "Check the default sorting");
                if (headerNames.Count > 0)
                {
                    CheckHeadersAndTickersAndGetTickers(dashboardWidgetTable, headerNames, expectedHeaderNames, DashboardMyOpportunitiesTab.MyGurus);

                    LogStep(step++, "Check that name and title values match for 'Ref Date' column");
                    var textValues = dashboardWidgetTable.GetTableColumnTextValuesByColumnName(GeneralTablesHeaders.RefDate);
                    var titleValues = dashboardWidgetTable.GetTableColumnTitleValuesByColumnName(GeneralTablesHeaders.RefDate);
                    Checker.CheckListsEquals(titleValues, textValues,
                        $"Text and title '{GeneralTablesHeaders.RefDate.GetStringMapping()}' values don't match");

                    LogStep(step++, "Check Publisher column");
                    var publisherTextValues = dashboardWidgetTable.GetTableColumnNameTextValuesByColumnName(GeneralTablesHeaders.Publisher);
                    var publisherTitleValues = dashboardWidgetTable.GetTableColumnNameTitleValuesByColumnName(GeneralTablesHeaders.Publisher);
                    Checker.CheckListsEquals(publisherTitleValues, publisherTextValues,
                        $"Text and title '{GeneralTablesHeaders.Publisher.GetStringMapping()}' symbols don't match");

                    LogStep(step, "Set 'DESC' sorting status for column 'Ticker' and check sort");
                    new EmbeddedTableElementSteps(dashboardWidgetTable).CheckSortInColumnsForAscDescSortingStatuses(step++, new List<GeneralTablesHeaders> { GeneralTablesHeaders.Ticker });
                }
            }
        }

        private void CheckOptionStrategies(EmbeddedTableElement dashboardWidgetTable)
        {
            var strategyValues = dashboardWidgetTable.GetTableColumnTextValuesByColumnName(GeneralTablesHeaders.Strategies);
            Checker.IsTrue(strategyValues.Any(), "Strategy options values are missed");
            foreach (var strategyValue in strategyValues)
            {
                Checker.IsTrue(expectedOptionsStrategiesItems.Contains(strategyValue),
                    $"Strategy option value is not as expected: {strategyValue}");
            }
        }

        private void CheckPopAndRoiColumns(EmbeddedTableElement dashboardWidgetTable, List<string> tickerTextValues)
        {
            var popValues = dashboardWidgetTable.GetTableColumnTextValuesByColumnName(GeneralTablesHeaders.Pop);
            var roiValues = dashboardWidgetTable.GetTableColumnTextValuesByColumnName(GeneralTablesHeaders.Roi);
            Checker.IsTrue(popValues.Any(), "Probability Of Profit is empty");
            Checker.IsTrue(roiValues.Any(), "ROI is empty");
            var optionsQueries = new OptionsQueries();
            for (int i = 0; i < tickerTextValues.Count; i++)
            {
                var optionStrategyData = optionsQueries.SelectOptionStrategyModelBySymbol(tickerTextValues[i]);
                Checker.IsTrue(optionStrategyData.Any(), $"Options Strategy is empty for {tickerTextValues[i]}");
                if (optionStrategyData.Any())
                {
                    Checker.CheckEquals($"{optionStrategyData.First().ProbabilityOfProfit.ToFractionalString()}%",
                        popValues[i],
                        $"Probability Of Profit for {tickerTextValues[i]} don't match");
                    Checker.CheckEquals($"{optionStrategyData.First().ROI.ToFractionalString()}%",
                        roiValues[i],
                        $"ROI for {tickerTextValues[i]} don't match");
                }
            }
        }

        private void CheckStrategiesColumnAndPopup(WidgetForm dashboardWidget, EmbeddedTableElement dashboardWidgetTable, List<string> tickerTextValues)
        {
            LogStep(step++, "Check that number in Strategies non-empty and click able");
            var strategyValues = dashboardWidgetTable.GetTableColumnTextValuesByColumnName(GeneralTablesHeaders.Strategies);
            Checker.IsTrue(strategyValues.Any(), "Strategy values are missed");
            foreach (var strategyValue in strategyValues)
            {
                Checker.IsTrue(Constants.NumbersRegex.IsMatch(strategyValue), $"Strategy value are not number: {strategyValue}");
            }
            var itemOrderToCheck = SRandom.Instance.Next(strategyValues.Count);
            dashboardWidgetTable.HoverToTableColumnValueByColumnNameAndRowPosition(GeneralTablesHeaders.Strategies, itemOrderToCheck);
            Checker.CheckEquals(expectedHintText, dashboardWidget.GetTooltipInnerText(),
                $"Strategy [{itemOrderToCheck}]: Hint is not as expected");

            LogStep(step++, "Click on one digit strategies. Close popup after checking");
            dashboardWidgetTable.ClickTableColumnValueByColumnNameIndex(GeneralTablesHeaders.Strategies, itemOrderToCheck);
            var strategiesPopup = new StrategiesPopup();
            strategiesPopup.AssertIsOpen();
            Checker.CheckEquals(string.Format(expectedContentInfoText, tickerTextValues[itemOrderToCheck]), strategiesPopup.GetContentInfoText(),
                "Content info text is not as expected");
            var actualStrategies = strategiesPopup.GetListViewItems();
            var invalidStrategies = actualStrategies.Where(e => !expectedStrategiesItems.Contains(e)).ToList();
            Checker.IsFalse(invalidStrategies.Any(),
                $"{tickerTextValues[itemOrderToCheck]} has invalid strategies: {GetActualResultsString(invalidStrategies)}");
            strategiesPopup.ClickCrossButton();
        }

        private void CheckVqColumn(EmbeddedTableElement dashboardWidgetTable, List<string> tickerTextValues)
        {
            var vqValues = dashboardWidgetTable.GetTableColumnTextValuesWithoutLinksByColumnName(GeneralTablesHeaders.RiskVqPercent);
            Checker.IsTrue(vqValues.Any(), "Vq values are missed");
            var positionsQueries = new PositionsQueries();
            for (int i = 0; i < vqValues.Count; i++)
            {
                var symbolId = Parsing.ConvertToInt(positionsQueries.SelectSymbolIdNameUsingSymbol(tickerTextValues[i]).SymbolId);
                var currentVq = positionsQueries.SelectCurrentVqBySymbolId(symbolId);
                var shownVq = string.IsNullOrEmpty(currentVq)
                    ? Constants.DefaultVqValueIfNotExist
                    : Parsing.ConvertToDouble(currentVq).ToString("N2");
                Checker.CheckEquals($"{shownVq}%", vqValues[i],
                    $"Health for {tickerTextValues[i]} don't match");
            }
        }

        private void CheckHealthColumn(EmbeddedTableElement dashboardWidgetTable, List<string> tickerTextValues)
        {
            var ssiValues = dashboardWidgetTable.GetTableColumnSsiPillZonesByColumnName(GeneralTablesHeaders.HealthSsi)
                .Where(t => t.HasValue).Select(t => (HealthZoneTypes)t).ToList();
            for (int i = 0; i < ssiValues.Count; i++)
            {
                Checker.IsTrue(ssiValues[i].In(HealthZoneTypes.Green, HealthZoneTypes.YellowSide, HealthZoneTypes.YellowUp, HealthZoneTypes.YellowDown),
                    $"Health icon {ssiValues[i]} don't match expectation for {tickerTextValues[i]}");
            }
            var symbolsQueries = new SymbolsQueries();
            for (int i = 0; i < ssiValues.Count; i++)
            {
                Checker.CheckEquals(ssiValues[i],
                    (HealthZoneTypes)Parsing.ConvertToInt(symbolsQueries.SelectAnalyzedDataforSymbol(tickerTextValues[i]).LongAdjDsi),
                    $"Health for {tickerTextValues[i]} don't match");
            }
        }

        private List<string> CheckHeadersAndTickersAndGetTickers(EmbeddedTableElement dashboardWidgetTable, List<string> headerNames, List<string> expectedHeaderNames, DashboardMyOpportunitiesTab tab)
        {
            Checker.CheckEquals(expectedHeaderNames.Count, headerNames.Count,
                $"Count of columns is not as expected for tab {tab.GetStringMapping()}");
            Checker.CheckListsEquals(expectedHeaderNames, headerNames,
                $"Displayed columns are not as expected for tab {tab.GetStringMapping()}");

            LogStep(step++, "Check that tab contains 6 rows");
            var actualRowsQuantity = dashboardWidgetTable.GetTableRowsOrderWithNonEmptyValuesByColumnName(headerSortedAsDefault[tab]).Count;
            Checker.CheckEquals(rowsQuantity[tab],
                actualRowsQuantity,
                $"Rows quantity is not as expected in tab {tab.GetStringMapping()}");
            new EmbeddedTableElementSteps(dashboardWidgetTable).CheckThatHeaderSortedAsDefault(headerSortedAsDefault[tab],
                directionSortedAsDefault[tab]);

            LogStep(step++, "Check that name and title tickers match for 'Ticker' column");
            var tickerTextValues = dashboardWidgetTable.GetTableColumnSymbolTextValuesByColumnName(GeneralTablesHeaders.Ticker);
            var tickerTitleValues = dashboardWidgetTable.GetTableColumnSymbolTitleValuesByColumnName(GeneralTablesHeaders.Ticker);
            Checker.CheckEquals(tickerTitleValues.Count, tickerTextValues.Count,
                $"Text and title '{GeneralTablesHeaders.Ticker.GetStringMapping()}' symbols don't match");
            var positionsQueries = new PositionsQueries();
            foreach (var ticker in tickerTextValues)
            {
                var symbolId = Parsing.ConvertToInt(positionsQueries.SelectSymbolIdNameUsingSymbol(ticker).SymbolId);
                var assetType = positionsQueries.SelectAssetTypeNameBySymbolId(symbolId);
                if (assetType == PositionAssetTypes.Crypto.ToString())
                {
                    Checker.IsTrue(isUserHasAccessToCrypto, "User should not have access to Crypto");
                }
            }

            return tickerTextValues;
        }
    }
}