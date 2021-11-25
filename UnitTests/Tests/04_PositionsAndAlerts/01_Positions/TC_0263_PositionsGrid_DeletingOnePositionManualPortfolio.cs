using System.Collections.Generic;
using System.Linq;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Events;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms.Dashboard;
using AutomatedTests.Forms.Other;
using AutomatedTests.Forms.Popups;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Models;
using AutomatedTests.Models.EventsModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Events;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;
using WebdriverFramework.Framework.WebDriver;

namespace UnitTests.Tests._04_PositionsAndAlerts._01_Positions
{
    [TestClass]
    public class TC_0263_PositionsGrid_DeletingOnePositionManualPortfolio : BaseTestUnitTests
    {
        private const int TestNumber = 263;

        private readonly List<int> positionsIds = new List<int>();
        private int portfolioId;
        private string expectedWording;
        private const string DeleteEventDescriptionWording = "{0} Position Deleted";

        [TestInitialize]
        public void TestInitialize()
        {
            expectedWording = GetTestDataAsString(nameof(expectedWording));

            var userProductSubscriptions = GetUserProductSubscriptions("User");

            var addPortfolioManualModel = new AddPortfolioManualModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency")
            };

            var positionsModels = new List<PositionAtManualCreatingPortfolioModel>();
            var positionsQuantity = GetTestDataAsInt("positionsQuantity");
            for (int i = 1; i <= positionsQuantity; i++)
            {
                positionsModels.Add(new PositionAtManualCreatingPortfolioModel
                {
                    Ticker = GetTestDataAsString($"Ticker{i}"),
                    PositionAssetType = GetTestDataParsedAsEnumFromStringMapping<PositionAssetTypes>($"StockType{i}"),
                    TradeType = GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>($"tradeType{i}"),
                    EntryDate = GetTestDataAsString($"entryDate{i}"),
                    Quantity = GetTestDataAsString($"positionQty{i}"),
                    EntryPrice = GetTestDataAsString($"entryPrice{i}")
                });
            }

            var entryPrice = GetTestDataAsString("entryPrice1");
            var entryDate = GetTestDataAsString("expiredEntryDate");
            var shares = GetTestDataAsString("positionQty1");

            var expiredDelistedModels = new List<PositionsDBModel>
            {
                new PositionsDBModel
                {
                    Symbol = GetTestDataAsString("expiredTicker"),
                    TradeType = $"{(int)PositionTradeTypes.Long}",
                    PurchasePriceAdj = entryPrice,
                    PurchaseDate = entryDate,
                    Shares = shares,
                    StatusType = ((int)AutotestPositionStatusTypes.Expired).ToString()
                },

                new PositionsDBModel
                {
                    Symbol = GetTestDataAsString("delistedTicker"),
                    TradeType = $"{(int)PositionTradeTypes.Long}",
                    PurchasePriceAdj = entryPrice,
                    PurchaseDate = entryDate,
                    Shares = shares,
                    StatusType = ((int)AutotestPositionStatusTypes.Delisted).ToString()
                }
            };

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));

            LoginSetUp.LogIn(UserModels.First());
            new AddPortfoliosSteps().NavigateToAddManualPortfolioSavePortfolioAfterFillingFields(addPortfolioManualModel, positionsModels);

            portfolioId = new PortfoliosQueries().SelectPortfolioIdForLastPortfolioByUserEmail(UserModels.Last().Email);
            PositionsAlertsSetUp.AddPositionsViaDB(portfolioId, expiredDelistedModels);

            positionsIds.AddRange(new PositionsQueries().SelectPositionIdsForPortfolio(portfolioId));
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_263$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PositionsGrid"), TestCategory("PositionsGridPositionActionsPopup")]
        [Description("The test checks possibility to delete only one position from grid; https://tr.a1qa.com/index.php?/cases/view/19235165")]
        public override void RunTest()
        {
            DoStepsFrom1To8(new List<int> { positionsIds[0] });

            DoStepsFrom1To8(positionsIds.Except(new List<int> { positionsIds[0] }).ToList());
        }

        private void DoStepsFrom1To8(List<int> currentPositionsIds)
        {
            var mainMenuNavigation = new MainMenuNavigation();

            var urlOfPositionCardBeforeDeleting = new List<string>();
            foreach (var positionId in currentPositionsIds)
            {
                LogStep(1, "Open the first position in another window and copy URL of this position.");

                mainMenuNavigation.OpenPositionCardInNewTabWithSwitchingToTab(positionId);
                new PositionCardForm().AssertIsOpen();
                urlOfPositionCardBeforeDeleting.Add(Browser.GetDriver().Url);
                Browser.Instance.GetDriver().Close();
                Browser.Instance.GetDriver().SwitchTo().Window(Browser.Instance.GetDriver().WindowHandles.First());
            }

            LogStep(2, "Go to Positions tab and remember ticker and Position Name of the first position.");
            mainMenuNavigation.OpenPositionsGrid();
            var positionTabForm = new PositionsTabForm();
            var tickers = new List<string>();
            var names = new List<string>();
            foreach (var positionId in currentPositionsIds)
            {
                var tickerName = positionTabForm.GetPositionsGridCellValue(new TableCellMetrics { PositionId = positionId, ColumnHeader = PositionsGridDataField.Ticker.GetStringMapping() })
                   .Split('\r');
                tickers.Add(tickerName[0]);
                names.Add(tickerName[1].Replace("\n", ""));
            }

            LogStep(3, "Tick the first position of this portfolio and click the button 'Delete'.");
            if (currentPositionsIds.Count == 1)
            {
                positionTabForm.SelectPositionCheckboxByPositionIdState(currentPositionsIds[0], true);
                positionTabForm.SelectPositionContextMenuOption(currentPositionsIds[0], PositionContextNavigation.DeletePosition);
            }
            else
            {
                positionTabForm.SelectAllItemsInGrid();
                positionTabForm.ClickGroupActionButton(PositionsGroupAction.Delete);
            }            

            LogStep(4, "Click the button 'Yes' and then 'OK'.");
            new ConfirmPopup(PopupNames.Confirm).ClickYesButton();
            new ConfirmPopup(PopupNames.Success).ClickOkButton();
            positionTabForm.AssertIsOpen();
            foreach (var positionId in currentPositionsIds)
            {
                Checker.IsFalse(positionTabForm.IsPositionPresentInGridById(positionId), $"Deleted position is shown in grid for {positionId}");
            }
            
            foreach (var deletedUrl in urlOfPositionCardBeforeDeleting)
            {
                LogStep(5, "Open URL for the first position card of deleted position (kept in the step 1)");
                Browser.Instance.GetDriver().Navigate().GoToUrl(deletedUrl);
                var error404Form = new Error404Form();
                error404Form.AssertIsOpen();
                Checker.CheckEquals(expectedWording, error404Form.GetErrorDescription(), $"Description text is not as expected for {deletedUrl}");

                LogStep(6, "Click the button 'Go to Homepage'.");
                error404Form.ClicknGoToHomePage();
                new DashboardForm().AssertIsOpen();
            }

            var positionsQueries = new PositionsQueries();
            foreach (var positionId in currentPositionsIds)
            {
                LogStep(7, "Check that position in DB has corresponded status for deleting ");
                Checker.IsTrue(positionsQueries.SelectPositionStatusOfDeleting(positionId),
                   $"Position in DB has not corresponded status for deleting for {positionId}");
            }

            LogStep(8, "Open Settings - Events, Select the Manual portfolio in dropdowns");
            var eventForm = new EventsSteps().OpenEventsSelectPortfolioByIdGetEventsForm(portfolioId);
            eventForm.SelectPeriod(GridFilterPeriods.Last7Days);

            for (int i = 0; i < tickers.Count; i++)
            {
                var eventsModels = new EventsModel
                {
                    EventType = EventTypes.DeletePosition.GetStringMapping(),
                    Symbol = tickers[i],
                    PositionName = names[i],
                    Description = string.Format(DeleteEventDescriptionWording, tickers[i])
                };

                Checker.IsTrue(eventForm.IsEventGridContainsCertainEventDescriptionType(eventsModels),
                    $"Grid does not contain event {eventsModels.Description}");
            }
        }
    }
}