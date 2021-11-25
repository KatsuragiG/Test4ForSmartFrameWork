using System.Collections.Generic;
using System.Linq;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Forms;
using AutomatedTests.Forms.MyPortfolios;
using AutomatedTests.Forms.ResearchPages.AssetAllocation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._03_MyPortfolios._06_AssetAllocation
{
    [TestClass]
    public class TC_0105_Research_AssetAllocation_SelectAPortfolio : BaseTestUnitTests
    {
        private const int TestNumber = 107;

        private readonly List<PortfolioModel> portfoliosModels = new List<PortfolioModel>();
        private readonly List<PositionsDBModel> positionsModels = new List<PositionsDBModel>();
        private readonly List<int> portfolioIds = new List<int>();
        private int portfolioNumber;

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
                Symbol = GetTestDataAsString("SymbolStock5"),
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
            positionsModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolStock7"),
                TradeType = tradeTypeLong
            });
            positionsModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolStock8"),
                TradeType = tradeTypeShort,
                PurchaseDate = entryDates[1]
            });
            portfolioNumber = GetTestDataAsInt("Portfolio");

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
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_105$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("ResearchPage"), TestCategory("AssetAllocation")]
        [Description("The test checks workability of Asset Allocation tool https://tr.a1qa.com/index.php?/cases/view/19232909")]
        public override void RunTest()
        {
            LogStep(1, "Click Asset Allocation");
            new MyPortfoliosMenuForm().ClickMyPortfoliosMenuItem(MyPortfoliosMenuItems.AssetAllocation);

            LogStep(2, "Select any portfolio in dropdown ");
            var assetAllocationForm = new AssetAllocationForm();
            assetAllocationForm.SelectAnyPortfolioByNumber(portfolioNumber);
            assetAllocationForm.ClickAnalyzeAndScrollToTheDrillDown();
            var numberOfChartItems1 = assetAllocationForm.GetNumberOfFilledPathInChart();

            Checker.IsTrue(assetAllocationForm.IsAssetChartPresent(), "Asset Allocation chart is not present and empty");
            Checker.IsTrue(assetAllocationForm.IsLegendPresent(), "Legend area is not present and empty");
            Checker.CheckEquals(numberOfChartItems1, assetAllocationForm.GetNumberOfLegendItems(),
                "Number of item is not the same as number of filled path in chart");

            LogStep(3, "Click on any filled path on chart and compare Values with Legend and drilldown");
            assetAllocationForm.ClickItemInChartByNumber(numberOfChartItems1);
            var chartValueFromChartCenter = assetAllocationForm.GetChartValueFromChartCenter();
            var percentValueFromChartCenter = assetAllocationForm.GetPercentValueFromChartCenter();
            Checker.IsTrue(assetAllocationForm.IsPercentValueInCentreOfChartDisplayed(), "3 In center of chart percent value is  not displayed");
            Checker.IsTrue(assetAllocationForm.IsChartValueInCentreOfChartDisplayed(), "3 In center of chart value is  not displayed");
            Checker.CheckEquals(percentValueFromChartCenter, assetAllocationForm.GetPercentValueFromLegend(numberOfChartItems1),
                "3 Percent value is not the same as in Legend");
            Checker.CheckEquals(percentValueFromChartCenter, assetAllocationForm.GetColumnAxisValueInDrillDown().Split('\r')[1].Replace("\n", ""),
                "3 Percent value is not the same as in Drilldown");
            Assert.AreEqual(chartValueFromChartCenter, assetAllocationForm.GetChartValueFromLegend(numberOfChartItems1), "3 Chart value is not the same as in Legend");
            Checker.IsTrue(assetAllocationForm.GetDrillDownLabel().ToLower().Contains(chartValueFromChartCenter.ToLower())
                    && assetAllocationForm.GetDrillDownLabel().Contains(percentValueFromChartCenter),
                "3 Percent and Chart Value is not in Drilldown");

            LogStep(4, "Select 'All' item in dropdown ");
            assetAllocationForm.SelectPortfolioMultipleByText(AllPortfoliosKinds.All.GetStringMapping());
            assetAllocationForm.ClickAnalyzeAndScrollToTheDrillDown();
            Checker.IsTrue(assetAllocationForm.IsAssetChartPresent(), "Asset Allocation chart is not present and empty");
            Checker.IsTrue(assetAllocationForm.IsLegendPresent(), "Legend area is not present and empty");
            Checker.IsTrue(assetAllocationForm.GetNumberOfFilledPathInChart().Equals(assetAllocationForm.GetNumberOfLegendItems()),
                "Number of item is not the same as number of filled path in chart");

            LogStep(5, 6, "Click on any filled path on chart and compare Values with Legend and drilldown");
            var numberOfChartItems2 = assetAllocationForm.GetNumberOfFilledPathInChart();
            assetAllocationForm.ClickItemInChartByNumber(numberOfChartItems2);
            percentValueFromChartCenter = assetAllocationForm.GetPercentValueFromChartCenter();
            chartValueFromChartCenter = assetAllocationForm.GetChartValueFromChartCenter();
            Asserts.Batch(
                () =>
                Assert.IsTrue(assetAllocationForm.IsPercentValueInCentreOfChartDisplayed(), "7 In center of chart percent value is  not displayed"),
                () =>
                Assert.IsTrue(assetAllocationForm.IsChartValueInCentreOfChartDisplayed(), "7 In center of chart value is  not displayed"),
                () =>
                Assert.AreEqual(percentValueFromChartCenter, assetAllocationForm.GetPercentValueFromLegend(numberOfChartItems2), "7 Percent value is not the same as in Legend"),
                () =>
                Assert.AreEqual(percentValueFromChartCenter, assetAllocationForm.GetColumnAxisValueInDrillDown().Split('\r')[1].Replace("\n", ""),
                    "7 Percent value is not the same as in Drilldown"),
                () =>
                Assert.AreEqual(chartValueFromChartCenter, assetAllocationForm.GetChartValueFromLegend(numberOfChartItems2), "7 Chart value is not the same as in Legend"),
                () =>
                Assert.IsTrue(assetAllocationForm.GetDrillDownLabel().ToLower().Contains(chartValueFromChartCenter.ToLower())
                        && assetAllocationForm.GetDrillDownLabel().Contains(percentValueFromChartCenter),
                    "7 Percent and Chart Value is not in Drilldown")
                );
        }
    }
}