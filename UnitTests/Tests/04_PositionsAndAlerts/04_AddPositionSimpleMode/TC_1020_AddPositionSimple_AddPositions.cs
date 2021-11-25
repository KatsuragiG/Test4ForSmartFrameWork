using System;
using System.Collections.Generic;
using System.Linq;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._04_PositionsAndAlerts._04_AddPositionSimpleMode
{
    [TestClass]
    public class TC_1020_AddPositionSimple_AddPositions : BaseTestUnitTests
    {
        private const int TestNumber = 1026;
        private const string TickerToAddInWatchPortfolio = "VT";

        private readonly List<PositionAtManualCreatingPortfolioModel> addPositionsSimpleModels = new List<PositionAtManualCreatingPortfolioModel>();
        private List<string> optionsVariants = new List<string>();
        private PortfolioType portfolioTypeToAddPositions;
        private string portfolioNameToAddPositions;
        private int optionsQuantity;
        private List<PositionsGridDataField> columns;

        [TestInitialize]
        public void TestInitialize()
        {
            var userProductSubscriptions = GetUserProductSubscriptions("User");

            columns = new List<PositionsGridDataField>
            {
                PositionsGridDataField.Ticker,
                PositionsGridDataField.EntryDate,
                PositionsGridDataField.TradeType,
                PositionsGridDataField.Shares,
                PositionsGridDataField.EntryPrice
            };

            portfolioTypeToAddPositions = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("portfolioTypeToAddPositions");

            var positionsQuantity = GetTestDataAsInt("positionsQuantity");
            optionsQuantity = GetTestDataAsInt(nameof(optionsQuantity));
            for (int i = 1; i <= positionsQuantity; i++)
            {
                var shares = GetTestDataAsString($"positionQty{i}");
                var entryDate = GetTestDataAsString($"entryDate{i}");
                var entryPrice = GetTestDataAsString($"entryPrice{i}");

                addPositionsSimpleModels.Add(new PositionAtManualCreatingPortfolioModel
                {
                    Ticker = GetTestDataAsString($"Ticker{i}"),
                    PositionAssetType = GetTestDataParsedAsEnumFromStringMapping<PositionAssetTypes>($"StockType{i}"),
                    TradeType = GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>($"tradeType{i}"),
                    Quantity = string.IsNullOrEmpty(shares) ? null : shares,
                    EntryDate = string.IsNullOrEmpty(entryDate) ? null : entryDate,
                    EntryPrice = string.IsNullOrEmpty(entryPrice) ? null : entryPrice,
                });

                if (i <= optionsQuantity)
                {
                    var expirationDate = GetTestDataAsString($"ExpirationDate{i}");
                    var strikePrice = GetTestDataAsString($"StrikePrice{i}");
                    var optionType = GetTestDataAsString($"OptionType{i}");

                    addPositionsSimpleModels[i - 1].ExpirationDate = string.IsNullOrEmpty(expirationDate) ? null : expirationDate;
                    addPositionsSimpleModels[i - 1].StrikePrice = string.IsNullOrEmpty(strikePrice) ? null : strikePrice;
                    addPositionsSimpleModels[i - 1].OptionType = string.IsNullOrEmpty(optionType) ? null : optionType;
                }
            }
            optionsVariants = GetTestDataValuesAsListByColumnName(nameof(optionsVariants));

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));

            PortfoliosSetUp.AddInvestmentUsdPortfoliosWithOpenPosition(UserModels.First().Email);
            PortfoliosSetUp.AddWatchOnlyUsdPortfoliosWithOpenPosition(UserModels.First().Email, TickerToAddInWatchPortfolio);
            var portfoliosQueries = new PortfoliosQueries();
            var portfoliosIds = portfoliosQueries.SelectPortfoliosDataByUserId(UserModels.First());

            var portfolioIdToAddPosition = (int)portfolioTypeToAddPositions == Parsing.ConvertToInt(portfoliosIds[0].Type)
                ? portfoliosIds[0].PortfolioId 
                : portfoliosIds[1].PortfolioId;
            portfolioNameToAddPositions = portfoliosQueries.SelectPortfolioName(Parsing.ConvertToInt(portfolioIdToAddPosition));

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
            new MainMenuNavigation().OpenPositionsGrid();
            new PositionsAlertsStatisticsPanelForm().SelectPortfolio(AllPortfoliosKinds.All.GetStringMapping());
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1020$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PositionsGrid"), TestCategory("AddPositionPopup")]
        [Description("https://tr.a1qa.com/index.php?/cases/view/19232983 The test ability adding several positions with different AssetTypes, Entry Dates, " +
            "Entry Price into manual portfolio (Investment or Watch Only) ")]
        public override void RunTest()
        {
            LogStep(1, "Click 'Add Position' button.");
            var positionsTab = new PositionsTabForm();
            positionsTab.ClickAddPositionButton();

            LogStep(2, "Select the created in precondition portfolio (portfolio type in accordance of xls file)");
            var addPositionPopup = new AddPositionInFrameForm();
            addPositionPopup.SelectPortfolio(portfolioNameToAddPositions);
            Checker.CheckEquals(portfolioNameToAddPositions, addPositionPopup.GetSelectedPortfolio(), "Expected portfolio name is NOT typed for portfolio");

            LogStep(3, 6, "Select positions data. Check showing values");
            addPositionPopup.FillPositionsFields(addPositionsSimpleModels);
            for (int i = 0; i < addPositionsSimpleModels.Count; i++)
            {
                Checker.CheckEquals(addPositionsSimpleModels[i].Ticker, addPositionPopup.GetTextFromPositionsAutocompleteDataFields(PositionForManualPortfolioCreateInformation.Ticker, i + 1),
                    $"Unexpected ticker is selected for {i} row");
                Checker.CheckEquals(addPositionsSimpleModels[i].PositionAssetType, addPositionPopup.GetPositionType(i + 1),
                    $"Asset type is NOT as expected for {addPositionsSimpleModels[i].Ticker}");
                Checker.CheckEquals(addPositionsSimpleModels[i].TradeType, addPositionPopup.GetSelectedTradeTypeByOrder(i + 1).ParseAsEnumFromStringMapping<PositionTradeTypes>(),
                    $"Trade type is NOT as expected for {addPositionsSimpleModels[i].Ticker}");
                var displayedQuantity = addPositionPopup.GetTextFromPositionsDataFields(PositionForManualPortfolioCreateInformation.Quantity, i + 1).Replace(",", "");
                Checker.CheckEquals(addPositionsSimpleModels[i].Quantity,
                    string.IsNullOrEmpty(displayedQuantity) ? null : displayedQuantity,
                    $"Quantity is NOT as expected for {addPositionsSimpleModels[i].Ticker}");
                if (i < optionsQuantity)
                {
                    Checker.CheckEquals(addPositionsSimpleModels[i].ExpirationDate, addPositionPopup.GetExpirationDate(i + 1),
                        $"Expiration Date is NOT as expected for {addPositionsSimpleModels[i].Ticker}");
                    Checker.IsFalse(string.IsNullOrEmpty(addPositionPopup.GetStrikePrice(i + 1)),
                        $"Strike Price is empty for {addPositionsSimpleModels[i].Ticker}");
                    Checker.IsFalse(string.IsNullOrEmpty(addPositionPopup.GetOptionType(i + 1)),
                        $"Option Type is NOT as expected for {addPositionsSimpleModels[i].Ticker}");
                }
            }

            LogStep(7, "Click 'Save and Close' button.");
            addPositionPopup.ClickSaveAndClose();
            addPositionPopup.AssertIsClosed();

            LogStep(7, "Click 'Save and Close' button.");
            var positionsAlertsStatisticsPanelForm = new PositionsAlertsStatisticsPanelForm();
            positionsAlertsStatisticsPanelForm.SelectPortfolio(portfolioNameToAddPositions);
            Checker.CheckEquals(portfolioTypeToAddPositions == PortfolioType.Investment ? AllPortfoliosKinds.AllInvestment.GetStringMapping() : AllPortfoliosKinds.AllWatch.GetStringMapping(), 
                positionsAlertsStatisticsPanelForm.GetPortfolioName(), "Expected portfolio name is NOT selected on position grid");

            LogStep(9, "Make sure the grid contain expected data.");
            var tickerList = positionsTab.GetPositionColumnValues(PositionsGridDataField.Ticker).Select(t => t.Split('\r')[0]).ToList();
            foreach (var optionVariants in optionsVariants)
            {
                Checker.IsTrue(tickerList.Contains(optionVariants), $"Added option is NOT shown in Position grid for {optionVariants}");
            }
            for (int i = optionsQuantity; i < addPositionsSimpleModels.Count; i++)
            {
                Checker.IsTrue(tickerList.Contains(addPositionsSimpleModels[i].Ticker), $"Position list does NOT contain in Position grid {addPositionsSimpleModels[i].Ticker}");
            }
            var positionsData = positionsTab.GetPositionDataForAllPositions(columns);
            foreach (var addPositionsSimpleModel in addPositionsSimpleModels)
            {
                var mappedModel = positionsData.FirstOrDefault(u => u.Ticker.StartsWith(addPositionsSimpleModel.Ticker));
                if (mappedModel == null)
                {
                    Checker.Fail($"Position {addPositionsSimpleModel.Ticker} NOT found in Positions grid");
                }
                else
                {
                    var expectedShares = string.IsNullOrEmpty(addPositionsSimpleModel.Quantity) ? 0 : Parsing.ConvertToDecimal(addPositionsSimpleModel.Quantity);
                    Checker.CheckEquals(Math.Round(addPositionsSimpleModel.PositionAssetType == PositionAssetTypes.Option
                            ? expectedShares * Constants.DefaultContractSize
                            : expectedShares, Constants.DefaultDecimalRounding),
                        Math.Round(Parsing.ConvertToDecimal(mappedModel.Shares.Replace("-", "")), Constants.DefaultDecimalRounding),
                        $"Shares is NOT equals for {addPositionsSimpleModel.Ticker} on position grid");
                    var expectedEntryDate = string.IsNullOrEmpty(addPositionsSimpleModel.EntryDate) ? Constants.NotAvailableAcronym : addPositionsSimpleModel.EntryDate;
                    Checker.CheckEquals(expectedEntryDate, mappedModel.EntryDate, $"Entry date is NOT equals for {addPositionsSimpleModel.Ticker} on position grid");
                    if (string.IsNullOrEmpty(addPositionsSimpleModel.EntryPrice))
                    {
                        Checker.CheckNotEquals(Constants.NotAvailableAcronym, mappedModel.EntryPrice, "Entry Price is NOT N/A on position grid");
                    }
                    else
                    {
                        Checker.CheckEquals(Math.Round(Parsing.ConvertToDecimal(addPositionsSimpleModel.EntryPrice), Constants.DefaultDecimalRounding),
                            Math.Round(Parsing.ConvertToDecimal(StringUtility.ReplaceAllCurrencySigns(mappedModel.EntryPrice)), Constants.DefaultDecimalRounding),
                            $"Entry price is NOT equals for {addPositionsSimpleModel.Ticker} on position grid");
                    }
                    Checker.CheckEquals(addPositionsSimpleModel.TradeType.GetStringMapping(), mappedModel.TradeType,
                        $"Trade Type is NOT equals for {addPositionsSimpleModel.Ticker} on position grid");
                }
            }
        }
    }
}