using System;
using System.Collections.Generic;
using System.Linq;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.DsiForSymbols;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Dashboard;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Popups;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Dashboard;
using AutomatedTests.Steps.TableElementSteps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Constants;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._02_Dashboard._08_TrendingTickers
{
    [TestClass]
    public class TC_1325_Dashboard_TrendingStocks_CheckExpectedBehaviorOfGridStrategiesColumn : BaseTestUnitTests
    {
        private const int TestNumber = 1325;
        private const WidgetTypes Widget = WidgetTypes.TrendingTickers;

        private List<GeneralTablesHeaders> expectedWidgetHeaders;
        private readonly List<string> expectedColumnHeaders = new List<string>();
        private readonly List<string> expectedStrategiesItems = new List<string>();
        private string expectedHintText;
        private string expectedContentInfoText;

        [TestInitialize]
        public void TestInitialize()
        {
            expectedHintText = GetTestDataAsString(nameof(expectedHintText));
            expectedContentInfoText = GetTestDataAsString(nameof(expectedContentInfoText));

            expectedWidgetHeaders = new List<GeneralTablesHeaders>
            {
                GeneralTablesHeaders.Ticker,
                GeneralTablesHeaders.HealthSsi,
                GeneralTablesHeaders.RiskVqPercent,
                GeneralTablesHeaders.Newsletters,
                GeneralTablesHeaders.Strategies
            };
            expectedColumnHeaders.AddRange(expectedWidgetHeaders.Select(expectedWidgetHeader => expectedWidgetHeader.GetDescription()));
            expectedStrategiesItems.AddRange(Constants.AvailableStrategyTypes.Select(strategy => strategy.GetStringMapping()));

            LogStep(0, "Preconditions: Login. Open Dashboard page");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPlatinum));

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1325$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Dashboard"), TestCategory("DashboardTrendingStocks")]
        [Description("Test checks the expected behavior of the grid: https://tr.a1qa.com/index.php?/cases/view/19234300")]
        public override void RunTest()
        {
            Checker.CheckEquals(Enum.GetNames(typeof(DashboardTrendingTickersTab)).Length, new WidgetForm(Widget).GetWidgetContentTabQuantity(),
                $"Dashboard widget {Widget.GetDescription()} has unexpected tab quantity");

            Steps1To8(DashboardTrendingTickersTab.Stocks);
            Steps1To8(DashboardTrendingTickersTab.Crypto);

            Step10To13(expectedHintText, expectedContentInfoText, expectedStrategiesItems, DashboardTrendingTickersTab.Stocks);
        }

        private void Steps1To8(DashboardTrendingTickersTab tab)
        {
            LogStep(1, "Check that name of the widget is displayed");
            var dashboardWidgetForm = new WidgetForm(Widget);
            dashboardWidgetForm.ClickWidgetContentTab(tab);
            var dashboardWidgetTable = dashboardWidgetForm.WidgetTable;
            Checker.CheckEquals(Widget.GetStringMapping(), dashboardWidgetForm.GetWidgetHeaderText(),
                $"Dashboard widget name with tab {tab.GetStringMapping()} is not as expected");

            LogStep(2, $"Check that {expectedWidgetHeaders.Count} columns are presented in the widget");
            var embeddedTableElementSteps = new EmbeddedTableElementSteps(dashboardWidgetTable);
            embeddedTableElementSteps.CheckThatTableHeadersPresent(expectedWidgetHeaders);

            LogStep(3, "Check that ticker name and company name are presented in the 'Ticker' column");
            var dashboardWidgetSteps = new DashboardWidgetSteps(Widget);
            var tickerTextValues = dashboardWidgetSteps.CheckThatTickerNamesPresentedInTickerColumnGetThem();
            var companyTextValues = dashboardWidgetSteps.CheckThatCompanyNamesPresentedInTickerColumnGetThem();
            Checker.CheckEquals(tickerTextValues.Count, companyTextValues.Count,
                $"The number of ticker names and company names are not equal for {tab.GetStringMapping()} ");

            LogStep(4, 5, "Check that name and title tickers match for 'Ticker' column");
            foreach (var ticker in tickerTextValues)
            {
                if (tab == DashboardTrendingTickersTab.Crypto)
                {
                    Checker.CheckEquals(SymbolCategories.CryptoCurrencyCategory.ToString(), new SymbolsQueries().SelectDataFromHDSymbols(ticker).SymbolCategoryID,
                        $"Ticker {ticker} is not expected for {tab.GetStringMapping()}");
                }
                else
                {
                    Checker.CheckNotEquals(SymbolCategories.CryptoCurrencyCategory.ToString(), new SymbolsQueries().SelectDataFromHDSymbols(ticker).SymbolCategoryID,
                        $"Ticker {ticker} is not expected for {tab.GetStringMapping()}");
                }
            }

            LogStep(6, "Check that name and title columns match");
            var headerTextValues = dashboardWidgetTable.GetTableColumnNames();
            Checker.CheckListsEquals(expectedColumnHeaders, headerTextValues,
                $"Text and title headers don't match for {tab.GetStringMapping()} tab");

            LogStep(7, "Check that values in Risk (VQ%) column numeric with percentage symbol and corresponds to DB");
            dashboardWidgetSteps.CheckThatRiskVqPercentColumnNumericValuesWithPercentageSymbolCorrespondsToDbByTicker();

            LogStep(8, "Check that column 'Strategies' are presented in the widget if user has TradeIdeas subscription");
            Checker.IsTrue(headerTextValues.Contains(GeneralTablesHeaders.Strategies.GetStringMapping()),
                $"{GeneralTablesHeaders.Strategies.GetStringMapping()} column is not presented into widget for {tab.GetStringMapping()} tab");
        }

        private void Step10To13(string expectedHint, string expectedContentInfo, List<string> expectedStrategies, DashboardTrendingTickersTab tab)
        {
            var dashboardWidgetForm = new WidgetForm(Widget);
            dashboardWidgetForm.ClickWidgetContentTab(tab);
            var dashboardTable = dashboardWidgetForm.WidgetTable;
            var strategiesValues = dashboardTable.GetTableColumnTextValuesByColumnName(GeneralTablesHeaders.Strategies);
            var strategiesRows = dashboardTable.GetTableRowsOrderWithNonEmptyValuesByColumnName(GeneralTablesHeaders.Strategies);
            if (!strategiesValues.Any())
            {
                Log.Info($"{Widget.GetStringMapping()} widget {tab.GetStringMapping()} tab doesn't have any tickers with strategies");
            }

            LogStep(9, "Check that hint appears if hover cursor over values in 'Strategies' column");
            HoverToWidgetValuesAndGetHints(strategiesValues, expectedHint);

            var tickerValues = dashboardTable.GetTableColumnSymbolTextValuesByColumnName(GeneralTablesHeaders.Ticker);
            for (var index = 0; index < strategiesValues.Count; index++)
            {
                if (string.IsNullOrEmpty(strategiesValues[index]))
                {
                    Log.Info($"'{tickerValues[index]}' {tab.GetStringMapping()} tab doesn't have any strategies");
                    continue;
                }

                LogStep(10, $"Index [{index}]: '{tickerValues[index]} ticker': Click on value in 'Strategies' column");
                dashboardTable.ClickTableColumnValueByColumnNameIndex(GeneralTablesHeaders.Strategies, index);
                var strategiesPopup = new StrategiesPopup();
                strategiesPopup.AssertIsOpen();

                LogStep(11, $"Index [{index}]: '{tickerValues[index]} ticker': Check that text displays in the pop up");
                Checker.CheckEquals(string.Format(expectedContentInfo, tickerValues[strategiesRows[index] - 1]), strategiesPopup.GetContentInfoText(),
                    "Content info text is not as expected");
                var actualStrategies = strategiesPopup.GetListViewItems();
                var invalidStrategies = actualStrategies.Where(e => !expectedStrategies.Contains(e)).ToList();
                Checker.IsFalse(invalidStrategies.Any(),
                    $"{tickerValues[index]} has invalid strategies: {string.Join(", ", invalidStrategies)}");

                LogStep(12, $"Index [{index}]: '{tickerValues[index]} ticker': Check that pop up window can be closed using 'Close' button and 'X' button in the upper right corner");
                strategiesPopup.ClickCrossButton();
                strategiesPopup.AssertIsClosed();
            }
        }

        private void HoverToWidgetValuesAndGetHints(IReadOnlyList<string> symbolValues, string expectedHintValue)
        {
            var dashboardWidget = new WidgetForm(Widget);
            var dashboardWidgetTable = dashboardWidget.WidgetTable;
            for (var i = 0; i < symbolValues.Count; i++)
            {
                if (string.IsNullOrEmpty(symbolValues[i]))
                {
                    continue;
                }
                dashboardWidgetTable.HoverToTableColumnValueByColumnNameAndRowPosition(GeneralTablesHeaders.Strategies, i);
                Checker.CheckEquals(expectedHintValue, dashboardWidget.GetTooltipInnerText(), $"Index [{i}]: Hint is not as expected");
            }
        }
    }
}