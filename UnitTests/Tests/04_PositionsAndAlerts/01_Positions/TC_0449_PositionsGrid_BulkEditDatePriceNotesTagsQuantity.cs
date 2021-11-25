using AutomatedTests.ConstantVariables;
using AutomatedTests.Enums.Portfolios.CreateManual;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms.Popups;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System;
using TradeStops.Common.Enums;
using TradeStops.Common.Extensions;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._04_PositionsAndAlerts._01_Positions
{
    [TestClass]
    public class TC_0449_PositionsGrid_BulkEditDatePriceNotesTagsQuantity : BaseTestUnitTests
    {
        private const int TestNumber = 449;
        private const int DaysShiftForEntryDate = -1;
        private const string MinimalCryptoPrice = "0.00000001";
        private const string MaximalPrice = "9999999999.99999999";
        private const string MaximalPriceAfterEditing = "9999999999.999996";

        private AddPortfolioManualModel portfolioModel;
        private readonly List<PositionAtManualCreatingPortfolioModel> positionsModels = new List<PositionAtManualCreatingPortfolioModel>();
        private readonly List<string> pricesList = new List<string> 
        { 
            Constants.DefaultStringZeroIntValue, 
            MinimalCryptoPrice, 
            Constants.DefaultStringZeroIntValue, 
            MaximalPrice, 
            MinimalCryptoPrice, 
            MaximalPrice,
            MaximalPrice 
        };
        private readonly List<string> shownPricesList = new List<string> 
        { 
            Constants.DefaultStringZeroIntValue, 
            MinimalCryptoPrice, 
            Constants.DefaultStringZeroIntValue, 
            MaximalPriceAfterEditing, 
            MinimalCryptoPrice, 
            MaximalPriceAfterEditing, 
            MaximalPriceAfterEditing };
        private readonly List<string> quantitiesList = new List<string> 
        { 
            Constants.DefaultStringZeroIntValue, 
            MinimalCryptoPrice, 
            Constants.DefaultStringZeroIntValue, 
            MinimalCryptoPrice, 
            MaximalPrice, 
            MinimalCryptoPrice, 
            Constants.DefaultStringZeroIntValue 
        };
        private readonly List<string> shownQuantitiesList = new List<string> 
        { 
            Constants.DefaultStringZeroIntValue, 
            MinimalCryptoPrice, 
            Constants.DefaultStringZeroIntValue, 
            MinimalCryptoPrice, 
            MaximalPriceAfterEditing,
            MinimalCryptoPrice, 
            Constants.DefaultStringZeroIntValue 
        };
        private string tickerForStock;
        private string tickerForOption;

        [TestInitialize]
        public void TestInitialize()
        {
            portfolioModel = new AddPortfolioManualModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency")
            };
            var positionsQuantity = GetTestDataAsInt("positionsQuantity");
            for (int i = 1; i <= positionsQuantity; i++)
            {
                var expirationDate = GetTestDataAsString($"ExpirationDate{i}");
                var strikePrice = GetTestDataAsString($"StrikePrice{i}");
                var optionType = GetTestDataAsString($"OptionType{i}");
                positionsModels.Add(new PositionAtManualCreatingPortfolioModel
                {
                    PositionAssetType = GetTestDataParsedAsEnumFromStringMapping<PositionAssetTypes>($"AssetType{i}"),
                    TradeType = GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>($"TradeType{i}"),
                    Ticker = GetTestDataAsString($"Ticker{i}"),
                    EntryDate = GetTestDataAsString($"EntryDate{i}"),
                    Quantity = GetTestDataAsString($"Shares{i}"),
                    EntryPrice = GetTestDataAsString($"EntryPrice{i}"),
                    ExpirationDate = string.IsNullOrEmpty(expirationDate) ? null : expirationDate,
                    StrikePrice = string.IsNullOrEmpty(strikePrice) ? null : strikePrice,
                    OptionType = string.IsNullOrEmpty(optionType) ? null : optionType,
                });
            }
            tickerForStock = GetTestDataAsString("Ticker5");
            tickerForOption = GetTestDataAsString("Ticker6");

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, new List<ProductSubscriptions>
                {
                    ProductSubscriptions.TradeStopsPremium, ProductSubscriptions.CryptoStopsPremium
                }
            ));

            LoginSetUp.LogIn(UserModels.First());
            new AddPortfoliosSteps().NavigateToAddManualPortfolioSavePortfolioAfterFillingFields(portfolioModel, positionsModels);
            new AddAlertsAtCreatingPortfolioForm().ClickActionButton(AddAlertsAtCreatingPortfolioButtons.AddAlertsLater);

            PortfoliosSetUp.ImportDagSiteInvestment49(true);

            new MainMenuNavigation().OpenPositionsGrid();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_449$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PositionsGrid"), TestCategory("PositionsGridBulkActionButtons")]
        [Description("The test checks correctness editing of date, price. https://tr.a1qa.com/index.php?/cases/view/19232650")]
        public override void RunTest()
        {
            LogStep(1, "Choose on the Portfolio drop-down menu 'MyPortfolio1' and Import portfolio 'xx5555'");
            new PositionsAlertsStatisticsPanelForm().SelectPortfolio(AllPortfoliosKinds.All.GetStringMapping());

            LogStep(2, "Select all displayed in the grid positions. Click the Bulk Edit button");
            var positionsTab = new PositionsTabForm();
            positionsTab.SelectAllItemsInGrid();
            positionsTab.ClickGroupActionButton(PositionsGroupAction.BulkEdit);

            LogStep(3, "Make sure that Quantity is disabled for imported confirmed Ticker6");
            var bulkEditPositionFrame = new BulkEditPositionFrame();
            var tickersColumnValues = bulkEditPositionFrame.GetColumnValuesByDataFields(BulkEditColumnTypes.Ticker)
                .Select(t => t.Trim()).ToList();
            var rowOrderForPosition = GetRowOrderForPosition(tickersColumnValues, tickerForStock);
            if (rowOrderForPosition.Count == 0)
            {
                Checker.Fail($"Bulk Edit does not contains row with {tickerForStock} before editing");
            }
            else
            {
                Checker.IsTrue(bulkEditPositionFrame.IsBulkEditQuantityFieldDisabledByRowOrder(rowOrderForPosition[0] + 1),
                    $"Quantity for {tickerForStock} is not disabled");
            }

            LogStep(4, 5, "Make sure that Option Parameters fields, Quantity, Notes and Tags are disabled for imported unconfirmed Ticker6");
            rowOrderForPosition = GetRowOrderForPosition(tickersColumnValues, tickerForOption);
            if (rowOrderForPosition.Count == 0)
            {
                Checker.Fail($"Bulk Edit does not contains row with {tickerForOption} before editing");
            }
            else
            {
                Checker.IsTrue(bulkEditPositionFrame.IsBulkEditQuantityFieldDisabledByRowOrder(rowOrderForPosition[0] + 1),
                    $"Quantity for {tickerForOption} is not disabled");
                Checker.IsTrue(bulkEditPositionFrame.IsBulkEditNotesFieldDisabledByRowOrder(rowOrderForPosition[0] + 1),
                    $"Notes for {tickerForOption} is not disabled");
                Checker.IsTrue(bulkEditPositionFrame.IsBulkEditTagsFieldDisabledByRowOrder(rowOrderForPosition[0] + 1),
                    $"Tags for {tickerForOption} is not disabled");
                Checker.CheckEquals(Constants.NotAvailableAcronym, bulkEditPositionFrame.GetName(rowOrderForPosition[0] + 1),
                    $"Name for {tickerForOption} is not as expected");
            }

            LogStep(6, "Remember Entry Date, Entry Price, Quantity values of all positions");
            var entryDatesBeforeEditingColumnValues = bulkEditPositionFrame.GetColumnValuesByDataFields(BulkEditColumnTypes.EntryDate)
                .Select(t => t.Trim()).ToList();
            var entryPricesBeforeEditingColumnValues = bulkEditPositionFrame.GetColumnValuesByDataFields(BulkEditColumnTypes.EntryPrice)
                .Select(t => t.Trim()).ToList();
            var quantityBeforeEditingColumnValues = bulkEditPositionFrame.GetColumnValuesByDataFields(BulkEditColumnTypes.Quantity)
                .Select(t => t.Trim()).ToList();

            LogStep(7, "Remove data (for not disabled fields) in Entry Date, Entry Price, Quantity fields for all positions");
            var positionsQuantity = bulkEditPositionFrame.GetCountOfPositions();
            for (int i = 1; i <= positionsQuantity; i++)
            {
                if (!bulkEditPositionFrame.IsBulkEditEntryDateFieldDisabledByRowOrder(i))
                {
                    bulkEditPositionFrame.BulkEditSetEntryDateByRowOrder(i, string.Empty);
                }
                if (!bulkEditPositionFrame.IsBulkEditEntryPriceFieldDisabledByRowOrder(i))
                {
                    bulkEditPositionFrame.BulkEditSetFieldByTypeRowOrder(BulkEditColumnTypes.EntryPrice, i, string.Empty);
                }
                if (!bulkEditPositionFrame.IsBulkEditQuantityFieldDisabledByRowOrder(i))
                {
                    bulkEditPositionFrame.BulkEditSetFieldByTypeRowOrder(BulkEditColumnTypes.Quantity, i, string.Empty);
                }
            }

            LogStep(8, "Click Save Changes. Select all displayed in the grid positions. Click the Bulk Edit button. Check that all changes haven't been saved");
            bulkEditPositionFrame.ClickSaveButton();
            positionsTab.SelectAllItemsInGrid();
            positionsTab.ClickGroupActionButton(PositionsGroupAction.BulkEdit);
            var entryDatesAfterEditingColumnValues = bulkEditPositionFrame.GetColumnValuesByDataFields(BulkEditColumnTypes.EntryDate)
                .Select(t => t.Trim()).ToList();
            var entryPricesAfterEditingColumnValues = bulkEditPositionFrame.GetColumnValuesByDataFields(BulkEditColumnTypes.EntryPrice)
                .Select(t => t.Trim()).ToList();
            var quantityAfterEditingColumnValues = bulkEditPositionFrame.GetColumnValuesByDataFields(BulkEditColumnTypes.Quantity)
                .Select(t => t.Trim()).ToList();
            Checker.CheckListsEquals(entryDatesBeforeEditingColumnValues, entryDatesAfterEditingColumnValues, "Entry Dates are not as expected");
            Checker.CheckListsEquals(entryPricesBeforeEditingColumnValues, entryPricesAfterEditingColumnValues, "Entry Prices are not as expected");
            Checker.CheckListsEquals(quantityBeforeEditingColumnValues, quantityAfterEditingColumnValues, "Quantities are not as expected");

            LogStep(9, "Specify Entry Date in the future (e.g. today + 1 day) for all positions. Make sure that Save Changes button is disabled " +
                "and Entry Date fields have red border");
            var entryDate = DateTime.Now.AddYears(1).ToShortDateString();
            for (int i = 1; i <= positionsQuantity; i++)
            {
                if (!bulkEditPositionFrame.IsBulkEditEntryDateFieldDisabledByRowOrder(i))
                {
                    bulkEditPositionFrame.BulkEditSetEntryDateByRowOrder(i, entryDate);
                }
            }
            Checker.IsTrue(bulkEditPositionFrame.IsSaveButtonDisabled(), "Save Button is not disabled after dates in future");

            LogStep(10, "Make sure that Notes and Tags are empty for all positions");
            var notesColumnValues = bulkEditPositionFrame.GetColumnValuesByDataFields(BulkEditColumnTypes.Notes);
            for (int i = 0; i < notesColumnValues.Count; i++)
            {
                Checker.CheckEquals(string.Empty, notesColumnValues[i], $"Notes for {tickersColumnValues[i]} is not empty");
            }

            LogStep(11, "Specify any valid new Entry Date for all positions. Specify  Entry Prices for positions. " +
                "Specify the following Quantities for positions. Specify the random Notes for all positions. Specify the random Tags for all positions.");
            entryDate = DateTime.Now.AddDays(DaysShiftForEntryDate).ToShortDateString();
            var actualNotes = new List<string>();
            var actualTags = new List<string>();
            for (int i = 1; i <= positionsQuantity; i++)
            {
                if (!bulkEditPositionFrame.IsBulkEditEntryDateFieldDisabledByRowOrder(i))
                {
                    bulkEditPositionFrame.BulkEditSetEntryDateByRowOrder(i, entryDate);
                }
                if (!bulkEditPositionFrame.IsBulkEditEntryPriceFieldDisabledByRowOrder(i))
                {
                    bulkEditPositionFrame.BulkEditSetFieldByTypeRowOrder(BulkEditColumnTypes.EntryPrice, i, pricesList[i - 1]);
                }
                if (!bulkEditPositionFrame.IsBulkEditQuantityFieldDisabledByRowOrder(i))
                {
                    bulkEditPositionFrame.BulkEditSetFieldByTypeRowOrder(BulkEditColumnTypes.Quantity, i, quantitiesList[i - 1]);
                }
                if (!bulkEditPositionFrame.IsBulkEditNotesFieldDisabledByRowOrder(i))
                {
                    actualNotes.Add(StringUtility.RandomStringOfSize(Constants.DefaultSizeOfDateStringToClearField));
                    bulkEditPositionFrame.BulkEditSetNotesByRowOrder(i, actualNotes.Last());
                }
                else
                {
                    actualNotes.Add(string.Empty);
                }
                if (!bulkEditPositionFrame.IsTagsEnabledForEditingByRowOrder(i))
                {
                    actualTags.Add(StringUtility.RandomStringOfSize(Constants.DefaultSizeOfDateStringToClearField));
                    bulkEditPositionFrame.BulkEditSetTagByRowOrder(i, actualTags.Last());
                }
                else
                {
                    actualTags.Add(string.Empty);
                }
            }
            bulkEditPositionFrame.ClickSaveButton();

            LogStep(12, "Select all displayed in the grid positions. Click the Bulk Edit button");
            positionsTab.SelectAllItemsInGrid();
            positionsTab.ClickGroupActionButton(PositionsGroupAction.BulkEdit);

            LogStep(13, "Select all displayed in the grid positions.Click the Bulk Edit button");
            for (int i = 1; i <= positionsQuantity; i++)
            {
                if (!bulkEditPositionFrame.IsBulkEditEntryDateFieldDisabledByRowOrder(i))
                {
                    Checker.CheckEquals(Parsing.ConvertToShortDateString(entryDate), Parsing.ConvertToShortDateString(bulkEditPositionFrame.BulkEditGetEntryDateByRowOrder(i)),
                        $"Entry date is not saved for {tickersColumnValues[i - 1]}");
                }
                if (!bulkEditPositionFrame.IsBulkEditEntryPriceFieldDisabledByRowOrder(i))
                {
                    Checker.CheckEquals(shownPricesList[i - 1],
                        StringUtility.ReplaceAllCurrencySigns(bulkEditPositionFrame.BulkEditGetEntryPriceByRowOrder(i)).Replace(",", string.Empty),
                        $"Entry price is not saved for {tickersColumnValues[i - 1]}");
                }
                if (!bulkEditPositionFrame.IsBulkEditQuantityFieldDisabledByRowOrder(i))
                {
                    Checker.CheckEquals(shownQuantitiesList[i - 1], bulkEditPositionFrame.BulkEditGetQuantityByRowOrder(i).Replace(",", string.Empty),
                        $"Quantity is not saved for {tickersColumnValues[i - 1]}");
                }
                Checker.CheckEquals(actualNotes[i - 1], bulkEditPositionFrame.BulkEditGetNotesByRowOrder(i), $"Notes is not saved for {tickersColumnValues[i - 1]}");
                if (!bulkEditPositionFrame.IsTagsEnabledForEditingByRowOrder(i))
                {
                    Checker.CheckListsEquals(new List<string> { actualTags[i - 1] }, bulkEditPositionFrame.GetTagsFromTextBox(i),
                    $"Tags is not saved for {tickersColumnValues[i - 1]}");
                }
            }
        }

        private List<int> GetRowOrderForPosition(List<string> tickersColumnValues, string ticker)
        {
            return tickersColumnValues.Select((t, i) => (i, t)).Where(s => s.t.EqualsIgnoreCase(ticker)).Select(s => s.i).ToList();
        }
    }
}