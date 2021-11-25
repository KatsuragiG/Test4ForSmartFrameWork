using AutomatedTests;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Screener;
using AutomatedTests.Enums.FilterEnums;
using AutomatedTests.Enums.Sorting;
using AutomatedTests.Enums.Tools;
using AutomatedTests.Forms.ResearchPages.PureQuant;
using AutomatedTests.Forms.Screener;
using AutomatedTests.Models.FiltersModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Screener;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Enums;
using TradeStops.Common.Utils;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._07_Invest._04_Screener
{
    [TestClass]
    public class TC_1405_Screener_SavedScreenersAreShownCorrectlyAndWorkable : BaseTestUnitTests
    {
        private const int TestNumber = 1405;

        private int filtersQuantity;
        private int predefinedScreenersQuantity;
        private int addedScreenersQuantity;
        private bool isPqRunButtonAvailable;
        private readonly UserScreenerSearchesDbModel userScreenerDbModel = new UserScreenerSearchesDbModel();
        private readonly ScreenerSearchesDbModel screenerSearchesDbModel = new ScreenerSearchesDbModel();
        private readonly List<ScreenerDbSearchFilterModel> filtersModels = new List<ScreenerDbSearchFilterModel>();
        private List<string> predefinedScreenersNames;
        private readonly List<string> expectedFilterNames = new List<string>();
        private string newsletterFilterWording;
        private string industryFilterWording;
        private string sectorFilterWording;
        private readonly NewslettersFilterModel expectedNewslettersFilterModel = new NewslettersFilterModel();
        private readonly SectorIndustryFilterModel expectedSectorFilterModel = new SectorIndustryFilterModel();
        private readonly SectorIndustryFilterModel expectedIndustryFilterModel = new SectorIndustryFilterModel();

        [TestInitialize]
        public void TestInitialize()
        {
            var userProductSubscriptions = GetUserProductSubscriptions("userSubscription");

            filtersQuantity = GetTestDataAsInt(nameof(filtersQuantity));
            predefinedScreenersQuantity = GetTestDataAsInt(nameof(predefinedScreenersQuantity));
            addedScreenersQuantity = GetTestDataAsInt(nameof(addedScreenersQuantity));
            userScreenerDbModel.Name = StringUtility.RandomString(GetTestDataAsString("screenerName"));
            isPqRunButtonAvailable = GetTestDataAsBool(nameof(isPqRunButtonAvailable));
            newsletterFilterWording = GetTestDataAsString(nameof(newsletterFilterWording));
            industryFilterWording = GetTestDataAsString(nameof(industryFilterWording));
            sectorFilterWording = GetTestDataAsString(nameof(sectorFilterWording));

            screenerSearchesDbModel.IsSystem = GetTestDataAsBool("IsSystem");
            screenerSearchesDbModel.IsPredefined = GetTestDataAsBool("IsPredefined");
            screenerSearchesDbModel.DisplayCount = GetTestDataAsInt("displayCount");
            screenerSearchesDbModel.DefaultName = userScreenerDbModel.Name;
            screenerSearchesDbModel.OrderByField = GetTestDataAsString("orderByField");
            screenerSearchesDbModel.OrderType = (int)GetTestDataParsedAsEnumFromStringMapping<SortingStatus>("sorting");
            screenerSearchesDbModel.SearchTypeId = GetTestDataAsInt("searchTypeId");
            screenerSearchesDbModel.HashCodeValue = StringUtility.RandomStringOfSize(Constants.DigitsQuantityToFillNumericInStringToCompare);
            predefinedScreenersNames = EnumUtils.GetValues<DefaultScreenerNameTypes>().Select(t => t.GetStringMapping()).ToList();

            expectedNewslettersFilterModel.SliderState = SliderFilterStates.Yes;
            expectedNewslettersFilterModel.ActiveNewsletterName = GetTestDataValuesAsListByColumnNameAndRemoveEmpty("publisherPortfolios");
            expectedSectorFilterModel.SubFilterName = ScreenerFilterType.Sectors;
            expectedSectorFilterModel.ActiveSectorIndustryNames = GetTestDataValuesAsListByColumnNameAndRemoveEmpty("activeSectors");
            expectedIndustryFilterModel.SubFilterName = ScreenerFilterType.Sectors;
            expectedIndustryFilterModel.ActiveSectorIndustryNames = GetTestDataValuesAsListByColumnNameAndRemoveEmpty("activeIndustries");

            for (int i = 1; i <= filtersQuantity; i++)
            {
                SaveFilterValueInDb(i);
            }

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));
            userScreenerDbModel.TradeSmithUserId = UserModels.First().TradeSmithUserId;
            new ScreenerSteps().AddUserScreeners(userScreenerDbModel, screenerSearchesDbModel, filtersModels);

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuNavigation().OpenSavedScreeners();
        }

        private void SaveFilterValueInDb(int i)
        {
            var decimal1 = GetTestDataAsString($"filter{i}Decimal1");
            var decimal2 = GetTestDataAsString($"filter{i}Decimal2");
            var bool1 = GetTestDataAsString($"filter{i}Bool1");
            var int1 = GetTestDataAsString($"filter{i}Int1");
            var filterTypeId = GetTestDataAsInt($"filterTypeIds{i}");
            filtersModels.Add(new ScreenerDbSearchFilterModel
            {
                FilterTypeId = filterTypeId,
                Decimal1 = string.IsNullOrEmpty(decimal1)
                    ? (decimal?)null
                    : Parsing.ConvertToDecimal(decimal1),
                Decimal2 = string.IsNullOrEmpty(decimal2)
                    ? (decimal?)null
                    : Parsing.ConvertToDecimal(decimal2),
                Bool1 = string.IsNullOrEmpty(bool1)
                    ? (int?)null
                    : Parsing.ConvertToInt(bool1),
                Int1 = string.IsNullOrEmpty(int1)
                    ? (int?)null
                    : Parsing.ConvertToInt(int1),
                List1 = GetTestDataAsString($"filter{i}List1"),
                IndexValue = filterTypeId,
                TupleList1 = GetTestDataAsString($"filter{i}TupleList1"),
                List2 = GetTestDataAsString($"filter{i}List2")
            });
            expectedFilterNames.Add(Dictionaries.ScreenerFiltersDbToAutotestType[(StockFinderFilterTypes)filtersModels.Last().FilterTypeId].GetStringMapping());
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1405$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test checks that saved via DB screeners are shown correctly and workable https://tr.a1qa.com/index.php?/cases/view/22625937")]
        [TestCategory("Screener"), TestCategory("PureQuantTool")]
        public override void RunTest()
        {
            LogStep(1, "Check that 'Saved Screener' page is shown");
            var savedScreenersForm = new SavedScreenersForm();
            savedScreenersForm.AssertIsOpen();

            LogStep(2, "Check that screeners quantity matches expectation");
            var savedScreenersNames = savedScreenersForm.GetScreenerNames();
            Checker.CheckEquals(predefinedScreenersQuantity + addedScreenersQuantity, savedScreenersNames.Count,
                "Saved Screeners quantity is not as expected");
            Checker.CheckEquals(predefinedScreenersQuantity, savedScreenersNames.Count(t => predefinedScreenersNames.Contains(t)),
                $"Predefined Screeners are not as expected\n{GetActualResultsString(savedScreenersNames)}");
            Checker.IsTrue(savedScreenersNames.Contains(userScreenerDbModel.Name),
                $"Added Screeners {userScreenerDbModel.Name} is not shown\n{GetActualResultsString(savedScreenersNames)}");

            LogStep(3, "Check that saved screener has correct description in expanded state");
            var savedScreenerOrder = savedScreenersNames.FindIndex(t => t == userScreenerDbModel.Name) + 1;
            if (savedScreenerOrder != Constants.ItemNotFoundInCollection)
            {
                CheckScreenerDescription(savedScreenerOrder);
            }
            else
            {
                Assert.Fail($"Added via DB screener {userScreenerDbModel.Name} not found in UI");
            }

            LogStep(4, "Check that clicking on saved screener leads to it's editing");
            savedScreenersForm.ClickScreenerByName(userScreenerDbModel.Name);
            var screenerFiltersForm = new ScreenerFiltersForm();
            screenerFiltersForm.AssertIsOpen();

            LogStep(5, "Check that filters quantity is matched expectation");
            var actualFilterNames = screenerFiltersForm.GetActiveFiltersNames().Where(t => t != ScreenerFilterType.SortResults.GetStringMapping()).ToList();
            Checker.CheckEquals(filtersQuantity, actualFilterNames.Count,
                $"Filters quantity is not as expected\n{GetActualResultsString(actualFilterNames)}");
            foreach (var actualFilterName in actualFilterNames)
            {
                Checker.IsTrue(expectedFilterNames.Contains(actualFilterName),
                    $"Filter name {actualFilterName} does not shown at editing.\n{GetExpectedResultsString(expectedFilterNames)}");
            }

            LogStep(6, "Check that filters models are matched expectation");
            CheckFiltersModels();

            LogStep(7, "Click Run Screener and check non-empty grid");
            var screenerGridForm = new ScreenerSteps().ClickCalculateButtonReturnResultGridForm();
            var rowsQuantity = screenerGridForm.GetNumberOfRowsInGrid();
            Assert.IsTrue(rowsQuantity > 0, "There is no result grid on Screener");

            LogStep(8, "Click back");
            screenerFiltersForm.ClickBack();
            savedScreenersForm.AssertIsOpen();

            LogStep(9, "Check that visibility of PQ Run button is matched expectations");
            var actualPqRunButtonAvailable = savedScreenersForm.IsRunPqButtonExistsByOrder(savedScreenerOrder);
            Checker.CheckEquals(isPqRunButtonAvailable, actualPqRunButtonAvailable, "PQ Run button visibility is not as expected");

            LogStep(10, "Click PQ Run button if available");
            if (actualPqRunButtonAvailable && isPqRunButtonAvailable)
            {
                savedScreenersForm.ClickRunPqButtonByOrder(savedScreenerOrder);
                var pureQuantResultsForm = new PureQuantResultsForm();
                pureQuantResultsForm.AssertIsOpen();
                Checker.IsTrue(pureQuantResultsForm.IsGridTablePresent(), "PQ Grid is not shown");

                LogStep(11, "Check Pq Source");
                pureQuantResultsForm.ClickShowDetailsOfRun();
                var existedSourceWording = pureQuantResultsForm.GetAllDetailsOfRunWording();
                var expectedWording = $"{PureQuantDataSourceTypes.IdeasSavedSearch.GetStringMapping()}: {userScreenerDbModel.Name}";
                Checker.IsTrue(existedSourceWording.Contains(expectedWording),
                    $"PQ Source is not as expected\r{GetActualResultsString(existedSourceWording)}");
            }
        }

        private void CheckFiltersModels()
        {
            var screenerFiltersForm = new ScreenerFiltersForm();
            if (expectedFilterNames.Contains(ScreenerFilterType.HealthStatus.GetStringMapping()))
            {
                Checker.CheckEquals(new HealthStatusFilterModel(filtersModels.First(t => t.FilterTypeId == (int)StockFinderFilterTypes.HealthStatuses)),
                    screenerFiltersForm.GetCurrentHealthStatusFilterModelAfterScrolling(), "Health Status filter is not as expected");
            }
            if (expectedFilterNames.Contains(ScreenerFilterType.Trend.GetStringMapping()))
            {
                Checker.CheckEquals(new TrendFilterModel(filtersModels.First(t => t.FilterTypeId == (int)StockFinderFilterTypes.HealthTrends)),
                    screenerFiltersForm.GetCurrentTrendFilterModel(), "Health Trend filter is not as expected");
            }
            if (expectedFilterNames.Contains(ScreenerFilterType.EntrySignals.GetStringMapping()))
            {
                Checker.CheckEquals(new EntrySignalFilterModel(filtersModels.First(t => t.FilterTypeId == (int)StockFinderFilterTypes.EntrySignals)),
                    screenerFiltersForm.GetCurrentEntrySignalFilterModel(), "Entry Signal filter is not as expected");
            }
            if (expectedFilterNames.Contains(ScreenerFilterType.Rating.GetStringMapping()))
            {
                Checker.CheckEquals(new RatingFilterModel(filtersModels.First(t => t.FilterTypeId == (int)StockFinderFilterTypes.GlobalRank)),
                    screenerFiltersForm.GetCurrentRatingFilterModel(), "Rating filter is not as expected");
            }
            if (expectedFilterNames.Contains(ScreenerFilterType.TradeSmithStrategies.GetStringMapping()))
            {
                Checker.CheckEquals(new StrategyFilterModel(filtersModels.First(t => t.FilterTypeId == (int)StockFinderFilterTypes.Strategies)),
                    screenerFiltersForm.GetCurrentStrategyFilterModelAfterScrolling(), "TradeSmith Strategy filter is not as expected");
            }
            if (expectedFilterNames.Contains(ScreenerFilterType.CycleConvictionLevel.GetStringMapping()))
            {
                Checker.CheckEquals(new CycleConvictionLevelFilterModel(filtersModels.First(t => t.FilterTypeId == (int)StockFinderFilterTypes.TimingTurnStrength)),
                    screenerFiltersForm.GetCurrentCycleConvictionLevelFilterModel(), "Cycle Conviction Level filter is not as expected");
            }
            if (expectedFilterNames.Contains(ScreenerFilterType.CycleTurnArea.GetStringMapping()))
            {
                Checker.CheckEquals(new CycleTurnAreaFilterModel(filtersModels.First(t => t.FilterTypeId == (int)StockFinderFilterTypes.TimingTurnArea)),
                    screenerFiltersForm.GetCurrentCycleTurnAreaFilterModel(), "Cycle Turn Area filter is not as expected");
            }
            if (expectedFilterNames.Contains(ScreenerFilterType.CyclePeriod.GetStringMapping()))
            {
                Checker.CheckEquals(new CyclePeriodFilterModel(filtersModels.First(t => t.FilterTypeId == (int)StockFinderFilterTypes.TimingSerieType)),
                    screenerFiltersForm.GetCurrentCyclePeriodFilterModel(), "Cycle Period filter is not as expected");
            }
            if (expectedFilterNames.Contains(ScreenerFilterType.TradeSmithBaskets.GetStringMapping()))
            {
                Checker.CheckEquals(new TradeSmithBasketsFilterModel(filtersModels.First(t => t.FilterTypeId == (int)StockFinderFilterTypes.Baskets)),
                    screenerFiltersForm.GetTradeSmithBasketsFilterModel(), "Baskets filter is not as expected");
            }
            if (expectedFilterNames.Contains(ScreenerFilterType.CountryOfExchange.GetStringMapping()))
            {
                Checker.CheckEquals(new CountryOfExchangeFilterModel(filtersModels.First(t => t.FilterTypeId == (int)StockFinderFilterTypes.ExchangeCounties)),
                    screenerFiltersForm.GetCurrentCountryOfExchangeFilterModelAfterScrolling(), "Country Of Exchange filter is not as expected");
            }
            if (expectedFilterNames.Contains(ScreenerFilterType.Markets.GetStringMapping()))
            {
                Checker.CheckEquals(new IndexFilterModel(filtersModels.First(t => t.FilterTypeId == (int)StockFinderFilterTypes.SymbolGroups)),
                    screenerFiltersForm.GetCurrentIndexFilterModel(), "Market filter is not as expected");
            }
            if (expectedFilterNames.Contains(ScreenerFilterType.Newsletters.GetStringMapping()))
            {
                Checker.CheckEquals(expectedNewslettersFilterModel, screenerFiltersForm.GetCurrentStateNewsletterFilterAfterScrolling(),
                    "Newsletter filter is not as expected");
            }
            if (expectedFilterNames.Contains(ScreenerFilterType.Sectors.GetStringMapping()))
            {
                Checker.CheckEquals(expectedSectorFilterModel, screenerFiltersForm.GetCurrentSectorIndustryFilterModel(ScreenerFilterType.Sectors),
                    "Sector filter is not as expected");
            }
            if (expectedFilterNames.Contains(ScreenerFilterType.Industries.GetStringMapping()))
            {
                Checker.CheckEquals(expectedIndustryFilterModel, screenerFiltersForm.GetCurrentSectorIndustryFilterModel(ScreenerFilterType.Industries),
                    "Industries filter is not as expected");
            }
            if (expectedFilterNames.Contains(ScreenerFilterType.QuoteCurrencies.GetStringMapping()))
            {
                Checker.CheckEquals(new QuoteCurrenciesFilterModel(filtersModels.First(t => t.FilterTypeId == (int)StockFinderFilterTypes.QuoteCurrencies)),
                    screenerFiltersForm.GetCurrentQuoteCurrenciesFilterModel(), "Quote Currencies filter is not as expected");
            }
            if (expectedFilterNames.Contains(ScreenerFilterType.AssetType.GetStringMapping()))
            {
                Checker.CheckEquals(new AssetTypeFilterModel(filtersModels.First(t => t.FilterTypeId == (int)StockFinderFilterTypes.AssetTypes)),
                    screenerFiltersForm.GetCurrentAssetTypeFilterModelAfterScrolling(), "Asset Type filter is not as expected");
            }
            if (expectedFilterNames.Contains(ScreenerFilterType.MarketCapitalization.GetStringMapping()))
            {
                Checker.CheckEquals(new MarketCapitalizationFilterModel(filtersModels.First(t => t.FilterTypeId == (int)StockFinderFilterTypes.MarketCapitalization)),
                    screenerFiltersForm.GetCurrentMarketCapitalizationFilterModel(), "Market Capitalization filter is not as expected");
            }

            CheckNumericFiltersModels();
        }

        private void CheckNumericFiltersModels()
        {
            var screenerFiltersForm = new ScreenerFiltersForm();
            foreach (var expectedFilterName in expectedFilterNames)
            {
                var filterType = expectedFilterName.ParseAsEnumFromStringMapping<ScreenerFilterType>();
                if (Instance.GetListOfNumericScreenerFilters().Contains(filterType))
                {
                    Checker.CheckEquals(new NumericFilterModel(filterType, filtersModels
                            .First(t => t.FilterTypeId == (int)Dictionaries.ScreenerFiltersDbToAutotestType.FirstOrDefault(f => f.Value == filterType).Key)),
                        screenerFiltersForm.GetCurrentNumericFilterModelAfterScroll(filterType),
                        $"{filterType.GetStringMapping()} filter is not as expected");
                }
            }
        }

        private void CheckScreenerDescription(int savedScreenerOrder)
        {
            var savedScreenersForm = new SavedScreenersForm();
            savedScreenersForm.ClickExpandScreenerByOrder(savedScreenerOrder);

            var savedScreenerDescription = savedScreenersForm.GetScreenerFilterDescription(savedScreenerOrder);
            Checker.IsFalse(savedScreenerDescription == null, "Screener description in collapsed state is empty");

            var filterNames = savedScreenerDescription.Keys.Select(t => t.Replace(Constants.SavedScreenerNameSeparator, string.Empty)).ToList();
            var filterValues = savedScreenerDescription.Values.ToList();
            foreach (var filterModel in filtersModels)
            {
                var dbFilterType = (StockFinderFilterTypes)filterModel.FilterTypeId;
                var mappedFilterName = Dictionaries.ScreenerFiltersDbToAutotestType[dbFilterType].GetStringMapping();
                Checker.IsTrue(filterNames.Contains(mappedFilterName),
                    $"Filter {dbFilterType} does not have expected name in description.\n{GetActualResultsString(filterNames)}");

                var expectedValues = filterModel.GetExpectedDescriptionForFilter();
                if (dbFilterType == StockFinderFilterTypes.Newsletters)
                {
                    expectedValues = newsletterFilterWording;
                }
                if (dbFilterType == StockFinderFilterTypes.Sectors)
                {
                    expectedValues = sectorFilterWording;
                }
                if (dbFilterType == StockFinderFilterTypes.Industries)
                {
                    expectedValues = industryFilterWording;
                }
                Checker.IsFalse(string.IsNullOrEmpty(expectedValues),
                    $"Filter {mappedFilterName} does not found any value in description.\n{GetActualResultsString(filterValues)}");
                Checker.IsTrue(filterValues.Contains(expectedValues),
                    $"Filter {mappedFilterName} does not have expected value '{expectedValues}' in description.\n{GetActualResultsString(filterValues)}");
            }
        }
    }
}