using System;
using System.Collections.Generic;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;
using System.Linq;
using TradeStops.Common.Utils;
using TradeStops.Common.Extensions;

namespace UnitTests.Tests._04_PositionsAndAlerts._01_Positions
{
    [TestClass]
    public class TC_0182_ClosedPositionsGrid_FiltrationUsingADropDownListPeriod : BaseTestUnitTests
    {
        private const int TestNumber = 182;
        private const int DaysQuantityToCompensateLast7DaysPositions = 9;
        private const int DaysQuantityToCompensateCurrentPreviousMonthPositions = 7;
        private const int DaysQuantityToCompensateYesterdayPositions = 3;
        private const string DateForYearCompensation = "12/31";

        private PortfolioModel portfolioModel;
        private readonly List<GridFilterPeriods> gridFilterPeriodList = new List<GridFilterPeriods>();
        private readonly List<int> positionsQuantityByGridFilterPeriodItem = new List<int> { 1, 1, 3, 4, 1, 6, 8, 7, 2, 10, 1 };

        [TestInitialize]
        public void TestInitialize()
        {
            portfolioModel = new PortfolioModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency")
            };
            var entryDate = GetTestDataAsString("EntryDate");
            var closedStatus = GetTestDataAsString("Closed");
            var currentDay = DateTime.Now.Day;
            var currentMonth = DateTime.Now.Month;
            var yearCompensateValue = 3;

            if (currentDay < DaysQuantityToCompensateLast7DaysPositions)
            {
                positionsQuantityByGridFilterPeriodItem[2]++;
            }
            if (currentDay < DaysQuantityToCompensateCurrentPreviousMonthPositions)
            {
                positionsQuantityByGridFilterPeriodItem[3]--;
                positionsQuantityByGridFilterPeriodItem[4]++;
                yearCompensateValue = 4;
            }
            if (currentDay < DaysQuantityToCompensateYesterdayPositions)
            {
                positionsQuantityByGridFilterPeriodItem[1]++;
            }
            if ($"{currentMonth}/{currentDay}".Equals(DateForYearCompensation))
            {
                positionsQuantityByGridFilterPeriodItem[7]++;
                positionsQuantityByGridFilterPeriodItem[8]--;
            }
            if (currentMonth == 1)
            {
                positionsQuantityByGridFilterPeriodItem[7] = positionsQuantityByGridFilterPeriodItem[7] - yearCompensateValue;
                positionsQuantityByGridFilterPeriodItem[8] = positionsQuantityByGridFilterPeriodItem[8] + yearCompensateValue;
            }
            else if (currentMonth < 4)
            {
                positionsQuantityByGridFilterPeriodItem[7] = positionsQuantityByGridFilterPeriodItem[7] - DaysQuantityToCompensateYesterdayPositions + 1;
                positionsQuantityByGridFilterPeriodItem[8] = positionsQuantityByGridFilterPeriodItem[8] + DaysQuantityToCompensateYesterdayPositions - 1;
            }
            else if (currentMonth.In(4, 5, 6, 7, 8, 9))
            {
                positionsQuantityByGridFilterPeriodItem[7] = positionsQuantityByGridFilterPeriodItem[7] - DaysQuantityToCompensateYesterdayPositions + 2;
                positionsQuantityByGridFilterPeriodItem[8] = positionsQuantityByGridFilterPeriodItem[8] + DaysQuantityToCompensateYesterdayPositions - 2;
            }
            if (currentDay == 1)
            {
                positionsQuantityByGridFilterPeriodItem[0]++;
                positionsQuantityByGridFilterPeriodItem[1]--;
                positionsQuantityByGridFilterPeriodItem[3]--;
                positionsQuantityByGridFilterPeriodItem[4]++;
            }

            var positionsModels = new List<PositionsDBModel>
            {
                new PositionsDBModel
                {
                    Symbol = GetTestDataAsString("SymbolGold"),
                    PurchaseDate = entryDate,
                    CloseDate = DateTimeProvider.GetDate(DateTime.Now).AsShortDate(),
                    Notes = GridFilterPeriods.Today.GetStringMapping(),
                    StatusType = closedStatus
                },
                new PositionsDBModel
                {
                    Symbol = GetTestDataAsString("SymbolE"),
                    PurchaseDate = entryDate,
                    CloseDate = DateTimeProvider.GetDate(DateTime.Now, -1).AsShortDate(),
                    Notes = GridFilterPeriods.Yesterday.GetStringMapping(),
                    StatusType = closedStatus
                },
                new PositionsDBModel
                {
                    Symbol = GetTestDataAsString("SymbolFB"),
                    PurchaseDate = entryDate,
                    CloseDate = DateTimeProvider.GetDate(DateTime.Now, -6).AsShortDate(),
                    Notes = GridFilterPeriods.Last7Days.GetStringMapping(),
                    StatusType = closedStatus
                },
                new PositionsDBModel
                {
                    Symbol = GetTestDataAsString("SymbolAAPL"),
                    PurchaseDate = entryDate,
                    CloseDate = $"{DateTime.Now.Month}/01/{DateTime.Now.Year}",
                    Notes = GridFilterPeriods.CurrentMonth.GetStringMapping(),
                    StatusType = closedStatus
                },
                new PositionsDBModel
                {
                    Symbol = GetTestDataAsString("SymbolBTC"),
                    PurchaseDate = entryDate,
                    CloseDate = $"{DateTime.Now.AddMonths(-1).Month}/01/{(currentMonth == 1 ? DateTime.Now.AddYears(-1).Year : DateTime.Now.Year)}",
                    Notes = GridFilterPeriods.PreviousMonth.GetStringMapping(),
                    StatusType = closedStatus
                },
                new PositionsDBModel
                {
                    Symbol = GetTestDataAsString("SymbolMSFT"),
                    PurchaseDate = entryDate,
                    CloseDate = $"{DateTime.Now.AddMonths(-3).Month}/01/{(currentMonth < 4 ? DateTime.Now.AddYears(-1).Year : DateTime.Now.Year)}",
                    Notes = GridFilterPeriods.Last6Months.GetStringMapping(),
                    StatusType = closedStatus
                },
                new PositionsDBModel
                {
                    Symbol = GetTestDataAsString("SymbolS"),
                    PurchaseDate = entryDate,
                    CloseDate = $"{DateTime.Now.AddMonths(-9).Month}/01/{(currentMonth < 10 ? DateTime.Now.AddYears(-1).Year : DateTime.Now.Year)}",
                    Notes = GridFilterPeriods.Last12Months.GetStringMapping(),
                    StatusType = closedStatus
                },
                new PositionsDBModel
                {
                    Symbol = GetTestDataAsString("SymbolBrent"),
                    PurchaseDate = entryDate,
                    CloseDate = DateTimeProvider.GetDate(DateTime.Now, 1, 0, -1).AsShortDate(),
                    Notes = GridFilterPeriods.YearToDate.GetStringMapping(),
                    StatusType = closedStatus
                },
                new PositionsDBModel
                {
                    Symbol = GetTestDataAsString("SymbolFv"),
                    PurchaseDate = entryDate,
                    CloseDate = $"01/01/{DateTime.Now.AddYears(-1).Year}",
                    Notes = GridFilterPeriods.LastYear.GetStringMapping(),
                    StatusType = closedStatus
                },
                new PositionsDBModel
                {
                    Symbol = GetTestDataAsString("SymbolBTCETH"),
                    PurchaseDate = entryDate,
                    CloseDate = $"01/01/{DateTime.Now.AddYears(-2).Year}",
                    Notes = GridFilterPeriods.LastYear.GetStringMapping(),
                    StatusType = closedStatus
                }
            };
            gridFilterPeriodList.AddRange(EnumUtils.GetValues<GridFilterPeriods>());

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPro));
            var portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            foreach (var positionModel in positionsModels)
            {
                PositionsAlertsSetUp.AddPositionViaDB(portfolioId, positionModel);
            }

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);
            new MainMenuNavigation().OpenPositionsGrid(PositionsTabs.ClosedPositions);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_182$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("ClosedPositionsGrid")]
        [Description("The test checks correctness of filtration using drop-down list of time period for Closed Position grid https://tr.a1qa.com/index.php?/cases/view/19232834")]
        public override void RunTest()
        {
            LogStep(1, "Select portfolio in dropdown (see precondition #2)");
            new PositionsAlertsStatisticsPanelForm().SelectPortfolio(portfolioModel.Name);

            LogStep(2, 4, "Select next period in dropdown near Period text");
            var closedPositionTabForm = new ClosedPositionsTabForm();
            for (int i = 0; i < gridFilterPeriodList.Count; i++)
            {
                closedPositionTabForm.SelectPeriod(gridFilterPeriodList[i]);
                if (gridFilterPeriodList[i] == GridFilterPeriods.CustomDateRange)
                {
                    closedPositionTabForm.SetCustomDateRangeFilter(CustomDateRangeElementTypes.From, $"01/01/{DateTime.Now.AddYears(-3).Year}");
                }
                Checker.CheckEquals(positionsQuantityByGridFilterPeriodItem[i], closedPositionTabForm.GetPositionsIds().Count, 
                    $"{gridFilterPeriodList[i].GetStringMapping()}: Number of positions is not equal as expected");
            }

            LogStep(5, "Check To Custom Date Range Filtration");
            closedPositionTabForm.SetCustomDateRangeFilter(CustomDateRangeElementTypes.To, $"02/02/{DateTime.Now.AddYears(-2).Year}");
            Checker.CheckEquals(positionsQuantityByGridFilterPeriodItem.Last(), closedPositionTabForm.GetPositionsIds().Count, 
                $"{gridFilterPeriodList.Last().GetStringMapping()}: Number of positions is not equal as expected at To filtration");
        }
    }
}