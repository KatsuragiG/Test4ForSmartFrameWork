using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Enums;
using AutomatedTests.Enums.FilterEnums;
using AutomatedTests.Enums.Newsletter;
using AutomatedTests.Enums.Opportunities;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.Tools;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Gurus;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.PositionsAlertsForm.Models;
using AutomatedTests.Models.FiltersModels;
using AutomatedTests.Models.Screener;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Enums;
using TradeStops.Common.Utils;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._08_MyGurus._02_TopRecommendations
{
    [TestClass]
    public class TC_1386_Gurus_TopRecommendation_СheckThatFiltrationWorksAsExpected : BaseTestUnitTests
    {
        private const int TestNumber = 1386;
        private const int MinPositionsQuantity = 1;

        private readonly List<PositionsGridDataField> columnsToAddInView = new List<PositionsGridDataField>
        {
            PositionsGridDataField.EntryDate, PositionsGridDataField.EntryPrice, PositionsGridDataField.TradeType,
            PositionsGridDataField.Health, PositionsGridDataField.Vq, PositionsGridDataField.Average30YearsVolatilityQuotient,
            PositionsGridDataField.Newsletters, PositionsGridDataField.Strategies, PositionsGridDataField.Volume,
            PositionsGridDataField.PriceEarnings, PositionsGridDataField.TrailingDividendYield, PositionsGridDataField.MarketCap,
            PositionsGridDataField.Sector, PositionsGridDataField.LatestClose
        };
        private readonly List<PositionsGridDataField> columnsToCollectDataOnGrid = new List<PositionsGridDataField>();
        private List<AllStrategiesTypes> defaultStrategies = new List<AllStrategiesTypes>();
        private List<AllStrategiesTypes> expectedStrategies = new List<AllStrategiesTypes>();
        private List<string> expectedNewsletters = new List<string>();

        private AddToPortfolioSelectType addToPortfolioKind;
        private string portfolioNameToAdd;
        private string viewName;
        private string textAfterSaving;
        private string dateForEntryDate;
        private bool isResultGridNonEmpty;
        private bool isDefaultResultGridNonEmpty;

        private readonly TopRecommendationFiltersModel defaultTopRecommendationFiltersModel = new TopRecommendationFiltersModel();
        private readonly TopRecommendationFiltersModel expectedTopRecommendationFiltersModel = new TopRecommendationFiltersModel();
        private readonly StrategyFilterModel emptyStrategyFilter = new StrategyFilterModel();

        [TestInitialize]
        public void TestInitialize()
        {
            var userProductSubscriptions = GetUserProductSubscriptions("userSubscription");

            var portfolioModel = new PortfolioDBModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = $"{(int)GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType")}",
                CurrencyId = $"{(int)GetTestDataParsedAsEnumFromStringMapping<Currency>("Currency")}"
            };

            columnsToCollectDataOnGrid.AddRange(columnsToAddInView);
            columnsToCollectDataOnGrid.AddRange(new List<PositionsGridDataField>
            {
                PositionsGridDataField.Ticker, PositionsGridDataField.Name, PositionsGridDataField.Status
            });

            isResultGridNonEmpty = GetTestDataAsBool(nameof(isResultGridNonEmpty));
            isDefaultResultGridNonEmpty = GetTestDataAsBool(nameof(isDefaultResultGridNonEmpty));
            addToPortfolioKind = GetTestDataParsedAsEnumFromStringMapping<AddToPortfolioSelectType>(nameof(addToPortfolioKind));
            portfolioNameToAdd = addToPortfolioKind == AddToPortfolioSelectType.New
                ? StringUtility.RandomString(GetTestDataAsString("PortfolioName"))
                : portfolioModel.Name;
            textAfterSaving = GetTestDataAsString(nameof(textAfterSaving));

            defaultStrategies = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(defaultStrategies))
                .Select(p => p.ParseAsEnumFromStringMapping<AllStrategiesTypes>()).ToList();
            expectedStrategies = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(expectedStrategies))
                .Select(p => p.ParseAsEnumFromStringMapping<AllStrategiesTypes>()).ToList();
            foreach (var strategy in expectedStrategies)
            {
                expectedTopRecommendationFiltersModel.StrategyFilter.StrategyFilterNameToState.Add(strategy, true);
            }
            var excludedStrategies = Constants.AvailableStrategyTypes.Except(expectedStrategies);
            foreach (var strategy in excludedStrategies)
            {
                expectedTopRecommendationFiltersModel.StrategyFilter.StrategyFilterNameToState.Add(strategy, false);
            }
            foreach (var strategy in Constants.AvailableStrategyTypes)
            {
                emptyStrategyFilter.StrategyFilterNameToState.Add(strategy, false);
            }

            defaultTopRecommendationFiltersModel.RankFilter = new RankFilterModel { SliderState = SliderFilterStates.No };
            expectedTopRecommendationFiltersModel.RankFilter = new RankFilterModel { SliderState = GetTestDataParsedAsEnumFromStringMapping<SliderFilterStates>("expectedRankSliderState") };

            defaultTopRecommendationFiltersModel.NewslettersFilter = new NewslettersFilterModel { SliderState = SliderFilterStates.Yes };
            expectedTopRecommendationFiltersModel.NewslettersFilter = new NewslettersFilterModel { SliderState = SliderFilterStates.Yes };

            expectedTopRecommendationFiltersModel.StrategyFilter.StrategiesCalendarDaysState = GetTestDataAsInt("Filter1Days");
            expectedTopRecommendationFiltersModel.StrategyFilter.StrategyFilterToggleCurrentState = GetTestDataParsedAsEnumFromStringMapping<StrategyFilterToggleState>("Filter1Union");

            expectedNewsletters = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(expectedNewsletters));
            foreach (var newsletter in expectedNewsletters)
            {
                expectedTopRecommendationFiltersModel.NewslettersFilter.ActiveNewsletterName.Add(newsletter);
            }

            for (int i = 1; i <= EnumUtils.GetValues<SortResultTypes>().Count(); i++)
            {
                defaultTopRecommendationFiltersModel.SortFilter.SortFilterTypeToState.Add(
                    (SortResultTypes)Enum.Parse(typeof(SortResultTypes), GetTestDataAsString($"DefaultFilter4Option{i}")),
                    GetTestDataAsString($"DefaultFilter4OptionValue{i}"));
            }

            for (int i = 1; i <= EnumUtils.GetValues<SortResultTypes>().Count(); i++)
            {
                expectedTopRecommendationFiltersModel.SortFilter.SortFilterTypeToState.Add(
                    (SortResultTypes)Enum.Parse(typeof(SortResultTypes), GetTestDataAsString($"DefaultFilter4Option{i}")),
                    GetTestDataAsString($"Filter4OptionValue{i}"));
            }

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));
            PortfoliosSetUp.AddPortfolioViaDb(UserModels.First(), portfolioModel);

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);

            new MainMenuNavigation().OpenPositionsGrid();
            viewName = StringUtility.RandomString("View#######");
            new PositionsTabForm().AddANewViewWithCheckboxesMarked(viewName, columnsToAddInView.Select(t => t.GetStringMapping()).ToList());
            new MainMenuForm().ClickMenuItem(MainMenuItems.MyGurus);
            new GurusMenuForm().ClickGurusMenuItem(GurusMenuItems.TopRecomendations);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1386$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("https://tr.a1qa.com/index.php?/cases/view/21338289 The test checks filtration for Top Recommendation page.")]
        [TestCategory("Smoke"), TestCategory("NewsLettersPage")]
        public override void RunTest()
        {
            LogStep(1, "Make sure that grid is not empty");
            var gurusTopRecommendationsForm = new GurusTopRecommendationsForm();
            Checker.CheckEquals(isDefaultResultGridNonEmpty, gurusTopRecommendationsForm.IsResultGridPresent(), "Default state of result grid is not as expected");

            LogStep(2, "Click Filters");
            gurusTopRecommendationsForm.ClickFiltersButton();
            foreach (var topRecommendationFilterType in EnumUtils.GetValues<TopRecommendationFilterType>())
            {
                Checker.IsTrue(gurusTopRecommendationsForm.IsFilterExist(topRecommendationFilterType),
                    $"{topRecommendationFilterType.GetStringMapping()} filter is not shown");
            }

            LogStep(3, "Check that default state of TradeSmith Strategies filter is matched expectations according to test data");
            var actualDefaultStrategies = gurusTopRecommendationsForm.GetSelectedStrategies();
            Checker.CheckListsEquals(defaultStrategies, actualDefaultStrategies,
                "Default state of TradeSmith Strategies filter is not as expected");

            LogStep(4, "Remove all default strategies in the TradeSmith Strategies filter");
            gurusTopRecommendationsForm.SetStrategyFilter(emptyStrategyFilter);
            Checker.CheckListsEquals(new List<AllStrategiesTypes>(),
                gurusTopRecommendationsForm.GetSelectedStrategies(),
                "State of TradeSmith Strategies filter after clearings not as expected");
            Checker.CheckEquals(isDefaultResultGridNonEmpty, gurusTopRecommendationsForm.IsResultGridPresent(),
                "State of result grid after clearings TradeSmith Strategies filter is not as expected");
            Checker.IsTrue(gurusTopRecommendationsForm.IsFiltersApplyingButtonActive(FiltersApplyingButtonTypes.Apply),
                $"Apply button is disabled after clearing {TopRecommendationFilterType.TradeSmithStrategies.GetStringMapping()}");

            LogStep(5, "Check that default state of Rank recommendation filter is No");
            Checker.CheckEquals(defaultTopRecommendationFiltersModel.RankFilter.SliderState,
                gurusTopRecommendationsForm.GetCurrentStateRankFilter().SliderState,
                "Default state of Rank recommendation is not as expected");

            LogStep(6, "Check that default state of Newsletters Recommendations filter is enabled. Check that dropdown contains publishers");
            var actualDefaultNewsletterFilter = gurusTopRecommendationsForm.GetCurrentStateNewsletterFilter();
            Checker.CheckEquals(defaultTopRecommendationFiltersModel.NewslettersFilter.SliderState,
                actualDefaultNewsletterFilter.SliderState,
                "Default state of Newsletters Recommendations is not as expected");
            Checker.IsTrue(actualDefaultNewsletterFilter.ActiveNewsletterName.Count > 0, "Default Newsletters does not contain any published items");
            foreach (var activeNewsletterName in actualDefaultNewsletterFilter.ActiveNewsletterName)
            {
                Checker.IsTrue(Constants.NewslettersPublishersNames.Contains(activeNewsletterName),
                    $"'{activeNewsletterName}' is not included into publishers: '{Constants.NewslettersPublishersNames.Aggregate(string.Empty, (current, value) => current + "\n" + value)}");
            }

            LogStep(7, "Remove all items from the Newsletters Recommendations filter's dropdown");
            gurusTopRecommendationsForm.SetNewsletterFilter(new NewslettersFilterModel());
            Checker.CheckListsEquals(new List<string>(),
                gurusTopRecommendationsForm.GetCurrentStateNewsletterFilter().ActiveNewsletterName,
                "State of Newsletters Recommendations filter after clearings not as expected");
            Checker.CheckEquals(isDefaultResultGridNonEmpty, gurusTopRecommendationsForm.IsResultGridPresent(),
                "State of result grid after clearings Newsletters Recommendations filter is not as expected");
            Checker.IsFalse(gurusTopRecommendationsForm.IsFiltersApplyingButtonActive(FiltersApplyingButtonTypes.Apply),
                $"Apply button is NOT disabled after clearing {TopRecommendationFilterType.NewslettersRecommendations.GetStringMapping()}");

            LogStep(8, "Check that default state of Display & Sort Results filter is Top100 + sort by health + direction ASC");
            var sortFilterModel = gurusTopRecommendationsForm.GetCurrentSortFilterModel();
            Checker.CheckEquals(defaultTopRecommendationFiltersModel.SortFilter, sortFilterModel,
                "Default state of Sort Filter is not as expected");

            LogStep(9, "Set filters in state from test data");
            gurusTopRecommendationsForm.SetAllFilters(expectedTopRecommendationFiltersModel);
            Checker.CheckEquals(expectedTopRecommendationFiltersModel.StrategyFilter,
                gurusTopRecommendationsForm.GetCurrentStrategyFilterModel(),
                "Current state of Strategy filter is not as expected");
            Checker.CheckEquals(expectedTopRecommendationFiltersModel.NewslettersFilter,
                gurusTopRecommendationsForm.GetCurrentStateNewsletterFilter(),
                "Current state of Newsletters Recommendations is not as expected");
            Checker.CheckEquals(expectedTopRecommendationFiltersModel.RankFilter,
                gurusTopRecommendationsForm.GetCurrentStateRankFilter(),
                "Current state of Rank recommendation is not as expected");
            Checker.CheckEquals(expectedTopRecommendationFiltersModel.SortFilter,
                gurusTopRecommendationsForm.GetCurrentSortFilterModel(),
                "Current state of Sort Filter is not as expected");

            LogStep(10, "Click Apply filter");
            gurusTopRecommendationsForm.ClickFiltersApplyingButton(FiltersApplyingButtonTypes.Apply);
            foreach (var topRecommendationFilterType in EnumUtils.GetValues<TopRecommendationFilterType>())
            {
                Checker.IsFalse(gurusTopRecommendationsForm.IsFilterExist(topRecommendationFilterType),
                    $"{topRecommendationFilterType.GetStringMapping()} filter is shown after Applying");
            }

            LogStep(11, "Check that grid contains positions according to isResultGridNonEmpty");
            var actualGridPresence = gurusTopRecommendationsForm.IsResultGridPresent();
            Checker.CheckEquals(isResultGridNonEmpty, actualGridPresence, "Result grid after filtration is not as expected");
            if (isResultGridNonEmpty && actualGridPresence)
            {
                var actualPositionsQuantity = gurusTopRecommendationsForm.GetNumberOfRowsInGrid();
                var resultsQuantityLabel = gurusTopRecommendationsForm.GetResultsQuantityMessage();
                Checker.CheckEquals(actualPositionsQuantity,
                    Parsing.ConvertToInt(Constants.NumbersRegex.Match(resultsQuantityLabel).Value),
                    "Results message contains count is not same as in grid");

                LogStep(12, "If grid contains positions - mark random positions (from one to all ) in grid. Remember Top recommendation grid for checked positions");
                var positionsQuantityToMark = SRandom.Instance.Next(MinPositionsQuantity, actualPositionsQuantity);
                var positionsIndicesToMark = Randoms.GetRandomNumbersInRange(MinPositionsQuantity, actualPositionsQuantity, positionsQuantityToMark);
                var topRecommendationsGridData = new List<ScreenerGridModel>();
                foreach (var positionOrder in positionsIndicesToMark)
                {
                    gurusTopRecommendationsForm.SelectPositionCheckboxByPositionOrderAndState(positionOrder, true);
                    topRecommendationsGridData.Add(gurusTopRecommendationsForm.GetResultsRow(positionOrder));
                }

                LogStep(13, "Check that footer contains expected quantity of selected positions");
                var addablePositionsQuantity = gurusTopRecommendationsForm.GetSelectedItemsNumberFromFooter();
                Checker.CheckEquals(positionsIndicesToMark.Count, addablePositionsQuantity, "Addable positions count is not same as in footer");

                LogStep(14, "Select Portfolios type (new or existed) and portfolio Name according to test data");
                gurusTopRecommendationsForm.SelectAddToPortfolioItem(addToPortfolioKind);
                Checker.CheckEquals(addToPortfolioKind, gurusTopRecommendationsForm.GetAddToPortfolioItem(),
                    "Expected portfolio type is not selected");
                if (addToPortfolioKind == AddToPortfolioSelectType.Existed)
                {
                    gurusTopRecommendationsForm.SelectPortfolioToAdd(portfolioNameToAdd);
                    Checker.CheckEquals(portfolioNameToAdd, gurusTopRecommendationsForm.GetSelectedPortfolioToAdd(),
                        "Expected portfolio name is not typed for existed portfolio");
                }
                else
                {
                    gurusTopRecommendationsForm.SetPortfolioName(portfolioNameToAdd);
                    Checker.CheckEquals(portfolioNameToAdd, gurusTopRecommendationsForm.GetPortfolioNameForAdding(),
                        "Expected portfolio name is not typed for new portfolio");
                }

                LogStep(15, "Click Add");
                gurusTopRecommendationsForm.ClickAddToPortfolioButton();
                dateForEntryDate = Parsing.ConvertToShortDateString(DateTime.Now.AsShortDate());
                Checker.CheckEquals(textAfterSaving, gurusTopRecommendationsForm.GetPortfolioActionLabelText(), "Text for saving result is not as expected");

                LogStep(16, "Open positions grid for Portfolio from step 4");
                new MainMenuNavigation().OpenPositionsGrid();
                new PositionsAlertsStatisticsPanelForm().SelectPortfolio(portfolioNameToAdd);
                var positionsTabForm = new PositionsTabForm();
                positionsTabForm.ScrollToLastRow();
                positionsTabForm.SelectView(viewName);
                var addedPositionsQuantity = positionsTabForm.GetNumberOfRowsInGrid();
                Checker.CheckEquals(addablePositionsQuantity, addedPositionsQuantity, "Saved positions quantity is not as expected");

                LogStep(17, "Make sure data match expectation for all added positions");
                var positionsData = positionsTabForm.GetPositionDataForAllPositions(columnsToCollectDataOnGrid);
                foreach (var topRecommendationsPositionData in topRecommendationsGridData)
                {
                    CheckPositionsData(positionsData, topRecommendationsPositionData);
                }
            }
        }

        private void CheckPositionsData(List<PositionGridModel> positionsData, ScreenerGridModel topRecommendationsPositionData)
        {
            var tickerAndName = topRecommendationsPositionData.ScreenerGridColumnToState[ScreenerColumnTypes.Ticker].Split('\r');
            var mappedModel = positionsData.FirstOrDefault(u => u.Ticker == tickerAndName[0]);

            if (mappedModel == null)
            {
                Checker.Fail($"Position {tickerAndName[0]} from Top Recommendation not found in Positions grid");
            }
            else
            {
                Checker.CheckEquals(tickerAndName[1].Replace("\n", ""), mappedModel.Name,
                    $"Positions names are not equals for {tickerAndName[0]}");
                Checker.CheckEquals(topRecommendationsPositionData.HealthZone, mappedModel.Health,
                    $"Health indicators are not equals for {tickerAndName[0]}");
                Checker.CheckEquals(FormatsExtensions.GetFormattedToOneDoubleValue(topRecommendationsPositionData.ScreenerGridColumnToState[ScreenerColumnTypes.Vq]),
                    FormatsExtensions.GetFormattedToOneDoubleValue(mappedModel.Vq),
                    $"Vq are not equals for {tickerAndName[0]}");
                Checker.CheckEquals(FormatsExtensions.GetFormattedToOneDoubleValue(topRecommendationsPositionData.ScreenerGridColumnToState[ScreenerColumnTypes.Average30YearsVqValue]),
                    FormatsExtensions.GetFormattedToOneDoubleValue(mappedModel.Average30YearsVolatilityQuotient),
                    $"Average Vq are not equals for {tickerAndName[0]}");
                Checker.CheckEquals(topRecommendationsPositionData.ScreenerGridColumnToState[ScreenerColumnTypes.Newsletters],
                    mappedModel.Newsletters,
                    $"Newsletters are not equals for {tickerAndName[0]}");
                Checker.CheckEquals(topRecommendationsPositionData.ScreenerGridColumnToState[ScreenerColumnTypes.Strategies],
                    mappedModel.Strategies,
                    $"Strategies are not equals for {tickerAndName[0]}");
                Checker.CheckEquals(topRecommendationsPositionData.ScreenerGridColumnToState[ScreenerColumnTypes.Volume],
                    mappedModel.Volume,
                    $"Volumes are not equals for {tickerAndName[0]}");

                if (topRecommendationsPositionData.ScreenerGridColumnToState[ScreenerColumnTypes.PeRatio] == Constants.NotAvailableAcronym)
                {
                    Checker.CheckEquals(topRecommendationsPositionData.ScreenerGridColumnToState[ScreenerColumnTypes.PeRatio],
                        mappedModel.PriceEarnings,
                        $"P/E Ratio N/A are not equals for {tickerAndName[0]}");
                }
                else
                {
                    var expectedPeRatio = Parsing.ConvertToDecimal(topRecommendationsPositionData.ScreenerGridColumnToState[ScreenerColumnTypes.PeRatio]).ToString("N2");
                    var actualPeRatio = Parsing.ConvertToDecimal(mappedModel.PriceEarnings).ToString("N2");
                    Checker.CheckEquals(expectedPeRatio.Replace(",", ""), actualPeRatio.Replace(",", ""), $"P/E Ratio are not equals for {tickerAndName[0]}");
                }

                Checker.CheckEquals(topRecommendationsPositionData.ScreenerGridColumnToState[ScreenerColumnTypes.DivYield],
                    mappedModel.TrailingDividendYield,
                    $"Dividend Yields are not equals for {tickerAndName[0]}");
                Checker.CheckEquals(topRecommendationsPositionData.ScreenerGridColumnToState[ScreenerColumnTypes.MarketCap],
                    mappedModel.MarketCap,
                    $"MarketCap values are not equals for {tickerAndName[0]}");
                Checker.CheckEquals(topRecommendationsPositionData.ScreenerGridColumnToState[ScreenerColumnTypes.LatestClose],
                    mappedModel.LatestClose,
                    $"Latest Close values are not equals for {tickerAndName[0]}");
                Checker.CheckEquals(topRecommendationsPositionData.ScreenerGridColumnToState[ScreenerColumnTypes.LatestClose],
                    mappedModel.EntryPrice,
                    $"Entry Price is not as expected for {tickerAndName[0]}");
                Checker.CheckEquals(dateForEntryDate, mappedModel.EntryDate,
                    $"Entry Date are not equals as expected for {tickerAndName[0]}");
                var sectorValue = topRecommendationsPositionData.ScreenerGridColumnToState[ScreenerColumnTypes.Sector];
                if (sectorValue.Contains(Constants.NotAvailableAcronym))
                {
                    Checker.IsTrue(mappedModel.Sector == Constants.NotAvailableAcronym || mappedModel.Sector == SymbolTypes.Fund.ToString(),
                        $"Sectors are not equals for {tickerAndName[0]}: opportunities: {sectorValue}, grid: {mappedModel.Sector}");
                }
                else
                {
                    Checker.CheckEquals(sectorValue, mappedModel.Sector, $"Sectors are not equals for {tickerAndName[0]}");
                }
            }
        }
    }
}