using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.PositionData;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.AddPosition;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.PositionCard;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.Tools;
using AutomatedTests.Enums;
using AutomatedTests.Forms.AddPositionAdvanced;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Forms.ResearchPages.PositionSizeForm;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.AddPositionAdvanced;
using AutomatedTests.Steps.PositionCard;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._07_Invest._05_PositionSize
{
    [TestClass]
    public class TC_1383_Invest_PositionSize_CheckThatEqualPositionSizeOfPortfolioWorksAsExpected : BaseTestUnitTests
    {
        private const int TestNumber = 1383;

        private PortfolioModel portfolioModel;
        private PositionsDBModel positionModel;
        private int portfolioId;

        private string ticker;
        private PositionTradeTypes tradeType;
        private PositionAssetTypes tickerAssetType;
        private string expectedCurrencySign;

        private string expectedDropdownInputText;
        private string expectedPurchasePriceWording;
        private string expectedPortfolioValue;
        private string expectedRiskDescription;

        private string purchasePrice;
        private string expectedRiskPercent;
        private string expectedSharesAction;

        private int expectedEqualExplanantionCount;
        private readonly List<string> expectedEqualExplanantions = new List<string>();

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
            positionModel = new PositionsDBModel
            {
                StatusType = GetTestDataAsString("DbStatus"),
                Symbol = GetTestDataAsString("SymbolBefore"),
                TradeType = ((int)GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>("tradeTypeDb")).ToString(),
                Shares = GetTestDataAsString("Shares")
            };

            ticker = GetTestDataAsString(nameof(ticker));
            tradeType = GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>(nameof(tradeType));
            var symbolId = positionsQueries.SelectSymbolIdNameUsingSymbol(ticker).SymbolId;
            tickerAssetType = GetTestDataParsedAsEnumFromStringMapping<PositionAssetTypes>(nameof(tickerAssetType));
            expectedCurrencySign = ((Currency)positionsQueries.SelectSymbolCurrencyBySymbol(ticker)).GetDescription();
            expectedPurchasePriceWording = GetTestDataAsString(nameof(expectedPurchasePriceWording));
            expectedRiskDescription = GetTestDataAsString(nameof(expectedRiskDescription));

            expectedDropdownInputText = GetTestDataAsString(nameof(expectedDropdownInputText));
            expectedPortfolioValue = GetTestDataAsString(nameof(expectedPortfolioValue));
            expectedRiskPercent = GetTestDataAsString(nameof(expectedRiskPercent));
            purchasePrice = GetTestDataAsString(nameof(purchasePrice));
            expectedSharesAction = GetTestDataAsString(nameof(expectedSharesAction));

            expectedEqualExplanantionCount = GetTestDataAsInt(nameof(expectedEqualExplanantionCount));
            for (int i = 1; i <= expectedEqualExplanantionCount; i++)
            {
                expectedEqualExplanantions.Add(GetTestDataAsString($"{nameof(expectedEqualExplanantions)}{i}"));
            }

            LogStep(0, "Precondition; Login");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));

            portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            var positionId = PositionsAlertsSetUp.AddPositionViaDB(portfolioId, positionModel);

            LoginSetUp.LogIn(UserModels.First());
            new PositionCardSteps().ResavePositionCard(positionId);
            new MainMenuNavigation().OpenPositionSizeForTicker(Parsing.ConvertToInt(symbolId));
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1383$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("ResearchPage"), TestCategory("PositionSize"), TestCategory("AddPositionPage"), TestCategory("PositionCard")]
        [Description("Test to check that Equal Position Size of Portfolio widget works as expected. https://tr.a1qa.com/index.php?/cases/view/19234228")]
        public override void RunTest()
        {
            LogStep(1, "Check 'Equal Position Size' section.");
            var investmentOptionSection = new InvestmentOptionSection();
            var positionSizeForm = new PositionSizeCalculatorForm();
            positionSizeForm.ClickPositionsTypeCheckbox(PositionForManualPortfolioCreateInformation.PositionType, tradeType, Constants.DefaultOrderOfSameItemsToReturn);
            Checker.CheckEquals(tradeType, 
                positionSizeForm.GetSelectedTradeTypeByOrder(Constants.DefaultOrderOfSameItemsToReturn), 
                "Trade Type is not as expected");
            positionSizeForm.ClickCalculatePositionSize();

            LogStep(2, "Check 'Equal Position Size of Portfolio' section");
            var latestCloseDbValue = positionDataQueries.SelectStockOrOptionData(tickerAssetType, ticker).TradeClose;
            var vqValue = Parsing.ConvertToDouble(
                positionsQueries.SelectCurrentVqBySymbolId(
                    Parsing.ConvertToInt(positionsQueries.SelectSymbolIdNameUsingSymbol(ticker).SymbolId)));
            var signToVqStopPrice = tradeType == PositionTradeTypes.Long
                ? -1
                : 1;
            var vqStopPrice = (Parsing.ConvertToDouble(latestCloseDbValue) * (1 + signToVqStopPrice * vqValue / 100)).ToString("N2");

            Checker.IsTrue(investmentOptionSection.IsInvestmentOptionResultPresent(PositionSizeInvestmentOptionTypes.EqualPositionRisk),
                $"Equal Position Size of Portfolio is not shown for {ticker}");
            Checker.CheckContains(expectedDropdownInputText,
                investmentOptionSection.GetInvestmentInputLabelForInvestmentOptionLabelText(PositionSizeInvestmentOptionTypes.EqualPositionRisk),
                $"Equal Position Size of Portfolio Dropdown has unexpected Label for {ticker}");
            Checker.CheckEquals(portfolioModel.Name,
                investmentOptionSection.GetSelectedPortfolioForInvestmentOption(PositionSizeInvestmentOptionTypes.EqualPositionRisk),
                $"Equal Position Size of Portfolio has unexpected portfolio Name for {ticker}");
            Checker.CheckContains(expectedPortfolioValue,
                investmentOptionSection.GetPortfolioValueTextForInvestmentOption(PositionSizeInvestmentOptionTypes.EqualPositionRisk),
                $"Equal Position Size of Portfolio has unexpected portfolio Value for {ticker}");

            var actualInvestmentAmount = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                PositionSizeInvestmentOptionTypes.EqualPositionRisk,
                PositionSizeVarietyTypes.YourInvestmentAmount,
                PositionSizeMainNumberTypes.Amount);
            Checker.IsFalse(string.IsNullOrEmpty(Constants.NumbersWithCommaForThousandAndWithDecimalsRegex.Match(actualInvestmentAmount).Value),
                $"Equal Position Size - Your Investment Amount - Main Value is not as expected for {ticker}");

            Checker.CheckContains(PositionSizeVarietyTypes.YourInvestmentAmount.GetStringMapping(),
                investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                    PositionSizeInvestmentOptionTypes.EqualPositionRisk,
                    PositionSizeVarietyTypes.YourInvestmentAmount,
                    PositionSizeMainNumberTypes.Description),
                $"Equal Position Size - Your Investment Amount - Description is not as expected for {ticker}");

            var actualRiskAtInvestmentAmount = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                    PositionSizeInvestmentOptionTypes.EqualPositionRisk,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeMainNumberTypes.Amount);
            Checker.IsFalse(string.IsNullOrEmpty(Constants.NumbersWithCommaForThousandAndWithDecimalsRegex.Match(actualRiskAtInvestmentAmount).Value),
                $"Equal Position Size - Your Investment Amount - Main Value is not as expected for {ticker}");
            Checker.CheckContains(PositionSizeVarietyTypes.YourAmountAtRisk.GetStringMapping(),
                investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                    PositionSizeInvestmentOptionTypes.EqualPositionRisk,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeMainNumberTypes.Description),
                $"Equal Position Size - Your Amount at Risk - Description is not as expected for {ticker}");

            var actualEqualRiskDescription = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                    PositionSizeInvestmentOptionTypes.EqualPositionRisk,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeMainNumberTypes.AdditionalOptionOrContractSize);
            Checker.IsFalse(string.IsNullOrEmpty(new Regex(expectedRiskDescription).Match(actualEqualRiskDescription).Value),
                $"Equal Position Size - Your Amount at Risk - Additional Description is not as expected for {ticker}");

            var actualShareToBuy = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                PositionSizeInvestmentOptionTypes.EqualPositionRisk,
                PositionSizeVarietyTypes.SharesToBuy,
                PositionSizeMainNumberTypes.Amount);
            Checker.IsFalse(string.IsNullOrEmpty(Constants.NumbersWithCommaForThousandAndWithDecimalsRegex.Match(actualShareToBuy).Value),
                $"Equal Position Size - Shares to Buy - Main Value is not as expected for {ticker}");
            Checker.CheckContains(string.Format(expectedSharesAction, AddPositionAdvancedFields.Shares.ToString()),
                investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                    PositionSizeInvestmentOptionTypes.EqualPositionRisk,
                    PositionSizeVarietyTypes.SharesToBuy,
                    PositionSizeMainNumberTypes.Description),
                $"Equal Position Size - Shares to Buy - Description is not as expected for {ticker}");
            Checker.CheckContains(string.Format(expectedPurchasePriceWording, expectedCurrencySign,
                    Parsing.ConvertToDouble(latestCloseDbValue).ToString("N2")),
                investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                    PositionSizeInvestmentOptionTypes.EqualPositionRisk,
                    PositionSizeVarietyTypes.SharesToBuy,
                    PositionSizeMainNumberTypes.AdditionalOptionOrContractSize),
                $"Equal Position Size - Shares to Buy - Additional Description is not as expected for {ticker}");

            var actualEqualExplanantions = investmentOptionSection.GetExplanationsForInvestmentOptionPresent(PositionSizeInvestmentOptionTypes.EqualPositionRisk);
            Checker.CheckEquals(expectedEqualExplanantionCount, actualEqualExplanantions.Count,
                "Equal Position Size explanations count is not as expected");

            for (int i = 0; i < actualEqualExplanantions.Count; i++)
            {
                var expectedExplanation = string.Format(expectedEqualExplanantions[i], ticker, actualInvestmentAmount,
                    vqStopPrice.RoundToDoubleWithFirstNonZeroDecimal(), vqValue.ToString("#.00"),
                    $"{expectedCurrencySign}{Parsing.ConvertToDouble(latestCloseDbValue):N2}", 
                    actualRiskAtInvestmentAmount, actualShareToBuy, expectedCurrencySign);
                Checker.CheckEquals(expectedExplanation, actualEqualExplanantions[i], $"Equal Position Size explanations count #{i + 1} is not as expected");
            }

            Checker.IsTrue(investmentOptionSection.IsActionButtonPresentForInvestmentOption(
                    PositionSizeInvestmentOptionTypes.EqualPositionRisk,
                    PositionSizeActionTypes.AddToWatchlist),
                "Add to Watchlist button is not shown for Equal Position Size");
            Checker.IsTrue(investmentOptionSection.IsActionButtonPresentForInvestmentOption(
                    PositionSizeInvestmentOptionTypes.EqualPositionRisk,
                    PositionSizeActionTypes.RunRiskRebalancer),
                "Run Risk rebalancer button is not shown for Equal Position Size");

            LogStep(3, "Click on 'edit' icon in the 4rd subsection of 'Equal Position Size' section near 'Purchase Price'.");
            investmentOptionSection.ClickEditPositionSizeForVarietyTypeForInvestmentOption(
                PositionSizeInvestmentOptionTypes.EqualPositionRisk,
                PositionSizeVarietyTypes.SharesToBuy,
                PositionSizeEditModeTypes.PurchasePrice);

            var actualPurschasePrice = investmentOptionSection.GetValueOfEditedPositionSizeForVarietyTypeForInvestmentOption(
                    PositionSizeInvestmentOptionTypes.EqualPositionRisk,
                    PositionSizeVarietyTypes.SharesToBuy,
                    PositionSizeEditModeTypes.PurchasePrice);
            var expectedPurchasePrice = $"{expectedCurrencySign}{StringUtility.SetFormatFromSample(latestCloseDbValue, StringUtility.ReplaceAllCurrencySigns(actualPurschasePrice))}";
            Checker.CheckEquals(expectedPurchasePrice, actualPurschasePrice.Replace(",", string.Empty),
                "Purchase Price at editing for Equal Position Size is not as expected");
            Checker.IsTrue(investmentOptionSection.IsClosingEditModeButtonPresentForVarietyTypeForInvestmentOption(
                    PositionSizeInvestmentOptionTypes.EqualPositionRisk,
                    PositionSizeVarietyTypes.SharesToBuy,
                    PositionSizeEditModeTypes.PurchasePrice,
                    PositionSizeClosingEditModeTypes.Ok),
                "Close Action 'Ok' for Your Amount at Risk at editing for Equal Position Size is not shown");
            Checker.IsTrue(investmentOptionSection.IsClosingEditModeButtonPresentForVarietyTypeForInvestmentOption(
                    PositionSizeInvestmentOptionTypes.EqualPositionRisk,
                    PositionSizeVarietyTypes.SharesToBuy,
                    PositionSizeEditModeTypes.PurchasePrice,
                    PositionSizeClosingEditModeTypes.Cancel),
                "Close Action 'Cancel' for Your Amount at Risk at editing for Equal Position Size is not shown");
            Checker.IsFalse(investmentOptionSection.IsEditPositionSizeForVarietyTypeForInvestmentOptionPresent(
                    PositionSizeInvestmentOptionTypes.EqualPositionRisk,
                    PositionSizeVarietyTypes.SharesToBuy,
                    PositionSizeEditModeTypes.PurchasePrice),
                "Edit sign for Purchase Price field is Shown for Risk at editing for Equal Position Size");

            LogStep(6, "Set value according to test data.");
            investmentOptionSection.SetValueForVarietyTypeForInvestmentOption(
                    PositionSizeInvestmentOptionTypes.EqualPositionRisk,
                    PositionSizeVarietyTypes.SharesToBuy,
                    PositionSizeEditModeTypes.PurchasePrice,
                    purchasePrice);

            LogStep(7, "Click 'Ok' link.");
            investmentOptionSection.ClickCloseEditModeButtonPresentForVarietyTypeForInvestmentOption(
                PositionSizeInvestmentOptionTypes.EqualPositionRisk,
                PositionSizeVarietyTypes.SharesToBuy,
                PositionSizeEditModeTypes.PurchasePrice,
                PositionSizeClosingEditModeTypes.Ok);

            Checker.IsFalse(investmentOptionSection.IsEditPositionSizeForVarietyTypeForInvestmentOptionDisabled(
                    PositionSizeInvestmentOptionTypes.EqualPositionRisk,
                    PositionSizeVarietyTypes.SharesToBuy,
                    PositionSizeEditModeTypes.PurchasePrice),
                "Purchase Price field is disabled for Shares to Buy after Risk editing for Risk Percentage of Portfolio");
            Checker.IsTrue(investmentOptionSection.IsEditPositionSizeForVarietyTypeForInvestmentOptionPresent(
                    PositionSizeInvestmentOptionTypes.EqualPositionRisk,
                    PositionSizeVarietyTypes.SharesToBuy,
                    PositionSizeEditModeTypes.PurchasePrice),
                "Edit sign for Purchase Price field is NOT Shown for Your Amount after Risk editing for Risk Percentage of Portfolio");

            var finalInvestmentAmount = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                PositionSizeInvestmentOptionTypes.EqualPositionRisk,
                PositionSizeVarietyTypes.YourInvestmentAmount,
                PositionSizeMainNumberTypes.Amount);
            Checker.CheckNotEquals(actualInvestmentAmount, finalInvestmentAmount,
                $"Equal Position Size - Your Investment Amount - Main Value is NOT changed after changing Purchase Price for {ticker}");

            var finalRiskAtInvestmentAmount = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                PositionSizeInvestmentOptionTypes.EqualPositionRisk,
                PositionSizeVarietyTypes.YourAmountAtRisk,
                PositionSizeMainNumberTypes.Amount);
            Checker.CheckEquals(actualRiskAtInvestmentAmount, finalRiskAtInvestmentAmount,
                $"Equal Position Size - Your Amount at Risk - Additional Description is changed after changing Purchase Price for {ticker}");

            var finalRiskPercentWording = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                PositionSizeInvestmentOptionTypes.EqualPositionRisk,
                PositionSizeVarietyTypes.YourAmountAtRisk,
                PositionSizeMainNumberTypes.AdditionalOptionOrContractSize);
            Checker.CheckEquals(actualEqualRiskDescription, finalRiskPercentWording,
                $"Equal Position Size - Your Amount at Risk - Risk percent wording is changed after changing Purchase Price for {ticker}");

            var finalSharesToBuy = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                PositionSizeInvestmentOptionTypes.EqualPositionRisk,
                PositionSizeVarietyTypes.SharesToBuy,
                PositionSizeMainNumberTypes.Amount);
            Checker.CheckNotEquals(actualShareToBuy, finalSharesToBuy,
                $"Equal Position Size - Shares to Buy - Main Value is not as expected after changing Purchase Price for {ticker}");

            var finalPurchasePrice = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                    PositionSizeInvestmentOptionTypes.EqualPositionRisk,
                    PositionSizeVarietyTypes.SharesToBuy,
                    PositionSizeMainNumberTypes.AdditionalOptionOrContractSize);
            expectedPurchasePrice = string.Format(expectedPurchasePriceWording, expectedCurrencySign, Parsing.ConvertToDouble(purchasePrice).ToString("N2"));
            Checker.CheckEquals(expectedPurchasePrice, finalPurchasePrice,
                "Purchase Price at editing for Equal Position Size is not as expected");

            LogStep(8, "Click Add to watchlist");
            investmentOptionSection.ClickActionButtoForInvestmentOption(PositionSizeInvestmentOptionTypes.EqualPositionRisk, PositionSizeActionTypes.AddToWatchlist);
            var addPositionAdvancedForm = new AddPositionAdvancedForm();
            Checker.CheckEquals(ticker, addPositionAdvancedForm.GetSymbolTreeSelectSingleValue(),
                "Ticker is not as expected at adding");
            Checker.IsTrue(addPositionAdvancedForm.IsBtnTradeTypeActive(tradeType), "Trade Type is not as expected at adding");

            var sharesAtAdding = addPositionAdvancedForm.GetValueFromTextBoxField(AddPositionAdvancedFields.Shares);
            Checker.CheckEquals(StringUtility.SetFormatFromSample(finalSharesToBuy, sharesAtAdding),
                sharesAtAdding.Replace(",", string.Empty),
                "Shares is not as expected at adding");
            var formattedpurchasePrice = $"{expectedCurrencySign}{Parsing.ConvertToDouble(purchasePrice):N2}";
            Checker.CheckEquals(formattedpurchasePrice.TrimEnd('0').TrimEnd('0').TrimEnd('.'),
                addPositionAdvancedForm.GetValueFromTextBoxField(AddPositionAdvancedFields.EntryPrice),
                "Entry price is not as expected at adding");

            LogStep(9, "Select portfolio and click save");
            new AddPositionAdvancedSteps().SelectPortfolioByIdClickSaveWaitPositionCard(portfolioId);
            var positionDetailsTabPositionCardForm = new PositionDetailsTabPositionCardForm();
            Checker.CheckEquals(ticker, new PositionCardForm().GetSymbol(),
                "Ticker is not as expected on card");
            Checker.CheckEquals(tradeType == PositionTradeTypes.Long,
                positionDetailsTabPositionCardForm.IsTradeTypeLong(),
                "Trade Type is not as expected on card");

            var fieldWithShares = positionDetailsTabPositionCardForm.GetSharesTypeFieldWithDetectingType();
            Checker.CheckEquals(sharesAtAdding.ToFractionalString(),
                positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(fieldWithShares).Replace(",", string.Empty).DeleteMathSigns(),
                "Shares is not as expected on card");
            var entryPricePositionCard = StringUtility.ReplaceAllCurrencySigns(positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.EntryPrice));
            Checker.CheckEquals(StringUtility.SetFormatFromSample(formattedpurchasePrice, entryPricePositionCard),
                entryPricePositionCard.Replace(",", string.Empty),
                "Entry price is not as expected on card");
        }
    }
}