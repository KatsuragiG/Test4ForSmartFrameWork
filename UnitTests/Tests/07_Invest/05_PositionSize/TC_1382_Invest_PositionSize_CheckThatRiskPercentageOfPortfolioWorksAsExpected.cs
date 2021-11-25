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
using AutomatedTests.Forms.ResearchPages.StockAnalyzer;
using AutomatedTests.Enums.Tools.StockAnalyzer;

namespace UnitTests.Tests._07_Invest._05_PositionSize
{
    [TestClass]
    public class TC_1382_Invest_PositionSize_CheckThatRiskPercentageOfPortfolioWorksAsExpected : BaseTestUnitTests
    {
        private const int TestNumber = 1382;

        private PortfolioModel portfolioModel;
        private PositionsDBModel positionModel;
        private int portfolioId;

        private string ticker;
        private PositionTradeTypes tradeType;
        private PositionAssetTypes tickerAssetType;
        private string expectedCurrencySign;

        private string expectedDropdownInputText;
        private string expectedAdditionalRiskOnRiskPercentageOfPortfolio;
        private string expectedPurchasePriceWording;
        private string expectedPortfolioValue;

        private string purchasePrice;
        private string expectedRiskPercent;
        private string expectedSharesAction;

        private int expectedRiskPercentageExplanantionCount;
        private readonly List<string> expectedRiskExplanations = new List<string>();

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

            expectedDropdownInputText = GetTestDataAsString(nameof(expectedDropdownInputText));
            expectedAdditionalRiskOnRiskPercentageOfPortfolio = GetTestDataAsString(nameof(expectedAdditionalRiskOnRiskPercentageOfPortfolio));
            expectedPortfolioValue = GetTestDataAsString(nameof(expectedPortfolioValue));
            expectedRiskPercent = GetTestDataAsString(nameof(expectedRiskPercent));
            purchasePrice = GetTestDataAsString(nameof(purchasePrice));
            expectedSharesAction = GetTestDataAsString(nameof(expectedSharesAction));

            expectedRiskPercentageExplanantionCount = GetTestDataAsInt(nameof(expectedRiskPercentageExplanantionCount));
            for (int i = 1; i <= expectedRiskPercentageExplanantionCount; i++)
            {
                expectedRiskExplanations.Add(GetTestDataAsString($"{nameof(expectedRiskExplanations)}{i}"));
            }

            LogStep(0, "Precondition; Login");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));

            portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            var positionId = PositionsAlertsSetUp.AddPositionViaDB(portfolioId, positionModel);

            LoginSetUp.LogIn(UserModels.First());
            new PositionCardSteps().ResavePositionCard(positionId);
            new MainMenuNavigation().OpenStockAnalyzerForSymbolId(Parsing.ConvertToInt(symbolId));
            new StockAnalyzerForm().ClickAdditionalActionsButton(StockAnalyzerAdditionalButtonTypes.CalculatePositionSize);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1382$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("ResearchPage"), TestCategory("PositionSize"), TestCategory("AddPositionPage"), TestCategory("PositionCard")]
        [Description("Test to check that Risk Percentage Of Portfolio widget works as expected. https://tr.a1qa.com/index.php?/cases/view/19234227")]
        public override void RunTest()
        {
            LogStep(1, "Check 'Risk Percentage of Portfolio' section.");
            var investmentOptionSection = new InvestmentOptionSection();
            var positionSizeForm = new PositionSizeCalculatorForm();
            positionSizeForm.ClickPositionsTypeCheckbox(PositionForManualPortfolioCreateInformation.PositionType, tradeType, Constants.DefaultOrderOfSameItemsToReturn);
            positionSizeForm.ClickCalculatePositionSize();

            var latestCloseDbValue = positionDataQueries.SelectStockOrOptionData(tickerAssetType, ticker).TradeClose;
            var vqValue = Parsing.ConvertToDouble(
                positionsQueries.SelectCurrentVqBySymbolId(
                    Parsing.ConvertToInt(positionsQueries.SelectSymbolIdNameUsingSymbol(ticker).SymbolId)));
            var signToVqStopPrice = tradeType == PositionTradeTypes.Long
                ? -1
                : 1;
            var vqStopPrice = (Parsing.ConvertToDouble(latestCloseDbValue) * (1 + signToVqStopPrice * vqValue / 100)).ToString("N2");

            Checker.IsTrue(investmentOptionSection.IsInvestmentOptionResultPresent(PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio),
                $"Risk Percentage of Portfolio is not shown for {ticker}");
            Checker.CheckContains(expectedDropdownInputText,
                investmentOptionSection.GetInvestmentInputLabelForInvestmentOptionLabelText(PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio),
                $"Risk Percentage of Portfolio Dropdown has unexpected Label for {ticker}");
            Checker.CheckEquals(portfolioModel.Name,
                investmentOptionSection.GetSelectedPortfolioForInvestmentOption(PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio),
                $"Risk Percentage of Portfolio has unexpected portfolio Name for {ticker}");
            Checker.CheckContains(expectedPortfolioValue,
                investmentOptionSection.GetPortfolioValueTextForInvestmentOption(PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio),
                $"Risk Percentage of Portfolio has unexpected portfolio Value for {ticker}");

            var actualInvestmentAmount = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio, PositionSizeVarietyTypes.YourInvestmentAmount,
                PositionSizeMainNumberTypes.Amount);

            Checker.IsFalse(string.IsNullOrEmpty(Constants.NumbersWithCommaForThousandAndWithDecimalsRegex.Match(actualInvestmentAmount).Value),
                $"Risk Percentage of Portfolio - Your Investment Amount - Main Value is not as expected for {ticker}");
            Checker.CheckContains(PositionSizeVarietyTypes.YourInvestmentAmount.GetStringMapping(),
                investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                    PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                    PositionSizeVarietyTypes.YourInvestmentAmount,
                    PositionSizeMainNumberTypes.Description),
                $"Risk Percentage of Portfolio - Your Investment Amount - Description is not as expected for {ticker}");

            var actualRiskAtInvestmentAmount = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                    PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeMainNumberTypes.Amount);
            var portfolioMoneyValue = new Regex(Constants.RiskValuePattern).Match(expectedPortfolioValue).Value.Replace(",", string.Empty);
            var expectedRiskAtInvestmentAmount =
                $"{expectedCurrencySign}{Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(portfolioMoneyValue)) / 100:N2}";
            Checker.CheckEquals(expectedRiskAtInvestmentAmount, actualRiskAtInvestmentAmount,
                $"Risk Percentage of Portfolio - Your Amount at Risk - Main Value is not as expected for {ticker}");
            Checker.CheckContains(PositionSizeVarietyTypes.YourAmountAtRisk.GetStringMapping(),
                investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                    PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeMainNumberTypes.Description),
                $"Risk Percentage of Portfolio - Your Amount at Risk - Description is not as expected for {ticker}");
            Checker.CheckContains(expectedAdditionalRiskOnRiskPercentageOfPortfolio,
                investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                    PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeMainNumberTypes.AdditionalOptionOrContractSize),
                $"Risk Percentage of Portfolio - Your Amount at Risk - Additional Description is not as expected for {ticker}");

            var actualShareToBuy = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                PositionSizeVarietyTypes.SharesToBuy,
                PositionSizeMainNumberTypes.Amount);
            Checker.IsFalse(string.IsNullOrEmpty(Constants.NumbersWithCommaForThousandAndWithDecimalsRegex.Match(actualShareToBuy).Value),
                $"Risk Percentage of Portfolio - Shares to Buy - Main Value is not as expected for {ticker}");
            Checker.CheckContains(string.Format(expectedSharesAction, AddPositionAdvancedFields.Shares.ToString()),
                investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                    PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                    PositionSizeVarietyTypes.SharesToBuy,
                    PositionSizeMainNumberTypes.Description),
                $"Risk Percentage of Portfolio - Shares to Buy - Description is not as expected for {ticker}");
            Checker.CheckContains(string.Format(expectedPurchasePriceWording, expectedCurrencySign, Parsing.ConvertToDouble(latestCloseDbValue).ToString("N2")),
                investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                    PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                    PositionSizeVarietyTypes.SharesToBuy,
                    PositionSizeMainNumberTypes.AdditionalOptionOrContractSize),
                $"Risk Percentage of Portfolio - Shares to Buy - Additional Description is not as expected for {ticker}");

            var riskPercentageExplanations = investmentOptionSection.GetExplanationsForInvestmentOptionPresent(PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio);
            Checker.CheckEquals(expectedRiskPercentageExplanantionCount, riskPercentageExplanations.Count,
                "Risk Percentage of Portfolio explanations count is not as expected");

            for (int i = 0; i < expectedRiskExplanations.Count; i++)
            {
                var expectedExplanation = string.Format(expectedRiskExplanations[i], ticker, actualInvestmentAmount,
                    vqStopPrice.RoundToDoubleWithFirstNonZeroDecimal(), vqValue.ToString("#.00"),
                    $"{expectedCurrencySign}{Parsing.ConvertToDouble(latestCloseDbValue):N2}", actualRiskAtInvestmentAmount, actualShareToBuy, expectedCurrencySign);
                Checker.CheckEquals(expectedExplanation, riskPercentageExplanations[i],
                    $"Risk Percentage of Portfolio explanations count #{i + 1} is not as expected");
            }

            Checker.IsTrue(investmentOptionSection.IsActionButtonPresentForInvestmentOption(
                    PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio, PositionSizeActionTypes.AddToWatchlist),
                "Add to Watchlist button is not shown for Risk Percentage of Portfolio");

            LogStep(2, "Click 'edit' icon in the 3rd subsection of 'Risk Percentage of Portfolio' section.");
            investmentOptionSection.ClickEditPositionSizeForVarietyTypeForInvestmentOption(
                PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                PositionSizeVarietyTypes.YourAmountAtRisk, PositionSizeEditModeTypes.RiskPercent);

            var actualRiskPercentValue = investmentOptionSection.GetValueOfEditedPositionSizeForVarietyTypeForInvestmentOption(
                    PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeEditModeTypes.RiskPercent);
            Checker.CheckEquals(StringUtility.SetFormatFromSample(Constants.PositionSizeDefaultRiskPercent.ToString(), actualRiskPercentValue),
                actualRiskPercentValue, "Risk: at editing for Risk Percentage of Portfolio is not as expected");
            Checker.IsTrue(investmentOptionSection.IsClosingEditModeButtonPresentForVarietyTypeForInvestmentOption(
                    PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeEditModeTypes.RiskPercent,
                    PositionSizeClosingEditModeTypes.Ok),
                "Close Action 'Ok' for Risk: 1.00% of the total capital in portfolio at editing for Risk Percentage of Portfolio is not shown");
            Checker.IsTrue(investmentOptionSection.IsClosingEditModeButtonPresentForVarietyTypeForInvestmentOption(
                    PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeEditModeTypes.RiskPercent,
                    PositionSizeClosingEditModeTypes.Cancel),
                "Close Action 'Cancel' for Risk: 1.00% of the total capital in portfolio at editing for Risk Percentage of Portfolio is not shown");
            Checker.IsTrue(investmentOptionSection.IsEditPositionSizeForVarietyTypeForInvestmentOptionDisabled(
                    PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                    PositionSizeVarietyTypes.SharesToBuy,
                    PositionSizeEditModeTypes.PurchasePrice),
                "Purchase Price field is NOT disabled for Shares to Buy at Risk editing for Risk Percentage of Portfolio");
            Checker.IsFalse(investmentOptionSection.IsEditPositionSizeForVarietyTypeForInvestmentOptionPresent(
                    PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeEditModeTypes.RiskPercent),
                "Edit sign for Risk: 1.00% of the total capital in portfolio field is Shown for Risk at editing for Risk Percentage of Portfolio");

            LogStep(3, "Set value according to test data.");
            investmentOptionSection.SetValueForVarietyTypeForInvestmentOption(
                    PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeEditModeTypes.RiskPercent,
                    expectedRiskPercent);

            LogStep(4, "Click 'Ok' link.");
            investmentOptionSection.ClickCloseEditModeButtonPresentForVarietyTypeForInvestmentOption(
                PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                PositionSizeVarietyTypes.YourAmountAtRisk,
                PositionSizeEditModeTypes.RiskPercent,
                PositionSizeClosingEditModeTypes.Ok);

            Checker.IsFalse(investmentOptionSection.IsEditPositionSizeForVarietyTypeForInvestmentOptionDisabled(
                    PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeEditModeTypes.RiskPercent),
                "Risk: 1.00% of the total capital in portfolio field is disabled for Your Amount after Risk editing for Risk Percentage of Portfolio");
            Checker.IsFalse(investmentOptionSection.IsEditPositionSizeForVarietyTypeForInvestmentOptionDisabled(
                    PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                    PositionSizeVarietyTypes.SharesToBuy,
                    PositionSizeEditModeTypes.PurchasePrice),
                "Purchase Price field is disabled for Shares to Buy after Risk editing for Risk Percentage of Portfolio");
            Checker.IsTrue(investmentOptionSection.IsEditPositionSizeForVarietyTypeForInvestmentOptionPresent(
                    PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeEditModeTypes.RiskPercent),
                "Edit sign for Risk field is NOT Shown for Your Amount after Risk editing for Risk Percentage of Portfolio");

            var changedInvestmentAmount = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                PositionSizeVarietyTypes.YourInvestmentAmount,
                PositionSizeMainNumberTypes.Amount);
            Checker.CheckNotEquals(actualInvestmentAmount, changedInvestmentAmount,
                $"Risk Percentage of Portfolio - Your Investment Amount - Main Value is NOT changed after changing Your Amount at Risk for {ticker}");

            var changedRiskAtInvestmentAmount = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                PositionSizeVarietyTypes.YourAmountAtRisk,
                PositionSizeMainNumberTypes.Amount);
            Checker.CheckNotEquals(actualRiskAtInvestmentAmount, changedRiskAtInvestmentAmount,
                $"Risk Percentage of Portfolio - Your Amount at Risk - Additional Description is NOT changed after changing Your Amount at Risk for {ticker}");

            var changedSharesToBuy = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                PositionSizeVarietyTypes.SharesToBuy,
                PositionSizeMainNumberTypes.Amount);
            Checker.CheckNotEquals(actualShareToBuy, changedSharesToBuy,
                $"Risk Percentage of Portfolio - Shares to Buy - Main Value is not as expected after changing Your Amount at Risk for {ticker}");

            var changedRiskPercentWording = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                PositionSizeVarietyTypes.YourAmountAtRisk,
                PositionSizeMainNumberTypes.AdditionalOptionOrContractSize);
            var changedRiskPercentValue = Constants.DecimalNumberWithoutSignRegex.Match(changedRiskPercentWording).Value;
            Checker.CheckEquals(StringUtility.SetFormatFromSample(expectedRiskPercent, changedRiskPercentValue),
                changedRiskPercentValue, "Risk: at editing for Risk Percentage of Portfolio is not as expected");

            LogStep(5, "Click on 'edit' icon in the 4rd subsection of 'Risk Percentage of Portfolio' section near 'Purchase Price'.");
            investmentOptionSection.ClickEditPositionSizeForVarietyTypeForInvestmentOption(
                PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio, PositionSizeVarietyTypes.SharesToBuy,
                PositionSizeEditModeTypes.PurchasePrice);

            var actualPurschasePrice = investmentOptionSection.GetValueOfEditedPositionSizeForVarietyTypeForInvestmentOption(
                    PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                    PositionSizeVarietyTypes.SharesToBuy,
                    PositionSizeEditModeTypes.PurchasePrice);
            Checker.CheckEquals($"{expectedCurrencySign}{StringUtility.SetFormatFromSample(latestCloseDbValue, StringUtility.ReplaceAllCurrencySigns(actualPurschasePrice))}",
                actualPurschasePrice.Replace(",", string.Empty),
                "Purchase Price at editing for Risk Percentage of Portfolio is not as expected");
            Checker.IsTrue(investmentOptionSection.IsClosingEditModeButtonPresentForVarietyTypeForInvestmentOption(
                    PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                    PositionSizeVarietyTypes.SharesToBuy,
                    PositionSizeEditModeTypes.PurchasePrice,
                    PositionSizeClosingEditModeTypes.Ok),
                "Close Action 'Ok' for Your Amount at Risk at editing for Risk Percentage of Portfolio is not shown");
            Checker.IsTrue(investmentOptionSection.IsClosingEditModeButtonPresentForVarietyTypeForInvestmentOption(
                    PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                    PositionSizeVarietyTypes.SharesToBuy,
                    PositionSizeEditModeTypes.PurchasePrice,
                    PositionSizeClosingEditModeTypes.Cancel),
                "Close Action 'Cancel' for Your Amount at Risk at editing for Risk Percentage of Portfolio is not shown");
            Checker.IsTrue(investmentOptionSection.IsEditPositionSizeForVarietyTypeForInvestmentOptionDisabled(
                    PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeEditModeTypes.RiskPercent),
                "Risk (VQ%) field is NOT disabled for Shares to Buy at Risk editing for Risk Percentage of Portfolio");
            Checker.IsFalse(investmentOptionSection.IsEditPositionSizeForVarietyTypeForInvestmentOptionPresent(
                    PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                    PositionSizeVarietyTypes.SharesToBuy,
                    PositionSizeEditModeTypes.PurchasePrice),
                "Edit sign for Purchase Price field is Shown for Risk at editing for Risk Percentage of Portfolio");

            LogStep(6, "Set value according to test data.");
            investmentOptionSection.SetValueForVarietyTypeForInvestmentOption(
                    PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                    PositionSizeVarietyTypes.SharesToBuy,
                    PositionSizeEditModeTypes.PurchasePrice,
                    purchasePrice);

            LogStep(7, "Click 'Ok' link.");
            investmentOptionSection.ClickCloseEditModeButtonPresentForVarietyTypeForInvestmentOption(
                PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                PositionSizeVarietyTypes.SharesToBuy,
                PositionSizeEditModeTypes.PurchasePrice,
                PositionSizeClosingEditModeTypes.Ok);

            Checker.IsFalse(investmentOptionSection.IsEditPositionSizeForVarietyTypeForInvestmentOptionDisabled(
                    PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                    PositionSizeVarietyTypes.YourAmountAtRisk,
                    PositionSizeEditModeTypes.RiskPercent),
                "Risk (VQ%) field is disabled for Your Amount after Risk editing for Risk Percentage of Portfolio");
            Checker.IsFalse(investmentOptionSection.IsEditPositionSizeForVarietyTypeForInvestmentOptionDisabled(
                    PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                    PositionSizeVarietyTypes.SharesToBuy,
                    PositionSizeEditModeTypes.PurchasePrice),
                    "Purchase Price field is disabled for Shares to Buy after Risk editing for Risk Percentage of Portfolio");
            Checker.IsTrue(investmentOptionSection.IsEditPositionSizeForVarietyTypeForInvestmentOptionPresent(
                    PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                    PositionSizeVarietyTypes.SharesToBuy,
                    PositionSizeEditModeTypes.PurchasePrice),
                "Edit sign for Purchase Price field is NOT Shown for Your Amount after Risk editing for Risk Percentage of Portfolio");

            var finalInvestmentAmount = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                PositionSizeVarietyTypes.YourInvestmentAmount,
                PositionSizeMainNumberTypes.Amount);
            Checker.CheckNotEquals(changedInvestmentAmount, finalInvestmentAmount,
                $"Risk Percentage of Portfolio - Your Investment Amount - Main Value is NOT changed after changing Purchase Price for {ticker}");

            var finalRiskAtInvestmentAmount = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                PositionSizeVarietyTypes.YourAmountAtRisk,
                PositionSizeMainNumberTypes.Amount);
            Checker.CheckEquals(changedRiskAtInvestmentAmount, finalRiskAtInvestmentAmount,
                $"Risk Percentage of Portfolio - Your Amount at Risk - Additional Description is changed after changing Purchase Price for {ticker}");

            var finalRiskPercentWording = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                PositionSizeVarietyTypes.YourAmountAtRisk,
                PositionSizeMainNumberTypes.AdditionalOptionOrContractSize);
            Checker.CheckEquals(changedRiskPercentWording, finalRiskPercentWording,
                $"Risk Percentage of Portfolio - Your Amount at Risk - Risk percent wording is changed after changing Purchase Price for {ticker}");

            var finalSharesToBuy = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                PositionSizeVarietyTypes.SharesToBuy,
                PositionSizeMainNumberTypes.Amount);
            Checker.CheckNotEquals(changedSharesToBuy, finalSharesToBuy,
                $"Risk Percentage of Portfolio - Shares to Buy - Main Value is not as expected after changing Purchase Price for {ticker}");

            var finalPurschasePrice = investmentOptionSection.GetMainNumberTypeForVarietyTypeForInvestmentOptionPresent(
                    PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio,
                    PositionSizeVarietyTypes.SharesToBuy,
                    PositionSizeMainNumberTypes.AdditionalOptionOrContractSize);
            var expectedPurchasePrice = string.Format(expectedPurchasePriceWording, expectedCurrencySign, Parsing.ConvertToDouble(purchasePrice).ToString("N2"));
            Checker.CheckEquals(expectedPurchasePrice, finalPurschasePrice,
                "Risk Percentage of Portfolio - Shares to Buy - Purchase Price is not as expected");

            LogStep(8, "Click Add to watchlist");
            investmentOptionSection.ClickActionButtoForInvestmentOption(PositionSizeInvestmentOptionTypes.RiskPercentageOfPortfolio, PositionSizeActionTypes.AddToWatchlist);
            var addPositionAdvancedForm = new AddPositionAdvancedForm();
            Checker.CheckEquals(ticker, addPositionAdvancedForm.GetSymbolTreeSelectSingleValue(), "Ticker is not as expected at adding");
            Checker.IsTrue(addPositionAdvancedForm.IsBtnTradeTypeActive(tradeType), "Trade Type is not as expected at adding");

            var sharesAtAdding = addPositionAdvancedForm.GetValueFromTextBoxField(AddPositionAdvancedFields.Shares);
            Checker.CheckEquals(StringUtility.SetFormatFromSample(finalSharesToBuy, sharesAtAdding), sharesAtAdding.Replace(",", string.Empty),
                "Shares is not as expected at adding");
            var formattedpurchasePrice = $"{expectedCurrencySign}{Parsing.ConvertToDouble(purchasePrice):N2}";
            Checker.CheckEquals(formattedpurchasePrice.TrimEnd('0').TrimEnd('0').TrimEnd('.'),
                addPositionAdvancedForm.GetValueFromTextBoxField(AddPositionAdvancedFields.EntryPrice),
                "Entry price is not as expected at adding");

            LogStep(9, "Select portfolio and click save");
            new AddPositionAdvancedSteps().SelectPortfolioByIdClickSaveWaitPositionCard(portfolioId);
            var positionDetailsTabPositionCardForm = new PositionDetailsTabPositionCardForm();
            Checker.CheckEquals(ticker, new PositionCardForm().GetSymbol(), "Ticker is not as expected on card");
            Checker.CheckEquals(tradeType == PositionTradeTypes.Long, positionDetailsTabPositionCardForm.IsTradeTypeLong(),
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