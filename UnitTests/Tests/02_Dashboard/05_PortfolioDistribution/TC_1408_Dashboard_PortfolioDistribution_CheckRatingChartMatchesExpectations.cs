using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.DsiForSymbols;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Elements;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Dashboard;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.Tools;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Dashboard;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Utils;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;
using WebdriverFramework.Framework.WebDriver;

namespace UnitTests.Tests._02_Dashboard._05_PortfolioDistribution
{
    [TestClass]
    public class TC_1408_Dashboard_PortfolioDistribution_CheckRatingChartMatchesExpectations : BaseTestUnitTests
    {
        private const int TestNumber = 1408;

        private string chartDefaultText;
        private string ratingLegendTitle;

        private List<GlobalRatingTypes> globalRatingItems = EnumUtils.GetValues<GlobalRatingTypes>().ToList();
        private Dictionary<string, string> ratingDistributionColors;
        private Dictionary<string, string> ratingDistributionValues = new Dictionary<string, string>();

        [TestInitialize]
        public void TestInitialize()
        {
            var userProductSubscriptions = GetUserProductSubscriptions("userSubscription");

            chartDefaultText = GetTestDataAsString(nameof(chartDefaultText));
            ratingLegendTitle = GetTestDataAsString(nameof(ratingLegendTitle));

            ratingDistributionColors = globalRatingItems.Select(t => t.GetStringMapping())
                .ToDictionary(t => t, t => Dictionaries.RatingColorCodes[t.ParseAsEnumFromStringMapping<GlobalRatingTypes>()]);
            ratingDistributionColors.Add(Constants.NotAvailableAcronym, ColorConstants.GrayNaHexColor);

            var portfolioModel = new PortfolioDBModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = ((int)GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType")).ToString(),
                CurrencyId = $"{(int)GetTestDataParsedAsEnumFromStringMapping<Currency>("Currency")}"
            };

            var ratingTickerValues = ratingDistributionColors.Keys.ToDictionary(t => t, t => 0);
            var tickersToAdd = GetTestDataValuesAsListByColumnNameAndRemoveEmpty("tickersToAdd");
            var tradeTypes = GetTestDataValuesAsListByColumnNameAndRemoveEmpty("TradeType");
            var positionsModels = PreparePositionsModels(ratingTickerValues, tickersToAdd, tradeTypes);

            var ratingTickerPercentValues = ratingTickerValues.Values.Select(t => Convert.ToDouble(t))
                .Select(t => $"{(t * 100 / tickersToAdd.Count).ToString().ToFractionalString()}{Constants.PercentSign}").ToList();
            ratingDistributionValues = ratingDistributionColors.Keys.Zip(ratingTickerPercentValues, (k, v) => new { Key = k, Value = v })
                .ToDictionary(x => x.Key, x => x.Value);

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));

            var portfolioId = PortfoliosSetUp.AddPortfolioViaDb(UserModels.First(), portfolioModel);
            foreach (var positionModel in positionsModels)
            {
                PositionsAlertsSetUp.AddPositionViaDB(portfolioId, positionModel);
            }
            PortfoliosSetUp.AddInvestmentAudPortfoliosWithOpenPosition(UserModels.First().Email);
            PortfoliosSetUp.AddWatchOnlyUsdPortfoliosWithOpenPosition(UserModels.First().Email);

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
            new DashboardForm().SelectPortfolioStatisticsWidgetPortfolio(portfolioModel.Name);
        }

        private List<PositionsDBModel> PreparePositionsModels(Dictionary<string, int> ratingTickerValues, List<string> tickersToAdd, List<string> tradeTypes)
        {
            var positionsModels = new List<PositionsDBModel>();
            var symbolsQueries = new SymbolsQueries();
            var positionsQueries = new PositionsQueries();
            for (int i = 0; i < tickersToAdd.Count; i++)
            {
                positionsModels.Add(new PositionsDBModel
                {
                    Symbol = tickersToAdd[i],
                    TradeType = ((int)tradeTypes[i].ParseAsEnumFromStringMapping<PositionTradeTypes>()).ToString()
                });
                var symbolId = Parsing.ConvertToInt(positionsQueries.SelectSymbolIdNameUsingSymbol(tickersToAdd[i]).SymbolId);
                var assetType = positionsQueries.SelectAssetTypeNameBySymbolId(symbolId);
                var dbRankData = symbolsQueries.SelectRankDataForSymbol(tickersToAdd[i]);
                if (tradeTypes[i].Equals(PositionTradeTypes.Long.GetStringMapping()) && dbRankData != null
                    && assetType != PositionAssetTypes.Crypto.ToString())
                {
                    ratingTickerValues[((GlobalRatingTypes)dbRankData.GlobalRank).GetStringMapping()]++;
                }
                else
                {
                    ratingTickerValues[Constants.NotAvailableAcronym]++;
                }
            }

            return positionsModels;
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1408$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("Test for data for Rating donut chart matched expectations. https://tr.a1qa.com/index.php?/cases/view/22681003")]
        [TestCategory("DashboardPortfolioDistribution"), TestCategory("Dashboard"), TestCategory("StockRating")]

        public override void RunTest()
        {
            LogStep(1, "Go to Portfolio Distribution widget.");
            var dashboardWidget = new WidgetForm(WidgetTypes.PortfolioDistribution, true);
            dashboardWidget.ClickWidgetContentTab(WidgetPortfolioDistributionTabs.Rating);
            Checker.CheckEquals(WidgetPortfolioDistributionTabs.Rating.GetStringMapping().ToUpper(),
                dashboardWidget.GetWidgetContentActiveTabName(), "Rating tab is not active");

            LogStep(2, "Check the donut chart.");
            Checker.CheckEquals(chartDefaultText, dashboardWidget.WidgetPieChart.GetPieChartHoverDistributionPercentDetailsText(),
                "Unexpected text displayed into the donut chart by default");
            var pieChart = dashboardWidget.WidgetPieChart;
            var actualRatingDonutColors = pieChart.GetPieChartSectionsColors();
            Checker.CheckEquals(ratingDistributionColors.Keys.Count, actualRatingDonutColors.Count,
                "Unexpected area quantity in Rating pie-chart");
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(ratingDistributionColors.Values.ToList(), actualRatingDonutColors),
                $"There are colors mismatching:\n{GetExpectedResultsString(ratingDistributionColors.Values.ToList())}\n{GetActualResultsString(actualRatingDonutColors)}");

            LogStep(3, "Check the legend on the right of the donut chart.");
            Checker.CheckEquals(ratingLegendTitle, pieChart.GetPieChartLegendDefaultLabel(),
                "Rating Chart Legend label by default is not as expected");
            var legendTextDictionary = pieChart.GetPieChartLegendItemLabels()
                .Select(t => t.Split('\n')).ToDictionary(t => t.First(), t => t.Last().ReplaceNewLineWithTrim());
            foreach (var ratingZone in ratingDistributionColors.Keys)
            {
                Checker.IsTrue(legendTextDictionary.ContainsKey(ratingZone),
                    $"Legend does not contain '{ratingZone}' key:\n{GetActualResultsString(legendTextDictionary.Keys.ToList())}");
                Checker.CheckEquals(ratingDistributionValues[ratingZone],
                    legendTextDictionary.GetValueByPartOfTheKeyOrDefault(ratingZone, $"{Constants.DefaultStringZeroDecimalValue}{Constants.PercentSign}"),
                    $"Value for Rating '{ratingZone}' is not as expected");
            }

            LogStep(4, 9, "Click on items in legend.");
            foreach (var item in ratingDistributionColors.Keys)
            {
                pieChart.ClickChartLegendItemLabelByText(item);

                var percentInCenter = pieChart.GetPieChartHoverDistributionPercentText();
                var colorOfPercentInCenter = pieChart.GetPieChartHoverDistributionPercentColor();
                Checker.IsTrue(Constants.PercentValuesRegex.IsMatch(percentInCenter.Trim()),
                    $"Percent value '{percentInCenter}' is not matched expectations for {item}");
                Checker.CheckContains(ratingDistributionColors[item].Replace("#", string.Empty), colorOfPercentInCenter.GetColorFromRgbString().Name,
                    $"Color for percent value in chart is not as expected for {item}");
                Checker.CheckEquals(item, pieChart.GetPieChartHoverDistributionPercentDetailsText(),
                    "Label for Rating zone is not matched expectations");
            }
            pieChart.ClickChartLegendItemLabelByText(ratingDistributionColors.Keys.Last());

            LogStep(10, 14, "Hover over the area of donut chart with color and click.");
            CheckPieChartInteractions(pieChart, legendTextDictionary);
        }

        private void CheckPieChartInteractions(PieChartElement pieChart, Dictionary<string, string> legendTextDictionary)
        {            
            var nonZerosItemsInLegend = legendTextDictionary.RemoveByValue($"{Constants.DefaultStringZeroDecimalValue}{Constants.PercentSign}");
            for (int i = 1; i <= ratingDistributionColors.Keys.Count; i++)
            {
                pieChart.ClickItemInChartByNumber(i);
                var textInCenter = pieChart.GetPieChartHoverDistributionPercentDetailsText();
                if (!textInCenter.Contains(chartDefaultText) && !string.IsNullOrEmpty(textInCenter))
                {
                    var percentInCenter = pieChart.GetPieChartHoverDistributionPercentText();
                    Checker.IsTrue(Constants.PercentValuesRegex.IsMatch(percentInCenter.Trim()),
                        $"Percent value '{percentInCenter}' is not matched expectations for legend");

                    var actualZonesWithPercentValues = nonZerosItemsInLegend.Where(t => t.Value.Equals(percentInCenter.Trim())).FirstOrDefault();
                    Checker.IsFalse(string.IsNullOrEmpty(actualZonesWithPercentValues.Key),
                        $"Legend does not contain record for {percentInCenter} value");

                    if (!string.IsNullOrEmpty(actualZonesWithPercentValues.Key))
                    {
                        Checker.CheckEquals(Dictionaries.RatingColorCodes[actualZonesWithPercentValues.Key.ParseAsEnumFromStringMapping<GlobalRatingTypes>()],
                            ratingDistributionColors[actualZonesWithPercentValues.Key],
                            $"Color for pie-chart is not as expected for {percentInCenter} value");
                    }
                }
                else if (string.IsNullOrEmpty(textInCenter))
                {
                    Checker.Fail($"Empty text at clicking {i} section rating pie chart");
                }
                else
                {
                    Logger.Instance.Warn($"Unexpected text '{textInCenter}' in center of pie chart for # {i}");
                }

                pieChart.ClickItemInChartByNumber(i);
            }
        }
    }
}