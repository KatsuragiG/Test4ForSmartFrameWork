using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Alerts;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Alerts;
using AutomatedTests.Enums.Newsletter;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.Settings;
using AutomatedTests.Enums.Tools;
using AutomatedTests.Enums.User;
using AutomatedTests.Enums;
using AutomatedTests.Forms.Gurus;
using AutomatedTests.Forms.MyPortfolios;
using AutomatedTests.Forms.Popups;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms.ResearchPages.RiskRebalancer;
using AutomatedTests.Forms.Settings;
using AutomatedTests.Forms.Templates;
using AutomatedTests.Forms;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.ResearchPages;
using AutomatedTests.Steps.Settings;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._01_General
{
    [TestClass]
    public class TC_1038_General_HealthAndRebalancerIsAvailableForNewlyLoggedInUser : BaseTestUnitTests
    {
        private const int TestNumber = 1038;

        private ProductSubscriptionTypes userType;
        private PortfolioModel portfolioModel;
        private readonly List<PositionsDBModel> positionsModels = new List<PositionsDBModel>();
        private AlertsDbModel alertModel;
        private readonly List<int> positionsIds = new List<int>();

        [TestInitialize]
        public void TestInitialize()
        {
            userType = GetTestDataParsedAsEnumFromStringMapping<ProductSubscriptionTypes>("User");

            portfolioModel = new PortfolioModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency")
            };
            positionsModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("Symbol1"),
                PurchaseDate = DateTime.Now.ToShortDateString()
            });
            positionsModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("Symbol2"),
                PurchaseDate = DateTime.Now.ToShortDateString()
            });
            positionsModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("Symbol3"),
                PurchaseDate = DateTime.Now.ToShortDateString()
            });
            alertModel = new AlertsDbModel
            {
                TriggerTypeId = $"{(int)AlertTypes.TwoVq}"
            };

            LogStep(0, "Precondition - Login as registered user");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, (ProductSubscriptions)(int)userType));

            var portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            positionsIds.Add(PositionsAlertsSetUp.AddPositionViaDB(portfolioId, positionsModels[0]));
            PositionsAlertsSetUp.AddPositionViaDB(portfolioId, positionsModels[1]);
            positionsIds.Add(PositionsAlertsSetUp.AddPositionViaDB(portfolioId, positionsModels[2]));

            PositionsAlertsSetUp.AddAlertViaDB(positionsIds[0], alertModel);
            PositionsAlertsSetUp.AddAlertViaDB(positionsIds[1], alertModel);

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1038$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("ResearchPage"), TestCategory("RiskRebalancer"), TestCategory("Permissions"), TestCategory("SettingsPageAlertsTab")]
        [Description("The test checks available Health and Risk Rebalancer for different subscriptions. https://tr.a1qa.com/index.php?/cases/view/19232018")]
        public override void RunTest()
        {
            LogStep(1, "Open Positions & Alerts -> Positions.Make sure SSI column is present on the page.");
            var mainMenuNavigation = new MainMenuNavigation();
            mainMenuNavigation.OpenPositionsGrid();
            var positionsTabForm = new PositionsTabForm();
            Checker.IsTrue(positionsTabForm.IsColumnPresent(PositionsGridDataField.Health), "Health column is not present on Positions grid");

            LogStep(2, "Check Health pill for positions in opened portfolio");
            var ssiRows = positionsTabForm.GetSsiColumnValues();
            Checker.IsTrue(ssiRows.Any(t => !string.IsNullOrEmpty(t)), "At least one SSI is NA on Open Positions grid");

            LogStep(3, "Click button Add Position");
            positionsTabForm.ClickAddPositionButton();

            LogStep(4, "Open drop-down Alerts.Make sure Stock State Indicator alert is present on the list");
            var addPositionInFrameForm = new AddPositionInFrameForm();
            Checker.IsFalse(addPositionInFrameForm.GetAvailableTemplates().Contains(DefaultTemplateTypes.HealthIndicator.GetStringMapping()), 
                "Health alert is exists in add position inline templates");
            addPositionInFrameForm.CloseInlineForm();

            LogStep(5, "Click arrow near symbol -> Close Position");
            positionsTabForm.AssertIsOpen();
            positionsTabForm.SelectPositionContextMenuOption(positionsIds[1], PositionContextNavigation.ClosePosition);

            LogStep(6, "Click Close Position in the Close Position popup.");
            new ClosePositionPopup().ClickClosePositionButton();

            LogStep(7, "Click OK button on SUCCESS popup");
            new ConfirmPopup(PopupNames.Success).ClickOkButton();

            LogStep(8, "Open *Closed Positions* tab");
            new PositionsAlertsStatisticsPanelForm().ActivateTab(PositionsTabs.ClosedPositions);

            LogStep(9, "Check Health pill for Position2_Ticker from test data");
            var closedPositionTabForm = new ClosedPositionsTabForm();
            Checker.IsTrue(closedPositionTabForm.IsColumnPresent(ClosedPositionsGridDataField.Health), "Health column is not exist on Closed Positions grid");
            
            var closedSsiRows = closedPositionTabForm.GetSsiColumnValues();
            Checker.IsTrue(closedSsiRows.Any(t => !string.IsNullOrEmpty(t)), "At least 1 SSI is NA on Closed Positions grid");

            LogStep(10, "Open *Alerts* tab");
            new MyPortfoliosMenuForm().ClickMyPortfoliosMenuItem(MyPortfoliosMenuItems.AlertsGrid);

            LogStep(11, "Check Health pill for Position1_Ticker from test data");
            var alertsTabForm = new AlertsTabForm();
            Checker.IsTrue(alertsTabForm.IsColumnPresent(AlertsGridColumnsDataField.Health), "Health column is not exists on Alerts grid");

            var alertsSsiRows = alertsTabForm.GetSsiColumnValues();
            Checker.IsTrue(alertsSsiRows.Any(t => !string.IsNullOrEmpty(t)), "At least 1 SSI is NA on Alerts grid");

            LogStep(12, "Open page of custom *publisher page*");
            mainMenuNavigation.OpenCustomPublisherGrid();

            LogStep(13, 14, "Make sure Health column is present on the page. Make sure Health pill for at least one symbol != N/A.");
            var customPublisherForm = new SelectedPublisherForm();
            Checker.IsTrue(customPublisherForm.IsColumnPresent(NewslettersGridColumnTypes.Health), "SSI column is not exists on Custom Publisher page");

            var publisherSsiRows = customPublisherForm.GetSsiPillsColumn();
            Checker.IsTrue(customPublisherForm.IsValidSsiExistInSsiPillsColumn(publisherSsiRows), "No SSI in non-NA on Custom Publisher page");

            LogStep(17, "Open *Settings* -> *Alerts*. Put Create alerts for stocks? radio-button to Yes");
            var alertsSettings = new SettingsSteps().NavigateToSettingsAlertsGetForm();

            LogStep(18, "Put Create alerts for stocks? toggle to Yes");
            alertsSettings.CreateAlertsForStock(true);

            LogStep(19, 20, "Open drop-down Stock alert type:.Make sure Stock State Indicator alert is present on the list");
            Checker.IsFalse(alertsSettings.IsHealthStockIndicatorExists(), "Health Indicator alert is present on the list of Templates");
            new SettingsMainForm().SaveSettings();

            LogStep(21, "Open *TEMPLATES*");
            new MainMenuForm().ClickMenuItem(MainMenuItems.AlertTemplates);

            LogStep(22, "Make sure Health Indicator template is present on the list");
            var templatesForm = new TemplatesForm();
            Checker.IsFalse(templatesForm.IsTemplateExist(DefaultTemplateTypes.HealthIndicator.GetStringMapping()), "Health template doesn't exists");

            LogStep(23, 25, "Open My Portfolios -> Risk Rebalancer. Select portfolio. Click Rebalance. Open on Rebalanced Results tab");
            new RiskRebalancerSteps().OpenRebalancerSelectPortfolioClickRebalanceSelectRebalancedTab(portfolioModel.Name, RiskRebalancerTabs.RebalancedResults);

            LogStep(26, "Make sure Health column is presented in the results grid.");
            var riskRebalancer = new RiskRebalancerForm();
            Checker.IsTrue(riskRebalancer.IsColumnGridPresent(RiskRebalancerResultColumnTypes.SsiStatus), "Health column is exist on Risk Rebalancer");

            var rebalancerSsiRows = riskRebalancer.GetSsiColumnValues();
            Checker.IsTrue(rebalancerSsiRows.Any(t => !string.IsNullOrEmpty(t)), "At least 1 SSI in non-NA on Risk Rebalancer page");
        }
    }
}