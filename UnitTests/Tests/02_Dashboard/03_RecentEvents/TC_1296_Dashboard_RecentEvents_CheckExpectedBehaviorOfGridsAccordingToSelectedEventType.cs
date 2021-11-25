using System;
using System.Collections.Generic;
using System.Linq;
using AutomatedTests.Database.DsiForSymbols;
using AutomatedTests.Database.Events;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Database.Users;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Dashboard;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.Sorting;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Dashboard;
using AutomatedTests.Models.PositionsModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps;
using AutomatedTests.Steps.AddPositionAdvanced;
using AutomatedTests.Steps.AddPositionInlineForm;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._02_Dashboard._03_RecentEvents
{
    [TestClass]
    public class TC_1296_Dashboard_RecentEvents_CheckExpectedBehaviorOfGridsAccordingToSelectedEventType : BaseTestUnitTests
    {
        private const int TestNumber = 1296;
        private const WidgetTypes Widget = WidgetTypes.RecentEvents;
        private const string TickerColumn = "ticker";
        private const string AlertTriggerTypeColumn = "alertTriggerType";

        private List<PositionsDBModel> positionsModels;
        private List<string> alertTriggerTypes;
        private List<GeneralTablesHeaders> expectedWidgetHeaders;
        private DashboardWidgetResentEventsDropDownValues eventType;
        private bool isAddAlert;
        private bool isAddPositionViaUi;
        private int countOfClosedPositionsViaUi;

        [TestInitialize]
        public void TestInitialize()
        {
            eventType = GetTestDataParsedAsEnumFromStringMapping<DashboardWidgetResentEventsDropDownValues>(nameof(eventType));

            isAddAlert = GetTestDataAsBool(nameof(isAddAlert));
            isAddPositionViaUi = GetTestDataAsBool(nameof(isAddPositionViaUi));
            countOfClosedPositionsViaUi = GetTestDataAsInt(nameof(countOfClosedPositionsViaUi));

            var portfolioModel = new PortfolioModel
            {
                Name = "CheckExpectedBehaviorOfGridsAccordingToSelectedEventType",
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("portfolioType"),
                Currency = Currency.USD.GetStringMapping()
            };

            positionsModels = InitializePositionModels();
            alertTriggerTypes = InitializeAlertTriggerTypes();

            var eventsModel = new EventsDBModel
            {
                SystemEventCategoryId = ((int)eventType).ToString(),
                CurrentValue = GetTestDataAsString("currentValue"),
                PriceType = GetTestDataAsString("priceType"),
                UseIntraday = GetTestDataAsString("useIntraday"),
                IsTriggered = GetTestDataAsString("isTriggered")
            };

            expectedWidgetHeaders = new List<GeneralTablesHeaders>
            {
                GeneralTablesHeaders.Ticker,
                GeneralTablesHeaders.HealthSsi,
                GeneralTablesHeaders.Details,
                GeneralTablesHeaders.Date
            };

            LogStep(0, "Precondition - Login as registered user");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPlatinum));

            var portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);

            LoginSetUp.LogIn(UserModels.First());
            var positionsIds = AddPositions(isAddPositionViaUi, portfolioId, portfolioModel, positionsModels);

            eventsModel.UserId = new UsersQueries().SelectUserIdFromDbByEmail(UserModels.First().Email).ToString();
            eventsModel.PortfolioId = portfolioId.ToString();
            AddAlertsForPositions(isAddAlert, eventsModel, positionsIds);
            CloseAddedPositionsViaUi(countOfClosedPositionsViaUi, positionsIds);

            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
        }

        private List<PositionsDBModel> InitializePositionModels()
        {
            var positions = new List<PositionsDBModel>();
            var tableColumns = TestContext.DataRow.Table.Columns;
            var entryDate = GetTestDataAsString("entryDate");
            var entryPrice = GetTestDataAsString("entryPrice");
            var shares = GetTestDataAsString("shares");
            foreach (var column in tableColumns)
            {
                if (column.ToString().Contains(TickerColumn))
                {
                    positions.Add(new PositionsDBModel
                    {
                        Symbol = GetTestDataAsString(column.ToString()),
                        PurchaseDate = entryDate,
                        PurchasePrice = entryPrice,
                        Shares = shares
                    });
                }
            }

            return positions;
        }

        private List<string> InitializeAlertTriggerTypes()
        {
            var triggerTypes = new List<string>();
            var tableColumns = TestContext.DataRow.Table.Columns;
            foreach (var column in tableColumns)
            {
                if (column.ToString().Contains(AlertTriggerTypeColumn))
                {
                    triggerTypes.Add(GetTestDataAsString(column.ToString()));
                }
            }

            return triggerTypes;
        }

        private List<int> AddPositions(bool isAddPositionsViaUi, int portfolioId, PortfolioModel portfolio, List<PositionsDBModel> positions)
        {
            if (isAddPositionsViaUi)
            {
                var positionIds = new List<int>();
                var mainMenuSteps = new MainMenuSteps();
                var addPositionPopupSteps = new AddPositionInlineFormSteps();
                var addPositionAdvancedSteps = new AddPositionAdvancedSteps();

                foreach (var positionModel in positions)
                {
                    var addPositionAdvancedModel = new AddPositionAdvancedModel
                    {
                        Ticker = positionModel.Symbol,
                        EntryDate = positionModel.PurchaseDate,
                        EntryPrice = positionModel.PurchasePrice,
                        Shares = positionModel.Shares,
                        Portfolio = portfolio.Name,
                        AssetType = positionModel.Symbol.Contains("/")
                            ? PositionAssetTypes.Crypto
                            : PositionAssetTypes.Stock
                    };

                    mainMenuSteps.OpenPositionGridViaPortfolioGridLink(portfolio);
                    addPositionPopupSteps.OpenAddPositionAdvancedViaAddPositionInlineFormGetAddPositionAdvancedForm();
                    positionIds.Add(addPositionAdvancedSteps.FillFieldsClickSaveGetPositionId(addPositionAdvancedModel, portfolioId));
                }

                return positionIds;
            }

            return PositionsAlertsSetUp.AddPositionsViaDB(portfolioId, positions);
        }

        private void AddAlertsForPositions(bool isAddAlerts, EventsDBModel eventsDbModel, IReadOnlyList<int> positionIds)
        {
            if (isAddAlerts)
            {
                var issueDate = DateTimeProvider.GetDate(DateTime.Now).ToShortDateString();
                var events = eventsDbModel;
                for (var index = 0; index < positionIds.Count; index++)
                {
                    events.ItemType = alertTriggerTypes[index];
                    events.ItemName = positionsModels[index].Symbol;
                    events.PositionId = positionIds[index].ToString();
                    events.IssueDate = issueDate;
                    EventsSetUp.AddNewRowIntoSystemEvents(events);
                }
            }
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1296$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Dashboard"), TestCategory("DashboardRecentEvents")]
        [Description("Test checks an expected behavior of the grids according to the Event Type: https://tr.a1qa.com/index.php?/cases/view/19234310")]
        public override void RunTest()
        {
            LogStep(1, "Select Portfolio - All Investment");
            var dashboardForm = new DashboardForm();
            var selectedPortfolioCategory = AllPortfoliosKinds.AllInvestment.GetStringMapping();
            dashboardForm.SelectPortfolioStatisticsWidgetPortfolio(selectedPortfolioCategory);

            LogStep(2, "Check that name of the widget is displayed");
            var dashboardWidget = new WidgetForm(Widget);
            var dashboardWidgetTable = dashboardWidget.WidgetTable;
            Checker.CheckEquals(Widget.GetStringMapping(), dashboardWidget.GetWidgetHeaderText(),
                "Dashboard widget name is not as expected");

            LogStep(3, "Select 'Event Type'");
            dashboardWidget.SelectValueInWidgetDropDownByText(DashboardWidgetRecentEventsDropDown.EventType, eventType.GetStringMapping());

            LogStep(4, "Check the default sorting");
            var headerNameSortedAsDefault = GeneralTablesHeaders.Date.GetStringMapping();
            Checker.CheckEquals(headerNameSortedAsDefault, dashboardWidgetTable.GetTableColumnSortedAsDefault(),
                $"{headerNameSortedAsDefault} is not sorted as default");
            Checker.CheckEquals(SortingStatus.Desc, dashboardWidgetTable.GetTableHeaderSortingState(GeneralTablesHeaders.Date),
                "Sorting status is not as expected");

            LogStep(5, "Check that 3 columns are presented in the widget");
            var headerNames = dashboardWidgetTable.GetTableColumnNames();
            var expectedHeaderNames = expectedWidgetHeaders.Select(header => header.GetStringMapping()).ToList();
            Checker.CheckEquals(expectedHeaderNames.Count, headerNames.Count, "Count of columns is not as expected");
            Checker.CheckListsEquals(expectedHeaderNames, headerNames, "Displayed columns are not as expected");

            LogStep(6, "Check that tickers match for 'Ticker' column");
            var tickerTextValues = dashboardWidgetTable.GetTableColumnSymbolTextValuesByColumnName(GeneralTablesHeaders.Ticker);

            LogStep(7, "Check that name and title companies match for 'Ticker' column");
            var companyTextValues = dashboardWidgetTable.GetTableColumnSymbolTitleValuesByColumnName(GeneralTablesHeaders.Ticker);
            Checker.CheckEquals(tickerTextValues.Count, companyTextValues.Count,
                $"Tickers and names quantity for'{GeneralTablesHeaders.Ticker.GetStringMapping()}' don't match");

            LogStep(8, "Check that name and title columns match");
            var headerTextValues = dashboardWidgetTable.GetTableColumnNames();
            var headerTitleValues = dashboardWidgetTable.GetTableColumnDataNames();
            Checker.CheckListsEquals(headerTitleValues, headerTextValues, "Text and title headers don't match");

            LogStep(9, "Check that name and title values match for 'Details' column");
            CheckThatTextAndTitleValuesMatch(GeneralTablesHeaders.Details);

            LogStep(10, "Check that name and title values match for 'Date' column");
            CheckThatTextAndTitleValuesMatch(GeneralTablesHeaders.Date);

            LogStep(10, "Check matching the values obtained in the grid for 'Health' column");
            var ssiPillZones = dashboardWidgetTable.GetTableColumnSsiPillZonesByColumnName(GeneralTablesHeaders.HealthSsi);
            var symbolValues = dashboardWidgetTable.GetTableColumnSymbolTextValuesByColumnName(GeneralTablesHeaders.Ticker);
            var dsiSymbolsModels = new SymbolsQueries().GetHealthSymbolsModels(symbolValues);
            new ObjectComparator().CheckSsiPillsZonesFromUiAndDb(dsiSymbolsModels, ssiPillZones, symbolValues);
        }

        private void CheckThatTextAndTitleValuesMatch(GeneralTablesHeaders header)
        {
            var dashboardWidgetTable = new WidgetForm(Widget).WidgetTable;
            var textValues = dashboardWidgetTable.GetTableColumnTextValuesByColumnName(header);
            var titleValues = dashboardWidgetTable.GetTableColumnTitleValuesByColumnName(header);
            Checker.CheckListsEquals(titleValues, textValues, $"Text and title '{header.GetStringMapping()}' values don't match");
        }

        private void CloseAddedPositionsViaUi(int countOfClosedPositions, IReadOnlyList<int> positionIds)
        {
            new MainMenuNavigation().OpenPositionsGrid();
            while (countOfClosedPositions-- > 0)
            {
                PositionsAlertsSetUp.ClosePositionWithSetSsiAlert(positionIds[countOfClosedPositions]);
            }
        }
    }
}