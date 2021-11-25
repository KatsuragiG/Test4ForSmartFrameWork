using AutomatedTests.Database.Portfolios;
using AutomatedTests.Enums;
using AutomatedTests.Enums.FilterEnums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.PositionCard;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.Tools;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.Screener;
using AutomatedTests.Models.FiltersModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Screener;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._07_Invest._04_Screener
{
    [TestClass]
    public class TC_1403_Screener_ResultsDataForNumericFiltersMatchesPositionsGrid : BaseTestUnitTests
    {
        private const int TestNumber = 1403;

        private NumericFilterModel expectedNumericFilterModel = new NumericFilterModel();
        private bool isRequiredCheckInScreenerGrid;
        private bool isRequiredCheckInPositionGrid;
        private bool isRequiredCheckOnGeneralStatistic;

        private ScreenerFilterType filterName;
        private ScreenerColumnTypes columnScreenerGrid;
        private PositionsGridDataField columnPositionGrid;
        private GeneralStatisticsFieldTypes fieldStatisticTab;
        private AddToPortfolioSelectType addToPortfolioKind;
        private string portfolioNameToAdd;
        private string viewName;
        private string textAfterSaving;

        private readonly MainMenuNavigation mainMenuNavigation = new MainMenuNavigation();

        [TestInitialize]
        public void TestInitialize()
        {
            var userProductSubscriptions = GetUserProductSubscriptions("userSubscription");

            var portfolioModel = new PortfolioDBModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString(nameof(portfolioNameToAdd))),
                Type = $"{(int)GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType")}",
                CurrencyId = $"{(int)GetTestDataParsedAsEnumFromStringMapping<Currency>("Currency")}"
            };

            isRequiredCheckInScreenerGrid = GetTestDataAsBool(nameof(isRequiredCheckInScreenerGrid));
            isRequiredCheckInPositionGrid = GetTestDataAsBool(nameof(isRequiredCheckInPositionGrid));
            isRequiredCheckOnGeneralStatistic = GetTestDataAsBool(nameof(isRequiredCheckOnGeneralStatistic));
            columnScreenerGrid = GetTestDataParsedAsEnumFromDescription<ScreenerColumnTypes>(nameof(columnScreenerGrid));
            columnPositionGrid = GetTestDataParsedAsEnumFromStringMapping<PositionsGridDataField>(nameof(columnPositionGrid));
            fieldStatisticTab = GetTestDataParsedAsEnumFromStringMapping<GeneralStatisticsFieldTypes>(nameof(fieldStatisticTab));

            addToPortfolioKind = GetTestDataParsedAsEnumFromStringMapping<AddToPortfolioSelectType>(nameof(addToPortfolioKind));
            portfolioNameToAdd = addToPortfolioKind == AddToPortfolioSelectType.New
                ? StringUtility.RandomString(GetTestDataAsString(nameof(portfolioNameToAdd)))
                : portfolioModel.Name;
            textAfterSaving = GetTestDataAsString(nameof(textAfterSaving));

            filterName = GetTestDataParsedAsEnumFromStringMapping<ScreenerFilterType>(nameof(filterName));
            expectedNumericFilterModel = FillDataForNumericFilter(filterName, "Filter");

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));
            PortfoliosSetUp.AddPortfolioViaDb(UserModels.First(), portfolioModel);

            LoginSetUp.LogIn(UserModels.First());
            viewName = ViewSetups.AddNewCustomViewForPositionsTabWithAllColumns();
            mainMenuNavigation.OpenScreenerFilters();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1403$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test checks data correctness in grids after filtration by numeric filters in the screener https://tr.a1qa.com/index.php?/cases/view/22387571")]
        [TestCategory("PositionCardStatistics"), TestCategory("PositionsGrid"), TestCategory("Screener")]
        public override void RunTest()
        {
            LogStep(1, "Click Manage filters. Enable only required filters (only filter from test data). Close Manage filters panel.");
            var screenerSteps = new ScreenerSteps();
            screenerSteps.MakeOnlyRequiredFiltersActive(new List<ScreenerFilterType> { filterName });

            LogStep(2, "Set filter's value according to test data");
            var screenerFiltersForm = new ScreenerFiltersForm();
            screenerFiltersForm.SetNumericFilter(filterName, expectedNumericFilterModel);
            var actualFilterModel = screenerFiltersForm.GetCurrentNumericFilterModel(filterName);
            Checker.CheckEquals(expectedNumericFilterModel, actualFilterModel, $"Filter {filterName.GetStringMapping()} is not set correctly");

            LogStep(3, "Click Run Screener");
            var screenerGridForm = new ScreenerGridForm();
            screenerGridForm.ClickRunScreener();
            var positionsQuantity = screenerGridForm.GetNumberOfRowsInGrid();
            Assert.IsTrue(positionsQuantity > 0, "Grid does not contains data");

            LogStep(4, "if isRequiredCheckInScreenerGrid - check that data in the specified column {columnScreenerGrid} has expected value according to filters option");
            CheckDataOnScreenerGrid(screenerGridForm);

            LogStep(5, "Mark All positions and Check that footer contains expected quantity of selected positions");
            screenerGridForm.SetCheckboxAllPositionsInState(true);
            var addablePositionsQuantity = screenerGridForm.GetSelectedItemsNumberFromFooter();
            Checker.CheckEquals(positionsQuantity, addablePositionsQuantity, "Addable positions count is not same as in footer");

            LogStep(6, "Select Portfolios type (new or existed) and portfolio Name according to test data");
            screenerGridForm.SelectAddToPortfolioItem(addToPortfolioKind);
            Checker.CheckEquals(addToPortfolioKind, screenerGridForm.GetAddToPortfolioItem(),
                "Expected portfolio type is not selected");
            if (addToPortfolioKind == AddToPortfolioSelectType.Existed)
            {
                screenerGridForm.SelectPortfolioToAdd(portfolioNameToAdd);
                Checker.CheckEquals(portfolioNameToAdd, screenerGridForm.GetSelectedPortfolioToAdd(),
                    "Expected portfolio name is not typed for existed portfolio");
            }
            else
            {
                screenerGridForm.SetPortfolioName(portfolioNameToAdd);
                Checker.CheckEquals(portfolioNameToAdd, screenerGridForm.GetPortfolioNameForAdding(),
                    "Expected portfolio name is not typed for new portfolio");
            }

            LogStep(7, "Click Add");
            screenerGridForm.ClickAddToPortfolioButton();
            Checker.CheckEquals(textAfterSaving, screenerGridForm.GetPortfolioActionLabelText(), "Text for saving result is not as expected");

            LogStep(8, "Open positions grid for Portfolio from step 6. Check positions quantity");
            mainMenuNavigation.OpenPositionsGrid();
            new PositionsAlertsStatisticsPanelForm().SelectPortfolio(portfolioNameToAdd);
            var positionsTabForm = new PositionsTabForm();
            positionsTabForm.ScrollToLastRow();
            positionsTabForm.SelectView(viewName);
            var addedPositionsQuantity = positionsTabForm.GetNumberOfRowsInGrid();
            Checker.CheckEquals(addablePositionsQuantity, addedPositionsQuantity, "Saved positions quantity is not as expected");

            LogStep(9, "if isRequiredCheckInPositionGrid - check that data in the specified column {columnPositionGrid} has expected value according to filters option");
            CheckDataOnPositionsGrid(positionsTabForm);

            LogStep(10, "if isRequiredCheckOnGeneralStatistic - check that data in the specified column {fieldStatisticTab} has expected value according to filters option");
            CheckDataOnPositionCard(positionsTabForm);
        }

        private void CheckDataOnPositionCard(PositionsTabForm positionsTabForm)
        {
            if (isRequiredCheckOnGeneralStatistic)
            {
                var positionsIds = positionsTabForm.GetPositionsIds();
                Checker.IsTrue(positionsIds.Any(), "Positions Ids are empty on position grid");
                foreach (var positionId in positionsIds)
                {
                    mainMenuNavigation.OpenPositionCard(positionId);
                    var statisticTab = new PositionCardForm().ActivateTabWithoutChartWaitingGetForm<StatisticTabForm>(PositionCardTabs.Statistics);
                    var actualValue = statisticTab.GetGeneralStatisticTabTabFieldValue(fieldStatisticTab);
                    Checker.IsTrue(expectedNumericFilterModel.IsValueMatchesFilter(actualValue),
                        $"Value {actualValue} does not matched filter condition on position card");
                }
            }
        }

        private void CheckDataOnPositionsGrid(PositionsTabForm positionsTabForm)
        {
            if (isRequiredCheckInPositionGrid)
            {
                var columnData = positionsTabForm.GetPositionColumnValuesWithoutAggregated(columnPositionGrid)
                    .Where(t => !string.IsNullOrEmpty(t));
                Checker.IsTrue(columnData.Any(), $"Column {columnPositionGrid} does not contains data on position grid");
                foreach (var value in columnData)
                {
                    Checker.IsTrue(expectedNumericFilterModel.IsValueMatchesFilter(value),
                        $"Value {value} does not matched filter condition on position grid");
                }
            }
        }

        private void CheckDataOnScreenerGrid(ScreenerGridForm screenerGridForm)
        {
            if (isRequiredCheckInScreenerGrid)
            {
                var columnData = screenerGridForm.GetValuesInScreenerGrid(new List<ScreenerColumnTypes> { columnScreenerGrid })
                    .Select(t => t.ScreenerGridColumnToState[columnScreenerGrid]).Where(t => !string.IsNullOrEmpty(t));
                Checker.IsTrue(columnData.Any(), $"Column {columnScreenerGrid} does not contains data on screener grid");
                foreach (var value in columnData)
                {
                    Checker.IsTrue(expectedNumericFilterModel.IsValueMatchesFilter(value),
                        $"Value {value} does not matched filter condition on screener grid");
                }
            }
        }
    }
}