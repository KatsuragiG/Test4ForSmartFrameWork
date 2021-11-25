using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.PositionData;
using AutomatedTests.Enums.FilterEnums;
using AutomatedTests.Enums.Markets;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Forms.Dashboard;
using AutomatedTests.Forms.Markets;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Forms.ResearchPages.StockAnalyzer;
using AutomatedTests.Forms;
using AutomatedTests.Models.MarketModels;
using AutomatedTests.Models.PillsModels;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Linq;
using System;
using TradeStops.Common.Utils;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._06_Markets._01_MarketOutlook
{
    [TestClass]
    public class TC_1376_Markets_MarketOutlook_AllExpectedElementsArePresent : BaseTestUnitTests
    {
        private const int TestNumber = 1375;

        private IndiceTypes market;
        private MarketPillsModel dashboardPillsModels;
        private readonly HDSymbolStatisticsModel hdSymbolStatisticsModel = new HDSymbolStatisticsModel();
        private int marketOutlookTabsQuantity;
        private bool isCryptoTabShown;
        private string marketName;
        private string rangePillName;
        private string healthDistributionName;
        private string pageShortDescription;
        private string dbStatisticOneDayChangeInPercent;

        [TestInitialize]
        public void TestInitialize()
        {
            var userProductSubscriptions = GetUserProductSubscriptions("userSubscription");

            marketOutlookTabsQuantity = GetTestDataAsInt(nameof(marketOutlookTabsQuantity));
            isCryptoTabShown = GetTestDataAsBool(nameof(isCryptoTabShown));
            rangePillName = GetTestDataAsString(nameof(rangePillName));
            healthDistributionName = GetTestDataAsString(nameof(healthDistributionName));
            pageShortDescription = GetTestDataAsString(nameof(pageShortDescription));
            marketName = GetTestDataAsString(nameof(marketName));
            market = marketName.ParseAsEnumFromStringMapping<IndiceTypes>();

            var positionDataQueries = new PositionDataQueries();
            hdSymbolStatisticsModel.TradeDate = positionDataQueries.SelectLastTradeDate(market.GetDescription());
            hdSymbolStatisticsModel.LatestClose = Parsing.ConvertToDouble(positionDataQueries.SelectStockOrOptionData(PositionAssetTypes.Stock, market.GetDescription()).TradeClose)
                .ToString(CultureInfo.InvariantCulture);
            hdSymbolStatisticsModel.PreviousClose =
                positionDataQueries.SelectHdAdjPriceForSymbolIdDate(market.GetDescription(),
                    Parsing.ConvertToShortDateString(DateTime.Parse(hdSymbolStatisticsModel.TradeDate).AddDays(Constants.DaysQuantityForPreviousClose).AsShortDate()))
                .GetTradeAdjClose().ToString(CultureInfo.InvariantCulture);
            dbStatisticOneDayChangeInPercent = positionDataQueries.SelectSymbolStatisticsForSymbol(market.GetDescription()).OneDayChangeInPercent;

            LogStep(0, "Precondition - Login as registered user");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));

            LoginSetUp.LogIn(UserModels.First());
            var mainMenuForm = new MainMenuForm();
            mainMenuForm.ClickMenuItem(MainMenuItems.Dashboard);
            dashboardPillsModels = new DashboardForm().GetOverallStatus(market);
            mainMenuForm.ClickMenuItem(MainMenuItems.Markets);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1376$", DataAccessMethod.Sequential)]
        [Description("The test checks all expected elements are present on the page. https://tr.a1qa.com/index.php?/cases/view/20417137")]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("MarketHealth")]
        public override void RunTest()
        {
            LogStep(1, "Make sure there are 3 or 4 tabs. Make sure there is a name and short description of the page.");
            var marketHealthForm = new MarketsHealthCommonForm();
            Checker.CheckEquals(marketOutlookTabsQuantity, marketHealthForm.GetMenuTabsQuantity(), "Page description is not as expected.");
            Checker.CheckEquals(pageShortDescription, marketHealthForm.GetExpandedTextAbovePageItems(), "Page description is not as expected.");
            Checker.CheckEquals(isCryptoTabShown, marketHealthForm.IsTabPresent(MarketsMenuItems.Crypto), "Crypto tab is not as expected.");

            LogStep(2, "Make sure there are expected markets quantity");
            Checker.CheckEquals(GridModeTypes.Tile, marketHealthForm.GetGridMode(), "Tile mode is not shown by default.");
            var expectedMarkets = EnumUtils.GetValues<IndiceTypes>();
            Checker.CheckEquals(expectedMarkets.Count(), marketHealthForm.GetTilesQuantity(), "Sections quantity is not expected.");
            foreach (var section in expectedMarkets)
            {
                Checker.IsTrue(marketHealthForm.IsMarketHealthSectionPresent(section), $"{section.GetStringMapping()} section is not present on the form");
            }

            LogStep(3, $"Collect data for tile in model MarketHealthSectionModel for {market.GetStringMapping()}");
            var indiceForm = new MajorIndiceForm((int)market);
            var indiceData = indiceForm.GetMajorIndiceData();
            Assert.IsFalse(indiceData.Equals(new MarketHealthSectionModel()),
                $"'{market.GetStringMapping()}' indice has empty data from the form");

            LogStep(4, $"Check that name for market section contains Market name and Ticker for {market.GetStringMapping()}");
            Checker.CheckContains(marketName.ToUpperInvariant(), indiceData.MarketName,
                $"Market name is not matched for the {market.GetStringMapping()} indice");
            Checker.CheckContains(market.GetDescription().ToUpperInvariant(), indiceData.MarketName,
                $"Market name is not contains correct ticker for the {market.GetStringMapping()} indice");

            LogStep(5, $"Check that name for market has link to stock analyzer with correct symbolid for {market.GetStringMapping()}");
            Checker.CheckContains(indiceData.SymbolData.SymbolId, indiceForm.GetSectionLink(),
                $"Market name link does not have correct symbolid for the {market.GetStringMapping()} indice");

            LogStep(6, $"Check Latest Price/Close wording and value for {market.GetStringMapping()}");
            var currencySign = ((Currency)indiceData.CurrencyId).GetDescription();
            var expectedLatestCloseDollarChange = $"{currencySign}{hdSymbolStatisticsModel.LatestCloseDollarChange().ToFractionalString()}";
            var expectedLatestClosePercentChange = hdSymbolStatisticsModel.LatestClosePercentChange().ToFractionalString();
            StringUtility.DetectLatestCloseChangingSingAndColor(expectedLatestClosePercentChange, out string expectedSignOfLatestCloseChanging, out string expectedColorOfLatestCloseChanging);

            var expectedLatestClose = $"{currencySign}{hdSymbolStatisticsModel.LatestClose.ToFractionalString()}";
            Checker.IsTrue(indiceData.MarketStatistics.TimeStampLatestPrice.StartsWith(MarketOutlookColumnTypes.LatestPrice.GetStringMapping())
                || indiceData.MarketStatistics.TimeStampLatestPrice.StartsWith(MarketOutlookColumnTypes.LatestClose.GetStringMapping()),
                $"Latest Price/Close wording {indiceData.MarketStatistics.TimeStampLatestPrice} starts unexpectedly for the {market.GetStringMapping()} indice");
            Checker.CheckContains(Parsing.ConvertToShortDateString(hdSymbolStatisticsModel.TradeDate),
                indiceData.MarketStatistics.TimeStampLatestPrice,
                $"Latest price/close date is not as expected for {market.GetStringMapping()}");
            Checker.CheckEquals(expectedLatestClose, indiceData.MarketStatistics.LatestClosePrice.Replace(",", string.Empty),
                $"Latest close price is not as expected for {market.GetStringMapping()}");
            Checker.CheckEquals($"{expectedSignOfLatestCloseChanging}{expectedLatestCloseDollarChange.DeleteMathSigns()}",
                indiceData.MarketStatistics.DailyChangeAbsolute,
                $"Latest Close change $ value is not as expected for {market.GetStringMapping()}");
            try
            {
                Assert.AreEqual($"({expectedSignOfLatestCloseChanging}{expectedLatestClosePercentChange.DeleteMathSigns()}%)",
                    indiceData.MarketStatistics.DailyChangePercent,
                    $"Latest Close change % value is not as expected for {market.GetStringMapping()}");
            }
            catch (AssertFailedException)
            {
                Checker.CheckEquals($"({expectedSignOfLatestCloseChanging}{dbStatisticOneDayChangeInPercent.DeleteMathSigns().ToFractionalString()}%)",
                    indiceData.MarketStatistics.DailyChangePercent,
                    $"Latest Close change % in Stat value is not as expected for {market.GetStringMapping()}");
            }

            Checker.CheckContains(expectedColorOfLatestCloseChanging, indiceData.MarketStatistics.DailyChangeColor,
                $"Latest Close change color is not as expected for {market.GetStringMapping()}");

            LogStep(7, $"Check pills for {market.GetStringMapping()}");
            Checker.CheckEquals(dashboardPillsModels.HealthPill, indiceData.MarketPills.HealthPill,
                $"Health pill is not as expected for {market.GetStringMapping()}");
            Checker.CheckEquals(dashboardPillsModels.BullBearPill, indiceData.MarketPills.BullBearPill,
                $"BullBear pill is not as expected for {market.GetStringMapping()}");

            LogStep(8, $"Check health Distribution widget for {market.GetStringMapping()}");
            Checker.CheckContains(ColorConstants.BackgroundColorForActiveCheckbox, indiceData.HealthDistributionLine.ColorGreenFill,
                $"Color for green Health in Distribution is not as expected for {market.GetStringMapping()}");
            Checker.CheckContains(ColorConstants.RGBColorPartForYellowHealth, indiceData.HealthDistributionLine.ColorYellowFill,
                $"Color for yellow Health in Distribution is not as expected for {market.GetStringMapping()}");
            Checker.CheckContains(ColorConstants.RGBColorPartForHighlightedField, indiceData.HealthDistributionLine.ColorRedFill,
                $"Color for red Health in Distribution is not as expected for {market.GetStringMapping()}");
            Checker.CheckContains(indiceData.HealthDistributionLine.GreenHealthDistributionValue.Replace("%", string.Empty).ToFractionalString(),
                indiceData.HealthDistributionLine.MainText,
                $"Main text Health in Distribution does not contains expected value for {market.GetStringMapping()}");
            Checker.CheckContains(HealthStatusFilter.Green.GetStringMapping(),
                indiceData.HealthDistributionLine.MainText,
                $"Main text Health in Distribution does not contains expected wording for zone for {market.GetStringMapping()}");
            Checker.CheckContains(indiceData.HealthDistributionLine.GreenHealthDistributionValue.Replace("%", string.Empty).ToFractionalString(),
                indiceData.HealthDistributionLine.TextInToolTip.First(),
                $"Tooltip line 1 value in Distribution is not as expected for {market.GetStringMapping()}");
            Checker.CheckContains(HealthStatusFilter.Green.GetStringMapping(),
                indiceData.HealthDistributionLine.TextInToolTip.First(),
                $"Tooltip line 1 wording in Distribution is not as expected for {market.GetStringMapping()}");
            Checker.CheckContains(indiceData.HealthDistributionLine.YellowHealthDistributionValue.Replace("%", string.Empty).ToFractionalString(),
                indiceData.HealthDistributionLine.TextInToolTip[1],
                $"Tooltip line 2 value in Distribution is not as expected for {market.GetStringMapping()}");
            Checker.CheckContains(HealthStatusFilter.Yellow.GetStringMapping(),
                indiceData.HealthDistributionLine.TextInToolTip[1],
                $"Tooltip line 2 wording in Distribution is not as expected for {market.GetStringMapping()}");
            Checker.CheckContains(indiceData.HealthDistributionLine.RedHealthDistributionValue.Replace("%", string.Empty).ToFractionalString(),
                indiceData.HealthDistributionLine.TextInToolTip[2],
                $"Tooltip line 3 value in Distribution is not as expected for {market.GetStringMapping()}");
            Checker.CheckContains(HealthStatusFilter.Red.GetStringMapping(),
                indiceData.HealthDistributionLine.TextInToolTip[2],
                $"Tooltip line 3 wording in Distribution is not as expected for {market.GetStringMapping()}");

            LogStep(9, $"Check Price Range widget for {market.GetStringMapping()}");
            Checker.CheckContains(rangePillName, indiceData.PriceRangePill.Name,
                $"Price Range slider name is not as expected for {market.GetStringMapping()}");
            Checker.CheckEquals(indiceData.MarketStatistics.LatestClosePrice,
                indiceData.PriceRangePill.LatestClose,
                $"Latest close price in Price Range Pill is not as expected for {market.GetStringMapping()}");
            Checker.IsFalse(string.IsNullOrEmpty(indiceData.PriceRangePill.LowClosePrice),
                $"Low close price in Price Range Pill is empty for {market.GetStringMapping()}");
            Checker.IsFalse(string.IsNullOrEmpty(indiceData.PriceRangePill.HighClosePrice),
                $"High close price in Price Range Pill is empty for {market.GetStringMapping()}");
            if (indiceData.PriceRangePill.HighClosePrice != indiceData.PriceRangePill.LowClosePrice)
            {
                var expectedPointerShiftValue = Constants.UpperLimitForPercent *
                    (GetDoublePrice(indiceData.PriceRangePill.LatestClose) - GetDoublePrice(indiceData.PriceRangePill.LowClosePrice)) /
                    (GetDoublePrice(indiceData.PriceRangePill.HighClosePrice) - GetDoublePrice(indiceData.PriceRangePill.LowClosePrice));
                Checker.CheckEquals(expectedPointerShiftValue.ToString("N1"), indiceData.PriceRangePill.PointerShift.ToString("N1"),
                    $"Pointer shift place is not as expected for {market.GetStringMapping()}");
            }
            else
            {
                Checker.CheckEquals(Constants.UpperLimitForPercent.ToString("N2"), indiceData.PriceRangePill.PointerShift.ToString("N2"),
                    $"Pointer shift place 100% is not as expected for {market.GetStringMapping()}");
            }

            LogStep(10, "Switch mode from tile to list");
            marketHealthForm.ClickMarketGridModeButton(GridModeTypes.List);
            Checker.CheckEquals(GridModeTypes.List, marketHealthForm.GetGridMode(), "List mode is not shown after button clicking");
            Checker.CheckEquals(0, marketHealthForm.GetTilesQuantity(), "Sections tile quantity is not 0 after switching to List");
            Checker.IsTrue(marketHealthForm.IsTableShown(), $"Table is not shown for {market.GetDescription()} after switching to List");
            Checker.CheckEquals(expectedMarkets.Count(), marketHealthForm.GetRowsQuantity(), "Rows quantity is not expected");

            LogStep(11, $"Remember data for the {market.GetStringMapping()} from grid");
            var gridData = marketHealthForm.GetGridRowModel((int)market);
            Assert.IsFalse(gridData == null, $"Table data is null for {market.GetDescription()}");

            LogStep(12, $"Check market name in table for {market.GetStringMapping()}");
            Checker.CheckContains(marketName, gridData.Market,
                $"Market name is not matched for the {market.GetStringMapping()} indice");
            Checker.CheckContains(market.GetDescription().ToUpperInvariant(), gridData.Market,
                $"Market name is not contains correct ticker for the {market.GetStringMapping()} indice");

            LogStep(13, $"Check that name for market has link to stock analyzer from grid for {market.GetStringMapping()}");
            Checker.CheckContains(indiceData.SymbolData.Symbol.Replace("$", string.Empty).Replace("^", string.Empty), 
                marketHealthForm.GetLinkFromGrid((int)market, MarketOutlookColumnTypes.Market),
                $"Market link in grid does not have correct symbolid for the {market.GetStringMapping()} indice");

            LogStep(14, $"Check Latest Price/Close column values for {market.GetStringMapping()}");
            Checker.CheckEquals(expectedLatestClose, gridData.LatestClose.Replace(",", string.Empty),
                $"Latest close is not as expected for {market.GetStringMapping()}");
            Checker.IsFalse(string.IsNullOrEmpty(gridData.LatestPrice),
                $"Latest price is empty as expected for {market.GetStringMapping()}");
            Checker.CheckEquals(indiceData.MarketStatistics.DailyChangeAbsolute.Replace("+", string.Empty), gridData.DailyGain.Split(' ')[0],
                $"Latest Close change $ in grid is not as expected for {market.GetStringMapping()}");
            Checker.CheckEquals(indiceData.MarketStatistics.DailyChangePercent.Replace("+", string.Empty),
                gridData.DailyGain.Split(' ')[1].ReplaceNewLineWithTrim(),
                $"Latest Close change % in grid is not as expected for {market.GetStringMapping()}");

            LogStep(15, $"Check grid pills for {market.GetStringMapping()}");
            Checker.CheckEquals(indiceData.MarketPills.HealthPill, gridData.Health,
                $"Health pill in grid is not as expected for {market.GetStringMapping()}");

            LogStep(16, $"Check Health Distribution in grid for {market.GetStringMapping()}");
            Checker.CheckContains(ColorConstants.BackgroundColorForActiveCheckbox, gridData.HealthDistribution.ColorGreenFill,
                $"Color for green Health in Distribution is not as expected for {market.GetStringMapping()}");
            Checker.CheckContains(ColorConstants.RGBColorPartForYellowHealth, gridData.HealthDistribution.ColorYellowFill,
                $"Color for yellow Health in Distribution is not as expected for {market.GetStringMapping()}");
            Checker.CheckContains(ColorConstants.RGBColorPartForHighlightedField, gridData.HealthDistribution.ColorRedFill,
                $"Color for red Health in Distribution is not as expected for {market.GetStringMapping()}");
            Checker.CheckEquals(indiceData.HealthDistributionLine.TextInToolTip.First(),
                gridData.HealthDistribution.TextInToolTip.First(),
                $"Tooltip line 1 in grid Distribution is not as expected for {market.GetStringMapping()}");
            Checker.CheckEquals(indiceData.HealthDistributionLine.TextInToolTip[1], gridData.HealthDistribution.TextInToolTip[1],
                $"Tooltip line 2 value in Distribution is not as expected for {market.GetStringMapping()}");
            Checker.CheckEquals(indiceData.HealthDistributionLine.TextInToolTip[2], gridData.HealthDistribution.TextInToolTip[2],
                $"Tooltip line 3 value in Distribution is not as expected for {market.GetStringMapping()}");

            LogStep(17, $"Check Price Range in grid for {market.GetStringMapping()}");
            Checker.CheckEquals(indiceData.PriceRangePill.LowClosePrice, gridData.PriceRange.LowClosePrice,
                $"Lowest price in Price Range Pill is not as expected for {market.GetStringMapping()}");
            Checker.CheckEquals(indiceData.PriceRangePill.HighClosePrice, gridData.PriceRange.HighClosePrice,
                $"Highest price in Price Range Pill is not as expected for {market.GetStringMapping()}");
            Checker.CheckEquals(indiceData.PriceRangePill.PointerShift, gridData.PriceRange.PointerShift,
                $"Pointer shift in grid is not as expected for {market.GetStringMapping()}");

            LogStep(18, $"Check that swim line for market {market.GetStringMapping()} is shown");
            Checker.IsTrue(marketHealthForm.IsSwimLineShown((int)market), $"Swim line for {market.GetDescription()} is not shown");

            LogStep(19, "Switch mode from list to tile");
            marketHealthForm.ClickMarketGridModeButton(GridModeTypes.Tile);
            Checker.CheckEquals(GridModeTypes.Tile, marketHealthForm.GetGridMode(), "Tile mode is not shown after button clicking");
            Checker.CheckEquals(expectedMarkets.Count(), marketHealthForm.GetTilesQuantity(), "Sections tile quantity is not 0 after switching to List");

            LogStep(20, "Click on tile header");
            indiceForm.ClickSectionLink();
            var stockAnalyzerForm = new StockAnalyzerForm();
            stockAnalyzerForm.AssertIsOpen();
            Checker.CheckEquals(market.GetDescription(), stockAnalyzerForm.GetSymbolTreeSelectSingleValue(),
                "Unexpected ticker is shown");

            LogStep(21, "Check that Distributions tab is shown");
            new DistributionsTabForm().AssertIsOpen();
        }

        private static double GetDoublePrice(string price)
        {
            return Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(price));
        }
    }
}