using System;
using System.Collections.Generic;
using System.Linq;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Portfolios;
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

namespace UnitTests.Tests._03_MyPortfolios._02_SynchPortfoliosImport
{
    [TestClass]
    public class TC_0787_SynchPortfoliosImport_AccountTransactions_SellingAndBuyingToCloseAreSuccessful : BaseTestUnitTests
    {
        private const int TestNumber = 787;
        private const string StartDateForCustomDateRangeFilter = "01/01/2016";

        private List<string> portfoliosNames;
        private readonly List<string> positionsSymbols = new List<string>();
        private readonly List<string> positionsVendorSymbols = new List<string>();
        private readonly List<string> positionsEntryDates = new List<string>();
        private readonly List<string> positionsEntryPrices = new List<string>();
        private readonly List<string> positionsShares = new List<string>();
        private readonly List<string> positionsSharesDb = new List<string>();
        private readonly List<string> positionsTypes = new List<string>();
        private readonly List<string> positionsExitDates = new List<string>();
        private readonly List<string> positionsExitPrices = new List<string>();
        private readonly List<Currency> positionsCurrencies = new List<Currency>();
        private int positionsQuantity;

        [TestInitialize]
        public void TestInitialize()
        {
            portfoliosNames = GetTestDataValuesAsListByColumnName("InitialPortfolioName");
            positionsQuantity = GetTestDataAsInt(nameof(positionsQuantity));
            var exitDate = GetTestDataAsString("ExitDates");
            for (int i = 1; i <= positionsQuantity; i++)
            {
                positionsSymbols.Add(GetTestDataAsString($"InitialPositionSymbol{i}"));
                positionsVendorSymbols.Add(GetTestDataAsString($"positionsVendorSymbols{i}"));
                positionsEntryDates.Add(GetTestDataAsString("InitialPositionsEntryDates"));
                positionsEntryPrices.Add(GetTestDataAsString($"InitialPositionsEntryPrices{i}"));
                positionsShares.Add(GetTestDataAsString($"InitialPositionsShares{i}"));
                positionsSharesDb.Add(GetTestDataAsString($"InitialPositionsSharesDB{i}"));
                positionsTypes.Add(GetTestDataAsString($"InitialPositionsTypes{i}"));
                positionsCurrencies.Add(GetTestDataParsedAsEnumFromStringMapping<Currency>($"InitialPositionsCurrencies{i}"));
                positionsExitDates.Add(exitDate);
                positionsExitPrices.Add(GetTestDataAsString($"ExitPrices{i}"));
            }

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPlatinum));
            LoginSetUp.LogIn(UserModels.First());

            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_787$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PortfoliosPage"), TestCategory("SyncPortfolio"), TestCategory("SyncPositions"), TestCategory("SyncPortfolioImport")]
        [Description("Test checks that syncing of imported portfolio with new transactions cause their applying to positions data with fully position closing as expected.Checked transactions:" +
            "Buy for long for all shares;Sell for short for all shares;TransferSharesIn in part of closing. https://tr.a1qa.com/index.php?/cases/view/19232327")]
        public override void RunTest()
        {
            LogStep(1, "Import portfolio");
            PortfoliosSetUp.ImportDagSiteInvestment15(true);
            var portfolioId = new PortfoliosQueries().SelectPortfolioId(UserModels.First().Email, portfoliosNames[0], PortfolioType.Investment);
            var portfolioForm = new PortfoliosForm();

            LogStep(2, "Open Positions & Alerts page -> Positions tab Select Portfolio with name 'xx3001'");
            portfolioForm.ClickOnPortfolioName(portfoliosNames[0]);
            var positionsTabForm = new PositionsTabForm();
            var positionsIds = positionsSymbols.Select(symbol => positionsTabForm.GetPositionsIdsByTicker(symbol)[0]).ToList();

            LogStep(3, "Compare values for positions in the grid ");
            var positionsEntryDate = new List<string>();
            var positionsEntryPrice = new List<string>();
            var positionsSharesGrid = new List<string>();
            var positionsType = new List<string>();
            foreach (var positionId in positionsIds)
            {
                positionsEntryDate.Add(positionsTabForm.GetPositionsGridCellValue(new TableCellMetrics { PositionId = positionId, ColumnHeader = PositionsGridDataField.EntryDate.GetStringMapping() }));
                positionsEntryPrice.Add(Constants.DecimalNumberRegex.Match(
                    positionsTabForm.GetPositionsGridCellValue(new TableCellMetrics { PositionId = positionId, ColumnHeader = PositionsGridDataField.EntryPrice.GetStringMapping() })).Value);
                positionsSharesGrid.Add(positionsTabForm.GetPositionsGridCellValue(new TableCellMetrics { PositionId = positionId, ColumnHeader = PositionsGridDataField.Shares.GetStringMapping() }));
                positionsType.Add(positionsTabForm.GetPositionsGridCellValue(new TableCellMetrics { PositionId = positionId, ColumnHeader = PositionsGridDataField.TradeType.GetStringMapping() }));
            }
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(positionsEntryDates, positionsEntryDate),
                $"Initial Entry Dates are not as expected\n{GetExpectedResultsString(positionsEntryDates)}\r\n{GetActualResultsString(positionsEntryDate)}");
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(positionsEntryPrices, positionsEntryPrice),
                $"Initial Entry Prices are not as expected\n{GetExpectedResultsString(positionsEntryPrices)}\r\n{GetActualResultsString(positionsEntryPrice)}");
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(positionsShares, positionsSharesGrid),
                $"Initial Shares are not as expected\n{GetExpectedResultsString(positionsShares)}\r\n{GetActualResultsString(positionsSharesGrid)}");
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(positionsTypes, positionsType),
                $"Initial Types are not as expected\n{GetExpectedResultsString(positionsTypes)}\r\n{GetActualResultsString(positionsTypes)}");

            LogStep(4, "Compare values in dbo.Positions for positions");
            var positionsDbData = positionsIds.Select(positionId => new PositionsQueries().SelectAllPositionData(positionId)).ToList();
            for (int i = 0; i < positionsDbData.Count; i++)
            {
                Checker.CheckEquals((int)positionsCurrencies[i], Parsing.ConvertToInt(positionsDbData[i].CurrencyId), "4CurrencyId is not as expected in DB");
                Checker.CheckEquals(positionsVendorSymbols[i], positionsDbData[i].VendorSymbol, "VendorSymbol is not as expected in DB");
                Checker.CheckEquals(Parsing.ConvertToShortDateString(positionsEntryDates[i]), Parsing.ConvertToShortDateString(positionsDbData[i].PurchaseDate),
                    "4PurshaseDate is not as expected in DB");
                Checker.CheckEquals((int)positionsTypes[i].ParseAsEnumFromStringMapping<PositionTradeTypes>(), Parsing.ConvertToInt(positionsDbData[i].TradeType),
                    "4TradeType is not as expected in DB");
                Checker.CheckEquals((int)AutotestPositionStatusTypes.Open, Parsing.ConvertToInt(positionsDbData[i].StatusType), "4StatusType is not as expected in DB");
                Checker.CheckEquals(Parsing.ConvertToDouble(positionsEntryPrices[i]), Parsing.ConvertToDouble(positionsDbData[i].PurchasePrice), 
                    "PurshasePrice is not as expected in DB");
                Checker.CheckEquals(Parsing.ConvertToDouble(positionsSharesDb[i]), Parsing.ConvertToDouble(positionsDbData[i].Shares), "Shares is not as expected in DB");
            }

            LogStep(5, 6, "Open portfolios grid. Click Edit sign for the portfolio.Click Update Credentials button." +
                "Change creds for Investments with creds Account20 / Password20.Close success popup");
            PortfoliosSetUp.UpdatePortfolioWithDagSiteInvestment20(portfolioId);

            LogStep(7, "Open Positions & Alerts page -> Positions tab Select Portfolio with name 'xx3001'");
            new MainMenuNavigation().OpenPositionsGrid();
            var positionsAlertsStatisticsPanelForm = new PositionsAlertsStatisticsPanelForm();
            positionsAlertsStatisticsPanelForm.SelectPortfolio(portfoliosNames[0]);

            LogStep(8, "Check that grid does not contain any positions from step 3");
            var positionsIdsAfterRefresh = positionsTabForm.GetPositionsIds();
            foreach (var positionId in positionsIds)
            {
                Checker.IsFalse(positionsIdsAfterRefresh.Contains(positionId), $"8 Grid contains position with ID {positionId} in {portfoliosNames[0]}");
            }

            LogStep(9, "Select Portfolio with name 'x4434'");
            positionsAlertsStatisticsPanelForm.SelectPortfolio(portfoliosNames[1]);

            LogStep(10, "Check that grid does not contain any positions from step 3");
            var positionsIdsInAnotherPortfolio = positionsTabForm.GetPositionsIds();
            foreach (var positionId in positionsIds)
            {
                Checker.IsFalse(positionsIdsInAnotherPortfolio.Contains(positionId), $"10 Grid contains position with ID {positionId} in {portfoliosNames[1]}");
            }

            LogStep(11, "open Closed position grid (for second portfolio)");
            positionsAlertsStatisticsPanelForm.ActivateTab(PositionsTabs.ClosedPositions);
            var closedPositionsTabForm = new ClosedPositionsTabForm();
            var positionsIdsClosed = closedPositionsTabForm.GetPositionsIds();
            Checker.CheckEquals(0, positionsIdsClosed.Count, $"Closed position grid is not empty for {portfoliosNames[1]}");

            LogStep(12, "Select Portfolio with name 'xx3001'.Select 'Custom' range. From 01/01/2016 To today.");
            positionsAlertsStatisticsPanelForm.SelectPortfolio(portfoliosNames[0]);
            closedPositionsTabForm.SelectCustomPeriodRangeWithStartEndDates(StartDateForCustomDateRangeFilter, DateTime.Now.ToShortDateString());

            LogStep(13, "Compare values for positions in the grid ");
            var positionsEntryDateForClosedPositions = new List<string>();
            var positionsEntryPriceForClosedPositions = new List<string>();
            var positionsSharesForClosedPositions = new List<string>();
            var positionsTypeForClosedPositions = new List<string>();
            var exitDatesForClosedPositions = new List<string>();
            var exitPricesForClosedPositions = new List<string>();
            foreach (var positionId in positionsIds)
            {
                positionsEntryDateForClosedPositions.Add(
                    closedPositionsTabForm.GetPositionsGridCellValue(new TableCellMetrics { PositionId = positionId, ColumnHeader = ClosedPositionsGridDataField.EntryDate.GetStringMapping() }));
                positionsEntryPriceForClosedPositions.Add(Constants.DecimalNumberRegex.Match(
                    closedPositionsTabForm.GetPositionsGridCellValue(new TableCellMetrics { PositionId = positionId, ColumnHeader = ClosedPositionsGridDataField.EntryPrice.GetStringMapping() }))
                    .Value);
                positionsSharesForClosedPositions.Add(
                    closedPositionsTabForm.GetPositionsGridCellValue(new TableCellMetrics { PositionId = positionId, ColumnHeader = ClosedPositionsGridDataField.Shares.GetStringMapping() }));
                positionsTypeForClosedPositions.Add(
                    closedPositionsTabForm.GetPositionsGridCellValue(new TableCellMetrics { PositionId = positionId, ColumnHeader = ClosedPositionsGridDataField.TradeType.GetStringMapping() }));
                exitDatesForClosedPositions.Add(
                    closedPositionsTabForm.GetPositionsGridCellValue(new TableCellMetrics { PositionId = positionId, ColumnHeader = ClosedPositionsGridDataField.ExitDate.GetStringMapping() }));
                exitPricesForClosedPositions.Add(Constants.DecimalNumberRegex.Match(
                    closedPositionsTabForm.GetPositionsGridCellValue(new TableCellMetrics { PositionId = positionId, ColumnHeader = ClosedPositionsGridDataField.ExitPrice.GetStringMapping() }))
                    .Value);
            }
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(positionsEntryDates, positionsEntryDateForClosedPositions),
                $"Closed Entry Dates are not as expected\n{GetExpectedResultsString(positionsEntryDates)}\r\n{GetActualResultsString(positionsEntryDateForClosedPositions)}");
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(positionsEntryPrices, positionsEntryPriceForClosedPositions),
                $"Closed Entry Prices are not as expected\n{GetExpectedResultsString(positionsEntryPrices)}\r\n{GetActualResultsString(positionsEntryPriceForClosedPositions)}");
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(positionsShares, positionsSharesForClosedPositions),
                $"Closed Shares are not as expected\n{GetExpectedResultsString(positionsShares)}\r\n{GetActualResultsString(positionsSharesForClosedPositions)}");
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(positionsTypes, positionsTypeForClosedPositions),
                $"Closed Types are not as expected\n{GetExpectedResultsString(positionsTypes)}\r\n{GetActualResultsString(positionsTypeForClosedPositions)}");
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(exitDatesForClosedPositions, positionsExitDates),
                $"Closed Exit Date are not as expected\n{GetExpectedResultsString(positionsExitDates)}\r\n{GetActualResultsString(exitDatesForClosedPositions)}");
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(exitPricesForClosedPositions, positionsExitPrices),
                $"Closed Exit Price are not as expected\n{GetExpectedResultsString(exitPricesForClosedPositions)}\r\n{GetActualResultsString(positionsExitPrices)}");

            LogStep(14, "Compare values in dbo.Positions for recognized positions ");
            var positionsDbDataForClosedPositions = positionsIds.Select(positionId => new PositionsQueries().SelectAllPositionData(positionId)).ToList();
            for (int i = 0; i < positionsDbData.Count; i++)
            {
                Checker.CheckEquals(Parsing.ConvertToShortDateString(positionsEntryDates[i]), Parsing.ConvertToShortDateString(positionsDbDataForClosedPositions[i].PurchaseDate),
                    $"14PurshaseDate is not as expected in DB for {positionsDbDataForClosedPositions[i].Symbol}");
                Checker.CheckEquals(Parsing.ConvertToDouble(positionsEntryPrices[i]), Parsing.ConvertToDouble(positionsDbDataForClosedPositions[i].PurchasePrice),
                    $"14PurshasePrice is not as expected in DB for {positionsDbDataForClosedPositions[i].Symbol}");
                Checker.CheckEquals((int)positionsTypes[i].ParseAsEnumFromStringMapping<PositionTradeTypes>(), Parsing.ConvertToInt(positionsDbDataForClosedPositions[i].TradeType),
                    $"14TradeType is not as expected in DB for {positionsDbDataForClosedPositions[i].Symbol}");
                Checker.CheckEquals((int)AutotestPositionStatusTypes.Close, Parsing.ConvertToInt(positionsDbDataForClosedPositions[i].StatusType),
                    $"14StatusType is not as expected in DB for {positionsDbDataForClosedPositions[i].Symbol}");
                Checker.CheckEquals(Parsing.ConvertToShortDateString(positionsExitDates[i]), Parsing.ConvertToShortDateString(positionsDbDataForClosedPositions[i].CloseDate),
                    $"14CloseDate is not as expected in DB for {positionsDbDataForClosedPositions[i].Symbol}");
                Checker.CheckEquals(Parsing.ConvertToDouble(positionsExitPrices[i]), Parsing.ConvertToDouble(positionsDbDataForClosedPositions[i].ClosePrice),
                    $"14ExitPrice is not as expected in DB for {positionsDbDataForClosedPositions[i].Symbol}");
                Checker.CheckEquals(positionsVendorSymbols[i], positionsDbDataForClosedPositions[i].VendorSymbol, "VendorSymbol is not as expected in DB");
                Checker.CheckEquals(Parsing.ConvertToDouble(positionsSharesDb[i]), Parsing.ConvertToDouble(positionsDbDataForClosedPositions[i].Shares), 
                    "Shares is not as expected in DB");
            }
        }
    }
}