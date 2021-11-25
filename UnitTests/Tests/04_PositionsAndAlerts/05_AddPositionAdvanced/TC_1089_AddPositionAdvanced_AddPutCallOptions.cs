using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Enums.AddPosition;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.PositionCard;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.User;
using AutomatedTests.Enums;
using AutomatedTests.Forms.AddPositionAdvanced;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Models.PositionsModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.AddPositionAdvanced;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Monads;
using System;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;
using System.Linq;

namespace UnitTests.Tests._04_PositionsAndAlerts._05_AddPositionAdvanced
{
    [TestClass]
    public class TC_1089_AddPositionAdvanced_AddPutCallOptions : BaseTestUnitTests
    {
        private const int TestNumber = 1089;
        private const int YearsToShowInClosedPositionsGrid = 5;
        private const string DefaultCommissionValue = "$0.00";
        private readonly List<string> columnsNamesToAddInView = new List<string>
        {
            PositionsGridDataField.Notes.GetStringMapping(), PositionsGridDataField.Tags.GetStringMapping(), PositionsGridDataField.Commissions.GetStringMapping()
        };
        private readonly List<PositionsGridDataField> columnsOpenPositionsNamesToGetDataFromGrid = new List<PositionsGridDataField>
        {
            PositionsGridDataField.Notes, PositionsGridDataField.Tags, PositionsGridDataField.Commissions, PositionsGridDataField.Shares,
            PositionsGridDataField.Ticker, PositionsGridDataField.EntryDate, PositionsGridDataField.EntryPrice, PositionsGridDataField.TradeType, PositionsGridDataField.Name
        };
        private readonly List<ClosedPositionsGridDataField> columnsClosedositionsNamesToGetDataFromGrid = new List<ClosedPositionsGridDataField>
        {
            ClosedPositionsGridDataField.Notes, ClosedPositionsGridDataField.Tags, ClosedPositionsGridDataField.Commissions, ClosedPositionsGridDataField.Shares,
            ClosedPositionsGridDataField.Ticker, ClosedPositionsGridDataField.EntryDate, ClosedPositionsGridDataField.EntryPrice, ClosedPositionsGridDataField.TradeType,
            ClosedPositionsGridDataField.ExitDate, ClosedPositionsGridDataField.ExitPrice, ClosedPositionsGridDataField.Name
        };

        private AddPositionAdvancedModel positionModel;
        private PositionsTabs currentTab;
        private bool isOptionVariantDisabled;
        private string optionVariant;
        private string optionName;
        private string expectedEntryPrice;
        private string expectedEntryPriceCard;
        private string expectedEntryCommission;
        private string expectedExitPrice;
        private string expectedExitPriceCard;
        private string expectedExitCommission;
        private string expectedExitCommissionCard;
        private string contractSize;

        [TestInitialize]
        public void TestInitialize()
        {
            var userType = GetTestDataParsedAsEnumFromStringMapping<ProductSubscriptionTypes>("User");

            var portfolioModel = new PortfolioModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency"),
                EntryCommission = GetTestDataAsString("entryPortfolioCommission"),
                ExitCommission = GetTestDataAsString("exitPortfolioCommission")
            };

            var contracts = GetTestDataAsString("Contracts");
            var statusType = GetTestDataAsString("statusType") == AutotestPositionStatusTypes.Open.GetStringMapping();
            var tags = GetTestDataAsString("Tags");
            var notes = GetTestDataAsString("Notes");
            var entryCommission = GetTestDataAsString("EntryCommission");
            var entryPrice = GetTestDataAsString("EntryPrice");
            optionName = GetTestDataAsString(nameof(optionName));

            positionModel = new AddPositionAdvancedModel
            {
                AssetType = PositionAssetTypes.Option,
                Ticker = GetTestDataAsString("Symbol"),
                ExpirationDate = GetTestDataAsString("ExpirationDate"),
                StrikePrice = GetTestDataAsString("ExpirationPrice"),
                OptionType = GetTestDataAsString("StrikeType"),
                Contracts = string.IsNullOrEmpty(contracts) ? null : contracts,
                IsLongTradeType = GetTestDataAsString("TradeType") == PositionTradeTypes.Long.GetStringMapping(),
                EntryCommission = string.IsNullOrEmpty(entryCommission) ? 0 : Parsing.ConvertToDecimal(entryCommission),
                IsOpenStatusType = statusType,
                Tags = string.IsNullOrEmpty(tags) ? null : tags,
                Notes = string.IsNullOrEmpty(notes) ? null : notes,
                Portfolio = portfolioModel.Name,
                EntryDate = GetTestDataAsString("EntryDate"),
                EntryPrice = string.IsNullOrEmpty(entryPrice) ? null : entryPrice
            };
            if (!statusType)
            {
                var exitCommission = GetTestDataAsString("ExitCommission");
                var exitPrice = GetTestDataAsString("ExitPrice");
                positionModel.ExitCommission = string.IsNullOrEmpty(exitCommission) ? null : exitCommission;
                positionModel.ExitPrice = string.IsNullOrEmpty(exitPrice) ? null : exitPrice;
                positionModel.ExitDate = GetTestDataAsString("ExitDate");
                expectedExitPrice = GetTestDataAsString(nameof(expectedExitPrice));
                expectedExitPriceCard = GetTestDataAsString(nameof(expectedExitPriceCard));
                expectedExitCommission = GetTestDataAsString(nameof(expectedExitCommission));
                expectedExitCommissionCard = GetTestDataAsString(nameof(expectedExitCommissionCard));
            }

            optionVariant = GetTestDataAsString(nameof(optionVariant));
            expectedEntryPrice = GetTestDataAsString(nameof(expectedEntryPrice));
            expectedEntryPriceCard = GetTestDataAsString(nameof(expectedEntryPriceCard));
            expectedEntryCommission = GetTestDataAsString(nameof(expectedEntryCommission));
            contractSize = GetTestDataAsString(nameof(contractSize));
            isOptionVariantDisabled = GetTestDataAsBool(nameof(isOptionVariantDisabled));

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, (ProductSubscriptions)(int)userType));

            new PortfolioGridsSteps().LoginCreatePortfolioViaDbGetPortfolioId(UserModels.First(), portfolioModel);
            currentTab = statusType ? PositionsTabs.OpenPositions : PositionsTabs.ClosedPositions;
            new MainMenuNavigation().OpenPositionsGrid(currentTab);
            if (statusType)
            {
                new PositionsTabForm().AddToCurrentViewColumns(columnsNamesToAddInView);
            }
            else
            {
                new ClosedPositionsTabForm().AddToCurrentViewColumns(columnsNamesToAddInView);
            }
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1089$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test checks correctness adding of Long/Short Call/Put Options with custom and default Entry Date Date" +
            "and Entry Price https://tr.a1qa.com/index.php?/cases/view/19232060")]
        [TestCategory("Smoke"), TestCategory("AddPositionPage"), TestCategory("PositionCard"), TestCategory("PositionCardTagsNotesTab"),
            TestCategory("PositionsGrid"), TestCategory("ClosedPositionCard")]
        public override void RunTest()
        {
            LogStep(1, "Open Add Position Advanced page ");
            new MainMenuNavigation().OpenAddPositionAdvanced();

            LogStep(2, "Click 'Option' button");
            var addPositionAdvancedForm = new AddPositionAdvancedForm();
            addPositionAdvancedForm.SelectPositionType(PositionAssetTypes.Option);
            Checker.IsTrue(addPositionAdvancedForm.IsOptionDropdownDisabled(SelectOptionDropdown.ExpirationDate), "Expiration Date is not disabled");
            Checker.CheckEquals(string.Empty, addPositionAdvancedForm.GetValueInOptionDropDown(SelectOptionDropdown.ExpirationDate), "Expiration Date is not empty");
            Checker.IsTrue(addPositionAdvancedForm.IsOptionDropdownDisabled(SelectOptionDropdown.StrikePrice), "Strike Price is not disabled");
            Checker.CheckEquals(string.Empty, addPositionAdvancedForm.GetValueInOptionDropDown(SelectOptionDropdown.StrikePrice), "Strike Price is not empty");
            Checker.IsTrue(addPositionAdvancedForm.IsOptionDropdownDisabled(SelectOptionDropdown.OptionType), "Option Type is not disabled");
            Checker.CheckEquals(string.Empty, addPositionAdvancedForm.GetValueInOptionDropDown(SelectOptionDropdown.OptionType), "Strike Type is not empty");
            Checker.IsTrue(addPositionAdvancedForm.IsOptionVariantDropdownDisabled(), "Option Variant is not disabled");

            LogStep(3, "Type and select parent symbol from autocomplete");
            addPositionAdvancedForm.SetSymbol(positionModel.Ticker);
            Checker.CheckEquals(positionModel.Ticker, addPositionAdvancedForm.GetSymbolTreeSelectSingleValue(), "Wrong Ticker is shown in autocomplete");
            Checker.IsFalse(addPositionAdvancedForm.IsOptionDropdownDisabled(SelectOptionDropdown.ExpirationDate), "Expiration Date is not enabled");
            Checker.IsFalse(addPositionAdvancedForm.IsOptionDropdownDisabled(SelectOptionDropdown.StrikePrice), "Strike Price is not enabled");
            Checker.IsFalse(addPositionAdvancedForm.IsOptionDropdownDisabled(SelectOptionDropdown.OptionType), "Option Type is not enabled");
            var prefilledExpirationDate = addPositionAdvancedForm.GetValueInOptionDropDown(SelectOptionDropdown.ExpirationDate);
            var prefilledStrikePrice = addPositionAdvancedForm.GetValueInOptionDropDown(SelectOptionDropdown.StrikePrice);
            var prefilledOptionType = addPositionAdvancedForm.GetValueInOptionDropDown(SelectOptionDropdown.OptionType);
            Checker.CheckNotEquals(string.Empty, prefilledExpirationDate, "Expiration Date is not prefilled after ticker selecting");
            Checker.CheckNotEquals(string.Empty, prefilledStrikePrice, "Strike Price is not prefilled after ticker selecting");
            Checker.CheckNotEquals(string.Empty, prefilledOptionType, "Strike Type is not prefilled after ticker selecting");
            var prefilledOptionName = addPositionAdvancedForm.GetValueFromTextBoxField(AddPositionAdvancedFields.Name);
            Checker.CheckContains(positionModel.Ticker, prefilledOptionName, "Parent Ticker is not included into Option Name");
            Checker.CheckContains(prefilledStrikePrice, prefilledOptionName, "Strike Price is not included into Option Name");
            Checker.CheckContains(prefilledOptionType, prefilledOptionName, "Option Type is not included into Option Name");
            Checker.CheckNotEquals(string.Empty, addPositionAdvancedForm.GetSharesPerContract(), "Contract Size is not prefilled after ticker selecting");

            LogStep(4, "Fill in Options parameters: Expiration Date, Strike Price, Option Type");
            addPositionAdvancedForm.SelectValueInOptionDropDown(SelectOptionDropdown.ExpirationDate, positionModel.ExpirationDate);
            addPositionAdvancedForm.SelectValueInOptionDropDown(SelectOptionDropdown.StrikePrice, positionModel.StrikePrice);
            addPositionAdvancedForm.SelectValueInOptionDropDown(SelectOptionDropdown.OptionType, positionModel.OptionType);
            Checker.CheckEquals(positionModel.ExpirationDate, addPositionAdvancedForm.GetValueInOptionDropDown(SelectOptionDropdown.ExpirationDate), "Expiration Date is not empty");
            Checker.CheckEquals(positionModel.StrikePrice, addPositionAdvancedForm.GetValueInOptionDropDown(SelectOptionDropdown.StrikePrice), "Strike Price is not empty");
            Checker.CheckEquals(positionModel.OptionType, addPositionAdvancedForm.GetValueInOptionDropDown(SelectOptionDropdown.OptionType), "Strike Type is not empty");
            var determinedOptionName = addPositionAdvancedForm.GetValueFromTextBoxField(AddPositionAdvancedFields.Name);
            Checker.CheckContains(positionModel.StrikePrice, determinedOptionName, "Strike Price is not included into Option Name after full option determining");
            Checker.CheckContains(positionModel.OptionType, determinedOptionName, "Option Type is not included into Option Name  after full option determining");
            Checker.CheckContains(optionVariant, addPositionAdvancedForm.GetOptionVariant(), "Option Variant is not matched expecteation");
            Checker.CheckEquals(isOptionVariantDisabled, addPositionAdvancedForm.IsOptionVariantDropdownDisabled(), "Option Variant state is not matched expecteation");

            LogStep(5, 6, "Fill in other parameters according to test data (if cell is empty - do not change prefilled value). Remember name and full option name");
            positionModel.Portfolio.Do(addPositionAdvancedForm.SelectPortfolio);
            positionModel.IsLongTradeType.Do(addPositionAdvancedForm.SelectTradeType);
            positionModel.IsOpenStatusType.Do(addPositionAdvancedForm.SelectStatus);
            positionModel.Contracts.Do(p => addPositionAdvancedForm.SetValueInTextBoxField(AddPositionAdvancedFields.Contracts, p));
            positionModel.EntryCommission.Do(p => addPositionAdvancedForm.SetValueInTextBoxField(AddPositionAdvancedFields.EntryCommission, p.ToString()));
            positionModel.EntryDate.Do(p => addPositionAdvancedForm.SetValueInDatePickerField(AddPositionAdvancedFields.EntryDate, p));
            positionModel.EntryPrice.Do(p => addPositionAdvancedForm.SetValueInTextBoxField(AddPositionAdvancedFields.EntryPrice, p));
            positionModel.ExitDate.Do(p => addPositionAdvancedForm.SetValueInDatePickerField(AddPositionAdvancedFields.CloseDate, p));
            positionModel.ExitPrice.Do(p => addPositionAdvancedForm.SetValueInTextBoxField(AddPositionAdvancedFields.ClosePrice, p));
            positionModel.ExitCommission.Do(p => addPositionAdvancedForm.SetValueInTextBoxField(AddPositionAdvancedFields.ExitCommission, p));
            positionModel.Tags.Do(addPositionAdvancedForm.SetTag);
            positionModel.Notes.Do(p => addPositionAdvancedForm.SetValueInTextBoxField(AddPositionAdvancedFields.Notes, p));

            var addPositionAdvancedSteps = new AddPositionAdvancedSteps();
            var currentAddPositionAdvancedModel = currentTab != PositionsTabs.OpenPositions 
                ? addPositionAdvancedSteps.GetCurrentAddPositionAdvancedClosedModel(PositionAssetTypes.Option) 
                : addPositionAdvancedSteps.GetCurrentAddPositionAdvancedOpenOptionModel();

            Checker.CheckEquals(positionModel.IsLongTradeType, currentAddPositionAdvancedModel.IsLongTradeType, "TradeType is not as expected on Add Position Advanced");
            Checker.CheckEquals(positionModel.IsOpenStatusType, currentAddPositionAdvancedModel.IsOpenStatusType, "Status Type is not as expected on Add Position Advanced");
            Checker.CheckEquals(positionModel.Contracts, currentAddPositionAdvancedModel.Contracts, "Contracts is not as expected on Add Position Advanced");
            Checker.CheckEquals(positionModel.EntryCommission, currentAddPositionAdvancedModel.EntryCommission, "Entry Commission is not as expected on Add Position Advanced");
            Checker.CheckEquals(positionModel.EntryDate, currentAddPositionAdvancedModel.EntryDate, "Entry Date is not as expected on Add Position Advanced");
            Checker.CheckEquals(expectedEntryPrice.TrimEnd('0'), currentAddPositionAdvancedModel.EntryPrice, "Entry Price is not as expected on Add Position Advanced");

            Checker.CheckEquals(positionModel.ExitDate, currentAddPositionAdvancedModel.ExitDate, "Exit Date is not as expected on Add Position Advanced");
            Checker.CheckEquals(expectedExitPrice, currentAddPositionAdvancedModel.ExitPrice, "Exit Price is not as expected on Add Position Advanced");
            Checker.CheckEquals(expectedExitCommission, currentAddPositionAdvancedModel.ExitCommission, "Exit Commission is not as expected on Add Position Advanced");
            Checker.CheckEquals(positionModel.IsOpenStatusType, currentAddPositionAdvancedModel.IsOpenStatusType, "Status Type is not as expected on Add Position Advanced");
            var actualTags = addPositionAdvancedForm.GetAllTags();
            Checker.CheckEquals(positionModel.Tags, actualTags.Count == 0 ? null : addPositionAdvancedForm.GetAllTags()[0], "Tags is not as expected on Add Position Advanced");
            var actualNotes = addPositionAdvancedForm.GetValueFromTextBoxField(AddPositionAdvancedFields.Notes);
            Checker.CheckEquals(positionModel.Notes, string.IsNullOrEmpty(actualNotes) ? null : actualNotes, 
                "Notes is not as expected on Add Position Advanced");
            Checker.CheckEquals(contractSize, addPositionAdvancedForm.GetSharesPerContract(), "Contract Size is not as expected on Add Position Advanced");

            LogStep(7, "Click 'Save and Close' button.");
            addPositionAdvancedSteps.ClickSaveButtonGetPositionCardForm();
            var positionId = addPositionAdvancedSteps.GetPositionIdFromPositionCard();

            LogStep(8, "Make sure values match expectation:- Ticker, Name, Expiration Date, Strike Price, Option Type, Option Variant;" +
                "Entry Date, Entry Price, Contracts, Entry Commission, Trade Type, Adjust alerts by dividends?");
            var positionCard = new PositionCardForm();
            var positionDetailsTabPositionCardForm = positionCard.ActivateTabWithoutChartWaitingGetForm<PositionDetailsTabPositionCardForm>(PositionCardTabs.PositionDetails);
            Checker.CheckEquals(currentAddPositionAdvancedModel.OptionVariant, positionCard.GetSymbol(), "Option full name is not as expected on Position Card");
            Checker.CheckEquals(optionName, positionCard.GetName(), "Name is not as expected on Position Card");
            Checker.CheckEquals(string.IsNullOrEmpty(currentAddPositionAdvancedModel.EntryDate) ? Constants.NotAvailableAcronym : currentAddPositionAdvancedModel.EntryDate,
                positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.EntryDate), "Entry Date is not as expected on Position Card");
            Checker.CheckEquals(expectedEntryPriceCard, positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.EntryPrice),
                "Entry Price is not as expected on Position Card");
            Checker.CheckEquals(currentAddPositionAdvancedModel.Contracts.ToFractionalString(), 
                positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.Contracts).Replace("-", string.Empty),
                "Contracts is not as expected on Position Card");
            Checker.CheckEquals(Math.Round(Parsing.ConvertToDouble(expectedEntryCommission), Constants.DefaultDecimalRounding),
                Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.EntryCommission))),
                "Entry Commission is not as expected on Position Card");
            Checker.CheckEquals(currentAddPositionAdvancedModel.IsLongTradeType, positionDetailsTabPositionCardForm.IsTradeTypeLong(), "Trade Type is not as expected on Position Card");
            if (currentTab != PositionsTabs.OpenPositions)
            {
                Checker.CheckEquals(string.IsNullOrEmpty(expectedExitCommission) ? DefaultCommissionValue : expectedExitCommissionCard, 
                    positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.ExitCommission),
                    "Exit Commission is not as expected on Position Card");
                Checker.CheckEquals(string.IsNullOrEmpty(currentAddPositionAdvancedModel.ExitDate) ? string.Empty : currentAddPositionAdvancedModel.ExitDate,
                    positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.ExitDate), "Exit Date is not as expected on Position Card");
                Checker.CheckEquals(expectedExitPriceCard, positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.ExitPrice),
                    "Exit Price is not as expected on Position Card");
            }

            LogStep(9, "Open tab 'Tags & Notes'.Make sure data match expectation for:-Tags;- Notes");
            var tagAndNotesForm = positionCard.ActivateTabGetForm<TagsNotesTabPositionCardForm>(PositionCardTabs.TagsAndNotes);
            var expectedTagOnPositionCard = string.IsNullOrEmpty(positionModel.Tags) ? string.Empty : positionModel.Tags;
            Checker.CheckEquals(expectedTagOnPositionCard, tagAndNotesForm.GetAllTagsAsString(), "Tags is not as expected on Position Card");
            var expectedNotesOnPositionCard = string.IsNullOrEmpty(actualNotes) ? actualNotes : tagAndNotesForm.GetNotes();
            Checker.CheckEquals(expectedNotesOnPositionCard, tagAndNotesForm.GetNotes(), "Notes is not as expected on Position Card");

            LogStep(10, "Open Positions & Alerts -> Positions page.Select Custom view.");            
            new MainMenuNavigation().OpenPositionsGrid(currentTab);
            if (currentTab != PositionsTabs.OpenPositions)
            {
                new ClosedPositionsTabForm().SelectCustomPeriodRangeWithStartEndDates(
                    Parsing.ConvertToShortDateString(DateTime.Now.AddYears(-YearsToShowInClosedPositionsGrid).AsShortDate()),
                    Parsing.ConvertToShortDateString(DateTime.Now.AsShortDate()));
            }

            LogStep(11, "Make sure values match expectation");
            var dataInGrid = currentTab == PositionsTabs.OpenPositions
                ? new PositionsTabForm().GetPositionDataByPositionId(columnsOpenPositionsNamesToGetDataFromGrid, positionId)
                : new ClosedPositionsTabForm().GetPositionDataByPositionId(columnsClosedositionsNamesToGetDataFromGrid, positionId);
            Checker.CheckEquals(currentAddPositionAdvancedModel.OptionVariant, dataInGrid.Ticker, "Option full name is not as expected on Positions tab");
            Checker.CheckEquals(currentAddPositionAdvancedModel.Name, dataInGrid.Name, "Name is not as expected on Positions tab");
            Checker.CheckEquals(currentAddPositionAdvancedModel.EntryDate, dataInGrid.EntryDate, "Entry Date is not as expected on Positions tab");
            Checker.CheckEquals(expectedEntryPriceCard, dataInGrid.EntryPrice, "Entry Price is not as expected on Positions tab");
            var expectedShares = Parsing.ConvertToDouble(currentAddPositionAdvancedModel.Contracts) * Parsing.ConvertToDouble(Constants.NumbersRegex.Match(contractSize).Value);
            Checker.CheckEquals(expectedShares.ToString("N2"), dataInGrid.Shares.Replace("-", string.Empty), "Shares is not as expected on Positions tab");
            var sumOfCommissions = Math.Round(Parsing.ConvertToDouble(currentAddPositionAdvancedModel.EntryCommission.ToString()) +
                (string.IsNullOrEmpty(expectedExitCommissionCard) 
                    ? 0 
                    : Parsing.ConvertToDouble(Constants.DecimalNumberWithIntegerPossibilityRegex.Match(currentAddPositionAdvancedModel.ExitCommission).Value)
                ), Constants.DefaultDecimalRounding);
            Checker.CheckEquals(sumOfCommissions, Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(dataInGrid.Commissions)),
                "Commissions is not as expected on Positions tab");
            Checker.CheckEquals(currentAddPositionAdvancedModel.IsLongTradeType, dataInGrid.TradeType.Equals(PositionTradeTypes.Long.ToString()),
                "Trade Type is not as expected on Positions tab");
            Checker.CheckEquals(expectedTagOnPositionCard, dataInGrid.Tags, "Tags is not as expected on Positions tab");
            Checker.CheckEquals(expectedNotesOnPositionCard, dataInGrid.Notes, "Notes is not as expected on Positions tab");
            if (currentTab != PositionsTabs.OpenPositions)
            {
                Checker.CheckEquals(string.IsNullOrEmpty(currentAddPositionAdvancedModel.ExitDate) ? string.Empty : currentAddPositionAdvancedModel.ExitDate,
                   dataInGrid.ExitDate, "Exit Date is not as expected on Positions tab");
                Checker.CheckEquals(expectedExitPriceCard, dataInGrid.ExitPrice, "Exit Price is not as expected on Positions tab");
            }
        }
    }
}