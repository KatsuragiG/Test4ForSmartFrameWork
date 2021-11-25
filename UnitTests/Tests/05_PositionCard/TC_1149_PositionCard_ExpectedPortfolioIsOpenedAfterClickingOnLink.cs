using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._05_PositionCard
{
    [TestClass]
    public class TC_1149_PositionCard_ExpectedPortfolioIsOpenedAfterClickingOnLink : BaseTestUnitTests
    {
        private const int TestNumber = 1149;

        private readonly List<int> positionsIds = new List<int>();

        [TestInitialize]
        public void TestInitialize()
        {
            var portfolioQuantity = GetTestDataAsInt("portfolioQuantity");
            var portfoliosModels = new List<PortfolioModel>();
            var currency = GetTestDataAsString("Currency");
            for (int i = 1; i <= portfolioQuantity; i++)
            {
                portfoliosModels.Add(new PortfolioModel
                {
                    Name = GetTestDataAsString($"PortfolioName{i}"),
                    Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>($"PortfolioType{i}"),
                    Currency = currency
                });
            }
            var positionsQuantity = GetTestDataAsInt("positionsQuantity");
            var positionsModels = new List<PositionsDBModel>();
            for (int i = 1; i <= positionsQuantity; i++)
            {
                var currentSymbol = GetTestDataAsString($"Symbol{i}");
                var currentTradeType = GetTestDataAsString($"tradeType{i}");
                positionsModels.Add(new PositionsDBModel
                {
                    Symbol = currentSymbol,
                    TradeType = currentTradeType,
                    PurchaseDate = DateTime.Now.AddDays(-5).ToShortDateString(),
                    StatusType = $"{(int)AutotestPositionStatusTypes.Close}",
                    CloseDate = DateTime.Now.ToShortDateString()
                });
                positionsModels.Add(new PositionsDBModel
                {
                    Symbol = currentSymbol,
                    TradeType = currentTradeType,
                    PurchaseDate = DateTime.Now.AddYears(-1).ToShortDateString()
                });
            }

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, new List<ProductSubscriptions> 
            { 
                ProductSubscriptions.TradeStopsPremium, ProductSubscriptions.CryptoStopsPremium 
            }));
            var portfoliosIds = new List<int>();
            foreach (var portfolioModel in portfoliosModels)
            {
                portfoliosIds.Add(PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel));
            }
            for (int i = 0; i < positionsModels.Count; i++)
            {
                positionsIds.Add(PositionsAlertsSetUp.AddPositionViaDB(portfoliosIds[i % 2], positionsModels[i]));
            }
            var positionsQueries = new PositionsQueries();
            portfoliosIds.Add(PortfoliosSetUp.ImportSynchronizedPortfolio01ViaDb(UserModels.First()));
            positionsIds.AddRange(positionsQueries.SelectPositionIdsForPortfolio(portfoliosIds.Last()));

            LoginSetUp.LogIn(UserModels.First());

            PortfoliosSetUp.DuplicatePortfolioAsWatchOnly(portfoliosIds[2]);
            portfoliosIds.Add(new PortfoliosQueries().SelectPortfolioIdForLastPortfolioByUserEmail(UserModels.First().Email));
            positionsIds.AddRange(positionsQueries.SelectPositionIdsForPortfolio(portfoliosIds.Last()));
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1149$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test checks redirection to Positions & Alerts -> Positions grid with selection expected portfolio https://tr.a1qa.com/index.php?/cases/view/19232672")]
        [TestCategory("Smoke"), TestCategory("PositionCard"), TestCategory("PositionCardChevrons"), TestCategory("ClosedPositionCard")]
        public override void RunTest()
        {
            LogStep(1, 3, "Repeat 1-2 for all positions");
            var positionsQueries = new PositionsQueries();
            foreach (var positionId in positionsIds)
            {
                CheckPortfolio(positionId, positionsQueries.SelectPortfolioNameByPositionId(positionId));
            }
        }

        private void CheckPortfolio(int positionId, string portfolioName)
        {
            LogStep(1, "Open Closed Position Card -> Position Details  via direct link for position");
            new MainMenuNavigation().OpenPositionCard(positionId);
            new PositionCardForm().ClickOnPortfolioLink();

            LogStep(2, "Click on portfolio name link. Make sure expected portfolio is selected by default");
            var positionsAlertsStatisticsPanelForm = new PositionsAlertsStatisticsPanelForm();
            Checker.CheckEquals(portfolioName, positionsAlertsStatisticsPanelForm.GetPortfolioName(), 
                "Expected Portfolio name is not shown in portfolios drop-down");
            Checker.IsTrue(positionsAlertsStatisticsPanelForm.IsTabActive(
                    (PositionsTabs)Parsing.ConvertToInt(new PositionsQueries().SelectPositionStatusTypeByPositionId(positionId))),
                "Tab is not active");
        }
    }
}