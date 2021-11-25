using System;
using System.Collections.Generic;
using System.Linq;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Chart;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.PositionCard;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.User;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Charts;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using TradeStops.Common.Extensions;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._05_PositionCard
{
    [TestClass]
    public class TC_1180_PositionCard_ChartSettings_TurningOnOffHealthLinesForPositions : BaseTestUnitTests
    {
        private const int TestNumber = 1180;
        
        private PortfolioDBModel portfolioModel;
        private readonly List<PositionsDBModel> positionsModels = new List<PositionsDBModel>();
        private bool entryDateIsNull;
        private bool entryPriceIsNull;
        private bool sharesIsNull;
        private string ssiYellowZone;
        private string ssiStopLoss;
        private string ssiTrend;
        private ChartPeriod chartPeriod;
        private readonly List<int> positionsIds = new List<int>();

        [TestInitialize]
        public void TestInitialize()
        {
            var userType = GetTestDataParsedAsEnumFromStringMapping<ProductSubscriptionTypes>("User");
            var entryDate = GetTestDataAsString("EntryDate");
            var isSynch = GetTestDataAsBool("IsSynch");
            var tradeType = $"{(int)GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>("TradeType")}";
            var symbol = GetTestDataAsString("Symbol");
            portfolioModel = new PortfolioDBModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = $"{(int)GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType")}",
                CurrencyId = $"{(int)GetTestDataParsedAsEnumFromStringMapping<Currency>("Currency")}",
                VendorPortfolioId = isSynch ? StringUtility.RandomString("###") : null
            };
            positionsModels.Add(new PositionsDBModel
            {
                Symbol = symbol,
                TradeType = tradeType,
                VendorHoldingId = isSynch ? StringUtility.RandomString("###") : null,
                PurchaseDate = entryDate.Equals("") ? null : entryDate
            });
            positionsModels.Add(new PositionsDBModel
            {
                Symbol = symbol,
                TradeType = tradeType,
                PurchaseDate = entryDate.Equals("") ? null : entryDate,
                StatusType = $"{(int)AutotestPositionStatusTypes.Close}",
                CloseDate = DateTime.Now.ToShortDateString()
            });
            entryDateIsNull = GetTestDataAsBool(nameof(entryDateIsNull));
            entryPriceIsNull = GetTestDataAsBool(nameof(entryPriceIsNull));
            sharesIsNull = GetTestDataAsBool(nameof(sharesIsNull));
            ssiYellowZone = GetTestDataAsString(nameof(ssiYellowZone));
            ssiStopLoss = GetTestDataAsString(nameof(ssiStopLoss));
            ssiTrend = GetTestDataAsString(nameof(ssiTrend));
            chartPeriod = GetTestDataParsedAsEnumFromStringMapping<ChartPeriod>(nameof(chartPeriod));

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, (ProductSubscriptions)(int)userType));
            var portfolioId = PortfoliosSetUp.AddPortfolioViaDb(UserModels.First(), portfolioModel);
            foreach (var positionModel in positionsModels)
            {
                positionsIds.Add(PositionsAlertsSetUp.AddPositionViaDB(portfolioId, positionModel));
            }

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1180$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PositionCard"), TestCategory("Chart"), TestCategory("PositionCardChartSettingsTab")]
        [Description("The test checks line become visible on the chart after checking appropriate checkboxes. https://tr.a1qa.com/index.php?/cases/view/19231935")]
        public override void RunTest()
        {
            foreach (var positionId in positionsIds)
            {
                LogStep(1, "Open Position Card -> 'Chart Settings'  via direct link for position ");
                MakePositionUnrecognized(positionId);
                new MainMenuNavigation().OpenPositionCard(positionId);

                CheckChartLineAndTooltip(ChartLineTypes.YellowZone, ssiYellowZone);
                CheckChartLineAndTooltip(ChartLineTypes.StopLoss, ssiStopLoss);
                CheckChartLineAndTooltip(ChartLineTypes.HealthTrend, ssiTrend);
            }
        }

        private void CheckChartLineAndTooltip(ChartLineTypes chartLine, string checkboxText)
        {
            LogStep(2, $"Check checkbox {checkboxText}");
            var positionCardForm = new PositionCardForm();
            var chartSettingsTabPositionCardForm = positionCardForm.ActivateTabGetForm<ChartSettingsTabForm>(PositionCardTabs.ChartSettings);
            chartSettingsTabPositionCardForm.OpenChartSettings();
            positionCardForm.Chart.SelectChartPeriod(chartPeriod);
            positionCardForm.Chart.SetCheckboxInState(chartLine, true);
            positionCardForm.Chart.SetCheckboxInState(ChartLineTypes.EntryPriceAdj, false);
            if (new PositionsQueries().SelectPositionStatusTypeByPositionId(positionCardForm.GetPositionIdFromUrl()) == ((int)AutotestPositionStatusTypes.Close).ToString())
            {
                positionCardForm.Chart.SetCheckboxInState(ChartLineTypes.ExitPrice, false);
            }

            LogStep(3, $"Hover cursor on chart. Make sure line is present on the chart and tooltip {checkboxText}");
            if (chartLine.In(ChartLineTypes.YellowZone, ChartLineTypes.StopLoss))
            {
                Checker.IsTrue(positionCardForm.Chart.IsInitBellPresent(chartLine), $"Init Bell for '{checkboxText}' is not present");
                Checker.IsTrue(positionCardForm.Chart.IsTriggeredBellPresent(chartLine), $"Triggered bell for '{checkboxText}' is not present");
            }
            if (chartLine.In(ChartLineTypes.StopLoss))
            {
                Checker.IsTrue(positionCardForm.Chart.IsCurrentHighBellPresent(chartLine), $"Current High for '{checkboxText}' is not present");
            }
            Checker.IsTrue(positionCardForm.Chart.IsChartLinePresent(chartLine), $"Chart for '{checkboxText}' is not present after checking {checkboxText}");
            if (positionCardForm.Chart.MoveCursorToChartGetTooltipWithExpectedLine(chartLine).ChartTooltipDataTypeToText.TryGetValue(chartLine, out string value))
            {
                Checker.IsTrue(value.Contains(checkboxText), $"Tooltip on chart does not contain text '{checkboxText}' after checking {checkboxText}");
            }
            else
            {
                Checker.Fail($"Tooltip on chart does not contain text '{checkboxText}' after checking {checkboxText} (value not found)");
            }

            LogStep(4, $"UnCheck checkbox {checkboxText}");
            positionCardForm.Chart.SetCheckboxInState(chartLine, false);

            LogStep(5, $"Make sure line is NOT present on the chart {checkboxText}");
            Checker.IsFalse(positionCardForm.Chart.IsChartLinePresent(chartLine), $"Chart for '{checkboxText}' is present after unchecking {checkboxText}");
        }

        private void MakePositionUnrecognized(int positionId)
        {
            if (entryDateIsNull)
            {
                new PositionsQueries().MakeEntryDateNull(positionId);
            }
            if (entryPriceIsNull)
            {
                new PositionsQueries().MakeEntryPriceNull(positionId);
            }
            if (sharesIsNull)
            {
                new PositionsQueries().MakeSharesNull(positionId);
            }
        }
    }
}