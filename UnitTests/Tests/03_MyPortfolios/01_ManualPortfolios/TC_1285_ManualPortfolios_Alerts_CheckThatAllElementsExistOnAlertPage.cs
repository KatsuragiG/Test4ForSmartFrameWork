using System;
using System.Collections.Generic;
using System.Linq;
using AutomatedTests.Enums.Portfolios.CreateManual;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._03_MyPortfolios._01_ManualPortfolios
{
    [TestClass]
    public class TC_1285_ManualPortfolios_Alerts_CheckThatAllElementsExistOnAlertPage : BaseTestUnitTests
    {
        private const int TestNumber = 1285;
        private const string TickerColumn = "ticker";
        private const string EntryDateColumn = "entryDate";
        private const string SharesColumn = "shares";
        private const string PositionTypeColumn = "positionType";

        private List<PositionAtManualCreatingPortfolioModel> positionsModels;
        private string headline;
        private string ssiText;
        private string vqText;
        private string tsText;
        private string tsFieldValue;
        private string lcFieldValue;
        private string lcText;
        private string lcDropdownValue;
        private string fpFieldValue;
        private string fpText;
        private string fpDropdownValue;
        private AlertsToPositionsStates ssiSetState;
        private AlertsToPositionsStates vqSetState;
        private AlertsToPositionsStates tsSetState;
        private AlertsToPositionsStates lcSetState;
        private AlertsToPositionsStates fpSetState;

        [TestInitialize]
        public void TestInitialize()
        {
            headline = GetTestDataAsString(nameof(headline));
            vqText = GetTestDataAsString(nameof(vqText));
            tsText = GetTestDataAsString(nameof(tsText));
            lcText = GetTestDataAsString(nameof(lcText));
            fpText = GetTestDataAsString(nameof(fpText));
            ssiText = GetTestDataAsString(nameof(ssiText));
            tsFieldValue = GetTestDataAsString(nameof(tsFieldValue));
            lcFieldValue = GetTestDataAsString(nameof(lcFieldValue));
            lcDropdownValue = GetTestDataAsString(nameof(lcDropdownValue));
            fpFieldValue = GetTestDataAsString(nameof(fpFieldValue));
            fpDropdownValue = GetTestDataAsString(nameof(fpDropdownValue));
            ssiSetState = GetTestDataAsString(nameof(ssiSetState)).ParseAsEnumFromStringMapping<AlertsToPositionsStates>();
            vqSetState = GetTestDataAsString(nameof(vqSetState)).ParseAsEnumFromStringMapping<AlertsToPositionsStates>();
            tsSetState = GetTestDataAsString(nameof(tsSetState)).ParseAsEnumFromStringMapping<AlertsToPositionsStates>();
            lcSetState = GetTestDataAsString(nameof(lcSetState)).ParseAsEnumFromStringMapping<AlertsToPositionsStates>();
            fpSetState = GetTestDataAsString(nameof(lcSetState)).ParseAsEnumFromStringMapping<AlertsToPositionsStates>();

            var portfolioModel = new AddPortfolioManualModel
            {
                Name = "CheckAlertsIfStartDateLessThanFirstTradingDay"
            };

            InitializePositionModels();

            LogStep(0, "Preconditions: Login as Premium. Set alerts via DB. Click 'Add Manually'");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, new List<ProductSubscriptions>
                {
                    ProductSubscriptions.TradeStopsLifetime, ProductSubscriptions.CryptoStopsLifetime
                }
            ));

            LoginSetUp.LogIn(UserModels.First());

            new AddPortfoliosSteps().NavigateToAddManualPortfolioSavePortfolioAfterFillingFields(portfolioModel, positionsModels);
        }

        private void InitializePositionModels()
        {
            positionsModels = new List<PositionAtManualCreatingPortfolioModel>();
            var tableColumns = TestContext.DataRow.Table.Columns;

            foreach (var column in tableColumns)
            {
                if (column.ToString().Contains(TickerColumn) && !string.IsNullOrEmpty(GetTestDataAsString(column.ToString())))
                {
                    var index = column.ToString().Split(new[] { TickerColumn }, StringSplitOptions.None).Last();
                    positionsModels.Add(new PositionAtManualCreatingPortfolioModel
                    {
                        Ticker = GetTestDataAsString($"{TickerColumn}{index}"),
                        EntryDate = GetTestDataAsString($"{EntryDateColumn}{index}"),
                        Quantity = GetTestDataAsString($"{SharesColumn}{index}"),
                        PositionAssetType = GetTestDataAsString($"{PositionTypeColumn}{index}").ParseAsEnumFromStringMapping<PositionAssetTypes>()
                    });
                }
            }
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1285$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("Alerts"), TestCategory("AlertAdd")]
        [Description("Test for existing of all elements on the Alerts page and its correct default state: https://tr.a1qa.com/index.php?/cases/view/19234200")]
        public override void RunTest()
        {
            LogStep(1, "Check page headline is displayed on the top of the page");
            var addAlertsAtCreatingPortfolioForm = new AddAlertsAtCreatingPortfolioForm();
            Checker.CheckEquals(headline, addAlertsAtCreatingPortfolioForm.GetAlertHeadlineText(), 
                "Alert headline is not as expected");
            
            CheckSliderAndAlertLabelAndDescription(2, AlertsToPositionsAtPortfolioCreation.HealthSsi, ssiSetState, ssiText);

            CheckSliderAndAlertLabelAndDescription(5, AlertsToPositionsAtPortfolioCreation.VqTrailingStop, vqSetState, vqText);

            var alert = AlertsToPositionsAtPortfolioCreation.TrailingStop;
            CheckSliderAndAlertLabelAndDescription(8, alert, tsSetState, tsText);

            LogStep(11, $"Check that {alert.GetStringMapping()} field is filled with '{tsFieldValue}' by default");
            Checker.CheckEquals(tsFieldValue, addAlertsAtCreatingPortfolioForm.GetAlertTextFromInput(alert), 
                $"{alert.GetStringMapping()} field value is not as expected");

            alert = AlertsToPositionsAtPortfolioCreation.PercentageGain;
            CheckSliderAndAlertLabelAndDescription(12, alert, lcSetState, lcText);

            LogStep(15, $"Check that {alert.GetStringMapping()} field is filled with '{lcFieldValue}' by default");
            Checker.CheckEquals(lcFieldValue, addAlertsAtCreatingPortfolioForm.GetAlertTextFromInput(alert),
                $"{alert.GetStringMapping()} field value is not as expected");

            LogStep(16, "Check that 'below' is selected in the Percentage gain dropdown by default");
            Checker.CheckEquals(lcDropdownValue, addAlertsAtCreatingPortfolioForm.GetValueFromAlertDropDown(alert),
                $"{alert.GetStringMapping()} dropdown value is not as expected");

            alert = AlertsToPositionsAtPortfolioCreation.FixedPrice;
            CheckSliderAndAlertLabelAndDescription(17, alert, fpSetState, fpText);

            LogStep(20, $"Check that {alert.GetStringMapping()} field is filled with '{fpFieldValue}' by default");
            Checker.CheckEquals(fpFieldValue, addAlertsAtCreatingPortfolioForm.GetAlertTextFromInput(alert),
                $"{alert.GetStringMapping()} field value is not as expected");

            LogStep(21, "Check that 'below' is selected in the Fixed Price dropdown by default");
            Checker.CheckEquals(fpDropdownValue, addAlertsAtCreatingPortfolioForm.GetValueFromAlertDropDown(alert),
                $"{alert.GetStringMapping()} dropdown value is not as expected");

            LogStep(22, "Check that 'I will add alerts later' button exists");
            Checker.IsTrue(addAlertsAtCreatingPortfolioForm.IsActionButtonPresent(AddAlertsAtCreatingPortfolioButtons.AddAlertsLater),
                $"{AddAlertsAtCreatingPortfolioButtons.AddAlertsLater.GetStringMapping()} button is not present");

            LogStep(23, "	Check that 'Add Alerts' button exists");
            Checker.IsTrue(addAlertsAtCreatingPortfolioForm.IsActionButtonPresent(AddAlertsAtCreatingPortfolioButtons.AddAlerts),
                $"{AddAlertsAtCreatingPortfolioButtons.AddAlerts.GetStringMapping()} button is not present");
        }

        private void CheckSliderAndAlertLabelAndDescription(int step, AlertsToPositionsAtPortfolioCreation alert, AlertsToPositionsStates expAlertSetState,
            string expAlertDescriptionText)
        {
            var addAlertsAtCreatingPortfolioForm = new AddAlertsAtCreatingPortfolioForm();

            LogStep(step++, $"Check that '{alert.GetStringMapping()}' toggle button is displayed and set On by default");
            Checker.CheckEquals(expAlertSetState, addAlertsAtCreatingPortfolioForm.GetAlertSlideState(alert), 
                $"{alert.GetStringMapping()} slider state is not as expected");

            LogStep(step++, $"Check that '{alert.GetStringMapping()}' label is displayed");
            Checker.IsTrue(addAlertsAtCreatingPortfolioForm.IsAlertLabelPresent(alert),
                $"{alert.GetStringMapping()} label is not present");

            LogStep(step, $"Check that Text for '{alert.GetStringMapping()}' from test data is displayed below the '{alert.GetStringMapping()}' label");
            Checker.CheckEquals(expAlertDescriptionText, addAlertsAtCreatingPortfolioForm.GetAlertDescriptionTextLabel(alert), 
                $"{alert.GetStringMapping()} description is not as expected");

            var labelLocation = addAlertsAtCreatingPortfolioForm.GetAlertLabelLocation(alert);
            var descriptionLocation = addAlertsAtCreatingPortfolioForm.GetAlertDescriptionTextLocation(alert);
            Checker.IsTrue(labelLocation.Y < descriptionLocation.Y,
                $"{alert.GetStringMapping()}: Description is not displayed below the label. " +
                    $"Description: [{descriptionLocation.X},{descriptionLocation.Y}]. Label: [{labelLocation.X},{labelLocation.Y}]");
        }
    }
}