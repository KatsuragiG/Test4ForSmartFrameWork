using AutomatedTests.Database.Publications;
using AutomatedTests.Enums.Dashboard;
using AutomatedTests.Enums.Publication;
using AutomatedTests.Enums.User;
using AutomatedTests.Enums;
using AutomatedTests.Forms.Dashboard;
using AutomatedTests.Forms;
using AutomatedTests.Models.PublicationsModels;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._02_Dashboard._09_TradeSmithInsights
{
    [TestClass]
    public class TC_1322_Dashboard_Publications_CheckThatLinksClickableAndRedirectToExpectedPages : BaseTestUnitTests
    {
        private const int TestNumber = 1322;
        private const WidgetTypes Widget = WidgetTypes.Publications;
        private const int NewsQuantityToCheck = 3;

        private int step = 2;
        private int minimalCountOfNewsInWidget;
        private ProductSubscriptionTypes userType;

        [TestInitialize]
        public void TestInitialize()
        {
            userType = GetTestDataParsedAsEnumFromStringMapping<ProductSubscriptionTypes>("User");
            minimalCountOfNewsInWidget = GetTestDataAsInt(nameof(minimalCountOfNewsInWidget));

            LogStep(0, "Preconditions: Login. Open Dashboard page");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, new List<ProductSubscriptions>
            {
                (ProductSubscriptions)(int)userType,
                ProductSubscriptions.TimingReport,
                ProductSubscriptions.TradeIdeasBasic,
                ProductSubscriptions.TrendsLifetime
            }));

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1322$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Dashboard"), TestCategory("DashboardDrSmithInsights"), TestCategory("Publications")]
        [Description("Test checks that all links lead to expected pages: https://tr.a1qa.com/index.php?/cases/view/19234161")]
        public override void RunTest()
        {
            LogStep(step++, "Click the widget header");

            CheckFileDownloadingForPublicationType(PublicationsTypes.TradesmithDaily);

            CheckFileDownloadingForPublicationType(PublicationsTypes.InsideTradeSmith);

            CheckFileDownloadingForPublicationType(PublicationsTypes.TimingByTradeSmith);

            CheckFileDownloadingForPublicationType(PublicationsTypes.TradeSmithTrends);
        }

        private void CheckFileDownloadingForPublicationType(PublicationsTypes dashboardWidgetPublicationsType)
        {
            LogStep(step++, $"Check news in {dashboardWidgetPublicationsType.GetStringMapping()}");
            var dashboardWidget = new WidgetForm(Widget);
            dashboardWidget.SelectValueInPublicationDropDown(dashboardWidgetPublicationsType.GetStringMapping());

            var newsFromDashboardPage = dashboardWidget.GetWidgetNewsModels();
            Checker.IsTrue(newsFromDashboardPage.Count >= minimalCountOfNewsInWidget,
                $"Count of news in widget for tab {dashboardWidgetPublicationsType.GetStringMapping()} is not as expected: {newsFromDashboardPage.Count}");

            SelectRandomNewsModelFromList(NewsQuantityToCheck, newsFromDashboardPage);
            foreach (var currentModel in newsFromDashboardPage)
            {
                LogStep(step++, $"Click on the '{currentModel.TopicName}' news in the widget");
                var topicName = currentModel.TopicName.Split('\'')[0].Split('£')[0].Trim();
                dashboardWidget.ClickWidgetNewsTopicHeader(topicName);
                var currentPublicationsDbModels = new PublicationsQueries().SelectPublicationsByTitleDate(topicName,
                    Parsing.ConvertToShortDateString(currentModel.PublicationDate));
                var currentPublicationDbModel = currentPublicationsDbModels.FirstOrDefault();
                CheckPublicationOpening(new PublicationModel { PublicationDate = currentModel.PublicationDate, PublicationTitle = currentModel.TopicName },
                    currentPublicationDbModel);
            }
        }

        private void SelectRandomNewsModelFromList(int randomElementsQuantity, List<DrSmithInsightsNewsModel> newsFromDashboardPage)
        {
            if (randomElementsQuantity != 0 && randomElementsQuantity < newsFromDashboardPage.Count)
            {
                var elementsIndicesToRemove = Enumerable.Range(0, newsFromDashboardPage.Count - 1).OrderBy(t => SRandom.Instance.Next())
                    .Take(newsFromDashboardPage.Count - randomElementsQuantity)
                    .OrderByDescending(p => p).ToList();
                foreach (var elementOrderToDelete in elementsIndicesToRemove)
                {
                    newsFromDashboardPage.RemoveAt(elementOrderToDelete);
                }
            }
        }
    }
}