using AutomatedTests.Enums.Dashboard;
using AutomatedTests.Enums;
using AutomatedTests.Forms;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._02_Dashboard._10_MarketHealth
{
    [TestClass]
    public class TC_1375_Dashboard_MarketHealth_AllExpectedElementsArePresent : BaseTestUnitTests
    {
        private const int TestNumber = 1375;

        private List<DashboardMarketHealthTab> tabsAvailableForSubscription;

        [TestInitialize]
        public void TestInitialize()
        {
            var userProductSubscriptions = GetUserProductSubscriptions("userSubscription");

            tabsAvailableForSubscription = GetTestDataValuesAsListByColumnName("nameTab").Where(s => !string.IsNullOrWhiteSpace(s)).Distinct()
                .Select(t => t.ParseAsEnumFromDescription<DashboardMarketHealthTab>()).ToList();

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1375$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test checks all expected elements are present in the Market Health widget. https://tr.a1qa.com/index.php?/cases/view/20121346")]
        [TestCategory("Timing"), TestCategory("Dashboard"), TestCategory("MarketHealth")]
        public override void RunTest()
        {
            LogStep(1, "Go to Portfolio Distribution widget. Open the 'HEALTH' tab.");
            var dashboardWidget = new WidgetForm(WidgetTypes.MarketHealth, false);
            Checker.CheckEquals(DashboardMarketHealthTab.MarketOutlook.GetDescription().ToLower(), dashboardWidget.GetWidgetContentActiveTabName().ToLower(),
                $"{DashboardMarketHealthTab.MarketOutlook.GetDescription()} is not default tab");

            LogStep(2, "Check that tabs quantity corresponds to subscription.");
            Checker.CheckEquals(tabsAvailableForSubscription.Count, dashboardWidget.GetWidgetContentTabQuantity(),
                "Market Health contains unexpected tab quantity");

            LogStep(3, 4, "Check that tabs are not empty.");
            foreach (var tab in tabsAvailableForSubscription)
            {
                LogStep(3, "Click the 1 tab in the widget (open one by one)");
                dashboardWidget.ClickWidgetContentTab(tab);
                Checker.CheckEquals(tab.GetDescription().ToLower(), dashboardWidget.GetWidgetContentActiveTabName().ToLower(),
                    $"{tab.GetDescription()} is not open after clicking");

                LogStep(4, "Check that the tab has: donut chart and legend");
                Checker.IsTrue(dashboardWidget.WidgetPieChart.IsExists(),
                    $"{tab.GetDescription()} does not contain chart");
                Checker.CheckNotEquals(0, dashboardWidget.WidgetPieChart.GetMarketHealthTabChartLegendItemLabels().Count,
                    $"{tab.GetDescription()} does not contain legend");
            }
        }
    }
}