using System.Collections.Generic;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.BaseTestsUnitTests;
using System.Linq;
using TradeStops.Common.Enums;
using AutomatedTests.Navigation;
using AutomatedTests.Enums.Sorting;
using AutomatedTests.Models.PortfoliosModels;
using AutomatedTests.Enums.Positions;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Forms.SyncFlow;
using AutomatedTests.Utils;
using System;

namespace UnitTests.Tests._03_MyPortfolios._02_SynchPortfoliosImport
{
    [TestClass]
    public class TC_0080_SynchPortfoliosImport_RestorePortfolio : BaseTestUnitTests
    {
        private const int TestNumber = 80;
        private const int PositionOrderInSecondPortfolio = 1;

        private List<string> positionsInfosFromFirstPortfolio;
        private List<int> importedPortfoliosIds;
        private List<string> expectedPortfoliosName = new List<string>();
        private readonly List<PortfoliosListRowModel> portfoliosInfos = new List<PortfoliosListRowModel>();
        private int step = 1;

        [TestInitialize]
        public void TestInitialize()
        {
            expectedPortfoliosName = GetTestDataValuesAsListByColumnName("portfoliosName");

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsLifetime));
            LoginSetUp.LogIn(UserModels.First());

            PortfoliosSetUp.ImportDagSiteInvestment13(true);
            new MainMenuNavigation().OpenInvestmentPortfoliosTab();
            importedPortfoliosIds = new PortfoliosForm().GetPortfoliosIds();
            Assert.IsTrue(importedPortfoliosIds.Count > 0, "portfoliosIds.Count==0. Maybe, it is because import failed");
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_78$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("PortfoliosPage"), TestCategory("Smoke"), TestCategory("SyncPortfolio"), TestCategory("SyncPortfolioUpdate")]
        [Description("Test checks availability of the restore portfolio feature for synch portfolio: https://tr.a1qa.com/index.php?/cases/view/19232922")]
        public override void RunTest()
        {            
            LogStep(step++, "Sort the Portfolios grid by Portfolio Name asc.Check that three portfolios are imported ");
            var portfolioGridsSteps = new PortfolioGridsSteps();
            portfolioGridsSteps.GetPortfolioNamesCompareWithExpected(expectedPortfoliosName);
            var createdDate = Parsing.ConvertToShortDateString(DateTime.Now.ToShortDateString());

            LogStep(step++, "Remember: - Positions number in each portfolio.");
            foreach (var importedPortfoliosId in importedPortfoliosIds)
            {
                portfoliosInfos.Add(portfolioGridsSteps.RememberPortfolioInformationForPortfolioId(importedPortfoliosId));
            }

            DoStepsFrom3To6();

            LogStep(step++, "Open My Portfolios page -> Manage tab. Open the second portfolio(xx4328).");
            var positionsAlertsStatisticsPanelForm = new PositionsAlertsStatisticsPanelForm();
            positionsAlertsStatisticsPanelForm.SelectPortfolio(expectedPortfoliosName[1]);
            Checker.CheckEquals(expectedPortfoliosName[1], positionsAlertsStatisticsPanelForm.GetPortfolioName(),
                "7 Selected portfolio is not same as expected");

            LogStep(step++, "Order the positions by Ticker column asc. Remember Ticker for the first position.");
            var positionsTab = new PositionsTabForm();
            positionsTab.ClickOnPositionColumnToSort(PositionsGridDataField.Ticker, SortingStatus.Asc);
            var positionsInfosFromSecondPortfolio = positionsTab.GetPositionColumnValues(PositionsGridDataField.Ticker);

            LogStep(step++, "Tick the first position in the portfolio. Make sure the number of 'Selected Items' in the footer is 1.");
            positionsTab.SelectItemCheckboxByOrderAndState(PositionOrderInSecondPortfolio, true);
            Checker.CheckEquals(PositionOrderInSecondPortfolio, positionsTab.GetSelectedItemsNumberFromFooter(),
                "9 Number of 'Selected Items' in the footer is not as expected");

            LogStep(step++, "Click Delete button. Confirm deleting in Confirm popup. Click OK in Success popup.");
            portfolioGridsSteps.ClickPositionsGroupActionButtonDoubleConfirm(PositionsGroupAction.Delete);

            LogStep(step++, "Tick all positions in the portfolio. Make sure the number of 'Selected Items' in the footer is 5.");
            positionsTab.SelectAllItemsInGrid();
            Checker.CheckEquals(positionsInfosFromSecondPortfolio.Count - PositionOrderInSecondPortfolio, positionsTab.GetSelectedItemsNumberFromFooter(),
                "11 Number of 'Selected Items' in the footer is not as expected");

            LogStep(step++, "Open My Portfolios page -> Manage tab. Click Refresh button in the grid(3rd column) for any portfolio. " +
                "Make sure that Vendor Account Identifier is the same as in the URL.");
            DoStep12();

            LogStep(step++, step++, "Select 'Restore positions' option. Choose portfolio from the step 3(xx4327) in the 'Apply For' drop - down. Click Synchronize." +
                "Wait until the refresh ends. Click Next or wait for 15 seconds to automatic redirection to the My Portfolios->Manage tab.");
            new ImportPortfoliosSteps().SelectPortfolioInApplyForClickRestorePositionsClickNexIfPresent();
            var portfoliosForm = new PortfoliosForm();

            LogStep(step++, "Sort the Portfolios grid by Portfolio Name asc. Check the number of positions for the first portfolio(xx4327).");
            portfolioGridsSteps.GetPortfolioNamesCompareWithExpected(expectedPortfoliosName);
            var portfolioPositionsData = portfoliosForm.GetValuesOfPortfolioColumn(PortfolioType.Investment, PortfolioGridColumnTypes.Positions);
            Checker.CheckEquals(portfoliosInfos.First().Positions, portfolioPositionsData[0],
                "The number of positions for the first portfolio (xx4327) is the same as in the step 2");
            Checker.CheckEquals(createdDate,
                portfoliosForm.GetValuesOfPortfolioColumn(PortfolioType.Investment, PortfolioGridColumnTypes.CreatedDate).First(),
                "Created date is not the same with current date");

            LogStep(step++, "Check the number of positions for the second portfolio (xx4328).");
            Checker.CheckEquals(portfoliosInfos[1].Positions, portfolioPositionsData[1],
                "The number of positions for the second portfolio (xx4328) is the same as in the step 2");

            LogStep(step++, "Repeat the steps 3 - 6.");
            DoStepsFrom3To6();

            LogStep(step++, "Repeat the steps 12.");
            DoStep12();

            LogStep(step++, "Select 'Restore positions' option. Make sure 'All Portfolios' option is active in the 'Apply For' drop - down. Click Synchronize.");
            var syncFlowEditForm = new SyncFlowEditForm();
            Checker.CheckEquals(Constants.DefaultApplyForPortfolioDropdownItem, syncFlowEditForm.GetPortfolioInApplyFor(),
                "All Portfolios' option is NOT active in the 'Apply For' drop-down");

            LogStep(step++, "Wait until the refresh ends. Click Next or wait for 15 seconds to automatic redirection to the My Portfolios->Manage tab..");
            syncFlowEditForm.ClickRestorePositionsClickNexIfPresent();
            portfoliosForm.AssertIsOpen();

            LogStep(step++, "Compare positions number in each portfolio with stored in the step 2.");
            portfolioPositionsData = portfoliosForm.GetValuesOfPortfolioColumn(PortfolioType.Investment, PortfolioGridColumnTypes.Positions);
            for (int i = 0; i < portfolioPositionsData.Count; i++)
            {
                Checker.CheckEquals(portfoliosInfos[i].Positions, portfolioPositionsData[i],
                   $"The number of positions for the portfolio {expectedPortfoliosName[i]} is the same as in the step 2");
            }

            LogStep(step++, "Open the first portfolio in the grid (xx4327). Compare Tickers of the positions in the portfolio with stored in the step 4.");
            portfoliosForm.ClickOnPortfolioName(expectedPortfoliosName[0]);
            var actualTickers = positionsTab.GetPositionColumnValues(PositionsGridDataField.Ticker);
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(positionsInfosFromFirstPortfolio, actualTickers),
               "Tickers of the positions in the first portfolio are the same as stored in the step 4." +
               $"{GetExpectedResultsString(positionsInfosFromFirstPortfolio)}\r\n{ GetActualResultsString(actualTickers)}");

            LogStep(step++, "Open My Portfolios page -> Manage tab. Open the second portfolio(xx4328). Compare the Ticker of the first position with stored in the step 8.");
            positionsAlertsStatisticsPanelForm.SelectPortfolio(expectedPortfoliosName[1]);
            Checker.CheckEquals(expectedPortfoliosName[1], positionsAlertsStatisticsPanelForm.GetPortfolioName(),
                $"{step} Selected portfolio is not same as expected");
            actualTickers = positionsTab.GetPositionColumnValues(PositionsGridDataField.Ticker);
            Checker.CheckListsEquals(positionsInfosFromSecondPortfolio, actualTickers, 
               "Tickers of the positions in the second portfolio are the same as stored in the step 8.");
        }

        private void DoStep12()
        {
            new MainMenuNavigation().OpenInvestmentPortfoliosTab();
            var portfoliosForm = new PortfoliosForm();
            portfoliosForm.ClickOnPortfolioColumnToSort(PortfolioGridColumnTypes.PortfolioName, SortingStatus.Asc);
            var portfolioIdToSync = new PortfoliosQueries().SelectPortfolioId(UserModels.First().Email, expectedPortfoliosName.Last(), PortfolioType.Investment);
            portfoliosForm.SelectPortfolioContextMenuOption(portfolioIdToSync, PortfolioContextNavigation.Synchronize);
        }

        private void DoStepsFrom3To6()
        {
            LogStep(step++, "Open the first portfolio in the grid (xx4327).");
            new PortfoliosForm().ClickOnPortfolioName(expectedPortfoliosName[0]);

            LogStep(step++, "Remember Ticker for all positions.");
            var positionsTab = new PositionsTabForm();
            positionsInfosFromFirstPortfolio = positionsTab.GetPositionColumnValues(PositionsGridDataField.Ticker);

            LogStep(step, "Tick all positions in the portfolio. Make sure the number of 'Selected Items' in the footer is 6.");
            positionsTab.SelectAllItemsInGrid();
            Checker.CheckEquals(positionsInfosFromFirstPortfolio.Count, positionsTab.GetSelectedItemsNumberFromFooter(),
                $"{step++} Number of 'Selected Items' in the footer is not as expected");

            LogStep(step++, "Click Delete button. Confirm deleting in Confirm popup. Click OK in Success popup.");
            new PortfolioGridsSteps().ClickPositionsGroupActionButtonDoubleConfirm(PositionsGroupAction.Delete);
        }
    }
}