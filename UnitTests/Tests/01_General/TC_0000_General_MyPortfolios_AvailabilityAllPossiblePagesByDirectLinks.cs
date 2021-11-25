using AutomatedTests.Enums.Portfolios.CreateManual;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Forms.AddPositionAdvanced;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms.ResearchPages.AssetAllocation;
using AutomatedTests.Forms.ResearchPages.PVQAnalyzer;
using AutomatedTests.Forms.ResearchPages.RiskRebalancer;
using AutomatedTests.Forms.SyncFlow;
using AutomatedTests.Forms;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.BrowserSteps;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests.Utils;
using AutomatedTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._01_General
{
    [TestClass]
    public class TC_0000_General_MyPortfolios_AvailabilityAllPossiblePagesByDirectLinks : BaseTestUnitTests
    {
        private AddPortfolioManualModel portfolioModel;
        private readonly List<PositionAtManualCreatingPortfolioModel> positionsModel = new List<PositionAtManualCreatingPortfolioModel>();
        private string defaultTicker;
        private const string SelectionFlowToCreatePageDescription = "Create Portfolio Selection Flow Page";
        private const string AddPositionAdvancedPageDescription = "Add Position Advanced Mode";

        [TestInitialize]
        public void TestInitialize()
        {
            defaultTicker = new CustomTestDataReader().GetDefaultTicker();
            portfolioModel = new AddPortfolioManualModel
            {
                Name = StringUtility.RandomString("##########"),
                Currency = Currency.USD.GetStringMapping()
            };
            positionsModel.Add(new PositionAtManualCreatingPortfolioModel { Ticker = defaultTicker, PositionAssetType = PositionAssetTypes.Stock });

            LogStep(0, "Precondition - Login as registered user");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(0, ProductSubscriptions.TradeStopsPlatinum));
            PortfoliosSetUp.AddInvestmentUsdPortfoliosWithOpenPosition(UserModels.First().Email);
            PortfoliosSetUp.AddWatchOnlyUsdPortfoliosWithClosedPosition(UserModels.First().Email);

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [Description("The test checks all possible pages availability and opening using direct links https://tr.a1qa.com/index.php?/cases/view/19232839")]
        [TestMethod]
        [TestCategory("Smoke")]
        public override void RunTest()
        {
            LogStep(1, "Check Investment and Watch Only portfolios page is opened. No errors in console.");
            var mainMenuNavigation = new MainMenuNavigation();
            mainMenuNavigation.OpenInvestmentPortfoliosTab();
            var portfoliosForm = new PortfoliosForm();
            portfoliosForm.AssertIsOpen();
            Checker.IsTrue(portfoliosForm.IsPortfolioTabActive(PortfolioType.Investment), "Not Investment portfolio tb is Active");
            var browserSteps = new BrowserSteps();
            browserSteps.CheckBrowserConsoleForErrors(PortfolioType.Investment.GetStringMapping());

            mainMenuNavigation.OpenWatchOnlyPortfoliosTab();
            portfoliosForm.AssertIsOpen();
            Checker.IsTrue(portfoliosForm.IsPortfolioTabActive(PortfolioType.WatchOnly), "Not WatchOnly portfolio tb is Active");
            browserSteps.CheckBrowserConsoleForErrors(PortfolioType.WatchOnly.GetStringMapping());

            LogStep(2, "Manage -> Add portfolio: -portfolios/create; -sync-flow/import; sync-flow/alternative-import; portfolios/create/manual;  portfolios/create/csv-import/file-selector");
            mainMenuNavigation.OpenCreatePortfolioPage();
            new SelectPortfolioFlowForm().AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(SelectionFlowToCreatePageDescription);

            mainMenuNavigation.OpenSyncFlowYodleeImport();
            new SyncFlowImportForm().AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(VendorTypes.Yodlee.ToString());

            mainMenuNavigation.OpenSyncFlowFastLinkImport();
            new FastLinkImportForm().AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(VendorTypes.YodleeRestApi.ToString());

            mainMenuNavigation.OpenSyncFlowPlaidImport();
            Checker.IsTrue(new PlaidImportForm().IsPlaidFormPresent(), "Plaid Form does not presentPresent");
            browserSteps.CheckBrowserConsoleForErrors(VendorTypes.Plaid.ToString());

            mainMenuNavigation.OpenImportPortfolioFromCsv();
            new ImportPortolioFromCsvForm().AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(AddPortfolioTypes.ImportFromFile.GetStringMapping());

            mainMenuNavigation.OpenAddManualPortfolio();
            var manualPortfolioCreationForm = new ManualPortfolioCreationForm();
            manualPortfolioCreationForm.AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(AddPortfolioTypes.Manual.GetStringMapping());

            LogStep(3, "Create Portfolio (manual) page: Select Ticker = AAPL. Click Save Portfolio");
            manualPortfolioCreationForm.FillPortfolioFields(portfolioModel);
            manualPortfolioCreationForm.FillPositionsFields(positionsModel);
            manualPortfolioCreationForm.ClickPortfolioManualFlowActionsButton(PortfolioManualFlowActionsButton.SavePortfolio);
            var addAlertsAtCreatingPortfolioForm = new AddAlertsAtCreatingPortfolioForm();
            addAlertsAtCreatingPortfolioForm.AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(AddAlertsAtCreatingPortfolioButtons.AddAlerts.GetStringMapping());

            LogStep(4, "Click Add Alerts.");
            var addAlertsAtCreatingPortfolioSteps = new AddAlertsAtCreatingPortfolioSteps();
            addAlertsAtCreatingPortfolioSteps.SetAlertSliderWithChecking(AlertsToPositionsAtPortfolioCreation.VqTrailingStop, AlertsToPositionsStates.On);
            addAlertsAtCreatingPortfolioSteps.SetAlertSliderWithChecking(AlertsToPositionsAtPortfolioCreation.TrailingStop, AlertsToPositionsStates.On);
            addAlertsAtCreatingPortfolioSteps.SetAlertSliderWithChecking(AlertsToPositionsAtPortfolioCreation.PercentageGain, AlertsToPositionsStates.On);
            addAlertsAtCreatingPortfolioSteps.SetAlertSliderWithChecking(AlertsToPositionsAtPortfolioCreation.FixedPrice, AlertsToPositionsStates.On);
            addAlertsAtCreatingPortfolioForm.ClickActionButton(AddAlertsAtCreatingPortfolioButtons.AddAlerts);
            new PositionsTabForm().AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(PositionsTabs.OpenPositions.ToString());

            LogStep(5, "Check Closed tab for positions are opened. No errors in console.");
            mainMenuNavigation.OpenPositionsGrid(PositionsTabs.ClosedPositions);
            var positionsAlertsStatisticsPanelForm = new PositionsAlertsStatisticsPanelForm();
            positionsAlertsStatisticsPanelForm.SelectPortfolio(AllPortfoliosKinds.All.GetStringMapping());
            new ClosedPositionsTabForm().AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(PositionsTabs.ClosedPositions.ToString());

            LogStep(6, "Check  -> Add position advanced mode.");
            mainMenuNavigation.OpenAddPositionAdvanced();
            new AddPositionAdvancedForm().AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(AddPositionAdvancedPageDescription);

            LogStep(7, "Check -> Alerts");
            mainMenuNavigation.OpenAlertsGrid();
            positionsAlertsStatisticsPanelForm.SelectPortfolio(AllPortfoliosKinds.All.GetStringMapping());
            new AlertsTabForm().AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(MyPortfoliosMenuItems.AlertsGrid.GetStringMapping());

            LogStep(8, "Check -> Risk Rebalancer");
            mainMenuNavigation.OpenRiskRebalancer();
            new RiskRebalancerForm().AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(MyPortfoliosMenuItems.RiskRebalancer.GetStringMapping());

            LogStep(9, "Check -> Asset Allocation");
            mainMenuNavigation.OpenAssetAllocation();
            new AssetAllocationForm().AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(MyPortfoliosMenuItems.AssetAllocation.GetStringMapping());

            LogStep(10, "Check -> Pvq Analyzer");
            mainMenuNavigation.OpenPvqAnalyzer();
            new PvqAnalyzerForm().AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(MyPortfoliosMenuItems.PvqAnalyzer.GetStringMapping());
        }
    }
}