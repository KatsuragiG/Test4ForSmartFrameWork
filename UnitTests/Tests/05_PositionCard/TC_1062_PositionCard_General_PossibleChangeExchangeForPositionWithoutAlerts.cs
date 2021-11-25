using AutomatedTests.Database.DsiForSymbols;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.User;
using AutomatedTests.Enums;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Forms;
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
    public class TC_1062_PositionCard_General_PossibleChangeExchangeForPositionWithoutAlerts : BaseTestUnitTests
    {
        private const int TestNumber = 1062;

        private List<HDSymbols> availableSymbolModelsOnValoren;
        private string symbol;
        private int symbolId;
        private readonly SymbolsQueries symbolsQueries = new SymbolsQueries();
        private readonly PositionsQueries positionsQueries = new PositionsQueries();

        [TestInitialize]
        public void TestInitialize()
        {
            var userType = GetTestDataParsedAsEnumFromStringMapping<ProductSubscriptionTypes>("user");

            var portfolioModel = new PortfolioModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency")
            };

            symbol = GetTestDataAsString(nameof(symbol));
            var tradeType = GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>("tradeType");
            symbolId = Parsing.ConvertToInt(symbolsQueries.SelectDataFromHDSymbols(symbol).SymbolId);
            var positionDbModel = new PositionsDBModel
            {
                Symbol = symbol,
                TradeType = $"{(int)tradeType}",
                PurchaseDate = DateTime.Now.ToShortDateString()
            };

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, (ProductSubscriptions)(int)userType));
            var portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            var positionId = PositionsAlertsSetUp.AddPositionViaDB(portfolioId, positionDbModel);
            symbolId = Parsing.ConvertToInt(positionsQueries.SelectAllPositionData(positionId).SymbolId);
            availableSymbolModelsOnValoren = new SymbolsQueries().SelectSymbolsFromValorenBySymbol(symbol)
                .Where(t => t.SymbolId != symbolId.ToString()).ToList();

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
            new MainMenuNavigation().OpenPositionCard(positionId);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1062$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test checks ability to change Exchange for position without alerts. https://tr.a1qa.com/index.php?/cases/view/19231999")]
        [TestCategory("PositionCard"), TestCategory("PositionCardGeneral")]
        public override void RunTest()
        {
            LogStep(1, "Check that edit exchange name is available.");
            var positionCardForm = new PositionCardForm();
            Checker.CheckEquals(availableSymbolModelsOnValoren.Any(), positionCardForm.IsEditExchangeNameAvailable(), "Edit exchange name is NOT available");

            foreach (var symbolDbModel in availableSymbolModelsOnValoren)
            {
                LogStep(2, "Remember LatestClose value");
                var previousLatestClose = positionCardForm.GetLatestClose();

                LogStep(3, "Click on 'Pencil'.");
                positionCardForm.EditExchangeName();
                var availableExchangeChanging = positionCardForm.GetAvailableExchangesOptions();
                Checker.CheckEquals(availableSymbolModelsOnValoren.Count + 1,
                    availableExchangeChanging.Count, "Unexpected quantity of options to change exchange");

                LogStep(4, "Put mark to not selected check-box.Remember name for the chosen exchange.");
                positionCardForm.ConvertToAnoterExchange(symbolDbModel.ExchangeName);

                LogStep(5, "Click 'OK'.Make sure expected exchange saved for the position.");
                positionCardForm.ClickOkChangeExchange();
                Browser.Refresh();
                Checker.CheckEquals(symbolDbModel.ExchangeName, positionCardForm.GetExchangeName(), "Exchanged name was not changed");
                Checker.CheckContains(symbol, positionCardForm.GetSymbol(), "Changed Symbol does not contain expected part");
                Checker.CheckNotEquals(previousLatestClose, positionCardForm.GetLatestClose(), "Latest Close was not changed");

                LogStep(6, "In DB: make sure symbol id is changed for the position.");
                var newSymbolId = Parsing.ConvertToInt(positionsQueries.SelectAllPositionData(positionCardForm.GetPositionIdFromUrl()).SymbolId);
                Checker.CheckNotEquals(symbolId, newSymbolId, "SymbolID was not changed after changing exchange name");

                LogStep(7, "In DB: make sure the symbol matched the new exchange.");
                Checker.CheckEquals(symbolDbModel.ExchangeName, symbolsQueries.SelectDataFromHDSymbols(newSymbolId).ExchangeName, "ExchangeName is not match selected in DB");
                symbolId = newSymbolId;
            }
        }
    }
}
