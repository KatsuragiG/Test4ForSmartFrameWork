using System.Collections.Generic;
using System.Linq;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Alerts;
using AutomatedTests.Enums.Dashboard;
using AutomatedTests.Enums.Events;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Portfolios.CreateManual;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.Sorting;
using AutomatedTests.Forms.Dashboard;
using AutomatedTests.Forms.MyPortfolios;
using AutomatedTests.Forms.Popups;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Models.EventsModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Events;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._04_PositionsAndAlerts._01_Positions
{
    [TestClass]
    public class TC_0621_PositionsGrid_CopyOnePositionWithSettingsCopyAlertsIsTrue : BaseTestUnitTests
    {
        private const int TestNumber = 621;
        private const int OrderOfPositionForUniqueCopy = 1;

        private AddPortfolioManualModel addPortfolioManualModel;
        private PortfolioModel portfolioDestinationModel;
        private readonly List<PositionAtManualCreatingPortfolioModel> positionsModels = new List<PositionAtManualCreatingPortfolioModel>();
        private List<string> alertsStates;
        private string copyEventDescription;
        private List<string> expectedPositionQty;
        private List<string> positionsTickerNames;
        private List<string> positionsAlertsQuantity;
        private int positionsQuantity;
        private int portfolioIdDestination;

        [TestInitialize]
        public void TestInitialize()
        {
            var portfolioName = GetTestDataAsString("PortfolioName");
            var portfolioType = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType");
            addPortfolioManualModel = new AddPortfolioManualModel
            {
                Name = StringUtility.RandomString(portfolioName),
                Type = portfolioType,
                Currency = GetTestDataAsString("Currency1")
            };
            portfolioDestinationModel = new PortfolioModel
            {
                Name = StringUtility.RandomString(portfolioName),
                Type = portfolioType,
                Currency = GetTestDataAsString("Currency2")
            };

            positionsQuantity = GetTestDataAsInt(nameof(positionsQuantity));
            for (int i = 1; i <= positionsQuantity; i++)
            {
                positionsModels.Add(new PositionAtManualCreatingPortfolioModel
                {
                    Ticker = GetTestDataAsString($"Symbol{i}"),
                    PositionAssetType = GetTestDataParsedAsEnumFromStringMapping<PositionAssetTypes>($"positionType{i}"),
                    EntryDate = GetTestDataAsString($"entryDate{i}"),
                    Quantity = GetTestDataAsString($"positionQty{i}"),
                    EntryPrice = GetTestDataAsString($"entryPrice{i}"),
                    TradeType = GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>($"tradeType{i}")
                });
            }

            var vqSetState = GetTestDataParsedAsEnumFromStringMapping<AlertsToPositionsStates>("vqSetState");
            var tsSetState = GetTestDataParsedAsEnumFromStringMapping<AlertsToPositionsStates>("tsSetState");
            var lcSetState = GetTestDataParsedAsEnumFromStringMapping<AlertsToPositionsStates>("lcSetState");

            alertsStates = GetTestDataValuesAsListByColumnName(nameof(alertsStates));
            expectedPositionQty = GetTestDataValuesAsListByColumnName(nameof(expectedPositionQty));
            copyEventDescription = "{0} position copied (from {1} to {2})";

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, new List<ProductSubscriptions>
                {
                    ProductSubscriptions.TradeStopsPremium, ProductSubscriptions.CryptoStopsPremium
                }
            ));

            portfolioIdDestination = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioDestinationModel);
            LoginSetUp.LogIn(UserModels.First());

            new DashboardForm().ClickActionItem(DashboardWidgetActionItems.TrackMyInvestments);
            new SelectPortfolioFlowForm().SelectPortfolioCreationFlow(AddPortfolioTypes.Manual);

            var manualPortfolioCreationForm = new ManualPortfolioCreationForm();
            manualPortfolioCreationForm.FillPortfolioFields(addPortfolioManualModel);
            manualPortfolioCreationForm.FillPositionsFields(positionsModels);
            manualPortfolioCreationForm.ClickPortfolioManualFlowActionsButton(PortfolioManualFlowActionsButton.SavePortfolio);
            var addAlertsAtCreatingPortfolioForm = new AddAlertsAtCreatingPortfolioForm();
            addAlertsAtCreatingPortfolioForm.SetAlertSlider(AlertsToPositionsAtPortfolioCreation.VqTrailingStop, vqSetState);
            addAlertsAtCreatingPortfolioForm.SetAlertSlider(AlertsToPositionsAtPortfolioCreation.TrailingStop, tsSetState);
            addAlertsAtCreatingPortfolioForm.SetAlertSlider(AlertsToPositionsAtPortfolioCreation.PercentageGain, lcSetState);
            addAlertsAtCreatingPortfolioForm.SetAlertSlider(AlertsToPositionsAtPortfolioCreation.FixedPrice, lcSetState);
            addAlertsAtCreatingPortfolioForm.ClickActionButton(AddAlertsAtCreatingPortfolioButtons.AddAlerts);

            new MainMenuNavigation().OpenPortfolios(addPortfolioManualModel.Type);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_621$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PositionsGrid"), TestCategory("PositionsGridBulkActionButtons"), TestCategory("Alerts"), 
            TestCategory("AlertAdd"), TestCategory("EventHistoryPage")]
        [Description("The test checks possibility to copy only one position with alerts from grid. https://tr.a1qa.com/index.php?/cases/view/19232510")]
        public override void RunTest()
        {
            LogStep(1, "Open 'MyPortfolio1'");
            new PortfoliosForm().ClickOnPortfolioName(addPortfolioManualModel.Name);
            var positionsAlertsStatisticsPanelForm = new PositionsAlertsStatisticsPanelForm();
            positionsAlertsStatisticsPanelForm.AssertIsOpen();

            LogStep(2, "Order positions grid by Ticker field DESC. Remember Symbol and Position Name for the first position in the grid. " +
                "Detect and remember number of alerts for the first position.");
            var positionsTab = new PositionsTabForm();
            positionsTab.ClickOnPositionColumnToSort(PositionsGridDataField.Ticker, SortingStatus.Desc);
            positionsTickerNames = positionsTab.GetPositionColumnValues(PositionsGridDataField.Ticker);
            positionsAlertsQuantity = positionsTab.GetAlertsQuantityForAllPositions();

            DoStepsFrom2To12(new List<int> { OrderOfPositionForUniqueCopy });

            DoStepsFrom2To12(Enumerable.Range(1, positionsTickerNames.Count).Where(x => x != OrderOfPositionForUniqueCopy).ToList());
        }

        private void DoStepsFrom2To12(List<int> positionsOrdersToCopy)
        {
            LogStep(3, $"Select the specified  position(s) and click 'Copy' for the selected position {positionsTickerNames.Last().Split('\r')[0]}.");
            var positionsTab = new PositionsTabForm();
            foreach (var positionOrder in positionsOrdersToCopy)
            {
                positionsTab.SelectItemCheckboxByOrderAndState(positionOrder, true);
            }
            positionsTab.ClickGroupActionButton(PositionsGroupAction.Copy);

            LogStep(4, "In the 'Select a Portfolio to paste the positions:' drop-down select the portfolio from the precondition #15. " +
                "Tick 'Copy Alerts' check - box. Click 'OK' button.");
            var copyPositionPopup = new CopyPositionPopup();
            copyPositionPopup.SelectPortfolio(portfolioDestinationModel.Name);
            copyPositionPopup.SelectCopyAlerts(true);
            copyPositionPopup.ClickOkButton();

            LogStep(5, "Click 'OK' in 'SUCCESS' popup.");
            new ConfirmPopup(PopupNames.Success).ClickOkButton();

            LogStep(6, "Make sure that Position grid contains position (by Ticker and Position Name) selected for copy.");
            var positionsInSourcePortfolioAfterCopying = positionsTab.GetPositionColumnValues(PositionsGridDataField.Ticker);
            foreach (var positionTicker in positionsTickerNames)
            {
                Checker.IsTrue(positionsInSourcePortfolioAfterCopying.Contains(positionTicker),
                $"Position grid does not contain position {positionTicker} of step #2.");
            }

            LogStep(7, "Open My Portfolios page -> Manage tab; Open portfolio from the precondition #15.");
            var positionsAlertsStatisticsPanelForm = new PositionsAlertsStatisticsPanelForm();
            positionsAlertsStatisticsPanelForm.SelectPortfolioById(portfolioIdDestination);
            positionsTab.AssertIsOpen();
            Checker.CheckEquals(portfolioDestinationModel.Name, positionsAlertsStatisticsPanelForm.GetPortfolioName(),
                "Selected portfolio is not same as expected.");

            LogStep(8, "Make sure that the portfolio contains copied position (expected values are in test data):");
            positionsTab.ClickOnPositionColumnToSort(PositionsGridDataField.Ticker, SortingStatus.Desc);
            var eventsModels = new List<EventsModel>();

            foreach (var positionOrder in positionsOrdersToCopy)
            {
                var positionAlertsCount = positionsTab.GetAlertQuantityForPositionByOrder(positionOrder);
                Checker.CheckEquals(positionsTickerNames[positionOrder - 1], positionsTab.GetTextInColumnByRowOrder(PositionsGridDataField.Ticker, positionOrder), 
                    "Position grid does not contain copied position of step #2.");
                Checker.CheckEquals(positionsModels[positionOrder - 1].EntryDate, positionsTab.GetTextInColumnByRowOrder(PositionsGridDataField.EntryDate, positionOrder),
                     "EntryDate for copied position is not positionOrder with sourced");
                Checker.CheckEquals(positionsModels[positionOrder - 1].EntryPrice.ToFractionalString(),
                    StringUtility.ReplaceAllCurrencySigns(positionsTab.GetTextInColumnByRowOrder(PositionsGridDataField.EntryPrice, positionOrder)),
                    "EntryPrice for copied position is not matched with sourced");
                Checker.CheckEquals(expectedPositionQty[positionOrder - 1].ToFractionalString(), positionsTab.GetTextInColumnByRowOrder(PositionsGridDataField.Shares, positionOrder),
                    "Shares for copied position is not matched with sourced");
                Checker.CheckEquals(positionsModels[positionOrder - 1].TradeType.GetStringMapping(), positionsTab.GetTextInColumnByRowOrder(PositionsGridDataField.TradeType, positionOrder),
                    "TradeType for copied position is not matched with sourced");
                Checker.CheckEquals(positionsAlertsQuantity[positionOrder - 1], positionAlertsCount,
                    "Alerts quantity for copied position is not matched with sourced");
                eventsModels.Add(new EventsModel
                {
                    EventType = EventTypes.CopyPosition.GetStringMapping(),
                    Description = string.Format(copyEventDescription, positionsTickerNames[positionOrder - 1].Split('\r')[0], addPortfolioManualModel.Name, portfolioDestinationModel.Name)
                });
            }

            LogStep(9, "Open My Portfolios page -> Manage tab; Open portfolio from the precondition #15.");
            var portfoliosMenuForm = new MyPortfoliosMenuForm();
            portfoliosMenuForm.ClickMyPortfoliosMenuItem(MyPortfoliosMenuItems.AlertsGrid);
            positionsAlertsStatisticsPanelForm.SelectPortfolioById(portfolioIdDestination);
            var alertsTabForm = new AlertsTabForm();
            Checker.CheckEquals(portfolioDestinationModel.Name, positionsAlertsStatisticsPanelForm.GetPortfolioName(),
                "Selected portfolio is not same as expected.");
            alertsTabForm.ClickOnPositionColumnToSort(AlertsGridColumnsDataField.Ticker, SortingStatus.Desc);
            foreach (var positionOrder in positionsOrdersToCopy)
            {
                Checker.CheckEquals(alertsStates[positionOrder - 1],
                    alertsTabForm.GetTextInColumnByRowOrder(AlertsGridColumnsDataField.AlertState, positionOrder),
                    $"Alert State is not as expected for source for position order {positionOrder}.");
            }

            LogStep(10, "Open Settings -> Events. Select the 'Portfolio2' on the Portfolio drop - down menu.");
            var eventsForm = new EventsSteps().OpenEventsSelectPortfolioByNameSelectEventTypeGetEventsForm(addPortfolioManualModel.Name, EventTypes.CopyPosition);
            eventsForm.SelectPeriod(GridFilterPeriods.Last7Days);
            Checker.IsTrue(eventsModels.Count > 0, "Number of events is 0");
            foreach (var model in eventsModels)
            {
                Checker.IsTrue(eventsForm.IsEventGridContainsCertainEventDescriptionType(model),
                    $"Grid does not contain event {model.Description}");
            }

            LogStep(11, "Repeat steps 2 - 7 for all positions in the portfolio except for the first one.");
            new MainMenuNavigation().OpenPositionsGrid();
            positionsAlertsStatisticsPanelForm.SelectPortfolio(addPortfolioManualModel.Name);
            positionsTab.ClickOnPositionColumnToSort(PositionsGridDataField.Ticker, SortingStatus.Desc);
        }
    }
}