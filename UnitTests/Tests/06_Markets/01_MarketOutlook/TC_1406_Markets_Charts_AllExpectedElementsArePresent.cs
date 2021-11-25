using AutomatedTests.ConstantVariables;
using AutomatedTests.Enums.Chart;
using AutomatedTests.Enums.Markets;
using AutomatedTests.Enums;
using AutomatedTests.Forms.Markets;
using AutomatedTests.Forms;
using AutomatedTests.Models.MarketModels;
using AutomatedTests.Models.PillsModels;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._06_Markets._01_MarketOutlook
{
    [TestClass]
    public class TC_1406_Markets_Charts_AllExpectedElementsArePresent : BaseTestUnitTests
    {
        private const int TestNumber = 1406;

        private IndiceTypes market;
        private string rangePillName;
        private string stockPillName;
        private string healthDistributionName;
        private string marketName;

        [TestInitialize]
        public void TestInitialize()
        {
            var userProductSubscriptions = GetUserProductSubscriptions("userSubscription");

            rangePillName = GetTestDataAsString(nameof(rangePillName));
            stockPillName = GetTestDataAsString(nameof(stockPillName));
            healthDistributionName = GetTestDataAsString(nameof(healthDistributionName));
            marketName = GetTestDataAsString(nameof(marketName));
            market = marketName.ParseAsEnumFromStringMapping<IndiceTypes>();

            LogStep(0, "Precondition - Login as registered user");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));

            LoginSetUp.LogIn(UserModels[0]);
            new MainMenuForm().ClickMenuItem(MainMenuItems.Markets);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1406$", DataAccessMethod.Sequential)]
        [Description("The test checks all expected elements are present on the market chart. https://tr.a1qa.com/index.php?/cases/view/22864686")]
        [TestMethod]
        [TestCategory("MarketHealth"), TestCategory("Chart")]
        public override void RunTest()
        {
            LogStep(1, "Set list Mode. Click required market in the grid");
            var marketHealthForm = new MarketsHealthCommonForm();
            marketHealthForm.ClickMarketGridModeButton(GridModeTypes.List);
            Assert.IsTrue(marketHealthForm.IsTableShown(), $"Table is not shown for {market.GetDescription()} after switching to List");
            marketHealthForm.ClickMarketItemInGrid(marketName);
            marketHealthForm.AssertIsOpen();

            LogStep(2, $"Make sure expected elements are present within section: Latest Close for {market.GetStringMapping()}");
            var indiceForm = new MajorIndiceForm(market);
            var indiceData = indiceForm.GetMajorIndiceChartData();
            Assert.IsFalse(indiceData.Equals(new MarketHealthSectionModel()), $"'{market.GetStringMapping()}' indice is not shown on the form");
            Checker.CheckNotEquals(string.Empty, indiceData.MarketStatistics.LatestClosePrice,
                $"Latest Close is not present for the {market.GetStringMapping()} indice");
            Checker.CheckNotEquals(string.Empty, indiceData.MarketStatistics.DailyChangeAbsolute,
                $"Absolute Date Price Change is not present for the {market.GetStringMapping()} indice");
            Checker.CheckNotEquals(string.Empty, indiceData.MarketStatistics.DailyChangePercent,
                $"Percent Date Price Change is not present for the {market.GetStringMapping()} indice");
            Checker.IsTrue(indiceData.MarketStatistics.TimeStampLatestPrice.StartsWith(MarketOutlookColumnTypes.LatestPrice.GetStringMapping())
                || indiceData.MarketStatistics.TimeStampLatestPrice.StartsWith(MarketOutlookColumnTypes.LatestClose.GetStringMapping()),
                $"Latest Price/Close wording {indiceData.MarketStatistics.TimeStampLatestPrice} starts unexpectedly for the {market.GetStringMapping()} indice");

            LogStep(3, $"Make sure expected elements are present within section: Pills for {market.GetStringMapping()}");
            Checker.IsFalse(HealthPillModel.CheckEmptyText(indiceData.MarketPills.HealthPill),
                $"Health pill is not present for the {market.GetStringMapping()} indice");
            Checker.IsFalse(VqPillModel.CheckEmptyText(indiceData.MarketPills.VqPill),
                $"VQ pill is not present for the {market.GetStringMapping()} indice");
            Checker.CheckEquals(stockPillName, indiceForm.GetPillsSectionTitle(),
                $"Name for Market Pills is not as expected for the {market.GetStringMapping()} indice");

            LogStep(4, $"Make sure expected elements are present within section: 'Health Distribution' pie chart for {market.GetStringMapping()}");
            Checker.IsFalse(HealthDistributionInDonutModel.IsDonutHasEmptyTexts(indiceData.HealthDistributionInDonut),
                $"Health Distribution pie chart does not have three pie chart for {market.GetStringMapping()} indice");
            Checker.IsFalse(HealthDistributionInDonutModel.IsPillHasEmptyTexts(indiceData.HealthDistributionInDonut),
                $"Health Distribution does not have three pill for {market.GetStringMapping()} indice");
            Checker.CheckEquals(healthDistributionName, indiceForm.GetHealthDistributionPieSectionTitle(),
                $"Name for Health Distribution pie chart is not as expected for the {market.GetStringMapping()} indice");

            LogStep(5, $"Make sure expected elements are present within section: Price chart for {market.GetStringMapping()}");
            Checker.IsTrue(indiceForm.IsChartLinePresent(ChartLineTypes.Price),
                $"Price line is not present for {market.GetStringMapping()} indice");
            Checker.IsTrue(indiceForm.IsChartLinePresent(ChartLineTypes.HealthTrend),
                $"Health trend is not present for {market.GetStringMapping()} indice");
            Checker.IsTrue(indiceForm.AreAllChartPeriodButtonsPresent(market),
                $"All buttons for chart for {market.GetStringMapping()} indice are not shown");
            Checker.IsTrue(indiceForm.IsPeriodScrollPresent(), $"Period Scroll for chart for {market.GetStringMapping()} indice is not shown");
            Checker.IsTrue(indiceForm.AreBothAxisPresent(), $"Two axes for chart for {market.GetStringMapping()} indice are not shown");
            indiceForm.SelectChartPeriod(ChartPeriod.All);
            Checker.IsTrue(indiceForm.IsChartLinePresent(ChartLineTypes.YellowZone),
                $"Yellow Zone is not present for {market.GetStringMapping()} indice");
            Checker.IsTrue(indiceForm.IsChartLinePresent(ChartLineTypes.StopLoss),
                $"Stop Loss is not present for {market.GetStringMapping()} indice");
            Checker.IsTrue(indiceForm.IsInitBellPresent(ChartLineTypes.YellowZone),
                $"Init Bell for '{ChartLineTypes.YellowZone.GetStringMapping()}' is not present for {market.GetStringMapping()} indice");
            Checker.IsTrue(indiceForm.IsInitBellPresent(ChartLineTypes.StopLoss),
                $"Init Bell for '{ChartLineTypes.StopLoss.GetStringMapping()}' is not present for {market.GetStringMapping()} indice");
            Checker.IsTrue(indiceForm.IsTriggeredBellPresent(ChartLineTypes.YellowZone),
                $"Triggered Bell for '{ChartLineTypes.YellowZone.GetStringMapping()}' is not present for {market.GetStringMapping()} indice");
            Checker.IsTrue(indiceForm.IsTriggeredBellPresent(ChartLineTypes.StopLoss),
                $"Triggered Bell for '{ChartLineTypes.StopLoss.GetStringMapping()}' is not present for {market.GetStringMapping()} indice");
            Checker.IsTrue(indiceForm.IsCurrentHighBellPresent(ChartLineTypes.StopLoss),
                $"Current High for '{ChartLineTypes.StopLoss.GetStringMapping()}' is not present for {market.GetStringMapping()} indice");
            Checker.IsTrue(indiceForm.IsChartLinePresent(ChartLineTypes.EntrySignal),
                $"Sign for '{ChartLineTypes.EntrySignal.GetStringMapping()}' is not present for {market.GetStringMapping()} indice");
            Checker.IsTrue(indiceForm.IsTriggeredBellPresent(ChartLineTypes.BullBearIndicator),
                $"Triggered bell for '{ChartLineTypes.BullBearIndicator.GetStringMapping()}' is not present for {market.GetStringMapping()} indice");

            LogStep(6, $"Make sure expected elements are present within section: Price range pill for {market.GetStringMapping()}");
            Checker.IsFalse(PriceRangePillModel.IsPillHasEmptyTexts(indiceData.PriceRangePill),
                $"Low/High Price is not present for the {market.GetStringMapping()} indice");
            Checker.CheckEquals(rangePillName, indiceData.PriceRangePill.Name,
                $"Name for price pill is not as expected for the {market.GetStringMapping()} indice");
            if (indiceForm.IsStatisticPresent(MarketStatisticTypes.LatestPrice, (int)market))
            {
                Checker.CheckEquals(indiceData.MarketStatistics.LatestClosePrice, indiceData.PriceRangePill.LatestClose,
                    $"Prices are not matched in the price pill and Latest close block for the {market.GetStringMapping()} indice");
            }

            LogStep(7, $"Make sure expected elements are present within section: Health Distribution History Chart for {market.GetStringMapping()}");
            Checker.IsTrue(indiceData.HealthDistributionHistory.ColorGreenFill.Contains(ColorConstants.BackgroundColorForActiveCheckbox),
                $"Color for Green Health Distribution History Chart is not present for the {market.GetStringMapping()} indice");
            Checker.IsTrue(indiceData.HealthDistributionHistory.ColorRedFill.Contains(ColorConstants.RGBColorPartForHighlightedField),
                $"Color for Red Health Distribution History Chart is not present for the {market.GetStringMapping()} indice");
            Checker.IsTrue(indiceData.HealthDistributionHistory.ColorYellowFill.Contains(ColorConstants.RGBColorPartForYellowHealth),
                $"Color for Yellow Health Distribution History Chart is not present for the {market.GetStringMapping()} indice");
            Checker.CheckEquals(healthDistributionName.Replace(" ", ""), indiceData.HealthDistributionHistory.YLegend.Replace("\r\n", ""),
                $"Y axe name is not as expected for the {market.GetStringMapping()} indice");
        }
    }
}