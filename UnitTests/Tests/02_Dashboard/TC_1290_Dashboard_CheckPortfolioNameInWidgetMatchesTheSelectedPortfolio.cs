using System.Collections.Generic;
using System.Linq;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Dashboard;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._02_Dashboard
{
    [TestClass]
    public class TC_1290_Dashboard_CheckPortfolioNameInWidgetMatchesTheSelectedPortfolio : BaseTestUnitTests
    {
        private const int TestNumber = 1290;
        private const string WidgetColumn = "widget";

        private AddPortfolioManualModel portfolioModel;
        private List<string> expWidgetNames;
        private string watchOnlyPortfolioName;

        [TestInitialize]
        public void TestInitialize()
        {
            portfolioModel = new AddPortfolioManualModel
            {
                Name = GetTestDataAsString("portfolioName")
            };

            var positionsModels = new List<PositionAtManualCreatingPortfolioModel>
            {
                new PositionAtManualCreatingPortfolioModel
                {
                    Ticker = GetTestDataAsString("ticker"),
                    EntryDate = GetTestDataAsString("entryDate"),
                    Quantity = GetTestDataAsString("shares")
                }
            };

            expWidgetNames = InitializeWidgetNames();

            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, new List<ProductSubscriptions>
                {
                    ProductSubscriptions.TradeStopsLifetime, ProductSubscriptions.TradeIdeasLifetime, ProductSubscriptions.CryptoStopsLifetime
                }
            ));

            PortfoliosSetUp.AddWatchOnlyUsdPortfoliosWithOpenPosition(UserModels.First().Email);
            var portfoliosQueries = new PortfoliosQueries();
            watchOnlyPortfolioName = portfoliosQueries.SelectPortfolioName(portfoliosQueries.SelectPortfolioIdForLastPortfolioByUserEmail(UserModels.First().Email));
            PortfoliosSetUp.AddInvestmentAudPortfoliosWithOpenPosition(UserModels.First().Email);

            LoginSetUp.LogIn(UserModels.First());
            new AddPortfoliosSteps().NavigateToAddManualPortfolioSavePortfolioAfterFillingFields(portfolioModel, positionsModels);
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
        }

        private List<string> InitializeWidgetNames()
        {
            var widgetNames = new List<string>();
            var tableColumns = TestContext.DataRow.Table.Columns;

            foreach (var column in tableColumns)
            {
                if (column.ToString().Contains(WidgetColumn))
                {
                    widgetNames.Add(GetTestDataAsString(column.ToString()));
                }
            }
            return widgetNames;
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1290$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("Test for comparison of all widgets headlines: https://tr.a1qa.com/index.php?/cases/view/19234221")]
        [TestCategory("DashboardWidgets"), TestCategory("Dashboard")]
        public override void RunTest()
        {
            LogStep(1, "Select Test portfolio in 'Select Portfolio' drop down");
            var dashboardForm = new DashboardForm();
            dashboardForm.SelectPortfolioStatisticsWidgetPortfolio(portfolioModel.Name);
            Checker.CheckEquals(portfolioModel.Name, dashboardForm.GetSelectedPortfolioStatisticsWidgetPortfolioName(),
                 "Created portfolio is not selected on dashboard page");

            CheckDashboardWidgetNaming(portfolioModel.Name);

            LogStep(3, "Repeat checking for other portfolios");
            dashboardForm.SelectPortfolioStatisticsWidgetPortfolio(AllPortfoliosKinds.AllInvestment.GetStringMapping());
            CheckDashboardWidgetNaming(AllPortfoliosKinds.AllInvestment.GetStringMapping());

            dashboardForm.SelectPortfolioStatisticsWidgetPortfolio(AllPortfoliosKinds.All.GetStringMapping());
            CheckDashboardWidgetNaming(AllPortfoliosKinds.All.GetStringMapping());

            dashboardForm.SelectMultiplePortfoliosStatisticsWidgetPortfolio(new List<string> { portfolioModel.Name, watchOnlyPortfolioName } );
            CheckDashboardWidgetNaming($"{portfolioModel.Name}, {watchOnlyPortfolioName}");
        }

        private void CheckDashboardWidgetNaming(string portfolioName)
        {
            LogStep(2, "Check widgets names");
            var dashboardForm = new DashboardForm();
            var widgetNames = dashboardForm.GetWidgetNames();
            Checker.CheckEquals(expWidgetNames.Count, widgetNames.Count, $"Count of widgets on dashboard page are not as expected at selecting {portfolioName}");

            foreach (var expWidgetName in expWidgetNames)
            {
                if (!widgetNames.Contains(expWidgetName))
                {
                    Checker.Fail($"'{expWidgetName}' widget is not present on dashboard page at selecting {portfolioName}");
                    continue;
                }
                widgetNames.Remove(expWidgetName);
            }

            Checker.IsFalse(widgetNames.Any(),
                $"The following widgets don't have matching names at selecting {portfolioName} in expected list:\n{string.Join("\n", widgetNames)}");
        }
    }
}