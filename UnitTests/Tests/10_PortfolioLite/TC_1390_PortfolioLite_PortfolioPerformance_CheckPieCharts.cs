using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.DsiForSymbols;
using AutomatedTests.Database.Positions;
using AutomatedTests.Database.Users;
using AutomatedTests.Elements;
using AutomatedTests.Enums;
using AutomatedTests.Enums.PortfolioLite;
using AutomatedTests.Forms.PortfolioLite;
using AutomatedTests.Models.PositionsModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Enums;
using TradeStops.Common.Extensions;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._10_PortfolioLite
{
    [TestClass]
    public class TC_1390_PortfolioLite_PortfolioPerformance_CheckPieCharts : BaseTestUnitTests
    {
        private const int TestNumber = 1390;

        private string drillDownTitlePattern;
        private int addedContributionSectionQuantity;
        private int quantityOfAddedPositions;
        private readonly Dictionary<string, string> industryOfTickers = new Dictionary<string, string>();
        private readonly Dictionary<string, string> valueOfTickers = new Dictionary<string, string>();

        [TestInitialize]
        public void TestInitialize()
        {
            var positionsQueries = new PositionsQueries();
            var symbolsQueries = new SymbolsQueries();

            quantityOfAddedPositions = GetTestDataAsInt(nameof(quantityOfAddedPositions));
            var entryDate = GetTestDataAsString("entryDate");
            var symbolsToAdd = GetTestDataValuesAsListByColumnNameAndRemoveEmpty("symbolsToAdd");
            var positionsModels = new List<PortfolioLitePositionModel>();
            for (int i = 1; i <= quantityOfAddedPositions; i++)
            {
                positionsModels.Add(new PortfolioLitePositionModel
                {
                    Ticker = symbolsToAdd[i - 1],
                    BuyDate = entryDate,
                    Qty = GetTestDataAsString($"quantities{i}"),
                    IsLongType = GetTestDataAsBool($"isLongType{i}")
                });
                var symbolId = positionsQueries.SelectSymbolIdNameUsingSymbol(symbolsToAdd[i - 1]).SymbolId;
                var tickerFundamentalData = symbolsQueries.SelectFundamentalDataForSymbolViaSymbolId(symbolId);
                industryOfTickers.Add(symbolsToAdd[i - 1], tickerFundamentalData.GICSIndustryName);
                valueOfTickers.Add(symbolsToAdd[i - 1], GetTestDataAsString($"valueTickers{i}"));
            }

            addedContributionSectionQuantity = GetTestDataAsInt(nameof(addedContributionSectionQuantity));
            drillDownTitlePattern = GetTestDataAsString(nameof(drillDownTitlePattern));

            LogStep(0, "Preconditions. Create user with subscription to PortfolioLite. Add position");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.PortfolioLite));

            var fakeSnaid = StringUtility.RandomString(Constants.SnaidPattern);
            new UsersQueries().UpdateUserSnaid(fakeSnaid, UserModels.First().TradeSmithUserId);
            new PortfolioLiteNavigation().OpenPortfolioLiteWithUserGuid(UserModels.First());

            var portfolioLiteMainForm = new PortfolioLiteMainForm();
            portfolioLiteMainForm.ClickAddAPosition();
            portfolioLiteMainForm.AddPositions(positionsModels);
            Assert.AreEqual(quantityOfAddedPositions, portfolioLiteMainForm.GetPositionQuantityInGrid(),
                "Grid does not contain added position");
            portfolioLiteMainForm.ClickPortfolioSummaryWithScrollDown();
            portfolioLiteMainForm.ExitFrame();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1390$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("PortfolioLite")]
        [Description("Test checks Pie Charts behavior https://tr.a1qa.com/index.php?/cases/view/21116201")]
        public override void RunTest()
        {
            LogStep(1, "Check that Industry Allocation pie-chart is shown and has expected filling parts");
            var portfolioLiteMainForm = new PortfolioLiteMainForm();
            CheckIndustryAllocationPieChart();

            LogStep(10, "Check that Sector Allocation pie-chart is shown and has expected filling parts - Repeat steps 2-9");
            CheckPositionAllocationPieChart();

            LogStep(11, "Detect positions quantity with non-zero gain");
            var portfolioLiteDetailsForm = new PortfolioLiteDetailsForm();
            var tickers = portfolioLiteMainForm.GetColumnValues(PortfolioLiteColumnTypes.Ticker)
                .Select(t => t.Split('\r')[0]).ToList();
            var totalGainDollarValues = portfolioLiteMainForm.GetColumnValues(PortfolioLiteColumnTypes.TotalGain)
                .Select(t => t.Split(' ')[0].DeleteMathSigns()).ToList();
            var dictionaryTickerGain = tickers.Select((ticker, order) => new { ticker, value = totalGainDollarValues[order] })
                .ToDictionary(x => x.ticker, x => x.value);
            var nonZeroGainsTickersQuantity = totalGainDollarValues.Count(t => t != Constants.DefaultStringZeroDecimalValue);

            LogStep(12, "Check that Contribution ToPerformance section equals positions quantity with non-zero gain + 2 for common gain/loss");
            var chart = portfolioLiteDetailsForm.GetPieChartByType(PortfolioLiteDonutTypes.ContributionToPerformance);
            chart.AssertIsPresent();
            Checker.CheckEquals(PortfolioLiteDonutTypes.ContributionToPerformance.GetStringMapping(), chart.GetPieChartTitleText(),
                $"{PortfolioLiteDonutTypes.ContributionToPerformance.GetStringMapping()} pie-chart has unexpected title");
            Checker.CheckEquals(nonZeroGainsTickersQuantity + addedContributionSectionQuantity, chart.GetPieChartSectionsColors().Count,
                $"{PortfolioLiteDonutTypes.ContributionToPerformance.GetStringMapping()} pie-chart has unexpected sections quantity by color");

            portfolioLiteDetailsForm.ScrollUpPage();
            LogStep(13, 18, "Check Contribution ToPerformance outer pie-chart and drill-down data matching");
            for (int i = 1; i <= nonZeroGainsTickersQuantity; i++)
            {
                LogStep(13, $"Click on {i} filled outer path on {PortfolioLiteDonutTypes.ContributionToPerformance.GetStringMapping()} Allocation pie-chart");
                chart.ClickItemInChartByNumber(i, nonZeroGainsTickersQuantity);

                LogStep(14, $"Check that on {i} outer pie-chart expected {PortfolioLiteDonutTypes.ContributionToPerformance.GetStringMapping()}");
                GetWordingFromChartCenter(chart, out string[] textInCenter, out string descriptionInCenter);
                dictionaryTickerGain.TryGetValue(descriptionInCenter, out string actualValue);
                if (Constants.AllCurrenciesRegex.Match(actualValue ?? string.Empty).Value.Equals(Currency.USD.GetDescription()))
                {
                    Checker.IsTrue(actualValue.EqualsIgnoreCase(textInCenter[0]),
                        $"{PortfolioLiteDonutTypes.ContributionToPerformance.GetStringMapping()} pie-chart # {i} has unexpected section {descriptionInCenter} {textInCenter[0]}");
                }

                LogStep(15, $"Check that drill-down # {i} outer is shown");
                var drilldown = portfolioLiteDetailsForm.GetDrillDownByType(PortfolioLiteDonutTypes.ContributionToPerformance);
                drilldown.AssertIsPresent();

                LogStep(16, $"Check that Pie-chart and drill-down # {i} outer contains expected {PortfolioLiteDonutTypes.ContributionToPerformance.GetStringMapping()}");
                var textInDrilldownText = drilldown.GetDrillDownTickersGainsText().Select(t => t.Split('\r'))
                     .ToDictionary(x => x[0], x => x[1].ReplaceNewLineWithTrim().Split(' ')[0]);
                Checker.IsTrue(textInDrilldownText.TryGetValue(descriptionInCenter, out string actualText) && actualText.EqualsIgnoreCase(textInCenter[0]),
                    $"{PortfolioLiteDonutTypes.ContributionToPerformance.GetStringMapping()} drill-down # {i} has unexpected section {textInDrilldownText.Keys} {textInDrilldownText.Values}");

                LogStep(17, $"Click on {i} filled outer path on {PortfolioLiteDonutTypes.ContributionToPerformance.GetStringMapping()} Allocation pie-chart and check drill-down disappearing");
                chart.ClickItemInChartByNumber(i, nonZeroGainsTickersQuantity);
                drilldown.AssertIsAbsent();
            }

            LogStep(19, 22, "Check Contribution ToPerformance inner path pie-chart and drill-down data matching");
            for (int i = 1; i <= addedContributionSectionQuantity; i++)
            {
                LogStep(19, $"Click on {i} inner filled path on {PortfolioLiteDonutTypes.ContributionToPerformance.GetStringMapping()} Allocation pie-chart");
                chart.ClickItemInChartByNumber(i, addedContributionSectionQuantity);

                LogStep(20, $"Check that on {i} inner pie-chart expected {PortfolioLiteDonutTypes.ContributionToPerformance.GetStringMapping()} wording");
                GetWordingFromChartCenter(chart, out string[] textInCenter, out string descriptionInCenter);
                var drilldown = portfolioLiteDetailsForm.GetDrillDownByType(PortfolioLiteDonutTypes.ContributionToPerformance);
                drilldown.AssertIsPresent();
                var textInDrilldownTitle = GetFormattedTextInDrilldownTitle(drilldown);
                Checker.CheckEquals($"{descriptionInCenter}: {textInCenter[0]}", textInDrilldownTitle.First(),
                    $"{PortfolioLiteDonutTypes.ContributionToPerformance.GetStringMapping()} inner pie-chart # {i} has unexpected section {descriptionInCenter} {textInCenter[0]}");

                LogStep(21, $"Click on {i} filled inner path on {PortfolioLiteDonutTypes.ContributionToPerformance.GetStringMapping()} Allocation pie-chart and check drill-down disappearing");
                chart.ClickItemInChartByNumber(i, addedContributionSectionQuantity);
                drilldown.AssertIsAbsent();
            }
        }

        private void CheckPositionAllocationPieChart()
        {
            var portfolioLiteDetailsForm = new PortfolioLiteDetailsForm();
            var chart = portfolioLiteDetailsForm.GetPieChartByType(PortfolioLiteDonutTypes.PositionAllocation);
            chart.AssertIsPresent();

            Checker.CheckEquals(PortfolioLiteDonutTypes.PositionAllocation.GetStringMapping(), chart.GetPieChartTitleText(),
                $"{PortfolioLiteDonutTypes.PositionAllocation.GetStringMapping()} pie-chart has unexpected title");
            var expectedSectionTitles = valueOfTickers.Values.Distinct().ToList();
            Checker.CheckEquals(expectedSectionTitles.Count, chart.GetPieChartSectionsColors().Count,
                $"{PortfolioLiteDonutTypes.PositionAllocation.GetStringMapping()} pie-chart has unexpected sections quantity by color");

            LogStep(2, 9, "Check pie-chart and drill-down data matching");
            for (int i = 1; i <= expectedSectionTitles.Count; i++)
            {
                LogStep(2, $"Click on {i} filled path on {PortfolioLiteDonutTypes.PositionAllocation.GetStringMapping()} Allocation pie-chart");
                chart.ClickItemInChartByNumber(i);

                LogStep(3, $"Check that on {i} pie-chart expected {PortfolioLiteDonutTypes.PositionAllocation.GetStringMapping()}");
                GetWordingFromChartCenter(chart, out string[] textInCenter, out string descriptionInCenter);
                Checker.IsTrue(expectedSectionTitles.Contains(textInCenter.First()),
                    $"{PortfolioLiteDonutTypes.PositionAllocation.GetStringMapping()} pie-chart # {i} has unexpected section {textInCenter.First()}");

                LogStep(4, $"Check that drill-down # {i} is shown");
                var drilldown = portfolioLiteDetailsForm.GetDrillDownByType(PortfolioLiteDonutTypes.PositionAllocation);
                drilldown.AssertIsPresent();

                LogStep(5, 7, $"Check that Pie-chart and drill-down # {i} contains expected {PortfolioLiteDonutTypes.PositionAllocation.GetStringMapping()} and percent in axis");
                var textInDrilldownTitle = GetFormattedTextInDrilldownTitle(drilldown);
                Checker.CheckEquals(string.Format(drillDownTitlePattern, "%"), textInDrilldownTitle[0],
                    $"{PortfolioLiteDonutTypes.PositionAllocation.GetStringMapping()} drill-down # {i} has unexpected title");

                var textInDrilldownText = drilldown.GetDrillDownTickersGainsText().Select(t => t.Split(' '))
                     .ToDictionary(x => x[0], x => x[1].Replace("(", string.Empty).Replace(")", string.Empty));
                Checker.IsTrue(new DictionaryUtility.DictionaryComparer<string, string>().Equals(valueOfTickers, textInDrilldownText),
                    $"{PortfolioLiteDonutTypes.PositionAllocation.GetStringMapping()} drill-down # {i} has unexpected structure");

                LogStep(8, $"Click on {i} filled path on {PortfolioLiteDonutTypes.PositionAllocation.GetStringMapping()} Allocation pie-chart and check drill-down disappearing");
                chart.ClickItemInChartByNumber(i);
                drilldown.AssertIsAbsent();
            }
        }

        private void CheckIndustryAllocationPieChart()
        {
            var portfolioLiteDetailsForm = new PortfolioLiteDetailsForm();
            var chart = portfolioLiteDetailsForm.GetPieChartByType(PortfolioLiteDonutTypes.IndustryAllocation);
            chart.AssertIsPresent();

            Checker.CheckEquals(PortfolioLiteDonutTypes.IndustryAllocation.GetStringMapping(), chart.GetPieChartTitleText(),
                $"{PortfolioLiteDonutTypes.IndustryAllocation.GetStringMapping()} pie-chart has unexpected title");
            var expectedSectionTitles = industryOfTickers.Values.Distinct().ToList();
            Checker.CheckEquals(expectedSectionTitles.Count, chart.GetPieChartSectionsColors().Count,
                $"{PortfolioLiteDonutTypes.IndustryAllocation.GetStringMapping()} pie-chart has unexpected sections quantity by color");

            LogStep(2, 9, "Check pie-chart and drill-down data matching");
            for (int i = 1; i <= expectedSectionTitles.Count; i++)
            {
                LogStep(2, $"Click on {i} filled path on {PortfolioLiteDonutTypes.IndustryAllocation.GetStringMapping()} Allocation pie-chart");
                chart.ClickItemInChartByNumber(i);

                LogStep(3, $"Check that on {i} pie-chart expected {PortfolioLiteDonutTypes.IndustryAllocation.GetStringMapping()}");
                GetWordingFromChartCenter(chart, out string[] textInCenter, out string descriptionInCenter);
                Checker.IsTrue(expectedSectionTitles.Contains(descriptionInCenter),
                    $"{PortfolioLiteDonutTypes.IndustryAllocation.GetStringMapping()} pie-chart # {i} has unexpected section {descriptionInCenter}");

                LogStep(4, $"Check that drill-down # {i} is shown");
                var drilldown = portfolioLiteDetailsForm.GetDrillDownByType(PortfolioLiteDonutTypes.IndustryAllocation);
                drilldown.AssertIsPresent();

                LogStep(5, $"Check that Pie-chart and drill-down # {i} contains expected {PortfolioLiteDonutTypes.IndustryAllocation.GetStringMapping()} and percent in axis");
                var textInDrilldownTitle = GetFormattedTextInDrilldownTitle(drilldown);
                Checker.CheckEquals(descriptionInCenter, textInDrilldownTitle[0],
                    $"{PortfolioLiteDonutTypes.IndustryAllocation.GetStringMapping()} drill-down # {i} has unexpected title");

                Checker.CheckEquals(string.Format(drillDownTitlePattern, textInCenter[0]), textInDrilldownTitle[1],
                $"{PortfolioLiteDonutTypes.IndustryAllocation.GetStringMapping()} drill-down # {i} has unexpected title");

                var textInDrilldownAxis = drilldown.GetColumnAxisValueInDrillDown().Split('\r').Select(t => t.ReplaceNewLineWithTrim()).ToList();
                Checker.CheckEquals(textInCenter[0], textInDrilldownAxis[1],
                    $"{PortfolioLiteDonutTypes.IndustryAllocation.GetStringMapping()} drill-down and pie chart # {i} has unexpected percent value");
                Checker.CheckEquals(drilldown.GetDefaultDrillDownStartAxisValue(), textInDrilldownAxis[0],
                    $"{PortfolioLiteDonutTypes.IndustryAllocation.GetStringMapping()} drill-down and pie chart # {i} has unexpected percent value");

                LogStep(6, $"Check that drill-down # {i} contains expected tickers quantity in legend and pie-chart");
                var expectedTickersInDrilldown = industryOfTickers.Where(pair => pair.Value == descriptionInCenter)
                    .Select(pair => pair.Key).ToList();
                Checker.IsTrue(expectedTickersInDrilldown.Any(), "Expected tickers in drill-down are empty");

                var actualTickersAndHintInDrilldownChart = drilldown.GetSymbolPercentDictionaryFromDrillDownChart();
                Checker.CheckEquals(expectedTickersInDrilldown.Count, actualTickersAndHintInDrilldownChart.Count,
                    $"{PortfolioLiteDonutTypes.IndustryAllocation.GetStringMapping()} drill-down chart # {i} has unexpected chart items quantity");

                var actualTickersAndHintInDrilldownLegend = drilldown.GetSymbolsHintDictionaryFromDrillDownLegend();
                Checker.CheckEquals(expectedTickersInDrilldown.Count, actualTickersAndHintInDrilldownLegend.Count,
                    $"{PortfolioLiteDonutTypes.IndustryAllocation.GetStringMapping()} drill-down legend # {i} has unexpected legend items quantity");

                LogStep(7, $"Check # {i} drill-down and pie-chart hints matching");
                foreach (var ticker in expectedTickersInDrilldown)
                {
                    Checker.CheckEquals(actualTickersAndHintInDrilldownLegend[ticker], actualTickersAndHintInDrilldownChart[ticker],
                        $"{PortfolioLiteDonutTypes.IndustryAllocation.GetStringMapping()} drill-down legend # {i} has error in hint matching for {ticker}");
                }

                LogStep(8, $"Click on {i} filled path on {PortfolioLiteDonutTypes.IndustryAllocation.GetStringMapping()} Allocation pie-chart and check drill-down disappearing");
                chart.ClickItemInChartByNumber(i);
                drilldown.AssertIsAbsent();
            }
        }

        private static void GetWordingFromChartCenter(PieChartElement chart, out string[] textInCenter, out string descriptionInCenter)
        {
            textInCenter = chart.GetPieChartHoverFullDistributionText().Split('\r');
            descriptionInCenter = textInCenter[1].ReplaceNewLineWithTrim();
        }

        private static List<string> GetFormattedTextInDrilldownTitle(DrillDownElement drilldown)
        {
            return drilldown.GetDrillDownTitleText().Split('(').Select(t => t.ReplaceNewLineWithTrim().Replace(")", string.Empty)).ToList();
        }
    }
}