using AutomatedTests;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.ImportedPositions;
using AutomatedTests.Enums;
using AutomatedTests.Enums.PositionCard;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Popups;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.BaseTestsUnitTests;
using TradeStops.Common.Enums;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Steps.PositionsGridSteps;
using System.Linq;
using WebdriverFramework.Framework.Util;
using TradeStops.Common.Extensions;
using AutomatedTests.Models;

namespace UnitTests.Tests._04_PositionsAndAlerts._01_Positions
{
    [TestClass]
    public class TC_0130_PositionsGrid_DefinitionUnrecognizedSymbols : BaseTestUnitTests
    {
        private const int TestNumber = 130;

        private CustomTestDataReader reader;
        private int portfolioId;
        private string symbolForStock;
        private string symbolForOption;
        private string expirationDate;
        private string strikePrice;
        private string optionType;
        private string optionVariant;

        [TestInitialize]
        public void TestInitialize()
        {
            reader = new CustomTestDataReader();
            symbolForStock = GetTestDataAsString(nameof(symbolForStock));
            symbolForOption = GetTestDataAsString(nameof(symbolForOption));
            expirationDate = GetTestDataAsString(nameof(expirationDate));
            strikePrice = GetTestDataAsString(nameof(strikePrice));
            optionType = GetTestDataAsString(nameof(optionType));
            optionVariant = GetTestDataAsString(nameof(optionVariant));

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPlatinum));

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);

            new PortfolioGridsSteps().ImportInvestmentPortfolioUsingCusmomCredentials(reader.GetBrokerAccount().BrokerFullName, 
                reader.GetBrokerAccount().Account06, reader.GetBrokerAccount().Password06, true);
            portfolioId = new PortfoliosForm().GetLastImportedPortfolioId(UserModels.First().Email);

            new PortfoliosForm().ClickOnPortfolioNameViaId(portfolioId);
            new PositionsAlertsStatisticsPanelForm().ActivateTab(PositionsTabs.OpenPositions);
            new PositionsTabForm().SelectView(Constants.DefaultViewName);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_130$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test checks determination by user incomplete data https://tr.a1qa.com/index.php?/cases/view/19232929")]
        [TestCategory("Smoke"), TestCategory("PositionsGrid"), TestCategory("SyncPositions"), TestCategory("SyncPortfolio"), TestCategory("SyncPositionEditing")]
        public override void RunTest()
        {
            LogStep(1, "Detect *undeterminated* imported positions with Shares and filled values for Shares, Entry Date and Entry price");
            var importedPositionsQueries = new ImportedPositionsQueries();
            var unrecognizedPositionsData = importedPositionsQueries.SelectAllImportedPositionsDataByPortfolioId(portfolioId);
            Assert.IsTrue(unrecognizedPositionsData.Count > 0, "Number of imported unrecognized positions is zero");
            var appropriatePositionsDataToRecognize = unrecognizedPositionsData.Where(p => !(string.IsNullOrEmpty(p.PurchaseDate)
                    || string.IsNullOrEmpty(p.PurchasePrice) || string.IsNullOrEmpty(p.Shares))).ToList();
            Assert.IsTrue(appropriatePositionsDataToRecognize.Count > 0, "Number of suitable positions is zero");

            LogStep(2, "Select one *stock* among detected PositionIds and found it in the grid");
            var stocksTickers = appropriatePositionsDataToRecognize.Where(p => !string.IsNullOrEmpty(p.ParsedHoldingSymbol) 
            && p.DataType.Contains(PositionAssetTypes.Stock.GetDescription())).ToList();
            var optionsTickers = appropriatePositionsDataToRecognize.Where(p => !(string.IsNullOrEmpty(p.ExpirationDate) 
                    || string.IsNullOrEmpty(p.StrikePrice) || string.IsNullOrEmpty(p.OptionType))
                && p.DataType.Contains(PositionAssetTypes.Option.GetDescription())).ToList();
            Checker.IsTrue(stocksTickers.Count > 0, "There is no appropriate Stock positions for selecting");
            Checker.IsTrue(optionsTickers.Count > 0, "There is no appropriate Options positions for selecting");

            LogStep(3, "Remember values in Entry Date, Entry Price, Shares fields");
            var stockTicker = stocksTickers.First().ParsedHoldingSymbol;
            var positionsGridSteps = new PositionsGridSteps();
            var stockToRecognizeOrder = positionsGridSteps.GetRowsOrdersByTicker(stockTicker).FirstOrDefault();
            Assert.IsTrue(stockToRecognizeOrder != 0, "There is no appropriate Stock's tickers for recognizing");

            var positionsForm = new PositionsTabForm();
            var editShares = positionsForm.GetPositionsGridCellValue(new TableCellMetrics { PositionOrder = stockToRecognizeOrder, ColumnHeader = PositionsGridDataField.Shares.GetStringMapping() });
            var editEntryDate = positionsForm.GetPositionsGridCellValue(new TableCellMetrics { PositionOrder = stockToRecognizeOrder, ColumnHeader = PositionsGridDataField.EntryDate.GetStringMapping() });
            var editEntryPrice = positionsForm.GetPositionsGridCellValue(new TableCellMetrics { PositionOrder = stockToRecognizeOrder, ColumnHeader = PositionsGridDataField.EntryPrice.GetStringMapping() });

            LogStep(4, "Click on Symbol position link in grid for one of such positions ");
            positionsForm.ClickOnPositionLink(stockTicker);

            LogStep(5, "Clear Symbol field, type AAPL and select AAPL from autocomplete");
            var confirmPositionPopup = new ConfirmPositionPopup();
            confirmPositionPopup.SetSymbol(symbolForStock);

            LogStep(6, "Click Save button");
            confirmPositionPopup.ClickOkButton();

            LogStep(7, "Check that remembered values from step3 are presented in grid in position card");
            var positionCardForm = new PositionCardForm();
            positionCardForm.ClickSave();
            var positionDetailsTabPositionCardForm = new PositionDetailsTabPositionCardForm();
            Checker.CheckEquals(positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.EntryDate),
                editEntryDate, "Entry Date are not equal");
            Checker.CheckEquals(Parsing.ConvertToDouble(positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.Shares)),
                Parsing.ConvertToDouble(editShares), "Shares are not equal");
            Checker.CheckEquals(positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.EntryPrice),
                editEntryPrice, "EntryPrice are not equal");
            Checker.CheckEquals(positionCardForm.GetSymbol(), symbolForStock, "Symbol are not equal");

            LogStep(8, 9, "Select one option among detected PositionId and found it in the grid,Click on Symbol position link for option in grid ");
            positionCardForm.ClickOnPortfolioLink();
            positionsForm = new PositionsTabForm();
            var optionTicker = optionsTickers.First().VendorSymbol;
            var optionToRecognizeOrder = positionsGridSteps.GetRowsOrdersByTicker(optionTicker).FirstOrDefault();
            var editSharesOption = 
                positionsForm.GetPositionsGridCellValue(new TableCellMetrics { PositionOrder = optionToRecognizeOrder, ColumnHeader = PositionsGridDataField.Shares.GetStringMapping() });
            var editEntryDateOption = 
                positionsForm.GetPositionsGridCellValue(new TableCellMetrics { PositionOrder = optionToRecognizeOrder, ColumnHeader = PositionsGridDataField.EntryDate.GetStringMapping() });
            var editEntryPriceOption = 
                positionsForm.GetPositionsGridCellValue(new TableCellMetrics { PositionOrder = optionToRecognizeOrder, ColumnHeader = PositionsGridDataField.EntryPrice.GetStringMapping() });
            positionsForm.ClickOnPositionLink(optionTicker);

            LogStep(10, "Clear Symbol field, type ACN and select ACN from autocomplete");
            confirmPositionPopup = new ConfirmPositionPopup();
            confirmPositionPopup.CheckExpiredOptionsCheckbox(false);
            confirmPositionPopup.SetSymbol(symbolForOption);

            LogStep(11, 12, "Detect empty fields among Expiration date, Strike price and Option Type and select remembered on step 14 if such empty fields are presented");
            confirmPositionPopup.SetExpirationDate(expirationDate);
            confirmPositionPopup.SetStrikePrice(strikePrice);
            confirmPositionPopup.SetOptionType(optionType);
            Checker.IsTrue(confirmPositionPopup.GetOptionVariant().Contains(optionVariant), $"Symbol {optionVariant} are not equal {confirmPositionPopup.GetOptionVariant()}");

            LogStep(13, "Click Save button");
            confirmPositionPopup.ClickOkButton();

            LogStep(14, "Check that remembered values from step 17 are presented in grid for edited position ");
            positionCardForm = new PositionCardForm();
            positionCardForm.ClickSave();
            positionDetailsTabPositionCardForm = new PositionDetailsTabPositionCardForm();
            Checker.CheckEquals(editEntryDateOption,
                positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.EntryDate),
                "Entry Date are not equal for option");
            Checker.CheckEquals(Parsing.ConvertToDouble(editSharesOption),
                Parsing.ConvertToDouble(positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.Contracts)) * Constants.DefaultContractSize, 
                "Shares are not equal for option");
            Checker.CheckEquals(editEntryPriceOption, 
                positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.EntryPrice), 
                "EntryPrice are not equal for option");
            Checker.CheckEquals(optionVariant, positionCardForm.GetSymbol(), "Option Tickers are not equal");

            LogStep(15, "Check that selected symbol on step 8 and optionVariant from step 17 is presented in DB (dbo.positions) and disappeared from dbo.UnconfirmedPositions");
            var unrecognizedPositionsDataAfter = importedPositionsQueries.SelectAllImportedPositionsDataByPortfolioId(portfolioId);

            Checker.IsFalse(unrecognizedPositionsDataAfter.Select(t => t.ParsedHoldingSymbol).Where(t => t.EqualsIgnoreCase(optionTicker)).ToList().Any(),
                "Recognized option is present in dbo.UnconfirmedPositions");
            Checker.IsFalse(unrecognizedPositionsDataAfter.Select(t => t.ParsedHoldingSymbol).Where(t => t.EqualsIgnoreCase(stockTicker)).ToList().Any(),
                "Recognized option is present in dbo.UnconfirmedPositions");
        }
    }
}