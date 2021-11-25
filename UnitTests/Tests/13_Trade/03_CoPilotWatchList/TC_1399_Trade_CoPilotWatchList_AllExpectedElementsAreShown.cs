using AutomatedTests.ConstantVariables;
using AutomatedTests.Enums.Chart;
using AutomatedTests.Enums.FilterEnums;
using AutomatedTests.Enums.PositionCard;
using AutomatedTests.Enums.Template;
using AutomatedTests.Enums.Tools;
using AutomatedTests.Enums.TradeSummary;
using AutomatedTests.Enums;
using AutomatedTests.Forms.LoginForm;
using AutomatedTests.Forms.Templates;
using AutomatedTests.Forms.Trade;
using AutomatedTests.Forms;
using AutomatedTests.Models.FiltersModels;
using AutomatedTests.Models.UserModels;
using AutomatedTests.Navigation;
using AutomatedTests.Steps.BrowserSteps;
using AutomatedTests.Steps.Screener;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System;
using TradeStops.Common.Utils;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._13_Trade._03_CoPilotWatchList
{
    [TestClass]
    public class TC_1399_Trade_CoPilotWatchList_AllExpectedElementsAreShown : BaseTestUnitTests
    {
        private const int GridRowToCheckTradeSummary = 1;

        private int filterOrder = 1;
        private string login;
        private string password;
        private string expectedBasketLabel;
        private string basketManagementLinkUrlPattern;
        private string basketManagementLinkWording;
        private List<string> basketTickers;
        private readonly List<OptionScreenerColumnTypes> columnsToSaveFromGrid = new List<OptionScreenerColumnTypes>
        {
            OptionScreenerColumnTypes.MaxProfit,
            OptionScreenerColumnTypes.MaxLoss,
            OptionScreenerColumnTypes.Ticker,
            OptionScreenerColumnTypes.ProbabilityOfProfit,
            OptionScreenerColumnTypes.Roi
        };
        private readonly CoPilotFiltersModel defaultCoPilotFiltersModel = new CoPilotFiltersModel();
        private readonly SortFilterModel changedSortFilter = new SortFilterModel();
        private NumericFilterModel changedDaysToExpiration = new NumericFilterModel();
        private List<CoPilotFilterTypes> expectedFilters;

        [TestInitialize]
        public void TestInitialize()
        {
            login = GetTestDataAsString(nameof(login));
            password = GetTestDataAsString(nameof(password));
            expectedBasketLabel = GetTestDataAsString(nameof(expectedBasketLabel));
            basketManagementLinkUrlPattern = GetTestDataAsString(nameof(basketManagementLinkUrlPattern));
            basketManagementLinkWording = GetTestDataAsString(nameof(basketManagementLinkWording));
            expectedFilters = EnumUtils.GetValues<CoPilotFilterTypes>().ToList();

            FillDataInFiltersModels();

            LogStep(0, "Precondition - Login as user with subscription to Portfolio Tracker");
            UserModels.Add(new UserModel { Email = login, Password = password });

            var mainMenuNavigation = new MainMenuNavigation();
            mainMenuNavigation.OpenQaLoginForm();

            var loginForm = new LoginForm();
            loginForm.AssertIsOpen();
            loginForm.LogInWithoutDbWaiting(UserModels.First());
            loginForm.CloseWalkMePopupIfExists();

            var mainMenuForm = new MainMenuForm();
            mainMenuForm.ClickMenuItem(MainMenuItems.AlertTemplates);
            var templatesMenuForm = new TemplatesMenuForm();
            templatesMenuForm.CloseWalkMePopupIfExists();
            templatesMenuForm.ClickTemplatesMenuItem(TemplatesMenuItems.Baskets);

            var basketsManagementForm = new BasketsManagementForm();
            basketsManagementForm.ClickOnBasketName(Constants.DefaultPositionsBasketName);
            var isEditBasketPageLoaded = basketsManagementForm.IsEditBasketPageLoaded();
            Checker.IsTrue(isEditBasketPageLoaded, "Edit basket form is not loaded");

            if (isEditBasketPageLoaded)
            {
                var editBasketForm = new AddEditBasketForm();
                basketTickers = editBasketForm.GetColumnValuesOnAllPages(BasketColumnFieldTypes.Ticker).Select(t => t.Split('\r').First()).ToList();
                Checker.IsTrue(basketTickers.Any(), "Positions in basket are empty");
            }

            mainMenuForm.ClickMenuItem(MainMenuItems.Trade);
        }

        private void FillDataInFiltersModels()
        {
            for (int i = 1; i <= EnumUtils.GetValues<CoPilotOptionType>().Count(); i++)
            {
                defaultCoPilotFiltersModel.OptionTypeFilter.OptionTypeToState.Add(
                    GetTestDataParsedAsEnumFromStringMapping<CoPilotOptionType>($"Filter{filterOrder}Option{i}"),
                    GetTestDataAsBool($"Filter{filterOrder}Value{i}"));
            }
            IncrementFilterOrder();
            defaultCoPilotFiltersModel.DaysToExpirationFilter = FillDataForNumericFilter(CoPilotFilterTypes.DaysToExpiration, "Filter", filterOrder);
            IncrementFilterOrder();
            defaultCoPilotFiltersModel.ProbabilityOfProfitFilter = FillDataForNumericFilter(CoPilotFilterTypes.ProbabilityOfProfit, "Filter", filterOrder);
            IncrementFilterOrder();
            defaultCoPilotFiltersModel.RoiFilter = FillDataForNumericFilter(CoPilotFilterTypes.Roi, "Filter", filterOrder);
            IncrementFilterOrder();
            defaultCoPilotFiltersModel.MaxProfitFilter = FillDataForNumericFilter(CoPilotFilterTypes.MaxProfit, "Filter", filterOrder);

            IncrementFilterOrder();
            for (int i = 1; i <= EnumUtils.GetValues<SortResultTypes>().Count(); i++)
            {
                defaultCoPilotFiltersModel.SortFilter.SortFilterTypeToState.Add(
                    (SortResultTypes)Enum.Parse(typeof(SortResultTypes), GetTestDataAsString($"Filter{filterOrder}Option{i}")),
                    GetTestDataAsString($"Filter{filterOrder}Value{i}"));
            }
            IncrementFilterOrder();
            for (int i = 1; i <= EnumUtils.GetValues<SortResultTypes>().Count(); i++)
            {
                changedSortFilter.SortFilterTypeToState.Add(
                    (SortResultTypes)Enum.Parse(typeof(SortResultTypes), GetTestDataAsString($"Filter{filterOrder}Option{i}")),
                    GetTestDataAsString($"Filter{filterOrder}Value{i}"));
            }
            IncrementFilterOrder();
            changedDaysToExpiration = FillDataForNumericFilter(CoPilotFilterTypes.DaysToExpiration, "Filter", filterOrder);
        }
        private void IncrementFilterOrder()
        {
            filterOrder++;
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1399$", DataAccessMethod.Sequential)]
        [Description("The test checks CoPilot WatchList page elements and logic https://tr.a1qa.com/index.php?/cases/view/22180092")]
        [TestMethod]
        [TestCategory("CoPilot"), TestCategory("OptionScreener")]
        public override void RunTest()
        {
            LogStep(1, "Check that the CoPilot Watchlist item in the Trade menu is active");
            Checker.IsTrue(new TradeMenuForm().IsTradeMenuTabSelected(TradeMenuItems.CoPilotWatchlist),
                "CoPilot Watchlist menu item is not active");

            LogStep(2, "Check that CoPilot Watchlist form is shown");
            var coPilotWatchlistForm = new CoPilotWatchlistForm();
            coPilotWatchlistForm.AssertIsOpen();

            LogStep(3, 4, "Click Filters. There are expected filters. ");
            coPilotWatchlistForm.ClickFiltersButton();
            var activeFilters = coPilotWatchlistForm.GetActiveFiltersNames();
            Checker.CheckEquals(activeFilters.Count, expectedFilters.Count,
                $"Filters quantity is not as expected: {GetExpectedResultsString(expectedFilters.Select(t => t.GetStringMapping()).ToList())}\r\n" +
                $"{GetActualResultsString(activeFilters)}");
            foreach (var coPilotFilterType in expectedFilters)
            {
                Checker.IsTrue(coPilotWatchlistForm.IsFilterExist(coPilotFilterType),
                    $"{coPilotFilterType.GetStringMapping()} filter is not shown");
            }

            LogStep(5, "Check that default value for filters");
            Checker.CheckEquals(defaultCoPilotFiltersModel, new ScreenerSteps().GetExpectedCoPilotFiltersModel(defaultCoPilotFiltersModel),
                "Default state of filters is not as expected");

            LogStep(6, "Select in Display & Sort Results the Volume/Open Interest item and click Apply buttons");
            coPilotWatchlistForm.SetNumericFilterAfterScrolling(CoPilotFilterTypes.DaysToExpiration, changedDaysToExpiration);
            coPilotWatchlistForm.SetSortByDropdown(OptionScreenerColumnTypes.Ticker.GetStringMapping());
            coPilotWatchlistForm.ClickFiltersApplyingButton(FiltersApplyingButtonTypes.Apply);

            LogStep(7, "Check that Baskets dropdown has default item CoPilot Companion, disabled and has correct wording for label");
            Checker.CheckContains(Constants.DefaultPositionsBasketName, coPilotWatchlistForm.GetBasketName(),
                "CoPilot Watchlist menu item is not active");
            Checker.IsTrue(coPilotWatchlistForm.IsBasketDropdownDisabled(), "Basket name is not disabled");
            Checker.CheckContains(expectedBasketLabel, coPilotWatchlistForm.GetBasketLabelWording(),
                "Basket Label Wording is not as expected");

            LogStep(8, "Check that Basket Management link leads to editing basket CoPilot Companion");
            var urlLink = coPilotWatchlistForm.GetBasketManagementUrl();
            Checker.IsTrue(new Regex(basketManagementLinkUrlPattern).IsMatch(urlLink), $"Unexpected Basket Management link {urlLink}");
            Checker.CheckContains(basketManagementLinkWording, coPilotWatchlistForm.GetBasketManagementText(),
                "Unexpected Basket Management text");
            coPilotWatchlistForm.ClickBasketManagement();
            var editBasketForm = new AddEditBasketForm();
            editBasketForm.AssertIsOpen();

            LogStep(9, "Click back. Check that CoPilot Watchlist form is shown");
            editBasketForm.ClickBack();
            coPilotWatchlistForm.AssertIsOpen();

            LogStep(10, "Check that default mode is Tile (grid is not shown, tiles quantity is more than 0, Tile button is active)");
            Checker.CheckEquals(GridModeTypes.Tile, coPilotWatchlistForm.GetGridMode(),
                "CoPilot Watchlist grid mode is not as expected by default");
            Checker.IsTrue(coPilotWatchlistForm.IsActiveOptionGridModeButton(GridModeTypes.Tile), "Tile button is not active");
            var tilesQuantity = coPilotWatchlistForm.GetNumberOfOptionsInTileOnCurrentPage();
            var expectedDefaultQuantity = Parsing.ConvertToInt(defaultCoPilotFiltersModel.SortFilter.SortFilterTypeToState[SortResultTypes.ShowTop]);
            Checker.CheckEquals(expectedDefaultQuantity, tilesQuantity, "Tiles quantity is not as expected");
            var gridRowsQuantity = coPilotWatchlistForm.GetNumberOfRowsInGrid();
            Checker.CheckEquals(0, gridRowsQuantity, "Grid is shown unexpectedly");

            LogStep(11, "Check that all available tiles are in GENERAL mode by default");
            for (int i = 1; i <= tilesQuantity; i++)
            {
                Checker.CheckEquals(TileCarouselPageTypes.General, coPilotWatchlistForm.GetCarouselPageTypeByTileOrder(i),
                    $"Tile #{i} in unexpected mode");
            }

            LogStep(12, "Click list. Check that grid is shown, tiles quantity is 0");
            coPilotWatchlistForm.ClickOptionGridModeButton(GridModeTypes.List);
            Checker.CheckEquals(GridModeTypes.List, coPilotWatchlistForm.GetGridMode(),
                "CoPilot Watchlist grid mode is not List after clicking");
            Checker.IsFalse(coPilotWatchlistForm.IsActiveOptionGridModeButton(GridModeTypes.Tile), "Tile button is active after switching in List mode");
            Checker.IsTrue(coPilotWatchlistForm.IsActiveOptionGridModeButton(GridModeTypes.List), "List button is not active after switching in List mode");
            tilesQuantity = coPilotWatchlistForm.GetNumberOfOptionsInTileOnCurrentPage();
            gridRowsQuantity = coPilotWatchlistForm.GetNumberOfRowsInGrid();
            Checker.CheckEquals(0, tilesQuantity, "Tiles is shown unexpectedly after switching in List mode");

            LogStep(13, "Check that table contains expected columns");
            var expectedColumns = EnumUtils.GetValues<OptionScreenerColumnTypes>().Select(t => t.GetStringMapping()).ToList();
            var actualColumns = coPilotWatchlistForm.GetColumnNamesFromGrid();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedColumns, actualColumns),
                $"Columns are not as expected: {GetExpectedResultsString(expectedColumns)}\r\n{GetActualResultsString(actualColumns)}");

            LogStep(14, "Check that table contains expected default rows quantity (20)");
            Checker.CheckEquals(expectedDefaultQuantity, gridRowsQuantity, "Grid rows quantity is not as expected after switching in List mode");

            LogStep(15, "Remember grid");
            var gridListModeData = coPilotWatchlistForm.GetCoPilotGrid(columnsToSaveFromGrid);
            Checker.CheckEquals(expectedDefaultQuantity, gridListModeData.Count, "Grid model does not contains expected rows quantity");

            LogStep(16, "Click Tile");
            coPilotWatchlistForm.ClickOptionGridModeButton(GridModeTypes.Tile);
            Checker.CheckEquals(GridModeTypes.Tile, coPilotWatchlistForm.GetGridMode(),
                "CoPilot Watchlist tile mode is not List after switching back");
            tilesQuantity = coPilotWatchlistForm.GetNumberOfOptionsInTileOnCurrentPage();
            Checker.CheckEquals(expectedDefaultQuantity, tilesQuantity, "Tiles is shown unexpectedly after switching Tile mode");

            LogStep(17, "Remember tiles data (Option names, Pop, Roi, Max Loss, Max Profit) for all available tiles");
            var tilesData = coPilotWatchlistForm.GetAllTilesModelsOnCurrentPage();
            Checker.CheckEquals(expectedDefaultQuantity, tilesData.Count, "Tiles models quantity is not as expected");

            LogStep(18, "Check that data from steps 15 and 17 are matched");
            for (int i = 0; i < tilesData.Count; i++)
            {
                Checker.CheckEquals(gridListModeData[i].ScreenerGridColumnToState[OptionScreenerColumnTypes.Ticker],
                    StringUtility.GetOptionTickerForUsdOptionName(tilesData[i].OptionName), $"Option ticker # {i + 1} is not as expected");
                Checker.CheckEquals(gridListModeData[i].ScreenerGridColumnToState[OptionScreenerColumnTypes.ProbabilityOfProfit],
                    tilesData[i].Pop, $"Probability Of Profit ticker # {i + 1} is not as expected");
                Checker.CheckEquals(gridListModeData[i].ScreenerGridColumnToState[OptionScreenerColumnTypes.Roi],
                    tilesData[i].Roi, $"Return of Investment ticker # {i + 1} is not as expected");
                Checker.CheckEquals(gridListModeData[i].ScreenerGridColumnToState[OptionScreenerColumnTypes.MaxLoss],
                    tilesData[i].MaxLoss, $"Max Loss ticker # {i + 1} is not as expected");
                Checker.CheckEquals(gridListModeData[i].ScreenerGridColumnToState[OptionScreenerColumnTypes.MaxProfit],
                    tilesData[i].MaxProfit, $"Max Profit ticker # {i + 1} is not as expected");
            }

            LogStep(19, "Click Filters Button. Click Apply and check that tiles are not changed in comparison with step 17");
            coPilotWatchlistForm.ClickFiltersButton();
            coPilotWatchlistForm.ClickFiltersApplyingButton(FiltersApplyingButtonTypes.Apply);
            var optionsNamesAfterFilters = coPilotWatchlistForm.GetAllTilesModelsOnCurrentPage().Select(t => t.OptionName).ToList();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsInOrder(tilesData.Select(t => t.OptionName).ToList(), optionsNamesAfterFilters),
                "Tiles has changed unexpectedly after default filters applying");

            LogStep(20, "Check that print and export button are shown");
            Checker.IsTrue(coPilotWatchlistForm.IsButtonAboveGridPresent(GridActionsButton.Export), "Export button is not shown");
            Checker.IsTrue(coPilotWatchlistForm.IsButtonAboveGridPresent(GridActionsButton.Print), "Print button is not shown");

            LogStep(21, "Check for one random tile that selecting UNDERLYING ASSET PRICE FORECAST " +
                "carousel item cause appearing chart with Strike Price, Expiration Date, probabilities lines");
            var tileOrderToClick = SRandom.Instance.Next(1, optionsNamesAfterFilters.Count);
            coPilotWatchlistForm.ClickTileArrowByTileOrder(tileOrderToClick, ChevronTypes.Next);
            var underlyPriceForecastWidget = coPilotWatchlistForm.GetUnderlyPriceForecastElement(tileOrderToClick);
            Checker.IsTrue(underlyPriceForecastWidget.IsExists(), "Probability of underlying stock Widget is NOT shown");
            Checker.IsTrue(underlyPriceForecastWidget.IsPriceLinePresent(ChartLineTypes.StrikePrice), "Strike price line is NOT shown for Probability of underlying");
            Checker.IsTrue(underlyPriceForecastWidget.IsPriceLinePresent(ChartLineTypes.ExpirationDate), "Expiration date line is NOT shown");
            Checker.IsTrue(underlyPriceForecastWidget.IsPriceLinePresent(ChartLineTypes.LowPriceProbability), "Low Price Probability line is NOT shown");
            Checker.IsTrue(underlyPriceForecastWidget.IsPriceLinePresent(ChartLineTypes.MediumPriceProbability), "Medium Price Probability line is NOT shown");
            Checker.IsTrue(underlyPriceForecastWidget.IsPriceLinePresent(ChartLineTypes.HighPriceProbability), "High Price Probability line is NOT shown");
            Checker.IsTrue(underlyPriceForecastWidget.IsPriceLinePresent(ChartLineTypes.Price), "Underly price line is NOT shown");

            LogStep(22, "Check for the tile from step 20 that selecting Key Statistics carousel item cause appearing three fun facts");
            coPilotWatchlistForm.ClickTileArrowByTileOrder(tileOrderToClick, ChevronTypes.Next);
            var keyStatisticsWidget = coPilotWatchlistForm.GetKeyStatisticsElement(tileOrderToClick);
            Checker.IsTrue(keyStatisticsWidget.IsExists(), "Key Statistics Widget is NOT exist");
            Checker.IsTrue(keyStatisticsWidget.GetAllKeyStatisticsText().Any(), "Key Statistics are empty");

            LogStep(23, "Check in the tile mode that there are no pagination by default");
            Checker.IsFalse(coPilotWatchlistForm.IsPaginationExist(), "Pagination is exist by default");

            LogStep(24, "Click trade summary link for any tile and check trade summary form is shown in new tab. Close tab");
            coPilotWatchlistForm.ClickTradeSummaryInTile(tileOrderToClick);
            var browserSteps = new BrowserSteps();
            browserSteps.CheckThatNewTabOpensPerformActionWithSwitchToNewTabBackAfterClosing(() =>
                new TradeSummaryForm().AssertIsOpen());
            coPilotWatchlistForm.AssertIsOpen();

            LogStep(25, "Click Filters, select from Display & Sort Results show 250 items and click Apply");
            coPilotWatchlistForm.ScrollUpPage();
            coPilotWatchlistForm.ClickFiltersButton();
            coPilotWatchlistForm.ScrollToPreviousFilter(CoPilotFilterTypes.SortResults);
            coPilotWatchlistForm.SetCurrentSortFilterModel(changedSortFilter);
            coPilotWatchlistForm.ClickFiltersApplyingButton(FiltersApplyingButtonTypes.Apply);
            Checker.CheckEquals(GridModeTypes.Tile, coPilotWatchlistForm.GetGridMode(),
                "CoPilot Watchlist tile mode is not List after set 250 tiles");
            Checker.IsTrue(coPilotWatchlistForm.IsPaginationExist(), "Pagination is NOT exist");

            LogStep(26, "Click list. Check that grid is shown. Check that Options are based only on tickers from All listed tickers on Basket CoPilot Companion (precondition)");
            coPilotWatchlistForm.ClickOptionGridModeButton(GridModeTypes.List);
            Checker.CheckEquals(GridModeTypes.List, coPilotWatchlistForm.GetGridMode(),
                "CoPilot Watchlist grid mode is not List after set 250 tiles");
            var parentTickersInOptionGrid = coPilotWatchlistForm.GetColumnValuesAsListOfString(OptionScreenerColumnTypes.Ticker)
                .Select(t => Constants.OptionSymbolRegex.Match(t).Value).ToList();
            Checker.CheckEquals(parentTickersInOptionGrid.Intersect(basketTickers).Count(), parentTickersInOptionGrid.Distinct().Count(),
                $"Not all option's parents from basket {GetExpectedResultsString(basketTickers)}\r\n{GetActualResultsString(parentTickersInOptionGrid)}");

            LogStep(27, "Select for option trade summary item from the context dropdown and check that trade summary form is shown in new tab");
            coPilotWatchlistForm.SelectContextMenuItemByGridRowOrder(GridRowToCheckTradeSummary, CoPilotContextNavigationTypes.TradeSummary);
            browserSteps.CheckThatNewTabOpensPerformActionWithSwitchToNewTabBackAfterClosing(() =>
                new TradeSummaryForm().AssertIsOpen());
        }

        [TestCleanup]
        public new void CleanAfterTest()
        {
            IsDeleteUserViaApi = false;
            base.CleanAfterTest();
        }
    }
}