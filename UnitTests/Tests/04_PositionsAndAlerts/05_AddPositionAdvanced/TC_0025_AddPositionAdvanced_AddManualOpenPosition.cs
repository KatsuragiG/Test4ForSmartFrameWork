using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.PositionData;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Enums.AddPosition;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.PositionCard;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.User;
using AutomatedTests.Forms.AddPositionAdvanced;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Models;
using AutomatedTests.Models.PositionsModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.AddPositionAdvanced;
using AutomatedTests.Steps.PositionCard;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._04_PositionsAndAlerts._05_AddPositionAdvanced
{
    [TestClass]
    public class TC_0025_AddPositionAdvanced_AddManualOpenPosition : BaseTestUnitTests
    {
        private const int TestNumber = 25;
        private const int ExpectedItemsAddedQuantity = 1;

        private int expectedTagsAddedQuantity;
        private PortfolioModel portfolioModel;
        private AddPositionAdvancedModel positionModel;
        private readonly List<string> checkboxes = new List<string>
        {
            PositionsGridDataField.Tags.GetStringMapping(), PositionsGridDataField.Notes.GetStringMapping(), PositionsGridDataField.Commissions.GetStringMapping()
        };

        [TestInitialize]
        public void TestInitialize()
        {
            var userType = GetTestDataParsedAsEnumFromStringMapping<ProductSubscriptionTypes>("User");

            portfolioModel = new PortfolioModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency")
            };
            var entryCommission = GetTestDataAsString("entryCommission");
            positionModel = new AddPositionAdvancedModel
            {
                AssetType = GetTestDataParsedAsEnumFromStringMapping<PositionAssetTypes>("positionType"),
                EntryDate = GetTestDataAsString("entryDate"),
                Ticker = GetTestDataAsString("Symbol"),
                Shares = GetTestDataAsString("Shares"),
                IsLongTradeType = GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>("TradeType") == PositionTradeTypes.Long,
                IsAdjustByDividends = GetTestDataAsBool("adjust"),
                Tags = StringUtility.ConvertEmptyStringToNull(GetTestDataAsString("tags")),
                Notes = StringUtility.ConvertEmptyStringToNull(GetTestDataAsString("notes")),
                EntryPrice = StringUtility.ConvertEmptyStringToNull(GetTestDataAsString("entryPrice")),
                EntryCommission = string.IsNullOrEmpty(entryCommission) ? 0 : Parsing.ConvertToDecimal(entryCommission),
                IsOpenStatusType = true
            };
            expectedTagsAddedQuantity = string.IsNullOrEmpty(positionModel.Tags) ? 0 : 1;

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, (ProductSubscriptions)(int)userType));
            PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);

            LoginSetUp.LogIn(UserModels.First());

            var mainMenuNavigation = new MainMenuNavigation();
            mainMenuNavigation.OpenPositionsGrid();
            var positionTab = new PositionsTabForm();
            positionTab.ClickEditSignForCurrentView();
            positionTab.CheckCertainCheckboxes(checkboxes);
            positionTab.ClickSaveViewButton();
            mainMenuNavigation.OpenAddPositionAdvanced();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_25$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test ability adding a open manual position (Stock, Crypto, Index, Forex, Futures, Fund, Commodity, Warrant)" +
            "from Add Position Advanced https://tr.a1qa.com/index.php?/cases/view/19231904")]
        [TestCategory("Smoke"), TestCategory("PositionsGrid"), TestCategory("AddPositionPage")]
        public override void RunTest()
        {
            LogStep(1, "Fill in data on Add Position Advanced Mode");
            var addPositionAdvancedSteps = new AddPositionAdvancedSteps();
            addPositionAdvancedSteps.FillRequiredFieldsOnAddPositionAdvanced(positionModel);

            var addPositionAdvancedForm = new AddPositionAdvancedForm();
            var positionsQueries = new PositionsQueries();
            var currencySign = ((Currency)positionsQueries.SelectSymbolCurrencyBySymbol(positionModel.Ticker)).GetDescription();
            var expectedEntryPrice = string.IsNullOrEmpty(positionModel.EntryPrice)
                ? new PositionDataQueries().SelectHdAdjPriceForSymbolIdDate(positionModel.Ticker, positionModel.EntryDate).GetTradeAdjClose().ToString(CultureInfo.InvariantCulture)
                : positionModel.EntryPrice;
            var positionName = positionsQueries.SelectSymbolIdNameUsingSymbol(positionModel.Ticker).SymbolName;
            Checker.CheckEquals(positionModel.Ticker, addPositionAdvancedForm.GetSymbolTreeSelectSingleValue(), "Ticker is not as expected on Add Position Advanced");
            Checker.CheckEquals(positionName, addPositionAdvancedForm.GetValueFromTextBoxField(AddPositionAdvancedFields.Name),
                $"Name is not as expected on Add Position Advanced for {positionModel.Ticker}");
            Checker.CheckEquals(positionModel.AssetType, addPositionAdvancedForm.GetAssetType(), $"Asset Type is not as expected on Add Position Advanced for {positionModel.Ticker}");
            Checker.CheckEquals(positionModel.EntryDate, addPositionAdvancedForm.GetValueFromTextBoxField(AddPositionAdvancedFields.EntryDate),
                $"Entry Date is not as expected on Add Position Advanced for {positionModel.Ticker}");
            var actualEntryCommission = addPositionAdvancedForm.GetValueFromTextBoxField(AddPositionAdvancedFields.EntryCommission);
            Checker.CheckEquals(positionModel.EntryCommission.ToString(), StringUtility.ReplaceAllCurrencySigns(actualEntryCommission),
                $"Entry Commission is not as expected on Add Position Advanced for {positionModel.Ticker}");
            Checker.CheckEquals(currencySign, Constants.AllCurrenciesRegex.Match(actualEntryCommission).Value,
                $"Entry Commission currency sign is not as expected on Add Position Advanced for {positionModel.Ticker}");
            Checker.CheckEquals(positionModel.Shares, addPositionAdvancedForm.GetValueFromTextBoxField(AddPositionAdvancedFields.Shares).Replace(",", ""),
                $"Quantity is not as expected on Add Position Advanced for {positionModel.Ticker}");
            Checker.CheckEquals(positionModel.IsLongTradeType, addPositionAdvancedForm.IsBtnTradeTypeActive(PositionTradeTypes.Long),
                $"TradeType is not as expected on Add Position Advanced for {positionModel.Ticker}");
            Checker.CheckEquals(positionModel.IsOpenStatusType, addPositionAdvancedForm.IsBtnStatusTypeActive(AutotestPositionStatusTypes.Open),
                $"Status Type is not as expected on Add Position Advanced for {positionModel.Ticker}");
            Checker.CheckEquals(positionModel.IsAdjustByDividends, addPositionAdvancedForm.IsAdjustmentEnabled(),
                $"Adjust dividends is not as expected on Add Position Advanced for {positionModel.Ticker}");
            var actualEntryPrice = addPositionAdvancedForm.GetValueFromTextBoxField(AddPositionAdvancedFields.EntryPrice);
            Checker.CheckEquals(expectedEntryPrice, StringUtility.ReplaceAllCurrencySigns(actualEntryPrice).Replace(",", ""),
                $"Entry Price is not as expected on Add Position Advanced for {positionModel.Ticker}");
            Checker.CheckEquals(currencySign, Constants.AllCurrenciesRegex.Match(actualEntryPrice).Value,
                $"Entry Price currency sign is not as expected on Add Position Advanced for {positionModel.Ticker}");
            var actualTags = addPositionAdvancedForm.GetAllTags();
            Checker.CheckEquals(expectedTagsAddedQuantity, actualTags.Count,
                $"Tags quantity  is not as expected on Add Position Advanced for {positionModel.Ticker}");
            Checker.CheckEquals(positionModel.Tags, actualTags.Count == 0 ? null : actualTags.First(),
                $"Tag is not as expected on Add Position Advanced for {positionModel.Ticker}");
            Checker.CheckEquals(positionModel.Notes, StringUtility.ConvertEmptyStringToNull(addPositionAdvancedForm.GetValueFromTextBoxField(AddPositionAdvancedFields.Notes)),
                $"Notes is not as expected on Add Position Advanced for {positionModel.Ticker}");

            addPositionAdvancedForm.SelectPortfolio(portfolioModel.Name);
            Checker.CheckEquals(portfolioModel.Name, addPositionAdvancedForm.GetSelectedPortfolioName(),
                $"Selected Portfolio Name is not as expected on Add Position Advanced for {positionModel.Ticker}");

            LogStep(2, "Click Save button. Check data on Position Details");
            var positionCardForm = addPositionAdvancedSteps.ClickSaveButtonGetPositionCardForm();
            var positionDetailsTabPositionCardForm = positionCardForm.ActivateTabGetForm<PositionDetailsTabPositionCardForm>(PositionCardTabs.PositionDetails);
            Checker.CheckEquals(positionModel.Ticker, positionCardForm.GetSymbol(), "Symbol is not as expected on Position Card");
            Checker.CheckEquals(portfolioModel.Name, positionCardForm.GetPortfolioLinkText(),
                $"Portfolio Name is not as expected on Position Card for {positionModel.Ticker}");
            Checker.CheckEquals(positionModel.EntryDate, positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.EntryDate),
                $"Entry Date is not as expected on Position Card for {positionModel.Ticker}");

            var cardEntryPrice = positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.EntryPrice);
            Checker.CheckEquals(Math.Round(Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(expectedEntryPrice)), Constants.DefaultDecimalRounding),
                Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(cardEntryPrice)),
                $"Entry Price is not as expected on Position Card for {positionModel.Ticker}");
            Checker.CheckEquals(currencySign, Constants.AllCurrenciesRegex.Match(cardEntryPrice).Value,
                $"Entry Price currency sign is not as expected on Position Card for {positionModel.Ticker}");
            Checker.CheckEquals(Math.Round(Parsing.ConvertToDouble(positionModel.Shares), Constants.DefaultDecimalRounding).ToString("N2").Replace(",", ""),
                positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(positionDetailsTabPositionCardForm.GetSharesTypeFieldWithDetectingType())
                    .Replace("-", "").Replace(",", "").ToFractionalString(),
                $"Shares is not as expected on Position Card for {positionModel.Ticker}");

            var cardEntryCommission = positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.EntryCommission);
            Checker.CheckEquals(Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(actualEntryCommission)).ToString("N2"),
                Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(cardEntryCommission)).ToString("N2"),
                $"Entry Commission is not as expected on Position Card for {positionModel.Ticker}");
            Checker.CheckEquals(currencySign, Constants.AllCurrenciesRegex.Match(cardEntryCommission).Value,
                $"Entry Commission currency sign is not as expected on Position Card for {positionModel.Ticker}");
            Checker.CheckEquals(positionModel.IsLongTradeType, positionDetailsTabPositionCardForm.IsTradeTypeLong(), $"Trade Type is not as expected on Position Card for {positionModel.Ticker}");
            Checker.CheckEquals(positionModel.IsAdjustByDividends, positionDetailsTabPositionCardForm.GetAdjustAlertsByDividends(),
                $"Adjust dividends is not as expected on Position Card for {positionModel.Ticker}");

            var tagsNotesTabPositionCardForm = positionCardForm.ActivateTabGetForm<TagsNotesTabPositionCardForm>(PositionCardTabs.TagsAndNotes);
            var cardTags = tagsNotesTabPositionCardForm.GetAllTags().Where(t => !string.IsNullOrEmpty(t)).ToList();
            Checker.CheckEquals(expectedTagsAddedQuantity, cardTags.Count, $"Tags quantity is not as expected on Position Card for {positionModel.Ticker}");
            Checker.CheckEquals(positionModel.Tags, cardTags.Count == 0 ? null : cardTags.First(),
                $"Tag is not as expected on Position Card for {positionModel.Ticker}");
            Checker.CheckEquals(positionModel.Notes, StringUtility.ConvertEmptyStringToNull(tagsNotesTabPositionCardForm.GetNotes()),
                $"Notes is not as expected on Position Card for {positionModel.Ticker}");

            LogStep(3, "Click on the portfolio link for this Position Card");
            var positionTabForm = new PositionCardSteps().ClickOnPortfolioLinkGetPositionTabForm(PositionsTabs.OpenPositions);
            Checker.CheckEquals(ExpectedItemsAddedQuantity, positionTabForm.GetNumberOfRowsInGrid(), $"Positions quantity is not as expected on Position Grid for {positionModel.Ticker}");
            var tickerAndName = positionTabForm.GetPositionsGridCellValue(
                new TableCellMetrics { PositionOrder = ExpectedItemsAddedQuantity, ColumnHeader = PositionsGridDataField.Ticker.GetStringMapping() })
                    .Split('\r');
            Checker.CheckEquals(positionModel.Ticker, tickerAndName[0], "Ticker is not as expected on Position Grid");
            Checker.CheckEquals(positionName, tickerAndName[1].Replace("\n", ""), $"Name is not as expected on Position Grid for {positionModel.Ticker}");

            LogStep(4, "Check that position grid for position has expected values for Entry Date, Commission, Shares, L/S, Entry Price, Tags, Notes");
            Checker.CheckEquals(positionModel.EntryDate,
                positionTabForm.GetPositionsGridCellValue(
                    new TableCellMetrics { PositionOrder = ExpectedItemsAddedQuantity, ColumnHeader = PositionsGridDataField.EntryDate.GetStringMapping() }),
                $"Entry Date is not as expected on Position Grid for {positionModel.Ticker}");
            var gridCommission = positionTabForm.GetPositionsGridCellValue(
                    new TableCellMetrics { PositionOrder = ExpectedItemsAddedQuantity, ColumnHeader = PositionsGridDataField.Commissions.GetStringMapping() });
            Checker.CheckEquals(Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(actualEntryCommission)).ToString("N2"),
                Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(gridCommission)).ToString("N2"),
                $"Entry Commission is not as expected on Position Grid for {positionModel.Ticker}");
            Checker.CheckEquals(currencySign, Constants.AllCurrenciesRegex.Match(gridCommission).Value,
                $"Entry Commission currency sign is not as expected on Position Grid for {positionModel.Ticker}");
            Checker.CheckEquals(Math.Round(Parsing.ConvertToDouble(positionModel.Shares), Constants.DefaultDecimalRounding).ToString("N2").Replace(",", ""),
                positionTabForm.GetPositionsGridCellValue(
                    new TableCellMetrics { PositionOrder = ExpectedItemsAddedQuantity, ColumnHeader = PositionsGridDataField.Shares.GetStringMapping() })
                        .Replace("-", "").ToFractionalString(),
                $"Shares is not as expected on Position Grid for {positionModel.Ticker}");
            if (positionModel.IsLongTradeType.HasValue)
            {
                Checker.CheckEquals((bool)positionModel.IsLongTradeType ? PositionTradeTypes.Long.GetStringMapping() : PositionTradeTypes.Short.GetStringMapping(),
                    positionTabForm.GetPositionsGridCellValue(
                        new TableCellMetrics { PositionOrder = ExpectedItemsAddedQuantity, ColumnHeader = PositionsGridDataField.TradeType.GetStringMapping() }),
                    $"TradeType is not as expected on Position Grid for {positionModel.Ticker}");
            }

            var gridEntryPrice = positionTabForm.GetPositionsGridCellValue(
                    new TableCellMetrics { PositionOrder = ExpectedItemsAddedQuantity, ColumnHeader = PositionsGridDataField.EntryPrice.GetStringMapping() });
            Checker.CheckEquals(Math.Round(Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(expectedEntryPrice)), Constants.DefaultDecimalRounding),
                Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(gridEntryPrice)),
                $"Entry Price is not as expected on Position Grid for {positionModel.Ticker}");
            Checker.CheckEquals(currencySign, Constants.AllCurrenciesRegex.Match(gridEntryPrice).Value,
                $"Entry Price currency sign is not as expected on Position Grid for {positionModel.Ticker}");
            Checker.CheckEquals(positionModel.Tags,
                StringUtility.ConvertEmptyStringToNull(positionTabForm.GetPositionsGridCellValue(
                    new TableCellMetrics { PositionOrder = expectedTagsAddedQuantity, ColumnHeader = PositionsGridDataField.Tags.GetStringMapping() })),
                $"Tag is not as expected on Position Grid for {positionModel.Ticker}");
            Checker.CheckEquals(positionModel.Notes,
                StringUtility.ConvertEmptyStringToNull(positionTabForm.GetPositionsGridCellValue(
                    new TableCellMetrics { PositionOrder = ExpectedItemsAddedQuantity, ColumnHeader = PositionsGridDataField.Notes.GetStringMapping() })),
                $"Notes is not as expected on Position Grid for {positionModel.Ticker}");
        }
    }
}