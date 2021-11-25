using AutomatedTests.Enums.FilterEnums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.Tools;
using AutomatedTests.Enums;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.ResearchPages.PureQuant;
using AutomatedTests.Forms;
using AutomatedTests.Models.FiltersModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._07_Invest._03_PureQuant
{
    [TestClass]
    public class TC_1212_Invest_PureQuant_AppropriatePositionsAreSelectedForCalculations : BaseTestUnitTests
    {
        private const int TestNumber = 1212;

        private AddPortfolioManualModel portfolioModel;

        private List<string> positionsInPortfolio = new List<string>();
        private int positionsQuantity;
        private bool isAverageVqStocks;
        private bool isAverageVqCrypto;
        private bool isVolumeSharesUs;
        private bool isVolumeSharesNonUs;
        private bool isVolumeSharesCrypto;
        private bool isVolumePriceUs;
        private bool isVolumePriceNonUs;
        private bool isVolumePriceCrypto;
        private readonly PureQuantStep2FiltersModel pureQuantFiltersModel = new PureQuantStep2FiltersModel();
        private readonly List<string> expectedSelectedItems = new List<string> { AllPortfoliosKinds.All.GetStringMapping() };

        [TestInitialize]
        public void TestInitialize()
        {
            var userProductSubscriptions = GetUserProductSubscriptions("userSubscription");

            isAverageVqStocks = GetTestDataAsBool(nameof(isAverageVqStocks));
            isAverageVqCrypto = GetTestDataAsBool(nameof(isAverageVqCrypto));
            isVolumeSharesUs = GetTestDataAsBool(nameof(isVolumeSharesUs));
            isVolumeSharesNonUs = GetTestDataAsBool(nameof(isVolumeSharesNonUs));
            isVolumeSharesCrypto = GetTestDataAsBool(nameof(isVolumeSharesCrypto));
            isVolumePriceUs = GetTestDataAsBool(nameof(isVolumePriceUs));
            isVolumePriceNonUs = GetTestDataAsBool(nameof(isVolumePriceNonUs));
            isVolumePriceCrypto = GetTestDataAsBool(nameof(isVolumePriceCrypto));

            portfolioModel = new AddPortfolioManualModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency")
            };

            positionsQuantity = GetTestDataAsInt(nameof(positionsQuantity));
            var positionsModels = new List<PositionAtManualCreatingPortfolioModel>();
            for (var i = 1; i <= positionsQuantity; i++)
            {
                positionsModels.Add(new PositionAtManualCreatingPortfolioModel
                {
                    Ticker = GetTestDataAsString($"SymbolName{i}"),
                    EntryDate = GetTestDataAsString($"EntryDate{i}"),
                    TradeType = PositionTradeTypes.Long,
                    Quantity = GetTestDataAsString($"Shares{i}"),
                    PositionAssetType = GetTestDataParsedAsEnumFromStringMapping<PositionAssetTypes>($"assetType{i}")
                });
            }
            positionsInPortfolio = positionsModels.Select(m => m.Ticker).ToList();

            pureQuantFiltersModel.HealthStatusFilter = FillDataForHealthFilter("health");

            pureQuantFiltersModel.AverageVqFilter = FillDataForNumericFilter(PureQuantFilterTypes.AverageVq, "averageVqFilter");
            pureQuantFiltersModel.AverageVqCryptoFilter = FillDataForNumericFilter(PureQuantFilterTypes.AverageVqCrypto, "averageVqCryptoFilter");
            pureQuantFiltersModel.AverageDailyVolumeSharesUsaFilter = FillDataForNumericFilter(PureQuantFilterTypes.AverageDailyVolumeSharesUsa, "volumeSharesFilter");
            pureQuantFiltersModel.AverageDailyVolumeSharesNonUsaFilter.SubFilterName = PureQuantFilterTypes.AverageDailyVolumeSharesNonUsa;
            pureQuantFiltersModel.AverageDailyVolumeSharesNonUsaFilter.NumericRangeSubFilterNameToState = pureQuantFiltersModel.AverageDailyVolumeSharesUsaFilter.NumericRangeSubFilterNameToState;
            pureQuantFiltersModel.AverageDailyVolumeSharesCryptoFilter.SubFilterName = PureQuantFilterTypes.AverageDailyVolumeSharesCrypto;
            pureQuantFiltersModel.AverageDailyVolumeSharesCryptoFilter.NumericRangeSubFilterNameToState = pureQuantFiltersModel.AverageDailyVolumeSharesUsaFilter.NumericRangeSubFilterNameToState;

            pureQuantFiltersModel.AverageDailyVolumePricesUsaFilter = FillDataForNumericFilter(PureQuantFilterTypes.AverageDailyVolumePriceUsa, "volumePriceFilter");
            pureQuantFiltersModel.AverageDailyVolumePricesNonUsaFilter.SubFilterName = PureQuantFilterTypes.AverageDailyVolumePriceNonUsa;
            pureQuantFiltersModel.AverageDailyVolumePricesNonUsaFilter.NumericRangeSubFilterNameToState = pureQuantFiltersModel.AverageDailyVolumePricesUsaFilter.NumericRangeSubFilterNameToState;
            pureQuantFiltersModel.AverageDailyVolumePricesCryptoFilter.SubFilterName = PureQuantFilterTypes.AverageDailyVolumePriceCrypto;
            pureQuantFiltersModel.AverageDailyVolumePricesCryptoFilter.NumericRangeSubFilterNameToState = pureQuantFiltersModel.AverageDailyVolumePricesUsaFilter.NumericRangeSubFilterNameToState;

            pureQuantFiltersModel.GainsFilter.SubFilterName = PureQuantFilterTypes.Gains.GetStringMapping();
            pureQuantFiltersModel.GainsFilter.SliderFilterStatesState = GetTestDataParsedAsEnumFromStringMapping<SliderFilterStates>("toggleStatGain");
            pureQuantFiltersModel.MaximizeDiversificationFilter.SubFilterName = PureQuantFilterTypes.MaximizeDiversification.GetStringMapping();
            pureQuantFiltersModel.MaximizeDiversificationFilter.SliderFilterStatesState = GetTestDataParsedAsEnumFromStringMapping<SliderFilterStates>("toggleStatDiversification");

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);

            new AddPortfoliosSteps().NavigateToAddManualPortfolioSavePortfolioAfterFillingFields(portfolioModel, positionsModels);
            new MainMenuNavigation().OpenPureQuant(PureQuantInternalSteps.Step1ChooseSources);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1212$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("ResearchPage"), TestCategory("PureQuantTool")]
        [Description("The test checks expected positions are selected for calculations. https://tr.a1qa.com/index.php?/cases/view/22388159")]
        public override void RunTest()
        {
            LogStep(1, "Check that Pure Quant is displayed.");
            var pureQuantStep1Form = new PureQuantStep1Form();
            pureQuantStep1Form.AssertIsOpen();

            LogStep(2, "Select in drop-down list Portfolios & Watchlists created portfolio {test data: PortfolioName}");
            pureQuantStep1Form.SelectPortfolioMultipleByText(PureQuantDataSourceTypes.PortfoliosAndWatchlists, portfolioModel.Name);
            var actualSelectedItems = pureQuantStep1Form.GetSelectedSourcesFromMultipleSelectionDropDown(PureQuantDataSourceTypes.PortfoliosAndWatchlists);
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedSelectedItems, actualSelectedItems),
                $"After selecting portfolio dropdown has unexpected item: {GetExpectedResultsString(expectedSelectedItems)}\n{GetActualResultsString(actualSelectedItems)}");

            LogStep(3, "Click on Change Default Settings button");
            var pureQuantCommonForm = new PureQuantCommonForm();
            pureQuantCommonForm.ClickPureQuantStep(PureQuantInternalSteps.Step2ChangeDefaultSettings);
            var pureQuantStep2Form = new PureQuantStep2Form();
            pureQuantStep2Form.AssertIsOpen();

            LogStep(4, "Check that Health Status block is displayed and select 'Health Status' to values according to test data.");
            pureQuantStep2Form.SetHealthStatusFilter(pureQuantFiltersModel.HealthStatusFilter);
            Checker.CheckEquals(pureQuantFiltersModel.HealthStatusFilter, pureQuantStep2Form.GetCurrentHealthStatusFilterModelAfterScrolling(),
                "Health Filter is unexpected");

            LogStep(5, "Check if isAverageVqStocks than 'Average VQ for Stocks' block is displayed and fill in field the next values according to averageVqStocks.");
            CheckNumericFilterVisibilityAndSetModel(PureQuantFilterTypes.AverageVq, isAverageVqStocks, pureQuantFiltersModel.AverageVqFilter);

            LogStep(6, "Check if isAverageVqCrypto than 'Average VQ for Crypto' block is displayed and fill in field the next values according to averageVqCrypto.");
            CheckNumericFilterVisibilityAndSetModel(PureQuantFilterTypes.AverageVqCrypto, isAverageVqCrypto, pureQuantFiltersModel.AverageVqCryptoFilter);

            LogStep(7, "Check if isVolumeShares* than 'Volume Shares *' block is displayed and fill in field the next values according to volumesFilterOption.");
            CheckNumericFilterVisibilityAndSetModel(PureQuantFilterTypes.AverageDailyVolumeSharesUsa, isVolumeSharesUs, pureQuantFiltersModel.AverageDailyVolumeSharesUsaFilter);
            CheckNumericFilterVisibilityAndSetModel(PureQuantFilterTypes.AverageDailyVolumeSharesNonUsa, isVolumeSharesNonUs, pureQuantFiltersModel.AverageDailyVolumeSharesNonUsaFilter);
            CheckNumericFilterVisibilityAndSetModel(PureQuantFilterTypes.AverageDailyVolumeSharesCrypto, isVolumeSharesCrypto, pureQuantFiltersModel.AverageDailyVolumeSharesCryptoFilter);

            LogStep(8, "Check if isVolumePrice* than 'Volume Price *' block is displayed and fill in field the next values according to volumesFilterOption.");
            CheckNumericFilterVisibilityAndSetModel(PureQuantFilterTypes.AverageDailyVolumePriceUsa, isVolumePriceUs, pureQuantFiltersModel.AverageDailyVolumePricesUsaFilter);
            CheckNumericFilterVisibilityAndSetModel(PureQuantFilterTypes.AverageDailyVolumePriceNonUsa, isVolumePriceNonUs, pureQuantFiltersModel.AverageDailyVolumePricesNonUsaFilter);
            CheckNumericFilterVisibilityAndSetModel(PureQuantFilterTypes.AverageDailyVolumePriceCrypto, isVolumePriceCrypto, pureQuantFiltersModel.AverageDailyVolumePricesCryptoFilter);

            LogStep(9, "Check that Gain have stat Yes according to { test data: toggleStatGain}");
            pureQuantStep2Form.SetSliderFilter(PureQuantFilterTypes.Gains, pureQuantFiltersModel.GainsFilter.SliderFilterStatesState);
            Checker.CheckEquals(pureQuantFiltersModel.GainsFilter, pureQuantStep2Form.GetSliderFilterModel(PureQuantFilterTypes.Gains),
                "Gains filter has unexpected state");

            LogStep(10, "Check that Maximize Diversification is displayed and switch toggle to the next state according to {test data: toggleDiversification}");
            pureQuantStep2Form.SetSliderFilter(PureQuantFilterTypes.MaximizeDiversification, pureQuantFiltersModel.MaximizeDiversificationFilter.SliderFilterStatesState);
            Checker.CheckEquals(pureQuantFiltersModel.MaximizeDiversificationFilter,
                pureQuantStep2Form.GetSliderFilterModel(PureQuantFilterTypes.MaximizeDiversification),
                "Maximize Diversification filter has unexpected state");

            LogStep(11, "Click on Build Portfolio button");
            pureQuantCommonForm.RunResearchOrRecalculate();

            LogStep(12, "Then click on View Pure Quant results button");
            new PureQuantProgressForm().WaitTaskFinishingWithClickingViewResult();
            var pureQuantResultsForm = new PureQuantResultsForm();
            pureQuantResultsForm.AssertIsOpen();

            LogStep(13, "Make sure that expected positions according to {test data: expectedQuantitiesShows} are shown in the grid.");
            Assert.IsTrue(pureQuantResultsForm.IsGridTablePresent(), "Result grid is not shown");
            Checker.CheckEquals(positionsQuantity, pureQuantResultsForm.GetNumberOfPositions(),
                "Unexpected positions quantity in result grid");
            var tickersinGrid = pureQuantResultsForm.GetColumnValuesAsListOfString(PureQuantResultsColumnTypes.Ticker)
                .Select(t => t.Split('\r')[0]).ToList();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(tickersinGrid, positionsInPortfolio),
                "Grid is not as expected after recalculating portfolio (SSI = Green; Long; VQ < 40%; TotalGain is positive): " +
                $"{GetExpectedResultsString(positionsInPortfolio)}\n{GetActualResultsString(tickersinGrid)}");
        }

        private void CheckNumericFilterVisibilityAndSetModel(PureQuantFilterTypes filterType, bool expectedVisibility, NumericFilterModel expectedFilterModel)
        {
            var pureQuantStep2Form = new PureQuantStep2Form();
            var isFilterPresent = pureQuantStep2Form.IsFilterPresent(filterType);
            Checker.CheckEquals(expectedVisibility, isFilterPresent, $"{filterType.GetStringMapping()} filter has unexpected visibility");
            if (expectedVisibility && isFilterPresent)
            {
                pureQuantStep2Form.SetNumericFilter(filterType, expectedFilterModel);
                Checker.CheckEquals(expectedFilterModel, pureQuantStep2Form.GetCurrentNumericFilterModel(filterType),
                    $"{filterType.GetStringMapping()} filter has unexpected state");
            }
        }
    }
}