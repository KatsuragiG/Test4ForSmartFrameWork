using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Alerts;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.Settings;
using AutomatedTests.Enums.User;
using AutomatedTests.Forms.MyPortfolios;
using AutomatedTests.Forms.Popups;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Models;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.PositionsGridSteps;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._04_PositionsAndAlerts._02_Alerts
{
    [TestClass]
    public class TC_0397_AlertsGrid_Search : BaseTestUnitTests
    {
        private const int TestNumber = 397;
        private const int AlertOrderToSearchInTriggered = 1;

        private readonly List<PositionsDBModel> stockModels = new List<PositionsDBModel>();
        private readonly List<PositionsDBModel> optionModels = new List<PositionsDBModel>();
        private List<string> searchTexts;
        private List<string> searchResults;
        private List<int> answersQuantity = new List<int>();
        private string positionTag;
        private string viewNameForAddedView;
        private int step;
        private readonly PositionsQueries positionsQueries = new PositionsQueries();

        [TestInitialize]
        public void TestInitialize()
        {
            var userType = GetTestDataParsedAsEnumFromStringMapping<ProductSubscriptionTypes>("userType");
            var portfolioModel = new PortfolioModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency")
            };

            FillDataInStockModels();

            FillDataInOptionModels();

            viewNameForAddedView = StringUtility.RandomString(GetTestDataAsString(nameof(viewNameForAddedView)));
            positionTag = GetTestDataAsString(nameof(positionTag));

            searchTexts = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(searchTexts));
            searchResults = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(searchResults));
            answersQuantity = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(answersQuantity))
                .Select(Parsing.ConvertToInt).ToList();

            LogStep(step++, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, (ProductSubscriptions)(int)userType));
            var portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            var positionsIds = new List<int>();
            foreach (var stockModel in stockModels)
            {
                positionsIds.Add(PositionsAlertsSetUp.AddPositionViaDB(portfolioId, stockModel));
            }
            foreach (var optionModel in optionModels)
            {
                PositionsAlertsSetUp.AddPositionViaDB(portfolioId, optionModel);
            }

            positionsQueries.AddNewRowIntoUserTags(UserModels.First().TradeSmithUserId, positionTag);
            positionsQueries.AddNewRowIntoUserPositionTags(UserModels.First().TradeSmithUserId, positionTag, positionsIds.First());

            LoginSetUp.LogIn(UserModels.First());

            PortfoliosSetUp.ImportDagSiteInvestment06(true);

            new MainMenuNavigation().OpenPositionsGrid();
            new PositionsAlertsStatisticsPanelForm().SelectPortfolio(AllPortfoliosKinds.All.GetStringMapping());

            var positionsTabForm = new PositionsTabForm();
            positionsTabForm.SelectAllItemsInGrid();
            positionsTabForm.ClickGroupActionButton(PositionsGroupAction.AddAlert);
            var positionsGridSteps = new PositionsGridSteps();
            positionsGridSteps.ApplyTemplateInAddAlertPopup(DefaultTemplateTypes.VqTrailingStop.GetStringMapping());
            new ConfirmPopup(PopupNames.Warning).ClickOkButton();

            positionsTabForm.SelectAllItemsInGrid();
            positionsTabForm.ClickGroupActionButton(PositionsGroupAction.AddAlert);
            positionsGridSteps.ApplyTemplateInAddAlertPopup(DefaultTemplateTypes.TrailingStop15.GetStringMapping());
            new ConfirmPopup(PopupNames.Success).ClickOkButton();

            new MyPortfoliosMenuForm().ClickMyPortfoliosMenuItem(MyPortfoliosMenuItems.AlertsGrid);
            var alertsTab = new AlertsTabForm();

            alertsTab.AddNewViewWithAllCheckboxesMarked(viewNameForAddedView);
            alertsTab.SelectView(viewNameForAddedView);
        }

        private void FillDataInOptionModels()
        {
            optionModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolOption1"),
                TradeType = $"{(int)PositionTradeTypes.Short}",
                Notes = GetTestDataAsString("NotesOption1")
            });
            optionModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolOption2"),
                TradeType = $"{(int)PositionTradeTypes.Long}"
            });
            optionModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolOption3"),
                TradeType = $"{(int)PositionTradeTypes.Short}",
                StatusType = $"{(int)AutotestPositionStatusTypes.Delisted}"
            });
            optionModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolOption4"),
                TradeType = $"{(int)PositionTradeTypes.Long}",
                StatusType = $"{(int)AutotestPositionStatusTypes.Expired}"
            });
        }

        private void FillDataInStockModels()
        {
            stockModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolStock1"),
                TradeType = $"{(int)PositionTradeTypes.Long}"
            });
            stockModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolStock2"),
                TradeType = $"{(int)PositionTradeTypes.Short}",
                Notes = GetTestDataAsString("NotesStock2")
            });
            stockModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolStock3"),
                TradeType = $"{(int)PositionTradeTypes.Long}",
                StatusType = $"{(int)AutotestPositionStatusTypes.Delisted}"
            });
            stockModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolStock4"),
                TradeType = $"{(int)PositionTradeTypes.Short}"
            });
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_397$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("AlertsGrid")]
        [Description("The test checks correctness of filtration using search for Alerts grid https://tr.a1qa.com/index.php?/cases/view/19232691")]
        public override void RunTest()
        {
            LogStep(step++, "Type in search field any random text");
            var alertsTabForm = new AlertsTabForm();
            alertsTabForm.Search(searchTexts[0]);
            Checker.IsTrue(alertsTabForm.IsNoResultsFoundTextPresent(), "No results found text is not shown");

            LogStep(step++, "Click clear sign ");
            alertsTabForm.ClearSearchField();
            Checker.IsFalse(alertsTabForm.IsNoResultsFoundTextPresent(), "No results found text is shown");

            LogStep(step, "Type in search field AG");
            alertsTabForm.Search(stockModels[0].Symbol);
            var positionsSymbols = alertsTabForm.GetColumnValues(AlertsGridColumnsDataField.Ticker);
            Checker.IsTrue(positionsSymbols.Any(), $"Grid does not contain a ticker {stockModels[0].Symbol} (step {step})");
            foreach (var positionSymbol in positionsSymbols)
            {
                Checker.CheckContains(stockModels[0].Symbol, positionSymbol.Split('\r')[0], $"Grid ticker should contain Symbol {stockModels[0].Symbol} (step {step})");
            }
            Checker.CheckEquals(answersQuantity[0], positionsSymbols.Count, $"Grid does not contain two positions (step {step++})");

            LogStep(step, "Clear search field and Type in search field 210115c");
            CheckSearchResults(step++, searchTexts[1], answersQuantity[1], searchResults[0]);

            LogStep(step, "Clear search field and Type in search field SPY210115P00180000");
            CheckSearchResults(step++, optionModels[2].Symbol, answersQuantity[2], string.Empty);

            LogStep(step, "Clear search field and Type in search field MSFT210115P00140000");
            CheckSearchResults(step++, optionModels[1].Symbol, answersQuantity[3], optionModels[1].Symbol);

            LogStep(step, "Clear search field and Type in search field AAPL");
            CheckSearchResults(step++, searchTexts[2], answersQuantity[4], searchResults[1]);

            LogStep(step, "Clear search field and Type in search field aPpLe");
            CheckSearchResults(step++, searchTexts[3], answersQuantity[5], searchResults[2]);

            LogStep(step, "Clear search field and Type in search field unique position name from AG");
            CheckSearchResults(step++, positionsQueries.SelectSymbolIdNameUsingSymbol(stockModels[0].Symbol).SymbolName.Split('\r')[0],
                answersQuantity[6], stockModels[0].Symbol);

            LogStep(step, "Clear search field and Type in search field unique position name from MSFT210115P00140000");
            CheckSearchResults(step++, positionsQueries.SelectSymbolIdNameUsingSymbol(optionModels[0].Symbol).SymbolName,
                answersQuantity[7], optionModels[0].Symbol);

            LogStep(step, "Clear search field and Type in search field unique position notes from IBM");
            CheckSearchResults(step++, stockModels[1].Notes, answersQuantity[8], stockModels[1].Symbol);

            LogStep(step, "Clear search field and Type in search field unique position tag from AG");
            CheckSearchResults(step++, positionTag, answersQuantity[9], string.Empty);

            LogStep(step++, "Enable checkbox Show Triggered Only");
            alertsTabForm.SetTriggeredOnlyInState(true);
            Checker.IsTrue(alertsTabForm.IsTriggeredOnlyChecked(), "Show Triggered Only is not checked");

            LogStep(step, "Check search by ticker");
            alertsTabForm.ClearSearchField();
            var triggeredPositionMetric = new TableCellMetrics { PositionOrder = AlertOrderToSearchInTriggered, ColumnHeader = AlertsGridColumnsDataField.Ticker.GetStringMapping() };
            var textToSearch = alertsTabForm.GetPositionsGridCellValue(triggeredPositionMetric).Split('\r')[0];
            alertsTabForm.Search(textToSearch);
            Assert.IsTrue(alertsTabForm.GetNumberOfRowsInGrid() > 0, $"Grid does not contain triggered alerts before search (step {step})");

            positionsSymbols = alertsTabForm.GetColumnValues(AlertsGridColumnsDataField.Ticker)
                .Select(t => t.Split('\r')[0]).ToList();
            Checker.IsTrue(positionsSymbols.Any(), $"Grid contains {positionsSymbols.Count} position (step {step})");
            foreach (var positionSymbol in positionsSymbols)
            {
                Checker.CheckContains(textToSearch, positionSymbol.Split('\r')[0], $"Grid ticker should contain triggered Symbol {textToSearch} (step {step})");
            }

            LogStep(++step, "Clear search. Enable checkbox Group by Position. Make sure grid is not empty");
            alertsTabForm.ClearSearchField();
            alertsTabForm.SetGroupByPositionInState(true);
            Checker.IsTrue(alertsTabForm.IsGroupByPositionChecked(), "Group By Position is not checked");
            var groupedAlertsModels = alertsTabForm.GetGroupedAlertsInformation();
            Assert.IsTrue(groupedAlertsModels.Any(), $"Grid does not contain grouped + triggered alerts before search (step {step})");

            LogStep(++step, "Check search by ticker");
            textToSearch = groupedAlertsModels.First().Ticker;
            alertsTabForm.Search(textToSearch);
            var groupedAlertsModelsAfterSearch = alertsTabForm.GetGroupedAlertsInformation();
            Checker.IsTrue(groupedAlertsModelsAfterSearch.Any(), $"Grid does not contain grouped alerts after search (step {step})");
            foreach (var groupedAlertModel in groupedAlertsModelsAfterSearch)
            {
                Checker.IsTrue(groupedAlertModel.Ticker.Contains(textToSearch) || groupedAlertModel.Name.Contains(textToSearch),
                    $"Grouped grid ticker {groupedAlertModel.Ticker} or name {groupedAlertModel.Name} does not contain '{textToSearch}' (step {step})");
                Checker.IsTrue(groupedAlertModel.IsTriggered.Contains(false),
                    $"Grouped grid ticker {groupedAlertModel.Ticker} has untriggered alert at search Grouped + Triggered (step {step})");
            }

            LogStep(++step, "Disable checkbox Show Triggered Only");
            alertsTabForm.SetTriggeredOnlyInState(false);
            Checker.IsFalse(alertsTabForm.IsTriggeredOnlyChecked(), "Show Triggered Only is checked");

            LogStep(++step, "Check search by ticker");
            alertsTabForm.ClearSearchField();
            groupedAlertsModels = alertsTabForm.GetGroupedAlertsInformation();
            Assert.IsTrue(groupedAlertsModels.Any(), $"Grid does not contain grouped alerts before search (step {step})");

            textToSearch = groupedAlertsModels.First().Ticker;
            alertsTabForm.Search(textToSearch);
            groupedAlertsModelsAfterSearch = alertsTabForm.GetGroupedAlertsInformation();
            Checker.IsTrue(groupedAlertsModelsAfterSearch.Any(), $"Grid does not contain grouped alerts after search (step {step})");
            foreach (var groupedAlertModel in groupedAlertsModelsAfterSearch)
            {
                Checker.IsTrue(groupedAlertModel.Ticker.Contains(textToSearch) || groupedAlertModel.Name.Contains(textToSearch),
                    $"Grouped grid ticker {groupedAlertModel.Ticker} or name {groupedAlertModel.Name} does not contain '{textToSearch}' (step {step})");
            }
        }

        private void CheckSearchResults(int stepNumber, string searchString, int resultsQuantity, string expectedResultTicker)
        {
            var positionsSymbols = SearchPositionSymbols(searchString);
            Checker.CheckEquals(resultsQuantity, positionsSymbols.Count, $"Grid does not contain one position (step {stepNumber})");
            if (positionsSymbols.Any() || !string.IsNullOrEmpty(expectedResultTicker))
            {
                Checker.IsTrue(positionsSymbols.Contains(expectedResultTicker), $"Grid does not contain position {expectedResultTicker} (step {stepNumber})");
            }
        }

        private List<string> SearchPositionSymbols(string searchString)
        {
            var alertsTabForm = new AlertsTabForm();
            alertsTabForm.ClearSearchField();
            alertsTabForm.Search(searchString);
            var positionsSymbols = alertsTabForm.GetColumnValues(AlertsGridColumnsDataField.Ticker)
                .Select(t => t.Split('\r')[0]).ToList();

            return positionsSymbols;
        }
    }
}