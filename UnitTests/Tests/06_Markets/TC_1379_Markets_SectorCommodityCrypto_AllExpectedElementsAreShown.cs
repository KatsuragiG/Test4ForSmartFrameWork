using AutomatedTests.Enums;
using AutomatedTests.Enums.Chart;
using AutomatedTests.Enums.FilterEnums;
using AutomatedTests.Enums.Markets;
using AutomatedTests.Enums.Tools;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Markets;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Forms.ResearchPages.StockAnalyzer;
using AutomatedTests.Forms.Screener;
using AutomatedTests.Models.MarketModels;
using AutomatedTests.Models.PillsModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TradeStops.Common.Enums;
using TradeStops.Common.Utils;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._06_Markets
{
    [TestClass]
    public class TC_1379_Markets_SectorCommodityCrypto_AllExpectedElementsAreShown : BaseTestUnitTests
    {
        private const int TestNumber = 1379;
        private const int ColumnQuantityOutlookWithHiddenMarkdown = 1;
        private const int ColumnQuantitySectorWithHiddenMarkdown = 2;

        private string pageSectorsShortDescription;
        private string pageCommoditiesShortDescription;
        private string pageCryptoShortDescription;

        [TestInitialize]
        public void TestInitialize()
        {
            pageSectorsShortDescription = GetTestDataAsString(nameof(pageSectorsShortDescription));
            pageCommoditiesShortDescription = GetTestDataAsString(nameof(pageCommoditiesShortDescription));
            pageCryptoShortDescription = GetTestDataAsString(nameof(pageCryptoShortDescription));

            LogStep(0, "Precondition - Login as registered user");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPlatinum));

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Markets);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1379$", DataAccessMethod.Sequential)]
        [Description("The test checks all expected elements are present on the pages Markets, Sectors, Commodities and Crypto. https://tr.a1qa.com/index.php?/cases/view/21011919")]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("MarketHealth"), TestCategory("Chart")]
        public override void RunTest()
        {
            LogStep(1, "Click S&P Sector tab.");
            var marketsHealthCommonForm = new MarketsHealthCommonForm();
            ClickMarketsMenuItemsAndCheckIt(MarketsMenuItems.SandPSectors);

            LogStep(2, "Make sure there is a grid with data.");
            marketsHealthCommonForm.ClickMarketGridModeButton(GridModeTypes.List);
            var expectedCellsInGrid = (EnumUtils.GetValues<SectorsColumnTypes>().Count() + ColumnQuantitySectorWithHiddenMarkdown) * EnumUtils.GetValues<SectorTypes>().Count();
            Checker.CheckEquals(expectedCellsInGrid, marketsHealthCommonForm.GetCountNotEmptyCellsInGrid(),
                $"{MarketsMenuItems.SandPSectors.GetStringMapping()} tab does not contain expected cells in tab");

            LogStep(3, "Check page description.");
            Checker.CheckEquals(pageSectorsShortDescription, marketsHealthCommonForm.GetExpandedTextAbovePageItems(),
                $"{MarketsMenuItems.SandPSectors.GetStringMapping()} page description is not as expected.");

            var mainMenuNavigation = new MainMenuNavigation();
            foreach (var sector in EnumUtils.GetValues<SectorTypes>())
            {
                LogStep(4, 7, "Check Sectors section. Make sure expected elements are present within section: Latest Close, Overall Status, Health Distribution, Chart and" +
                    $"Price range pill for {sector.GetStringMapping()}");
                marketsHealthCommonForm.ClickMarketGridModeButton(GridModeTypes.Tile);
                Checker.IsTrue(marketsHealthCommonForm.IsMarketHealthSectionPresent(sector),
                    $"{sector.GetStringMapping()} section is not present on the {MarketsMenuItems.SandPSectors.GetStringMapping()} form");

                var sectorForm = new MajorIndiceForm((int)sector);
                var indiceData = sectorForm.GetMajorIndiceData();
                Checker.CheckNotEquals(string.Empty, indiceData.MarketStatistics.LatestClosePrice,
                    $"Latest Close is not present for the {sector.GetStringMapping()} sector");
                Checker.IsFalse(HealthPillModel.CheckEmptyText(indiceData.MarketPills.HealthPill),
                    $"Health pill is not present for the {sector.GetStringMapping()} sector");
                Checker.IsFalse(BullBearPillModel.CheckEmptyText(indiceData.MarketPills.BullBearPill),
                    $"BullBear pill is not present for the {sector.GetStringMapping()} sector");
                var expectedSectorName = $"{sector.GetStringMapping()} ({sector.GetDescription()})";
                Checker.CheckEquals(expectedSectorName.ToUpperInvariant(), sectorForm.GetSectionName(),
                    $"Name for Market Pills is not as expected for the {sector.GetStringMapping()} sector");
                Checker.CheckContains(HealthStatusFilter.Green.GetStringMapping(),
                    indiceData.HealthDistributionLine.MainText,
                    $"Main text Health in Distribution does not contains expected wording for zone for {sector.GetStringMapping()}");
                Checker.IsFalse(PriceRangePillModel.IsPillHasEmptyTexts(indiceData.PriceRangePill),
                    $"Low/High Price is not present for the {sector.GetStringMapping()} sector");

                LogStep(6, $"Check Sectors links to stock Analyzer from tile for {sector.GetStringMapping()}");
                sectorForm.ClickSectionLink();
                var stockAnalyzerForm = new StockAnalyzerForm();
                stockAnalyzerForm.AssertIsOpen();
                Checker.CheckEquals(sector.GetDescription(), stockAnalyzerForm.GetSymbolTreeSelectSingleValue(),
                    "Unexpected ticker is shown");
                new DistributionsTabForm().AssertIsOpen();
                mainMenuNavigation.OpenSpSectors();

                LogStep(7, $"Check Sectors links to screener from grid for {sector.GetStringMapping()}");
                marketsHealthCommonForm.ClickMarketGridModeButton(GridModeTypes.List);
                marketsHealthCommonForm.SelectContextMenuItemFromMarketGrid((int)sector, MarketsContextNavigation.ViewComponents);
                var screenerFiltersForm = new ScreenerFiltersForm();
                screenerFiltersForm.AssertIsOpen();
                var actualSectors = screenerFiltersForm.GetCurrentSectorIndustryFilterModel(ScreenerFilterType.Sectors);
                Checker.IsTrue(actualSectors.ActiveSectorIndustryNames.Contains(sector.GetStringMapping()),
                    $"Sector does not contains expected item for {sector.GetStringMapping()}");
                screenerFiltersForm.ClickBack();
            }

            LogStep(8, "Open Commodities tab.");
            mainMenuNavigation.OpenSpSectors();
            ClickMarketsMenuItemsAndCheckIt(MarketsMenuItems.Commodities);
            marketsHealthCommonForm.ClickMarketGridModeButton(GridModeTypes.List);
            expectedCellsInGrid = (EnumUtils.GetValues<CommoditiesColumnTypes>().Count() + ColumnQuantityOutlookWithHiddenMarkdown) * EnumUtils.GetValues<CommoditiesEtfTypes>().Count();
            Checker.CheckEquals(expectedCellsInGrid, marketsHealthCommonForm.GetCountNotEmptyCellsInGrid(),
                $"{MarketsMenuItems.Commodities.GetStringMapping()} tab does not contain expected cells in tab");

            LogStep(9, "Check page description.");
            Checker.CheckEquals(pageCommoditiesShortDescription, marketsHealthCommonForm.GetExpandedTextAbovePageItems(),
                $"{MarketsMenuItems.Commodities.GetStringMapping()} page description is not as expected.");

            LogStep(10, "Check Commodity section");
            marketsHealthCommonForm.ClickMarketGridModeButton(GridModeTypes.Tile);
            foreach (var etfCommodity in EnumUtils.GetValues<CommoditiesEtfTypes>())
            {
                var commodityForm = new MajorIndiceForm((int)etfCommodity);
                commodityForm.ScrollToTheSection();
                Checker.CheckNotEquals(string.Empty, commodityForm.GetIndexStatistics().LatestClosePrice,
                    $"Latest Close is not present for the {etfCommodity.GetStringMapping()} sector");
                Checker.IsFalse(HealthPillModel.CheckEmptyText(commodityForm.GetHealthPill()),
                    $"Health pill is not present for the {etfCommodity.GetStringMapping()} sector");
                var expectedCommodityName = $"{etfCommodity.GetStringMapping()} ({etfCommodity.GetDescription()})";
                Checker.CheckEquals(expectedCommodityName.ToUpperInvariant(), commodityForm.GetSectionName(),
                    $"Name for Market Pills is not as expected for the {etfCommodity.GetStringMapping()} sector");

                Checker.IsFalse(PriceRangePillModel.IsPillHasEmptyTexts(commodityForm.GetPricesRangePillFromTile()),
                    $"Low/High Price is not present for the {etfCommodity.GetStringMapping()} sector");
            }

            LogStep(11, "Click Every ticker in grid and check that correct positions card is shown");
            marketsHealthCommonForm.ClickMarketGridModeButton(GridModeTypes.List);
            foreach (var etfCommodity in EnumUtils.GetValues<CommoditiesEtfTypes>())
            {
                marketsHealthCommonForm.ClickMarketItemInGrid(etfCommodity.GetDescription());
                var stockAnalyzerForm = new StockAnalyzerForm();
                Checker.CheckEquals(etfCommodity.GetDescription(), stockAnalyzerForm.GetSymbolTreeSelectSingleValue(),
                    $"{etfCommodity.GetStringMapping()} ETF link leads to unexpected symbol.");

                mainMenuNavigation.OpenCommodities();
                marketsHealthCommonForm.ClickMarketGridModeButton(GridModeTypes.List);
            }

            LogStep(12, "Open Crypto tab.");
            ClickMarketsMenuItemsAndCheckIt(MarketsMenuItems.Crypto);
            var cryptoMarketOutlookForm = new CryptoMarketOutlookForm();
            cryptoMarketOutlookForm.AssertIsOpen();

            expectedCellsInGrid = (EnumUtils.GetValues<CryptoOutlookColumnTypes>().Count() + ColumnQuantityOutlookWithHiddenMarkdown) * EnumUtils.GetValues<CryptoIndiceTypes>().Count();
            Checker.CheckEquals(expectedCellsInGrid, marketsHealthCommonForm.GetCountNotEmptyCellsInGrid(),
                $"{MarketsMenuItems.Commodities.GetStringMapping()} tab does not contain expected cells in tab");
            Checker.IsTrue(cryptoMarketOutlookForm.IsPureQuantQualificationChartPresent(),
                $"Pure Quant Qualification chart is not present on the {MarketsMenuItems.Crypto.GetStringMapping()} tab");
            Checker.IsTrue(cryptoMarketOutlookForm.IsChartLinePresent(PureQuantQualificationChartLines.PercentOfPureQuantQualifiedCoins),
                $"{PureQuantQualificationChartLines.PercentOfPureQuantQualifiedCoins.GetStringMapping()} line is not present on the {MarketsMenuItems.Crypto.GetStringMapping()} tab");

            LogStep(13, "Check page description.");
            Checker.CheckEquals(pageCryptoShortDescription, marketsHealthCommonForm.GeCryptoPageDescription(),
                $"{MarketsMenuItems.Crypto.GetStringMapping()} page description is not as expected.");
        }

        private void ClickMarketsMenuItemsAndCheckIt(MarketsMenuItems marketsMenuItems)
        {
            var marketsHealthCommonForm = new MarketsHealthCommonForm();
            marketsHealthCommonForm.ClickMarketsItem(marketsMenuItems);
            Checker.IsTrue(marketsHealthCommonForm.IsMarketsTabSelected(marketsMenuItems), $"{marketsMenuItems.GetStringMapping()} tab is not active.");
        }
    }
}