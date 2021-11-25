using System.Collections.Generic;
using System.Linq;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Database.Users;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Dashboard;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.User;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Dashboard;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.BrowserSteps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._02_Dashboard._01_Statistics
{
    [TestClass]
    public class TC_1292_Dashboard_Statistics_CheckAvailabilityOfPortfolioVq : BaseTestUnitTests
    {
        private const int TestNumber = 1292;

        private string dbParamTradeSmith;
        private string dbValueForPrecondition;
        private string dbValueForStep;

        private const string TickerColumn = "ticker";

        [TestInitialize]
        public void TestInitialize()
        {
            var userType = GetTestDataParsedAsEnumFromStringMapping<ProductSubscriptionTypes>("user");
            dbValueForPrecondition = GetTestDataAsString(nameof(dbValueForPrecondition));
            dbValueForStep = GetTestDataAsString(nameof(dbValueForStep));
            dbParamTradeSmith = GetTestDataAsString(nameof(dbParamTradeSmith));

            var portfolioModel = new PortfolioModel
            {
                Name = "CheckAvailabilityOfPortfolioVq",
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("portfolioType"),
                Currency = Currency.USD.GetStringMapping()
            };

            var positionsModels = InitializePositionModels();

            LogStep(0, "Precondition - Login as registered user");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, (ProductSubscriptions)(int)userType));

            var portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            PositionsAlertsSetUp.AddPositionsViaDB(portfolioId, positionsModels);

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
        }

        private List<PositionsDBModel> InitializePositionModels()
        {
            var tableColumns = TestContext.DataRow.Table.Columns;
            var entryDate = GetTestDataAsString("entryDate");
            return tableColumns.Cast<object>()
                .Where(column => column.ToString().Contains(TickerColumn))
                .Select(column => new PositionsDBModel
                {
                    Symbol = GetTestDataAsString(column.ToString()),
                    PurchaseDate = entryDate
                }).ToList();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1292$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Dashboard"), TestCategory("DashboardStatistics"), TestCategory("PVQAnalyzer")]
        [Description("Test for availability of PVQ: https://tr.a1qa.com/index.php?/cases/view/19234293")]
        public override void RunTest()
        {
            LogStep(1, "Enable PVQ Analyzer for the user");
            var dashboardForm = new DashboardForm();
            Checker.IsFalse(dashboardForm.IsPortfolioStatisticSectionLabelPresent(DashboardStatisticSections.PortfolioRiskPvq),
                "PVQ Analyzer section is present on dashboard page after login");
            var userQueries = new UsersQueries();
            userQueries.UpdateUserProductTradesmithFeatures(UserModels.First().TradeSmithUserId, dbParamTradeSmith, dbValueForPrecondition);

            LogStep(2, "Refresh Dashboard page");
            var browserSteps = new BrowserSteps();
            browserSteps.LogoutClearCookiesLogin(UserModels.First());

            LogStep(3, "Check Statistics section");
            var mainMenuForm = new MainMenuForm();
            mainMenuForm.ClickMenuItem(MainMenuItems.Dashboard);

            Checker.IsTrue(dashboardForm.IsPortfolioStatisticSectionLabelPresent(DashboardStatisticSections.PortfolioRiskPvq),
                "PVQ Analyzer section is not present on dashboard page after enabling");

            LogStep(4, "Disable PVQ Analyzer for the user");
            userQueries.UpdateUserProductTradesmithFeatures(UserModels.First().TradeSmithUserId, dbParamTradeSmith, dbValueForStep);

            LogStep(5, "Refresh Dashboard page");
            browserSteps.LogoutClearCookiesLogin(UserModels.First());
            mainMenuForm.ClickMenuItem(MainMenuItems.Dashboard);

            LogStep(6, "Check Statistics section");
            Checker.IsFalse(dashboardForm.IsPortfolioStatisticSectionLabelPresent(DashboardStatisticSections.PortfolioRiskPvq),
                "PVQ Analyzer section is present on dashboard page after disabling");
        }
    }
}