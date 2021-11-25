using System;
using System.Collections.Generic;
using System.Linq;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Alerts;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Alerts;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms.MyPortfolios;
using AutomatedTests.Forms.Popups;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Models.AlertsModels;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.PreconditionsSteps;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._04_PositionsAndAlerts._01_Positions
{
    [TestClass]
    public class TC_1033_PositionsGrid_AddAlert_ApplyCustomTemplateWithAllAlertsForStockPositionWithSsiAndFundamentalsData : BaseTestUnitTests
    {
        private const int TestNumber = 1033;

        private PortfolioModel portfolioModel;
        private readonly List<PositionsDBModel> positionsModels = new List<PositionsDBModel>();
        private readonly List<int> positionsIds = new List<int>();
        private string templateName;
        private int countOfNotAddedAlerts;
        private int countOfAddedAlerts;
        private AddedAndNotAddedAlertsModel addedAndNotAddedAlerts = new AddedAndNotAddedAlertsModel();

        [TestInitialize]
        public void TestInitialize()
        {
            portfolioModel = new PortfolioModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency")
            };
            positionsModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("Symbol1"),
                TradeType = $"{(int)PositionTradeTypes.Long}",
                PurchaseDate = DateTime.Now.ToShortDateString()
            });
            positionsModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("Symbol2"),
                TradeType = $"{(int)PositionTradeTypes.Short}",
                PurchaseDate = DateTime.Now.ToShortDateString(),
                VendorHoldingId = StringUtility.RandomString("###")
            });
            templateName = StringUtility.RandomString("Template######");
            countOfNotAddedAlerts = GetTestDataAsInt("CountOfNotAddedAlerts");
            countOfAddedAlerts = GetTestDataAsInt("CountOfAddedAlerts");
            new ReadTestDataFromDataSourceSteps().GetAddedShortAddedLongNotAddedAlertsListFromDataSourceByColumnPatternsAlertsQuantities(countOfNotAddedAlerts,
                "NotAddedAlerts", countOfAddedAlerts, "AddedAlerts", TestContext, ref addedAndNotAddedAlerts);
            addedAndNotAddedAlerts.AddedShortAlertsIds = new List<int> {
                (int)AlertTypes.PercentOfAverageVolume, (int)AlertTypes.AboveBelowMovingAverage, (int)AlertTypes.MovingAverageCrosses,
                (int)AlertTypes.CalendarDaysAfterEntry, (int)AlertTypes.TradingDaysAfterEntry, (int)AlertTypes.ProfitableClosesAfterEntry,
                (int)AlertTypes.SpecificDate, (int)AlertTypes.PercentageTrailingStop, (int)AlertTypes.PercentageGainLoss,
                (int)AlertTypes.DollarGainLoss, (int)AlertTypes.FixedPriceAboveBelow, (int)AlertTypes.Breakout, (int)AlertTypes.Target,
                (int)AlertTypes.Target, (int)AlertTypes.Target, (int)AlertTypes.Target, (int)AlertTypes.Target, (int)AlertTypes.Target,
                (int)AlertTypes.Target, (int)AlertTypes.VqTrailingStop};
            addedAndNotAddedAlerts.AddedLongAlertsIds = addedAndNotAddedAlerts.AddedShortAlertsIds.Concat(new List<int> { (int)AlertTypes.TwoVq }).ToList();

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPremium));
            var portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            foreach (var positionModel in positionsModels)
            {
                positionsIds.Add(PositionsAlertsSetUp.AddPositionViaDB(portfolioId, positionModel));
            }

            LoginSetUp.LogIn(UserModels.First());
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1033$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test checks correctness adding of Template with all alerts to a position with SSI and with fundamental data. https://tr.a1qa.com/index.php?/cases/view/19232068")]
        [TestCategory("Smoke"), TestCategory("PositionsGrid"), TestCategory("PositionsGridBulkActionButtons"), TestCategory("AlertAdd"), TestCategory("AlertTemplatesGroup")]
        public override void RunTest()
        {
            LogStep(1, "Select positions of precondition #3 -> click 'Add Alert' button -> Select the template(precondition #4) -> Click 'Add'");
            new PreconditionsCommonSteps().AddTemplateWithAllTypesOfAlertsToAllPositionsForNonBasic(templateName);

            LogStep(2, "Add Alert.Apply an Alert Template.Select custom template with all alerts.Click 'Apply' button.");
            var pleaseNote = new ConfirmPopup(PopupNames.Warning);
            var notAddedAlertsInPopup = pleaseNote.GetAllAlertsNamesFromPopup();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(notAddedAlertsInPopup, addedAndNotAddedAlerts.NotAddedShortAlerts),
                "Alerts in 'Please note' popup do not equal the expected\n" +
                $"{GetExpectedResultsString(addedAndNotAddedAlerts.NotAddedShortAlerts)}\r\n{GetActualResultsString(notAddedAlertsInPopup)}");

            LogStep(3, "Click OK");
            pleaseNote.ClickOkButton();

            LogStep(4, 5, "Open Alerts grid.Make sure expected alerts was not added to the both position. Make sure alerts are added to the both positions");
            new MyPortfoliosMenuForm().ClickMyPortfoliosMenuItem(MyPortfoliosMenuItems.AlertsGrid);
            var alertsTabForm = new AlertsTabForm();
            var alertsDescriptionsForLongPositions = alertsTabForm.GetColumnValuesByPositionId(AlertsGridColumnsDataField.AlertDescription, positionsIds[0]);
            var alertsDescriptionsForShortPositions = alertsTabForm.GetColumnValuesByPositionId(AlertsGridColumnsDataField.AlertDescription, positionsIds[1]);
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(alertsDescriptionsForLongPositions, addedAndNotAddedAlerts.AddedLongAlerts),
                $"Not all expected alerts added to the long position {positionsModels[0].Symbol}\n" +
                $"{GetExpectedResultsString(addedAndNotAddedAlerts.AddedLongAlerts)}\r\n{GetActualResultsString(alertsDescriptionsForLongPositions)}");
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(alertsDescriptionsForShortPositions, addedAndNotAddedAlerts.AddedShortAlerts),
                $"Not all expected alerts added to the short position {positionsModels[1].Symbol}\n" +
                $"{GetExpectedResultsString(addedAndNotAddedAlerts.AddedShortAlerts)}\r\n{GetActualResultsString(alertsDescriptionsForShortPositions)}");

            LogStep(6, 8, "In DB: make sure expected alerts added to the position 1 and 2");
            var alertsIdsLongPosition = new AlertsQueries().SelectAllAlertsIdUsingPositionId(positionsIds[0]);
            var alertsIdsShortPosition = new AlertsQueries().SelectAllAlertsIdUsingPositionId(positionsIds[1]);
            Assert.IsTrue(alertsIdsLongPosition.Count > 0, $"There are no alerts for position {positionsModels[0].Symbol} in DB");
            Assert.IsTrue(alertsIdsShortPosition.Count > 0, $"There are no alerts for position {positionsModels[1].Symbol} in DB");
            var alertsTypesLongPosition = alertsIdsLongPosition.Select(t => Parsing.ConvertToInt(new AlertsQueries().SelectAlertDataByAlertId(t).TriggerTypeId)).ToList();
            var alertsTypesShortPosition = alertsIdsShortPosition.Select(t => Parsing.ConvertToInt(new AlertsQueries().SelectAlertDataByAlertId(t).TriggerTypeId)).ToList();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(addedAndNotAddedAlerts.AddedLongAlertsIds, alertsTypesLongPosition),
                $"Expected alerts were added to position {positionsModels[0].Symbol} in DB\n" +
                $"{GetExpectedResultsString(addedAndNotAddedAlerts.AddedLongAlertsIds)}\r\n{GetActualResultsString(alertsDescriptionsForLongPositions)}");
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(addedAndNotAddedAlerts.AddedShortAlertsIds, alertsTypesShortPosition),
                $"Expected alerts were added to position {positionsModels[1].Symbol} in DB\n" +
                $"{GetExpectedResultsString(addedAndNotAddedAlerts.AddedShortAlertsIds)}\r\n{GetActualResultsString(alertsDescriptionsForShortPositions)}");
        }
    }
}