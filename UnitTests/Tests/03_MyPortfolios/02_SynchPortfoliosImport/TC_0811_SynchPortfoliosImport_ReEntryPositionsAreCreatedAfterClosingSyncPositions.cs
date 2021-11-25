using System;
using System.Collections.Generic;
using System.Linq;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Alerts;
using AutomatedTests.Database.Positions;
using AutomatedTests.Database.Users;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Settings;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._03_MyPortfolios._02_SynchPortfoliosImport
{
    [TestClass]
    public class TC_0811_SynchPortfoliosImport_ReEntryPositionsAreCreatedAfterClosingSyncPositions : BaseTestUnitTests
    {
        private const int TestNumber = 811;

        private string createdReEntryAlerts;
        private int yearsQuantityToCustomDateRange;
        private List<string> portfolioNames;

        [TestInitialize]
        public void TestInitialize()
        {
            portfolioNames = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(portfolioNames));
            createdReEntryAlerts = GetTestDataAsString(nameof(createdReEntryAlerts));
            yearsQuantityToCustomDateRange = GetTestDataAsInt(nameof(yearsQuantityToCustomDateRange));

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPlus));

            var settingsSteps = new SettingsSteps();
            settingsSteps.LoginNavigateToSettingsPositionGetForm(UserModels.First());
            settingsSteps.ChangeReEntryForSynchronizedPortfolioStateWithSaving(true);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_811$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("PortfoliosPage"), TestCategory("SyncPortfolio"), TestCategory("SyncPositions"), TestCategory("PositionsGridPositionActionsPopup")]
        [Description("Test checks that ReEntry Positions Are Created After Closing Sync Positions. https://tr.a1qa.com/index.php?/cases/view/19232297")]
        public override void RunTest()
        {
            LogStep(1, "Import portfolio");
            PortfoliosSetUp.ImportDagSiteInvestment15(true);
            new PortfoliosForm().AssertIsOpen();

            LogStep(2, "Remember portfolioId"); 
            var portfolioId = new UsersQueries().SelectAllUserPortfolioIds(UserModels.First().TradeSmithUserId).First();

            LogStep(3, "Click Edit sign for the portfolio. Click Update Credentials button.Change creds for Investments with creds Account20 / Password20. Close success popup");
            PortfoliosSetUp.UpdatePortfolioWithDagSiteInvestment20(portfolioId);

            LogStep(4, "Open Positions & Alerts page -> Closed Positions tab");
            new MainMenuNavigation().OpenPositionsGrid(PositionsTabs.ClosedPositions);

            LogStep(5, "Select first portfolio");
            var positionsAlertsStatisticsPanelForm = new PositionsAlertsStatisticsPanelForm();
            positionsAlertsStatisticsPanelForm.SelectPortfolio(portfolioNames.First());

            LogStep(6, "Select custom date range");
            var closedPositionsTab = new ClosedPositionsTabForm();
            closedPositionsTab.SelectCustomPeriodRangeWithStartEndDates(DateTime.Now.AddYears(-yearsQuantityToCustomDateRange).ToShortDateString(), 
                DateTime.Now.ToShortDateString());

            LogStep(7, "Remember Position's ids for appropriate positions in the grid");
            var positionsOrdersWithHealthStatuses = closedPositionsTab.GetSsiColumnValues()
                .Select((ssi, order) => (ssi, order))
                .Where(pair => !pair.ssi.Equals(HealthZoneTypes.NotAvailable.GetStringMapping()))
                .Select(pair => pair.order);
            var positionsIdsWithSsi = closedPositionsTab.GetPositionsIds()
                .Select((id, order) => (id, order))
                .Where(pair => positionsOrdersWithHealthStatuses.Contains(pair.order))
                .Select(pair => pair.id).ToList();
            Assert.IsTrue(positionsIdsWithSsi.Count > 0, "There are no stocks on Closed Positions Grid");

            LogStep(8, "Remember appropriate Position's symbols");
            var positionsQueries = new PositionsQueries();
            var positionsSymbolsOnClosedGrid = positionsIdsWithSsi.Select(id => positionsQueries.SelectSymbolByPositionId(id)).ToList();

            LogStep(9, "Select the 'Re - Entry Watchlist' from the portfolio dropdown. Open Positions grid ");
            positionsAlertsStatisticsPanelForm.ActivateTab(PositionsTabs.OpenPositions);
            positionsAlertsStatisticsPanelForm.SelectPortfolio(AllPortfoliosKinds.ReEntryWatchlist.GetStringMapping());

            LogStep(10, "Remember Position's ids for the all positions in the grid");
            var positionsTabForm = new PositionsTabForm();
            var reEntryIds = positionsTabForm.GetPositionsIds();
            Assert.IsTrue(reEntryIds.Count > 0, "There are no positions in Re-Entry Watchlist");

            LogStep(11, "Remember Position's symbols for the all positions in the grid");
            var positionsSymbolsOnReEntry = reEntryIds.Select(t => positionsQueries.SelectSymbolByPositionId(t)).ToList();

            LogStep(12, "Check that all positions in the grid have.- Entry Date = Today.- Shares = 0.- Entry Price = Latest Close");
            foreach (int stockId in reEntryIds)
            {
                var tableMetric = positionsTabForm.GetTickerTableCellMetric(stockId, PositionsGridDataField.EntryDate);
                Checker.CheckEquals(Parsing.ConvertToShortDateString(DateTime.Now.ToShortDateString()),
                    Parsing.ConvertToShortDateString(positionsTabForm.GetPositionsGridCellValue(tableMetric)),
                    "Entry Date is not Today");
                tableMetric = positionsTabForm.GetTickerTableCellMetric(stockId, PositionsGridDataField.Shares);
                Checker.CheckEquals(Constants.DefaultStringZeroDecimalValue, positionsTabForm.GetPositionsGridCellValue(tableMetric),
                    "Shares is not 0");
                tableMetric = positionsTabForm.GetTickerTableCellMetric(stockId, PositionsGridDataField.EntryPrice);
                Checker.CheckEquals(positionsTabForm.GetPositionsGridCellValue(tableMetric),
                    positionsTabForm.GetPositionsGridCellValue(positionsTabForm.GetTickerTableCellMetric(stockId, PositionsGridDataField.LatestClose)),
                    "Entry Price does not equal Latest Close");
            }

            LogStep(13, "Compare positions lists from steps 8 and 11");
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(positionsSymbolsOnClosedGrid, positionsSymbolsOnReEntry),
                $"Positions symbols are not matched: {GetExpectedResultsString(positionsSymbolsOnClosedGrid)}\r\n{ GetActualResultsString(positionsSymbolsOnReEntry)}");

            LogStep(14, "Check in DB (alert type) and UI that all positions in the grid does not have any alerts");
            var alertsQueries = new AlertsQueries();
            for (int i = 0; i < reEntryIds.Count; i++)
            {
                Checker.IsFalse(alertsQueries.SelectAlertsDataByPositionId(reEntryIds[i]).Any(),
                    "Position have some alerts in DB unexpectedly");
                Checker.CheckEquals(createdReEntryAlerts, positionsTabForm.GetAlertQuantityForPositionByOrder(i + 1),
                    "Position have alert on UI");
            }
        }
    }
}