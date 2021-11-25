using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.DsiForSymbols;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.PositionData;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.AddPosition;
using AutomatedTests.Enums.Alerts;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.PositionCard;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.Tools;
using AutomatedTests.Enums;
using AutomatedTests.Forms.AddPositionAdvanced;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Forms.ResearchPages.PositionSizeForm;
using AutomatedTests.Forms;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.AddPositionAdvanced;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._07_Invest._05_PositionSize
{
    [TestClass]
    public class TC_1368_Invest_PositionSize_DollarInvestmentRiskWidgetWorksAsExpected : BaseTestUnitTests
    {
        private const int TestNumber = 1368;

        private PortfolioModel portfolioModel;
        private PositionsDBModel positionModel;
        private int portfolioId;

        private string ticker;
        private PositionTradeTypes tradeType;
        private PositionAssetTypes tickerAssetType;

        private string expectedCurrencySign;
        private string expectedInvestWordingForDollarInvestmentRisk;
        private string expectedInvestmentAmountValue;
        private string expectedInvestSectionLabel;
        private string expectedPurchasePriceWording;
        private string expectedAdditionalVqRisk;
        private string expectedAdditionalTsRisk;
        private string expectedAmount;
        private string expectedRiskPercent;
        private string expectedSharesAction;

        private int expectedDollarExplanationsQuantity;
        private readonly List<string> expectedDollarInvestmentExplanations = new List<string>();

        private readonly PositionDataQueries positionDataQueries = new PositionDataQueries();
        private readonly PositionsQueries positionsQueries = new PositionsQueries();

        [TestInitialize]
        public void TestInitialize()
        {
            var userProductSubscriptions = GetUserProductSubscriptions("userSubscription");

            portfolioModel = new PortfolioModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency")
            };
            tradeType = GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>(nameof(tradeType));
            positionModel = new PositionsDBModel
            {
                StatusType = GetTestDataAsString("DbStatus"),
                Symbol = GetTestDataAsString("SymbolBefore"),
                TradeType = ((int)tradeType).ToString(),
                Shares = GetTestDataAsString("Shares")
            };

            ticker = GetTestDataAsString(nameof(ticker));
            tickerAssetType = GetTestDataParsedAsEnumFromStringMapping<PositionAssetTypes>(nameof(tickerAssetType));

            expectedCurrencySign = ((Currency)positionsQueries.SelectSymbolCurrencyBySymbol(ticker)).GetDescription();
            expectedInvestSectionLabel = string.Format(GetTestDataAsString(nameof(expectedInvestSectionLabel)), ticker);
            expectedInvestmentAmountValue = $"{expectedCurrencySign}{GetTestDataAsString(nameof(expectedInvestmentAmountValue))}";
            expectedPurchasePriceWording = GetTestDataAsString(nameof(expectedPurchasePriceWording));
            expectedSharesAction = GetTestDataAsString(nameof(expectedSharesAction));

            expectedAdditionalVqRisk = GetTestDataAsString(nameof(expectedAdditionalVqRisk));
            expectedAdditionalTsRisk = expectedAdditionalVqRisk.Replace(TrailingStopAlertTypes.Vq.ToString().ToUpper(),
                TrailingStopAlertTypes.Ts.ToString().ToUpper());

            expectedAmount = GetTestDataAsString(nameof(expectedAmount));
            expectedRiskPercent = GetTestDataAsString(nameof(expectedRiskPercent));

            expectedInvestWordingForDollarInvestmentRisk = GetTestDataAsString(nameof(expectedInvestWordingForDollarInvestmentRisk));
            expectedDollarExplanationsQuantity = GetTestDataAsInt(nameof(expectedDollarExplanationsQuantity));
            for (int i = 1; i <= expectedDollarExplanationsQuantity; i++)
            {
                expectedDollarInvestmentExplanations.Add(GetTestDataAsString($"{nameof(expectedDollarInvestmentExplanations)}{i}"));
            }

            LogStep(0, "Precondition; Login");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));

            portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            PositionsAlertsSetUp.AddPositionViaDB(portfolioId, positionModel);

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Invest);
            new MainMenuNavigation().OpenPositionSize();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1368$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("ResearchPage"), TestCategory("PositionSize"), TestCategory("AddPositionPage"), TestCategory("PositionCard")]
        [Description("Test to check that Dollar Investment Risk option works as expected. https://tr.a1qa.com/index.php?/cases/view/19234226")]
        public override void RunTest()
        {
            LogStep(1, "Enter symbol in the 'Search for Ticker' field.");
            var positionSizeForm = new PositionSizeCalculatorForm();
            positionSizeForm.SelectSource(PositionSizeSourceTypes.IndividualSecurities);
            positionSizeForm.SetPositionType(tickerAssetType.GetStringMapping(), Constants.DefaultOrderOfSameItemsToReturn);
            positionSizeForm.SetSymbol(ticker, Constants.DefaultOrderOfSameItemsToReturn);
            Checker.CheckEquals(ticker,
                positionSizeForm.GetTextFromPositionsAutocompleteDataFields(PositionForManualPortfolioCreateInformation.Ticker, Constants.DefaultOrderOfSameItemsToReturn),
                "Ticker in auto-complete is not as expected");
            Assert.IsTrue(positionSizeForm.IsCalculatePositionSizeButtonPresent(), "Calculate Position Size Button is not as expected");

            LogStep(2, "Enter symbol in the 'Search for Ticker' field.");
            positionSizeForm.ClickPositionsTypeCheckbox(PositionForManualPortfolioCreateInformation.PositionType, tradeType, Constants.DefaultOrderOfSameItemsToReturn);
            Checker.CheckEquals(tradeType, positionSizeForm.GetSelectedTradeTypeByOrder(Constants.DefaultOrderOfSameItemsToReturn), "Trade Type is not as expected");
            positionSizeForm.ClickCalculatePositionSize();

            LogStep(3, "Check Invest Section Label.");
            Checker.CheckEquals(expectedInvestSectionLabel, positionSizeForm.GetInvestSectionLabelWording(),
                $"Invest section label is not as expected for {ticker}");

            LogStep(4, "Check 'Dollar Investment Risk' section.");
            var investmentOptionSection = new InvestmentOptionSection();
            Checker.IsTrue(investmentOptionSection.IsInvestmentOptionResultPresent(PositionSizeInvestmentOptionTypes.DollarInvestmentRisk),
                $"Dollar Investment Risk is not shown for {ticker}");
            Checker.CheckContains(expectedInvestWordingForDollarInvestmentRisk,
                investmentOptionSection.GetInvestmentInputLabelForInvestmentOptionLabelText(PositionSizeInvestmentOptionTypes.DollarInvestmentRisk),
                $"Dollar Investment Risk Input has unexpected Label for {ticker}");

            Checker.CheckEquals(expectedInvestmentAmountValue, investmentOptionSection.GetInvestmentAmountValue(),
                $"Dollar Investment Risk Input has unexpected prefilled Value for {ticker}");
            Checker.IsTrue(investmentOptionSection.IsApplyInvestmentAmountButtonPresent(),
                $"Apply Investment Amount button is not present for {ticker}");

            var actualInvestmentAmount = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                    PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                    PositionSizeVarietyTypes.YourInvestmentAmount,
                    PositionSizeMainNumberTypes.Amount);
            Checker.CheckEquals($"{expectedInvestmentAmountValue}.00",
                actualInvestmentAmount, $"Dollar Investment Risk - Your Investment Amount - Main Value is not as expected for {ticker}");
            Checker.CheckContains(PositionSizeVarietyTypes.YourInvestmentAmount.GetStringMapping(),
                investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                    PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                    PositionSizeVarietyTypes.YourInvestmentAmount,
                    PositionSizeMainNumberTypes.Description),
                $"Dollar Investment Risk - Your Investment Amount - Description is not as expected for {ticker}");

            var actualRiskAtInvestmentAmount = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                PositionSizeVarietyTypes.YourAmountAtRisk,
                PositionSizeMainNumberTypes.Amount);
            var riskMoneyValueRegex = new Regex(Constants.RiskValuePattern);
            Checker.IsFalse(string.IsNullOrEmpty(riskMoneyValueRegex.Match(actualRiskAtInvestmentAmount).Value),
                $"Dollar Investment Risk - Your Amount at Risk - Main Value is not as expected for {ticker}");
            Checker.CheckContains(PositionSizeVarietyTypes.YourAmountAtRisk.GetStringMapping(),
                investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                    PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeMainNumberTypes.Description),
                $"Dollar Investment Risk - Your Amount at Risk - Description is not as expected for {ticker}");
            var vqValue = Parsing.ConvertToDouble(
                positionsQueries.SelectCurrentVqBySymbolId(
                    Parsing.ConvertToInt(positionsQueries.SelectSymbolIdNameUsingSymbol(ticker).SymbolId)));
            var actualRiskPercent = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                    PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeMainNumberTypes.AdditionalOptionOrContractSize);
            Checker.CheckContains(string.Format(expectedAdditionalVqRisk, vqValue.ToString("#.00")), actualRiskPercent,
                $"Dollar Investment Risk - Your Amount at Risk - Additional Description is not as expected for {ticker}");

            var actualSharesToBuy = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                    PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                    PositionSizeVarietyTypes.SharesToBuy,
                    PositionSizeMainNumberTypes.Amount);
            Checker.IsFalse(string.IsNullOrEmpty(Constants.NumbersWithCommaForThousandAndWithDecimalsRegex.Match(actualSharesToBuy).Value),
                $"Dollar Investment Risk - Shares to Buy - Main Value is not as expected for {ticker}");
            var expectedUnitNaming = tickerAssetType == PositionAssetTypes.Option
                ? AddPositionAdvancedFields.Contracts
                : AddPositionAdvancedFields.Shares;
            Checker.CheckContains(string.Format(expectedSharesAction, expectedUnitNaming.ToString()),
                investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                    PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                    PositionSizeVarietyTypes.SharesToBuy,
                    PositionSizeMainNumberTypes.Description),
                $"Dollar Investment Risk - Shares to Buy - Description is not as expected for {ticker}");

            var latestCloseDbValue = positionDataQueries.SelectStockOrOptionData(tickerAssetType, ticker).TradeClose;
            var assetTypeRelatedField = tickerAssetType == PositionAssetTypes.Option
                ? PositionSizeMainNumberTypes.AdditionalOption
                : PositionSizeMainNumberTypes.AdditionalOptionOrContractSize;
            Checker.CheckContains(string.Format(expectedPurchasePriceWording, expectedCurrencySign,
                    Parsing.ConvertToDouble(latestCloseDbValue).ToString("N2")),
                investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                    PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                    PositionSizeVarietyTypes.SharesToBuy,
                    assetTypeRelatedField),
                $"Dollar Investment Risk - Shares to Buy - Additional Description is not as expected for {ticker}");

            var dollarInvestmentExplanations =
                investmentOptionSection.GetExplanationsForInvestmentOptionPresent(PositionSizeInvestmentOptionTypes.DollarInvestmentRisk);
            Checker.CheckEquals(expectedDollarInvestmentExplanations.Count, dollarInvestmentExplanations.Count,
                "Dollar Investment Risk explanations count is not as expected");

            var analyzedDataForSymbol = new SymbolsQueries().SelectAnalyzedDataforSymbol(ticker);
            var adjustedExtremumDate = tradeType == PositionTradeTypes.Long
                ? analyzedDataForSymbol.LongAdjExtremumDate
                : analyzedDataForSymbol.ShortExtremumDate;
            var maxCloseDate = string.IsNullOrEmpty(adjustedExtremumDate)
                ? Constants.NotAvailableAcronym
                : Parsing.ConvertToShortDateString(adjustedExtremumDate);
            positionSizeForm.ScrollPageUp();
            var ssiStopPrice = GetRequiredSsiStopPrice(analyzedDataForSymbol);
            var expectedSsiStopPrice = Parsing.ConvertToDouble(string.IsNullOrEmpty(ssiStopPrice.Replace(".", string.Empty))
                        ? Constants.DefaultStringZeroIntValue
                        : ssiStopPrice)
                .ToString(CultureInfo.InvariantCulture);

            var healthZone = GetRequiredHealthZone(analyzedDataForSymbol);
            var adjustedReEntryDate = tradeType == PositionTradeTypes.Long
                ? analyzedDataForSymbol.LongAdjReEntryDate
                : analyzedDataForSymbol.ShortReEntryDate;
            var adjustedChangeDate = tradeType == PositionTradeTypes.Long
                ? analyzedDataForSymbol.LongAdjChangeDate
                : analyzedDataForSymbol.ShortChangeDate;
            var adjustedExtremumPrices = tradeType == PositionTradeTypes.Long
                ? analyzedDataForSymbol.LongAdjExtremumPrice
                : analyzedDataForSymbol.ShortExtremumPrice;
            var lastHealthChangedDate = string.IsNullOrEmpty(adjustedReEntryDate) || healthZone == HealthZoneTypes.Red
                ? adjustedChangeDate
                : adjustedReEntryDate;

            for (int i = 0; i < dollarInvestmentExplanations.Count; i++)
            {
                var expectedExplanation = string.Format(expectedDollarInvestmentExplanations[i], actualInvestmentAmount,
                    ticker, actualRiskAtInvestmentAmount, vqValue.ToString("#.00"), 
                    $"{expectedCurrencySign}{Parsing.ConvertToDouble(latestCloseDbValue):N2}", expectedCurrencySign,
                    maxCloseDate,
                    Parsing.ConvertToDouble(adjustedExtremumPrices ?? Constants.DefaultStringZeroIntValue)
                        .ToString(CultureInfo.InvariantCulture).RoundToDoubleWithFirstNonZeroDecimal(),
                    expectedSsiStopPrice.RoundToDoubleWithFirstNonZeroDecimal(),
                    Parsing.ConvertToDouble(string.IsNullOrEmpty(ssiStopPrice) ? Constants.DefaultStringZeroIntValue : ssiStopPrice)
                        .ToString(CultureInfo.InvariantCulture).RoundToDoubleWithFirstNonZeroDecimal(),
                    Parsing.ConvertToShortDateString(lastHealthChangedDate ?? DateTime.Now.ToShortDateString()));
                Checker.CheckEquals(expectedExplanation, dollarInvestmentExplanations[i],
                    $"Dollar Investment Risk explanations count #{i + 1} is not as expected");
            }

            Checker.IsTrue(investmentOptionSection.IsActionButtonPresentForInvestmentOption(PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                    PositionSizeActionTypes.AddToWatchlist),
                "Add to Watchlist button is not shown for Dollar Investment Risk");

            LogStep(5, "Click on 'edit' icon in the 'Dollar Investment Risk' section near 'Your Amount at Risk'.");
            investmentOptionSection.ClickEditPositionSizeForVarietyTypeForInvestmentOption(
                PositionSizeInvestmentOptionTypes.DollarInvestmentRisk, PositionSizeVarietyTypes.YourAmountAtRisk, PositionSizeEditModeTypes.Amount);

            var amountAtRiskAtEditing = investmentOptionSection.GetValueOfEditedPositionSizeForVarietyTypeForInvestmentOption(
                    PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeEditModeTypes.Amount).Replace(",", string.Empty);
            var expectedAmountAtRiskAtEditing = expectedCurrencySign + StringUtility.SetFormatFromSample(StringUtility.ReplaceAllCurrencySigns(
                    actualRiskAtInvestmentAmount.Replace(",", string.Empty)), StringUtility.ReplaceAllCurrencySigns(amountAtRiskAtEditing))
                .Split('.')[0];

            Checker.CheckEquals(expectedAmountAtRiskAtEditing, amountAtRiskAtEditing.Split('.')[0],
                "Your Amount at Risk at editing for Dollar Investment Risk is not as expected");
            Checker.IsTrue(investmentOptionSection.IsClosingEditModeButtonPresentForVarietyTypeForInvestmentOption(
                    PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeEditModeTypes.Amount,
                    PositionSizeClosingEditModeTypes.Ok),
                "Close Action 'Ok' for Your Amount at Risk at editing for Dollar Investment Risk is not shown");

            Checker.IsTrue(investmentOptionSection.IsClosingEditModeButtonPresentForVarietyTypeForInvestmentOption(
                    PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeEditModeTypes.Amount,
                    PositionSizeClosingEditModeTypes.Cancel),
                "Close Action 'Cancel' for Your Amount at Risk at editing for Dollar Investment Risk is not shown");

            Checker.IsTrue(investmentOptionSection.IsEditPositionSizeForVarietyTypeForInvestmentOptionDisabled(
                    PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeEditModeTypes.RiskPercent),
                "Risk (VQ%) field is NOT disabled for Your Amount at Amount editing for Dollar Investment Risk");

            Checker.IsTrue(investmentOptionSection.IsEditPositionSizeForVarietyTypeForInvestmentOptionDisabled(
                    PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                    PositionSizeVarietyTypes.SharesToBuy,
                    PositionSizeEditModeTypes.PurchasePrice),
                "Purchase Price field is NOT disabled for Shares to Buy at Amount editing for Dollar Investment Risk");

            Checker.IsFalse(investmentOptionSection.IsEditPositionSizeForVarietyTypeForInvestmentOptionPresent(
                    PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeEditModeTypes.Amount),
                "Edit sign for Your Amount at Risk field is Shown for Your Amount at Amount editing for Dollar Investment Risk");

            LogStep(6, "Set value according to test data.");
            investmentOptionSection.SetValueForVarietyTypeForInvestmentOption(
                    PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeEditModeTypes.Amount, expectedAmount);

            LogStep(7, "Click Ok.");
            investmentOptionSection.ClickCloseEditModeButtonPresentForVarietyTypeForInvestmentOption(
                PositionSizeInvestmentOptionTypes.DollarInvestmentRisk, PositionSizeVarietyTypes.YourAmountAtRisk,
                PositionSizeEditModeTypes.Amount, PositionSizeClosingEditModeTypes.Ok);

            Checker.IsFalse(investmentOptionSection.IsEditPositionSizeForVarietyTypeForInvestmentOptionDisabled(
                    PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeEditModeTypes.Amount),
                "Your Amount at Risk field is disabled for Your Amount after Amount editing for Dollar Investment Risk");
            Checker.IsFalse(investmentOptionSection.IsEditPositionSizeForVarietyTypeForInvestmentOptionDisabled(
                    PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeEditModeTypes.RiskPercent),
                "Risk (VQ%) field is disabled for Your Amount after Amount editing for Dollar Investment Risk");
            Checker.IsFalse(investmentOptionSection.IsEditPositionSizeForVarietyTypeForInvestmentOptionDisabled(
                    PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                    PositionSizeVarietyTypes.SharesToBuy,
                    PositionSizeEditModeTypes.PurchasePrice),
                "Purchase Price field is disabled for Shares to Buy after Amount editing for Dollar Investment Risk");
            Checker.IsTrue(investmentOptionSection.IsEditPositionSizeForVarietyTypeForInvestmentOptionPresent(
                    PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeEditModeTypes.Amount),
                "Edit sign for Your Amount at Risk field is NOT Shown for Your Amount after Amount editing for Dollar Investment Risk");

            var changedInvestmentAmount = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                PositionSizeVarietyTypes.YourInvestmentAmount,
                PositionSizeMainNumberTypes.Amount);
            Checker.CheckNotEquals(actualInvestmentAmount, changedInvestmentAmount,
                $"Dollar Investment Risk - Your Investment Amount - Main Value is not Changed after changing Your Amount at Risk for {ticker}");

            var changedRiskAtInvestmentAmount = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                PositionSizeVarietyTypes.YourAmountAtRisk,
                PositionSizeMainNumberTypes.Amount);
            Checker.CheckEquals($"{expectedCurrencySign}{expectedAmount.ToFractionalString()}", changedRiskAtInvestmentAmount.Replace(",", string.Empty),
                $"Dollar Investment Risk - Your Investment Amount - Main Value is Changed after changing Your Amount at Risk for {ticker}");
            var changedRiskPercent = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                PositionSizeVarietyTypes.YourAmountAtRisk,
                PositionSizeMainNumberTypes.AdditionalOptionOrContractSize);
            Checker.CheckContains(string.Format(expectedAdditionalVqRisk, vqValue.ToString("#.00")), changedRiskPercent,
                $"Dollar Investment Risk - Your Amount at Risk - Additional Description is not as expected after changing Your Amount at Risk for {ticker}");
            var changedSharesToBuy = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                PositionSizeVarietyTypes.SharesToBuy,
                PositionSizeMainNumberTypes.Amount);
            Checker.CheckNotEquals(actualSharesToBuy, changedSharesToBuy,
                $"Dollar Investment Risk - Shares to Buy - Main Value is not as expected after changing Your Amount at Risk for {ticker}");

            LogStep(8, "Click on 'edit' icon in the 'Dollar Investment Risk' section near 'Risk (VQ%)'.");
            investmentOptionSection.ClickEditPositionSizeForVarietyTypeForInvestmentOption(
                PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                PositionSizeVarietyTypes.YourAmountAtRisk,
                PositionSizeEditModeTypes.RiskPercent);

            var actualVqValue = investmentOptionSection.GetValueOfEditedPositionSizeForVarietyTypeForInvestmentOption(
                PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                PositionSizeVarietyTypes.YourAmountAtRisk,
                PositionSizeEditModeTypes.RiskPercent);
            Checker.CheckEquals(StringUtility.SetFormatFromSample(vqValue.ToString(CultureInfo.InvariantCulture), actualVqValue),
                actualVqValue, "Risk (VQ%): at editing for Dollar Investment Risk is not as expected");
            Checker.IsTrue(investmentOptionSection.IsClosingEditModeButtonPresentForVarietyTypeForInvestmentOption(
                    PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeEditModeTypes.RiskPercent,
                    PositionSizeClosingEditModeTypes.Ok),
                "Close Action 'Ok' for Your Amount at Risk at editing for Dollar Investment Risk is not shown");
            Checker.IsTrue(investmentOptionSection.IsClosingEditModeButtonPresentForVarietyTypeForInvestmentOption(
                    PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeEditModeTypes.RiskPercent,
                    PositionSizeClosingEditModeTypes.Cancel),
                "Close Action 'Cancel' for Your Amount at Risk at editing for Dollar Investment Risk is not shown");
            Checker.IsTrue(investmentOptionSection.IsEditPositionSizeForVarietyTypeForInvestmentOptionDisabled(
                    PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeEditModeTypes.Amount),
                "Your Amount at Risk field is NOT disabled for Your Amount at Risk editing for Dollar Investment Risk");
            Checker.IsTrue(investmentOptionSection.IsEditPositionSizeForVarietyTypeForInvestmentOptionDisabled(
                    PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                    PositionSizeVarietyTypes.SharesToBuy,
                    PositionSizeEditModeTypes.PurchasePrice),
                "Purchase Price field is NOT disabled for Shares to Buy at Risk editing for Dollar Investment Risk");
            Checker.IsFalse(investmentOptionSection.IsEditPositionSizeForVarietyTypeForInvestmentOptionPresent(
                    PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeEditModeTypes.RiskPercent),
                "Edit sign for Risk (VQ%) field is Shown for Risk at editing for Dollar Investment Risk");

            LogStep(9, "Set value according to test data.");
            investmentOptionSection.SetValueForVarietyTypeForInvestmentOption(
                PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                PositionSizeVarietyTypes.YourAmountAtRisk,
                PositionSizeEditModeTypes.RiskPercent,
                expectedRiskPercent);

            LogStep(10, "Click Ok.");
            investmentOptionSection.ClickCloseEditModeButtonPresentForVarietyTypeForInvestmentOption(
                PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                PositionSizeVarietyTypes.YourAmountAtRisk,
                PositionSizeEditModeTypes.RiskPercent,
                PositionSizeClosingEditModeTypes.Ok);

            Checker.IsFalse(investmentOptionSection.IsEditPositionSizeForVarietyTypeForInvestmentOptionDisabled(
                    PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeEditModeTypes.Amount),
                "Your Amount at Risk field is disabled for Your Amount after Risk editing for Dollar Investment Risk");
            Checker.IsFalse(investmentOptionSection.IsEditPositionSizeForVarietyTypeForInvestmentOptionDisabled(
                    PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeEditModeTypes.RiskPercent),
                "Risk (VQ%) field is disabled for Your Amount after Risk editing for Dollar Investment Risk");
            Checker.IsFalse(investmentOptionSection.IsEditPositionSizeForVarietyTypeForInvestmentOptionDisabled(
                    PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                    PositionSizeVarietyTypes.SharesToBuy,
                    PositionSizeEditModeTypes.PurchasePrice),
                "Purchase Price field is disabled for Shares to Buy after Risk editing for Dollar Investment Risk");
            Checker.IsTrue(investmentOptionSection.IsEditPositionSizeForVarietyTypeForInvestmentOptionPresent(
                    PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeEditModeTypes.RiskPercent),
                "Edit sign for Risk (VQ%): at Risk field is NOT Shown for Your Amount after Risk editing for Dollar Investment Risk");

            var finalInvestmentAmount = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                PositionSizeVarietyTypes.YourInvestmentAmount,
                PositionSizeMainNumberTypes.Amount);
            Checker.CheckNotEquals(changedInvestmentAmount, finalInvestmentAmount,
                $"Dollar Investment Risk - Your Investment Amount - Main Value is not Changed after changing Risk percent for {ticker}");

            var finalRiskAtInvestmentAmount = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                PositionSizeVarietyTypes.YourAmountAtRisk,
                PositionSizeMainNumberTypes.Amount);
            Checker.CheckEquals(changedRiskAtInvestmentAmount, finalRiskAtInvestmentAmount,
                $"Dollar Investment Risk - Your Investment Amount - Main Value is Changed after changing Risk percent for {ticker}");
            var finalRiskPercent = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                PositionSizeVarietyTypes.YourAmountAtRisk,
                PositionSizeMainNumberTypes.AdditionalOptionOrContractSize);
            Checker.CheckContains(string.Format(expectedAdditionalTsRisk, expectedRiskPercent.ToFractionalString()), finalRiskPercent,
                $"Dollar Investment Risk - Your Amount at Risk - Additional Description is not as expected after changing Risk percent for {ticker}");
            var finalSharesToBuy = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                PositionSizeVarietyTypes.SharesToBuy,
                PositionSizeMainNumberTypes.Amount);
            Checker.CheckNotEquals(changedSharesToBuy, finalSharesToBuy,
                $"Dollar Investment Risk - Shares to Buy - Main Value is not as expected after changing Your Amount at Risk for {ticker}");

            investmentOptionSection.ClickEditPositionSizeForVarietyTypeForInvestmentOption(
                PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                PositionSizeVarietyTypes.SharesToBuy,
                PositionSizeEditModeTypes.PurchasePrice);
            var actualPurschasePrice = investmentOptionSection.GetValueOfEditedPositionSizeForVarietyTypeForInvestmentOption(
                PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                PositionSizeVarietyTypes.SharesToBuy,
                PositionSizeEditModeTypes.PurchasePrice);
            investmentOptionSection.ClickCloseEditModeButtonPresentForVarietyTypeForInvestmentOption(
                PositionSizeInvestmentOptionTypes.DollarInvestmentRisk,
                PositionSizeVarietyTypes.SharesToBuy,
                PositionSizeEditModeTypes.PurchasePrice,
                PositionSizeClosingEditModeTypes.Cancel);

            LogStep(11, "Click Add to watchlist");
            investmentOptionSection.ClickActionButtoForInvestmentOption(PositionSizeInvestmentOptionTypes.DollarInvestmentRisk, PositionSizeActionTypes.AddToWatchlist);
            var addPositionAdvancedForm = new AddPositionAdvancedForm();
            var tickerAtAdding = tickerAssetType == PositionAssetTypes.Option
                ? addPositionAdvancedForm.GetOptionVariant()
                : addPositionAdvancedForm.GetSymbolTreeSelectSingleValue();
            Checker.CheckEquals(ticker, tickerAtAdding, "Ticker is not as expected at adding");
            Checker.IsTrue(addPositionAdvancedForm.IsBtnTradeTypeActive(tradeType), "Trade Type is not as expected at adding");

            var sharesAtAdding = addPositionAdvancedForm.GetValueFromTextBoxField(AddPositionAdvancedFields.Shares);
            Checker.CheckEquals(StringUtility.SetFormatFromSample(finalSharesToBuy, sharesAtAdding), sharesAtAdding.Replace(",", string.Empty),
                "Shares is not as expected at adding");
            Checker.CheckEquals(actualPurschasePrice, addPositionAdvancedForm.GetValueFromTextBoxField(AddPositionAdvancedFields.EntryPrice),
                "Entry price is not as expected at adding");

            LogStep(12, "Select portfolio and click save");
            new AddPositionAdvancedSteps().SelectPortfolioByIdClickSaveWaitPositionCard(portfolioId);
            var positionCardForm = new PositionCardForm();
            var positionDetailsTabPositionCardForm = positionCardForm.ActivateTabGetForm<PositionDetailsTabPositionCardForm>(PositionCardTabs.PositionDetails);
            Checker.CheckEquals(ticker, positionCardForm.GetSymbol(), "Ticker is not as expected on card");
            Checker.CheckEquals(tradeType == PositionTradeTypes.Long, positionDetailsTabPositionCardForm.IsTradeTypeLong(), "Trade Type is not as expected on card");
            var fieldWithShares = positionDetailsTabPositionCardForm.GetSharesTypeFieldWithDetectingType();
            Checker.CheckEquals(sharesAtAdding.ToFractionalString(),
                positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(fieldWithShares).Replace(",", string.Empty).DeleteMathSigns(),
                "Shares is not as expected on card");
            var entryPricePositionCard = StringUtility.ReplaceAllCurrencySigns(positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.EntryPrice));
            Checker.CheckEquals(StringUtility.SetFormatFromSample(actualPurschasePrice, entryPricePositionCard),
                entryPricePositionCard.Replace(",", string.Empty),
                "Entry price is not as expected on card");
        }

        private HealthZoneTypes GetRequiredHealthZone(DsiSymbolsModel analyzedDataForSymbol)
        {
            return tradeType == PositionTradeTypes.Long
                ? string.IsNullOrEmpty(analyzedDataForSymbol.LongAdjDsi)
                    ? HealthZoneTypes.NotAvailable
                    : (HealthZoneTypes)Parsing.ConvertToInt(analyzedDataForSymbol.LongAdjDsi)
                : string.IsNullOrEmpty(analyzedDataForSymbol.ShortDsi)
                    ? HealthZoneTypes.NotAvailable
                    : (HealthZoneTypes)Parsing.ConvertToInt(analyzedDataForSymbol.ShortDsi);
        }

        private string GetRequiredSsiStopPrice(DsiSymbolsModel analyzedDataForSymbol)
        {
            return tradeType == PositionTradeTypes.Long
                ? string.IsNullOrEmpty(analyzedDataForSymbol.LongAdjStopPrice)
                    ? string.Empty
                    : analyzedDataForSymbol.LongAdjStopPrice.ToFractionalString()
                : string.IsNullOrEmpty(analyzedDataForSymbol.ShortStopPrice)
                    ? string.Empty
                    : analyzedDataForSymbol.ShortStopPrice.ToFractionalString();
        }
    }
}