using AutomatedTests.Enums.Tools.StockAnalyzer;
using AutomatedTests.Enums;
using AutomatedTests.Forms.ResearchPages.StockAnalyzer;
using AutomatedTests.Forms;
using AutomatedTests.SetUpsTearDowns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using AutomatedTests.Forms.Charts;
using AutomatedTests.Enums.Chart;
using AutomatedTests.Utils;

namespace UnitTests.Tests._07_Invest._01_StockAnalyzer
{
    [TestClass]
    public class TC_1411_StockAnalyzer_CheckThatComparisonButtonWorksAsExpected : BaseTestUnitTests
    {
        private const int TestNumber = 1411;

        private string ticker;
        private string tickerToComparison;
        private string expectedComparisonLabel;
        private string singInMarkerAtSameCurrency;
        private string singInMarkerAtDifferentCurrency;
        private string expectedCompareWithLabel;

        [TestInitialize]
        public void TestInitialize()
        {
            ticker = GetTestDataAsString(nameof(ticker));
            tickerToComparison = GetTestDataAsString(nameof(tickerToComparison));
            expectedComparisonLabel = GetTestDataAsString(nameof(expectedComparisonLabel));
            expectedCompareWithLabel = GetTestDataAsString(nameof(expectedCompareWithLabel));
            singInMarkerAtSameCurrency = GetTestDataAsString(nameof(singInMarkerAtSameCurrency));
            singInMarkerAtDifferentCurrency = GetTestDataAsString(nameof(singInMarkerAtDifferentCurrency));

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsLifetime));

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1411$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("Test checks that Comparison button works as expected: https://tr.a1qa.com/index.php?/cases/view/19234198")]
        [TestCategory("StockAnalyzer"), TestCategory("Chart")]
        public override void RunTest()
        {
            LogStep(1, $"Enter '{ticker}' in Search for Ticker field");
            new MainMenuForm().SetSymbol(ticker);
            var stockAnalyzerForm = new StockAnalyzerForm();
            stockAnalyzerForm.AssertIsOpen();
            var chartLegendForm = new ChartsLegendSettingsForm();
            
            LogStep(2, "Open Chart tab");
            stockAnalyzerForm.ActivateTabWithoutChartWaiting(StockAnalyzerTabs.ChartSettings);
            Checker.IsTrue(stockAnalyzerForm.IsTabActive(StockAnalyzerTabs.ChartSettings), "Chart settings tab is not active");
            var dataYMarkers = chartLegendForm.GetYMarkersLabelsText();
            Checker.IsTrue(dataYMarkers.Any(), "Markers are not present");
            Checker.IsTrue(dataYMarkers.First().Contains(singInMarkerAtSameCurrency), "Sign currency is not present");
            
            LogStep(3, "Click on 'Comparison' button and check that 'Search for Ticker' field appeared under 'Comparison' button");
            var chartSettingsForm = new ChartSettingsTabForm();
            chartSettingsForm.ClickComparisonActionsButton();
            Checker.IsTrue(chartSettingsForm.IsComparisonAutoCompletePresent(), "Autocompleate is not present");

            LogStep(4, "Enter {test data: symbol1} in the Search for Ticker field under 'Comparison' button and click it from autocomplete dropdown.");
            chartSettingsForm.SetSymbolComparison(tickerToComparison);
            dataYMarkers = chartLegendForm.GetYMarkersLabelsText();
            Asserts.Batch(
                    () =>
                    Assert.IsTrue(chartLegendForm.IsChartLinePresent(ChartLineTypes.Comparison), "Comparison price line is not present"),
                    () =>
                    Assert.IsTrue(dataYMarkers.First().Contains(singInMarkerAtDifferentCurrency), $"Sign {singInMarkerAtDifferentCurrency} is not present"),
                    () =>
                    Assert.AreEqual(expectedCompareWithLabel, chartSettingsForm.GetComparisonLabelText(), "Comparison text is not present"),
                    () =>
                    Assert.IsTrue(chartSettingsForm.IsCrossButtonPresent(), "Cross button is shown"),
                    () =>
                    Assert.AreEqual(tickerToComparison, chartSettingsForm.GetComparisonTicker(), $"{tickerToComparison} is not present")
                    );

            LogStep(5, "Hover cursor over price lines on the chart and check that price of added ticker is displayed in tooltip");
            chartLegendForm.IsTooltipPresentOnChartByPriceLineFocus();
            var comparisonSymbol = chartLegendForm.GetTooltipItems().ChartTooltipDataTypeToText[ChartLineTypes.Comparison].ToString();
            Checker.CheckContains(tickerToComparison, comparisonSymbol, $"{tickerToComparison} is not present in tooltip");

            LogStep(6, "Click on 'cross' icon on added ticker." +
                "Check that Text 'Compare with' and selected ticker with 'cross' icon has been replaced by 'Comparison' button." +
                "Purple price line disappeared on chart.");
            chartSettingsForm.ClickComparisonCrossButton();
            dataYMarkers = chartLegendForm.GetYMarkersLabelsText();
            Checker.CheckEquals(expectedComparisonLabel, chartSettingsForm.GetComparisonLabelText(), "Comparison button is not present");
            Checker.IsTrue(dataYMarkers.First().Contains(singInMarkerAtSameCurrency), "Sign currency is not present");
            Assert.IsFalse(chartLegendForm.IsChartLinePresent(ChartLineTypes.Comparison), "Comparison line is present");
        }
    }
}