using System;
using System.Linq;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Positions;
using AutomatedTests.Database.Users;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Chart;
using AutomatedTests.Enums.PortfolioLite;
using AutomatedTests.Enums.PositionCard;
using AutomatedTests.Forms.PortfolioLite;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Models.PositionsModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._10_PortfolioLite
{
    [TestClass]
    public class TC_1211_PortfolioLite_AddPosition_AddingStockWithCustomValues : BaseTestUnitTests
    {
        private const int TestNumber = 1211;
        private const int QuantityOfAddedPositions = 1;

        private PortfolioLitePositionModel positionModel;
        private string expectedAdjustment;
        private string expectedSharesSign;
        private string expectedSharesSignOnCard;
        private bool expectedPortfolioLineVisibility;

        [TestInitialize]
        public void TestInitialize()
        {
            positionModel = new PortfolioLitePositionModel
            {
                Ticker = GetTestDataAsString("Symbol"),
                BuyDate = GetTestDataAsString("EntryDate"),
                Qty = GetTestDataAsString("Shares"),
                BuyPrice = GetTestDataAsString("EntryPrice"),
                IsLongType = GetTestDataAsBool("IsLongType"),
                Currency = GetTestDataParsedAsEnumFromStringMapping<Currency>("Currency")
            };
            expectedAdjustment = (bool)positionModel.IsLongType
                ? false.ToString()
                : true.ToString();
            expectedSharesSign = (bool)positionModel.IsLongType || positionModel.Qty == Constants.DefaultStringZeroIntValue
                ? string.Empty
                : Constants.MinusSign;
            expectedSharesSignOnCard = (bool)positionModel.IsLongType
                ? string.Empty
                : Constants.MinusSign;
            expectedPortfolioLineVisibility = GetTestDataAsBool(nameof(expectedPortfolioLineVisibility));

            LogStep(0, "Preconditions. Create user with subscription to PortfolioLite");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.PortfolioLite));
            var fakeSnaid = StringUtility.RandomString(Constants.SnaidPattern);
            new UsersQueries().UpdateUserSnaid(fakeSnaid, UserModels.First().TradeSmithUserId);
            new PortfolioLiteNavigation().OpenPortfolioLiteWithUserGuid(UserModels.First());
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1211$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("PortfolioLite")]
        [Description("Test checks that user can add position with custom values from the Portfolio Lite. https://tr.a1qa.com/index.php?/cases/view/21116213")]
        public override void RunTest()
        {
            LogStep(1, "Click edit sign for defined in xls position");
            var portfolioLiteMainForm = new PortfolioLiteMainForm();
            Checker.IsTrue(portfolioLiteMainForm.IsInfoAboutEmptyPortfolioPresent(), "Info about Empty Portfolio is NOT shown");
            Checker.IsFalse(portfolioLiteMainForm.IsAddPositionBlockPresent(), "Add position Block is shown for empty portfolio");
            Checker.IsFalse(portfolioLiteMainForm.IsPortfolioStatBlockPresent(), "Portfolio Stat Block is shown for empty portfolio");

            LogStep(2, "Click Add a Position");
            portfolioLiteMainForm.ClickAddAPosition();
            Checker.IsFalse(portfolioLiteMainForm.IsInfoAboutEmptyPortfolioPresent(), "Info about Empty Portfolio is shown");
            Checker.IsTrue(portfolioLiteMainForm.IsAddPositionBlockPresent(), "Add position Block is NOT shown before adding a position");
            Checker.IsTrue(portfolioLiteMainForm.IsPortfolioStatBlockPresent(), "Portfolio Stat Block is NOT shown before adding a position");

            LogStep(3, "Fill all fields for creating position. Click Add");
            portfolioLiteMainForm.AddPosition(positionModel);
            Checker.CheckEquals(QuantityOfAddedPositions, portfolioLiteMainForm.GetPositionQuantityInGrid(),
                "Grid does not contain only the added position");

            LogStep(4, "Check in the grid that created position has expected values");
            var actualDataForPositionPortfolioLite = portfolioLiteMainForm.GetPositionGridDataByOrder(QuantityOfAddedPositions);
            Checker.CheckEquals(positionModel.Ticker, actualDataForPositionPortfolioLite.Ticker.Split('\r')[0],
                "Ticker in the grid is not matched expectation");
            Checker.CheckEquals(positionModel.BuyDate, actualDataForPositionPortfolioLite.EntryDate,
                "Entry Date in the grid is not matched expectation");
            Checker.CheckEquals(StringUtility.SetFormatFromSample(positionModel.BuyPrice, actualDataForPositionPortfolioLite.EntryPrice),
                Constants.DecimalNumberRegex.Match(actualDataForPositionPortfolioLite.EntryPrice.Replace(",", "")).Value,
                "Entry Price in the grid is not matched expectation");
            Checker.CheckEquals(positionModel.Currency.ToString(),
                Constants.AllCurrenciesRegex.Match(actualDataForPositionPortfolioLite.EntryPrice).Value.ParseAsEnumFromDescription<Currency>().ToString(),
                "Currency of Entry Price in the grid is not matched expectation");
            Checker.CheckEquals($"{expectedSharesSign}{positionModel.Qty.ToFractionalString()}",
                actualDataForPositionPortfolioLite.Shares.Replace(",", ""),
                "Shares in the grid is not matched expectation");

            LogStep(5, "Check in DB that created position has expected values");
            var positionsQueries = new PositionsQueries();
            var positionFromDb = positionsQueries.SelectAllPositionData(positionsQueries.SelectPositionIdByUserEmailWithSymbol(UserModels.First().Email, positionModel.Ticker).First());
            Checker.CheckEquals(positionModel.Ticker, positionsQueries.SelectSymbolBySymbolId(Parsing.ConvertToInt(positionFromDb.SymbolId)),
                "Symbol in DB is not matched expectation");
            Checker.CheckEquals(positionModel.BuyDate, DateTime.Parse(positionFromDb.PurchaseDate).ToString(Constants.ShortDateFormat),
                "Entry Date in DB is not matched expectation");
            Checker.CheckEquals(Parsing.ConvertToDouble(positionModel.BuyPrice).ToString("#0.00000000"), positionFromDb.PurchasePrice,
                "Entry Price in DB is not matched expectation");
            Checker.CheckEquals(((int)positionModel.Currency).ToString(), positionFromDb.CurrencyId,
                "Currency in DB is not matched expectation");
            Checker.CheckEquals(expectedAdjustment, positionFromDb.IgnoreDividend,
                "Portfolio Lite Position adjustment in DB is not matched expectation");
            Checker.CheckEquals(Parsing.ConvertToDouble(positionModel.Qty).ToString("#0.00000000"), positionFromDb.Shares,
                "Shares in DB is not matched expectation");

            LogStep(6, "Click More details");
            portfolioLiteMainForm.ClickPortfolioSummary();
            var portfolioLiteDetailsForm = new PortfolioLiteDetailsForm();
            Checker.IsTrue(portfolioLiteDetailsForm.Chart.IsChartLinePresent(ChartLineTypes.Index),
                "Index line is not shown on the equity chart");
            Checker.CheckEquals(expectedPortfolioLineVisibility, portfolioLiteDetailsForm.Chart.IsChartLinePresent(ChartLineTypes.UserPortfolio),
                "User portfolio equity line is not shown on the equity chart");
            Checker.IsTrue(portfolioLiteDetailsForm.IndustryPieChart.IsExists(), "Industry Pie Chart is not shown");
            Checker.IsTrue(portfolioLiteDetailsForm.PositionAllocationPieChart.IsExists(), "PositionAllocation Pie Chart is not shown");
            Checker.IsTrue(portfolioLiteDetailsForm.GainPieChart.IsExists(), "Gain Pie Chart is not shown");

            LogStep(7, "Click Less details");
            portfolioLiteMainForm.ClickPortfolioSummary();
            Checker.IsFalse(portfolioLiteDetailsForm.Chart.IsChartAreaExist(), "Chart area is shown after clicking Less details");
            Checker.IsFalse(portfolioLiteDetailsForm.IndustryPieChart.IsExists(), "Industry Pie Chart is shown");
            Checker.IsFalse(portfolioLiteDetailsForm.PositionAllocationPieChart.IsExists(), "PositionAllocation Pie Chart is shown");
            Checker.IsFalse(portfolioLiteDetailsForm.GainPieChart.IsExists(), "Gain Pie Chart is shown");

            LogStep(8, "Click on position link");
            portfolioLiteMainForm.ClickPositionLinkInGridByNumber(QuantityOfAddedPositions);
            var portfolioLiteCardForm = new PortfolioLiteCardForm();
            Checker.CheckEquals(positionModel.Ticker, portfolioLiteCardForm.GetSymbol(),
                "Ticker on the position card is not matched expectation");

            LogStep(9, "Check data on position card and tabs");
            new PortfolioLiteChartTabForm().AssertIsOpen();
            Checker.IsTrue(portfolioLiteCardForm.Chart.IsChartLinePresent(ChartLineTypes.Price),
                "Price line on the position card is not shown");

            portfolioLiteCardForm.ActivateTabGetForm<PositionDetailsTabPositionCardForm>(PortfolioLiteCardTabs.PositionDetails);
            var positionDetailsTab = new PositionDetailsTabPositionCardForm();
            Checker.CheckEquals(positionModel.BuyDate, positionDetailsTab.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.EntryDate),
                "Entry Date on the position card  is not matched expectation");

            var actualEntryPrice = positionDetailsTab.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.EntryPrice);
            if (actualEntryPrice != Constants.NotAvailableAcronym)
            {
                Checker.CheckEquals(StringUtility.SetFormatFromSample(positionModel.BuyPrice, actualEntryPrice),
                    Constants.DecimalNumberRegex.Match(actualEntryPrice.Replace(",", "")).Value,
                    "Entry Price on the position card  is not matched expectation");

                Checker.CheckEquals(positionModel.Currency.ToString(),
                    Constants.AllCurrenciesRegex.Match(actualEntryPrice).Value.ParseAsEnumFromDescription<Currency>().ToString(),
                    "Currency of Entry Price on the position card  is not matched expectation");
            }

            Checker.CheckEquals($"{expectedSharesSignOnCard}{positionModel.Qty.ToFractionalString()}",
                positionDetailsTab.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.Shares).Replace(",", ""),
                "Shares on the position card is not matched expectation");

            LogStep(10, "Click every tab and check that corresponded tab is shown");
            portfolioLiteCardForm.ActivateTabWithoutChartWaiting(PortfolioLiteCardTabs.Performance);
            new PerformanceTabPositionCardForm().AssertIsOpen();

            portfolioLiteCardForm.ActivateTabWithoutChartWaiting(PortfolioLiteCardTabs.Statistics);
            new StatisticTabForm().AssertIsOpen();
        }
    }
}