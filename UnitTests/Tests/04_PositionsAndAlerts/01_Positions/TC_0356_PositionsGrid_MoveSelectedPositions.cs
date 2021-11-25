using System.Collections.Generic;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Alerts;
using AutomatedTests.Enums.Events;
using AutomatedTests.Enums.Portfolios.CreateManual;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.Sorting;
using AutomatedTests.Enums;
using AutomatedTests.Forms.MyPortfolios;
using AutomatedTests.Forms.Popups;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;
using AutomatedTests.Steps.Events;
using AutomatedTests.Forms;
using AutomatedTests.Models.EventsModels;
using AutomatedTests.Models;
using System.Linq;

namespace UnitTests.Tests._04_PositionsAndAlerts._01_Positions
{
    [TestClass]
    public class TC_0356_PositionsGrid_MoveSelectedPositions : BaseTestUnitTests
    {
        private const int TestNumber = 356;

        private AddPortfolioManualModel addPortfolioManualModel;
        private readonly List<PositionAtManualCreatingPortfolioModel> positionsModels = new List<PositionAtManualCreatingPortfolioModel>();
        private List<string> alertsStates;
        private List<string> expectedPositionQty;
        private List<int> positionsIdsNotMoved;
        private int positionsQuantity;
        private int destinationPortfolioId;
        private string expectedText;
        private string moveEventDescription;

        [TestInitialize]
        public void TestInitialize()
        {
            addPortfolioManualModel = new AddPortfolioManualModel
            {
                Name = StringUtility.RandomStringOfSize(Constants.DefaultSizeOfDateStringToClearField),
                Type = PortfolioType.Investment,
                Currency = Currency.USD.GetStringMapping()
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

            var positionsUnMoveAble = new List<PositionsDBModel>
            {
                new PositionsDBModel
                {
                    Symbol = GetTestDataAsString("SymbolDelisted"),
                    TradeType = $"{(int)PositionTradeTypes.Long}",
                    StatusType = ((int)AutotestPositionStatusTypes.Delisted).ToString()
                },
                new PositionsDBModel
                {
                    Symbol = GetTestDataAsString("SymbolExpired"),
                    TradeType = $"{(int)PositionTradeTypes.Short}",
                    StatusType = ((int)AutotestPositionStatusTypes.Expired).ToString()
                }
            };

            var vqSetState = GetTestDataParsedAsEnumFromStringMapping<AlertsToPositionsStates>("vqSetState");
            var tsSetState = GetTestDataParsedAsEnumFromStringMapping<AlertsToPositionsStates>("tsSetState");
            var lcSetState = GetTestDataParsedAsEnumFromStringMapping<AlertsToPositionsStates>("lcSetState");

            expectedText = GetTestDataAsString(nameof(expectedText));
            moveEventDescription = "{0} position moved (from {1} to {2})";

            alertsStates = GetTestDataValuesAsListByColumnName(nameof(alertsStates));
            expectedPositionQty = GetTestDataValuesAsListByColumnName(nameof(expectedPositionQty));

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, new List<ProductSubscriptions>
                {
                    ProductSubscriptions.TradeStopsPremium, ProductSubscriptions.CryptoStopsPremium
                }
            ));

            destinationPortfolioId = PortfoliosSetUp.AddInvestmentPortfoliosDefaultUSD(UserModels.First().Email);
            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);

            var mainMenuNavigation = new MainMenuNavigation();
            mainMenuNavigation.OpenAddManualPortfolio();

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

            var portfolioIdSource = new PortfoliosQueries().SelectPortfolioIdForLastPortfolioByUserEmail(UserModels.First().Email);
            positionsIdsNotMoved = PositionsAlertsSetUp.AddPositionsViaDB(portfolioIdSource, positionsUnMoveAble);

            mainMenuNavigation.OpenPortfolios(addPortfolioManualModel.Type);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_356$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PositionsGrid"), TestCategory("PositionsGridBulkActionButtons"),
            TestCategory("PositionsGridPositionActionsPopup"), TestCategory("EventHistoryPage")]
        [Description("The test checks moving of positions on positions grid. https://tr.a1qa.com/index.php?/cases/view/19232698")]
        public override void RunTest()
        {
            LogStep(1, "Select Portfolio #2 (precondition #1-2) on portfolios  dropdown");
            new MainMenuNavigation().OpenPositionsGrid();
            var positionsAlertsStatisticsPanelForm = new PositionsAlertsStatisticsPanelForm();
            positionsAlertsStatisticsPanelForm.SelectPortfolio(addPortfolioManualModel.Name);

            LogStep(2, "Tick all positions, keep their positionID and number of alerts.");
            var positionTabForm = new PositionsTabForm();
            positionTabForm.ClickOnPositionColumnToSort(PositionsGridDataField.Ticker, SortingStatus.Desc);
            var positionsIds = positionTabForm.GetPositionsIds();
            positionTabForm.SelectAllItemsInGrid();

            LogStep(3, "Click on Move button");
            positionTabForm.ClickGroupActionButton(PositionsGroupAction.Move);

            LogStep(4, "Select portfolio name = 'MyPortfolio1'");
            var movePosition = new MovePositionPopup();
            var destinationPortfolioName = new PortfoliosQueries().SelectPortfolioName(destinationPortfolioId);
            movePosition.SelectPortfolio(destinationPortfolioName);

            LogStep(5, "Click OK button.");
            movePosition.ClickOk();

            LogStep(6, "Click OK");
            var successPopupForm = new ConfirmPopup(PopupNames.Success);
            Checker.CheckEquals(expectedText, successPopupForm.GetMessage(), "Popup hasn't expected text");
            successPopupForm.ClickOkButton();
            var positionsNotMovedIds = positionTabForm.GetPositionsIds();
            Checker.CheckEquals(positionsIdsNotMoved.Count, positionsNotMovedIds.Count,
                "Quantity of not moved position in grid is not as expected");

            LogStep(7, 9, "Select Portfolio 'MyPortfolio2' on the portfolios drop-down menu.");
            positionsAlertsStatisticsPanelForm.SelectPortfolio(destinationPortfolioName);
            var actualMovedPositionsIds = positionTabForm.GetPositionsIds();
            Checker.CheckEquals(positionsIds.Count - positionsNotMovedIds.Count, actualMovedPositionsIds.Count,
                "Quantity of moved position in grid is not as expected");
            var eventsModels = new List<EventsModel>();

            foreach (var movedPositionId in actualMovedPositionsIds)
            {
                Checker.IsTrue(positionsIds.Contains(movedPositionId), $"Position with id = '{movedPositionId}' is not moved");
                var ticker = positionTabForm.GetPositionsGridCellValue(
                        new TableCellMetrics { PositionId = movedPositionId, ColumnHeader = PositionsGridDataField.Ticker.GetStringMapping() })
                    .Split('\r')[0];
                eventsModels.Add(new EventsModel
                {
                    EventType = EventTypes.MovePosition.GetStringMapping(),
                    Description = string.Format(moveEventDescription, ticker, addPortfolioManualModel.Name, destinationPortfolioName)
                });
            }
            foreach (var notMovedPositionId in positionsNotMovedIds)
            {
                Checker.IsFalse(actualMovedPositionsIds.Contains(notMovedPositionId), $"Position with id = '{notMovedPositionId}' is moved");
            }

            LogStep(10, "Click Alerts tab.");
            new MyPortfoliosMenuForm().ClickMyPortfoliosMenuItem(MyPortfoliosMenuItems.AlertsGrid);

            LogStep(11, "Make sure that alerts of movable positions have been presented on the Alerts grid. " +
                "Make sure that Alert State = [Ticker1 Alert State - Ticker5 Alert State].");
            positionsAlertsStatisticsPanelForm.SelectPortfolio(destinationPortfolioName);
            var alertsTabForm = new AlertsTabForm();
            alertsTabForm.ClickOnPositionColumnToSort(AlertsGridColumnsDataField.Ticker, SortingStatus.Desc);
            for (int i = 0; i < positionsQuantity; i++)
            {
                Checker.CheckEquals(alertsStates[i],
                    alertsTabForm.GetTextInColumnByRowOrder(AlertsGridColumnsDataField.AlertState, i + 1),
                    $"Alert State is not as expected for source for position order {i + 1}.");
            }

            LogStep(12, "Open Settings -> Events. Select the 'MyPortfolio1' on the Portfolio drop - down menu.");
            var eventsForm = new EventsSteps().OpenEventsSelectPortfolioByNameSelectEventTypeGetEventsForm(destinationPortfolioName, EventTypes.MovePosition);
            eventsForm.SelectPeriod(GridFilterPeriods.Last7Days);
            Checker.IsTrue(eventsModels.Count > 0, "Number of events is 0");
            foreach (var model in eventsModels)
            {
                Checker.IsTrue(eventsForm.IsEventGridContainsCertainEventDescriptionType(model),
                    $"Grid does not contain event {model.Description}");
            }
        }
    }
}