using System.Linq;
using AutomatedTests.Enums.Dashboard;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Portfolios.CreateManual;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Dashboard;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.ResearchPages.AssetAllocation;
using AutomatedTests.Forms.ResearchPages.RiskRebalancer;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Dashboard;
using AutomatedTests.Steps.Portfolios;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._02_Dashboard._02_CallToActions
{
    [TestClass]
    public class TC_1261_Dashboard_Cta_CheckCtaDisplaysAccordingToSubscriptionLevelDistributionWidget : BaseTestUnitTests
    {
        private const int TestNumber = 1261;
        private const int NumberOfPosition = 1;
        private const int CountOfOpenedTab = 1;
        private const WidgetTypes Widget = WidgetTypes.PortfolioDistribution;

        private AddPortfolioManualModel portfolioManualModel;
        private string redirectedUrl;
        private string ssiText;
        private string ssiLink;
        private string ssiLinkPage;
        private string ssiLabelOnPage;
        private bool checkSsiUrl;
        private bool checkLabelOnSsiPage;
        private string assetText;
        private string assetLink;
        private string assetLinkPage;
        private string assetLabelOnPage;
        private bool checkAssetUrl;
        private bool checkLabelOnAssetPage;
        private string riskText;
        private string riskLink;
        private string riskLinkPage;
        private string riskLabelOnPage;
        private bool checkRiskUrl;
        private bool checkLabelOnRiskPage;
        private bool isRatingTabShown;

        [TestInitialize]
        public void TestInitialize()
        {
            var userProductSubscriptions = GetUserProductSubscriptions("userSubscription");

            redirectedUrl = GetTestDataAsString(nameof(redirectedUrl));
            ssiText = GetTestDataAsString(nameof(ssiText));
            ssiLink = GetTestDataAsString(nameof(ssiLink));
            ssiLinkPage = GetTestDataAsString(nameof(ssiLinkPage));
            ssiLabelOnPage = GetTestDataAsString(nameof(ssiLabelOnPage));
            checkSsiUrl = GetTestDataAsBool(nameof(checkSsiUrl));
            checkLabelOnSsiPage = GetTestDataAsBool(nameof(checkLabelOnSsiPage));

            assetText = GetTestDataAsString(nameof(assetText));
            assetLink = GetTestDataAsString(nameof(assetLink));
            assetLinkPage = GetTestDataAsString(nameof(assetLinkPage));
            assetLabelOnPage = GetTestDataAsString(nameof(assetLabelOnPage));
            checkAssetUrl = GetTestDataAsBool(nameof(checkAssetUrl));
            checkLabelOnAssetPage = GetTestDataAsBool(nameof(checkLabelOnAssetPage));

            riskText = GetTestDataAsString(nameof(riskText));
            riskLink = GetTestDataAsString(nameof(riskLink));
            riskLinkPage = GetTestDataAsString(nameof(riskLinkPage));
            riskLabelOnPage = GetTestDataAsString(nameof(riskLabelOnPage));
            checkRiskUrl = GetTestDataAsBool(nameof(checkRiskUrl));
            checkLabelOnRiskPage = GetTestDataAsBool(nameof(checkLabelOnRiskPage));
            isRatingTabShown = GetTestDataAsBool(nameof(isRatingTabShown));

            portfolioManualModel = new AddPortfolioManualModel
            {
                Name = $"CtaDisplaysOnPortfolioDistributionWidget_{userProductSubscriptions.First()}",
                Type = PortfolioType.Investment
            };

            var positionsModel = new PositionAtManualCreatingPortfolioModel
            {
                Ticker = GetTestDataAsString("ticker"),
                EntryDate = GetTestDataAsString("entryDate"),
                Quantity = GetTestDataAsString("shares"),
                PositionAssetType = GetTestDataParsedAsEnumFromStringMapping<PositionAssetTypes>("positionType")
            };

            LogStep(0, "Precondition - Login as registered user");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));

            PortfoliosSetUp.AddInvestmentPortfoliosDefaultUSD(UserModels.First().Email);
            new DashboardSteps().LoginAsUser(UserModels.First());
            new AddPortfoliosSteps().OpenPortfolioCreationFormViaSelectionFlowPage(AddPortfolioTypes.Manual);

            var manualPortfolioCreationForm = new ManualPortfolioCreationForm();
            manualPortfolioCreationForm.FillPortfolioFields(portfolioManualModel);
            manualPortfolioCreationForm.FillPositionFields(positionsModel, NumberOfPosition);
            manualPortfolioCreationForm.ClickPortfolioManualFlowActionsButton(PortfolioManualFlowActionsButton.SavePortfolio);
            new AddAlertsAtCreatingPortfolioForm().ClickActionButton(AddAlertsAtCreatingPortfolioButtons.AddAlertsLater);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1261$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("Test for CTA displaying in accordance with the subscription: https://tr.a1qa.com/index.php?/cases/view/19234146")]
        [TestCategory("DashboardPortfolioDistribution"), TestCategory("Dashboard"), TestCategory("Permissions"), TestCategory("RiskRebalancer"), TestCategory("AssetAllocation")]
        public override void RunTest()
        {
            LogStep(1, "Check Portfolio Distribution widget, HEALTH (SSI) tab");
            NavigateToDashboardPageAndSelectTabAndCheckLinkTextAndUrlText(WidgetPortfolioDistributionTabs.HealthSsi, ssiText, ssiLink);

            LogStep(2, "Click SSI link");
            var dashboardForm = new WidgetForm(Widget);
            dashboardForm.ClickWidgetLinkUrl();
            CheckLinkPage(checkSsiUrl, ssiLinkPage, redirectedUrl, WidgetPortfolioDistributionTabs.HealthSsi.GetStringMapping());
            CheckLabelAndPortfolioNameOnRiskPage(checkLabelOnSsiPage, ssiLabelOnPage);

            LogStep(3, "Check Portfolio Distribution widget, ASSET ALLOCATION tab");
            NavigateToDashboardPageAndSelectTabAndCheckLinkTextAndUrlText(WidgetPortfolioDistributionTabs.AssetAllocation, assetText, assetLink);

            LogStep(4, "Click ASSET link");
            dashboardForm.ClickWidgetLinkUrl();
            CheckLinkPage(checkAssetUrl, assetLinkPage, redirectedUrl, WidgetPortfolioDistributionTabs.AssetAllocation.GetStringMapping());
            CheckLabelAndPortfolioNameOnAssetPage(checkLabelOnAssetPage, assetLabelOnPage);

            LogStep(5, "Check Portfolio Distribution widget, RISK (VQ) tab");
            NavigateToDashboardPageAndSelectTabAndCheckLinkTextAndUrlText(WidgetPortfolioDistributionTabs.RiskVq, riskText, riskLink);

            LogStep(6, "Click RISK link");
            dashboardForm.ClickWidgetLinkUrl();
            CheckLinkPage(checkRiskUrl, riskLinkPage, redirectedUrl, WidgetPortfolioDistributionTabs.RiskVq.GetStringMapping());
            CheckLabelAndPortfolioNameOnRiskPage(checkLabelOnRiskPage, riskLabelOnPage);
        }

        private void CheckLinkPage(bool condition, string expectedUrl, string expectedUrlRedirected, string urlType)
        {
            if (condition)
            {
                var isOpenedIntoNewTab = Browser.GetDriver().WindowHandles.Count > CountOfOpenedTab;

                if (isOpenedIntoNewTab)
                {
                    Browser.SwitchToLastWindow();
                }

                var actualUrl = Browser.GetDriver().Url;
                Checker.IsTrue(actualUrl.Contains(expectedUrl) || actualUrl.Contains(expectedUrlRedirected),
                    $"{urlType} is not as expected. Actual:\n{actualUrl}\nExpected:\n{expectedUrl} or {expectedUrlRedirected}");

                if (isOpenedIntoNewTab)
                {
                    Browser.GetDriver().Close();
                    Browser.SwitchToFirstWindow();
                }
            }
        }

        private void CheckLabelAndPortfolioNameOnAssetPage(bool condition, string expectedLabel)
        {
            if (condition)
            {
                var assetAllocationForm = new AssetAllocationForm();
                assetAllocationForm.AssertIsOpen();

                Checker.IsTrue(assetAllocationForm.IsSectionTitlePresent(expectedLabel),
                    $"{expectedLabel} section title is not displayed");
                 Checker.IsTrue(assetAllocationForm.IsSelectedPortfolioPresent(portfolioManualModel.Name),
                    $"{portfolioManualModel.Name} is not selected into portfolios block in AA");
            }
        }

        private void CheckLabelAndPortfolioNameOnRiskPage(bool condition, string expectedLabel)
        {
            if (condition)
            {
                var riskRebalancerForm = new RiskRebalancerForm();
                riskRebalancerForm.AssertIsOpen();

                Checker.IsTrue(riskRebalancerForm.IsSectionTitlePresent(expectedLabel),
                    $"{expectedLabel} section title is not displayed");
                Checker.IsTrue(riskRebalancerForm.IsSelectedPortfolioPresent(portfolioManualModel.Name),
                    $"{portfolioManualModel.Name} is not selected into portfolios block in RR");
            }
        }

        private void NavigateToDashboardPageAndSelectTabAndCheckLinkTextAndUrlText(WidgetPortfolioDistributionTabs tab, string expectedLink, string expectedLinkUrl)
        {
            new MainMenuNavigation().OpenDashboard();
            new DashboardForm().SelectPortfolioStatisticsWidgetPortfolio(portfolioManualModel.Name);
            var dashboardWidgetForm = new WidgetForm(Widget);
            dashboardWidgetForm.ClickWidgetContentTab(tab);

            Checker.CheckEquals(expectedLink, dashboardWidgetForm.GetWidgetLinkText(), 
                $"Link text is not as expected for '{tab.GetStringMapping()}' tab");
            Checker.CheckEquals(expectedLinkUrl, dashboardWidgetForm.GetWidgetLinkUrlText(), 
                $"Link url text is not as expected for '{tab.GetStringMapping()}' tab");
        }
    }
}