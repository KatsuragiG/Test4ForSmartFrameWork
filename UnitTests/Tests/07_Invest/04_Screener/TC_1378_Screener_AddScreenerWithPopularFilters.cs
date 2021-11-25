using System;
using System.Collections.Generic;
using System.Linq;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Enums.FilterEnums;
using AutomatedTests.Enums.Opportunities;
using AutomatedTests.Enums.Tools;
using AutomatedTests.Enums.User;
using AutomatedTests.Forms.Screener;
using AutomatedTests.Models.FiltersModels;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Screener;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using TradeStops.Common.Utils;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._07_Invest._04_Screener
{
    [TestClass]
    public class TC_1378_Screener_AddScreenerWithPopularFilters : BaseTestUnitTests
    {
        private const int TestNumber = 1378;
        private const int UniqueTickerQuantity = 1;
        private readonly List<SymbolTypes> availableAssetTypes =
            EnumUtils.GetValues<SymbolTypes>().ToList()
                .Except(new List<SymbolTypes> { SymbolTypes.SeasonalContracts, SymbolTypes.Option, SymbolTypes.Bond, SymbolTypes.Other }).ToList();

        private readonly ScreenerFiltersModel expectedScreenerFiltersModel = new ScreenerFiltersModel();
        private int filterOrder = 1;

        [TestInitialize]
        public void TestInitialize()
        {
            var userType = GetTestDataParsedAsEnumFromStringMapping<ProductSubscriptionTypes>("user");
            expectedScreenerFiltersModel.HealthStatusFilter = FillDataForHealthFilter("Filter", filterOrder);

            IncrementFilterOrder();
            for (int i = 1; i <= EnumUtils.GetValues<TrendFilterTypes>().Count(); i++)
            {
                expectedScreenerFiltersModel.TrendFilter.TrendFilterNameToState.Add(
                    GetTestDataParsedAsEnumFromStringMapping<TrendFilterTypes>($"Filter{filterOrder}Option{i}"),
                    GetTestDataAsBool($"Filter{filterOrder}OptionValue{i}"));
            }
            IncrementFilterOrder();
            expectedScreenerFiltersModel.EntrySignalFilter.EntrySignalState = GetTestDataAsBool($"Filter{filterOrder}OptionValue1");
            expectedScreenerFiltersModel.EntrySignalFilter.EarlyEntrySignalState = GetTestDataAsBool($"Filter{filterOrder}OptionValue2");
            expectedScreenerFiltersModel.EntrySignalFilter.SignalsCalendarDaysState = GetTestDataAsInt($"Filter{filterOrder}OptionValue3");
            expectedScreenerFiltersModel.StrategyFilter.StrategiesCalendarDaysState = GetTestDataAsInt($"Filter{filterOrder}OptionValue3");

            IncrementFilterOrder();
            expectedScreenerFiltersModel.VqRangeFilter = FillDataForNumericFilter(ScreenerFilterType.VqRange, "Filter", filterOrder);

            foreach (var strategy in Constants.AvailableStrategyTypes)
            {
                expectedScreenerFiltersModel.StrategyFilter.StrategyFilterNameToState.Add(strategy, false);
            }
            IncrementFilterOrder();
            expectedScreenerFiltersModel.StrategyFilter.StrategyFilterNameToState[GetTestDataParsedAsEnumFromStringMapping<AllStrategiesTypes>($"Filter{filterOrder}Option1")] =
                GetTestDataAsBool($"Filter{filterOrder}OptionValue1");

            IncrementFilterOrder();
            expectedScreenerFiltersModel.NewslettersFilter.SliderState = GetTestDataParsedAsEnumFromStringMapping<SliderFilterStates>($"ForFilter{filterOrder}OptionValue1");
            expectedScreenerFiltersModel.NewslettersFilter.ActiveNewsletterName = GetTestDataValuesAsListByColumnName($"Filter{filterOrder}OptionValue2");

            IncrementFilterOrder();
            for (int i = 1; i <= EnumUtils.GetValues<CountryOfExchangeTypes>().Count(); i++)
            {
                expectedScreenerFiltersModel.CountryOfExchangeFilter.CountryOfExchangeFilterNameToState.Add(
                    GetTestDataParsedAsEnumFromStringMapping<CountryOfExchangeTypes>($"Filter{filterOrder}Option{i}"),
                    GetTestDataAsBool($"Filter{filterOrder}OptionValue{i}"));
            }
            IncrementFilterOrder();
            expectedScreenerFiltersModel.SectorFilter.ActiveSectorIndustryNames = GetTestDataValuesAsListByColumnName($"Filter{filterOrder}Option1");

            IncrementFilterOrder();
            for (int i = 1; i <= EnumUtils.GetValues<MarketCapitalizationTypes>().Count(); i++)
            {
                expectedScreenerFiltersModel.MarketCapitalizationFilter.MarketCapFilterNameToState.Add(
                    GetTestDataParsedAsEnumFromStringMapping<MarketCapitalizationTypes>($"Filter{filterOrder}Option{i}"),
                    GetTestDataAsBool($"Filter{filterOrder}OptionValue{i}"));
            }

            foreach (var assetType in availableAssetTypes)
            {
                expectedScreenerFiltersModel.AssetTypeFilter.AssetFilterNameToState.Add(assetType.ToString(), false);
            }
            IncrementFilterOrder();
            expectedScreenerFiltersModel.AssetTypeFilter.AssetFilterNameToState[GetTestDataAsString($"Filter{filterOrder}Option1")] = GetTestDataAsBool($"Filter{filterOrder}OptionValue1");

            IncrementFilterOrder();
            for (int i = 1; i <= EnumUtils.GetValues<SortResultTypes>().Count(); i++)
            {
                expectedScreenerFiltersModel.SortFilter.SortFilterTypeToState.Add(
                    (SortResultTypes)Enum.Parse(typeof(SortResultTypes), GetTestDataAsString($"Filter{filterOrder}Option{i}")),
                    GetTestDataAsString($"Filter{filterOrder}OptionValue{i}"));
            }

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, (ProductSubscriptions)(int)userType));

            new ScreenerSteps().LogInOpenNewScreener(UserModels.First());
        }

        private void IncrementFilterOrder()
        {
            filterOrder++;
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1378$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test checks the addition of popular filters in the screener https://tr.a1qa.com/index.php?/cases/view/20636708")]
        [TestCategory("Smoke"), TestCategory("Screener")]
        public override void RunTest()
        {
            LogStep(1, "Add popular filters to the screener");
            var screenerSteps = new ScreenerSteps();
            screenerSteps.DetectOnlyRequiredFiltersMakeActive(expectedScreenerFiltersModel);

            LogStep(2, "Set filters value according to test data");
            screenerSteps.SetAllFilters(expectedScreenerFiltersModel);

            LogStep(3, 13, "Check Filters on Screener");
            DoSteps3To13();

            LogStep(14, "Click Run Screener");
            var screenerGridForm = new ScreenerGridForm();
            screenerGridForm.ClickRunScreener();
            var gridDataBefore = screenerGridForm.GetValuesInScreenerGrid();
            Assert.IsTrue(gridDataBefore.Count > 0, "Grid does not contains data");

            LogStep(15, "Click Save Screener");
            var screenerName = StringUtility.RandomStringOfSize(Constants.DefaultSizeOfDateStringToClearField);
            screenerGridForm.ClickSaveMySearch();
            screenerSteps.SaveScreenerWithConfirming(screenerName);

            LogStep(16, "Click Back");
            var screenerFiltersForm = new ScreenerFiltersForm();
            screenerFiltersForm.ClickBack();
            var savedScreenersForm = new SavedScreenersForm();
            Checker.IsTrue(savedScreenersForm.GetScreenerNames().Contains(screenerName), "Saved screener name is not shown");

            LogStep(17, "Click the saved screener by name");
            savedScreenersForm.ClickScreenerByName(screenerName);
            screenerFiltersForm.AssertIsOpen();
            screenerGridForm.AssertIsOpen();
            screenerGridForm.ClickRunScreener();
            var gridDataAfter = screenerGridForm.GetValuesInScreenerGrid();
            Checker.CheckEquals(gridDataBefore.Count, gridDataAfter.Count, "Items quantity in screener is not as before");
            foreach (var gridRow in gridDataAfter)
            {
                var mappedRowBefore = gridDataBefore.Where(t =>
                    t.ScreenerGridColumnToState[ScreenerColumnTypes.Ticker].Split('\r')[0].Equals(gridRow.ScreenerGridColumnToState[ScreenerColumnTypes.Ticker].Split('\r')[0])).ToList();
                Checker.CheckEquals(UniqueTickerQuantity, mappedRowBefore.Count, "There are not mapped column before and after going from Screener");
                if (mappedRowBefore.Count == UniqueTickerQuantity)
                {
                    Checker.CheckEquals(mappedRowBefore.First(), gridRow, "There is difference in grid data before and after");
                }
            }

            LogStep(18, "Repeat steps from 3 to 18");
            DoSteps3To13();
        }

        private void DoSteps3To13()
        {
            LogStep(3, "Make sure that the 'Country of Exchange' filter present on the page.");
            var screenerFiltersForm = new ScreenerFiltersForm();
            Checker.CheckEquals(expectedScreenerFiltersModel.CountryOfExchangeFilter, screenerFiltersForm.GetCurrentCountryOfExchangeFilterModelAfterScrolling(),
                "Country of Exchanges Filter are not equal");

            LogStep(4, "Make sure that the 'Health Status' filter present on the page.");
            Checker.CheckEquals(expectedScreenerFiltersModel.HealthStatusFilter, screenerFiltersForm.GetCurrentHealthStatusFilterModelAfterScrolling(),
                "Health Status Filter are not equal");

            LogStep(5, "Make sure that the 'Trend' filter present on the page.");
            Checker.CheckEquals(expectedScreenerFiltersModel.TrendFilter, screenerFiltersForm.GetCurrentTrendFilterModel(),
                "Trend Filter are not equal");

            LogStep(6, "Make sure that the 'Entry Signal' filter present on the page.");
            Checker.CheckEquals(expectedScreenerFiltersModel.EntrySignalFilter, screenerFiltersForm.GetCurrentEntrySignalFilterModel(),
                "Entry Signal Filter are not equal");

            LogStep(7, "Make sure that the 'VQ Range present on the page.");
            Checker.CheckEquals(expectedScreenerFiltersModel.VqRangeFilter, screenerFiltersForm.GetCurrentNumericFilterModelAfterScroll(ScreenerFilterType.VqRange),
                "VQ Range are not equal");

            LogStep(8, "Make sure that the 'TradeSmith Strategies' filter present on the page.");
            Checker.CheckEquals(expectedScreenerFiltersModel.StrategyFilter, screenerFiltersForm.GetCurrentStrategyFilterModelAfterScrolling(),
                "TradeSmith Strategies Filter are not equal");

            LogStep(9, "Make sure that the 'Newsletters Recommendations' filter present on the page.");
            Checker.CheckEquals(expectedScreenerFiltersModel.NewslettersFilter, screenerFiltersForm.GetCurrentStateNewsletterFilterAfterScrolling(),
                "Newsletters Recommendations are not equal");

            LogStep(10, "Make sure that the 'Market Capitalization' filter present on the page.");
            Checker.CheckEquals(expectedScreenerFiltersModel.MarketCapitalizationFilter, screenerFiltersForm.GetCurrentMarketCapitalizationFilterModel(),
                "Market Capitalization Filter are not equal");

            LogStep(11, "Make sure that the 'Asset Type present on the page.");
            Checker.CheckEquals(expectedScreenerFiltersModel.AssetTypeFilter, screenerFiltersForm.GetCurrentAssetTypeFilterModel(),
                "Asset Type are not equal");

            LogStep(12, "Make sure that the 'Sector' filter present on the page.");
            Checker.CheckEquals(expectedScreenerFiltersModel.SectorFilter, screenerFiltersForm.GetCurrentSectorIndustryFilterModel(ScreenerFilterType.Sectors),
                "Sector Filter are not equal");

            LogStep(13, "Make sure that the 'Display & Sort Results present on the page.");
            Checker.CheckEquals(expectedScreenerFiltersModel.SortFilter, screenerFiltersForm.GetCurrentSortFilterModel(),
                "Display & Sort Results are not equal");
        }
    }
}