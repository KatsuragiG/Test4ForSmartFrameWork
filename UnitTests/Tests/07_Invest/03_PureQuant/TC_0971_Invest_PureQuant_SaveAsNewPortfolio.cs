using System;
using System.Collections.Generic;
using System.Linq;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.Tools;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.ResearchPages.PureQuant;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests.Steps.ResearchPages;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._07_Invest._03_PureQuant
{
    [TestClass]
    public class TC_0971_Invest_PureQuant_SaveAsNewPortfolio : BaseTestUnitTests
    {
        private const int TestNumber = 971;
        private readonly List<PositionsGridDataField> columnsToCollectData = new List<PositionsGridDataField>
        {
            PositionsGridDataField.Shares, PositionsGridDataField.Notes, PositionsGridDataField.Value, PositionsGridDataField.Ticker
        };
        private const string TextToExcludeFromSharesValues = " shares";

        private PortfolioModel portfolioModel;
        private readonly List<PositionsDBModel> positionsModels = new List<PositionsDBModel>();
        private int positionsQuantity;
        private string customPortfolioName;
        private List<string> positionsSymbols;

        [TestInitialize]
        public void TestInitialize()
        {
            portfolioModel = new PortfolioModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataAsString("PortfolioType").ParseAsEnumFromStringMapping<PortfolioType>(),
                Currency = GetTestDataAsString("Currency")
            };
            positionsQuantity = GetTestDataAsInt("positionQuantity");
            for (int i = 1; i <= positionsQuantity; i++)
            {
                positionsModels.Add(new PositionsDBModel
                {
                    Symbol = GetTestDataAsString($"Symbol{i}"),
                    TradeType = $"{(int)PositionTradeTypes.Long}"
                });
            }
            customPortfolioName = StringUtility.RandomString("Portfolio#######");
            positionsSymbols = positionsModels.Select(m => m.Symbol).ToList();

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsLifetime));

            PortfoliosSetUp.AddWatchOnlyPortfolio(UserModels.First().Email);
            var portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            foreach (var position in positionsModels)
            {
                PositionsAlertsSetUp.AddPositionViaDB(portfolioId, position);
            }

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
            new MainMenuNavigation().OpenPositionsGrid();
            new PositionsAlertsStatisticsPanelForm().SelectPortfolio(portfolioModel.Name);
            var positionTab = new PositionsTabForm();
            positionTab.ClickEditSignForCurrentView();
            positionTab.CheckCertainCheckboxes(columnsToCollectData.Where(t => t != PositionsGridDataField.Ticker).Select(t => t.GetStringMapping()).ToList());
            positionTab.ClickSaveViewButton();
            new MainMenuNavigation().OpenPureQuant(PureQuantInternalSteps.Step1ChooseSources);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_971$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("ResearchPage"), TestCategory("PureQuantTool"), TestCategory("AddPortfolio")]
        [Description("The test checks possibility saving rebalanced results as a new portfolio. https://tr.a1qa.com/index.php?/cases/view/19232170")]
        public override void RunTest()
        {
            LogStep(1, "Select manual portfolio on the dropdown 'Select a portfolio'.");
            var pureQuantSteps = new PureQuantSteps();
            pureQuantSteps.ClearPredefinedSelectPortfolioClickRunResearch(portfolioModel.Name);

            LogStep(2, "Click 'Run Research' button. Wait Result Appearing. Remember grid data'");
            var pureQuantResultsForm = new PureQuantResultsForm();
            Checker.CheckEquals(positionsSymbols.Count, pureQuantResultsForm.GetNumberOfSelectedPositions(),
                "Positions on Pure Quant grid after calculations are not as expected");
            var pureQuantResultData = pureQuantResultsForm.GetPureQuantGrid();

            LogStep(3, "Select saving data in new portfolio");
            pureQuantSteps.SelectNewPortfolioAddingClickAddPortfolio(customPortfolioName);

            LogStep(4, "Make sure name of selected portfolio as expected.");
            Checker.CheckEquals(customPortfolioName, new PositionsAlertsStatisticsPanelForm().GetPortfolioName(),
                "Name of selected portfolio does not equal set at popup");

            LogStep(5, "Make sure number of positions as expected. Make sure positions(detected by symbol) as expected.");
            var positionsTab = new PositionsTabForm();
            var positionsData = positionsTab.GetPositionDataForAllPositions(columnsToCollectData);
            var tickersInGrid = positionsData.Select(t => t.Ticker).ToList();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(tickersInGrid, positionsSymbols),
                $"Positions on Positions Tab is not as expected\n{GetExpectedResultsString(positionsSymbols)}\r\n{GetActualResultsString(tickersInGrid)}");

            var actualShares = positionsData.Select(t => t.Shares).ToList();
            var expectedShares = pureQuantResultData.Select(t => t.Shares.Replace(TextToExcludeFromSharesValues, "")).ToList();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(actualShares, expectedShares),
                $"Shares on Positions Tab is not as expected\n{GetExpectedResultsString(expectedShares)}\r\n{GetActualResultsString(actualShares)}");

            var actualValues = positionsData.Select(t => Constants.NumbersWithCommaForThousandAndWithDecimalsRegex.Match(t.Value).Value).ToList();
            var expectedValues = pureQuantResultData.Select(t => Constants.NumbersWithCommaForThousandAndWithDecimalsRegex.Match(t.PositionSizeMoney).Value).ToList();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(actualValues, expectedValues),
                $"Values on Positions Tab is not as expected\n{GetExpectedResultsString(expectedValues)}\r\n{GetActualResultsString(actualValues)}");

            var actualNotes = positionsData.Select(t => t.Notes).ToList();
            var expectedNotes = pureQuantResultData.Select(t => string.Format(Constants.WordingForPureQuantNotes, t.PureQuantRank.Trim(), 
                Parsing.ConvertToShortDateString(DateTime.Now.AsShortDate()))).ToList();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(actualNotes, expectedNotes),
                $"Notes on Positions Tab is not as expected\n{GetExpectedResultsString(expectedNotes)}\r\n{GetActualResultsString(actualNotes)}");

            LogStep(6, "Open Portfolios page -> Open Positions tab");
            new MainMenuNavigation().OpenPortfolios();
            var portfoliosForm = new PortfoliosForm();

            LogStep(7, "Make sure name of calculated portfolio as expected.");
            var portfolioIdNew = new PortfoliosQueries().SelectPortfolioIdForLastPortfolioByUserEmail(UserModels.First().Email);
            var portfolioGridsSteps = new PortfolioGridsSteps();
            Checker.CheckEquals(customPortfolioName, portfolioGridsSteps.RememberPortfolioInformationForPortfolioId(portfolioIdNew).PortfolioName,
                "Name of calculated portfolio does not equal set in popup");

            LogStep(8, "Make sure type of calculated portfolio as expected.");
            Checker.IsTrue(portfoliosForm.IsTypeOfPortfolioAsExpected(portfolioIdNew, PortfolioType.WatchOnly),
                "Type of portfolio does not equal set in popup. Portfolio is not in correct section.");

            LogStep(9, "Make sure expected number of positions shown on column 'Positions'.");
            Checker.CheckEquals(positionsSymbols.Count, portfolioGridsSteps.GetNumberOfPositionsViaPortfolioId(portfolioIdNew),
                "Number of positions on Portfolios is not as expected");
        }
    }
}