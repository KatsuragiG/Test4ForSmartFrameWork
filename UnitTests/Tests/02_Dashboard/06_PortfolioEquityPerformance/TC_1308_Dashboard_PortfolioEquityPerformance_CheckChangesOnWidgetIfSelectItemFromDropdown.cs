using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.PositionData;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Dashboard;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Popups;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Models.ChartModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._02_Dashboard._06_PortfolioEquityPerformance
{
    [TestClass]
    public class TC_1308_Dashboard_PortfolioEquityPerformance_CheckChangesOnWidgetIfSelectItemFromDropdown : BaseTestUnitTests
    {
        private const int TestNumber = 1308;
        private const WidgetTypes Widget = WidgetTypes.PortfolioEquityPerformance;

        private PortfolioModel portfolioModel;
        private string firstCompareValue;
        private string secondCompareValue;
        private string portfolioPositiveColor;
        private string portfolioPositiveColorName;
        private string portfolioNegativeColor;
        private string portfolioNegativeColorName;
        private string compareColor;
        private string compareColorName;
        private string expectedDateFormat;
        private int countOfLines;
        private int monthOffsetForFirstCompareValue;
        private int monthOffsetForSecondCompareValue;
        private readonly List<DateTime> lastTradeDates = new List<DateTime>();

        [TestInitialize]
        public void TestInitialize()
        {
            firstCompareValue = GetTestDataAsString(nameof(firstCompareValue));
            secondCompareValue = GetTestDataAsString(nameof(secondCompareValue));

            portfolioPositiveColor = GetTestDataAsString(nameof(portfolioPositiveColor));
            portfolioPositiveColorName = GetTestDataAsString(nameof(portfolioPositiveColorName));

            portfolioNegativeColor = GetTestDataAsString(nameof(portfolioNegativeColor));
            portfolioNegativeColorName = GetTestDataAsString(nameof(portfolioNegativeColorName));

            compareColor = GetTestDataAsString(nameof(compareColor));
            compareColorName = GetTestDataAsString(nameof(compareColorName));

            expectedDateFormat = GetTestDataAsString(nameof(expectedDateFormat));
            countOfLines = GetTestDataAsInt(nameof(countOfLines));

            monthOffsetForFirstCompareValue = GetTestDataAsInt(nameof(monthOffsetForFirstCompareValue));
            monthOffsetForSecondCompareValue = GetTestDataAsInt(nameof(monthOffsetForSecondCompareValue));

            portfolioModel = new PortfolioModel
            {
                Name = "CheckChangesOnWidgetIfSelectItemFromDropdown",
                Type = PortfolioType.Investment,
                Currency = Currency.USD.GetStringMapping()
            };
            var entryDate = GetTestDataAsString("entryDate");
            var shares = GetTestDataAsString("shares");
            var positionsModels = new List<PositionsDBModel>
            {
                new PositionsDBModel
                {
                    Symbol = GetTestDataAsString("ticker1"),
                    PurchaseDate = entryDate,
                    PurchasePrice = GetTestDataAsString("entryPrice1"),
                    Shares = shares
                },
                new PositionsDBModel
                {
                    Symbol = GetTestDataAsString("ticker2"),
                    PurchaseDate = entryDate,
                    PurchasePrice = GetTestDataAsString("entryPrice2"),
                    Shares = shares
                }
            };

            LogStep(0, "Preconditions: Login. Create the portfolio. Open Dashboard page");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPlatinum));

            var portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            PositionsAlertsSetUp.AddPositionsViaDB(portfolioId, positionsModels);

            LoginSetUp.LogIn(UserModels.First());
            foreach (var positionModel in positionsModels)
            {
                var actualLastTradeDate = DateTime.Parse(new PositionDataQueries().SelectLastTradeDate(positionModel.Symbol));
                lastTradeDates.Add(actualLastTradeDate > DateTime.Now ? DateTime.Now : actualLastTradeDate);
            }

            var mainMenuForm = new MainMenuForm();
            mainMenuForm.ClickMenuItem(MainMenuItems.MyPortfolios);
            new MainMenuNavigation().OpenPositionsGrid();

            var positionsTabForm = new PositionsTabForm();
            positionsTabForm.SelectAllItemsInGrid();
            positionsTabForm.ClickGroupActionButton(PositionsGroupAction.BulkEdit);
            new BulkEditPositionFrame().ClickSaveButton();

            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1308$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Dashboard"), TestCategory("DashboardPortfolioEquityPerformance"), TestCategory("Chart")]
        [Description("Test for widget elements changing after selecting an item from the dropdowns: https://tr.a1qa.com/index.php?/cases/view/19234307")]
        public override void RunTest()
        {
            LogStep(1, "Go to Portfolio Equity Performance (% change)");
            var dashboardWidget = new WidgetForm(Widget);
            dashboardWidget.AssertIsOpen();

            Checker.CheckEquals(firstCompareValue, dashboardWidget.GetSelectedValueFromWidgetDropDown(EquityPerformanceDropDownTypes.ComparePortfolios), 
                    $"{firstCompareValue} is not selected by default");

            var firstDashboardWidgetChartModel = Step2To9(firstCompareValue, lastTradeDates[0], monthOffsetForFirstCompareValue);

            Log.LogStep(10, $"Repeat steps 2-9 for '{secondCompareValue}'");
            var secondDashboardWidgetChartModel = Step2To9(secondCompareValue, lastTradeDates[1], monthOffsetForSecondCompareValue);

            Log.LogStep(11, "Check the following values");
            Checker.CheckNotEquals(secondDashboardWidgetChartModel, firstDashboardWidgetChartModel, 
                $"[{firstCompareValue}] chart model equals to [{secondCompareValue}] chart model");
        }

        private ChartModel Step2To9(string compareValue, DateTime lastTradeDate, int dateOffset)
        {
            LogStep(2, $"[{compareValue}] compare value: Select '{compareValue}' from 'Compare' dropdown");
            var dashboardWidget = new WidgetForm(Widget);
            dashboardWidget.SelectValueInWidgetDropDownInTreeSelectByText(EquityPerformanceDropDownTypes.ComparePortfolios, compareValue);

            LogStep(3, $"[{compareValue}] compare value: Select 'lastTradeDate' with offset [{dateOffset}] from 'FromDate' dropdown");
            dashboardWidget.SelectEntryDateByDatepicker(EquityPerformanceDropDownTypes.FromDate, lastTradeDate.AddMonths(dateOffset).AsShortDate());

            LogStep(4, $"[{compareValue}] compare value: Check that graph is presented");
            var highChart = dashboardWidget.HighChart;
            highChart.AssertIsPresent();

            LogStep(5, $"[{compareValue}] compare value: Check that {countOfLines} lines are displayed on the graph");
            var highChartLegendLines = highChart.GetChartGraphLines();
            var lineNames = highChartLegendLines.Select(e => e.LineName).ToList();
            Checker.CheckEquals(countOfLines, highChartLegendLines.Count, 
                $"Count of high chart legend lines is not as expected. Actual name of lines: {string.Join(", ", lineNames)}");
            Checker.IsTrue(lineNames.Contains(portfolioModel.Name), $"Legend block doesn't contain the '{portfolioModel.Name}' line");
            Checker.IsTrue(lineNames.Contains(compareValue), $"Legend block doesn't contain the '{compareValue}' line");

            LogStep(6, $"[{compareValue}] compare value: Remember the values");
            var dashboardHighChartModel = highChart.GetChartModel();
            Checker.IsTrue(dashboardHighChartModel.Positions.Any(), $"[{compareValue}] chart model is empty");

            CheckDashboardHighChartModel(7, compareValue, dashboardHighChartModel, portfolioModel.Name, portfolioNegativeColor, portfolioNegativeColorName);
            CheckDashboardHighChartModel(8, compareValue, dashboardHighChartModel, portfolioModel.Name, portfolioPositiveColor, portfolioPositiveColorName);
            CheckDashboardHighChartModel(9, compareValue, dashboardHighChartModel, compareValue, compareColor, compareColorName);

            return dashboardHighChartModel;
        }

        private void CheckDashboardHighChartModel(int step, string compareValue, ChartModel dashboardHighChartModel, string lineName, string expectedColor, string colorName)
        {
            LogStep(step, $"[{compareValue}] compare value: Check values for {colorName} color");
            var color = expectedColor.GetColorFromHexString();
            foreach (var position in dashboardHighChartModel.Positions)
            {
                foreach (var element in position.Value)
                {
                    if (color.Equals(element.CircleColor))
                    {
                        Checker.CheckEquals(lineName, element.LineName, 
                            $"Index [{position.Key}]: {lineName}: {colorName}: Line name is not as expected");
                        Checker.CheckEquals(color, element.CircleColor, 
                            $"Index [{position.Key}]: {lineName}: {colorName}: Circle color is not as expected");
                        Checker.IsTrue(element.IsVisibleInChart, $"Index [{position.Key}]: {lineName}: {colorName}: Point was not visible");
                        Checker.IsTrue( CheckThatDateMatchesFormat(element.Date),
                            $"Index [{position.Key}]: {lineName}: {colorName}: Date format [{expectedDateFormat}] is not as expected for date [{element.Date}]");
                        Checker.IsFalse(string.IsNullOrEmpty(element.Value),
                            $"Index [{position.Key}]: {lineName}: {colorName}: Value '{element.Value}' is empty or null");

                        if (color.Equals(portfolioNegativeColor.GetColorFromHexString()))
                        {
                            Checker.IsTrue(element.Value.Contains(Constants.MinusSign), $"Index [{position.Key}]: {lineName}: {colorName}: Value {element.Value} is positive");
                        }
                        else if (color.Equals(portfolioPositiveColor.GetColorFromHexString()))
                        {
                            Checker.IsFalse(element.Value.Contains(Constants.MinusSign), $"Index [{position.Key}]: {lineName}: {colorName}: Value '{element.Value}' is negative");
                        }
                        else
                        {
                            continue;
                        }

                        Checker.CheckEquals(color, element.BorderColor,  
                            $"Index [{position.Key}]: {lineName}: {colorName}: Border color is not as expected");
                    }
                }
            }
        }

        private bool CheckThatDateMatchesFormat(string expDate)
        {
            try
            {
                DateTime.ParseExact(expDate, expectedDateFormat, CultureInfo.InvariantCulture);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}