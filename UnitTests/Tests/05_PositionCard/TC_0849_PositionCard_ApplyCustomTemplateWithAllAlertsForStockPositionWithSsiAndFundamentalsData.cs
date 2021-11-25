using System;
using System.Collections.Generic;
using System.Linq;
using AutomatedTests.Database.Alerts;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Alerts;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Models.AlertsModels;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.PositionCard;
using AutomatedTests.Steps.PreconditionsSteps;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;


namespace UnitTests.Tests._05_PositionCard
{
    [TestClass]
    public class TC_0849_PositionCard_ApplyCustomTemplateWithAllAlertsForStockPositionWithSsiAndFundamentalsData : BaseTestUnitTests
    {
        private const int TestNumber = 849;

        private string templateName;
        private AddedAndNotAddedAlertsModel addedAndNotAddedAlerts = new AddedAndNotAddedAlertsModel();
        private List<int> positionsIds;
        
        [TestInitialize]
        public void TestInitialize()
        {
            var portfolioModel = new PortfolioModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency")
            };
            var positionModelLong = new PositionsDBModel
            {
                Symbol = GetTestDataAsString("Symbol1"),
                TradeType = $"{(int)PositionTradeTypes.Long}",
                PurchaseDate = DateTime.Now.ToShortDateString()
            };
            var positionModelShort = new PositionsDBModel
            {
                Symbol = GetTestDataAsString("Symbol2"),
                TradeType = $"{(int)PositionTradeTypes.Short}",
                PurchaseDate = DateTime.Now.ToShortDateString(),
                VendorHoldingId = StringUtility.RandomString("###")
            };
            templateName = StringUtility.RandomString("Template######");
            var countOfNotAddedAlerts = GetTestDataAsInt("CountOfNotAddedAlerts");
            var countOfAddedAlerts = GetTestDataAsInt("CountOfAddedAlerts");

            new ReadTestDataFromDataSourceSteps().GetAddedNotAddedAlertsForLongShortFromDataSourceByColumnPatternsAlertsQuantities(
                countOfNotAddedAlerts, "NotAddedAlerts", countOfAddedAlerts, "AddedAlerts", TestContext, ref addedAndNotAddedAlerts);
            addedAndNotAddedAlerts.AddedShortAlertsIds = new List<int> 
            { 
                (int)AlertTypes.PercentOfAverageVolume, (int)AlertTypes.AboveBelowMovingAverage, (int)AlertTypes.MovingAverageCrosses, 
                (int)AlertTypes.CalendarDaysAfterEntry, (int)AlertTypes.TradingDaysAfterEntry, (int)AlertTypes.ProfitableClosesAfterEntry, 
                (int)AlertTypes.SpecificDate, (int)AlertTypes.PercentageTrailingStop, (int)AlertTypes.PercentageGainLoss, (int)AlertTypes.DollarGainLoss, 
                (int)AlertTypes.FixedPriceAboveBelow, (int)AlertTypes.Breakout, (int)AlertTypes.Target, (int)AlertTypes.Target, (int)AlertTypes.Target, 
                (int)AlertTypes.Target, (int)AlertTypes.Target, (int)AlertTypes.Target, (int)AlertTypes.Target, (int)AlertTypes.VqTrailingStop };
            addedAndNotAddedAlerts.AddedLongAlertsIds = addedAndNotAddedAlerts.AddedShortAlertsIds.Concat(new List<int> { (int)AlertTypes.TwoVq }).ToList();

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPremium));
            var portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            positionsIds = new List<int>
            {
                PositionsAlertsSetUp.AddPositionViaDB(portfolioId, positionModelLong),
                PositionsAlertsSetUp.AddPositionViaDB(portfolioId, positionModelShort)
            };

            LoginSetUp.LogIn(UserModels.First());

            new TemplateSetUps().CreateTemplateWithAllTypesOfAlertsWithDefaultSettings(templateName);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_849$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test checks correctness adding of Template with all alerts to a position with SSI and with fundamental data. https://tr.a1qa.com/index.php?/cases/view/19232268")]
        [TestCategory("Smoke"), TestCategory("PositionCard"), TestCategory("PositionCardAlertsTab"), TestCategory("Alerts"), TestCategory("AlertAdd"), TestCategory("AlertTemplatesGroup")]
        public override void RunTest()
        {
            foreach (var positionId in positionsIds)
            {
                LogStep(1, 5, "Open Position Card -> Validate Added Via Template Alerts");
                var positionType = Parsing.ConvertToInt(new PositionsQueries().SelectAllPositionData(positionId).TradeType);
                new AlertTabPositionCardSteps().ValidateAddedViaTemplateAlerts(templateName, addedAndNotAddedAlerts.NotAddedLongAlerts, addedAndNotAddedAlerts.NotAddedShortAlerts,
                     addedAndNotAddedAlerts.AddedLongAlerts, addedAndNotAddedAlerts.AddedShortAlerts, positionId);

                LogStep(6, 7, "In DB: make sure expected alerts added to the position. In DB: make sure expected alerts are NOT added to the position");
                var alertsIds = new AlertsQueries().SelectAllAlertsIdUsingPositionId(positionId);
                Assert.IsTrue(alertsIds.Count > 0, "There are no alerts for position in DB");
                var alertsTypes = alertsIds.Select(t => Parsing.ConvertToInt(new AlertsQueries().SelectAlertDataByAlertId(t).TriggerTypeId)).ToList();
                var addedAlertsDb = positionType == (int)PositionTradeTypes.Long ? addedAndNotAddedAlerts.AddedLongAlertsIds : addedAndNotAddedAlerts.AddedShortAlertsIds;
                Checker.IsTrue(ListsComparator.AreTwoListsEqualsInOrder(addedAlertsDb, alertsTypes), 
                    $"Expected alerts were added to position in DB for {new PositionsQueries().SelectSymbolByPositionId(positionId)} \n" +
                     $"{GetExpectedResultsString(addedAlertsDb)}\r\n{GetActualResultsString(alertsTypes)}");
            }
        }
    }
}