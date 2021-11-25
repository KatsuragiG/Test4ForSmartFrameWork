using System.Linq;
using System.Collections.Generic;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Alerts;
using AutomatedTests.Enums.Events;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.Sorting;
using AutomatedTests.Enums.User;
using AutomatedTests.Enums;
using AutomatedTests.Forms.MyPortfolios;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.PositionsAlertsForm.Models;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Models.EventsModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Events;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests.Steps.Settings;
using AutomatedTests.Utils;
using AutomatedTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._03_MyPortfolios._04_PortfolioGrid
{
    [TestClass]
    public class TC_0284_PortfoliosGrid_DuplicateSyncPortfolio : BaseTestUnitTests
    {
        private const int TestNumber = 284;

        private string optionAlert;
        private string stockAlert;
        private string alertType;
        private string alertInEventDescription;
        private string defaultExitCommission;
        private string defaultEntryCommission;
        private PortfolioDBModel initialPortfolioData;
        private List<PositionGridModel> positionsDataInitial;
        private List<PositionsGridDataField> userViewColumns;
        private List<bool> alertsStatusesInitial;
        private List<string> alertsDescriptionsInitial;
        private List<string> alertsSymbolsInitial;
        private List<int> positionsIds;        
        private List<int> positionsIdsAlertsTab;
        private int portfolioId;

        [TestInitialize]
        public void TestInitialize()
        {
            var userType = GetTestDataParsedAsEnumFromStringMapping<ProductSubscriptionTypes>("User");
            optionAlert = GetTestDataAsString(nameof(optionAlert));
            stockAlert = GetTestDataAsString(nameof(stockAlert));
            alertType = GetTestDataAsString(nameof(alertType));
            alertInEventDescription = GetTestDataAsString(nameof(alertInEventDescription));
            defaultEntryCommission = GetTestDataAsString(nameof(defaultEntryCommission));
            defaultExitCommission = GetTestDataAsString(nameof(defaultExitCommission));

            LogStep(0, "Precondition - Login as registered user");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, (ProductSubscriptions)(int)userType));

            new SettingsSteps().LoginNavigateToSettingsAlertsSetAlertsForStockOptionSave(UserModels.First(),
                ToggleStates.Yes.GetStringMapping().Equals(stockAlert), ToggleStates.Yes.GetStringMapping().Equals(optionAlert), 
                alertType, string.Empty);

            PortfoliosSetUp.ImportDagSiteInvestment06(true);
            var portfoliosQueries = new PortfoliosQueries();
            portfolioId = portfoliosQueries.SelectPortfolioIdForLastPortfolioByUserEmail(UserModels.First().Email);
            portfoliosQueries.UpdateCommissionInPortfoliosByPortfolioId(defaultEntryCommission, defaultExitCommission, portfolioId);
            initialPortfolioData = portfoliosQueries.SelectPortfolioDataByPortfolioId(portfolioId);

            var mainMenuNavigation = new MainMenuNavigation();
            mainMenuNavigation.OpenPositionsGrid();
            new PositionsAlertsStatisticsPanelForm().ActivateTab(PositionsTabs.OpenPositions);
            var positionsTabForm = new PositionsTabForm();
            userViewColumns = UserModels.First().SubscriptionType.Contains(ProductSubscriptionTypes.TSBasic)
                ? Instance.GetListOfDefaultViewPositionsBasicColumns()
                : Instance.GetListOfDefaultViewPositionsColumnsForPremium();
            positionsDataInitial = positionsTabForm.GetPositionDataForAllPositions(userViewColumns);
            positionsIds = positionsTabForm.GetPositionsIds();
            new MyPortfoliosMenuForm().ClickMyPortfoliosMenuItem(MyPortfoliosMenuItems.AlertsGrid);
            var alertsTabForm = new AlertsTabForm();
            alertsTabForm.ClickOnPositionColumnToSort(AlertsGridColumnsDataField.Ticker, SortingStatus.Asc);
            alertsStatusesInitial = alertsTabForm.GetValuesOfAlertStatusColumn();
            alertsDescriptionsInitial = alertsTabForm.GetColumnValues(AlertsGridColumnsDataField.AlertDescription);
            positionsIdsAlertsTab = alertsTabForm.GetPositionsIds();
            alertsSymbolsInitial = alertsTabForm.GetColumnValues(AlertsGridColumnsDataField.Ticker).Select(t => t.Split('\r')[0]).ToList();
            mainMenuNavigation.OpenPortfolios();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_284$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PortfoliosPage"), TestCategory("PortfoliosPageActionsPopup"), TestCategory("SyncPortfolio")]
        [Description("https://tr.a1qa.com/index.php?/cases/view/19232752 The test checks correctness of working Duplicate portfolio functionality for sync portfolio")]
        public override void RunTest()
        {
            LogStep(1, "Open portfolio grid - Open positions ");
            var portfoliosForm = new PortfoliosForm();
            portfoliosForm.SelectShowPortfolioStatsFor(PortfolioStatisticForTypes.OpenPositions);

            LogStep(2, "Remember quantity of opened positions and alerts for all sync portfolios, Cash, AVG Day Held, % gain");
            var portfolioGridsSteps = new PortfolioGridsSteps();
            var portfolioInfoInitial = portfolioGridsSteps.RememberPortfolioInformationForPortfolioId(portfolioId);

            LogStep(3, 4, "Click Duplicate button ");
            portfoliosForm.SelectPortfolioContextMenuOption(portfolioId, PortfolioContextNavigation.DuplicatePortfolio);

            LogStep(5, "Check and remember prepopulated values on the popup");
            var duplicatePortfolioPopup = new DuplicatePortfolioPopup();
            var expectedName = $"{initialPortfolioData.Name} - Duplicate";
            var duplicatePortfolioPopupSteps = new DuplicatePortfolioPopupSteps();
            duplicatePortfolioPopupSteps.CheckDuplicatePortfolioInfo(initialPortfolioData, portfolioInfoInitial.Currency, expectedName, isNoteEmpty: true);

            LogStep(6, "Click Save");
            duplicatePortfolioPopup.ClickSave();

            LogStep(7, "Click Edit for duplicate portfolio");
            var portfoliosQueries = new PortfoliosQueries();
            var duplicatedPortfolioId = portfoliosQueries.SelectPortfolioIdForLastPortfolioByUserEmail(UserModels.First().Email);
            Browser.GetDriver().Navigate().Refresh();
            portfoliosForm.SelectPortfolioContextMenuOption(duplicatedPortfolioId, PortfolioContextNavigation.DuplicatePortfolio);

            LogStep(8, "Check  values on the popup:");
            duplicatePortfolioPopup = new DuplicatePortfolioPopup();
            duplicatePortfolioPopupSteps.CheckDuplicatePortfolioInfo(initialPortfolioData, portfolioInfoInitial.Currency, expectedName);
            duplicatePortfolioPopup.ClickCancel();

            LogStep(9, "Compare quantity of opened positions, AVG Day Held, % gainfor the duplicate portfolio with initial(step 4) in the grid ");
            var portfolioInfoDuplicated = portfolioGridsSteps.RememberPortfolioInformationForPortfolioId(duplicatedPortfolioId);
            Checker.CheckEquals(portfolioInfoInitial.Positions, portfolioInfoDuplicated.Positions, "Number of positions is not equals");
            Checker.CheckEquals(portfolioInfoInitial.AvgDayHeld, portfolioInfoDuplicated.AvgDayHeld, "AVG Day Held is not the same");
            Checker.CheckEquals(portfolioInfoInitial.Gain, portfolioInfoDuplicated.Gain, "% gain is not the same");

            LogStep(10, "Click on the portfolio name of the duplicated portfolio");
            portfoliosForm.ClickOnPortfolioName(expectedName);

            LogStep(11, "Check that position Name, position Symbol, position Status, positions Entry Date for all open position are same " +
                "as for step 4 of precondition for this portfolio");
            var positionsAlertsStatisticsPanelForm = new PositionsAlertsStatisticsPanelForm();
            positionsAlertsStatisticsPanelForm.ActivateTab(PositionsTabs.OpenPositions);
            var positionsDataDuplicated = new PositionsTabForm().GetPositionDataForAllPositions(userViewColumns);
            Checker.IsTrue(positionsDataDuplicated.Any(), "There are no positions in grid");
            for (var i = 0; i < positionsDataDuplicated.Count; i++)
            {
                Checker.IsTrue(ObjectComparator.ArePositionsEqualsWithoutId(positionsDataInitial[i], positionsDataDuplicated[i]),
                    $"position with id {positionsDataInitial[i].Id} is not equal");
            }

            LogStep(12, "Click Alerts tab");
            new MyPortfoliosMenuForm().ClickMyPortfoliosMenuItem(MyPortfoliosMenuItems.AlertsGrid);

            LogStep(13, "Check that  alert status and alert description for all open position are same as for step 4 of precondition");
            var alertsTabForm = new AlertsTabForm();
            alertsTabForm.ClickOnPositionColumnToSort(AlertsGridColumnsDataField.Ticker, SortingStatus.Asc);

            var alertsStatusesDuplicated = alertsTabForm.GetValuesOfAlertStatusColumn();
            var alertsDescriptionsDuplicated = alertsTabForm.GetColumnValues(AlertsGridColumnsDataField.AlertDescription);

            Asserts.Batch(
                () =>
                Assert.IsTrue(BaseObjectComparator.AreListsEquals(alertsStatusesInitial, alertsStatusesDuplicated), "Alert status for all open position are not the same " +
                    "as for step 4 of precondition"),
                () =>
                Assert.IsTrue(BaseObjectComparator.AreListsEquals(alertsDescriptionsInitial, alertsDescriptionsDuplicated), "Alert description for all open position " +
                    "are not the same as for step 4 of precondition")
                );

            LogStep(14, 15, "Click Events. Select the duplicated portfolio in dropdowns");
            var eventsForm = new EventsSteps().OpenEventsSelectPortfolioByNameGetEventsForm(expectedName);
            eventsForm.SelectPeriod(GridFilterPeriods.Last7Days);
            eventsForm.SelectEventType(EventTypes.CopyPosition);

            LogStep(16, "Check that event grid contains events for Copy position for *all* recognized positions ");
            var positionQueries = new PositionsQueries();
            foreach (var positionId in positionsIds)
            {
                var ticker = positionQueries.SelectSymbolByPositionId(positionId);
                var eventModel = new EventsModel
                {
                    Symbol = ticker,
                    PositionName = positionQueries.SelectPositionName(positionId),
                    EventType = EventTypes.CopyPosition.GetStringMapping(),
                    Description = $"{ticker} position copied (from {portfolioInfoInitial.PortfolioName} to {expectedName})"
                };
                Checker.IsTrue(eventsForm.IsEventGridContainsCertainEvent(eventModel), $"Grid does not contain event for Copy position for {eventModel.Symbol}");
            }

            LogStep(17, "Check that event grid contains events for Alert created for *all* alerts");
            eventsForm.SelectEventType(EventTypes.AlertCreated);
            for (var i = 0; i < positionsIdsAlertsTab.Count; i++)
            {
                var eventModel = new EventsModel
                {
                    Symbol = alertsSymbolsInitial[i],
                    PositionName = positionQueries.SelectPositionName(positionsIdsAlertsTab[i]),
                    EventType = EventTypes.AlertCreated.GetStringMapping(),
                    Description = $"Alert created for {alertsSymbolsInitial[i]} ({alertInEventDescription})"
                };
                Checker.IsTrue(eventsForm.IsEventGridContainsCertainEvent(eventModel), $"Grid does not contain event for Alert created for {eventModel.Symbol}");
            }

            LogStep(18, "Compare both portfolios in DB");
            var initialPortfolio2 = portfoliosQueries.SelectPortfolioDataByPortfolioId(portfolioId);
            var duplicatedPortfolio = portfoliosQueries.SelectPortfolioDataByPortfolioId(duplicatedPortfolioId);
            Assert.IsTrue(ObjectComparator.ArePortfoliosMainInfromationInDbEqual(initialPortfolio2, duplicatedPortfolio), "Portfolios in DB are not equal");
        }
    }
}