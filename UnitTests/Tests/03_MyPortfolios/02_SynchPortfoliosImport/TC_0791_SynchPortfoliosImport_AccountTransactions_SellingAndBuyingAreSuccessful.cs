using System.Collections.Generic;
using System.Linq;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.Sorting;
using AutomatedTests.Enums;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;
using AutomatedTests.Models;

namespace UnitTests.Tests._03_MyPortfolios._02_SynchPortfoliosImport
{
    [TestClass]
    public class TC_0791_SynchPortfoliosImport_AccountTransactions_SellingAndBuyingAreSuccessful : BaseTestUnitTests
    {
        private const int TestNumber = 791;

        private string portfolioName;
        private int positionsQuantity;
        private int positionsClosedQuantity;
        private readonly List<string> positionsSymbols = new List<string>();
        private readonly List<string> closedPositionsSymbols = new List<string>();
        private readonly List<string> positionsEntryDates = new List<string>();
        private readonly List<string> positionsEntryPrices = new List<string>();
        private readonly List<string> closedPositionsEntryDates = new List<string>();
        private readonly List<string> closedPositionsEntryPrices = new List<string>();
        private readonly List<string> positionsShares = new List<string>();
        private readonly List<string> closedPositionsShares = new List<string>();
        private readonly List<string> positionsFinalShares = new List<string>();
        private readonly List<string> positionsTypes = new List<string>();
        private readonly List<string> closedPositionsTypes = new List<string>();
        private readonly List<string> closedPositionsExitDates = new List<string>();
        private readonly List<string> closedPositionsExitPrices = new List<string>();
        private readonly List<Currency> positionsCurrencies = new List<Currency>();

        [TestInitialize]
        public void TestInitialize()
        {
            portfolioName = GetTestDataAsString("PortfolioName1");
            positionsQuantity = GetTestDataAsInt(nameof(positionsQuantity));
            var entryDate = GetTestDataAsString("InitialPositionsEntryDates");
            var exitDate = GetTestDataAsString("ClosedPositionsExitDates");
            for (int i = 1; i <= positionsQuantity; i++)
            {
                positionsSymbols.Add(GetTestDataAsString($"InitialPositionSymbol{i}"));
                positionsEntryDates.Add(entryDate);
                positionsEntryPrices.Add(GetTestDataAsString($"InitialPositionsEntryPrices{i}"));
                positionsShares.Add(GetTestDataAsString($"InitialPositionsShares{i}"));
                positionsFinalShares.Add(GetTestDataAsString($"PositionsFinalShares{i}"));
                positionsTypes.Add(GetTestDataAsString($"InitialPositionsTypes{i}"));
                positionsCurrencies.Add(GetTestDataParsedAsEnumFromStringMapping<Currency>($"InitialPositionsCurrencies{i}"));
            }
            positionsClosedQuantity = GetTestDataAsInt(nameof(positionsClosedQuantity));
            for (int i = 1; i <= positionsClosedQuantity; i++)
            {
                closedPositionsSymbols.Add(GetTestDataAsString($"ClosedPositionsSymbols{i}"));
                closedPositionsEntryDates.Add(entryDate);
                closedPositionsEntryPrices.Add(GetTestDataAsString($"ClosedPositionsEntryPrices{i}"));
                closedPositionsShares.Add(GetTestDataAsString($"ClosedPositionsShares{i}"));
                closedPositionsTypes.Add(GetTestDataAsString($"ClosedPositionsTypes{i}"));
                closedPositionsExitDates.Add(exitDate);
                closedPositionsExitPrices.Add(GetTestDataAsString($"ClosedPositionsExitPrices{i}"));
            }

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPlatinum));
            LoginSetUp.LogIn(UserModels.First());

            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_791$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PortfoliosPage"), TestCategory("SyncPortfolio"), TestCategory("SyncPositions"), TestCategory("SyncPortfolioImport")]
        [Description("Test checks that syncing of imported portfolio with new transactions cause their applying to positions data without fully position closing as expected." +
            "Checked transactions:Buy for long;Sell for short;TransferSharesIn. https://tr.a1qa.com/index.php?/cases/view/19232325")]
        public override void RunTest()
        {
            LogStep(1, "Import portfolio");
            PortfoliosSetUp.ImportDagSiteInvestment15(true);
            var portfolioId = new PortfoliosQueries().SelectPortfolioId(UserModels.First().Email, portfolioName, PortfolioType.Investment);
            var portfolioForm = new PortfoliosForm();

            LogStep(2, "Open Positions & Alerts page -> Positions tab Select Portfolio with name 'xx3001'");
            new MainMenuNavigation().OpenPositionsGrid();
            new PositionsAlertsStatisticsPanelForm().SelectPortfolio(portfolioName);
            var positionsTabForm = new PositionsTabForm();
            var positionsIds = positionsSymbols.Select(symbol => positionsTabForm.GetPositionsIdsByTicker(symbol)[0]).ToList();

            LogStep(3, "Compare values for positions in the grid ");
            var positionsEntryDate = new List<string>();
            var positionsEntryPrice = new List<string>();
            var positionsSharesGrid = new List<string>();
            var positionsType = new List<string>();
            foreach (var positionId in positionsIds)
            {
                positionsEntryDate.Add(positionsTabForm.GetPositionsGridCellValue(
                    new TableCellMetrics { PositionId = positionId, ColumnHeader = PositionsGridDataField.EntryDate.GetStringMapping() }));
                positionsEntryPrice.Add(Constants.DecimalNumberRegex.Match(
                        positionsTabForm.GetPositionsGridCellValue(
                            new TableCellMetrics { PositionId = positionId, ColumnHeader = PositionsGridDataField.EntryPrice.GetStringMapping() }))
                    .Value);
                positionsSharesGrid.Add(positionsTabForm.GetPositionsGridCellValue(
                    new TableCellMetrics { PositionId = positionId, ColumnHeader = PositionsGridDataField.Shares.GetStringMapping() }));
                positionsType.Add(positionsTabForm.GetPositionsGridCellValue(
                    new TableCellMetrics { PositionId = positionId, ColumnHeader = PositionsGridDataField.TradeType.GetStringMapping() }));
            }
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(positionsEntryDates, positionsEntryDate),
                $"Initial Entry Dates are not as expected\n{GetExpectedResultsString(positionsEntryDates)}\r\n{GetActualResultsString(positionsEntryDate)}");
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(positionsEntryPrices, positionsEntryPrice),
                $"Initial Entry Prices are not as expected\n{GetExpectedResultsString(positionsEntryPrices)}\r\n{GetActualResultsString(positionsEntryPrice)}");
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(positionsShares, positionsSharesGrid),
                $"Initial Shares are not as expected\n{GetExpectedResultsString(positionsShares)}\r\n{GetActualResultsString(positionsSharesGrid)}");
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(positionsTypes, positionsType),
                $"Initial Types are not as expected\n{GetExpectedResultsString(positionsTypes)}\r\n{GetActualResultsString(positionsType)}");

            LogStep(4, "Compare values in dbo.Positions for positions");
            var positionQueries = new PositionsQueries();
            var positionsDbData = positionsIds.Select(positionId => positionQueries.SelectAllPositionData(positionId)).ToList();
            for (int i = 0; i < positionsDbData.Count; i++)
            {
                Checker.CheckEquals((int)positionsCurrencies[i], Parsing.ConvertToInt(positionsDbData[i].CurrencyId), "4CurrencyId is not as expected in DB");
                Checker.CheckEquals(ReplaceAllVendorPostfixes(positionsSymbols[i].Replace("BTC/USD", "BTC")), 
                    positionsDbData[i].VendorSymbol, "4YodleeSymbol is not as expected in DB");
                Checker.CheckEquals(Parsing.ConvertToShortDateString(positionsEntryDates[i]), Parsing.ConvertToShortDateString(positionsDbData[i].PurchaseDate), 
                    "4PurshaseDate is not as expected in DB");
                Checker.CheckEquals((int)positionsTypes[i].ParseAsEnumFromStringMapping<PositionTradeTypes>(), Parsing.ConvertToInt(positionsDbData[i].TradeType), 
                    "4TradeType is not as expected in DB");
                Checker.CheckEquals((int)AutotestPositionStatusTypes.Open, Parsing.ConvertToInt(positionsDbData[i].StatusType), "4StatusType is not as expected in DB");
            }

            LogStep(5, 6, "Open portfolios grid. Click Edit sign for the portfolio.Click Update Credentials button." +
                "Change creds for Investments with creds Account20 / Password20.Close success popup");
            PortfoliosSetUp.UpdatePortfolioWithDagSiteInvestment20(portfolioId);

            LogStep(7, "Open Positions & Alerts page -> Positions tab Select Portfolio with name 'xx3001'");
            portfolioForm.ClickOnPortfolioName(portfolioName);

            LogStep(8, "Compare values for positions in the grid");
            var positionsEntryDateAfterRefresh = new List<string>();
            var positionsEntryPriceAfterRefresh = new List<string>();
            var positionsSharesGridAfterRefresh = new List<string>();
            var positionsTypeAfterRefresh = new List<string>();
            foreach (var positionId in positionsIds)
            {
                positionsEntryDateAfterRefresh.Add(positionsTabForm.GetPositionsGridCellValue(
                    new TableCellMetrics { PositionId = positionId, ColumnHeader = PositionsGridDataField.EntryDate.GetStringMapping() }));
                positionsEntryPriceAfterRefresh.Add(Constants.DecimalNumberRegex.Match(
                    positionsTabForm.GetPositionsGridCellValue(
                        new TableCellMetrics { PositionId = positionId, ColumnHeader = PositionsGridDataField.EntryPrice.GetStringMapping() }))
                    .Value);
                positionsSharesGridAfterRefresh.Add(positionsTabForm.GetPositionsGridCellValue(
                    new TableCellMetrics { PositionId = positionId, ColumnHeader = PositionsGridDataField.Shares.GetStringMapping() }));
                positionsTypeAfterRefresh.Add(positionsTabForm.GetPositionsGridCellValue(
                    new TableCellMetrics { PositionId = positionId, ColumnHeader = PositionsGridDataField.TradeType.GetStringMapping() }));
            }
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(positionsEntryDates, positionsEntryDateAfterRefresh),
                $"Final Entry Dates are not as expected for {portfolioName}\n{GetExpectedResultsString(positionsEntryDates)}\r\n{GetActualResultsString(positionsEntryDateAfterRefresh)}");
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(positionsEntryPrices, positionsEntryPriceAfterRefresh),
                $"Final Entry Prices are not as expected for {portfolioName}\n{GetExpectedResultsString(positionsEntryPrices)}\r\n{GetActualResultsString(positionsEntryPriceAfterRefresh)}");
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(positionsFinalShares, positionsSharesGridAfterRefresh),
                $"Final Shares are not as expected for {portfolioName}\n{GetExpectedResultsString(positionsFinalShares)}\r\n{GetActualResultsString(positionsSharesGridAfterRefresh)}");
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(positionsTypes, positionsTypeAfterRefresh),
                $"Final Types are not as expected for {portfolioName}\n{GetExpectedResultsString(positionsTypes)}\r\n{GetActualResultsString(positionsTypeAfterRefresh)}");

            LogStep(9, "Compare values in dbo.Positions for recognized positions ");
            var positionsDbDataAfterRefresh = positionsIds.Select(positionId => positionQueries.SelectAllPositionData(positionId)).ToList();
            for (int i = 0; i < positionsDbData.Count; i++)
            {
                Checker.CheckEquals((int)positionsCurrencies[i], Parsing.ConvertToInt(positionsDbDataAfterRefresh[i].CurrencyId), "9CurrencyId is not as expected in DB");
                Checker.CheckEquals(ReplaceAllVendorPostfixes(positionsSymbols[i].Replace("BTC/USD", "BTC")), 
                    positionsDbDataAfterRefresh[i].VendorSymbol, 
                    "9YodleeSymbol is not as expected in DB");
                Checker.CheckEquals(Parsing.ConvertToShortDateString(positionsEntryDates[i]),
                    Parsing.ConvertToShortDateString(positionsDbDataAfterRefresh[i].PurchaseDate),
                    "9PurshaseDate is not as expected in DB");
                Checker.CheckEquals((int)positionsTypes[i].ParseAsEnumFromStringMapping<PositionTradeTypes>(), Parsing.ConvertToInt(positionsDbDataAfterRefresh[i].TradeType), 
                    "9TradeType is not as expected in DB");
                Checker.CheckEquals((int)AutotestPositionStatusTypes.Open, Parsing.ConvertToInt(positionsDbDataAfterRefresh[i].StatusType),  "9StatusType is not as expected in DB");
            }

            LogStep(10, "Open Closed position grid (for the same portfolio xx3001)");
            new PositionsAlertsStatisticsPanelForm().ActivateTab(PositionsTabs.ClosedPositions);
            var closedPositionsTabForm = new ClosedPositionsTabForm();
            closedPositionsTabForm.SelectPeriod(GridFilterPeriods.Last12Months);
            closedPositionsTabForm.ClickOnPositionColumnToSort(ClosedPositionsGridDataField.Ticker, SortingStatus.Asc);
            var closedPositionsIds = (closedPositionsSymbols.SelectMany(symbol => 
                positionQueries.SelectPositionIdByUserEmailWithSymbol(UserModels.First().Email, symbol)
                    .Where(positionIds => 
                        Parsing.ConvertToInt(positionQueries.SelectPositionStatusTypeByPositionId(positionIds)) == 
                        (int)AutotestPositionStatusTypes.Close))).ToList();

            LogStep(11, "Compare values for positions in the grid ");
            var positionsEntryDateForClosedPositions = new List<string>();
            var positionsEntryPriceForClosedPositions = new List<string>();
            var positionsSharesGridForClosedPositions = new List<string>();
            var positionsTypeForClosedPositions = new List<string>();
            var positionExitDate = new List<string>();
            var positionExitPrice = new List<string>();
            foreach (var closedPositionId in closedPositionsIds)
            {
                positionsEntryDateForClosedPositions.Add(
                    closedPositionsTabForm.GetPositionsGridCellValue(
                        new TableCellMetrics { PositionId = closedPositionId, ColumnHeader = ClosedPositionsGridDataField.EntryDate.GetStringMapping() }));
                positionsEntryPriceForClosedPositions.Add(Constants.DecimalNumberRegex.Match(
                    closedPositionsTabForm.GetPositionsGridCellValue(
                        new TableCellMetrics { PositionId = closedPositionId, ColumnHeader = ClosedPositionsGridDataField.EntryPrice.GetStringMapping() }))
                    .Value);
                positionsSharesGridForClosedPositions.Add(
                    closedPositionsTabForm.GetPositionsGridCellValue(
                        new TableCellMetrics { PositionId = closedPositionId, ColumnHeader = ClosedPositionsGridDataField.Shares.GetStringMapping() }));
                positionsTypeForClosedPositions.Add(
                    closedPositionsTabForm.GetPositionsGridCellValue(
                        new TableCellMetrics { PositionId = closedPositionId, ColumnHeader = ClosedPositionsGridDataField.TradeType.GetStringMapping() }));
                positionExitDate.Add(
                    closedPositionsTabForm.GetPositionsGridCellValue(
                        new TableCellMetrics { PositionId = closedPositionId, ColumnHeader = ClosedPositionsGridDataField.ExitDate.GetStringMapping() }));
                positionExitPrice.Add(Constants.DecimalNumberRegex.Match(
                    closedPositionsTabForm.GetPositionsGridCellValue(
                        new TableCellMetrics { PositionId = closedPositionId, ColumnHeader = ClosedPositionsGridDataField.ExitPrice.GetStringMapping() }))
                    .Value);
            }
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(closedPositionsEntryDates, positionsEntryDateForClosedPositions),
                $"Closed Entry Dates are not as expected\n{GetExpectedResultsString(closedPositionsEntryDates)}\r\n{GetActualResultsString(positionsEntryDateForClosedPositions)}");
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(closedPositionsEntryPrices, positionsEntryPriceForClosedPositions),
                $"Closed Entry Prices are not as expected\n{GetExpectedResultsString(closedPositionsEntryPrices)}\r\n{GetActualResultsString(positionsEntryPriceForClosedPositions)}");
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(closedPositionsShares, positionsSharesGridForClosedPositions),
                $"Closed Shares are not as expected\n{GetExpectedResultsString(closedPositionsShares)}\r\n{GetActualResultsString(positionsSharesGridForClosedPositions)}");
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(closedPositionsTypes, positionsTypeForClosedPositions),
                $"Closed Types are not as expected\n{GetExpectedResultsString(closedPositionsTypes)}\r\n{GetActualResultsString(positionsTypeForClosedPositions)}");
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(closedPositionsExitDates, positionExitDate),
                $"Closed Exit Date are not as expected\n{GetExpectedResultsString(closedPositionsExitDates)}\r\n{GetActualResultsString(positionExitDate)}");
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(closedPositionsExitPrices, positionExitPrice),
                $"Closed Exit Price are not as expected\n{GetExpectedResultsString(closedPositionsExitPrices)}\r\n{GetActualResultsString(positionExitPrice)}");

            LogStep(12, "Compare values in dbo.Positions for recognized positions ");
            var positionsDbData3 = closedPositionsIds.Select(positionId => new PositionsQueries().SelectAllPositionData(positionId)).ToList();
            for (int i = 0; i < positionsDbData3.Count; i++)
            {
                Checker.CheckEquals(Parsing.ConvertToShortDateString(closedPositionsEntryDates[i]), Parsing.ConvertToShortDateString(positionsDbData3[i].PurchaseDate), 
                    $"12PurshaseDate is not as expected in DB for {positionsDbData3[i].Symbol}");
                Checker.CheckEquals(Parsing.ConvertToDouble(closedPositionsEntryPrices[i]), Parsing.ConvertToDouble(positionsDbData3[i].PurchasePrice), 
                    $"12PurshasePrice is not as expected in DB for {positionsDbData3[i].Symbol}");
                Checker.CheckEquals((int)closedPositionsTypes[i].ParseAsEnumFromStringMapping<PositionTradeTypes>(), Parsing.ConvertToInt(positionsDbData3[i].TradeType), 
                    $"12TradeType is not as expected in DB for {positionsDbData3[i].Symbol}");
                Checker.CheckEquals((int)AutotestPositionStatusTypes.Close, Parsing.ConvertToInt(positionsDbData3[i].StatusType),
                    $"12StatusType is not as expected in DB for {positionsDbData3[i].Symbol}");
                Checker.CheckEquals(Parsing.ConvertToShortDateString(closedPositionsExitDates[i]), Parsing.ConvertToShortDateString(positionsDbData3[i].CloseDate), 
                    $"12CloseDate is not as expected in DB for {positionsDbData3[i].Symbol}");
                Checker.CheckEquals(Parsing.ConvertToDouble(closedPositionsExitPrices[i]), Parsing.ConvertToDouble(positionsDbData3[i].ClosePrice), 
                    $"12ExitPrice is not as expected in DB for {positionsDbData3[i].Symbol}");
            }
        }

        private string ReplaceAllVendorPostfixes(string text)
        {
            return text.Replace("FRES-L", "FRES").Replace("-CN", "").Replace(".AX", "").Replace(".F", "").Replace(".SG", "").Replace("-T", "")
                        .Replace("ROX-V", "ROX").Replace("WES", "WES.AX").Replace("V210115P00090000", "V220116P00090000").Replace("FB210115P00085000", "FB220116P00085000");
        }
    }
}