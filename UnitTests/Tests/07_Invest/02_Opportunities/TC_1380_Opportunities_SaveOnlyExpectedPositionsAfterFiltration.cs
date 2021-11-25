using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Enums.FilterEnums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Forms.OpportunitiesForm;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.PositionsAlertsForm.Models;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms;
using AutomatedTests.Models.FiltersModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System;
using TradeStops.Common.Enums;
using TradeStops.Common.Utils;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._07_Invest._02_Opportunities
{
    [TestClass]
    public class TC_1380_Opportunities_SaveOnlyExpectedPositionsAfterFiltration : BaseTestUnitTests
    {
        private const int TestNumber = 1380;
        private const int MinPositionsQuantity = 1;
        private const int MinPercentOfSelectedToSavePositions = 10;
        private const int MaxPercentOfSelectedToSavePositions = 75;

        private AddToPortfolioSelectType addToPortfolioKind;
        private string portfolioNameToAdd;
        private string emptyStrategyWording;
        private string textAfterSaving;
        private string viewName;
        private string dateForEntryDate;
        private bool isResultGridNonEmpty;

        private List<string> strategiesToSelect;
        private List<string> defaultStrategies;

        private readonly List<PositionsGridDataField> columnsToAddInView = new List<PositionsGridDataField>
        {
            PositionsGridDataField.EntryDate, PositionsGridDataField.EntryPrice, PositionsGridDataField.TradeType,
            PositionsGridDataField.Health, PositionsGridDataField.Vq, PositionsGridDataField.Average30YearsVolatilityQuotient,
            PositionsGridDataField.Newsletters, PositionsGridDataField.Strategies, PositionsGridDataField.Volume,
            PositionsGridDataField.PriceEarnings, PositionsGridDataField.TrailingDividendYield, PositionsGridDataField.MarketCap,
            PositionsGridDataField.Sector, PositionsGridDataField.LatestClose
        };
        private readonly List<PositionsGridDataField> columnsToCollectDataOnGrid = new List<PositionsGridDataField>();

        private readonly List<SymbolTypes> availableAssetTypes = new List<SymbolTypes> { SymbolTypes.Stock, SymbolTypes.Crypto, SymbolTypes.Index, SymbolTypes.Fund };

        private readonly OpportunitiesFiltersModel defaultOpportunitiesFiltersModel = new OpportunitiesFiltersModel();
        private readonly OpportunitiesFiltersModel expectedOpportunitiesFiltersModel = new OpportunitiesFiltersModel();

        [TestInitialize]
        public void TestInitialize()
        {
            var userProductSubscriptions = GetUserProductSubscriptions("userSubscription");

            columnsToCollectDataOnGrid.AddRange(columnsToAddInView);
            columnsToCollectDataOnGrid.AddRange(new List<PositionsGridDataField>
            {
                PositionsGridDataField.Ticker, PositionsGridDataField.Name, PositionsGridDataField.Status
            });

            var portfolioModel = new PortfolioDBModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = $"{(int)GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType")}",
                CurrencyId = $"{(int)GetTestDataParsedAsEnumFromStringMapping<Currency>("Currency")}"
            };

            for (int i = 1; i <= availableAssetTypes.Count; i++)
            {
                defaultOpportunitiesFiltersModel.AssetTypeFilter.AssetFilterNameToState.Add(
                    GetTestDataAsString($"DefaultFilter2Option{i}"), GetTestDataAsBool($"DefaultFilter2OptionValue{i}"));
            }
            for (int i = 1; i <= EnumUtils.GetValues<SortResultTypes>().Count(); i++)
            {
                defaultOpportunitiesFiltersModel.SortFilter.SortFilterTypeToState.Add(
                    (SortResultTypes)Enum.Parse(typeof(SortResultTypes), GetTestDataAsString($"DefaultFilter3Option{i}")),
                    GetTestDataAsString($"DefaultFilter3OptionValue{i}"));
            }
            var availableCountriesCount = EnumUtils.GetValues<CountryOfExchangeTypes>().Count();
            for (int i = 1; i <= availableCountriesCount; i++)
            {
                expectedOpportunitiesFiltersModel.CountryOfExchangeFilter.CountryOfExchangeFilterNameToState.Add(
                    GetTestDataParsedAsEnumFromStringMapping<CountryOfExchangeTypes>($"Filter1Option{i}"), GetTestDataAsBool($"DefaultFilter2OptionValue{i}"));
            }
            for (int i = 1; i <= availableAssetTypes.Count; i++)
            {
                expectedOpportunitiesFiltersModel.AssetTypeFilter.AssetFilterNameToState.Add(
                    GetTestDataAsString($"Filter2Option{i}"), GetTestDataAsBool($"DefaultFilter2OptionValue{i}"));
            }
            for (int i = 1; i <= EnumUtils.GetValues<SortResultTypes>().Count(); i++)
            {
                expectedOpportunitiesFiltersModel.SortFilter.SortFilterTypeToState.Add(
                    (SortResultTypes)Enum.Parse(typeof(SortResultTypes), GetTestDataAsString($"DefaultFilter3Option{i}")),
                    GetTestDataAsString($"Filter3OptionValue{i}"));
            }
            defaultStrategies = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(defaultStrategies)).OrderBy(p => p).ToList();
            strategiesToSelect = GetTestDataValuesAsListByColumnNameAndRemoveEmpty("Strategies").ToList();

            addToPortfolioKind = GetTestDataParsedAsEnumFromStringMapping<AddToPortfolioSelectType>(nameof(addToPortfolioKind));
            portfolioNameToAdd = addToPortfolioKind == AddToPortfolioSelectType.New
                ? StringUtility.RandomString(GetTestDataAsString("PortfolioName"))
                : portfolioModel.Name;
            textAfterSaving = GetTestDataAsString(nameof(textAfterSaving));
            emptyStrategyWording = GetTestDataAsString(nameof(emptyStrategyWording));
            isResultGridNonEmpty = GetTestDataAsBool(nameof(isResultGridNonEmpty));

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));
            PortfoliosSetUp.AddPortfolioViaDb(UserModels.First(), portfolioModel);

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);

            var mainMenuNavigation = new MainMenuNavigation();
            mainMenuNavigation.OpenPositionsGrid();
            viewName = StringUtility.RandomString("View#######");
            new PositionsTabForm().AddANewViewWithCheckboxesMarked(viewName, columnsToAddInView.Select(t => t.GetStringMapping()).ToList());
            mainMenuNavigation.OpenOpportunities();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1380$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("https://tr.a1qa.com/index.php?/cases/view/21123565 The test checks adding several positions from Opportunities after some filtration.")]
        [TestCategory("Smoke"), TestCategory("Opportunities")]
        public override void RunTest()
        {
            LogStep(1, "Check that default state of the Strategies dropdown and union filter");
            var opportunitiesForm = new OpportunitiesForm();
            var strategyFilterModel = opportunitiesForm.GetCurrentStrategyFilterModel();
            var actualDefaultStrategyState = strategyFilterModel.StrategyFilterNameToState.Keys
                .Select(t => t.GetStringMapping()).OrderBy(p => p).ToList();
            Checker.CheckListsEquals(defaultStrategies, actualDefaultStrategyState,
                "Default item in the Strategies dropdown is not as expected");
            Checker.CheckEquals(StrategyFilterToggleState.Any, strategyFilterModel.StrategyFilterToggleCurrentState,
                "Default union for the Strategies dropdown is not as expected");

            LogStep(2, "Check that grid is not empty");
            Checker.IsTrue(opportunitiesForm.IsResultGridOrNoResultMessagePresent(), "Grid is not as expected");

            LogStep(3, "Click Filters");
            opportunitiesForm.ClickFiltersButton();
            foreach (var opportunitiesFilterType in EnumUtils.GetValues<OpportunitiesFilterType>())
            {
                Checker.IsTrue(opportunitiesForm.IsFilterExist(opportunitiesFilterType),
                    $"{opportunitiesFilterType.GetStringMapping()} filter is not shown");
            }

            LogStep(4, "Check that default states of shown filters");
            var countryOfExchangeFilterModel = opportunitiesForm.GetCurrentCountryOfExchangeFilterModel();
            foreach (var country in EnumUtils.GetValues<CountryOfExchangeTypes>())
            {
                Checker.IsFalse(countryOfExchangeFilterModel.CountryOfExchangeFilterNameToState[country],
                    $"Default item in the Country Of Exchange is not as expected for {country}");
            }
            var assetTypeFilterModel = opportunitiesForm.GetCurrentAssetTypeFilterModel();
            Checker.CheckEquals(defaultOpportunitiesFiltersModel.AssetTypeFilter, assetTypeFilterModel,
                "Default state of Asset Type Filter is not as expected");
            var sortFilterModel = opportunitiesForm.GetCurrentSortFilterModel();
            Checker.CheckEquals(defaultOpportunitiesFiltersModel.SortFilter, sortFilterModel,
                "Default state of Sort Filter is not as expected");

            LogStep(5, "Click clear in dropdown. Check that grid is missing");
            opportunitiesForm.ClearDropdown();
            Checker.IsFalse(opportunitiesForm.IsResultGridPresent(),
                "Empty Strategy does not cause expected wording in grid");
            Checker.CheckContains(emptyStrategyWording, opportunitiesForm.GetNoResultMessage(),
                "Empty Strategy does not cause expected wording in grid");

            LogStep(6, "Set Country of exchange according to test data");
            opportunitiesForm.SetCountryOfExchangeFilter(expectedOpportunitiesFiltersModel.CountryOfExchangeFilter);
            Checker.CheckEquals(expectedOpportunitiesFiltersModel.CountryOfExchangeFilter, opportunitiesForm.GetCurrentCountryOfExchangeFilterModel(),
                "Country of Exchanges Filters are not equal before filtering");

            LogStep(7, "Set Asset Type according to test data");
            opportunitiesForm.SetAssetTypeFilter(expectedOpportunitiesFiltersModel.AssetTypeFilter);
            Checker.CheckEquals(expectedOpportunitiesFiltersModel.AssetTypeFilter, opportunitiesForm.GetCurrentAssetTypeFilterModel(),
                "Asset Type Filters are not equal before filtering");

            LogStep(8, "Set Display & Sort Results according to test data");
            opportunitiesForm.SetCurrentSortFilterModel(expectedOpportunitiesFiltersModel.SortFilter);
            Checker.CheckEquals(expectedOpportunitiesFiltersModel.SortFilter, opportunitiesForm.GetCurrentSortFilterModel(),
                "Sort Filters are not equal before filtering");

            LogStep(9, "Click Apply filter");
            opportunitiesForm.ClickFiltersApplyingButton(FiltersApplyingButtonTypes.Apply);
            foreach (var opportunitiesFilterType in EnumUtils.GetValues<OpportunitiesFilterType>())
            {
                Checker.IsFalse(opportunitiesForm.IsFilterExist(opportunitiesFilterType),
                    $"{opportunitiesFilterType.GetStringMapping()} filter is shown");
            }

            LogStep(10, "Select strategies according to test data");
            opportunitiesForm.SelectStrategiesMultipleByListOfText(strategiesToSelect);
            var actualStrategies = opportunitiesForm.GetSelectedItemsInDropdown();

            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(strategiesToSelect, actualStrategies),
                $"Selected Strategies are not as expected\n{GetExpectedResultsString(strategiesToSelect)}\r\n{GetActualResultsString(actualStrategies)}");

            LogStep(11, "Select strategies according to test data");
            var actualGridPresence = opportunitiesForm.IsResultGridPresent();
            Checker.CheckEquals(isResultGridNonEmpty, actualGridPresence,
                "Result grid after filtration and selecting strategies is not as expected");

            if (isResultGridNonEmpty && actualGridPresence)
            {
                LogStep(12, "Mark about 50 % of positions in grid. Remember Opportunities grid for checked positions");
                var positionsQuantity = opportunitiesForm.GetNumberOfRowsInStrategyGrid();
                var elementsIndicesToSelect =
                    Randoms.GetRandomNumbersInRange(MinPositionsQuantity, positionsQuantity, MinPercentOfSelectedToSavePositions, MaxPercentOfSelectedToSavePositions);

                foreach (var positionOrder in elementsIndicesToSelect)
                {
                    opportunitiesForm.SelectPositionCheckboxByPositionOrderAndState(positionOrder, true);
                }
                var opportunitiesGridData = opportunitiesForm.GetOpportunitiesGrid(elementsIndicesToSelect);

                LogStep(13, "Check that footer contains expected quantity of selected positions");
                var addablePositionsQuantity = opportunitiesForm.GetSelectedItemsNumberFromFooter();
                Checker.CheckEquals(elementsIndicesToSelect.Count, addablePositionsQuantity, "Addable positions count is not same as in footer");

                LogStep(14, "Select Portfolios type (new or existed) and portfolio Name according to test data");
                opportunitiesForm.SelectAddToPortfolioItem(addToPortfolioKind);
                Checker.CheckEquals(addToPortfolioKind, opportunitiesForm.GetAddToPortfolioItem(), "Expected portfolio type is not selected");
                if (addToPortfolioKind == AddToPortfolioSelectType.Existed)
                {
                    opportunitiesForm.SelectPortfolioToAdd(portfolioNameToAdd);
                    Checker.CheckEquals(portfolioNameToAdd, opportunitiesForm.GetSelectedPortfolioToAdd(), "Expected portfolio name is not typed for existed portfolio");
                }
                else
                {
                    opportunitiesForm.SetPortfolioName(portfolioNameToAdd);
                    Checker.CheckEquals(portfolioNameToAdd, opportunitiesForm.GetPortfolioNameForAdding(), "Expected portfolio name is not typed for new portfolio");
                }

                LogStep(15, "Click Add");
                opportunitiesForm.ClickAddToPortfolioButton();
                dateForEntryDate = Parsing.ConvertToShortDateString(DateTime.Now.AsShortDate());
                Checker.CheckEquals(textAfterSaving, opportunitiesForm.GetPortfolioActionLabelText(), "Text for saving result is not as expected");

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
                foreach (var opportunitiesPositionData in opportunitiesGridData)
                {
                    CheckPositionsData(positionsData, opportunitiesPositionData);
                }
            }
        }

        private void CheckPositionsData(List<PositionGridModel> positionsData, OpportunitiesGridModel opportunitiesPositionData)
        {
            var tickerAndName = opportunitiesPositionData.Ticker.Split('\r');
            var mappedModel = positionsData.FirstOrDefault(u => u.Ticker == tickerAndName[0]);
            if (mappedModel == null)
            {
                Checker.Fail($"Position {tickerAndName[0]} from Opportunities not found in Positions grid");
            }
            else
            {
                Checker.CheckEquals(tickerAndName[1].Replace("\n", ""), mappedModel.Name,
                    $"Positions names are not equals for {tickerAndName[0]}");
                Checker.CheckEquals(opportunitiesPositionData.Health, mappedModel.Health, $"Health indicators are not equals for {tickerAndName[0]}");
                Checker.CheckEquals(FormatsExtensions.GetFormattedToOneDoubleValue(opportunitiesPositionData.Vq),
                    FormatsExtensions.GetFormattedToOneDoubleValue(mappedModel.Vq), $"Vq are not equals for {tickerAndName[0]}");
                Checker.CheckEquals(FormatsExtensions.GetFormattedToOneDoubleValue(opportunitiesPositionData.AvgVq),
                    FormatsExtensions.GetFormattedToOneDoubleValue(mappedModel.Average30YearsVolatilityQuotient),
                    $"Average Vq are not equals for {tickerAndName[0]}");
                Checker.CheckEquals(opportunitiesPositionData.Newsletters, mappedModel.Newsletters, $"Newsletters are not equals for {tickerAndName[0]}");
                Checker.CheckEquals(opportunitiesPositionData.Strategies, mappedModel.Strategies, $"Strategies are not equals for {tickerAndName[0]}");
                Checker.CheckEquals(opportunitiesPositionData.Volume, mappedModel.Volume, $"Volumes are not equals for {tickerAndName[0]}");

                if (opportunitiesPositionData.PeRatio == Constants.NotAvailableAcronym)
                {
                    Checker.CheckEquals(opportunitiesPositionData.PeRatio, mappedModel.PriceEarnings, $"P/E Ratio N/A are not equals for {tickerAndName[0]}");
                }
                else
                {
                    var expectedPeRatio = Parsing.ConvertToDecimal(opportunitiesPositionData.PeRatio).ToString("N2");
                    var actualPeRatio = Parsing.ConvertToDecimal(mappedModel.PriceEarnings).ToString("N2");
                    Checker.CheckEquals(expectedPeRatio.Replace(",", ""), actualPeRatio.Replace(",", ""), $"P/E Ratio are not equals for {tickerAndName[0]}");
                }

                Checker.CheckEquals(opportunitiesPositionData.DivYield, mappedModel.TrailingDividendYield,
                    $"Dividend Yields are not equals for {tickerAndName[0]}");
                Checker.CheckEquals(opportunitiesPositionData.MarketCap, mappedModel.MarketCap.Replace(Currency.USD.GetDescription(), string.Empty), 
                    $"MarketCap values are not equals for {tickerAndName[0]}");
                Checker.CheckEquals(opportunitiesPositionData.LatestClose, mappedModel.LatestClose,
                    $"Latest Close values are not equals for {tickerAndName[0]}");
                Checker.CheckEquals(opportunitiesPositionData.LatestClose, mappedModel.EntryPrice,
                    $"Entry Price is not as expected for {tickerAndName[0]}");
                Checker.CheckEquals(dateForEntryDate, mappedModel.EntryDate, $"Entry Date are not equals as expected for {tickerAndName[0]}");
                if (opportunitiesPositionData.Sector.Contains(Constants.NotAvailableAcronym))
                {
                    Checker.IsTrue(mappedModel.Sector == Constants.NotAvailableAcronym || mappedModel.Sector == SymbolTypes.Fund.ToString(),
                        $"Sectors are not equals for {tickerAndName[0]}: opportunities: {opportunitiesPositionData.Sector}, grid: {mappedModel.Sector}");
                }
                else
                {
                    Checker.CheckEquals(opportunitiesPositionData.Sector, mappedModel.Sector, $"Sectors are not equals for {tickerAndName[0]}");
                }
            }
        }
    }
}