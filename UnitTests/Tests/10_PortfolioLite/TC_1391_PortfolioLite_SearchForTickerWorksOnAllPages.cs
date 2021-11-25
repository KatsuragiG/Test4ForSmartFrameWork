using System.Collections.Generic;
using System.Linq;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.DsiForSymbols;
using AutomatedTests.Database.Positions;
using AutomatedTests.Database.Users;
using AutomatedTests.Enums.PortfolioLite;
using AutomatedTests.Enums.Tools;
using AutomatedTests.Forms.PortfolioLite;
using AutomatedTests.Models.PositionsModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._10_PortfolioLite
{
    [TestClass]
    public class TC_1391_PortfolioLite_SearchForTickerWorksOnAllPages : BaseTestUnitTests
    {
        private const int TestNumber = 1391;

        private PortfolioLitePositionModel positionModel;
        private List<string> validTickers;
        private List<string> validNames;
        private List<string> validNameTickers;
        private List<string> invalidTickers;
        private int minimalCountOfTreeSelectItems;
        private string itemPattern;
        private string autocompleteOnFormDescription;

        [TestInitialize]
        public void TestInitialize()
        {
            positionModel = new PortfolioLitePositionModel
            {
                Ticker = GetTestDataAsString("symbolToAdd"),
                IsLongType = true
            };

            validTickers = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(validTickers));
            validNames = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(validNames));
            validNameTickers = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(validNameTickers));
            invalidTickers = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(invalidTickers));

            itemPattern = GetTestDataAsString(nameof(itemPattern));
            minimalCountOfTreeSelectItems = GetTestDataAsInt(nameof(minimalCountOfTreeSelectItems));
            autocompleteOnFormDescription = GetTestDataAsString(nameof(autocompleteOnFormDescription));

            LogStep(0, "Preconditions. Create user with subscription to PortfolioLite. Add position");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.PortfolioLite));

            var fakeSnaid = StringUtility.RandomString(Constants.SnaidPattern);
            new UsersQueries().UpdateUserSnaid(fakeSnaid, UserModels.First().TradeSmithUserId);
            new PortfolioLiteNavigation().OpenPortfolioLiteWithUserGuid(UserModels.First());

            var portfolioLiteMainForm = new PortfolioLiteMainForm();
            portfolioLiteMainForm.ClickAddAPosition();
            portfolioLiteMainForm.AddPosition(positionModel);
            portfolioLiteMainForm.ExitFrame();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1391$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("PortfolioLite")]
        [Description("Test checks that search for ticker works on all pages https://tr.a1qa.com/index.php?/cases/view/21116192")]
        public override void RunTest()
        {
            LogStep(1, 10, "Main Portfolio lite page: Check Search for valid/Invalid tickers");
            var portfolioLiteMainForm = new PortfolioLiteMainForm();
            EnterSymbolOrNameToSearchFieldAndCheck(portfolioLiteMainForm, portfolioLiteMainForm.GetFormTitle());

            LogStep(11, "Repeat steps 1-10 after Click Add positions: Check Search for valid/Invalid tickers");
            portfolioLiteMainForm.ClickAddAPosition();
            var autocompleteDescription = $"{portfolioLiteMainForm.GetFormTitle()} {autocompleteOnFormDescription}";
            CheckAutocompleteAtPositionAdding(portfolioLiteMainForm, autocompleteDescription);

            LogStep(12, "Repeat steps 1-10 - Portfolio lite Position card: Check Search for valid/Invalid tickers");
            portfolioLiteMainForm.ClickPositionLinkInGridByNumber(minimalCountOfTreeSelectItems);
            var portfolioLiteCardForm = new PortfolioLiteCardForm();
            EnterSymbolOrNameToSearchFieldAndCheck(portfolioLiteCardForm, portfolioLiteCardForm.GetFormTitle());

            LogStep(13, "Portfolio lite Analyzer: Check Search for valid/Invalid tickers");
            portfolioLiteMainForm.SearhSymbol(validTickers.First());
            var portfolioLiteAnalyzerForm = new PortfolioLiteAnalyzerForm();
            EnterSymbolOrNameToSearchFieldAndCheck(portfolioLiteAnalyzerForm, portfolioLiteAnalyzerForm.GetFormTitle());

            LogStep(14, "Portfolio lite Analyzer internal autocomplete: Check Search for valid/Invalid tickers");
            autocompleteDescription = $"{portfolioLiteAnalyzerForm.GetFormTitle()} {autocompleteOnFormDescription}";
            CheckAutocompleteOnAnalyzer(portfolioLiteAnalyzerForm, autocompleteDescription);

            LogStep(15, "Portfolio lite Position Size: Check Search for valid/Invalid tickers");
            portfolioLiteAnalyzerForm.ClickAdditionalActionsButton(PortfolioLiteAdditionalButtons.PositionSize);
            var portfolioLitePositionSizeForm = new PortfolioLitePositionSizeForm();
            EnterSymbolOrNameToSearchFieldAndCheck(portfolioLitePositionSizeForm, portfolioLitePositionSizeForm.GetFormTitle());

            LogStep(16, "Portfolio lite Position Size internal autocomplete: Check Search for valid/Invalid tickers");
            autocompleteDescription = $"{portfolioLitePositionSizeForm.GetFormTitle()} {autocompleteOnFormDescription}";
            CheckAutocompleteOnPositionSize(portfolioLitePositionSizeForm, autocompleteDescription);

            LogStep(17, "Portfolio lite Add To Position form: Check Search for valid/Invalid tickers");
            portfolioLitePositionSizeForm.ClickActionButtoForInvestmentOption(PositionSizeInvestmentOptionTypes.DollarInvestmentRisk);
            var portfolioLiteAddToPortfolioForm = new PortfolioLiteAddToPortfolioForm();
            EnterSymbolOrNameToSearchFieldAndCheck(portfolioLiteAddToPortfolioForm, portfolioLiteAddToPortfolioForm.GetFormTitle());

            LogStep(18, "Portfolio lite Add To Position internal autocomplete: Check Search for valid/Invalid tickers");
            autocompleteDescription = $"{portfolioLiteAddToPortfolioForm.GetFormTitle()} {autocompleteOnFormDescription}";
            CheckAutocompleteOnAddToPortfolioForm(portfolioLiteAddToPortfolioForm, autocompleteDescription);
        }

        private void EnterSymbolOrNameToSearchFieldAndCheck(BasePortfolioLiteForm portfolioLiteMainForm, string pageName)
        {
            LogStep(1, 2, "Check search via ticker in different cases");
            foreach (var ticker in validTickers)
            {
                var formattedItems = TypeInSearchAndCheckItemsQuantityAutocompleteAndGetIt(portfolioLiteMainForm, pageName, ticker, minimalCountOfTreeSelectItems);

                CheckValidAutocompleteResults(pageName, ticker, formattedItems);
            }

            for (int i = 0; i < validNames.Count; i++)
            {
                var formattedItems = TypeInSearchAndCheckItemsQuantityAutocompleteAndGetIt(portfolioLiteMainForm, pageName, validNames[i], minimalCountOfTreeSelectItems);

                CheckValidAutocompleteResults(pageName, validNameTickers[i], formattedItems);
            }

            foreach (var invalidTicker in invalidTickers)
            {
                TypeInSearchAndCheckItemsQuantityAutocompleteAndGetIt(portfolioLiteMainForm, pageName, invalidTicker, 0);
            }
        }

        private void CheckValidAutocompleteResults(string pageName, string ticker, List<string> formattedItems)
        {
            var positionsQueries = new PositionsQueries();
            var symbolsQueries = new SymbolsQueries();

            var tickerName = positionsQueries.SelectSymbolIdNameUsingSymbol(ticker).SymbolName;
            var exchangeName = symbolsQueries.SelectDataFromHDSymbols(ticker).ExchangeName;
            var expectedItemName = string.Format(itemPattern, ticker.ToUpperInvariant(), exchangeName, tickerName);

            Checker.IsTrue(formattedItems.Contains(expectedItemName),
                $"List of tree select items on page {pageName} doesn't contain expected item [{expectedItemName}]\nFound items:\n{string.Join("\n", formattedItems)}");
        }

        private List<string> TypeInSearchAndCheckItemsQuantityAutocompleteAndGetIt(BasePortfolioLiteForm portfolioLiteMainForm, string pageName, string searchText, int minimalItemsInResult)
        {
            var itemsInSearch = portfolioLiteMainForm.GetItemsInSymbolTreeSelectAutocomplete(searchText);

            return CheckItemsQuantityInSearchAutocompleteAndGetIt(pageName, itemsInSearch, minimalItemsInResult);
        }

        private List<string> CheckItemsQuantityInSearchAutocompleteAndGetIt(string pageName, List<string> itemsInSearch, int minimalItemsInResult)
        {
            var formattedItems = itemsInSearch.Select(e => e.ReplaceNewLineWithTrim()).ToList();
            Checker.IsTrue(minimalItemsInResult <= itemsInSearch.Count,
                $"Count of tree select items {itemsInSearch.Count} is not as expected {minimalCountOfTreeSelectItems} on page {pageName}. Found items:\n{string.Join("\n", formattedItems)}");

            return formattedItems;
        }

        private void CheckAutocompleteOnAddToPortfolioForm(PortfolioLiteAddToPortfolioForm portfolioLiteAddToPortfolioForm, string autocompleteDescription)
        {
            foreach (var ticker in validTickers)
            {
                var formattedItems = portfolioLiteAddToPortfolioForm.GetItemsInSymbolTreeSelectAutocompleteOnForm(ticker);

                CheckValidAutocompleteResults(autocompleteDescription, ticker, formattedItems);
            }

            for (int i = 0; i < validNames.Count; i++)
            {
                var formattedItems = portfolioLiteAddToPortfolioForm.GetItemsInSymbolTreeSelectAutocompleteOnForm(validNames[i]);

                CheckValidAutocompleteResults(autocompleteDescription, validNameTickers[i], formattedItems);
            }

            foreach (var invalidTicker in invalidTickers)
            {
                var formattedItems = portfolioLiteAddToPortfolioForm.GetItemsInSymbolTreeSelectAutocompleteOnForm(invalidTicker);
                CheckItemsQuantityInSearchAutocompleteAndGetIt(autocompleteDescription, formattedItems, 0);
            }
        }

        private void CheckAutocompleteOnPositionSize(PortfolioLitePositionSizeForm portfolioLitePositionSizeForm, string autocompleteDescription)
        {
            foreach (var ticker in validTickers)
            {
                var formattedItems = portfolioLitePositionSizeForm.GetItemsInSymbolTreeSelectAutocompleteOnForm(ticker);

                CheckValidAutocompleteResults(autocompleteDescription, ticker, formattedItems);
            }

            for (int i = 0; i < validNames.Count; i++)
            {
                var formattedItems = portfolioLitePositionSizeForm.GetItemsInSymbolTreeSelectAutocompleteOnForm(validNames[i]);

                CheckValidAutocompleteResults(autocompleteDescription, validNameTickers[i], formattedItems);
            }

            foreach (var invalidTicker in invalidTickers)
            {
                var formattedItems = portfolioLitePositionSizeForm.GetItemsInSymbolTreeSelectAutocompleteOnForm(invalidTicker);
                CheckItemsQuantityInSearchAutocompleteAndGetIt(autocompleteDescription, formattedItems, 0);
            }
        }

        private void CheckAutocompleteOnAnalyzer(PortfolioLiteAnalyzerForm portfolioLiteAnalyzerForm, string autocompleteDescription)
        {
            foreach (var ticker in validTickers)
            {
                var formattedItems = portfolioLiteAnalyzerForm.GetItemsInSymbolTreeSelectAutocompleteOnForm(ticker);

                CheckValidAutocompleteResults(autocompleteDescription, ticker, formattedItems);
            }

            for (int i = 0; i < validNames.Count; i++)
            {
                var formattedItems = portfolioLiteAnalyzerForm.GetItemsInSymbolTreeSelectAutocompleteOnForm(validNames[i]);

                CheckValidAutocompleteResults(autocompleteDescription, validNameTickers[i], formattedItems);
            }

            foreach (var invalidTicker in invalidTickers)
            {
                var formattedItems = portfolioLiteAnalyzerForm.GetItemsInSymbolTreeSelectAutocompleteOnForm(invalidTicker);
                CheckItemsQuantityInSearchAutocompleteAndGetIt(autocompleteDescription, formattedItems, 0);
            }
        }

        private void CheckAutocompleteAtPositionAdding(PortfolioLiteMainForm portfolioLiteMainForm, string autocompleteDescription)
        {
            foreach (var ticker in validTickers)
            {
                var formattedItems = portfolioLiteMainForm.GetItemsInSymbolTreeSelectAutocompleteOnForm(ticker);

                CheckValidAutocompleteResults(autocompleteDescription, ticker, formattedItems);
            }

            for (int i = 0; i < validNames.Count; i++)
            {
                var formattedItems = portfolioLiteMainForm.GetItemsInSymbolTreeSelectAutocompleteOnForm(validNames[i]);

                CheckValidAutocompleteResults(autocompleteDescription, validNameTickers[i], formattedItems);
            }

            foreach (var invalidTicker in invalidTickers)
            {
                var formattedItems = portfolioLiteMainForm.GetItemsInSymbolTreeSelectAutocompleteOnForm(invalidTicker);
                CheckItemsQuantityInSearchAutocompleteAndGetIt(autocompleteDescription, formattedItems, 0);
            }
        }
    }
}