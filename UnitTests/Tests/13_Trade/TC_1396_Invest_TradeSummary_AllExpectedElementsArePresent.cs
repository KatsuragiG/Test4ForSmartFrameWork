using AutomatedTests.ConstantVariables;
using AutomatedTests.Enums.Chart;
using AutomatedTests.Enums.Dashboard;
using AutomatedTests.Enums.FilterEnums;
using AutomatedTests.Enums.Sorting;
using AutomatedTests.Enums.Tools;
using AutomatedTests.Enums.TradeSummary;
using AutomatedTests.Enums;
using AutomatedTests.Forms.LoginForm;
using AutomatedTests.Forms.Screener;
using AutomatedTests.Forms.Templates;
using AutomatedTests.Forms.Trade;
using AutomatedTests.Forms;
using AutomatedTests.Models.FiltersModels;
using AutomatedTests.Models.TradeSummaryModels;
using AutomatedTests.Models.UserModels;
using AutomatedTests.Navigation;
using AutomatedTests.Steps.BrowserSteps;
using AutomatedTests.Steps.Screener;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System;
using TradeStops.Common.Utils;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._13_Trade
{
    [TestClass]
    public class TC_1396_Invest_TradeSummary_AllExpectedElementsArePresent : BaseTestUnitTests
    {
        private string login;
        private string password;
        private string expectedHeader;
        private string optionTicker;
        private string actionLabel;
        private string quantityLabel;
        private string prefilledQuantity;
        private string expectedBestScenario;
        private string expectedWorstScenario;
        private string checklistHeader;
        private string defaultChecklist;
        private string checklistLinkUrl;
        private string checklistLinkWording;
        private string checklistLinkInDropdownWording;
        private string checklistSummary;
        private string emptyKeyStatisticWording;
        private int expectedCriteriaQuantity;
        private bool isPopCalculated;

        private List<string> columnNames;
        private List<string> benefitLabels;
        private List<string> profitAxisNames;
        private List<string> probabilityAxisNames;
        private TileCarouselPageTypes defaultCarouselPageType;
        private OptionScreenerColumnTypes sortByColumn;
        private SortingStatus sortStatus;
        private TileModel tileModel;
        private CreditPillType expectedCreditPill;
        private readonly OptionSourceFilterModel optionSourceFilterModel = new OptionSourceFilterModel();
        private readonly IncludeFilterModel includeFilterModel = new IncludeFilterModel();
        private readonly OptionTradeTypeFilterModel optionTypeFilterModel = new OptionTradeTypeFilterModel();
        private readonly List<OptionScreenerFilterTypes> requiredFilters = new List<OptionScreenerFilterTypes> { OptionScreenerFilterTypes.OptionTradeType };
        private readonly List<string> itemsToReplaceForOneCriterion = new List<string> { "criteria", "criterion" };

        [TestInitialize]
        public void TestInitialize()
        {
            login = GetTestDataAsString(nameof(login));
            password = GetTestDataAsString(nameof(password));
            expectedHeader = GetTestDataAsString(nameof(expectedHeader));
            actionLabel = GetTestDataAsString(nameof(actionLabel));
            quantityLabel = GetTestDataAsString(nameof(quantityLabel));
            prefilledQuantity = GetTestDataAsString(nameof(prefilledQuantity));
            expectedBestScenario = GetTestDataAsString(nameof(expectedBestScenario));
            expectedWorstScenario = GetTestDataAsString(nameof(expectedWorstScenario));
            checklistHeader = GetTestDataAsString(nameof(checklistHeader));
            defaultChecklist = GetTestDataAsString(nameof(defaultChecklist));
            checklistLinkUrl = GetTestDataAsString(nameof(checklistLinkUrl));
            checklistLinkWording = GetTestDataAsString(nameof(checklistLinkWording));
            checklistLinkInDropdownWording = GetTestDataAsString(nameof(checklistLinkInDropdownWording));
            checklistSummary = GetTestDataAsString(nameof(checklistSummary));
            emptyKeyStatisticWording = GetTestDataAsString(nameof(emptyKeyStatisticWording));

            isPopCalculated = GetTestDataAsBool(nameof(isPopCalculated));
            expectedCriteriaQuantity = GetTestDataAsInt(nameof(expectedCriteriaQuantity));
            columnNames = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(columnNames));
            benefitLabels = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(benefitLabels));
            profitAxisNames = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(profitAxisNames));
            probabilityAxisNames = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(probabilityAxisNames));

            sortStatus = GetTestDataParsedAsEnumFromStringMapping<SortingStatus>(nameof(sortStatus));
            sortByColumn = GetTestDataParsedAsEnumFromStringMapping<OptionScreenerColumnTypes>(nameof(sortByColumn));
            expectedCreditPill = GetTestDataParsedAsEnumFromStringMapping<CreditPillType>(nameof(expectedCreditPill));
            optionSourceFilterModel.SourceItem = GetTestDataParsedAsEnumFromStringMapping<OptionScreenerSourceTypes>("source");
            optionSourceFilterModel.SubSourceItems = GetTestDataValuesAsListByColumnNameAndRemoveEmpty("market");
            includeFilterModel.IncludeFilterNameToState.Add(IncludeFilterTypes.UnderlyingAssets, true);
            includeFilterModel.IncludeFilterNameToState.Add(IncludeFilterTypes.Options, false);
            defaultCarouselPageType = GetTestDataParsedAsEnumFromStringMapping<TileCarouselPageTypes>(nameof(defaultCarouselPageType));

            for (int i = 1; i <= EnumUtils.GetValues<CoPilotOptionType>().Count(); i++)
            {
                optionTypeFilterModel.OptionTypeToState.Add(
                    GetTestDataParsedAsEnumFromStringMapping<CoPilotOptionType>($"Filter1Option{i}"),
                    GetTestDataAsBool($"Filter1OptionValue{i}"));
            }

            LogStep(0, "Precondition - Login as user with subscription to Portfolio Tracker");
            UserModels.Add(new UserModel { Email = login, Password = password });

            var mainMenuNavigation = new MainMenuNavigation();
            mainMenuNavigation.OpenQaLoginForm();

            var loginForm = new LoginForm();
            loginForm.AssertIsOpen();
            loginForm.LogInWithoutDbWaiting(UserModels.First());
            loginForm.CloseWalkMePopupIfExists();

            new MainMenuForm().ClickMenuItem(MainMenuItems.Trade);
            new TradeMenuForm().ClickTradeMenuItem(TradeMenuItems.OptionsScreener);
            new SavedOptionScreenersForm().ClickNewScreener();

            new ScreenerSteps().MakeOnlyRequiredFiltersActive(requiredFilters);
            var optionScreenerForm = new OptionScreenerForm();
            optionScreenerForm.SetOptionSourceFilter(optionSourceFilterModel);
            optionScreenerForm.SetIncludeFilter(includeFilterModel);
            optionScreenerForm.SetOptionTypeFilter(optionTypeFilterModel);
            optionScreenerForm.SetSortByDropdown(sortByColumn.GetStringMapping());
            optionScreenerForm.SetSortDirectionDropdown(sortStatus);

            var optionScreenerGridForm = new OptionScreenerGridForm();
            optionScreenerGridForm.ClickRunScreener();
            optionScreenerGridForm.AssertIsOpen();
            var tilesQuantity = optionScreenerGridForm.GetNumberOfOptionsInTileOnCurrentPage();
            Assert.IsTrue(tilesQuantity > 0, "There are no Tile object after running screener");

            var tilesModels = new List<TileModel>();
            for (int i = 1; i <= tilesQuantity; i++)
            {
                tilesModels.Add(optionScreenerGridForm.GetTileModelByOrder(i));
            }
            Assert.IsTrue(tilesModels.Any(), "There are no Tiles models before clicking");

            var appropriateTilesOrders = tilesModels.Select((tile, order) => (tile, order))
                .Where(pair => (pair.tile.Pop.Equals(Constants.DefaultPopValueIfNotExist) || pair.tile.Pop.Equals(Constants.NotAvailableAcronym)) == !isPopCalculated)
                .Select(pair => pair.order).ToList();
            Assert.IsTrue(appropriateTilesOrders.Any(), "There are no suitable Tiles models before clicking");

            var tileOrderToClick = appropriateTilesOrders.ElementAt(SRandom.Instance.Next(1, appropriateTilesOrders.Count));
            tileModel = tilesModels[tileOrderToClick - 1];

            optionScreenerGridForm.ClickTradeSummaryInTile(tileOrderToClick);
            optionTicker = StringUtility.GetOptionTickerForUsdOptionName(tileModel.OptionName);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1396$", DataAccessMethod.Sequential)]
        [Description("The test checks Trade Summary page elements https://tr.a1qa.com/index.php?/cases/view/22094403")]
        [TestMethod]
        [TestCategory("TradeSummary"), TestCategory("OptionScreener")]
        public override void RunTest()
        {
            LogStep(1, "Check that new tab is shown");
            var tradeSummaryForm = new BrowserSteps().CheckThatNewTabIsOpenedGetPage<TradeSummaryForm>();

            LogStep(2, $"Check that Trade Summary page is shown {Browser.GetDriver().Url}");
            tradeSummaryForm.AssertIsOpen();

            LogStep(3, "Check that URL contains correct symbolId from precondition");
            var currentUrl = Browser.GetDriver().Url;
            Checker.CheckContains(tileModel.Id.ToString(), currentUrl, "Trade Summary page has wrong id in Url");

            LogStep(4, "Check that page header has correct wording according to test data");
            Checker.CheckEquals(string.Format(expectedHeader, optionTicker),
                tradeSummaryForm.GetPageHeader(), "Unexpected page header");

            LogStep(5, "Check that table contains 5 columns");
            Checker.CheckListsEquals(columnNames,
                tradeSummaryForm.GetTransactionColumnsHeaders(), "Unexpected transaction form headers");

            LogStep(6, "Check that action has label 'Sell to Open' for Short and 'Buy to Open' for long");
            Checker.CheckEquals(actionLabel, tradeSummaryForm.GetActionText(), "Unexpected Action label");

            LogStep(7, "Check prefilled quantity value and there are wording Contract");
            var transactionModel = tradeSummaryForm.GetTransactionModel();
            Checker.CheckEquals(prefilledQuantity, transactionModel.Quantity, "Unexpected prefilled Quantity");
            Checker.CheckEquals(quantityLabel, tradeSummaryForm.GetQuantityText(), "Unexpected Quantity label");
            Checker.CheckEquals(expectedHeader.Split(' ')[1], transactionModel.Type, "Unexpected Type Value");

            LogStep(8, "Check that Expiration Date, Strike Price are corresponded to option data");
            var parsedOptionName = tileModel.OptionName.Split(' ');
            var expectedExpirationDate = DateTime.Parse($"{parsedOptionName[5]}{parsedOptionName[6]}{parsedOptionName[7]}").AsShortDate();
            var actualExpirationDate = DateTime.Parse(transactionModel.ExpirationDate).AsShortDate();
            Checker.CheckEquals(expectedExpirationDate, actualExpirationDate, "Unexpected Expiration Date");
            var currencySign = Constants.AllCurrenciesRegex.Match(transactionModel.StrikePrice).Value;
            var actualFormattedStrikePrice =
                $"{currencySign}{StringUtility.ReplaceAllCurrencySigns(transactionModel.StrikePrice).ToFractionalString()}";
            Checker.CheckEquals(parsedOptionName[2].Replace(",", string.Empty), actualFormattedStrikePrice, "Unexpected Strike Price");
            Checker.IsTrue(tradeSummaryForm.IsTransactionFieldDisabled(TransactionSummaryFieldTypes.ExpirationDate),
                "Expiration Date is not disabled");
            Checker.IsTrue(tradeSummaryForm.IsTransactionFieldDisabled(TransactionSummaryFieldTypes.StrikePrice),
                "Strike Price is not disabled");
            Checker.IsFalse(tradeSummaryForm.IsTransactionFieldDisabled(TransactionSummaryFieldTypes.EntryPrice),
                "Entry Price is disabled");
            Checker.IsFalse(tradeSummaryForm.IsTransactionFieldDisabled(TransactionSummaryFieldTypes.Quantity),
                "Quantity is disabled");
            var isCreditPillShown = tradeSummaryForm.IsCreditPillShown();
            Checker.IsTrue(isCreditPillShown, "Credit Pill is not shown");
            if (isCreditPillShown)
            {
                Checker.CheckEquals(expectedCreditPill.GetDescription(), tradeSummaryForm.GetCreditPillAcronym(), "Credit Pill Acronym is not as expected");
                Checker.CheckEquals(expectedCreditPill.GetStringMapping(), tradeSummaryForm.GetCreditPillHint(), "Credit Pill Hint is not as expected");
            }

            LogStep(9, "Check summary benefits labels - Best scenario and Worst scenario");
            Checker.CheckListsEquals(benefitLabels,
                tradeSummaryForm.GetScenarioHeaders(), "Unexpected transaction form headers");

            LogStep(10, "Check that scenarios wording matched expectations according to test data");
            currencySign = Constants.AllCurrenciesRegex.Match(parsedOptionName[2]).Value;
            var strikePriceDouble = Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(parsedOptionName[2]));
            var stopPrice = (optionTypeFilterModel.OptionTypeToState[CoPilotOptionType.BuyPut] || optionTypeFilterModel.OptionTypeToState[CoPilotOptionType.SellPut])
                ? (strikePriceDouble - Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(transactionModel.EntryPrice)))
                : (strikePriceDouble + Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(transactionModel.EntryPrice)));
            var profitLimited = optionTypeFilterModel.OptionTypeToState[CoPilotOptionType.BuyPut]
                ? (stopPrice * Constants.DefaultContractSize).ToString("#,##0.00")
                : (Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(transactionModel.EntryPrice)) * Constants.DefaultContractSize).ToString("#,##0.00");
            var expectedScenarioWording = string.Format(expectedBestScenario, parsedOptionName[1], currencySign, strikePriceDouble.ToString("#,##0.00"),
                transactionModel.ExpirationDate, profitLimited, tileModel.Pop);
            Checker.CheckEquals(expectedScenarioWording, tradeSummaryForm.GetScenarioWording(TradeScenarioTypes.BestScenario),
                "Unexpected Best Scenario wording");

            var probability = tileModel.Pop.Equals(Constants.NotAvailableAcronym)
                ? Constants.NotAvailableAcronym
                : (100 - Parsing.ConvertToDouble(tileModel.Pop.Replace("%", string.Empty))).ToString("N2") + "%";
            profitLimited = optionTypeFilterModel.OptionTypeToState[CoPilotOptionType.SellPut]
                ? (stopPrice * Constants.DefaultContractSize).ToString("#,##0.00")
                : (Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(transactionModel.EntryPrice)) * Constants.DefaultContractSize).ToString("#,##0.00");
            expectedScenarioWording = string.Format(expectedWorstScenario, parsedOptionName[1], currencySign, strikePriceDouble.ToString("#,##0.00"),
                transactionModel.ExpirationDate, profitLimited, probability);
            Checker.CheckEquals(expectedScenarioWording, tradeSummaryForm.GetScenarioWording(TradeScenarioTypes.WorstScenario),
                "Unexpected Worst Scenario wording");

            LogStep(11, "Check that checklist Results header is present");
            Checker.CheckEquals(checklistHeader, tradeSummaryForm.GetChecklistHeader(), "Unexpected Checklist header");

            LogStep(12, "Check that there is dropdown for checklists with prefilled value according to default checklist");
            var parsedExpectedHeader = expectedHeader.Split(' ');
            Checker.CheckEquals(string.Format(defaultChecklist, $"{Dictionaries.OptionActionToTradeType[parsedExpectedHeader[0]]} {parsedExpectedHeader[1]}"),
                tradeSummaryForm.GetSelectedChecklistName(), "Unexpected Default Item in Checklist dropdown");

            LogStep(13, "Check that dropdown has + Add New Checklist link and URL is correct");
            Checker.CheckContains(checklistLinkInDropdownWording, tradeSummaryForm.GetAddNewChecklistInDropdownText(),
                "Unexpected Add New Checklist link wording in Checklist dropdown");
            tradeSummaryForm.ClickAddNewChecklistInDropdown();
            var createCheckListForm = new CreateCheckListForm();
            createCheckListForm.AssertIsOpen();
            createCheckListForm.ClickBack();
            tradeSummaryForm.AssertIsOpen();

            LogStep(14, "Check that Checklists Management link has correct wording and URL");
            Checker.CheckContains(checklistLinkUrl, tradeSummaryForm.GetChecklistUrl(), "Unexpected Checklists Management link");
            Checker.CheckEquals(checklistLinkWording, tradeSummaryForm.GetChecklistManagementText(), "Unexpected Checklists Management text");

            LogStep(15, "Check that there are non-empty criteria wording and results in quantity as expectedCriteriaQuantity");
            var checklistResults = tradeSummaryForm.GetChecklistResultsModels();
            Checker.CheckEquals(expectedCriteriaQuantity, checklistResults.Count, "Unexpected Checklists results quantity");
            Checker.IsTrue(checklistResults.Select(t => !string.IsNullOrEmpty(t.RuleWording)).Contains(true),
                $"Checklists rules wording has empty value: {GetActualResultsString(checklistResults.Select(t => t.RuleWording).ToList())}");
            Checker.IsTrue(checklistResults.Select(t => !string.IsNullOrEmpty(t.RuleResultsWording)).Contains(true),
                $"Checklists rules result wording has empty value: {GetActualResultsString(checklistResults.Select(t => t.RuleResultsWording).ToList())}");

            LogStep(16, "Check that there are non-empty criteria wording and results in quantity as expectedCriteriaQuantity");
            var successQuantity = checklistResults.Count(t => t.RuleResult == ChecklistResultTypes.Success);
            var expectedWordingChecklistSummary = successQuantity == 1
                ? checklistSummary.Replace(itemsToReplaceForOneCriterion.First(), itemsToReplaceForOneCriterion.Last())
                : checklistSummary;
            var expectedChecklistSummary = string.Format(expectedWordingChecklistSummary, successQuantity, checklistResults.Count);
            Checker.CheckEquals(expectedChecklistSummary, tradeSummaryForm.GetChecklistSummaryText(), "Unexpected Checklists Summary text");

            LogStep(17, "Check that there is carousel with default item Key Statistics");
            Checker.IsTrue(tradeSummaryForm.IsCarouselShown(), "Carousel is not shown");
            Checker.CheckEquals(defaultCarouselPageType, tradeSummaryForm.GetCurrentCarouselPageType(), "Unexpected Default Carousel page type");

            LogStep(18, "Check that Key Statistics contains three non-empty facts");
            var funFactsWidget = tradeSummaryForm.KeyStatisticsWidget;
            Checker.IsTrue(funFactsWidget.IsExists(), "Key Statistics Widget is NOT exist");
            var actualFunFacts = funFactsWidget.GetAllKeyStatisticsText();
            Checker.IsTrue(actualFunFacts.Any() || funFactsWidget.GetNoFactsText().Contains(emptyKeyStatisticWording),
                "Key Statistics are empty or does not contain no results message");

            LogStep(19, 20, "Click Arrow right and check that Profit / Loss item is shown. Check that profit loss chart has correct internal structure");
            tradeSummaryForm.ClickArrow(CarouselActionArrows.Next);
            var profitLossWidget = tradeSummaryForm.ProfitLossWidget;
            Checker.IsTrue(profitLossWidget.IsExists(), "Profit / Loss Widget is NOT exist");
            Checker.CheckEquals(TileCarouselPageTypes.ProfitLossChart, tradeSummaryForm.GetCurrentCarouselPageType(), "Profit/Loss Carousel page type is not current");
            Checker.IsTrue(profitLossWidget.IsProfitLossChartPriceLinePresent(), "Option price line is NOT shown");
            Checker.IsTrue(profitLossWidget.IsStrikeLinePresent(), "Strike price line is NOT shown for Profit / Loss Widget");
            Checker.IsTrue(profitLossWidget.IsProfitMarkerPresent(), "Profit marker is NOT shown");
            Checker.CheckEquals($"{parsedExpectedHeader[0].ToUpperInvariant()} {parsedExpectedHeader[1].ToUpperInvariant()}", profitLossWidget.GetChartName(),
                "Unexpected chart name");
            Checker.CheckListsEquals(profitAxisNames, profitLossWidget.GetAllAxisTitles(),
                "Unexpected Axis titles for Profit / Loss Widget");

            LogStep(21, 22, "Click Arrow right and check that Probability of underlying stock price change item is shown. Check that chart has correct internal structure");
            tradeSummaryForm.ClickArrow(CarouselActionArrows.Next);
            var underlyPriceForecastWidget = tradeSummaryForm.UnderlyPriceForecastWidget;
            Checker.IsTrue(underlyPriceForecastWidget.IsExists(), "Probability of underlying stock Widget is NOT exist");
            Checker.CheckEquals(TileCarouselPageTypes.UnderlyPriceForecast, tradeSummaryForm.GetCurrentCarouselPageType(), "Underly Price Forecast type is not current");
            var isNoDataShouldBeShown = underlyPriceForecastWidget.IsNoDataToDisplayShown() && !isPopCalculated;
            Checker.IsTrue(underlyPriceForecastWidget.IsPriceLinePresent(ChartLineTypes.Price) || isNoDataShouldBeShown,
                "Underly price line is NOT shown");
            Checker.IsTrue(underlyPriceForecastWidget.IsPriceLinePresent(ChartLineTypes.StrikePrice) || isNoDataShouldBeShown,
                "Strike price line is NOT shown for Probability of underlying");
            Checker.IsTrue(underlyPriceForecastWidget.IsPriceLinePresent(ChartLineTypes.ExpirationDate) || isNoDataShouldBeShown,
                "Expiration date line is NOT shown");
            Checker.IsTrue(underlyPriceForecastWidget.IsPriceLinePresent(ChartLineTypes.LowPriceProbability) || isNoDataShouldBeShown,
                "Low Price Probability line is NOT shown");
            Checker.IsTrue(underlyPriceForecastWidget.IsPriceLinePresent(ChartLineTypes.MediumPriceProbability) || isNoDataShouldBeShown,
                "Medium Price Probability line is NOT shown");
            Checker.IsTrue(underlyPriceForecastWidget.IsPriceLinePresent(ChartLineTypes.HighPriceProbability) || isNoDataShouldBeShown,
                "High Price Probability line is NOT shown");
            if (!isNoDataShouldBeShown)
            {
                Checker.CheckListsEquals(probabilityAxisNames, underlyPriceForecastWidget.GetAllAxisTitles(),
                    "Unexpected Axis titles for Probability of underlying Widget");
            }

            LogStep(23, "Check that Add to portfolio shown");
            Checker.IsTrue(tradeSummaryForm.IsAddToPortfolioButtonShown(), "Add to Portfolio & Choose Exit Strategy button is not shown");

            LogStep(24, "Check clicking Add to portfolio causes appearing Exit Strategy form with at least two sections");
            tradeSummaryForm.ClickAddToPortfolioButton();
            var exitStrategyForm = new ExitStrategyForm();
            Checker.IsTrue(exitStrategyForm.AreAddToPortfolioAndAlertsSectionsShown(), "Two sections are NOT shown on Exit Strategy form");
        }

        [TestCleanup]
        public new void CleanAfterTest()
        {
            IsDeleteUserViaApi = false;
            base.CleanAfterTest();
        }
    }
}