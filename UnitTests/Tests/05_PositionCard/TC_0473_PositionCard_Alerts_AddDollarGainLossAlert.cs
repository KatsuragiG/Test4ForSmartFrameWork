using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Alerts;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Alerts;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Alerts;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Models.AlertsModels;
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
    public class TC_0473_PositionCard_Alerts_AddDollarGainLossAlert : BaseTestUnitTests
    {
        private const int TestNumber = 473;
        private const int QuantityOfAddedAlerts = 1;

        private DollarGainLossAlertModel alertModel = new DollarGainLossAlertModel();
        private GainLossOperationType operationType;
        private bool alertState;
        private bool isNotificationSettingsShown;
        private bool? isIntraday;
        private bool isIntradayPriceRequired;
        private string positionCurrency;
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
                Shares = GetTestDataAsString("Shares"),
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

            alertModel = new DollarGainLossAlertModel
            {
                OperationType = operationType.GetStringMapping(),
                IsIntraday = isIntraday,
                ThresholdValue = alertValue
            };

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));
            var portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            positionId = PositionsAlertsSetUp.AddPositionViaDB(portfolioId, positionModel);

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
            new PositionCardSteps().ResavePositionCard(positionId);
            Browser.Refresh();

            positionCurrency = Constants.AllCurrenciesRegex.Match(new PositionCardForm().GetLatestClose()).Value;
            expectedFilledAlertDescription =
                string.Format(expectedAlertDescription, operationType.GetStringMapping(), positionCurrency, alertValue.ToFractionalString(),
                isIntradayPriceRequired
                    ? intradayDescription 
                    : string.Empty).TrimEnd();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_473$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test checks correct adding of Dollar Gain/Loss alerts from Position Card page. https://tr.a1qa.com/index.php?/cases/view/22465063")]
        [TestCategory("PositionCard"), TestCategory("Alerts"), TestCategory("AlertAdd"), TestCategory("PriceAlertsGroup")]
        public override void RunTest()
        {
            LogStep(1, 2, "Click on Alert tab in Position Card. Click 'Add Alert' button and than click on Price tab in list.");
            new AlertTabPositionCardSteps().GetAddAlertsFormByAlertCategory(positionId, AlertCategoryTypes.Price);

            LogStep(3, "Click on drop-down list in 'Dollar Gain/Loss' alert and choose operation type according to test data: operationType'");
            var dollarGainLossAlertForm = new DollarGainLossAlertForm();
            Checker.CheckEquals(GainLossOperationType.Gain.GetStringMapping(), dollarGainLossAlertForm.GetDollareGainLossOperationType(),
                "Default operation type is not as expected");
            Checker.CheckEquals(Constants.DefaultAlertValue, dollarGainLossAlertForm.GetDollarGainLossValue(),
                "Default Gain Loss Value is not as expected");
            Checker.CheckEquals(isNotificationSettingsShown, dollarGainLossAlertForm.IsNotificationSettingsShown(AlertTypes.DollarGainLoss),
                "Default Gain Loss notification settings is not as expected");
            dollarGainLossAlertForm.ScrollToTheAlertWithFillingDollarGainLossFields(alertModel);
            Checker.CheckEquals(operationType.GetStringMapping(), dollarGainLossAlertForm.GetDollareGainLossOperationType(),
                "Expected operation type is not as set");
            Checker.CheckEquals(alertValue, dollarGainLossAlertForm.GetDollarGainLossValue(),
                "Expected Gain Loss Value is not as expected");
            if (isNotificationSettingsShown && isIntraday.HasValue)
            {
                Checker.CheckEquals((bool)isIntraday, dollarGainLossAlertForm.IsLatestPriceIntradayEnabled(AlertTypes.DollarGainLoss),
                    "Expected Intraday state is not as expected");
            }

            LogStep(4, "Click on 'Add Alert' button.");
            new AddAlertsPositionCardForm().ClickOnAddAlertButton(AlertTypes.DollarGainLoss.GetStringMapping());
            var alertsTabPositionCardForm = new AlertsTabPositionCardForm();
            Assert.IsTrue(alertsTabPositionCardForm.IsAlertPresent(expectedFilledAlertDescription),
                $"Alert '{expectedFilledAlertDescription}' is not present on Position Card page");

            LogStep(5, "Click sign Pencil (Edit) sign for the alert and make sure that parameters are shown correctly.");
            var addedDollarGainLossAlertForm = alertsTabPositionCardForm.EditAlertGetAlertForm<DollarGainLossAlertForm>(expectedFilledAlertDescription);
            Asserts.Batch(
                () =>
                    Assert.AreEqual(operationType.GetStringMapping(), addedDollarGainLossAlertForm.GetDollareGainLossOperationType(),
                        $"Dollar Gain Loss Operation Type is not {operationType}"),
                () =>
                    Assert.AreEqual(alertValue, addedDollarGainLossAlertForm.GetDollarGainLossValue(),
                        $"Dollar Gain Loss Value is not {alertValue}"),
                () =>
                    Assert.AreEqual(positionCurrency, addedDollarGainLossAlertForm.GetCurrencySign(AlertTypes.DollarGainLoss),
                        $"Dollar Gain Loss Currency Sign is not {positionCurrency}")
            );
            if (isNotificationSettingsShown && isIntraday.HasValue)
            {
                Checker.CheckEquals((bool)isIntraday, addedDollarGainLossAlertForm.IsLatestPriceIntradayEnabled(AlertTypes.DollarGainLoss),
                    "Intraday state is not as expected");
            }

            LogStep(6, "Click 'Cancel' button.");
            alertsTabPositionCardForm.ClickCancelEditAlertByNumberOfAlert(QuantityOfAddedAlerts);

            LogStep(7, "Make sure that color of Bell icon is purple.");
            Checker.CheckEquals(alertState, alertsTabPositionCardForm.IsAlertTriggeredHasPurpleBellIcon(expectedFilledAlertDescription), "Color of Bell icon is not as expected");
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
                    Assert.AreEqual(Parsing.ConvertToDouble(alertValue), Parsing.ConvertToDouble(alertDb.ThresholdValue),
                        "The alert in DB doesn't have expected value for column TreshholdValue"),
                () =>
                    Assert.AreEqual(positionCurrency, alertDb.Currency, "The alert in DB doesn't have expected value for column Currency"),
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
                    Assert.AreEqual((int)AlertTypes.DollarGainLoss, Parsing.ConvertToInt(alertDb.TriggerTypeId),
                        "The alert in DB doesn't have expected value for column TriggerTypeId")
            );
        }
    }
}