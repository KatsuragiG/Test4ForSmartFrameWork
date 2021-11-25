using System.Collections.Generic;
using System.Linq;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.BaseTestsUnitTests;
using AutomatedTests.Enums.Portfolios.CreateManual;
using TradeStops.Common.Enums;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms.MyPortfolios;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Alerts;
using TradeStops.Common.Extensions;

namespace UnitTests.Tests._04_PositionsAndAlerts._02_Alerts
{
    [TestClass]
    public class TC_1275_ManualPortfolios_CheckTrailingStopPercentAlertForPosition : BaseTestUnitTests
    {
        private const int TestNumber = 1275;

        private AddPortfolioManualModel portfolioModel;
        private List<PositionAtManualCreatingPortfolioModel> positionsModels;
        private string alertsDescription;
        private string trailingStops;

        [TestInitialize]
        public void TestInitialize()
        {
            portfolioModel = new AddPortfolioManualModel
            {
                Name = "CheckTrailingStopPercentAlertForPosition"
            };

            trailingStops = GetTestDataAsString(nameof(trailingStops));

            positionsModels = new List<PositionAtManualCreatingPortfolioModel>
            {
                new PositionAtManualCreatingPortfolioModel { Ticker = GetTestDataAsString("ticker1") },
                new PositionAtManualCreatingPortfolioModel { Ticker = GetTestDataAsString("ticker2") }
            };
            foreach (var positionModel in positionsModels)
            {
                positionModel.PositionAssetType = positionModel.Ticker.Contains("/") ? PositionAssetTypes.Crypto : PositionAssetTypes.Stock;
            }

            alertsDescription = GetTestDataAsString(nameof(alertsDescription));

            LogStep(0, "Preconditions: Login as Premium. Create the Portfolio");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPlatinum));
            PortfoliosSetUp.AddInvestmentUsdPortfoliosWithOpenPosition(UserModels.First().Email);

            LoginSetUp.LogIn(UserModels.First());

            new AddPortfoliosSteps().NavigateToAddManualPortfolioSavePortfolioAfterFillingFields(portfolioModel, positionsModels);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1275$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("Alerts"), TestCategory("AlertAdd"), TestCategory("TrailingStopAlertsGroup")]
        [Description("Test for Trailing Stop alert save with default and custom values: https://tr.a1qa.com/index.php?/cases/view/19234203")]
        public override void RunTest()
        {
            LogStep(1, 2, "Switch the all alerts Off and switch the Trailing Stop alert On");
            new AddAlertsAtCreatingPortfolioSteps().SetAlertSlidersWithChecking(tsState: AlertsToPositionsStates.On);

            LogStep(3, "Specify Trailing Stop % from test data column Trailing Stop");
            var addAlertsAtCreatingPortfolioForm = new AddAlertsAtCreatingPortfolioForm();
            if (!string.IsNullOrEmpty(trailingStops))
            {
                addAlertsAtCreatingPortfolioForm.SetAlertTextIntoInput(AlertsToPositionsAtPortfolioCreation.TrailingStop, trailingStops);
                Checker.CheckEquals(trailingStops, addAlertsAtCreatingPortfolioForm.GetAlertTextFromInput(AlertsToPositionsAtPortfolioCreation.TrailingStop),
                    "Filled TrailingStop value is not as expected");
            }

            LogStep(4, "Click 'Add Alerts' button");
            addAlertsAtCreatingPortfolioForm.ClickActionButton(AddAlertsAtCreatingPortfolioButtons.AddAlerts);
            var positionsAlertsStatisticsPanelForm = new PositionsAlertsStatisticsPanelForm();
            positionsAlertsStatisticsPanelForm.AssertIsOpen();

            LogStep(5, "Go to Alerts tab");
            new MyPortfoliosMenuForm().ClickMyPortfoliosMenuItem(MyPortfoliosMenuItems.AlertsGrid);
            var alertsTabForm = new AlertsTabForm();
            Checker.CheckEquals(portfolioModel.Name, positionsAlertsStatisticsPanelForm.GetPortfolioName(),
                "Portfolio name is not selected in the portfolio drop-down");

            LogStep(6, "Make sure there is 'Trailing Stop %' alert for the added into the portfolio position");
            var alertsSymbols = alertsTabForm.GetColumnValues(AlertsGridColumnsDataField.Ticker).Select(r => r.Split('\r')[0]).ToList();
            var alertsDescriptionsOnGrid = alertsTabForm.GetColumnValues(AlertsGridColumnsDataField.AlertDescription);

            foreach (var ticker in positionsModels.Select(positionModel => positionModel.Ticker))
            {
                if (!alertsSymbols.Any(r => r.EqualsIgnoreCase(ticker)))
                {
                    Checker.Fail($"{ticker} alerts was not added on grid");
                }
                else
                {
                    var indexOnGrid = alertsSymbols.IndexOf(ticker);
                    Checker.CheckEquals(alertsDescription, alertsDescriptionsOnGrid[indexOnGrid],
                        $"{ticker} alert description is not as expected");
                }
            }
        }
    }
}