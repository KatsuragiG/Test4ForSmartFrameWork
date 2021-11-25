using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.PositionData;
using AutomatedTests.Database.Users;
using AutomatedTests.Enums.Dashboard;
using AutomatedTests.Forms.PortfolioLite;
using AutomatedTests.Models.ChartModels;
using AutomatedTests.Models.PositionsModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.WebDriver;

namespace UnitTests.Tests._10_PortfolioLite
{
    [TestClass]
    public class TC_1389_PortfolioLite_PortfolioPerformance_CheckChangesOnEquityChartIfSelectItemsFromDropdowns : BaseTestUnitTests
    {
        private const int TestNumber = 1389;
        
        private int step;
        private int quantityOfAddedPositions;
        private readonly List<PortfolioLitePositionModel> positionsModels = new List<PortfolioLitePositionModel>();
        private string defaultPortfolioName;
        private string firstCompareValue;
        private string secondCompareValue;
        private Color portfolioPositiveColor;
        private Color portfolioNegativeColor;
        private Color compareColor;
        private string expectedDateFormat;
        private int countOfLines;
        private int monthOffsetForFirstCompareValue;
        private int monthOffsetForSecondCompareValue;
        private DateTime lastTradeDate = DateTime.Now;

        [TestInitialize]
        public void TestInitialize()
        {
            var positionDataQueries = new PositionDataQueries();
            quantityOfAddedPositions = GetTestDataAsInt(nameof(quantityOfAddedPositions));
            for (int i = 1; i <= quantityOfAddedPositions; i++)
            {
                positionsModels.Add(new PortfolioLitePositionModel
                {
                    Ticker = GetTestDataAsString($"symbolsToAdd{i}"),
                    BuyDate = GetTestDataAsString($"entryDate{i}"),
                    Qty = GetTestDataAsString($"quantities{i}"),
                    IsLongType = GetTestDataAsBool($"isLongType{i}")
                });

                var actualLastTradeDate = DateTime.Parse(positionDataQueries.SelectLastTradeDate(positionsModels.Last().Ticker));
                if (lastTradeDate > actualLastTradeDate)
                {
                    lastTradeDate = actualLastTradeDate;
                }
            }

            defaultPortfolioName = GetTestDataAsString(nameof(defaultPortfolioName));
            firstCompareValue = GetTestDataAsString(nameof(firstCompareValue));
            secondCompareValue = GetTestDataAsString(nameof(secondCompareValue));

            portfolioPositiveColor = GetTestDataAsString(nameof(portfolioPositiveColor)).GetColorFromHexString();
            portfolioNegativeColor = GetTestDataAsString(nameof(portfolioNegativeColor)).GetColorFromHexString();
            compareColor = GetTestDataAsString(nameof(compareColor)).GetColorFromHexString();

            expectedDateFormat = GetTestDataAsString(nameof(expectedDateFormat));
            countOfLines = GetTestDataAsInt(nameof(countOfLines));

            monthOffsetForFirstCompareValue = GetTestDataAsInt(nameof(monthOffsetForFirstCompareValue));
            monthOffsetForSecondCompareValue = GetTestDataAsInt(nameof(monthOffsetForSecondCompareValue));

            LogStep(step++, "Preconditions. Create user with subscription to PortfolioLite. Add position");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.PortfolioLite));

            var fakeSnaid = StringUtility.RandomString(Constants.SnaidPattern);
            new UsersQueries().UpdateUserSnaid(fakeSnaid, UserModels.First().TradeSmithUserId);
            new PortfolioLiteNavigation().OpenPortfolioLiteWithUserGuid(UserModels.First());

            var portfolioLiteMainForm = new PortfolioLiteMainForm();
            portfolioLiteMainForm.ClickAddAPosition();
            portfolioLiteMainForm.AddPositions(positionsModels);
            Assert.AreEqual(quantityOfAddedPositions, portfolioLiteMainForm.GetPositionQuantityInGrid(),
                "Grid does not contain added position");
            portfolioLiteMainForm.ClickPortfolioSummary();
            portfolioLiteMainForm.ExitFrame();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1389$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("PortfolioLite")]
        [Description("Test checks changing after selecting an item from the dropdowns on Equity chart https://tr.a1qa.com/index.php?/cases/view/21116202")]
        public override void RunTest()
        {
            LogStep(step++, "Check that equity chart is shown");
            var portfolioLiteMainForm = new PortfolioLiteMainForm();
            var portfolioLiteDetailsForm = new PortfolioLiteDetailsForm();
            var chart = portfolioLiteDetailsForm.Chart;
            chart.AssertIsPresent();

            LogStep(step++, "Make sure that the current portfolio has name Portfolio Lite and compared to 'S&P 500' by default");
            Checker.CheckEquals(defaultPortfolioName, portfolioLiteDetailsForm.GetPortfolioNameFromEquityChart(),
                $"{defaultPortfolioName} is not shown in description");
            Checker.CheckEquals(firstCompareValue, portfolioLiteDetailsForm.GetSelectedValueFromEquityDropDown(EquityPerformanceDropDownTypes.ComparePortfolios),
                $"{firstCompareValue} is not selected by default");

            LogStep(step++, "Select 'LastTradeDate - 1 month' from 'FromDate' dropdown");
            var selectedDateAsString = lastTradeDate.AddMonths(monthOffsetForFirstCompareValue).AsShortDate();
            portfolioLiteDetailsForm.SelectEntryDateByDatepicker(EquityPerformanceDropDownTypes.FromDate, selectedDateAsString);

            LogStep(step++, "Check that equity chart is shown");
            chart.AssertIsPresent();

            LogStep(step++, "Check that 2 lines are displayed on the graph according to legend");
            var chartLegendLines = chart.GetChartGraphLines();
            var lineNames = chartLegendLines.Select(e => e.LineName).ToList();
            Checker.CheckEquals(countOfLines, chartLegendLines.Count,
                $"Count of high chart legend lines is not as expected. Actual name of lines: {string.Join(", ", lineNames)}");
            Checker.IsTrue(lineNames.Contains(defaultPortfolioName), $"Legend block doesn't contain the '{defaultPortfolioName}' line");
            Checker.IsTrue(lineNames.Contains(firstCompareValue), $"Legend block doesn't contain the '{firstCompareValue}' line");

            var firstChartModel = CheckChartModelAndGetIt(step++, firstCompareValue);

            LogStep(step++, "Select another index from to dropdown");
            portfolioLiteDetailsForm.SelectValueInEquityDropDown(EquityPerformanceDropDownTypes.ComparePortfolios, secondCompareValue);
            Checker.CheckEquals(secondCompareValue, portfolioLiteDetailsForm.GetSelectedValueFromEquityDropDown(EquityPerformanceDropDownTypes.ComparePortfolios),
                $"{secondCompareValue} is not selected by default");

            LogStep(step++, "Select values from 'FromDate' and 'ToDateDate' dropdowns");
            selectedDateAsString = lastTradeDate.AddMonths(monthOffsetForSecondCompareValue).AsShortDate();
            portfolioLiteDetailsForm.SelectEntryDateByDatepicker(EquityPerformanceDropDownTypes.FromDate, selectedDateAsString);
            selectedDateAsString = lastTradeDate.AddMonths(monthOffsetForFirstCompareValue).AsShortDate();
            portfolioLiteDetailsForm.SelectEntryDateByDatepicker(EquityPerformanceDropDownTypes.ToDate, selectedDateAsString);
            Checker.CheckEquals(defaultPortfolioName, portfolioLiteDetailsForm.GetPortfolioNameFromEquityChart(),
                $"{defaultPortfolioName} is not shown in description");
            chart.AssertIsPresent();

            LogStep(step, "Repeat steps 6-7");
            var secondChartModel = CheckChartModelAndGetIt(step++, secondCompareValue);

            Log.LogStep(step++, "Check that chart model is not same as for step 7");
            Checker.CheckNotEquals(secondChartModel, firstChartModel,
                $"[{firstCompareValue}] chart model equals to [{secondCompareValue}] chart model");
        }

        private ChartModel CheckChartModelAndGetIt(int stepNumber, string indexName)
        {
            LogStep(stepNumber, "Remember tooltip structure and colors for all available points");
            var сhartModel = new PortfolioLiteDetailsForm().Chart.GetChartModel();
            Checker.IsTrue(сhartModel.Positions.Any(), $"[{firstCompareValue}] chart model is empty for first index");

            CheckChartModelByColors(stepNumber++, indexName, сhartModel, defaultPortfolioName);

            return сhartModel;
        }

        private void CheckChartModelByColors(int stepNumber, string indexName, ChartModel chartModel, string lineName)
        {
            LogStep(stepNumber, "Check values for red, green and purple colors");
            var positionLinesInToolTipsQuantity = chartModel.Positions.Select(t => t.Value.Count).Distinct().ToList();
            Checker.IsTrue(positionLinesInToolTipsQuantity.Contains(countOfLines), "Chart contains unexpected lines quantity from tooltip");

            foreach (var position in chartModel.Positions)
            {
                foreach (var element in position.Value)
                {
                    Checker.IsTrue(Constants.DateFormatMonDDcommaYyyyRegex.IsMatch(element.Date),
                        $"Index [{position.Key}]: {lineName}: {indexName}: Date format [{expectedDateFormat}] is not as expected for date [{element.Date}]");
                    Checker.IsTrue(element.IsVisibleInChart, $"Index [{position.Key}]: {lineName}: {indexName}: Point was not visible");
                    Checker.IsFalse(string.IsNullOrEmpty(element.Value),
                        $"Index [{position.Key}]: {lineName}: {indexName}: Value '{element.Value}' is empty or null");

                    CheckLinesColorAndValues(lineName, indexName, position.Key, element);
                }
            }
        }

        private void CheckLinesColorAndValues(string lineName, string indexName, int lineOrder, ChartPositionLineModel element)
        {
            if (element.LineName.Contains(lineName) && !string.IsNullOrEmpty(element.Value))
            {
                try
                {
                    var percentValue = Parsing.ConvertToDouble(Constants.DecimalNumberWithIntegerPossibilityRegex.Match(element.Value).ToString());

                    CheckLineDependsOnSign(lineName, lineOrder, element, percentValue);
                }
                catch (ArgumentNullException)
                {
                    Logger.Instance.Error($"Index [{lineOrder}]: {lineName}: No appropriate value: {element.Value}");
                }
            }
            else if (element.LineName.Contains(indexName))
            {
                Checker.CheckEquals(compareColor, element.CircleColor,
                    $"Index [{lineOrder}]: {lineName}: Circle color is not as expected for {indexName}");
            }
            else
            {
                Logger.Instance.Error($"Unexpected line is shown: {element.LineName}");
            }
        }

        private void CheckLineDependsOnSign(string lineName, int lineOrder, ChartPositionLineModel element, double percentValue)
        {
            if (percentValue < 0)
            {
                Checker.CheckEquals(portfolioNegativeColor, element.CircleColor,
                    $"Index [{lineOrder}]: {lineName}: Circle negative color is not as expected");
                Checker.CheckEquals(portfolioNegativeColor, element.BorderColor,
                    $"Index [{lineOrder}]: {lineName}: Border negative color is not as expected");
                Checker.IsTrue(element.Value.Contains(Constants.MinusSign),
                    $"Index [{lineOrder}]: {lineName}: Value '{element.Value}' should be negative");
            }
            else
            {
                Checker.CheckEquals(portfolioPositiveColor, element.CircleColor,
                    $"Index [{lineOrder}]: {lineName}: Circle positive color is not as expected");
                Checker.CheckEquals(portfolioPositiveColor, element.BorderColor,
                    $"Index [{lineOrder}]: {lineName}: Border positive color is not as expected");
                Checker.IsFalse(element.Value.Contains(Constants.MinusSign),
                    $"Index [{lineOrder}]: {lineName}: Value '{element.Value}' should be positive");
            }
        }
    }
}