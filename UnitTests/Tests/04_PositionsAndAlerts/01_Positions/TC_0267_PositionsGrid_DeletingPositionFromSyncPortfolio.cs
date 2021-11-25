using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Events;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.User;
using AutomatedTests.Enums;
using AutomatedTests.Forms.MyPortfolios;
using AutomatedTests.Forms.Other;
using AutomatedTests.Forms.Popups;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Events;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Enums;
using TradeStops.Common.Extensions;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;
using AutomatedTests.Models;
using AutomatedTests.Steps.PositionsGridSteps;

namespace UnitTests.Tests._04_PositionsAndAlerts._01_Positions
{
    [TestClass]
    public class TC_0267_PositionsGrid_DeletingPositionFromSyncPortfolio : BaseTestUnitTests
    {
        private const int TestNumber = 267;

        private int portfolioId;
        private string portfolioName;
        private IList<int> positionIds;
        private List<int> stockIds;
        private List<int> optionIds;

        [TestInitialize]
        public void TestInitialize()
        {
            var userType = GetTestDataParsedAsEnumFromStringMapping<ProductSubscriptionTypes>("user");

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, (ProductSubscriptions)(int)userType));

            LoginSetUp.LogIn(UserModels.First());
            PortfoliosSetUp.ImportDagSiteInvestment06(true);

            var portfoliosForm = new PortfoliosForm();
            portfolioId = portfoliosForm.GetLastImportedPortfolioId(UserModels.First().Email);
            portfolioName = new PortfolioGridsSteps().RememberPortfolioInformationForPortfolioId(portfolioId).PortfolioName;
            var positionsQueries = new PositionsQueries();
            positionIds = positionsQueries.SelectPositionIdsForPortfolio(portfolioId);
            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);
            new MyPortfoliosMenuForm().ClickMyPortfoliosMenuItem(MyPortfoliosMenuItems.PositionsGrid);
            new PositionsAlertsStatisticsPanelForm().ActivateTab(PositionsTabs.OpenPositions);
            stockIds = positionIds.Where(id => positionsQueries.SelectAssetTypeNameByPositionId(id).EqualsIgnoreCase(PositionAssetTypes.Stock.GetStringMapping())).ToList();
            optionIds = positionIds.Where(id => positionsQueries.SelectAssetTypeNameByPositionId(id).EqualsIgnoreCase(PositionAssetTypes.Option.GetStringMapping())).ToList();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_267$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PositionsGrid"), TestCategory("PositionsGridPositionActionsPopup"), TestCategory("SyncPositions"), TestCategory("SyncPortfolio")]
        [Description("The test checks possibility to delete only one position from grid for Sync portfolio https://tr.a1qa.com/index.php?/cases/view/19232806")]
        public override void RunTest()
        {
            LogStep(1, 7, "Run checks for stock position");
            DoStep1To7(stockIds);

            LogStep(8, "Repeat steps 1-7 for recognized Option position");
            DoStep1To7(optionIds);

            LogStep(9, "Positions&Alerts - Positions tab.Select Imported portfolio in dropdowns.Select All from status dropdown");
            var positionsAlertsStatisticsPanelForm = new PositionsAlertsStatisticsPanelForm();
            positionsAlertsStatisticsPanelForm.SelectPortfolio(portfolioName);
            var positionsTabForm = new PositionsTabForm();
            var positionsGridSteps = new PositionsGridSteps();
            positionsGridSteps.ResetPositionsFilters();

            DoSteps10To11();

            LogStep(12, "Repeat steps 10-11 for another unrecognized position ");
            Browser.Refresh();
            DoSteps10To11();

            LogStep(13, "Check that *unrecognized* position in DB has deleted");
            Checker.CheckEquals(0, new PositionsQueries().SelectImportedPositionsIds(portfolioId).Count, "Unrecognized positions count in DB is not as expected");

            LogStep(14, "Remember remaining positions quantity");
            var remainingPositionsQuantity = positionsTabForm.GetNumberOfRowsInGrid();

            LogStep(15, "Remember remaining positions quantity");
            new MainMenuNavigation().OpenPortfolios();
            new PortfolioGridsSteps().ClickRefreshPortfolioIdViaSyncFlow(portfolioId);

            LogStep(16, "Click on portfolio Link and check positions quantity");
            new PortfoliosForm().ClickOnPortfolioNameViaId(portfolioId);
            positionsGridSteps.ResetPositionsFilters();
            Checker.CheckEquals(remainingPositionsQuantity, positionsTabForm.GetNumberOfRowsInGrid(), "Positions count is not as expected after refresh");
        }

        private void DoSteps10To11()
        {
            LogStep(10, "Click on delete sign for any *unrecognized* position");
            var positionsTabForm = new PositionsTabForm();
            var unrecognizedPositionsRowOrders = positionsTabForm.GetRowsWithUnrecognizedPositions();
            var ticker = positionsTabForm.GetPositionsGridCellValue(
                    new TableCellMetrics { PositionOrder = unrecognizedPositionsRowOrders[0], ColumnHeader = PositionsGridDataField.Ticker.GetStringMapping() })
                .Split('\r')[0];
            positionsTabForm.SelectItemCheckboxByOrderAndState(unrecognizedPositionsRowOrders[0], true);
            positionsTabForm.ClickGroupActionButton(PositionsGroupAction.Delete);

            LogStep(11, "Confirm Deletion");
            new ConfirmPopup(PopupNames.Confirm).ClickYesButton();
            new ConfirmPopup(PopupNames.Success).ClickOkButton();
            Checker.IsFalse(positionsTabForm.GetPositionColumnValues(PositionsGridDataField.Ticker).Select(t => t.Split('\r')[0]).ToList().Contains(ticker),
                $"Grid contains deleted unrecognized position {ticker}");
        }

        private void DoStep1To7(List<int> positionsIds)
        {
            LogStep(1, "Select Imported portfolio in dropdowns");
            var positionsAlertsStatisticsPanelForm = new PositionsAlertsStatisticsPanelForm();
            positionsAlertsStatisticsPanelForm.SelectPortfolio(portfolioName);

            LogStep(2, "Select All from status dropdown");
            var positionsTabForm = new PositionsTabForm();
            new PositionsGridSteps().ResetPositionsFilters();

            LogStep(3, "Click on delete sign for any *recognized* position");
            var positionId = SelectPositionIdRandomly(positionsIds);
            var positionQueries = new PositionsQueries();
            var positionSymbol = positionQueries.SelectPositionData(positionId).Symbol;
            var positionName = positionQueries.SelectPositionName(positionId);
            positionsTabForm.SelectPositionContextMenuOption(positionId, PositionContextNavigation.DeletePosition);

            LogStep(4, "Confirm deletion of position");
            new ConfirmPopup(PopupNames.Confirm).ClickYesButton();
            new ConfirmPopup(PopupNames.Success).ClickOkButton();
            Checker.IsFalse(positionsTabForm.IsPositionPresentInGridById(positionId), $"Position {positionSymbol} is not deleted");

            LogStep(5, "Open URL for position card of deleted position");
            var mainMenuNavigation = new MainMenuNavigation();
            mainMenuNavigation.OpenPositionCard(positionId);
            new Error404Form().AssertIsOpen();

            LogStep(6, "Check that position in DB has corresponded status for deleting ");
            Checker.IsTrue(positionQueries.SelectPositionStatusOfDeleting(positionId), "Position in DB has not corresponded status for deleting ");

            LogStep(7, "Open Settings - Events, Select the Manual portfolio in dropdowns");
            var eventsForm = new EventsSteps().OpenEventsSelectPortfolioByNameSelectEventTypeGetEventsForm(
                new PortfoliosQueries().SelectPortfolioDataByPortfolioId(portfolioId).Name, EventTypes.DeletePosition);
            eventsForm.SelectPeriod(GridFilterPeriods.Last7Days);
            var lastEvent = eventsForm.GetEventByPositionInGrid(1);
            Checker.CheckEquals(EventTypes.DeletePosition.GetStringMapping(), lastEvent.EventType, "Event Type is not Delete position");
            Checker.CheckEquals(positionSymbol, lastEvent.Symbol, $"Symbol is not {positionSymbol}");
            Checker.CheckEquals(positionName, lastEvent.PositionName, $"Position Name is not {positionName}");
            Checker.IsTrue(lastEvent.Description.Contains(positionSymbol) && lastEvent.Description.Contains("Position Deleted"), 
                $"Event description is not as expected for {positionSymbol}: '{lastEvent}'");

            mainMenuNavigation.OpenPositionsGrid();
        }

        private static int SelectPositionIdRandomly(IReadOnlyList<int> positionIds)
        {
            return positionIds[SRandom.Instance.Next(0, positionIds.Count - 1)];
        }
    }
}