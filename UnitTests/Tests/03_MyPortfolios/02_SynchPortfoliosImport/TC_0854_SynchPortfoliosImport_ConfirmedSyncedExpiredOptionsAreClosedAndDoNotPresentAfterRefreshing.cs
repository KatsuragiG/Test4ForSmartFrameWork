using System;
using System.Collections.Generic;
using System.Linq;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.ImportedPositions;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.PositionCard;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Forms.Popups;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.PositionsAlertsForm.Models;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms;
using AutomatedTests.Models.PositionsModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._03_MyPortfolios._02_SynchPortfoliosImport
{
    [TestClass]
    public class TC_0854_SynchPortfoliosImport_ConfirmedSyncedExpiredOptionsAreClosedAndDoNotPresentAfterRefreshing : BaseTestUnitTests
    {
        private const int TestNumber = 854;
        private const string DateFrom = "01/01/2016";

        private Dictionary<int, string> idsWithOptionVariants;
        private readonly List<AddPositionAdvancedModel> allExpectedPositions = new List<AddPositionAdvancedModel>();
        private PositionGridModel expectedDataForRecognizedOptionPosition;
        private List<int> allPositionsPrevious = new List<int>();
        private int positionsQuantity;
        
        [TestInitialize]
        public void TestInitialize()
        {
            idsWithOptionVariants = new Dictionary<int, string>();
            positionsQuantity = GetTestDataAsInt(nameof(positionsQuantity));
            for (int i = 1; i <= positionsQuantity; i++)
            {
                allExpectedPositions.Add(new AddPositionAdvancedModel
                {
                    Ticker = GetTestDataAsString($"Symbol{i}"),
                    Contracts = GetTestDataAsString($"Quantity{i}"),
                    IsLongTradeType = !GetTestDataAsBool($"IsShort{i}"),
                    EntryPrice = GetTestDataAsString($"OpenPrice{i}"),
                    ExpirationDate = GetTestDataAsString($"ExpirationDate{i}"),
                    StrikePrice = GetTestDataAsString($"StrikePrice{i}"),
                    OptionType = GetTestDataAsString($"StrikeType{i}"),
                    OptionVariant = GetTestDataAsString($"YodleeSymbol{i}"),
                    EntryDate = GetTestDataAsString($"OpenDate{i}"),
                    Notes = GetTestDataAsString($"hdTicker{i}"),
                });
            }
            var symbolExample = allExpectedPositions.First();
            expectedDataForRecognizedOptionPosition = new PositionGridModel
            {
                Ticker = symbolExample.Ticker,
                Name = symbolExample.Ticker,
                Shares = symbolExample.Contracts,
                TradeType = symbolExample.IsLongTradeType.ToString(),
                EntryDate = symbolExample.EntryDate,
                EntryPrice = Constants.DefaultCustomTsAlertValue,
                ExitDate = symbolExample.EntryDate,
                ExitPrice = Constants.DefaultCustomTsAlertValue
            };

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPlus));
            LoginSetUp.LogIn(UserModels.First());

            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);
        }
        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_854$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PortfoliosPage"), TestCategory("SyncPortfolio"), TestCategory("SyncPositions"), TestCategory("SyncPositionEditing")]
        [Description("The test checks that Confirming imported expired options causes it closing and does not cause re-appearing options " +
            "after refreshing https://tr.a1qa.com/index.php?/cases/view/19232269")]
        public override void RunTest()
        {
            LogStep(1, "Import portfolio");
            PortfoliosSetUp.ImportDagSiteInvestment21(true);
            var portfoliosForm = new PortfoliosForm();
            portfoliosForm.AssertIsOpen();

            LogStep(2, "Open 'open positions grid' for the portfolio");
            new MainMenuNavigation().OpenPositionsGrid();
            var positionsTabForm = new PositionsTabForm();
            var positionsImportedQuantity = positionsTabForm.GetNumberOfRowsInGrid();
            Assert.IsTrue(positionsImportedQuantity > 0, "no imported positions");

            var portfolioId = new PortfoliosQueries().SelectPortfolioIdForLastPortfolioByUserEmail(UserModels.First().Email);
            var importedPositionsIds = new PositionsQueries().SelectImportedPositionsIds(portfolioId);
            var importedPositionsQueries = new ImportedPositionsQueries();
            foreach (var positionId in importedPositionsIds)
            {
                idsWithOptionVariants.Add(positionId, importedPositionsQueries.SelectAllImportedPositionDataByPositionId(positionId).ParsedHoldingSymbol);
            }

            LogStep(8, "Repeat steps 2-7 for others options");
            Assert.IsTrue(idsWithOptionVariants.Count > 0, "There are no expired options");
            var mainMenuNavigation = new MainMenuNavigation();

            foreach (var idsWithOptionVariant in idsWithOptionVariants)
            {
                LogStep(3, "Click on position with id corresponded to VendorSymbol");
                var countBefore = positionsTabForm.GetNumberOfRowsInGrid();
                positionsTabForm.ClickOnPositionLink(idsWithOptionVariant.Value.Substring(0, 4));
                var confirmPositionPopup = new ConfirmPositionPopup();
                var position = allExpectedPositions.First(z => z.OptionVariant == idsWithOptionVariant.Value);

                LogStep(4, "Check prefilled data on the form");
                Checker.CheckEquals(position.Ticker, confirmPositionPopup.GetSymbol(), $"Symbol for {idsWithOptionVariant.Value} is not as expected");
                Checker.CheckEquals(Parsing.ConvertToShortDateString(position.ExpirationDate), Parsing.ConvertToShortDateString(confirmPositionPopup.GetExpirationDate()), 
                    $"ExpirationDate for {idsWithOptionVariant.Value} is not as expected");
                Checker.CheckEquals(position.StrikePrice, confirmPositionPopup.GetStrikePrice(), $"Strike Price for {idsWithOptionVariant.Value} is not as expected");
                Checker.CheckEquals(position.OptionType.ToLower(), confirmPositionPopup.GetOptionType().ToLower(), 
                    $"StrikeType for {idsWithOptionVariant.Value} is not as expected");

                confirmPositionPopup.SetSymbol(position.Ticker);
                confirmPositionPopup.SetExpirationDate(DateTime.Parse(position.ExpirationDate).ToString("dd-MMM-yy").TrimStart('0'));
                confirmPositionPopup.SetStrikePrice(position.StrikePrice);
                confirmPositionPopup.SetOptionType(position.OptionType.ToLower().Replace("c", "C").Replace("p", "P"));
                Checker.CheckEquals(position.Notes, confirmPositionPopup.GetOptionVariant(), $"OptionVariant for {idsWithOptionVariant.Value} is not as expected");

                LogStep(5, "Click Save");
                confirmPositionPopup.ClickOkButton();
                var positionDetailsTabForm = new PositionDetailsTabPositionCardForm();
                positionDetailsTabForm.SetValueInTextBoxField(PositionDetailsFieldTypes.EntryDate, DateTime.Parse(position.EntryDate).ToShortDateString());
                positionDetailsTabForm.ClickGetQuote();
                var positionCardForm = new PositionCardForm();
                positionCardForm.ClickSave();
                positionCardForm.ClickOnPortfolioLink();

                var positionsAlertsStatisticsPanelForm = new PositionsAlertsStatisticsPanelForm();
                positionsAlertsStatisticsPanelForm.ActivateTab(PositionsTabs.OpenPositions);
                Checker.CheckEquals(countBefore - 1, positionsTabForm.GetNumberOfRowsInGrid(), "Position is present");

                LogStep(6, "Click Closed position tab");
                positionsAlertsStatisticsPanelForm.ActivateTab(PositionsTabs.ClosedPositions);
                var closedPositionsTabForm = new ClosedPositionsTabForm();
                closedPositionsTabForm.SelectCustomPeriodRangeWithStartEndDates(DateFrom, DateTime.Now.ToShortDateString());

                var allPositionsCurrent = closedPositionsTabForm.GetPositionsIds();
                var positionId = allPositionsCurrent.First(u => !allPositionsPrevious.Contains(u));
                allPositionsPrevious = allPositionsCurrent;
                var positionClosed = closedPositionsTabForm.GetBasicPositionData(positionId, expectedDataForRecognizedOptionPosition);

                LogStep(7, "Check in the grid data for option:");
                Checker.CheckEquals(position.Notes, positionClosed.Ticker.Split('\r')[0], 
                    $"Closed Symbol for {idsWithOptionVariant.Value} is not as expected");
                Checker.IsFalse(string.IsNullOrEmpty(positionClosed.Name), $"Closed Name for {idsWithOptionVariant.Value} is not as expected");
                Checker.CheckEquals(Parsing.ConvertToDouble(position.EntryPrice), Parsing.ConvertToDouble(positionClosed.EntryPrice.Replace("$", string.Empty)), 
                    $"Closed EntryPrice for {idsWithOptionVariant.Value} is not as expected");
                Checker.CheckEquals(Parsing.ConvertToShortDateString(position.EntryDate), Parsing.ConvertToShortDateString(positionClosed.EntryDate), 
                    $"Closed EntryDate for {idsWithOptionVariant.Value} is not as expected");
                Checker.CheckEquals(Parsing.ConvertToShortDateString(position.ExpirationDate), Parsing.ConvertToShortDateString(positionClosed.ExitDate), 
                    $"Closed ExitDate for {idsWithOptionVariant.Value} is not as expected");
                Checker.CheckEquals("$0.00", positionClosed.ExitPrice, $"Closed ExitPrice for {idsWithOptionVariant.Value} is not as expected");
                Checker.CheckEquals(Math.Abs(Parsing.ConvertToDouble(position.Contracts) * 100), Math.Abs(Parsing.ConvertToDouble(positionClosed.Shares)), 
                    $"Closed Contracts for {idsWithOptionVariant.Value} is not as expected");
                Checker.CheckEquals(position.IsLongTradeType, positionClosed.TradeType.ToLower().Equals("long"), $"Closed LongType for {idsWithOptionVariant.Value} is not as expected");

                positionsAlertsStatisticsPanelForm.ActivateTab(PositionsTabs.OpenPositions);
            }

            LogStep(9, "Open portfolio grid Click Refresh button for the portfolio Click OK");
            mainMenuNavigation.OpenPortfolios();
            portfoliosForm.ClickPortfolioTypeTab(PortfolioType.Investment);
            new PortfolioGridsSteps().ClickRefreshPortfolioIdViaSyncFlow(portfolioId);

            LogStep(10, "Open open positions grid for the portfolio *empty* open positions grid is shown");
            mainMenuNavigation.OpenPositionsGrid();
            Checker.CheckEquals(0, positionsTabForm.GetNumberOfRowsInGrid(), "number of positions is not 0");
        }
    }
}