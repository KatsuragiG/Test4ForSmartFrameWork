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
using AutomatedTests.Models.AlertsModels;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.PositionCard;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._05_PositionCard
{
    [TestClass]
    public class TC_0469_PositionCard_Alerts_AddBreakoutAlert : BaseTestUnitTests
    {
        private const int TestNumber = 469;
        private const int QuantityOfAddedAlerts = 1;

        private BreakoutAlertModel alertModel = new BreakoutAlertModel();
        private DatePeriodTypes dayPeriod;
        private IntradayAlertTypes priceType;
        private BreakoutOptionTypes breakoutType;
        private int positionId;
        private bool alertState;
        private bool isIntraday;
        private string alertValue;
        private string positionCurrency;
        private string expectedFilledAlertDescription;

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

            dayPeriod = GetTestDataParsedAsEnumFromStringMapping<DatePeriodTypes>(nameof(dayPeriod));
            breakoutType = GetTestDataParsedAsEnumFromStringMapping<BreakoutOptionTypes>(nameof(breakoutType));
            priceType = GetTestDataParsedAsEnumFromStringMapping<IntradayAlertTypes>(nameof(priceType));
            alertValue = GetTestDataAsString(nameof(alertValue));
            alertState = GetTestDataAsBool(nameof(alertState));
            isIntraday = GetTestDataAsBool(nameof(isIntraday));
            var expectedAlertDescription = GetTestDataAsString("expectedAlertDescription");

            var entryPrice = GetTestDataAsString("EntryPrice");
            var entryDate = GetTestDataAsString("EntryDate");
            var positionModel = new PositionsDBModel
            {
                Symbol = GetTestDataAsString("Symbol"),
                TradeType = $"{(int)GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>("TradeType")}",
                PurchasePrice = string.IsNullOrEmpty(entryPrice)
                    ? null
                    : entryPrice,
                SplitsAdj = string.IsNullOrEmpty(entryPrice)
                    ? null
                    : entryPrice,
                PurchaseDate = string.IsNullOrEmpty(entryDate)
                    ? null
                    : entryDate,
                VendorHoldingId = GetTestDataAsBool("IsSynch")
                    ? StringUtility.RandomString("###")
                    : null,
                UseIntraday = isIntraday
                    ? Constants.DefaultAlertValue
                    : Constants.DefaultStringZeroIntValue
            };

            alertModel = new BreakoutAlertModel
            {
                BreakoutPeriod = breakoutType.GetStringMapping(),
                DatePeriod = dayPeriod.GetStringMapping(),
                ThresholdValue = alertValue
            };
            if (isIntraday)
            {
                alertModel.PriceType = priceType.GetStringMapping();
            }

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));
            var portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            positionId = PositionsAlertsSetUp.AddPositionViaDB(portfolioId, positionModel);

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
            new PositionCardSteps().ResavePositionCard(positionId);
            Browser.Refresh();

            positionCurrency = Constants.AllCurrenciesRegex.Match(new PositionCardForm().GetLatestClose()).Value;
            expectedFilledAlertDescription = string.Format(expectedAlertDescription, alertValue,
                dayPeriod.GetStringMapping(), breakoutType.GetStringMapping().ToLowerInvariant());
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_469$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test checks correct adding of Breakout Alert from Position Card page. https://tr.a1qa.com/index.php?/cases/view/19232635")]
        [TestCategory("PositionCard"), TestCategory("Alerts"), TestCategory("AlertAdd"), TestCategory("PriceAlertsGroup")]
        public override void RunTest()
        {
            LogStep(1, 2, "Click on position and open New Position Card. Click 'Add Alert' button and than click on Price tab in list.");
            new AlertTabPositionCardSteps().GetAddAlertsFormByAlertCategory(positionId, AlertCategoryTypes.Price);

            LogStep(3, "Make sure that values be default are: 1 Month High.");
            var breakoutAlertForm = new BreakoutAlertForm();
            Checker.CheckEquals(Constants.DefaultAlertValue, breakoutAlertForm.GetBreakoutAlertPeriodValue(),
                $"Default Breakout Alert Value is not {Constants.DefaultAlertValue}");
            Checker.CheckEquals(DatePeriodTypes.Month.ToString(), breakoutAlertForm.GetBreakoutAlertPeriodType(),
                $"Default Breakout Alert Date Period is not {DatePeriodTypes.Month}");
            Checker.CheckEquals(BreakoutOptionTypes.High.ToString(), breakoutAlertForm.GetBreakoutAlertPeriodOption(),
                $"Default Breakout Alert Period is not {BreakoutOptionTypes.High}");
            var actualPriceType = isIntraday
                ? IntradayAlertTypes.LatestPrice.GetStringMapping()
                : IntradayAlertTypes.LatestClose.GetStringMapping();
            Checker.CheckEquals(actualPriceType, breakoutAlertForm.GetBreakoutAlertPriceType(), $"Breakout Price Type is not {actualPriceType}");

            LogStep(4, "Fill alert data");
            breakoutAlertForm.FillBreakoutAlertFields(alertModel);
            Checker.CheckEquals(alertModel.ThresholdValue, breakoutAlertForm.GetBreakoutAlertPeriodValue(),
                "Changed Breakout Alert Value is not as expected");
            Checker.CheckEquals(alertModel.DatePeriod, breakoutAlertForm.GetBreakoutAlertPeriodType(),
                "Changed Breakout Alert Date Period is not as expected");
            Checker.CheckEquals(alertModel.BreakoutPeriod, breakoutAlertForm.GetBreakoutAlertPeriodOption(),
                "Changed Breakout Alert Period is not as expected");
            if (isIntraday)
            {
                Checker.CheckEquals(alertModel.PriceType, breakoutAlertForm.GetBreakoutAlertPriceType(),
                    "Changed Breakout Alert Period is not as expected");
            }

            LogStep(5, "Click on 'Add Alert' button.");
            new AddAlertsPositionCardForm().ClickOnAddAlertButton(AlertTypes.Breakout.GetStringMapping());
            var alertsTabPositionCardForm = new AlertsTabPositionCardForm();
            Assert.IsTrue(alertsTabPositionCardForm.IsAlertPresent(expectedFilledAlertDescription), $"Alert '{expectedFilledAlertDescription}' is not present on Position Card page");

            LogStep(6, "Click sign Pencil (Edit) sign for the alert and make sure that parameters are shown correctly.");
            var addedBreakoutAlertForm = new AlertsTabPositionCardForm().EditAlertGetAlertForm<BreakoutAlertForm>(expectedFilledAlertDescription);
            Checker.CheckEquals(alertModel.ThresholdValue, addedBreakoutAlertForm.GetBreakoutAlertPeriodValue(),
                "Saved Breakout Alert Value is not as expected");
            Checker.CheckEquals(alertModel.DatePeriod, addedBreakoutAlertForm.GetBreakoutAlertPeriodType(),
                "Saved Breakout Alert Date Period is not as expected");
            Checker.CheckEquals(alertModel.BreakoutPeriod, addedBreakoutAlertForm.GetBreakoutAlertPeriodOption(),
                "Saved Breakout Alert Period is not as expected");

            LogStep(7, "Click 'Cancel' button.");
            alertsTabPositionCardForm.ClickCancelEditAlertByNumberOfAlert(QuantityOfAddedAlerts);

            LogStep(8, "Make sure that color of Bell icon is purple.");
            Checker.CheckEquals(alertState, alertsTabPositionCardForm.IsAlertTriggeredHasPurpleBellIcon(expectedFilledAlertDescription), "Color of Bell icon is not as expected");
            var lastTriggered = alertState
                ? Parsing.ConvertToShortDateString(Constants.DateFormatRegex.Match(alertsTabPositionCardForm.GetAlertHint(expectedFilledAlertDescription)).ToString())
                : null;

            LogStep(9, "Check that the alert in DB have corresponded changeable settings and correct trigger type. Make sure correspondences between trigger type and color of 'Bell' icon.");
            var alertsQueries = new AlertsQueries();
            var alertDb = alertsQueries.SelectAlertDataByAlertId(alertsQueries.SelectLastAddedAlertId(positionId));
            Asserts.Batch(
                () =>
                    Assert.AreEqual(alertState, Parsing.ConvertToBool(alertDb.IsTriggered), "The alert in DB doesn't have expected value for column IsTriggered"),
                () =>
                    Assert.AreEqual(positionCurrency, alertDb.Currency, "The alert in DB doesn't have expected value for column Currency"),
                () =>
                    Assert.AreEqual((int)breakoutType, Parsing.ConvertToInt(alertDb.Operation), "The alert in DB doesn't have expected value for column Operation"),
                () =>
                    Assert.AreEqual(Parsing.ConvertToDouble(alertValue), Parsing.ConvertToDouble(alertDb.ThresholdValue),
                        "The alert in DB doesn't have expected value for column TreshholdValue"),
                () =>
                    Assert.AreEqual((int)PriceType.Close, Parsing.ConvertToInt(alertDb.PriceType), "The alert in DB doesn't have expected value for column PriceType"),
                () =>
                    Assert.AreEqual((int)dayPeriod, Parsing.ConvertToInt(alertDb.PeriodType), "The alert in DB doesn't have expected value for column PeriodType"),
                () =>
                    Assert.AreEqual(isIntraday && priceType == IntradayAlertTypes.LatestPrice, Parsing.ConvertToBool(alertDb.UseIntraday),
                        "The alert in DB doesn't have expected value for column UseIntraday"),
                () =>
                    Assert.AreEqual(lastTriggered, 
                        alertState 
                            ? Parsing.ConvertToShortDateString(alertDb.LastTriggered) 
                            : alertDb.LastTriggered,
                        "The alert in DB doesn't have expected value for column LastTriggered"),
                () =>
                    Assert.AreEqual((int)TriggerTypes.Breakout, Parsing.ConvertToInt(alertDb.TriggerTypeId), "The alert in DB doesn't have expected value for column TriggerTypeId")
            );
        }
    }
}