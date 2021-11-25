using System;
using System.Collections.Generic;
using System.Linq;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.User;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Popups;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Models;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._04_PositionsAndAlerts._01_Positions
{
    [TestClass]
    public class TC_0085_PositionsGrid_ClosePosition : BaseTestUnitTests
    {
        private const int TestNumber = 85;

        private PortfolioModel portfolioModel;
        private readonly List<PositionsDBModel> positionsModels = new List<PositionsDBModel>();
        private readonly List<int> positionsIds = new List<int>();
        private readonly List<int> closedPositionsIds = new List<int>();
        private string popupHeader;
        private string popupMessage;
        private string statusTypeDb;

        [TestInitialize]
        public void TestInitialize()
        {
            var userType = GetTestDataParsedAsEnumFromStringMapping<ProductSubscriptionTypes>("User");

            portfolioModel = new PortfolioModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency"),
                ExitCommission = (SRandom.Instance.NextDouble() * Constants.UpperLimitForPercent).ToString("N2")
            };
            var shares = GetTestDataAsString("Shares");
            positionsModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("Symbol1"),
                Shares = shares
            });
            positionsModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("Symbol2"),
                Shares = shares
            });

            var numberOfCheckBoxes = GetTestDataAsInt("numberOfCheckBoxes");
            var checkboxes = new List<string>();
            for (var i = 0; i < numberOfCheckBoxes; i++)
            {
                checkboxes.Add(GetTestDataAsString($"CheckBox{i + 1}"));
            }
            popupHeader = GetTestDataAsString(nameof(popupHeader));
            popupMessage = GetTestDataAsString(nameof(popupMessage));
            statusTypeDb = GetTestDataAsString(nameof(statusTypeDb));

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, (ProductSubscriptions)(int)userType));
            var portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            foreach (var positionModel in positionsModels)
            {
                positionsIds.Add(PositionsAlertsSetUp.AddPositionViaDB(portfolioId, positionModel));
            }
            LoginSetUp.LogIn(UserModels.First());

            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);
            new MainMenuNavigation().OpenPositionsGrid();
            new PositionsAlertsStatisticsPanelForm().SelectPortfolio(portfolioModel.Name);
            var positionTab = new PositionsTabForm();
            positionTab.ClickEditSignForCurrentView();
            positionTab.CheckCertainCheckboxes(checkboxes);
            positionTab.ClickSaveViewButton();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_85$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PositionsGrid"), TestCategory("PositionsGridPositionActionsPopup")]
        [Description("The test checks correct work of closing for position; https://tr.a1qa.com/index.php?/cases/view/19232925")]
        public override void RunTest()
        {
            LogStep(1, "Remember values for prepared position for closing");
            var positionTab = new PositionsTabForm();
            var closePositionSymbol = positionTab.GetSymbol(positionsIds[0]);
            var closePositionLatestClose = positionTab.GetPositionsGridCellValue(
                        new TableCellMetrics { PositionId = positionsIds[0], ColumnHeader = PositionsGridDataField.LatestClose.GetStringMapping() });

            LogStep(2, 3, "Open Action context menu for a position; Select Close Position in Context Menu");
            positionTab.SelectPositionContextMenuOption(positionsIds[0], PositionContextNavigation.ClosePosition);

            LogStep(4, "Check prepopulated values for popup");
            var closePositionPopup = new ClosePositionPopup();
            var closedDate = Parsing.ConvertToShortDateString(DateTime.Now.ToShortDateString());
            Checker.IsTrue(closePositionPopup.GetPopupHeader().Contains(closePositionSymbol.Substring(0, positionsModels[0].Symbol.Length)),
                $"Popup window header doesn't contain {closePositionSymbol}");
            Checker.IsTrue(closePositionPopup.GetCloseDate().Contains(closedDate), "Data field doesn't contain today's date");
            Checker.IsTrue(Constants.DecimalNumberWithIntegerPossibilityRegex.Match(closePositionLatestClose).Value
                    .Contains(Constants.DecimalNumberWithIntegerPossibilityRegex.Match(closePositionPopup.GetClosePrice()).Value),
                "Price field doesn't contain latest close price");
            Checker.CheckEquals(Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(closePositionPopup.GetCommission())),
                    Parsing.ConvertToDouble(portfolioModel.ExitCommission), "Price field doesn't contain commission");

            LogStep(5, 6, "If popup contains Re-Entry block - Set switcher Set SSI Alert: for No; Click OK");
            closePositionPopup.CheckReEntryBlockClickNo();
            closePositionPopup.ClickClosePositionButton();

            LogStep(7, 8, "Check that Closing is finished successfully; Click OK");
            var successPopup = new ConfirmPopup(PopupNames.Success);
            Checker.CheckEquals(popupHeader, successPopup.GetPopupHeader(), $"Popup Header doesn't contain {popupHeader}");
            Checker.CheckEquals(popupMessage, successPopup.GetMessage(), $"Text {popupMessage} is not present");
            successPopup.ClickOkButton();

            LogStep(9, "Go to Closed Positions tab");
            new PositionsAlertsStatisticsPanelForm().ActivateTab(PositionsTabs.ClosedPositions);

            LogStep(10, "Check that position is listed in grid with entered value");
            var closedPositionsTab = new ClosedPositionsTabForm();
            closedPositionsTab.SelectPeriod(GridFilterPeriods.Last7Days);
            closedPositionsIds.AddRange(closedPositionsTab.GetPositionsIds());
            var closedPositionId = closedPositionsIds[closedPositionsIds.Count - 1];
            Assert.AreEqual(positionsIds[0], closedPositionId, "PositionId incorrect");
            Checker.CheckEquals(closePositionSymbol, closedPositionsTab.GetSymbol(closedPositionId),
                       $"Symbol {closePositionSymbol} for closed position is not listed");
            Checker.CheckEquals(closedDate, 
                closedPositionsTab.GetPositionsGridCellValue(
                        new TableCellMetrics { PositionId = closedPositionId, ColumnHeader = ClosedPositionsGridDataField.ExitDate.GetStringMapping() }), 
                "Exit Date field doesn't contain today's date");
            Checker.CheckEquals(closePositionLatestClose, 
                closedPositionsTab.GetPositionsGridCellValue(
                        new TableCellMetrics { PositionId = closedPositionId, ColumnHeader = ClosedPositionsGridDataField.ExitPrice.GetStringMapping() }), 
                "Exit Price field doesn't contain latest close");

            LogStep(11, "Check Status, Exit Date, Exit Price for the closed stock into DB (by Position ID)");
            var positionQueries = new PositionsQueries();
            var closedPositionFromDb = positionQueries.SelectPositionData(closedPositionId);
            Asserts.Batch(
                   () =>
                  Assert.AreEqual(statusTypeDb, closedPositionFromDb.StatusType, $"Status type is not {statusTypeDb}"),
                  () =>
                  Assert.AreEqual(closedDate, Parsing.ConvertToShortDateString(closedPositionFromDb.ExitDate), "Exit Date field doesn't contain today's date"),
                   () =>
                  Assert.AreEqual(Parsing.ConvertToDouble(Constants.DecimalNumberRegex.Match(closePositionLatestClose).Value).ToString("N8"), 
                    Constants.DecimalNumberRegex.Match(closedPositionFromDb.ExitPrice).Value, 
                    "Exit Price field doesn't contain latest close")
            );
        }
    }
}