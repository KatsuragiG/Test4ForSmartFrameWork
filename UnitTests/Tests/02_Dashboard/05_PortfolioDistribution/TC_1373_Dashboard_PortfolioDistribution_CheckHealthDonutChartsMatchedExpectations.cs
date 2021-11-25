using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Dashboard;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Dashboard;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.PieChartElementSteps;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._02_Dashboard._05_PortfolioDistribution
{
    [TestClass]
    public class TC_1373_Dashboard_PortfolioDistribution_CheckHealthDonutChartsMatchedExpectations : BaseTestUnitTests
    {
        private const int TestNumber = 1373;

        private int legendItemsQuantity;
        private string ssiChartDefaultHover;
        private string ssiChartLegendDefaultText;
        private string ssiChartLegendDefaultLabel;
        private List<string> ssiChartAreaColorHex;
        private List<string> ssiChartRgbColor;
        private List<string> ssiChartAreaName;
        private readonly Dictionary<string, string> healthZoneToPercentValue = new Dictionary<string, string>();
        private readonly Dictionary<string, string> healthZoneToColorRgbValue = new Dictionary<string, string>();        

        [TestInitialize]
        public void TestInitialize()
        {
            legendItemsQuantity = GetTestDataAsInt(nameof(legendItemsQuantity));
            ssiChartDefaultHover = GetTestDataAsString(nameof(ssiChartDefaultHover));

            var portfolioModel = new PortfolioDBModel
            {
                Name = StringUtility.RandomString("My Portfolio #########"),
                Type = ((int)PortfolioType.Investment).ToString(),
                CurrencyId = ((int)Currency.USD).ToString()
            };

            var tickersToAdd = GetTestDataValuesAsListByColumnName("ticker");
            ssiChartAreaColorHex = GetTestDataValuesAsListByColumnName(nameof(ssiChartAreaColorHex));
            ssiChartRgbColor = GetTestDataValuesAsListByColumnName(nameof(ssiChartRgbColor));
            ssiChartAreaName = GetTestDataValuesAsListByColumnName(nameof(ssiChartAreaName));

            ssiChartLegendDefaultText = GetTestDataAsString(nameof(ssiChartLegendDefaultText));
            ssiChartLegendDefaultLabel = GetTestDataAsString(nameof(ssiChartLegendDefaultLabel));

            for (int i = 1; i <= legendItemsQuantity; i++)
            {
                healthZoneToPercentValue.Add(GetTestDataAsString($"{nameof(ssiChartAreaName)}{i}"), GetTestDataAsString($"ssiChartAreaColoredHover{i}"));
            }
            for (int i = 1; i <= legendItemsQuantity; i++)
            {
                healthZoneToColorRgbValue.Add(GetTestDataAsString($"{nameof(ssiChartAreaName)}{i}"), GetTestDataAsString($"{nameof(ssiChartRgbColor)}{i}"));
            }

            var positionsModels = new List<PositionsDBModel>();
            foreach (var ticker in tickersToAdd)
            {
                positionsModels.Add(new PositionsDBModel 
                { 
                    Symbol = ticker,
                    TradeType = ((int)PositionTradeTypes.Long).ToString()
                });
            }

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, new List<ProductSubscriptions>
                {
                    ProductSubscriptions.TradeStopsLifetime, ProductSubscriptions.TradeIdeasLifetime, ProductSubscriptions.CryptoStopsLifetime
                }
            ));

            var portfolioId = PortfoliosSetUp.AddPortfolioViaDb(UserModels.First(), portfolioModel);
            foreach (var positionModel in positionsModels)
            {
                PositionsAlertsSetUp.AddPositionViaDB(portfolioId, positionModel);
            }

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
            new DashboardForm().SelectPortfolioStatisticsWidgetPortfolio(portfolioModel.Name);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1373$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("Test for data for health donut chart matched expectations. https://tr.a1qa.com/index.php?/cases/view/19234141")]
        [TestCategory("DashboardPortfolioDistribution"), TestCategory("Dashboard"), TestCategory("HealthGroup")]

        public override void RunTest()
        {
            LogStep(1, "Go to Portfolio Distribution widget. Open the 'HEALTH' tab.");
            var dashboardWidget = new WidgetForm(WidgetTypes.PortfolioDistribution, false);
            dashboardWidget.ClickWidgetContentTab(WidgetPortfolioDistributionTabs.HealthSsi);

            LogStep(2, "Check the donut chart.");
            Checker.CheckEquals(ssiChartDefaultHover, dashboardWidget.WidgetPieChart.GetPieChartHoverDistributionPercentDetailsText(),
                "Unexpected text displayed into the donut chart by default");

            var pieChartSteps = new PieChartElementSteps(dashboardWidget.WidgetPieChart);
            var ssiDonutColors = dashboardWidget.WidgetPieChart.GetPieChartSectionsColors();            
            foreach (var ssiColor in ssiChartAreaColorHex)
            {
                Checker.IsTrue(ssiDonutColors.Contains(ssiColor),
                $"Color {ssiColor} is not shown in Chart Sections:\n {ssiDonutColors.Aggregate(string.Empty, (current, color) => current + "\n" + color)} ");
            }

            LogStep(3, "Check the information to the right of the donut chart.");
            Checker.CheckEquals(ssiChartLegendDefaultText, dashboardWidget.WidgetPieChart.GetPieChartLegendDefaultText(),
                "Chart legend text by default is not as expected");
            Checker.CheckEquals(ssiChartLegendDefaultLabel, dashboardWidget.WidgetPieChart.GetPieChartLegendDefaultLabel(),
                "Chart Legend label by default is not as expected");

            var legendTextValues = dashboardWidget.WidgetPieChart.GetPieChartLegendItemLabels();
            foreach (var ssiAreaName in ssiChartAreaName)
            {
                Checker.IsTrue(legendTextValues.Contains(ssiAreaName),
                $"Health area name {ssiAreaName} is not shown:\n {legendTextValues.Aggregate(string.Empty, (current, name) => current + "\n" + name)} ");
            }

            var ssiLegendItemCirclesColors = pieChartSteps.GetAllPieChartColors();
            foreach (var ssiLegendItemColor in ssiChartRgbColor)
            {
                Checker.IsTrue(ssiLegendItemCirclesColors.Select(x => x.Contains(ssiLegendItemColor)).Any(),
                $"Color {ssiLegendItemColor} is not shown in Legend:\n {ssiLegendItemCirclesColors.Aggregate(string.Empty, (current, color) => current + "\n" + color)} ");
            }

            LogStep(4, 7, "Click the Green/Red/yellow/Grey area on the donut chart.");
            var ssiValuesDictionary = pieChartSteps.ClickAllPieChartLegendItemsGetHoverDistributionPercentValues();
            var pieChartLegendItems = pieChartSteps.GetAllPieChartLegendItems();
            var percentValuesColorDictionary = pieChartSteps.ClickSpecificPieChartLegendItemsGetColorOfPercentValues(pieChartLegendItems);

            foreach (var healthZoneText in legendTextValues)
            {
                if (ssiValuesDictionary.ContainsKey(healthZoneText))
                {
                    Checker.CheckEquals(healthZoneToPercentValue[healthZoneText], ssiValuesDictionary[healthZoneText],
                        $"Percent value for zone  {healthZoneText} is not as expected");
                }
                else
                {
                    Checker.Fail($"Health zone {healthZoneText} is not found in Legend Items");
                }                
            }

            foreach (var healthZoneText in legendTextValues)
            {
                if (percentValuesColorDictionary.ContainsKey(healthZoneText))
                {
                    Checker.IsTrue(percentValuesColorDictionary[healthZoneText].Contains(healthZoneToColorRgbValue[healthZoneText]),
                        $"Color for percent value for zone {healthZoneText} is not as expected");
                }
                else
                {
                    Checker.Fail($"Health zone {healthZoneText} is not found in Legend Colors");
                }
            }
        }
    }
}