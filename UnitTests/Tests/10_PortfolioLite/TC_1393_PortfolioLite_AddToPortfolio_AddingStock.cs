using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.PositionData;
using AutomatedTests.Database.Positions;
using AutomatedTests.Database.Users;
using AutomatedTests.Enums.PortfolioLite;
using AutomatedTests.Enums.PositionCard;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Forms.PortfolioLite;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Models.PositionsModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System;
using System.Globalization;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._10_PortfolioLite
{
    [TestClass]
    public class TC_1393_PortfolioLite_AddToPortfolio_AddingStock : BaseTestUnitTests
    {
        private const int TestNumber = 1393;

        private PortfolioLitePositionModel addedPositionModel;
        private PortfolioLiteCardModel expectedPositionCardModel;
        private DateTime lastTradeDate;
        private readonly PositionDataQueries positionDataQueries = new PositionDataQueries();
        private readonly PositionsQueries positionsQueries = new PositionsQueries();
        private int quantityOfAddedPositions;
        private string symbolToSearch;
        private string expectedAdjustment;

        [TestInitialize]
        public void TestInitialize()
        {
            quantityOfAddedPositions = GetTestDataAsInt(nameof(quantityOfAddedPositions));
            symbolToSearch = GetTestDataAsString(nameof(symbolToSearch));
            lastTradeDate = DateTime.Parse(positionDataQueries.SelectLastTradeDate(symbolToSearch));

            var symbolToAdd = GetTestDataAsString("symbolToAdd");
            var entryDate = GetTestDataAsString("entryDate");
            var entryPrice = GetTestDataAsString("entryPrice");
            var quantity = GetTestDataAsString("quantity");
            addedPositionModel = new PortfolioLitePositionModel
            {
                Ticker = string.IsNullOrEmpty(symbolToAdd) 
                    ? null 
                    : symbolToAdd,
                BuyDate = string.IsNullOrEmpty(entryDate) 
                    ? null 
                    : entryDate,
                Qty = string.IsNullOrEmpty(quantity) 
                    ? null 
                    : quantity,
                BuyPrice = string.IsNullOrEmpty(entryPrice) 
                    ? null 
                    : entryPrice,
                IsLongType = GetTestDataAsBool("isLongType")
            };

            expectedAdjustment = (bool)addedPositionModel.IsLongType
                ? false.ToString()
                : true.ToString();

            expectedPositionCardModel = new PortfolioLiteCardModel
            {
                Ticker = string.IsNullOrEmpty(symbolToAdd) 
                    ? symbolToSearch 
                    : symbolToAdd,
                EntryDate = string.IsNullOrEmpty(entryDate) 
                    ? Parsing.ConvertToShortDateString(lastTradeDate.ToString(CultureInfo.InvariantCulture)) 
                    : entryDate,
                EntryPrice = string.IsNullOrEmpty(entryPrice) 
                    ? Constants.NotAvailableAcronym 
                    : entryPrice,
                Shares = string.IsNullOrEmpty(quantity) 
                    ? Constants.DefaultStringZeroIntValue 
                    : quantity
            };

            var hdSymbolStatisticsModel = positionDataQueries.SelectSymbolStatisticsForSymbol(expectedPositionCardModel.Ticker);
            addedPositionModel.Currency = (Currency)positionsQueries.SelectSymbolCurrencyBySymbol(expectedPositionCardModel.Ticker);
            expectedPositionCardModel.LatestClose = $"{addedPositionModel.Currency.GetDescription()}{hdSymbolStatisticsModel.LatestClose}";
            addedPositionModel.Name = positionsQueries.SelectSymbolIdNameUsingSymbol(expectedPositionCardModel.Ticker).SymbolName;
            if (expectedPositionCardModel.EntryPrice == Constants.NotAvailableAcronym)
            {
                expectedPositionCardModel.EntryPrice = positionDataQueries.SelectHdAdjPriceForSymbolIdDate(expectedPositionCardModel.Ticker, expectedPositionCardModel.EntryDate)
                    .GetTradeSplitOnlyAdjClose().ToString(CultureInfo.InvariantCulture);
            }

            LogStep(0, "Preconditions. Create user with subscription to PortfolioLite. navigate To Add portfolio Form");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.PortfolioLite));
            var fakeSnaid = StringUtility.RandomString(Constants.SnaidPattern);
            new UsersQueries().UpdateUserSnaid(fakeSnaid, UserModels.First().TradeSmithUserId);
            new PortfolioLiteNavigation().OpenPortfolioLiteWithUserGuid(UserModels.First());
            var portfolioLiteMainForm = new PortfolioLiteMainForm();
            portfolioLiteMainForm.SearhSymbol(symbolToSearch);
            new PortfolioLiteAnalyzerForm().ClickAdditionalActionsButton(PortfolioLiteAdditionalButtons.AddToPortfolio);
            portfolioLiteMainForm.ExitFrame();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1393$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("PortfolioLite")]
        [Description("Test checks that user can add position with custom values from the Portfolio Lite. https://tr.a1qa.com/index.php?/cases/view/21116213")]
        public override void RunTest()
        {
            LogStep(1, "Check that Add to Portfolio from is shown");
            var portfolioLiteMainForm = new PortfolioLiteMainForm();
            var addToPortfolioForm = new PortfolioLiteAddToPortfolioForm();
            addToPortfolioForm.AssertIsOpen();

            LogStep(2, "Check prefilled data");
            Checker.IsTrue(addToPortfolioForm.IsBtnTradeTypeActive(PositionTradeTypes.Long), "Default trade type is not Long");
            Checker.CheckEquals(symbolToSearch, addToPortfolioForm.GetValueFromTextBoxField(PortfolioLiteAddPositionFields.Ticker),
                "Ticker from precondition is NOT shown in the ticker field");
            var expectedEntryDate = Parsing.ConvertToShortDateString(lastTradeDate.ToString(CultureInfo.InvariantCulture));
            Checker.CheckEquals(expectedEntryDate,
                addToPortfolioForm.GetValueFromTextBoxField(PortfolioLiteAddPositionFields.BuyDate),
                "Buy Date has NOT expected value");
            var expectedEntryPrice = positionDataQueries.SelectHdAdjPriceForSymbolIdDate(symbolToSearch, expectedEntryDate).GetTradeSplitOnlyAdjClose();
            Checker.CheckEquals($"{((Currency)positionsQueries.SelectSymbolCurrencyBySymbol(symbolToSearch)).GetDescription()}{expectedEntryPrice}",
                addToPortfolioForm.GetValueFromTextBoxField(PortfolioLiteAddPositionFields.BuyPrice),
                "Buy price has NOT expected value");
            Checker.CheckEquals(string.Empty,
                addToPortfolioForm.GetValueFromTextBoxField(PortfolioLiteAddPositionFields.Qty),
                "Quantity has NOT expected value");

            LogStep(3, "Fill all fields for the position according to test data");
            addToPortfolioForm.FillFields(addedPositionModel);
            Checker.CheckEquals(expectedPositionCardModel.Ticker,
                addToPortfolioForm.GetValueFromTextBoxField(PortfolioLiteAddPositionFields.Ticker),
                "Ticker has NOT expected value after filling fields");
            Checker.CheckEquals(expectedPositionCardModel.EntryDate,
                addToPortfolioForm.GetValueFromTextBoxField(PortfolioLiteAddPositionFields.BuyDate),
                "Buy Date has NOT expected value after filling fields");
            var expectedFinishEntryPrice = $"{addedPositionModel.Currency.GetDescription()}{expectedPositionCardModel.EntryPrice}";
            Checker.CheckEquals(expectedFinishEntryPrice,
                addToPortfolioForm.GetValueFromTextBoxField(PortfolioLiteAddPositionFields.BuyPrice),
                "Buy price has NOT expected value after filling fields");
            var expectedShares = string.IsNullOrEmpty(addedPositionModel.Qty) && expectedPositionCardModel.Shares == Constants.DefaultStringZeroIntValue
                ? string.Empty
                : expectedPositionCardModel.Shares;
            Checker.CheckEquals(expectedShares,
                addToPortfolioForm.GetValueFromTextBoxField(PortfolioLiteAddPositionFields.Qty).Replace(",", string.Empty),
                "Quantity has NOT expected value after filling fields");
            Checker.CheckEquals(addedPositionModel.IsLongType,
                addToPortfolioForm.IsBtnTradeTypeActive(PositionTradeTypes.Long),
                "Trade type is not As expected after filling");

            LogStep(4, "Click Add");
            addToPortfolioForm.ClickAdditionalActionsButton(PortfolioLiteAddActionsTypes.Save);
            Assert.AreEqual(quantityOfAddedPositions, portfolioLiteMainForm.GetPositionQuantityInGrid(),
                "Grid does not contain added position");

            LogStep(5, "Click on position link");
            portfolioLiteMainForm.ClickPositionLinkInGridByNumber(quantityOfAddedPositions);
            var portfolioLiteCardForm = new PortfolioLiteCardForm();

            LogStep(6, "Check data on position card at editing");
            portfolioLiteCardForm.ActivateTabGetForm<PositionDetailsTabPositionCardForm>(PortfolioLiteCardTabs.PositionDetails);
            var positionDetailsTab = new PositionDetailsTabPositionCardForm();
            positionDetailsTab.EditPositionCard();
            Checker.CheckEquals(expectedPositionCardModel.Ticker, portfolioLiteCardForm.GetSymbol(),
                "Ticker on the position card is not matched expectation");
            Checker.CheckEquals(expectedPositionCardModel.EntryDate, positionDetailsTab.GetValueInTextBoxField(PositionDetailsFieldTypes.EntryDate),
                "Entry Date on the position card is not matched expectation");
            Checker.CheckEquals(expectedFinishEntryPrice.EndsWith(Constants.DefaultStringZeroIntValue) ? string.Empty : expectedFinishEntryPrice,
                positionDetailsTab.GetValueInTextBoxField(PositionDetailsFieldTypes.EntryPrice),
                "Entry Price on the position card  is not matched expectation");
            Checker.CheckEquals(expectedPositionCardModel.Shares,
                positionDetailsTab.GetValueInTextBoxField(PositionDetailsFieldTypes.Shares).Replace(",", string.Empty),
                "Shares on the position card is not matched expectation");

            LogStep(7, "Check position adjustment in DB");
            var positionId = portfolioLiteCardForm.GetPositionIdFromElement();
            var positionFromDb = positionsQueries.SelectAllPositionData(positionId);
            Checker.CheckEquals(expectedAdjustment, positionFromDb.IgnoreDividend,
                "Portfolio Lite Position adjustment in DB is not matched expectation");
        }
    }
}