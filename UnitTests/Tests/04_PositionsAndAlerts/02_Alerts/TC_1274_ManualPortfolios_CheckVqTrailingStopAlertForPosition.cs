using System.Collections.Generic;
using System.Linq;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.BaseTestsUnitTests;
using AutomatedTests.Enums.Positions;
using TradeStops.Common.Enums;
using AutomatedTests.Enums.Portfolios.CreateManual;
using AutomatedTests.Forms.MyPortfolios;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Alerts;
using AutomatedTests.Enums.Sorting;
using TradeStops.Common.Extensions;

namespace UnitTests.Tests._04_PositionsAndAlerts._02_Alerts
{
    [TestClass]
    public class TC_1274_ManualPortfolios_CheckVqTrailingStopAlertForPosition : BaseTestUnitTests
    {
        private const int TestNumber = 1274;

        private AddPortfolioManualModel portfolioModel;
        private readonly List<PositionAtManualCreatingPortfolioModel> positionsModels = new List<PositionAtManualCreatingPortfolioModel>();
        private List<string> vqValues;
        private string alertsDescription;
        private int positionQuantity;

        [TestInitialize]
        public void TestInitialize()
        {
            portfolioModel = new AddPortfolioManualModel
            {
                Name = "CheckVqTrailingStopAlertForPosition"
            };

            positionQuantity = GetTestDataAsInt(nameof(positionQuantity));

            for (int i = 1; i <= positionQuantity; i++)
            {
                positionsModels.Add(new PositionAtManualCreatingPortfolioModel
                {
                    Ticker = GetTestDataAsString($"ticker{i}")
                });
                positionsModels.Last().PositionAssetType = positionsModels.Last().Ticker.Contains("/")
                    ? PositionAssetTypes.Crypto
                    : PositionAssetTypes.Stock;
            }

            vqValues = GetTestDataValuesAsListByColumnName(nameof(vqValues));

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
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1274$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("Alerts"), TestCategory("AlertAdd"), TestCategory("TrailingStopAlertsGroup")]
        [Description("Test for VQ Trailing Stop saves with 25% value for non-VQ Symbols; " +
            "VQ Trailing Stop saves with an appropriate value for VQ Symbols: https://tr.a1qa.com/index.php?/cases/view/19234202")]
        public override void RunTest()
        {
            LogStep(1, 2, "Switch the all alerts Off and switch the VQ Trailing Stop alert On");
            new AddAlertsAtCreatingPortfolioSteps().SetAlertSlidersWithChecking(vqState: AlertsToPositionsStates.On);

            LogStep(3, "Click 'Add Alerts' button");
            new AddAlertsAtCreatingPortfolioForm().ClickActionButton(AddAlertsAtCreatingPortfolioButtons.AddAlerts);
            var positionsAlertsStatisticsPanelForm = new PositionsAlertsStatisticsPanelForm();
            positionsAlertsStatisticsPanelForm.AssertIsOpen();

            LogStep(4, "Go to Alerts tab");
            new MyPortfoliosMenuForm().ClickMyPortfoliosMenuItem(MyPortfoliosMenuItems.AlertsGrid);
            var alertsTabForm = new AlertsTabForm();
            Checker.CheckEquals(portfolioModel.Name, positionsAlertsStatisticsPanelForm.GetPortfolioName(),
                "Portfolio name is not selected in the portfolio drop-down");

            LogStep(5, "Sort grid by Symbol ASC");
            alertsTabForm.ClickOnPositionColumnToSort(AlertsGridColumnsDataField.Ticker, SortingStatus.Asc);
            var alertsSymbols = alertsTabForm.GetColumnValues(AlertsGridColumnsDataField.Ticker).Select(r => r.Split('\r')[0]).ToList();
            var sortedAlertsSymbols = alertsSymbols.OrderBy(q => q).ToList();
            Checker.IsFalse(ObjectComparator.CompareArrays(sortedAlertsSymbols, alertsSymbols).Any(), 
                "Alerts are not sorted by Symbol field ASC");

            LogStep(6, "Make sure there are 'VQ Trailing Stop' alerts for the added into the portfolio positions.");
            var alertsDescriptionsOnGrid = alertsTabForm.GetColumnValues(AlertsGridColumnsDataField.AlertDescription);
            var vqValuesOnGrid = alertsTabForm.GetColumnValues(AlertsGridColumnsDataField.Vq);

            for (var i = 0; i < positionsModels.Count; i++)
            {
                var ticker = positionsModels[i].Ticker;

                if (!alertsSymbols.Any(r => r.EqualsIgnoreCase(ticker)))
                {
                    Checker.Fail($"{ticker} alerts was not added on grid");
                }
                else
                {
                    var indexOnGrid = alertsSymbols.IndexOf(ticker);
                    Checker.CheckEquals(vqValues[i], vqValuesOnGrid[indexOnGrid],  $"{ticker} VQ value is not as expected");
                    Checker.CheckEquals(alertsDescription, alertsDescriptionsOnGrid[indexOnGrid],  
                        $"{ticker} alert description is not as expected");
                }
            }
        }
    }
}