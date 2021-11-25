using AutomatedTests.Database.DsiForSymbols;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Dashboard;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Forms.Dashboard;
using AutomatedTests.Forms.MyPortfolios;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Enums;
using TradeStops.Common.Extensions;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;
using AutomatedTests.Navigation;
using AutomatedTests.ConstantVariables;

namespace UnitTests.Tests._02_Dashboard
{
    [TestClass]
    public class TC_1289_Dashboard_CheckThatAllExpectedElementsArePresent : BaseTestUnitTests
    {
        private const int TestNumber = 1289;

        private const string Without = "without";
        private const string Investment = "investment";
        private const string WatchOnly = "watchOnly";
        private const string Carousel = "Carousel";
        private const string OutlookSlider = "OutlookSlider";
        private const string TrendingTickers = "TrendingTickers";
        private const string MarketHealth = "MarketHealth";
        private const string MyOpportunities = "MyOpportunities";
        private const string TradeSmithInsights = "TradeSmithInsights";
        private const string StatisticsModule = "StatisticsModule";
        private const string PortfolioDistribution = "PortfolioDistribution";
        private const string PortfolioEquityPerformance = "PortfolioEquityPerformance";
        private const string WinnersAndLosers = "WinnersAndLosers";
        private const string RecentEvents = "RecentEvents";

        private PortfolioModel portfolioTemplate;
        private PositionsDBModel positionTemplate;
        private Dictionary<WidgetTypes, bool> withoutPortfoliosWidgetConditions;
        private Dictionary<WidgetTypes, bool> investmentPortfoliosWidgetConditions;
        private Dictionary<WidgetTypes, bool> watchOnlyPortfoliosWidgetConditions;
        private Dictionary<string, bool> statisticsModuleConditions;
        private Dictionary<string, bool> carouselWidgetConditions;
        private Dictionary<string, bool> outlookSliderWidgetConditions;

        [TestInitialize]
        public void TestInitialize()
        {
            portfolioTemplate = new PortfolioModel
            {
                Currency = "USD",
                Notes = "Portfolio: Notes",
                EntryCommission = Constants.DefaultStringZeroIntValue,
                ExitCommission = Constants.DefaultStringZeroIntValue,
                Cash = "100"
            };
            positionTemplate = new PositionsDBModel
            {
                Symbol = "AAPL",
                PurchaseDate = "07/07/2017",
                Shares = "1",
                PurchasePrice = "3.6718"
            };
            withoutPortfoliosWidgetConditions = InitializeWidgetConditions(Without);
            investmentPortfoliosWidgetConditions = InitializeWidgetConditions(Investment);
            watchOnlyPortfoliosWidgetConditions = InitializeWidgetConditions(WatchOnly);
            statisticsModuleConditions = InitializeModuleConditions(StatisticsModule);
            carouselWidgetConditions = InitializeModuleConditions(Carousel);
            outlookSliderWidgetConditions = InitializeModuleConditions(OutlookSlider);

            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, new List<ProductSubscriptions>
                {
                    ProductSubscriptions.TradeStopsLifetime, ProductSubscriptions.TradeIdeasLifetime, ProductSubscriptions.CryptoStopsLifetime
                }
            ));
            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);
            new MainMenuNavigation().OpenDashboard();
        }

        private Dictionary<string, bool> InitializeModuleConditions(string moduleName)
        {
            var moduleNameConditions = new Dictionary<string, bool>();
            var tableColumns = TestContext.DataRow.Table.Columns;
            foreach (var column in tableColumns)
            {
                if (column.ToString().Contains(moduleName))
                {
                    var conditionType = column.ToString().Replace(moduleName, string.Empty);
                    moduleNameConditions.Add(conditionType, GetTestDataAsBool(column.ToString()));
                }
            }

            return moduleNameConditions;
        }

        private Dictionary<WidgetTypes, bool> InitializeWidgetConditions(string conditionType)
        {
            var widgetConditions = new Dictionary<WidgetTypes, bool>();
            var tableColumns = TestContext.DataRow.Table.Columns;

            foreach (var column in tableColumns)
            {
                if (column.ToString().StartsWith(conditionType))
                {
                    var condition = GetTestDataAsBool(column.ToString());
                    var columnName = column.ToString().Replace(conditionType, string.Empty);
                    WidgetTypes widget;
                    switch (columnName)
                    {
                        case TrendingTickers: widget = WidgetTypes.TrendingTickers; break;
                        case MarketHealth: widget = WidgetTypes.MarketHealth; break;
                        case MyOpportunities: widget = WidgetTypes.MyOpportunities; break;
                        case TradeSmithInsights: widget = WidgetTypes.Publications; break;
                        case PortfolioDistribution: widget = WidgetTypes.PortfolioDistribution; break;
                        case PortfolioEquityPerformance: widget = WidgetTypes.PortfolioEquityPerformance; break;
                        case WinnersAndLosers: widget = WidgetTypes.WinnersAndLosers; break;
                        case RecentEvents: widget = WidgetTypes.RecentEvents; break;
                        default: continue;
                    }
                    widgetConditions.Add(widget, condition);
                }
            }

            return widgetConditions;
        }

        private Dictionary<WidgetTypes, bool> GetWidgetConditions(string conditionType)
        {
            var widgetConditions = watchOnlyPortfoliosWidgetConditions;
            if (conditionType.EqualsIgnoreCase(Investment))
            {
                widgetConditions = investmentPortfoliosWidgetConditions;
            }
            else if (conditionType.EqualsIgnoreCase(Without))
            {
                widgetConditions = withoutPortfoliosWidgetConditions;
            }

            return widgetConditions;
        }

        private void CreateActiveStocks(PortfolioType portfolioType)
        {
            var portfolio = portfolioTemplate;
            portfolio.Name = $"Portfolio: Active Stock: {portfolioType.GetStringMapping()}";
            portfolio.Type = portfolioType;
            portfolio.Cash = "321";

            CreatePortfolioAndPosition(portfolio, positionTemplate);
        }

        private void CreateOptions(PortfolioType portfolioType)
        {
            var portfolio = portfolioTemplate;
            portfolio.Name = $"Portfolio: Option: {portfolioType.GetStringMapping()}";
            portfolio.Type = portfolioType;

            CreatePortfolioAndPosition(portfolio, positionTemplate);
        }

        private void CreateDelisted(PortfolioType portfolioType)
        {
            var portfolio = portfolioTemplate;
            portfolio.Name = $"Portfolio: Delisted: {portfolioType.GetStringMapping()}";
            portfolio.Type = portfolioType;

            var position = positionTemplate;
            position.SymbolId = new SymbolsQueries().SelectDelistedStock().SymbolId;
            position.Symbol = null;
            position.StatusType = $"{(int)AutotestPositionStatusTypes.Delisted}";
            CreatePortfolioAndPosition(portfolio, position);
        }

        private void CreateExpired(PortfolioType portfolioType)
        {
            var portfolio = portfolioTemplate;
            portfolio.Name = $"Portfolio: Expired: {portfolioType.GetStringMapping()}";
            portfolio.Type = portfolioType;

            var position = positionTemplate;
            position.SymbolId = new SymbolsQueries().SelectDelistedStock().SymbolId;
            position.Symbol = null;
            position.StatusType = $"{(int)AutotestPositionStatusTypes.Expired}";
            CreatePortfolioAndPosition(portfolio, position);
        }

        private void CreateClosed(PortfolioType portfolioType)
        {
            var portfolio = portfolioTemplate;
            portfolio.Name = $"Portfolio: Closed: {portfolioType.GetStringMapping()}";
            portfolio.Type = portfolioType;

            var position = positionTemplate;
            position.StatusType = $"{(int)AutotestPositionStatusTypes.Close}";
            CreatePortfolioAndPosition(portfolio, position);
        }

        private void CreateExpectedPortfoliosAndPositions(PortfolioType portfolioType)
        {
            CreateActiveStocks(portfolioType);
            CreateOptions(portfolioType);
            CreateDelisted(portfolioType);
            CreateExpired(portfolioType);
            CreateClosed(portfolioType);
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1289$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("Test checks that all elements are existing on the Dashboard page and its correct default state according to the user portfolios: " +
                     "https://tr.a1qa.com/index.php?/cases/view/19234220")]
        [TestCategory("Smoke"), TestCategory("DashboardStatistics"), TestCategory("DashboardPortfolioDistribution"), TestCategory("Dashboard")]
        public override void RunTest()
        {          
            LogStep(1, "Check that all expected elements are presented on Dashboard page");
            CheckAllExpectedElements(Without);

            LogStep(2, "Create different Investment portfolios for next position types (via DB)");
            CreateExpectedPortfoliosAndPositions(PortfolioType.Investment);

            LogStep(3, "Check that all expected elements are presented on Dashboard page");
            var dashboardForm = new DashboardForm();
            dashboardForm.SelectPortfolioStatisticsWidgetPortfolio(AllPortfoliosKinds.All.GetStringMapping());
            CheckAllExpectedElements(Investment);

            LogStep(4, "Delete all Investment portfolios");
            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);
            new MyPortfoliosMenuForm().ClickMyPortfoliosMenuItem(MyPortfoliosMenuItems.PortfoliosGrid);
            var portfolioForm = new PortfoliosForm();
            portfolioForm.SelectAllPortfoliosByType(PortfolioType.Investment);
            portfolioForm.ClickDeleteButton();
            new PortfolioGridsSteps().ConfirmDeletingPortfoliosCloseSuccessPopup();

            LogStep(5, "Create different Watch Only portfolios for next position types (via DB)");
            CreateExpectedPortfoliosAndPositions(PortfolioType.WatchOnly);

            LogStep(6, "Check that all expected elements are presented on Dashboard page");
            dashboardForm.SelectPortfolioStatisticsWidgetPortfolio(AllPortfoliosKinds.All.GetStringMapping());
            CheckAllExpectedElements(WatchOnly);
        }

        private void CreatePortfolioAndPosition(PortfolioModel portfolio, PositionsDBModel position)
        {
            var portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolio);
            PositionsAlertsSetUp.AddPositionViaDB(portfolioId, position);
        }

        private void CheckAllExpectedElements(string conditionType)
        {
            var dashboardForm = new DashboardForm();
            var widgetNames = dashboardForm.GetWidgetNames();
            Checker.CheckEquals(statisticsModuleConditions[conditionType], dashboardForm.IsPortfolioStatisticsWidgetPresent(),
                $"{conditionType} portfolios: Statistic widget visible state is not as expected");
            Checker.CheckEquals(carouselWidgetConditions[conditionType], dashboardForm.IsActionPillsWidgetPresent(),
                $"{conditionType} portfolios: Carousel widget visible state is not as expected");
            Checker.CheckEquals(outlookSliderWidgetConditions[conditionType], dashboardForm.IsActionPillsWidgetPresent(),
                $"{conditionType} portfolios: Outlook-slider widget visible state is not as expected");

            var widgetConditions = GetWidgetConditions(conditionType);
            foreach (var widgetCondition in widgetConditions)
            {
                var indexToRemove = -1;
                for (var index = 0; index < widgetNames.Count; index++)
                {
                    if (widgetNames[index].Contains(widgetCondition.Key.GetStringMapping()))
                    {
                        Checker.IsTrue(widgetCondition.Value,
                            $"{conditionType} portfolios: {widgetCondition.Key.GetStringMapping()} widget visible state is not as expected");
                        indexToRemove = index;
                        break;
                    }
                }

                if (indexToRemove < 0)
                {
                    Checker.IsFalse(widgetCondition.Value,
                        $"{conditionType} portfolios: {widgetCondition.Key.GetStringMapping()} widget visible state is not as expected");
                    continue;
                }
                widgetNames.RemoveAt(indexToRemove);
            }

            if (widgetNames.Any())
            {
                Checker.Fail($"{conditionType} portfolios: The following widgets do not have visible states as expected:\n {string.Join("\n", widgetNames)}");
            }
        }
    }
}