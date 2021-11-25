using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Forms;
using AutomatedTests.Forms.MyPortfolios;
using AutomatedTests.Forms.ResearchPages.PVQAnalyzer;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._03_MyPortfolios._07_PvqAnalyzer
{
    [TestClass]
    public class TC_0107_Research_PvqAnalyzer_SelectAPortfolio : BaseTestUnitTests
    {
        private const int TestNumber = 107;

        private readonly List<PortfolioModel> portfoliosModels = new List<PortfolioModel>();
        private readonly List<PositionsDBModel> positionsModels = new List<PositionsDBModel>();
        private readonly List<int> portfolioIds = new List<int>();

        [TestInitialize]
        public void TestInitialize()
        {
            var portfolioCurrency = GetTestDataAsString("Currency");
            portfoliosModels.Add(new PortfolioModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = portfolioCurrency
            });
            portfoliosModels.Add(new PortfolioModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName2")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType2"),
                Currency = portfolioCurrency
            });
            var tradeTypeLong = GetTestDataAsString("TradeTypeLong");
            var tradeTypeShort = GetTestDataAsString("TradeTypeShort");
            var entryDates = GetTestDataValuesAsListByColumnName("EntryDate");
            positionsModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolStock1"),
                TradeType = tradeTypeLong,
                PurchaseDate = entryDates[0]
            });
            positionsModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolStock2"),
                TradeType = tradeTypeShort,
                PurchaseDate = entryDates[1]
            });
            positionsModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolOption2"),
                TradeType = tradeTypeShort
            });
            positionsModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolOption1"),
                TradeType = tradeTypeLong
            });
            positionsModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolStock3"),
                TradeType = tradeTypeLong,
                PurchaseDate = entryDates[0]
            });
            positionsModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolStock4"),
                TradeType = tradeTypeShort,
                PurchaseDate = entryDates[1]
            });
            positionsModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolStock5"),
                TradeType = tradeTypeLong,
                PurchaseDate = entryDates[0]
            });
            positionsModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolStock6"),
                TradeType = tradeTypeShort
            });
            positionsModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolOption3"),
                TradeType = tradeTypeShort
            });
            positionsModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolOption4"),
                TradeType = tradeTypeLong
            });

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPremium));
            foreach (var portfolioModel in portfoliosModels)
            {
                portfolioIds.Add(PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel));
            }
            int positionsQuantityInOnePortfolio = positionsModels.Count / 2;
            for (int i = 0; i < positionsQuantityInOnePortfolio; i++)
            {
                PositionsAlertsSetUp.AddPositionViaDB(portfolioIds[0], positionsModels[i]);
                PositionsAlertsSetUp.AddPositionViaDB(portfolioIds[1], positionsModels[i + positionsQuantityInOnePortfolio]);
            }
            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_107$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("ResearchPage"), TestCategory("PVQAnalyzer")]
        [Description("The test checks workability of PVQ Analyzer tool https://tr.a1qa.com/index.php?/cases/view/19232908")]
        public override void RunTest()
        {
            LogStep(1, "Click PVQ Analyzer");
            new MyPortfoliosMenuForm().ClickMyPortfoliosMenuItem(MyPortfoliosMenuItems.PvqAnalyzer);

            LogStep(2, "Select any portfolio(for example, first after the item 'All Watch' in dropdown");
            var pvqAnalyzerForm = new PvqAnalyzerForm();
            pvqAnalyzerForm.SelectPortfolioMultipleByText(new PortfoliosQueries().SelectPortfolioName(portfolioIds[0]));
            pvqAnalyzerForm.ClickAnalyzeAndScrollToTheDrillDown();

            Checker.IsTrue(pvqAnalyzerForm.IsVqChartPresent(), "VQ chart is not present and empty");
            Checker.IsTrue(pvqAnalyzerForm.IsLegendPresent(), "Legend area is not present and empty");
            Checker.CheckEquals(pvqAnalyzerForm.GetNumberOfLegendItems(), pvqAnalyzerForm.GetNumberOfFilledPathInChart(),
                "Number of item in the legend is not the same as number of filled path in chart");

            LogStep(3, "Click on any filled path on chart and compare Values with Legend and drilldown");
            var numberOfChartItems1 = pvqAnalyzerForm.GetNumberOfFilledPathInChart();
            pvqAnalyzerForm.ClickItemByChartTick(numberOfChartItems1 - 1);
            var percentValueFromCenter = pvqAnalyzerForm.GetPercentValueFromChartCenter();
            var drillDownText = pvqAnalyzerForm.GetDrillDownLabel();

            Checker.IsTrue(pvqAnalyzerForm.IsPercentValueInCentreOfChartDisplayed(), "In center of chart percent value is not displayed");
            Checker.CheckContains(percentValueFromCenter, drillDownText,
                "Percent value is not the same as in corresponded record in drilldown");

            LogStep(4, "Select 'All' item in dropdown ");
            pvqAnalyzerForm.SelectPortfolioMultipleByText(AllPortfoliosKinds.All.ToString());
            pvqAnalyzerForm.ClickAnalyzeAndScrollToTheDrillDown();

            Checker.IsTrue(pvqAnalyzerForm.IsVqChartPresent(), "VQ chart is not present and empty");
            Checker.IsTrue(pvqAnalyzerForm.IsLegendPresent(), "Legend area is not present and empty");
            Checker.CheckEquals(pvqAnalyzerForm.GetNumberOfLegendItems(), pvqAnalyzerForm.GetNumberOfFilledPathInChart(),
                "Number of item in the legend is not the same as number of filled path in chart for All portfolios");

            LogStep(5, "Click on any filled path on chart and compare Values with Legend and drilldown");
            var numberOfChartItems2 = pvqAnalyzerForm.GetNumberOfFilledPathInChart();
            pvqAnalyzerForm.ClickItemByChartTick(numberOfChartItems2 - 1);
            var quantityOfFilledPathInChart = pvqAnalyzerForm.GetNumberOfFilledPathInChart();
            var quantityOfLegendItems = pvqAnalyzerForm.GetNumberOfLegendItems();
            Checker.IsTrue(pvqAnalyzerForm.IsVqChartPresent(), "VQ chart is not present and empty");
            Checker.IsTrue(pvqAnalyzerForm.IsLegendPresent(), "Legend area is not present and empty");
            Checker.CheckEquals(quantityOfFilledPathInChart, quantityOfLegendItems,
                $"Number of item {quantityOfLegendItems} is not the same as number of filled path in chart {quantityOfFilledPathInChart}");
        }
    }
}