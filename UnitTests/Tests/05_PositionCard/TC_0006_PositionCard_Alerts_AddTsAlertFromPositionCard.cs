using System;
using System.Linq;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Alerts;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests.Steps.PositionCard;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.BaseTestsUnitTests;
using AutomatedTests.Enums.User;
using TradeStops.Common.Enums;
using AutomatedTests.Enums.PositionCard;
using AutomatedTests.Forms.MyPortfolios;
using AutomatedTests.Models.AlertsModels;
using AutomatedTests.Enums.Positions;

namespace UnitTests.Tests._05_PositionCard
{
    [TestClass]
    public class TC_0006_PositionCard_Alerts_AddTsAlertFromPositionCard : BaseTestUnitTests
    {
        private const int TestNumber = 6;
        private const int AddedAlertsQuantity = 1;
        private const int DaysShiftForEntryDate = -3;

        private PortfolioModel portfolioModel;
        private PositionsDBModel positionModel;
        private PercentageTrailingStopModel percentageTrailingStopModel;
        private int positionId;
        private string expectedAlertDescription;
        private bool isIntradayAvailable;
        private bool alertState;

        [TestInitialize]
        public void TestInitialize()
        {
            var userType = GetTestDataParsedAsEnumFromStringMapping<ProductSubscriptionTypes>("user");

            portfolioModel = new PortfolioModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency")
            };
            positionModel = new PositionsDBModel
            {
                Symbol = GetTestDataAsString("Symbol"),
                TradeType = $"{(int)GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>("TradeType")}",
                PurchasePrice = GetTestDataAsString("EntryPrice"),
                SplitsAdj = GetTestDataAsString("EntryPrice"),
                PurchaseDate = DateTime.Now.AddDays(DaysShiftForEntryDate).ToShortDateString()
            };
            percentageTrailingStopModel = new PercentageTrailingStopModel
            {
                Type = TrailingStopAlertTypes.Ts.ToString(),
                Percent = GetTestDataAsString("AlertValue"),
                StartDate = Parsing.ConvertToShortDateString(DateTime.Now.AddDays(GetTestDataAsInt("StartDate")).ToShortDateString())
            };
            isIntradayAvailable = GetTestDataAsBool(nameof(isIntradayAvailable));
            percentageTrailingStopModel.IsIntraday = isIntradayAvailable && GetTestDataAsBool("isIntradayAlert");
            alertState = GetTestDataAsBool(nameof(alertState));
            expectedAlertDescription = $"{Parsing.ConvertToDouble(percentageTrailingStopModel.Percent):.00}% Trailing Stop";

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, (ProductSubscriptions)(int)userType));
            var portfolioId = new PortfolioGridsSteps().LoginCreatePortfolioViaDbGetPortfolioId(UserModels.First(), portfolioModel);
            positionId = PositionsAlertsSetUp.AddPositionViaDB(portfolioId, positionModel);
            new PositionCardSteps().ResavePositionCard(positionId);
            new PositionCardForm().ActivateTab(PositionCardTabs.Alerts);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_06$", DataAccessMethod.Sequential)]
        [Description("The test checks Possibility to add Trailing stop alert from position card and check alert presence on grid and DB. https://tr.a1qa.com/index.php?/cases/view/20397831")]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PositionCard"), TestCategory("Alerts"), TestCategory("AlertAdd"), TestCategory("TrailingStopAlertsGroup")]
        public override void RunTest()
        {
            LogStep(1, 2, "Add required alert. ");
            var alertTabPositionCardSpaSteps = new AlertTabPositionCardSteps();
            alertTabPositionCardSpaSteps.GetAlertPercentageTrailingStopForm(TrailingStopAlertTypes.Ts);

            LogStep(3, 5, "Validate Added Ts Alert in UI and DB");
            alertTabPositionCardSpaSteps.ValidateAddedTsAlert(positionId, percentageTrailingStopModel, expectedAlertDescription, alertState);

            LogStep(6, "Click on the portfolio link for this Position Card");
            new PositionCardForm().ClickOnPortfolioLink();
            new PositionsTabForm().AssertIsOpen();

            LogStep(7, "Go to on Alerts tab");
            new MyPortfoliosMenuForm().ClickMyPortfoliosMenuItem(MyPortfoliosMenuItems.AlertsGrid);
            var alertsTab = new AlertsTabForm();

            LogStep(8, "Check Alert Description for the created alert in UI and DB");
            var alertDescriptions = alertsTab.GetColumnValues(AlertsGridColumnsDataField.AlertDescription);
            Assert.IsFalse(alertDescriptions.Count == 0,
                $"Alert Description column does not contain info for position '{positionModel.Symbol}'");
            Checker.CheckEquals(expectedAlertDescription, alertDescriptions.First(), 
                $"Alert Description '{expectedAlertDescription}' not found on Alerts tab for position '{positionModel.Symbol}'");
            Assert.AreEqual(alertState, alertsTab.IsAlertTriggered(AddedAlertsQuantity), "Color of Bell icon is not as expected");
        }
    }
}