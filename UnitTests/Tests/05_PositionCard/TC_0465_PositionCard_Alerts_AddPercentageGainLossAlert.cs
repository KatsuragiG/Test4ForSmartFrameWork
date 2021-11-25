using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Alerts;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Alerts;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Forms.Alerts;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Forms;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.PositionCard;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._05_PositionCard
{
    [TestClass]
    public class TC_0465_PositionCard_Alerts_AddPercentageGainLossAlert : BaseTestUnitTests
    {
        private const int TestNumber = 465;

        private GainLossOperationType operationType;
        private bool alertState;
        private bool isNotificationSettingsShown;
        private bool? isIntraday;
        private bool isIntradayPriceRequired;
        private string expectedFilledAlertDescription;
        private string alertValue;
        private int positionId;

        [TestInitialize]
        public void TestInitialize()
        {
            var userProductSubscriptions = GetUserProductSubscriptions("user");

            var portfolioModel = new PortfolioModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency")
            };

            operationType = GetTestDataParsedAsEnumFromStringMapping<GainLossOperationType>(nameof(operationType));
            alertValue = GetTestDataAsString(nameof(alertValue));
            var expectedAlertDescription = GetTestDataAsString("expectedAlertDescription");
            var intradayDescription = GetTestDataAsString("intradayDescription");
            alertState = GetTestDataAsBool(nameof(alertState));
            isNotificationSettingsShown = GetTestDataAsBool(nameof(isNotificationSettingsShown));
            if (isNotificationSettingsShown)
            {
                isIntraday = GetTestDataAsBool(nameof(isIntraday));
            }

            var entryPrice = GetTestDataAsString("EntryPrice");
            var entryDate = GetTestDataAsString("EntryDate");
            isIntradayPriceRequired = isIntraday.HasValue && isNotificationSettingsShown && (bool)isIntraday;
            var positionModel = new PositionsDBModel
            {
                Symbol = GetTestDataAsString("Symbol"),
                TradeType = $"{(int)GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>("TradeType")}",
                PurchasePrice = entryPrice,
                SplitsAdj = entryPrice,
                PurchaseDate = string.IsNullOrEmpty(entryDate)
                    ? null
                    : entryDate,
                VendorHoldingId = GetTestDataAsBool("IsSynch")
                    ? StringUtility.RandomString("###")
                    : null,
                UseIntraday = isIntradayPriceRequired
                    ? Constants.DefaultAlertValue
                    : Constants.DefaultStringZeroIntValue
            };

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));
            var portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            positionId = PositionsAlertsSetUp.AddPositionViaDB(portfolioId, positionModel);

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
            new PositionCardSteps().ResavePositionCard(positionId);
            Browser.Refresh();

            expectedFilledAlertDescription =
                string.Format(expectedAlertDescription, operationType.GetStringMapping(), alertValue.RoundToDoubleWithFirstNonZeroDecimal(),
                    isIntradayPriceRequired
                        ? intradayDescription
                        : string.Empty)
                .TrimEnd();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_465$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test checks correctness adding not triggered Percentage Gain/Loss alert. https://tr.a1qa.com/index.php?/cases/view/22467325")]
        [TestCategory("PositionCard"), TestCategory("Alerts"), TestCategory("AlertAdd"), TestCategory("PriceAlertsGroup")]
        public override void RunTest()
        {
            LogStep(1, 2, "Click on position and open New Position Card. Click 'Add Alert' button and than click on Price tab in list.");
            new AlertTabPositionCardSteps().GetAddAlertsFormByAlertCategory(positionId, AlertCategoryTypes.Price);

            LogStep(3, 4, $"Percentage Gain/Loss'.Select / Enter:-{alertValue}; - {operationType}. Click on 'Add Alert' button");
            var percentageGainLossAlertForm = new PercentageGainLossAlertForm();
            Checker.CheckEquals(GainLossOperationType.Gain.GetStringMapping(), percentageGainLossAlertForm.GetPercentageGainLossOperationType(),
                "Default operation type is not as expected");
            Checker.CheckEquals(Constants.TsDefaultPercent.ToString(), percentageGainLossAlertForm.GetPercentageGainLossValue(),
                "Default Percentage Gain Loss Value is not as expected");
            Checker.CheckEquals(isNotificationSettingsShown, percentageGainLossAlertForm.IsNotificationSettingsShown(AlertTypes.PercentageGainLoss),
                "Default Percentage Gain Loss notification settings is not as expected");
            percentageGainLossAlertForm.AddPercentageGainLoss(alertValue, operationType, isIntraday);
            var alertsTabPositionCardForm = new AlertsTabPositionCardForm();
            Assert.IsTrue(alertsTabPositionCardForm.IsAlertPresent(expectedFilledAlertDescription),
                $"Alert '{expectedFilledAlertDescription}' is not present on Position Card page");

            LogStep(5, "Click sign Pencil (Edit) sign for the alert and make sure that parameters are shown correctly.");
            var addedPercentageGainLossAlertForm = alertsTabPositionCardForm.EditAlertGetAlertForm<PercentageGainLossAlertForm>(expectedFilledAlertDescription);
            Asserts.Batch(
                () =>
                    Assert.AreEqual(operationType.ToString(), addedPercentageGainLossAlertForm.GetPercentageGainLossOperationType(),
                        $"Percentage Gain Loss Operation Type is not {operationType}"),
                () =>
                    Assert.AreEqual(alertValue.TrimEnd(Constants.DefaultStringZeroIntValue[0]), addedPercentageGainLossAlertForm.GetPercentageGainLossValue(),
                        $"Percentage Gain Loss Value is not {alertValue}")
            );
            if (isNotificationSettingsShown && isIntraday.HasValue)
            {
                Checker.CheckEquals((bool)isIntraday,
                    addedPercentageGainLossAlertForm.IsLatestPriceIntradayEnabled(AlertTypes.PercentageGainLoss),
                    "Intraday state is not as expected");
            }

            LogStep(6, "Click sign 'Cancel' button");
            new PositionCardForm().ClickCancel();

            LogStep(7, "Make sure that color of Bell icon is white.");
            Checker.CheckEquals(alertState, alertsTabPositionCardForm.IsAlertTriggeredHasPurpleBellIcon(expectedFilledAlertDescription),
                "Color of Bell icon is not as expected");
            var lastTriggered = alertState
                ? Parsing.ConvertToShortDateString(Constants.DateFormatRegex.Match(alertsTabPositionCardForm.GetAlertHint(expectedFilledAlertDescription)).ToString())
                : null;

            LogStep(8, "Check that the alert in DB have corresponded changeable settings and correct trigger type. Make sure correspondences between trigger type and color of 'Bell' icon.");
            var alertsQueries = new AlertsQueries();
            var alertDb = alertsQueries.SelectAlertDataByAlertId(alertsQueries.SelectLastAddedAlertId(positionId));
            Asserts.Batch(
               () =>
                    Assert.AreEqual(alertState, Parsing.ConvertToBool(alertDb.IsTriggered), "The alert in DB doesn't have expected value for column IsTriggered"),
               () =>
                    Assert.AreEqual(Parsing.ConvertToDouble(alertValue), Parsing.ConvertToDouble(alertDb.ThresholdValue), "The alert in DB doesn't have expected value for column TreshholdValue"),
               () =>
                    Assert.AreEqual((int)operationType, Parsing.ConvertToInt(alertDb.Operation), "The alert in DB doesn't have expected value for column Operation"),
               () =>
                    Assert.AreEqual((int)PriceType.Close, Parsing.ConvertToInt(alertDb.PriceType), "The alert in DB doesn't have expected value for column PriceType"),
               () =>
                    Assert.AreEqual(lastTriggered, 
                        alertState 
                            ? Parsing.ConvertToShortDateString(alertDb.LastTriggered) 
                            : alertDb.LastTriggered,
                        "The alert in DB doesn't have expected value for column LastTriggered"),
               () =>
                    Assert.AreEqual(isIntradayPriceRequired, Parsing.ConvertToBool(alertDb.UseIntraday),
                        "The alert in DB doesn't have expected value for column UseIntraday"),
               () =>
                    Assert.AreEqual((int)AlertTypes.PercentageGainLoss, Parsing.ConvertToInt(alertDb.TriggerTypeId), "The alert in DB doesn't have expected value for column TriggerTypeId")
            );
        }
    }
}