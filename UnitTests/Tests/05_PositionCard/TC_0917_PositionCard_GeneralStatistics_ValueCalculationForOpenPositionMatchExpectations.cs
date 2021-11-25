using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.PositionCard;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._05_PositionCard
{
    [TestClass]
    public class TC_0917_PositionCard_GeneralStatistics_ValueCalculationForOpenPositionMatchExpectations : BaseTestUnitTests
    {
        private const int TestNumber = 917;

        private string expectedValue;
        private int positionId;

        [TestInitialize]
        public void TestInitialize()
        {
            var portfolioModel = new PortfolioModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency")
            };
            var positionAssetType = GetTestDataAsString("StockType").ParseAsEnumFromStringMapping<PositionAssetTypes>();
            var expirationDate = GetTestDataAsString("OptionDate");
            var optionType = GetTestDataAsString("OptionType");
            var strikePrice = GetTestDataAsString("OptionPrice");
            var entryPrice = GetTestDataAsString("EntryDate");
            var positionModel = new PositionAtManualCreatingPortfolioModel
            {
                PositionAssetType = positionAssetType,
                Ticker = GetTestDataAsString("Symbol"),
                TradeType = GetTestDataAsBool("TradeType") ? PositionTradeTypes.Long : PositionTradeTypes.Short,
                Quantity = positionAssetType == PositionAssetTypes.Option ? GetTestDataAsString("Contracts") : GetTestDataAsString("Shares"),
                ExpirationDate = string.IsNullOrEmpty(expirationDate) ? null : expirationDate,
                StrikePrice = string.IsNullOrEmpty(strikePrice) ? null : strikePrice,
                OptionType = string.IsNullOrEmpty(optionType) ? null : optionType,
                EntryDate = GetTestDataAsString("EntryDate"),
                EntryPrice = string.IsNullOrEmpty(entryPrice) ? null : entryPrice
            };
            expectedValue = GetTestDataAsString(nameof(expectedValue));

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, new List<ProductSubscriptions>
                {
                    ProductSubscriptions.TradeStopsPremium, ProductSubscriptions.CryptoStopsPremium
                }
            ));
            var portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);

            positionId = PositionsAlertsSetUp.AddPositionFromInlineForm(portfolioId, positionModel);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_917$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test checks calculation of 'Value' for Open Position matched expectations. https://tr.a1qa.com/index.php?/cases/view/19232216")]
        [TestCategory("Smoke"), TestCategory("PositionCard"), TestCategory("PositionCardStatistics"), TestCategory("Statistics")]
        public override void RunTest()
        {
            LogStep(1, 2, "Click on position and open New Position Card. Open the tab Performance");
            new MainMenuNavigation().OpenPositionCard(positionId);
            var performanceTabPositionCardForm = new PositionCardForm().ActivateTabGetForm<PerformanceTabPositionCardForm>(PositionCardTabs.Performance);

            LogStep(3, 4, "Remember the value of 'Value'. Make sure that the value of 'Value' match expectation (Test Data document)");
            var valueFromDb = new PositionsQueries().SelectAllPositionData(positionId).Value;
            Checker.CheckEquals(expectedValue, StringUtility.SetFormatFromSample(valueFromDb, expectedValue),
                "'Value' does not match expectation in DB");
            Checker.CheckEquals(expectedValue,
                Constants.DecimalNumberRegex.Match(performanceTabPositionCardForm.GetPerformanceTabFieldValue(PerformanceTabFieldTypes.Value).Replace(",", string.Empty)).Value,
                "'Value' does not match expectation on Position Card");
        }
    }
}