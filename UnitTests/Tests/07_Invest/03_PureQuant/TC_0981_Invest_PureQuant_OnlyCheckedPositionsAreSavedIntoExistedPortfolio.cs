using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Enums.FilterEnums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.Tools;
using AutomatedTests.Enums;
using AutomatedTests.Forms.MyPortfolios;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.ResearchPages.PureQuant;
using AutomatedTests.Forms;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.ResearchPages;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._07_Invest._03_PureQuant
{
    [TestClass]
    public class TC_0981_Invest_PureQuant_OnlyCheckedPositionsAreSavedIntoExistedPortfolio : BaseTestUnitTests
    {
        private const int TestNumber = 981;

        private readonly List<PositionsGridDataField> columnsToCollectData = new List<PositionsGridDataField>
        {
            PositionsGridDataField.Shares, PositionsGridDataField.Notes, PositionsGridDataField.Value, PositionsGridDataField.Ticker
        };
        private PortfolioDBModel portfolioModel;
        private int expectedPositionsQuantity;
        private int positionsPercentToCheck;
        private string expectedCurrency;
        private string expectedAmount;
        private List<string> billionairePublishers;
        private List<string> basketSources;

        [TestInitialize]
        public void TestInitialize()
        {
            portfolioModel = new PortfolioDBModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = ((int)GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("portfolioType")).ToString(),
                CurrencyId = $"{(int)GetTestDataParsedAsEnumFromStringMapping<Currency>("Currency")}"
            };

            expectedPositionsQuantity = GetTestDataAsInt(nameof(expectedPositionsQuantity));
            positionsPercentToCheck = GetTestDataAsInt(nameof(positionsPercentToCheck));
            expectedAmount = GetTestDataAsString(nameof(expectedAmount));
            expectedCurrency = GetTestDataAsString(nameof(expectedCurrency));

            billionairePublishers = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(billionairePublishers));
            basketSources = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(basketSources));

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPlatinum));

            PortfoliosSetUp.AddPortfolioViaDb(UserModels.First(), portfolioModel);

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);
            new MyPortfoliosMenuForm().ClickMyPortfoliosMenuItem(MyPortfoliosMenuItems.PositionsGrid);

            var positionTab = new PositionsTabForm();
            positionTab.ClickEditSignForCurrentView();
            positionTab.CheckCertainCheckboxes(columnsToCollectData.Where(t => t != PositionsGridDataField.Ticker).Select(t => t.GetStringMapping()).ToList());
            positionTab.ClickSaveViewButton();
            new MainMenuNavigation().OpenPureQuant(PureQuantInternalSteps.Step1ChooseSources);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_981$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("ResearchPage"), TestCategory("PureQuantTool"), TestCategory("AddPortfolio")]
        [Description("The test checks possibility saving only checked positions. https://tr.a1qa.com/index.php?/cases/view/19232164")]
        public override void RunTest()
        {
            LogStep(1, "Select All item on the dropdown TradeSmith Billionaire's Club.");
            var pureQuantStep1Form = new PureQuantStep1Form();
            pureQuantStep1Form.AssertIsOpen();
            SelectItemsInSourceAndCheckIt(PureQuantDataSourceTypes.TradeSmithBillionairesClub, billionairePublishers);

            LogStep(2, "Select All Cryptocurrencies and Copilot Companion on the dropdown TradeSmith Baskets.");
            SelectItemsInSourceAndCheckIt(PureQuantDataSourceTypes.TradeSmithBaskets, basketSources);

            LogStep(3, "Set Sources Combination in Any.");
            pureQuantStep1Form.SetSourcesCombination(StrategyFilterToggleState.Any);
            Checker.CheckEquals(StrategyFilterToggleState.Any, pureQuantStep1Form.GetSourcesCombinationModel().SourcesCombinationFilterState,
                "Sources Combination is not as expected");

            LogStep(4, "Go to step 2 and disable diversification'");
            var pureQuantCommonForm = new PureQuantCommonForm();
            pureQuantCommonForm.ClickPureQuantStep(PureQuantInternalSteps.Step2ChangeDefaultSettings);
            var pureQuantStep2Form = new PureQuantStep2Form();
            pureQuantStep2Form.AssertIsOpen();
            pureQuantStep2Form.SetSliderFilter(PureQuantFilterTypes.MaximizeDiversification, SliderFilterStates.No);
            Checker.CheckEquals(SliderFilterStates.No, pureQuantStep2Form.GetSliderFilterModel(PureQuantFilterTypes.MaximizeDiversification).SliderFilterStatesState,
                "Maximize Diversification is not as expected");

            LogStep(5, "Go to step 3");
            pureQuantCommonForm.ClickPureQuantStep(PureQuantInternalSteps.Step3CustomizePortfolio);
            var pureQuantStep3Form = new PureQuantStep3Form();

            LogStep(6, $"Type {expectedPositionsQuantity} in positions quantity");
            pureQuantStep3Form.SetNumberOfPositions(expectedPositionsQuantity);
            Checker.CheckEquals(expectedPositionsQuantity, Parsing.ConvertToInt(pureQuantStep3Form.GetNumberOfPositions()),
                "Positions quantity is not as expected");

            LogStep(7, $"Type {expectedAmount} and {expectedCurrency} in the How much would you like to invest in this new portfolio fields");
            pureQuantStep3Form.SetInvestmentAmount(expectedAmount);
            pureQuantStep3Form.SetInvestmentCurrency(expectedCurrency);
            Checker.CheckEquals(expectedAmount, pureQuantStep3Form.GetInvestmentAmount().Replace(",", string.Empty),
                "Invest Amount is not as expected");
            Checker.CheckEquals(expectedCurrency, pureQuantStep3Form.GetInvestmentCurrency(),
                "Invest Currency is not as expected");

            LogStep(8, "Click 'Build Portfolio' button. Wait Result Appearing");
            pureQuantCommonForm.RunResearchOrRecalculate();
            new PureQuantProgressForm().WaitTaskFinishingWithClickingViewResult();
            var pureQuantResultsForm = new PureQuantResultsForm();
            pureQuantResultsForm.AssertIsOpen();

            LogStep(9, "Check that grid is shown");
            Checker.IsTrue(pureQuantResultsForm.IsGridTablePresent(), "9 Grid is not shown");
            var actualPositionsQuantity = pureQuantResultsForm.GetNumberOfPositions();
            Checker.IsTrue(actualPositionsQuantity > 0, "9 Positions on Pure Quant grid after first calculations are not as expected");

            LogStep(10, "Check that expectedPositionsQuantity of positions is marked by checkbox");
            Checker.CheckEquals(expectedPositionsQuantity, pureQuantResultsForm.GetNumberOfSelectedPositions(),
                "10 Selected positions quantity is not as expected");

            LogStep(11, "Uncheck all positions");
            pureQuantResultsForm.SetSelectAllPositionsInState(true);
            Checker.IsTrue(pureQuantResultsForm.IsSelectAllChecked(),
                "Selected all state is not true");
            Checker.CheckEquals(actualPositionsQuantity, pureQuantResultsForm.GetNumberOfSelectedPositions(),
                "11 Selected positions quantity is not as expected");
            pureQuantResultsForm.SetSelectAllPositionsInState(false);
            Checker.IsFalse(pureQuantResultsForm.IsSelectAllChecked(),
                "Selected all state is not false");
            Checker.CheckEquals(0, pureQuantResultsForm.GetNumberOfSelectedPositions(),
                "Unselected positions quantity is not as expected");

            LogStep(12, $"Check checkboxes for {positionsPercentToCheck} positions");
            var markedPositionsQuantity = positionsPercentToCheck * expectedPositionsQuantity / 100;
            var positionsOrdersToClick = Randoms.GetRandomNumbersInRange(1, Constants.DefaultContractSize, markedPositionsQuantity);
            positionsOrdersToClick.Reverse();
            var positionsTickers = pureQuantResultsForm.GetSymbolsFromGrid();
            pureQuantResultsForm.ScrollPageDown();
            var markedTickers = new List<string>();
            foreach (var positionOrdersToClick in positionsOrdersToClick)
            {
                pureQuantResultsForm.SetPositionCheckboxByTickerState(positionsTickers[positionOrdersToClick - 1], true);
                markedTickers.Add(positionsTickers[positionOrdersToClick - 1]);
                Checker.IsTrue(pureQuantResultsForm.IsSelectedPositionCb(positionsTickers[positionOrdersToClick - 1]),
                    $"Ticker {positionsTickers[positionOrdersToClick - 1]} should be selected");
            }

            LogStep(13, "Click 'Update Results' button");
            pureQuantResultsForm.ClickUpdateResultsButton();
            pureQuantResultsForm.AssertIsOpen();
            Checker.IsTrue(pureQuantResultsForm.IsGridTablePresent(), "12 Grid is not shown");
            var rebalancedPositionsQuantity = pureQuantResultsForm.GetNumberOfSelectedPositions();
            Checker.IsTrue(rebalancedPositionsQuantity > 0, "12 Positions on Pure Quant grid after first calculations are not as expected");

            LogStep(14, "Make sure that positionsPercentToCheck positions are checked. Make sure that other positions are unchecked");
            Checker.CheckEquals(markedPositionsQuantity, pureQuantResultsForm.GetNumberOfSelectedPositions(),
                "14 Selected positions quantity is not as expected");
            foreach (var unmarkedTicker in positionsTickers.Except(markedTickers))
            {
                Checker.IsFalse(pureQuantResultsForm.IsSelectedPositionCb(unmarkedTicker),
                    $"Ticker {unmarkedTicker} should be unselected");
            }

            LogStep(15, "Keep positions data in grid for the marked positions");
            pureQuantResultsForm.ScrollTableRowInView(Constants.DefaultOrderOfSameItemsToReturn);
            var sharesPositionsData = pureQuantResultsForm.GetColumnValuesAsListOfString(PureQuantResultsColumnTypes.Shares)
                .Take(markedPositionsQuantity).ToList();
            Checker.IsTrue(sharesPositionsData.Any(), "No kept data for Shares");
            var positionSizeMoneyPositionsData = pureQuantResultsForm.GetColumnValuesAsListOfString(PureQuantResultsColumnTypes.PositionSizeMoney)
                .Take(markedPositionsQuantity).ToList();
            Checker.IsTrue(positionSizeMoneyPositionsData.Any(), "No kept data for Position Size Money");
            var rankPositionsData = pureQuantResultsForm.GetColumnValuesAsListOfString(PureQuantResultsColumnTypes.Rank)
                .Take(markedPositionsQuantity).ToList();
            Checker.IsTrue(rankPositionsData.Any(), "No kept data for Rank");

            LogStep(16, 17, "Select saving to 'Existing Portfolio' and select the portfolioName from precondition. Click 'Save'");
            new PureQuantSteps().SelectExistedPortfolioClickAddPortfolio(portfolioModel.Name);

            LogStep(18, "Make sure number of positions as expected. Make sure positions(detected by symbol) as expected.");
            var positionsTab = new PositionsTabForm();
            var positionsData = positionsTab.GetPositionDataForAllPositions(columnsToCollectData);
            Checker.CheckEquals(markedPositionsQuantity, positionsData.Count,
                "Saved positions quantity is not as expected");
            var tickersInGrid = positionsData.Select(t => t.Ticker).ToList();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(tickersInGrid, markedTickers),
                $"Positions on Positions Tab is not as expected\n{GetExpectedResultsString(markedTickers)}\r\n{GetActualResultsString(tickersInGrid)}");

            LogStep(18, "Make sure positions (detected by symbol) as expected for.");
            var actualShares = positionsData.Select(t => t.Shares).ToList();
            var expectedShares = sharesPositionsData.Select(t => Constants.DecimalNumberWithIntegerPossibilityRegex.Match(t).Value.ToString()).ToList();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(actualShares, expectedShares),
                $"Shares on Positions Tab is not as expected\n{GetExpectedResultsString(expectedShares)}\r\n{GetActualResultsString(actualShares)}");

            var actualValues = positionsData.Select(t => Constants.NumbersWithCommaForThousandAndWithDecimalsRegex.Match(t.Value).Value).ToList();
            var expectedValues = positionSizeMoneyPositionsData.Select(t => Constants.NumbersWithCommaForThousandAndWithDecimalsRegex.Match(t).Value).ToList();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(actualValues, expectedValues),
                $"Values on Positions Tab is not as expected\n{GetExpectedResultsString(expectedValues)}\r\n{GetActualResultsString(actualValues)}");

            var actualNotes = positionsData.Select(t => t.Notes).ToList();
            var expectedNotes = rankPositionsData.Select(t => string.Format(Constants.WordingForPureQuantNotes, t.Trim(),
                Parsing.ConvertToShortDateString(DateTime.Now.AsShortDate()))).ToList();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(actualNotes, expectedNotes),
                $"Notes on Positions Tab is not as expected\n{GetExpectedResultsString(expectedNotes)}\r\n{GetActualResultsString(actualNotes)}");
        }

        private void SelectItemsInSourceAndCheckIt(PureQuantDataSourceTypes source, List<string> items)
        {
            var pureQuantStep1Form = new PureQuantStep1Form();
            pureQuantStep1Form.SelectPortfoliosMultiple(source, items);
            var actualSelectedItems = pureQuantStep1Form.GetSelectedSourcesFromMultipleSelectionDropDown(source);
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(items, actualSelectedItems),
                $"After selecting {source.GetStringMapping()} dropdown has unexpected item: {GetExpectedResultsString(items)}\n{GetActualResultsString(actualSelectedItems)}");
        }
    }
}