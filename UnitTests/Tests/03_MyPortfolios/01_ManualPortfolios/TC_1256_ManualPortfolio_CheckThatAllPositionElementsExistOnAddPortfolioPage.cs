using System.Collections.Generic;
using System.Linq;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using TradeStops.Common.Extensions;
using TradeStops.Common.Utils;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._03_MyPortfolios._01_ManualPortfolios
{
    [TestClass]
    public class TC_1256_ManualPortfolio_CheckThatAllPositionElementsExistOnAddPortfolioPage : BaseTestUnitTests
    {
        private const int TestNumber = 1256;
        private const int NumberRowPosition = 1;

        private PositionAtManualCreatingPortfolioModel positionCreationModel;
        private Dictionary<PositionForManualPortfolioCreateInformation, bool> positionCreationFieldsVisibleStates;
        private Dictionary<SelectOptionDropdown, bool> optionFieldsVisibleStates;
        private Dictionary<SelectOptionDropdown, bool> optionFieldsDisabledStates;
        private string addStocksHeaderName;
        private string tooltipText;
        private string enteredEntryDate;
        private string expectedEntryPrice;

        [TestInitialize]
        public void TestInitialize()
        {
            addStocksHeaderName = GetTestDataAsString("AddStocksHeaderName");
            tooltipText = GetTestDataAsString("TooltipText");
            enteredEntryDate = GetTestDataAsString("EntryDate");
            expectedEntryPrice = GetTestDataAsString("EntryPrice");

            positionCreationModel = new PositionAtManualCreatingPortfolioModel
            {
                PositionAssetType = GetTestDataParsedAsEnumFromStringMapping<PositionAssetTypes>("Type"),
                Ticker = GetTestDataAsString("ValueField1"),
                ExpirationDate = GetTestDataAsString("ValueField2"),
                StrikePrice = GetTestDataAsString("ValueField3"),
                OptionType = GetTestDataAsString("ValueField4"),
                TradeType = GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>("ValueField8"),
            };

            positionCreationFieldsVisibleStates = new Dictionary<PositionForManualPortfolioCreateInformation, bool>
            {
                { PositionForManualPortfolioCreateInformation.Type, GetTestDataAsBool("DisabledField1")},
                { PositionForManualPortfolioCreateInformation.Ticker, GetTestDataAsBool("DisabledField1")},
                { PositionForManualPortfolioCreateInformation.EntryDate, GetTestDataAsBool("DisabledField5")},
                { PositionForManualPortfolioCreateInformation.EntryPrice, GetTestDataAsBool("DisabledField6")},
                { PositionForManualPortfolioCreateInformation.Quantity, GetTestDataAsBool("DisabledField7")},
                { PositionForManualPortfolioCreateInformation.PositionType, GetTestDataAsBool("DisabledField8")}
            };

            optionFieldsVisibleStates = new Dictionary<SelectOptionDropdown, bool>
            {
                { SelectOptionDropdown.ExpirationDate, GetTestDataAsBool("ExistField2")},
                { SelectOptionDropdown.StrikePrice, GetTestDataAsBool("ExistField3")},
                { SelectOptionDropdown.OptionType, GetTestDataAsBool("ExistField4")}
            };

            optionFieldsDisabledStates = new Dictionary<SelectOptionDropdown, bool>
            {
                { SelectOptionDropdown.ExpirationDate, GetTestDataAsBool("DisabledField2")},
                { SelectOptionDropdown.StrikePrice, GetTestDataAsBool("DisabledField3")},
                { SelectOptionDropdown.OptionType, GetTestDataAsBool("DisabledField4")}
            };

            LogStep(0, "Precondition - Login as registered user");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPlatinum));
            LoginSetUp.LogIn(UserModels.First());
            new AddPortfoliosSteps().OpenPortfolioCreationFormViaSelectionFlowPage(AddPortfolioTypes.Manual);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1256$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("AddPositionPage"), TestCategory("AddPortfolio")]
        [Description("Test for existing of all elements on the Add Portfolio page and its correct default state: https://tr.a1qa.com/index.php?/cases/view/19243133")]
        public override void RunTest()
        {
            LogStep(1, "Check that 'Add Stocks to your Portfolio:' label is displayed in positions creation area");
            var manualPortfolioCreationForm = new ManualPortfolioCreationForm();
            Checker.IsTrue(manualPortfolioCreationForm.IsPortfolioHeaderPresent(addStocksHeaderName),
                $"'{addStocksHeaderName}' label is not displayed");

            LogStep(2, "Check that Type drop-down and appropriate label are displayed");
            var labelLocation = manualPortfolioCreationForm.GetPositionLabelLocation(PositionForManualPortfolioCreateInformation.Type.GetDescription());
            var fieldLocation = manualPortfolioCreationForm.GetPositionTypeDropdownLocation(NumberRowPosition);
            Checker.IsTrue(manualPortfolioCreationForm.IsPositionTypeDropdownPresent(NumberRowPosition),
                $"Type drop-down is not displayed in row {NumberRowPosition}");
            Checker.IsFalse(manualPortfolioCreationForm.IsPositionTypeDropdownDisabled(NumberRowPosition),
                $"Type drop-down is disabled in row {NumberRowPosition}");
            Checker.IsTrue(manualPortfolioCreationForm.IsPositionLabelPresent(PositionForManualPortfolioCreateInformation.Type.GetDescription()),
                $"{PositionForManualPortfolioCreateInformation.Type.GetDescription()} label is not displayed");
            Checker.IsTrue(labelLocation.Y < fieldLocation.Y,
                $"Label to the under of the field. Label: {labelLocation.X}-{labelLocation.Y}. Field: {fieldLocation.X}-{fieldLocation.Y}");

            LogStep(3, "Check that position Type drop - down contains all necessary items: Stock; Option;  Crypto.");
            var availableItems = manualPortfolioCreationForm.GetPositionTypeDropdownItems(NumberRowPosition);
            foreach (var positionType in EnumUtils.GetValues<PositionAssetTypes>())
            {
                Checker.IsTrue(availableItems.Contains(positionType.GetStringMapping()),
                    $"Position Type dropdown does not contain item {positionType.GetStringMapping()}");
            }
            manualPortfolioCreationForm.SetPositionType(positionCreationModel.PositionAssetType.GetStringMapping(), NumberRowPosition);

            LogStep(4, "Check that Ticker field and appropriate label are displayed");
            CheckPositionLabelAndFieldLocationIfExist(PositionForManualPortfolioCreateInformation.Ticker);
            CheckPositionLabelAndFieldLocationIfExist(PositionForManualPortfolioCreateInformation.EntryDate);
            CheckPositionLabelAndFieldLocationIfExist(PositionForManualPortfolioCreateInformation.EntryPrice);
            CheckPositionLabelAndFieldLocationIfExist(PositionForManualPortfolioCreateInformation.Quantity);

            Checker.IsTrue(manualPortfolioCreationForm.IsPositionLabelPresent(PositionForManualPortfolioCreateInformation.PositionType.GetDescription()),
                $"{PositionForManualPortfolioCreateInformation.PositionType.GetDescription()} label is not displayed");
            Checker.CheckEquals(positionCreationFieldsVisibleStates[PositionForManualPortfolioCreateInformation.PositionType],
                manualPortfolioCreationForm.IsPositionRadioButtonDisabled(PositionForManualPortfolioCreateInformation.PositionType, PositionTradeTypes.Long, NumberRowPosition),
                $"{PositionForManualPortfolioCreateInformation.PositionType.GetDescription()} Field state {PositionTradeTypes.Long} is not as expected");
            Checker.CheckEquals(positionCreationFieldsVisibleStates[PositionForManualPortfolioCreateInformation.PositionType],
                manualPortfolioCreationForm.IsPositionRadioButtonDisabled(PositionForManualPortfolioCreateInformation.PositionType, PositionTradeTypes.Short, NumberRowPosition),
                $"{PositionForManualPortfolioCreateInformation.PositionType.GetDescription()} Field state {PositionTradeTypes.Short} is not as expected");

            CheckOptionsLabelAndIfExist();

            LogStep(5, "Check that Delete icon is absent until a Ticker hasn't been selected");
            Checker.IsFalse(manualPortfolioCreationForm.IsPositionsDeleteButtonPresent(NumberRowPosition), "Delete button is displayed");

            LogStep(6, "Enter Ticker and select the Ticker from autocomplete");
            manualPortfolioCreationForm.SetSymbol(positionCreationModel.Ticker, NumberRowPosition);
            Checker.CheckEquals(positionCreationModel.Ticker,
                manualPortfolioCreationForm.GetTextFromPositionsAutocompleteDataFields(PositionForManualPortfolioCreateInformation.Ticker, NumberRowPosition),
                $"'{PositionForManualPortfolioCreateInformation.Ticker}' is not as expected at step 6");

            Checker.CheckEquals(string.Empty,
                manualPortfolioCreationForm.GetTextFromPositionsDataFields(PositionForManualPortfolioCreateInformation.EntryDate, NumberRowPosition),
                $"'{PositionForManualPortfolioCreateInformation.EntryDate}' is not as expected at step 6");
            var actualDefaulEntryPrice = manualPortfolioCreationForm.GetTextFromPositionsDataFields(PositionForManualPortfolioCreateInformation.EntryPrice, NumberRowPosition);
            Checker.IsTrue(actualDefaulEntryPrice.EqualsIgnoreCase(string.Empty) || StringUtility.ReplaceAllCurrencySigns(actualDefaulEntryPrice).EqualsIgnoreCase(0.ToString()),
                $"'{PositionForManualPortfolioCreateInformation.EntryPrice}' is not as expected at step 6: {actualDefaulEntryPrice} when expected empty or zero");
            Checker.CheckEquals(string.Empty,
                manualPortfolioCreationForm.GetTextFromPositionsDataFields(PositionForManualPortfolioCreateInformation.Quantity, NumberRowPosition),
                $"'{PositionForManualPortfolioCreateInformation.Quantity}' is not as expected at step 6");
            Checker.CheckEquals(positionCreationModel.TradeType,
                manualPortfolioCreationForm.GetSelectedTradeTypeByOrder(NumberRowPosition).ParseAsEnumFromStringMapping<PositionTradeTypes>(),
                $"'{PositionForManualPortfolioCreateInformation.PositionType}' is not as expected at step 6");

            if (optionFieldsVisibleStates[SelectOptionDropdown.ExpirationDate])
            {
                Checker.CheckEquals(positionCreationModel.ExpirationDate,
                    manualPortfolioCreationForm.GetExpirationDate(NumberRowPosition),
                    "Expiration Date is not as expected at step 6");
                Checker.CheckEquals(positionCreationModel.StrikePrice,
                    manualPortfolioCreationForm.GetStrikePrice(NumberRowPosition),
                    "Strike Price is not as expected at step 6");
                Checker.CheckEquals(positionCreationModel.OptionType,
                    manualPortfolioCreationForm.GetOptionType(NumberRowPosition),
                    "Option Type is not as expected at step 6");
            }
            new AddPortfoliosSteps().CheckThatPositionFieldsDisabledByNumberRowPosition(NumberRowPosition);

            Checker.CheckEquals(NumberRowPosition + 1, manualPortfolioCreationForm.GetCountOfPositions(), "Count of positions is not as expected");

            LogStep(7, "Check that Delete icon is displayed.");
            Checker.IsTrue(manualPortfolioCreationForm.IsPositionsDeleteButtonPresent(NumberRowPosition), "Delete button is displayed");

            LogStep(8, "Point to the Delete icon.");
            manualPortfolioCreationForm.FocusToDeleteButton(NumberRowPosition);
            var isTooltipPresent = manualPortfolioCreationForm.IsDeleteTooltipPresent();
            if (isTooltipPresent)
            {
                Checker.CheckEquals(tooltipText, manualPortfolioCreationForm.GetDeleteTooltipText(), "Tooltip is not as expected");
            }

            LogStep(9, "Click the Entry Date field");
            manualPortfolioCreationForm.ClickPositionsField(PositionForManualPortfolioCreateInformation.EntryDate, NumberRowPosition);
            Checker.IsTrue(manualPortfolioCreationForm.IsEntryDateCalendarPresent(), "Entry Date calendar is not present");

            LogStep(10, "Enter Entry Date");
            manualPortfolioCreationForm.SetTextInPositionsDataFieldsByRowOrder(PositionForManualPortfolioCreateInformation.EntryDate, enteredEntryDate, NumberRowPosition);
            manualPortfolioCreationForm.ClickPositionsField(PositionForManualPortfolioCreateInformation.Quantity, NumberRowPosition);
            Checker.CheckEquals(enteredEntryDate,
                manualPortfolioCreationForm.GetTextFromPositionsDataFields(PositionForManualPortfolioCreateInformation.EntryDate, NumberRowPosition),
                "'Entry Date' is not as expected at step 10");
            Checker.CheckEquals(expectedEntryPrice,
                manualPortfolioCreationForm.GetTextFromPositionsDataFields(PositionForManualPortfolioCreateInformation.EntryPrice, NumberRowPosition),
                "'Entry Price' is not as expected at step 10");

            LogStep(11, "Click Short radio-button");
            SelectTradeTypeAndCheckSelecting(PositionTradeTypes.Short);

            LogStep(12, "Click Long radio-button");
            SelectTradeTypeAndCheckSelecting(PositionTradeTypes.Long);
        }

        private void SelectTradeTypeAndCheckSelecting(PositionTradeTypes type)
        {
            var manualPortfolioCreationForm = new ManualPortfolioCreationForm();
            manualPortfolioCreationForm.ClickPositionsTypeCheckbox(PositionForManualPortfolioCreateInformation.PositionType, type, NumberRowPosition);
            var selectedTradeType = manualPortfolioCreationForm.GetSelectedTradeTypeByOrder(NumberRowPosition);
            Checker.CheckEquals(type.GetStringMapping(), selectedTradeType, $"{type} is not selected");
        }

        private void CheckOptionsLabelAndIfExist()
        {
            var manualPortfolioCreationForm = new ManualPortfolioCreationForm();
            foreach (var optionDropdownType in EnumUtils.GetValues<SelectOptionDropdown>())
            {
                Checker.CheckEquals(optionFieldsVisibleStates[optionDropdownType],
                    manualPortfolioCreationForm.IsOptionDropdownPresent(optionDropdownType, NumberRowPosition),
                    $"{optionDropdownType.GetStringMapping()} existing Field state is not as expected");

                if (optionFieldsVisibleStates[optionDropdownType])
                {
                    Checker.CheckEquals(optionFieldsDisabledStates[optionDropdownType],
                        manualPortfolioCreationForm.IsOptionDropdownDisabled(optionDropdownType, NumberRowPosition),
                        $"{optionDropdownType.GetStringMapping()} disabling Field state is not as expected");
                }                
            }
        }

        private void CheckPositionLabelAndFieldLocationIfExist(PositionForManualPortfolioCreateInformation positionType)
        {
            var manualPortfolioCreationForm = new ManualPortfolioCreationForm();
            var isLabelPresent = manualPortfolioCreationForm.IsPositionLabelPresent(positionType.GetDescription());
            var isFieldPresent = manualPortfolioCreationForm.IsPositionDataTextBoxPresent(positionType, NumberRowPosition);
            Checker.IsTrue(isLabelPresent, $"{positionType.GetDescription()} label is not displayed");
            Checker.IsTrue(isFieldPresent, $"{positionType.GetDescription()} field is not displayed");
            if (isLabelPresent && isFieldPresent)
            {
                var labelLocation = manualPortfolioCreationForm.GetPositionLabelLocation(positionType.GetDescription());
                var fieldLocation = manualPortfolioCreationForm.GetPositionFieldLocation(positionType, NumberRowPosition);

                Checker.IsTrue(labelLocation.Y < fieldLocation.Y,
                    $"Label to the under of the field. Label: {labelLocation.X}-{labelLocation.Y}. Field: {fieldLocation.X}-{fieldLocation.Y}");

                Checker.CheckEquals(positionCreationFieldsVisibleStates[positionType],
                   manualPortfolioCreationForm.IsPositionDataTextBoxDisabled(positionType, NumberRowPosition),
                   $"{positionType.GetDescription()} Field state is not as expected");
            }
        }
    }
}