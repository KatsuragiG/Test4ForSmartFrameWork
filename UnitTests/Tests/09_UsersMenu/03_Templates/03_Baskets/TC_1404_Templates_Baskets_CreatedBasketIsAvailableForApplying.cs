using AutomatedTests.ConstantVariables;
using AutomatedTests.Enums.FilterEnums;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.Template;
using AutomatedTests.Enums.Tools;
using AutomatedTests.Forms.ResearchPages.PureQuant;
using AutomatedTests.Forms.Screener;
using AutomatedTests.Forms.Templates;
using AutomatedTests.Forms.TimingCalendar;
using AutomatedTests.Models.FiltersModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Screener;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._09_UsersMenu._03_Templates._03_Baskets
{
    [TestClass]
    public class TC_1404_Templates_Baskets_CreatedBasketIsAvailableForApplying : BaseTestUnitTests
    {
        private const int TestNumber = 1404;

        private List<string> assetTypes;
        private List<string> tickers;
        private int symbolsQuantity;
        private int predefinedBasketsQuantity;
        private readonly int checkedTickerRow = 1;
        private bool isStockAvailable;
        private bool isCryptoAvailable;
        private bool isScreenerAvailable;
        private bool isPureQuantAvailable;
        private bool isCalendarAvailable;
        private bool isOptionScreenerAvailable;
        private string placeholderWording;
        private string addPositionsFormHeader;
        private string basketName;
        private readonly MainMenuNavigation mainMenuNavigation = new MainMenuNavigation();
        private readonly TradeSmithBasketsFilterModel basketFilterModel = new TradeSmithBasketsFilterModel();
        private readonly OptionSourceFilterModel optionSourceFilterModel = new OptionSourceFilterModel();
        private readonly IncludeFilterModel includeFilterModel = new IncludeFilterModel();

        [TestInitialize]
        public void TestInitialize()
        {
            var userProductSubscriptions = GetUserProductSubscriptions("userSubscription");

            isStockAvailable = GetTestDataAsBool(nameof(isStockAvailable));
            isCryptoAvailable = GetTestDataAsBool(nameof(isCryptoAvailable));
            isScreenerAvailable = GetTestDataAsBool(nameof(isScreenerAvailable));
            isPureQuantAvailable = GetTestDataAsBool(nameof(isPureQuantAvailable));
            isCalendarAvailable = GetTestDataAsBool(nameof(isCalendarAvailable));
            isOptionScreenerAvailable = GetTestDataAsBool(nameof(isOptionScreenerAvailable));
            symbolsQuantity = GetTestDataAsInt(nameof(symbolsQuantity));
            predefinedBasketsQuantity = GetTestDataAsInt(nameof(predefinedBasketsQuantity));
            assetTypes = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(assetTypes));
            tickers = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(tickers));
            placeholderWording = GetTestDataAsString(nameof(placeholderWording));
            addPositionsFormHeader = GetTestDataAsString(nameof(addPositionsFormHeader));

            basketName = StringUtility.RandomStringOfSize(Constants.DigitsQuantityToFillNumericInStringToCompare);
            basketFilterModel.TradesmithBasketItemNameToState.Add(basketName, true);

            optionSourceFilterModel.SourceItem = OptionScreenerSourceTypes.Baskets;
            optionSourceFilterModel.SubSourceItems.Add(basketName);

            includeFilterModel.IncludeFilterNameToState.Add(IncludeFilterTypes.UnderlyingAssets, true);
            includeFilterModel.IncludeFilterNameToState.Add(IncludeFilterTypes.Options, false);

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));

            LoginSetUp.LogIn(UserModels.First());
            mainMenuNavigation.OpenTemplates();
            new TemplatesMenuForm().ClickTemplatesMenuItem(TemplatesMenuItems.Baskets);

        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1404$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test checks possibility to create basket and apply it on different pages https://tr.a1qa.com/index.php?/cases/view/22552038")]
        [TestCategory("TemplatesPage"), TestCategory("Basket"), TestCategory("Screener"), TestCategory("PureQuantTool"), TestCategory("OptionScreener"), TestCategory("Calendar")]
        public override void RunTest()
        {
            LogStep(1, "Check that baskets page is shown. Check that non - empty basket grid is shown.");
            var basketsManagementForm = new BasketsManagementForm();
            basketsManagementForm.AssertIsOpen();
            var previouslyExistedmarkets = basketsManagementForm.GetAllBaskets();
            Checker.CheckEquals(predefinedBasketsQuantity, previouslyExistedmarkets.Count, "There are predefined baskets issue");

            LogStep(2, "Click Add Basket button.");
            basketsManagementForm.ClickAddBasket();
            var addEditBasketForm = new AddEditBasketForm();
            addEditBasketForm.AssertIsOpen();
            Checker.IsTrue(addEditBasketForm.IsBasketNameTextBoxShown(), "There are no basket name textBox");
            Checker.CheckEquals(string.Empty, addEditBasketForm.GetBasketName(), "There are predefined basket name");
            Checker.IsTrue(addEditBasketForm.IsAddPositionButtonShown(), "There are no add position button");
            Checker.IsFalse(addEditBasketForm.IsTickersGridShown(), "There are default tickers grid before adding a new");

            LogStep(3, "Click Add position button.");
            addEditBasketForm.ClickAddPosition();
            var addPositionInBasketForm = new AddPositionInBasketForm();
            addPositionInBasketForm.AssertIsOpen();

            LogStep(4, "Check ADD POSITIONS frame.");
            Checker.CheckEquals(addPositionsFormHeader, addPositionInBasketForm.GetAddPositionFormHeader(), "Header is not as expected");
            Checker.IsTrue(addPositionInBasketForm.IsPositionLabelPresent(AddPositionInBasketFieldTypes.AssetType), "Asset type label is not as expected");
            Checker.IsTrue(addPositionInBasketForm.IsPositionLabelPresent(AddPositionInBasketFieldTypes.Ticker), "Ticker label is not as expected");
            Checker.IsTrue(addPositionInBasketForm.IsAssetTypeDropdownPresent(checkedTickerRow), "Asset type  dropdown is not shown");
            Checker.CheckEquals(isStockAvailable && isCryptoAvailable, !addPositionInBasketForm.IsAssetTypeDropdownDisabled(checkedTickerRow),
                "Asset type dropdown has unexpected dropdown state");
            Checker.CheckContains(placeholderWording, addPositionInBasketForm.GetTickerPlaceHolder(checkedTickerRow),
                "Autocomplete has unexpected placeholder");
            Checker.IsFalse(addPositionInBasketForm.IsTickerDeleteButtonPresent(checkedTickerRow), "Delete button is unexpectedly shown");
            Checker.IsTrue(addPositionInBasketForm.IsCancelButtonPresent(), "Cancel button is not shown");
            Checker.IsTrue(addPositionInBasketForm.IsSaveButtonPresent(), "Save button is not shown");
            Checker.IsTrue(addPositionInBasketForm.IsSaveButtonDisabled(), "Save button is not disabled");

            LogStep(5, "Select required first Asset Type if dropdown is not disabled.");
            if (isStockAvailable && isCryptoAvailable)
            {
                addPositionInBasketForm.SetAssetType(assetTypes.First(), checkedTickerRow);
            }            
            Checker.CheckEquals(assetTypes.First().ParseAsEnumFromStringMapping<PositionAssetTypes>(), addPositionInBasketForm.GetAssetType(checkedTickerRow),
                "Selected asset type is not as expected");

            LogStep(6, "Select required first ticker from autocomplete.");
            addPositionInBasketForm.SetTicker(tickers.First(), checkedTickerRow);
            Checker.CheckEquals(tickers.First(), addPositionInBasketForm.GetTicker(checkedTickerRow), "Selected ticker is not as expected");
            Checker.IsTrue(addPositionInBasketForm.IsSaveButtonDisabled(), "Save button is not disabled after selecting ticker");
            Checker.IsTrue(addPositionInBasketForm.IsTickerDeleteButtonPresent(checkedTickerRow), "Delete button is unexpectedly NOT shown");
            Checker.IsTrue(addPositionInBasketForm.IsAssetTypeDropdownPresent(checkedTickerRow + 1), "Asset type #2 dropdown is not shown");

            LogStep(7, "Select other asset types and tickers.");
            for (int i = 1; i < tickers.Count; i++)
            {
                if (isStockAvailable && isCryptoAvailable)
                {
                    addPositionInBasketForm.SetAssetType(assetTypes[i], i + 1);
                }
                addPositionInBasketForm.SetTicker(tickers[i], i + 1);
                Checker.CheckEquals(assetTypes[i].ParseAsEnumFromStringMapping<PositionAssetTypes>(), addPositionInBasketForm.GetAssetType(i + 1),
                    $"Selected asset type is not as expected in row {i + 1}");
                Checker.CheckEquals(tickers[i], addPositionInBasketForm.GetTicker(i + 1), $"Selected ticker is not as expected in row {i + 1}");
            }

            LogStep(8, "Type the Basket Name.");
            addEditBasketForm.SetBasketName(basketName);
            Checker.CheckEquals(basketName, addEditBasketForm.GetBasketName(), "Basket Name is not as expected");
            Checker.IsFalse(addPositionInBasketForm.IsSaveButtonDisabled(), "Save button is not active after typing basket name");

            LogStep(9, "Click Save.");
            addPositionInBasketForm.ClickSave();
            basketsManagementForm.AssertIsOpen();
            var newlyExistedMarkets = basketsManagementForm.GetAllBaskets();
            Checker.CheckEquals(previouslyExistedmarkets.Count + 1, newlyExistedMarkets.Count, "There are no added baskets");
            Checker.IsTrue(newlyExistedMarkets.Contains(basketName), $"There are no added basket '{basketName}' by name");

            LogStep(10, 12, "if isScreenerAvailable == true then Open new run for screener.");
            if (isScreenerAvailable)
            {
                CheckScreenerBasketApplying();
            }

            LogStep(13, 14, "if isPureQuantAvailable == true then Open Pure Quant new run and select the added basket");
            if (isPureQuantAvailable)
            {
                CheckPureQuantBasketUpplying();
            }

            LogStep(15, 16, "if isCalendarAvailable == true then Open Calendar. Select the added basket name from Sources dropdown");
            if (isCalendarAvailable)
            {
                CheckCalendarBasketApplying();
            }

            LogStep(17, 19, "if isOptionScreenerAvailable == true then Open option screener run. Select Source - Basket and the added Basket in Baskets dropdown");
            if (isOptionScreenerAvailable)
            {
                CheckOptionScreenerBasketApplying();
            }
        }

        private void CheckOptionScreenerBasketApplying()
        {
            mainMenuNavigation.OpenOptionScreeners();
            var optionScreenerForm = new OptionScreenerForm();
            optionScreenerForm.AssertIsOpen();

            optionScreenerForm.SetOptionSourceFilter(optionSourceFilterModel);
            optionScreenerForm.SetIncludeFilter(includeFilterModel);
            var optionScreenerGridForm = new OptionScreenerGridForm();
            optionScreenerGridForm.ClickRunScreener();

            var gridData = optionScreenerGridForm.GetValuesInScreenerGrid(new List<OptionScreenerColumnTypes> { OptionScreenerColumnTypes.Ticker });
            foreach (var row in gridData)
            {
                var actualTicker = row.ScreenerGridColumnToState[OptionScreenerColumnTypes.Ticker].Split('\r')[0];
                Checker.IsTrue(tickers.Contains(actualTicker), $"Ticker {actualTicker} from Option Screener is not included in basket");
            }
        }

        private void CheckCalendarBasketApplying()
        {
            mainMenuNavigation.OpenTimingCalendar();
            var timingCalendarForm = new TimingCalendarForm();
            timingCalendarForm.AssertIsOpen();
            timingCalendarForm.SelectSourceByText(basketName);

            Checker.IsTrue(timingCalendarForm.IsResultGridPresent(), "Timing Grid is not shown");
            var actualTickers = timingCalendarForm.GetTickersColumnValues().Select(t => t.Split('\r')[0]).ToList();
            Checker.IsTrue(actualTickers.Count <= symbolsQuantity, $"Positions Calendar grid {actualTickers.Count} more than expected {symbolsQuantity}");

            foreach (var ticker in actualTickers)
            {
                Checker.IsTrue(tickers.Contains(ticker), $"Ticker {ticker} from Calendar is not included in basket");
            }
        }

        private void CheckPureQuantBasketUpplying()
        {
            mainMenuNavigation.OpenPureQuant(PureQuantInternalSteps.Step1ChooseSources);
            var pureQuantStep1Form = new PureQuantStep1Form();
            pureQuantStep1Form.AssertIsOpen();
            pureQuantStep1Form.SelectPortfolioMultipleByText(PureQuantDataSourceTypes.TradeSmithBaskets, basketName);

            LogStep(14, "Click Build portfolio");
            new PureQuantCommonForm().RunResearchOrRecalculate();
            new PureQuantProgressForm().WaitTaskFinishingWithClickingViewResult();
            var pureQuantResultsForm = new PureQuantResultsForm();
            pureQuantResultsForm.AssertIsOpen();
            var actualPositionsQuantity = pureQuantResultsForm.GetNumberOfSelectedPositions();
            Checker.IsTrue(actualPositionsQuantity <= symbolsQuantity, $"Positions on Pure Quant grid {actualPositionsQuantity} more than expected {symbolsQuantity}");

            var positionsTickers = pureQuantResultsForm.GetSymbolsFromGrid();
            foreach (var ticker in positionsTickers)
            {
                Checker.IsTrue(tickers.Contains(ticker), $"Ticker {ticker} from PQ is not included in basket");
            }
        }

        private void CheckScreenerBasketApplying()
        {
            mainMenuNavigation.OpenScreenerFilters();
            var screenerFiltersForm = new ScreenerFiltersForm();
            screenerFiltersForm.AssertIsOpen();
            new ScreenerSteps().MakeOnlyRequiredFiltersActive(new List<ScreenerFilterType> { ScreenerFilterType.TradeSmithBaskets });

            LogStep(11, "Select the added basket name from TradeSmith Baskets dropdown");

            screenerFiltersForm.SetTradeSmithBasketsFilterAfterScrolling(basketFilterModel);
            Checker.CheckEquals(basketFilterModel, screenerFiltersForm.GetTradeSmithBasketsFilterModel(), "Basket models are not equal");

            LogStep(12, "Click Run Screener");
            var screenerGridForm = new ScreenerGridForm();
            screenerGridForm.ClickRunScreener();
            var positionsQuantity = screenerGridForm.GetNumberOfRowsInGrid();
            Checker.IsTrue(positionsQuantity > 0, "Screener Grid does not contain data");
            Checker.CheckEquals(symbolsQuantity, positionsQuantity, "Screener Grid contains unexpected data");
            var gridData = screenerGridForm.GetValuesInScreenerGrid(new List<ScreenerColumnTypes> { ScreenerColumnTypes.Ticker });
            foreach (var row in gridData)
            {
                var actualTicker = row.ScreenerGridColumnToState[ScreenerColumnTypes.Ticker].Split('\r')[0];
                Checker.IsTrue(tickers.Contains(actualTicker), $"Ticker {actualTicker} from Screener is not included in basket");
            }
        }
    }
}