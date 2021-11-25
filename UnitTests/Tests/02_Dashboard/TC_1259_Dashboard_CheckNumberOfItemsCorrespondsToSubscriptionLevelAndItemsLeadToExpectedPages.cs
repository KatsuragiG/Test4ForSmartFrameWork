using System;
using System.Collections.Generic;
using System.Linq;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Dashboard;
using AutomatedTests.Enums.User;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Dashboard;
using AutomatedTests.Forms.Gurus;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.ResearchPages.AssetAllocation;
using AutomatedTests.Forms.ResearchPages.PositionSizeForm;
using AutomatedTests.Forms.ResearchPages.PureQuant;
using AutomatedTests.Forms.ResearchPages.PVQAnalyzer;
using AutomatedTests.Forms.ResearchPages.RiskRebalancer;
using AutomatedTests.Forms.Settings.Alerts;
using AutomatedTests.Forms.Templates;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Dashboard;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;
namespace UnitTests.Tests._02_Dashboard
{
    [TestClass]
    public class TC_1259_Dashboard_CheckNumberOfItemsCorrespondsToSubscriptionLevelAndItemsLeadToExpectedPages : BaseTestUnitTests
    {
        private const int TestNumber = 1259;

        private int numberOfItems;
        private bool arrowsAvailability;
        private Dictionary<DashboardWidgetActionItems, bool> dashboardItems;
        
        [TestInitialize]
        public void TestInitialize()
        {
            var userType = GetTestDataParsedAsEnumFromStringMapping<ProductSubscriptionTypes>("User");
            numberOfItems = GetTestDataAsInt(nameof(numberOfItems));
            arrowsAvailability = GetTestDataAsBool(nameof(arrowsAvailability));
            dashboardItems = new Dictionary<DashboardWidgetActionItems, bool>
            {
                { DashboardWidgetActionItems.TrackMyInvestments, GetTestDataAsBool("isTrackMyInvestmentsVisible") },
                { DashboardWidgetActionItems.BuildNewPureQuantPortfolio, GetTestDataAsBool("isBuildNewPureVisible") },
                { DashboardWidgetActionItems.AnalyzeMyCurrentPortfolioRisk, GetTestDataAsBool("isAnalizeMyCurrentPortfolioRiskVisible") },
                { DashboardWidgetActionItems.RebalanceMyPortfolioForEqualRisk, GetTestDataAsBool("isRebalanceMyPortfolioVisible") },
                { DashboardWidgetActionItems.CalculatePositionSize, GetTestDataAsBool("isCalculatePositionSizeVisible") },
                { DashboardWidgetActionItems.AccessMyNewsletters, GetTestDataAsBool("isAccessMyNewslettersVisible") },
                { DashboardWidgetActionItems.ExploreBillionairesPortfolios, GetTestDataAsBool("isExploreBillionariesPortfoliosVisible") },
                { DashboardWidgetActionItems.ReviewMyAssetAllocation, GetTestDataAsBool("isReviewMyAssetAllocationVisible") },
                { DashboardWidgetActionItems.CustomizeAlertPreferences, GetTestDataAsBool("isCustomizeAlertPreferencesVisible") },
                { DashboardWidgetActionItems.CreateAnAlertTemplate, GetTestDataAsBool("isCreateAlertTemplateVisible") }
            };

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, (ProductSubscriptions)(int)userType));
            var dashboardSteps = new DashboardSteps();
            dashboardSteps.LoginAsUser(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1259$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("Test checks that items quantity corresponds to the subscription level of the user: https://tr.a1qa.com/index.php?/cases/view/19234137")]
        [TestCategory("Smoke"), TestCategory("Dashboard"), TestCategory("DashboardActionCarousel"), TestCategory("NewsLettersPage"),
         TestCategory("PureQuantTool"), TestCategory("AddPortfolio"), TestCategory("SettingsPage"), TestCategory("TemplateAdd"), TestCategory("PortfoliosPage"), 
         TestCategory("TemplatesPage"), TestCategory("PVQAnalyzer"), TestCategory("AssetAllocation"), TestCategory("RiskRebalancer")]
        public override void RunTest()
        {
            LogStep(1, "Check the number of items");
            var dashboardForm = new DashboardForm();
            var itemsNames = dashboardForm.GetActionPillsItemsNames();
            Checker.CheckEquals(numberOfItems, itemsNames.Count, "Number of items is not as expected");

            LogStep(2, "Check text in items");
            foreach (var item in dashboardItems)
            {
                Checker.CheckEquals(item.Value, itemsNames.Contains(item.Key.GetStringMapping()),
                    $"Visible state for '{item.Key.GetStringMapping()}' item is not as expected. Visible: {item.Value}");
            }

            if (numberOfItems > 0)
            {
                LogStep(3, "Check that the arrows works as expected");
                Checker.CheckEquals(arrowsAvailability, !dashboardForm.IsArrowDisabled(CarouselActionArrows.Next),
                    "Next arrow is not disabled");

                Steps4To12();
            }
        }

        private void Steps4To12()
        {
            var numberOfStep = 4;
            var dashboardSteps = new DashboardSteps();
            foreach (var item in dashboardItems)
            {
                if (!item.Value) continue;
                LogStep(numberOfStep++, $"Click '{item.Key.GetStringMapping()}' item");

                dashboardSteps.NavigateToDashboardClickActionItem(item.Key);
                try
                {
                    switch (item.Key)
                    {
                        case DashboardWidgetActionItems.TrackMyInvestments:
                            new SelectPortfolioFlowForm().AssertIsOpen();
                            break;
                        case DashboardWidgetActionItems.AnalyzeMyCurrentPortfolioRisk:
                            new PvqAnalyzerForm().AssertIsOpen();
                            break;
                        case DashboardWidgetActionItems.RebalanceMyPortfolioForEqualRisk:
                            new RiskRebalancerForm().AssertIsOpen();
                            break;
                        case DashboardWidgetActionItems.CalculatePositionSize:
                            new PositionSizeCalculatorForm().AssertIsOpen();
                            break;
                        case DashboardWidgetActionItems.AccessMyNewsletters:
                            new MainPublishersForm().AssertIsOpen();
                            break;
                        case DashboardWidgetActionItems.ReviewMyAssetAllocation:
                            new AssetAllocationForm().AssertIsOpen();
                            break;
                        case DashboardWidgetActionItems.CustomizeAlertPreferences:
                            new AlertsSettingsForm().AssertIsOpen();
                            break;
                        case DashboardWidgetActionItems.CreateAnAlertTemplate:
                            new AddTemplateForm().AssertIsOpen();
                            break;
                        case DashboardWidgetActionItems.ExploreBillionairesPortfolios:
                            new SelectedPublisherForm().AssertIsOpen();
                            break;
                        case DashboardWidgetActionItems.BuildNewPureQuantPortfolio:
                            new PureQuantCommonForm().AssertIsOpen();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(item.Key.GetStringMapping(), $"'{item.Key.GetStringMapping()}' widget action item is not supported");
                    }
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Checker.Fail($"'{item.Key.GetStringMapping()}' is not opened: {ex}");
                }
            }
        }
    }
}