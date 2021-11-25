using System.Collections.Generic;
using System.Linq;
using AutomatedTests.Database.Alerts;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Alerts;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Forms.Popups;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.PreconditionsSteps;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.BaseTestsUnitTests;
using AutomatedTests.Forms.Portfolios;
using System;
using AutomatedTests.Enums.Positions;
using TradeStops.Common.Enums;
using AutomatedTests.Models.AlertsModels;

namespace UnitTests.Tests._04_PositionsAndAlerts._04_AddPositionSimpleMode
{
    [TestClass]
    public class TC_1082_AddPositionSimple_ApplyTmplWithAllAlertsForStockWithSSIAndFundData : BaseTestUnitTests
    {
        private const int TestNumber = 1082;

        private PositionAtManualCreatingPortfolioModel positionModel;
        private string templateName;
        private AddedAndNotAddedAlertsModel addedAndNotAddedAlerts = new AddedAndNotAddedAlertsModel();
        private int portfolioId;

        [TestInitialize]
        public void TestInitialize()
        {
            var portfolioModel = new PortfolioModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency")
            };
            positionModel = new PositionAtManualCreatingPortfolioModel
            {
                Ticker = GetTestDataAsString("Symbol"),
                TradeType = GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>("TradeType"),
                PositionAssetType = PositionAssetTypes.Stock,
                EntryDate = DateTimeProvider.GetDate(DateTime.Now, 0, 0, -1).AsShortDate(),
                Quantity = GetTestDataAsString("Shares")
            };
            templateName = StringUtility.RandomString("Template######");
            var countOfNotAddedAlerts = GetTestDataAsInt("CountOfNotAddedAlerts");
            var countOfAddedAlerts = GetTestDataAsInt("CountOfAddedAlerts");
            new ReadTestDataFromDataSourceSteps().GetAddedNotAddedAlertsForLongShortFromDataSourceByColumnPatternsAlertsQuantities(countOfNotAddedAlerts,
                "NotAddedAlerts", countOfAddedAlerts, "AddedAlerts", TestContext, ref addedAndNotAddedAlerts);
            addedAndNotAddedAlerts.AddedShortAlertsIds = new List<int>
            {
                (int)AlertTypes.PercentOfAverageVolume, (int)AlertTypes.AboveBelowMovingAverage, (int)AlertTypes.MovingAverageCrosses,
                (int)AlertTypes.CalendarDaysAfterEntry, (int)AlertTypes.TradingDaysAfterEntry, (int)AlertTypes.ProfitableClosesAfterEntry,
                (int)AlertTypes.SpecificDate, (int)AlertTypes.PercentageTrailingStop, (int)AlertTypes.PercentageGainLoss,
                (int)AlertTypes.DollarGainLoss, (int)AlertTypes.FixedPriceAboveBelow, (int)AlertTypes.Breakout,
                (int)AlertTypes.Target, (int)AlertTypes.Target, (int)AlertTypes.Target, (int)AlertTypes.Target,
                (int)AlertTypes.Target, (int)AlertTypes.Target, (int)AlertTypes.Target, (int)AlertTypes.VqTrailingStop
            };
            addedAndNotAddedAlerts.AddedLongAlertsIds = addedAndNotAddedAlerts.AddedShortAlertsIds.Concat(new List<int> { (int)AlertTypes.TwoVq }).ToList();

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPremium));
            portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);

            LoginSetUp.LogIn(UserModels.First());

            new TemplateSetUps().CreateTemplateWithAllTypesOfAlertsWithDefaultSettings(templateName);

            new MainMenuNavigation().OpenPositionsGrid();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1082$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test checks correctness adding of Template with all alerts to a position with SSI and with fundamental data. https://tr.a1qa.com/index.php?/cases/view/19231988")]
        [TestCategory("Smoke"), TestCategory("PositionsGrid"), TestCategory("AddPositionPopup"), TestCategory("Alerts"), TestCategory("AlertAdd"), TestCategory("AlertTemplatesGroup")]
        public override void RunTest()
        {
            LogStep(1, "Click 'Add Position'.Fill in:Symbol;Entry Date: by default;Entry Price(per unit): by default;Shares: any positive value;Trade Type;" +
                "Template: select template with all alerts ");
            new PositionsTabForm().ClickAddPositionButton();
            var addPositionInFrameForm = new AddPositionInFrameForm();
            addPositionInFrameForm.FillPositionFields(positionModel, 1);
            addPositionInFrameForm.SelectTemplate(templateName);

            LogStep(2, "Click 'Save and Close' button.");
            addPositionInFrameForm.ClickSaveAndClose();
            var warningPopup = new ConfirmPopup(PopupNames.Warning);
            var notAddedAlertsInPopup = warningPopup.GetAllAlertsNamesFromPopup();
            var positionId = new PositionsQueries().SelectLastAddedPositionId(portfolioId);
            var positionType = Parsing.ConvertToInt(new PositionsQueries().SelectAllPositionData(positionId).TradeType);
            var notAddedAlerts = positionType == (int)PositionTradeTypes.Long ? addedAndNotAddedAlerts.NotAddedLongAlerts : addedAndNotAddedAlerts.NotAddedShortAlerts;
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(notAddedAlertsInPopup, notAddedAlerts),
                $"Alerts in 'Please note' popup do not equal the expected for {positionModel.Ticker} \n" +
                $"{GetExpectedResultsString(notAddedAlerts)}\r\n{GetActualResultsString(notAddedAlertsInPopup)}");

            LogStep(3, "Click OK");
            warningPopup.ClickOkButton();

            LogStep(4, 5, "Open Alerts grid.Make sure expected alerts was not added to the both position. Make sure alerts are added to the both positions");
            new MainMenuNavigation().OpenAlertsGrid();
            var alertsTabForm = new AlertsTabForm();
            var alertsForPosition = alertsTabForm.GetColumnValuesByPositionId(AlertsGridColumnsDataField.AlertDescription, positionId);
            var addedAlerts = positionType == (int)PositionTradeTypes.Long ? addedAndNotAddedAlerts.AddedLongAlerts : addedAndNotAddedAlerts.AddedShortAlerts;
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(alertsForPosition, addedAlerts),
                $"Not all expected alerts added to the position {positionModel.Ticker}\n" +
                $"{GetExpectedResultsString(addedAlerts)}\r\n{GetActualResultsString(alertsForPosition)}");

            LogStep(6, 7, "In DB: make sure expected alerts added to the position 1 and 2");
            var alertsIdsPosition = new AlertsQueries().SelectAllAlertsIdUsingPositionId(positionId);
            Assert.IsTrue(alertsIdsPosition.Count > 0, $"There are no alerts for position {positionModel.Ticker} in DB");
            var alertsTypesPosition = alertsIdsPosition.Select(t => Parsing.ConvertToInt(new AlertsQueries().SelectAlertDataByAlertId(t).TriggerTypeId)).ToList();
            var addedAlertsDb = positionType == (int)PositionTradeTypes.Long ? addedAndNotAddedAlerts.AddedLongAlertsIds : addedAndNotAddedAlerts.AddedShortAlertsIds;
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(addedAlertsDb, alertsTypesPosition),
                $"Expected alerts were added to position {positionModel.Ticker} in DB\n" +
                $"{GetExpectedResultsString(addedAlertsDb)}\r\n{GetActualResultsString(alertsForPosition)}");
        }
    }
}