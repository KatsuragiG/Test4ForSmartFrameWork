using AutomatedTests.ConstantVariables;
using AutomatedTests.Elements;
using AutomatedTests.Enums.Chart;
using AutomatedTests.Enums.Dashboard;
using AutomatedTests.Enums.FilterEnums;
using AutomatedTests.Enums.Markets;
using AutomatedTests.Enums.Tools.StockAnalyzer;
using AutomatedTests.Enums.Tools;
using AutomatedTests.Enums.User;
using AutomatedTests.Enums;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Forms.ResearchPages.StockAnalyzer;
using AutomatedTests.Forms.Screener;
using AutomatedTests.Forms;
using AutomatedTests.Models.FiltersModels;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.BrowserSteps;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TradeStops.Common.Enums;
using TradeStops.Common.Utils;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;
using WebdriverFramework.Framework.WebDriver;

namespace UnitTests.Tests._07_Invest._01_StockAnalyzer
{
    [TestClass]
    public class TC_1407_StockAnalyzer_DistributionsTabHasAllExpectedItems : BaseTestUnitTests
    {
        private const int TestNumber = 1407;

        private int legendOrderToCheckLink;
        private string symbol;
        private string expectedStockRatingDistributionTitle;
        private string expectedDefaultTextinDonutChart;
        private string expectedWordingOnDonut;
        private string expectedHintForHealthDistribution;
        private string expectedPriceChartAxisTitle;
        private string expectedStockRatingAxisTitle;
        private string expectedTrendAxisTitle;
        private string expectedHealthAxisTitle;
        private string priceInTooltip;
        private List<string> pieChartColors;
        private Dictionary<string, string> legendText;
        private readonly IndexFilterModel indexFilterModel = new IndexFilterModel();
        private readonly SectorIndustryFilterModel sectorFilterModel = new SectorIndustryFilterModel { SubFilterName = ScreenerFilterType.Sectors };
        private readonly CountryOfExchangeFilterModel countryFilterModel = new CountryOfExchangeFilterModel();
        private readonly AssetTypeFilterModel assetFilterModel = new AssetTypeFilterModel();
        private readonly HealthStatusFilterModel healthFilterModel = new HealthStatusFilterModel();
        private readonly List<ChartPeriod> chartPeriods = new List<ChartPeriod> { ChartPeriod.OneYear, ChartPeriod.ThreeYears, ChartPeriod.FiveYears, ChartPeriod.All };
        private List<ChartLineTypes> linesToCheckOnTooltip = new List<ChartLineTypes>();
        private readonly List<ChartLineTypes> linesToCheckOnChart = new List<ChartLineTypes>
        {
            ChartLineTypes.MarketPrice,
            ChartLineTypes.BullBearIndicator,
            ChartLineTypes.TrendUpDistribution,
            ChartLineTypes.TrendDownDistribution,
            ChartLineTypes.TrendSidewaysDistribution,
            ChartLineTypes.HealthRedDistribution,
            ChartLineTypes.HealthYellowDistribution,
            ChartLineTypes.HealthGreenDistribution,
            ChartLineTypes.RatingStrongBullishDistribution,
            ChartLineTypes.RatingBullishDistribution,
            ChartLineTypes.RatingNeutralDistribution,
            ChartLineTypes.RatingBearishDistribution
        };

        [TestInitialize]
        public void TestInitialize()
        {
            var userType = GetTestDataParsedAsEnumFromStringMapping<ProductSubscriptionTypes>("userSubscription");
            symbol = GetTestDataAsString(nameof(symbol));
            expectedStockRatingDistributionTitle = GetTestDataAsString(nameof(expectedStockRatingDistributionTitle));
            expectedDefaultTextinDonutChart = GetTestDataAsString(nameof(expectedDefaultTextinDonutChart));
            expectedWordingOnDonut = GetTestDataAsString(nameof(expectedWordingOnDonut));
            expectedHintForHealthDistribution = GetTestDataAsString(nameof(expectedHintForHealthDistribution));
            expectedPriceChartAxisTitle = GetTestDataAsString(nameof(expectedPriceChartAxisTitle));
            expectedStockRatingAxisTitle = GetTestDataAsString(nameof(expectedStockRatingAxisTitle));
            expectedHealthAxisTitle = GetTestDataAsString(nameof(expectedHealthAxisTitle));
            expectedTrendAxisTitle = GetTestDataAsString(nameof(expectedTrendAxisTitle));
            priceInTooltip = GetTestDataAsString(nameof(priceInTooltip));
            legendOrderToCheckLink = GetTestDataAsInt(nameof(legendOrderToCheckLink));

            linesToCheckOnTooltip = linesToCheckOnChart.Except(new List<ChartLineTypes> { ChartLineTypes.MarketPrice, ChartLineTypes.BullBearIndicator }).ToList();
            linesToCheckOnTooltip.Add(ChartLineTypes.RatingStrongBullishDistribution);

            InitializeFiltersModels();

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, (ProductSubscriptions)(int)userType));

            LoginSetUp.LogIn(UserModels.First());
            var mainMenuForm = new MainMenuForm();
            mainMenuForm.ClickMenuItem(MainMenuItems.Dashboard);
            mainMenuForm.SetSymbol(symbol);
        }

        private void InitializeFiltersModels()
        {
            foreach (var subfilter in Constants.AvailableIndices)
            {
                indexFilterModel.IndexFilterNameToState.Add(subfilter, false);
            }
            if (EnumUtils.GetValues<IndiceTypes>().Select(t => t.GetDescription()).ToList().Contains(symbol))
            {
                indexFilterModel.IndexFilterNameToState[symbol.ParseAsEnumFromDescription<IndiceTypes>()] = true;
            }
            else
            {
                indexFilterModel.IndexFilterNameToState[IndiceTypes.Sp500] = true;
                sectorFilterModel.ActiveSectorIndustryNames.Add(symbol.ParseAsEnumFromDescription<SectorTypes>().GetStringMapping());
            }

            foreach (var subfilter in EnumUtils.GetValues<CountryOfExchangeTypes>())
            {
                countryFilterModel.CountryOfExchangeFilterNameToState.Add(subfilter, false);
            }
            countryFilterModel.CountryOfExchangeFilterNameToState[CountryOfExchangeTypes.USA] = true;

            assetFilterModel.AssetFilterNameToState.Add(SymbolTypes.Stock.ToString(), true);

            foreach (var healthZone in EnumUtils.GetValues<HealthStatusFilter>())
            {
                healthFilterModel.HealthStatusFilterNameToState.Add(healthZone, false);
            }
            healthFilterModel.HealthStatusFilterNameToState[(HealthStatusFilter)legendOrderToCheckLink] = true;
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1407$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("Test for existing of all elements on 'Distributions' tabs and its correct default state. https://tr.a1qa.com/index.php?/cases/view/22686213")]
        [TestCategory("StockAnalyzer")]
        public override void RunTest()
        {
            LogStep(1, $"Check that Stock analyzer for {symbol} is shown;");
            var stockAnalyzerForm = new StockAnalyzerForm();
            stockAnalyzerForm.AssertIsOpen();
            Checker.CheckEquals(symbol, stockAnalyzerForm.GetSymbolTreeSelectSingleValue(), "Ticker is not as expected in autocomplete");
            Checker.IsTrue(stockAnalyzerForm.IsTabPresent(StockAnalyzerTabs.Distributions), "Distribution tab is not shown");

            LogStep(2, "Click on 'Distribution' tab;");
            var distributionsTabForm = stockAnalyzerForm.ActivateTabAndGetForm<DistributionsTabForm>(StockAnalyzerTabs.Distributions);
            stockAnalyzerForm.AssertIsOpen();
            distributionsTabForm.AssertIsOpen();

            LogStep(3, "Check that carousel and pie-chart are shown for Rating");
            var pieChart = distributionsTabForm.DistributionPieChart;
            CheckCarouselPieChartPresence(pieChart, DistributionCarouselTypes.RatingDistribution);

            LogStep(4, "Check legend text and pie chart colors for Rating");
            var expectedRatingItems = EnumUtils.GetValues<GlobalRatingTypes>().Select(t => t.GetStringMapping()).ToList();
            CheckLegendTextPieChartColors(pieChart, expectedRatingItems, DistributionCarouselTypes.RatingDistribution);

            LogStep(5, "Click on internal areas in donut chart and verify area colors for Rating");
            CheckPieChartInteractions(pieChart, DistributionCarouselTypes.RatingDistribution);
            CheckCarouselArrows();

            LogStep(6, "Click on 'right' arrow on carousel");
            distributionsTabForm.ClickCarouselArrow(CarouselActionArrows.Next);

            LogStep(7, "Check that carousel and pie-chart are shown for Health");
            pieChart = distributionsTabForm.DistributionPieChart;
            CheckCarouselPieChartPresence(pieChart, DistributionCarouselTypes.HealthDistribution);

            LogStep(8, "Check legend text and pie chart colors for Health");
            var expectedHealthItems = EnumUtils.GetValues<HealthStatusFilter>().Select(t => t.GetStringMapping()).ToList();
            CheckLegendTextPieChartColors(pieChart, expectedHealthItems, DistributionCarouselTypes.HealthDistribution);

            LogStep(9, "Check that {value}{%}sign is shown as link. Check that link leads to Screener.");
            Checker.CheckEquals(expectedHintForHealthDistribution, distributionsTabForm.GetHintOverScreenerLinkByLegendOrder(legendOrderToCheckLink),
                "Hint for Legend link is not as expected");
            distributionsTabForm.ClickLinkToScreenerByLegendOrder(legendOrderToCheckLink);
            var screenerFiltersForm = new BrowserSteps().CheckThatPageOpensGetPageWithSoftAssert<ScreenerFiltersForm>();
            Checker.CheckEquals(countryFilterModel, screenerFiltersForm.GetCurrentCountryOfExchangeFilterModelAfterScrolling(),
                "Country filter is not as expected");
            Checker.CheckEquals(assetFilterModel, screenerFiltersForm.GetCurrentAssetTypeFilterModelAfterScrolling(),
                "Asset Type filter is not as expected");
            Checker.CheckEquals(healthFilterModel, screenerFiltersForm.GetCurrentHealthStatusFilterModelAfterScrolling(),
                "Health Zone filter is not as expected");
            Checker.CheckEquals(indexFilterModel, screenerFiltersForm.GetCurrentIndexFilterModel(),
                "Market filter is not as expected");
            if (screenerFiltersForm.IsFilterExist(ScreenerFilterType.Sectors))
            {
                Checker.CheckEquals(sectorFilterModel, screenerFiltersForm.GetCurrentSectorIndustryFilterModel(ScreenerFilterType.Sectors),
                    "Sector filter is not as expected");
            }
            screenerFiltersForm.ClickBack();
            distributionsTabForm.AssertIsOpen();
            distributionsTabForm.SelectCarouselItem(DistributionCarouselTypes.HealthDistribution);

            LogStep(10, "Click on 'Green' area in donut chart and verify that area has color");
            pieChart = distributionsTabForm.DistributionPieChart;
            CheckPieChartInteractions(pieChart, DistributionCarouselTypes.HealthDistribution);
            CheckCarouselArrows();

            LogStep(11, "Click on 'right' arrow on carousel and Verify that Trend Distribution is shown");
            distributionsTabForm.ClickCarouselArrow(CarouselActionArrows.Next);
            pieChart = distributionsTabForm.DistributionPieChart;
            CheckCarouselPieChartPresence(pieChart, DistributionCarouselTypes.TrendDistribution);

            LogStep(12, "Check legend text and pie chart colors for Trend");
            var expectedTrendItems = EnumUtils.GetValues<TrendFilterTypes>().Select(t => t.GetStringMapping()).ToList();
            CheckLegendTextPieChartColors(pieChart, expectedTrendItems, DistributionCarouselTypes.TrendDistribution);

            LogStep(13, "Click on internal areas in donut chart and verify area colors");
            CheckPieChartInteractions(pieChart, DistributionCarouselTypes.TrendDistribution);
            CheckCarouselArrows();

            LogStep(14, "Check that charts is shown on the right side of tab");
            var chart = stockAnalyzerForm.Chart;
            Checker.IsTrue(chart.IsChartAreaExist(), "Main chart is not shown");
            Checker.IsTrue(chart.AreAllChartPeriodButtonsPresent(chartPeriods),
                "Not all chart period buttons are shown");
            Checker.CheckEquals(ChartPeriod.ThreeYears, chart.GetCurrentPeriodButton(),
                "Default chart period button is not as expected");
            Checker.CheckEquals(expectedPriceChartAxisTitle, chart.GetAxisLegend(ChartTypes.Price),
                "Price axis name not as expected");
            Checker.CheckEquals(expectedStockRatingAxisTitle, chart.GetAxisLegend(ChartTypes.RatingDistribution),
                "Rating Distribution axis name not as expected");
            Checker.CheckEquals(expectedHealthAxisTitle, chart.GetAxisLegend(ChartTypes.HealthDistribution),
                "Health Distribution axis name not as expected");
            Checker.CheckEquals(expectedTrendAxisTitle, chart.GetAxisLegend(ChartTypes.TrendDistribution),
                "Trend Distribution axis name not as expected");

            LogStep(15, "Check that charts contain all expected lines");
            foreach (var line in linesToCheckOnChart)
            {
                Checker.IsTrue(chart.IsChartLinePresent(line), $"{line} line is not shown");
            }

            LogStep(16, "Click on '1y' button and hover over the some point on Close Price chart; ");
            Checker.IsTrue(chart.IsTooltipPresentOnChartByPriceLineFocus(), "Chart tooltip is not shown");
            var toolTipsWording = chart.GetTooltipItems();
            Checker.IsTrue(Constants.DateFormatMonDDcommaYyyyRegex.IsMatch(toolTipsWording.DateForPoint),
                $"Tooltip on chart does wrong format for date text '{toolTipsWording.DateForPoint}'");
            Checker.IsTrue(new Regex(string.Format(priceInTooltip, symbol, Constants.RiskValuePattern)).IsMatch(toolTipsWording.TextDescription),
                $"Price wording {toolTipsWording.TextDescription} on chart does not matched expectations");
            foreach (var line in linesToCheckOnTooltip)
            {
                Checker.IsTrue(new Regex(string.Format(priceInTooltip, line.GetStringMapping(), Constants.PercentValuesRegex)).IsMatch(toolTipsWording.ChartTooltipDataTypeToText[line]),
                    $"Tooltip for {line} is not as expected: '{toolTipsWording.ChartTooltipDataTypeToText[line]}'");
            }
        }

        private void CheckPieChartInteractions(PieChartElement pieChart, DistributionCarouselTypes type)
        {
            var nonZerosItemsInLegend = legendText.RemoveByValue($"{Constants.DefaultStringZeroDecimalValue}{Constants.PercentSign}");
            for (int i = 1; i <= pieChartColors.Count; i++)
            {
                pieChart.ClickItemInChartByNumber(i);
                var textInCenter = pieChart.GetPieChartHoverDistributionPercentDetailsText();
                if (!textInCenter.Contains(expectedDefaultTextinDonutChart) && !string.IsNullOrEmpty(textInCenter))
                {
                    var percentInCenter = pieChart.GetPieChartHoverDistributionPercentText();
                    Checker.IsTrue(Constants.PercentValuesRegex.IsMatch(percentInCenter.Trim()),
                        $"Percent value '{percentInCenter}' is not matched expectations for {type}");

                    var actualZonesWithPercentValues = nonZerosItemsInLegend.Where(t => t.Value.Equals(percentInCenter.Trim())).FirstOrDefault();
                    Checker.IsFalse(string.IsNullOrEmpty(actualZonesWithPercentValues.Key),
                        $"Legend for {type} does not contain record for {percentInCenter} value");

                    if (!string.IsNullOrEmpty(actualZonesWithPercentValues.Key))
                    {
                        Checker.CheckEquals(string.Format(expectedWordingOnDonut, actualZonesWithPercentValues.Key),
                            textInCenter,
                            $"Unexpected text in center for {type} pie chart");
                        Checker.CheckEquals(DictionaryColorsSwitcher(type, actualZonesWithPercentValues.Key),
                            pieChartColors[i - 1],
                            $"Color for {type} pie-chart is not as expected for {percentInCenter} value");
                    }
                }
                else if (string.IsNullOrEmpty(textInCenter))
                {
                    Checker.Fail($"Empty text at clicking {i} section rating pie chart");
                }
                else
                {
                    Logger.Instance.Warn($"Unexpected text '{textInCenter}' in center of pie chart for {type} and # {i}");
                }

                pieChart.ClickItemInChartByNumber(i);
            }
        }

        private void CheckCarouselArrows()
        {
            var distributionsTabForm = new DistributionsTabForm();
            foreach (var arrow in EnumUtils.GetValues<CarouselActionArrows>())
            {
                Checker.IsTrue(distributionsTabForm.IsCarouselArrowShown(arrow), $"Arrow '{arrow}' is not shown");
            }
        }

        private static string DictionaryColorsSwitcher(DistributionCarouselTypes type, string key)
        {
            switch (type)
            {
                case DistributionCarouselTypes.RatingDistribution:
                    return Dictionaries.RatingColorCodes[key.ParseAsEnumFromStringMapping<GlobalRatingTypes>()];
                case DistributionCarouselTypes.HealthDistribution:
                    return Dictionaries.HealthFillColorCodes[key.ParseAsEnumFromStringMapping<HealthStatusFilter>()];
                case DistributionCarouselTypes.TrendDistribution:
                    return Dictionaries.TrendFillColorCodes[key.ParseAsEnumFromStringMapping<TrendFilterTypes>()];
                default:
                    return string.Empty;
            }
        }

        private void CheckLegendTextPieChartColors(PieChartElement pieChart, List<string> expectedLegendNames, DistributionCarouselTypes type)
        {
            legendText = pieChart.GetDistributionPieChartLegend();
            Checker.CheckEquals(expectedLegendNames.Count(), legendText.Count,
                $"Unexpected quantity of items in {type} legend");
            pieChartColors = pieChart.GetDistributionPieChartSectionsColors();
            Checker.CheckEquals(legendText.Count, pieChartColors.Count,
                $"Unexpected quantity of filled part in {type} legend pie chart");

            foreach (var item in expectedLegendNames)
            {
                var isKeyPresent = legendText.TryGetValue(item, out string legendValue);
                Checker.IsTrue(isKeyPresent,
                    $"{type} distribution '{item}' is not shown:\n {legendText.Keys.Aggregate(string.Empty, (current, name) => current + "\n" + name)}");
                Checker.IsTrue(Constants.PercentValuesRegex.IsMatch(legendValue),
                    $"Value '{legendValue}' for '{item}' is not matched expectations for {type}");
            }
        }

        private void CheckCarouselPieChartPresence(PieChartElement pieChart, DistributionCarouselTypes type)
        {
            var distributionsTabForm = new DistributionsTabForm();
            Checker.IsTrue(distributionsTabForm.IsCarouselShown(), "Distribution carousel item is not shown by header");
            Checker.CheckEquals(type, distributionsTabForm.GetCurrentCarouselDistributionType(),
                $"{type} distribution is NOT shown by default by header");
            Checker.IsTrue(pieChart.IsExists(), $"{type} distribution does not contain pie-chart");
            Checker.CheckEquals(expectedDefaultTextinDonutChart, pieChart.GetPieChartHoverDistributionPercentDetailsText(),
                $"Unexpected text displayed into the donut chart by default for {type}");
        }
    }
}