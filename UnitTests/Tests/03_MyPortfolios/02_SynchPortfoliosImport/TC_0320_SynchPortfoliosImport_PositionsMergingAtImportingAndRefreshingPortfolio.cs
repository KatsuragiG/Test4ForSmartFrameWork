using System.Collections.Generic;
using System.Linq;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.PositionCard;
using AutomatedTests.Forms.Popups;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.PositionsAlertsForm.Models;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.BaseTestsUnitTests;
using TradeStops.Common.Enums;
using AutomatedTests.Enums.Positions;
using AutomatedTests;
using AutomatedTests.Enums.Sorting;
using AutomatedTests.ConstantVariables;
using System;

namespace UnitTests.Tests._03_MyPortfolios._02_SynchPortfoliosImport
{
    [TestClass]
    public class TC_0320_SynchPortfoliosImport_PositionsMergingAtImportingAndRefreshingPortfolio : BaseTestUnitTests
    {
        private const int TestNumber = 320;

        private List<PositionGridModel> symbols;
        private List<PositionGridModel> unrecognizedSymbols;
        private PositionGridModel expectedDataForMergedPosition;
        private int numberOfSymbols;
        private int numberOfUnrecognizedSymbols;
        private string tickerToRecognize;
        private string tickerToMerge;
        private string tickerToRecognizeOption;
        private string tickerForOption;
        private string expirationForOption;
        private string strikeForOption;
        private string typeForOption;
        private string recognizeTickerEntryDate;

        [TestInitialize]
        public void TestInitialize()
        {
            symbols = new List<PositionGridModel>();
            unrecognizedSymbols = new List<PositionGridModel>();
            numberOfSymbols = GetTestDataAsInt(nameof(numberOfSymbols));
            tickerToRecognize = GetTestDataAsString(nameof(tickerToRecognize));
            tickerToMerge = GetTestDataAsString(nameof(tickerToMerge));
            tickerToRecognizeOption = GetTestDataAsString(nameof(tickerToRecognizeOption));
            tickerForOption = GetTestDataAsString(nameof(tickerForOption));
            expirationForOption = GetTestDataAsString(nameof(expirationForOption));
            strikeForOption = GetTestDataAsString(nameof(strikeForOption));
            typeForOption = GetTestDataAsString(nameof(typeForOption));
            recognizeTickerEntryDate = GetTestDataAsString(nameof(recognizeTickerEntryDate));
            numberOfUnrecognizedSymbols = GetTestDataAsInt(nameof(numberOfUnrecognizedSymbols));

            for (int i = 1; i <= numberOfSymbols; i++)
            {
                symbols.Add(new PositionGridModel
                {
                    Ticker = GetTestDataAsString($"Symbol{i}"),
                    Shares = GetTestDataAsString($"Shares{i}"),
                    TradeType = GetTestDataAsString($"LS{i}")
                });
            }
            for (int i = 1; i <= numberOfUnrecognizedSymbols; i++)
            {
                var position = new PositionGridModel
                {
                    Ticker = GetTestDataAsString($"UnrecognizedSymbol{i}"),
                    Shares = GetTestDataAsString($"UnrecognizedShares{i}"),
                    TradeType = GetTestDataAsString($"UnrecognizedLS{i}")
                };
                unrecognizedSymbols.Add(position);
            }
            var symbolExample = symbols.First();
            expectedDataForMergedPosition = new PositionGridModel
            {
                Ticker = symbolExample.Ticker,
                Shares = symbolExample.Shares,
                TradeType = symbolExample.TradeType,
                EntryDate = Parsing.ConvertToShortDateString(DateTime.Now.ToShortDateString()),
                EntryPrice = Constants.DefaultCustomTsAlertValue
            };

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPremium));
            LoginSetUp.LogIn(UserModels.First());
            new AddPortfoliosSteps().OpenPortfolioCreationFormViaSelectionFlowPage(AddPortfolioTypes.Synchronized);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_320$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PortfoliosPage"), TestCategory("SyncPortfolio"), TestCategory("SyncPortfolioImport"), TestCategory("SyncPortfolioUpdate"), TestCategory("SyncPositions")]
        [Description("Test checks Positions merging at importing and refreshing portfolio https://tr.a1qa.com/index.php?/cases/view/19232716")]
        public override void RunTest()
        {
            LogStep(1, 2, "Import portfolio with Token");
            PortfoliosSetUp.ImportDagSiteToken03(true);
            var portfolioId = new PortfoliosQueries().SelectPortfolioIdForLastPortfolioByUserEmail(UserModels.First().Email);
            Assert.IsTrue(portfolioId != 0, "portfolioId is not found after import");
            var portfolioGridsSteps = new PortfolioGridsSteps();
            var portfolioName = portfolioGridsSteps.RememberPortfolioInformationForPortfolioId(portfolioId).PortfolioName;

            LogStep(3, "Open Positions & Alerts page -> Positions tab Select Portfolio from the dropdown");
            var mainMenuNavigation = new MainMenuNavigation();
            mainMenuNavigation.OpenPositionsGrid();
            var positionsAlertsStatisticsPanelForm = new PositionsAlertsStatisticsPanelForm();
            positionsAlertsStatisticsPanelForm.SelectPortfolio(portfolioName);
            var positionsTab = new PositionsTabForm();
            positionsTab.ClickOnPositionColumnToSort(PositionsGridDataField.Ticker, SortingStatus.Asc);

            LogStep(4, "Check that there are follow positions in grid");
            var positionsInformation = positionsTab.GetPositionDataForAllPositions(Instance.GetListOfDefaultViewPositionsColumnsForPremium());
            var positionOrderForMergingWith = positionsInformation.FindIndex(p => p.Ticker == tickerToMerge && p.TradeType == PositionTradeTypes.Long.ToString()) + 1;

            Assert.IsTrue(positionsInformation.Count > 0, "number of positions more than 0");
            for (var i = 0; i < numberOfSymbols; i++)
            {
                Checker.IsTrue(positionsInformation.Any(position =>
                        (int)Parsing.ConvertToDouble(position.Shares.Replace("-", string.Empty)) == (int)Parsing.ConvertToDouble(symbols[i].Shares)
                        && position.Ticker.Split('\r')[0].Equals(symbols[i].Ticker)),
                    $"Grid doesn't contain shares {symbols[i].Shares} for position {symbols[i].Ticker}");
                Checker.IsTrue(positionsInformation.Any(position => position.TradeType.Equals(symbols[i].TradeType)
                        && position.Ticker.Split('\r')[0].Equals(symbols[i].Ticker)),
                    $"Grid doesn't contain TradeType {symbols[i].TradeType} for position {symbols[i].Ticker}");
            }
            for (var i = 0; i < numberOfUnrecognizedSymbols; i++)
            {
                Checker.IsTrue(positionsInformation.Any(position =>
                        (int)Parsing.ConvertToDouble(position.Shares.Replace("-", string.Empty).Replace(",", string.Empty)) == (int)Parsing.ConvertToDouble(unrecognizedSymbols[i].Shares)
                        && position.Ticker.Split('\r')[0].Equals(unrecognizedSymbols[i].Ticker)),
                    $"Unrecognized Grid doesn't contain shares {unrecognizedSymbols[i].Shares} for position {unrecognizedSymbols[i].Ticker}");
                Checker.IsTrue(positionsInformation.Any(position => position.TradeType.Equals(unrecognizedSymbols[i].TradeType)
                        && position.Ticker.Split('\r')[0].Equals(unrecognizedSymbols[i].Ticker)),
                    $"Unrecognized Grid doesn't contain TradeType {unrecognizedSymbols[i].TradeType} for position {unrecognizedSymbols[i].Ticker}");
            }

            LogStep(5, "Remember Shares, Entry Date and Entry price for AAPL long symbol");
            var positionInfoForMergedTickerBeforeRecognition = positionsInformation[positionOrderForMergingWith - 1];
            var positionOrderForRecognize = positionsInformation.FindIndex(p => p.Ticker == tickerToRecognize) + 1;

            LogStep(6, "Click on Symbol position link in grid for unrecognized position AAPL7");
            positionsTab.ClickOnPositionLink(tickerToRecognize);

            LogStep(7, "Clear Symbol field, type AAPL and select AAPL from autocomplete");
            var confirmPositionPopup = new ConfirmPositionPopup();
            confirmPositionPopup.SetSymbol(tickerToMerge);

            LogStep(8, "Click OK");
            confirmPositionPopup.ClickOkButton();
            var positionCardForm = new PositionCardForm();
            var positionDetailsTabPositionCardForm = new PositionDetailsTabPositionCardForm();

            LogStep(9, "Select Entry date and click Get quote");
            positionDetailsTabPositionCardForm.SetValueInTextBoxField(PositionDetailsFieldTypes.EntryDate, recognizeTickerEntryDate);
            positionDetailsTabPositionCardForm.ClickGetQuote();

            LogStep(10, "Click Save button");
            positionCardForm.ClickSave();
            var newPrice = positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.EntryPrice);
            positionCardForm.ClickOnPortfolioLink();

            LogStep(11, "Check that AAPL long position has shares = Value from step 5 + value from step 6 " +
                "Check that Entry Date and Entry price for AAPL long position is not changed from step 5");
            var positionInfoAfterMergingViaRecognition = positionsTab.GetBasicPositionDataByRowOrder(positionOrderForMergingWith, expectedDataForMergedPosition);
            Checker.CheckEquals(Parsing.ConvertToDouble(positionsInformation[positionOrderForRecognize - 1].Shares) + Parsing.ConvertToDouble(positionInfoForMergedTickerBeforeRecognition.Shares),
                Parsing.ConvertToDouble(positionInfoAfterMergingViaRecognition.Shares),
                "Shares is not step 6 shares + step10 shares");
            Checker.CheckEquals(recognizeTickerEntryDate, positionInfoAfterMergingViaRecognition.EntryDate, "Entry Date changed");
            Checker.CheckEquals(newPrice, positionInfoAfterMergingViaRecognition.EntryPrice, "Entry Price changed");

            LogStep(12, "Click on unrecognized position link for option in grid (AWCN)");
            positionsInformation = positionsTab.GetPositionDataForAllPositions(Instance.GetListOfDefaultViewPositionsColumnsForPremium());
            positionsTab.ClickOnPositionLink(tickerToRecognizeOption);

            LogStep(13, "Clear Symbol field, type ACN and select ACN from autocomplete");
            var confirmPositionPopupOption = new ConfirmPositionPopup();
            confirmPositionPopupOption.CheckExpiredOptionsCheckbox(false);
            confirmPositionPopupOption.SetSymbol(tickerForOption);

            LogStep(14, "Detect empty fields among Expiration date, Strike price and Option Type and select any values from dropdown for these fields");
            confirmPositionPopup.SetExpirationDate(expirationForOption);
            confirmPositionPopup.SetStrikePrice(strikeForOption);
            confirmPositionPopup.SetOptionType(typeForOption);
            confirmPositionPopup.ClickOkButton();
            positionCardForm = new PositionCardForm();
            positionDetailsTabPositionCardForm = new PositionDetailsTabPositionCardForm();

            LogStep(15, "Set Entry date (recognizeTickerEntryDate), Click Get Quote, Save changes in Position card and click Portfolio Link");
            positionDetailsTabPositionCardForm.SetValueInTextBoxField(PositionDetailsFieldTypes.EntryDate, recognizeTickerEntryDate);
            positionDetailsTabPositionCardForm.ClickGetQuote();
            positionCardForm.ClickSave();
            positionCardForm.ClickOnPortfolioLink();

            LogStep(16, "Remember Shares, Entry Date and Entry price for *all* positions in grid");
            var positionsAfterRecognitionAndMerging = positionsTab.GetPositionDataForAllPositions(Instance.GetListOfDefaultViewPositionsColumnsForPremium());
            Checker.CheckEquals(positionsInformation.Count, positionsAfterRecognitionAndMerging.Count, "Options are merged");

            LogStep(17, "Open Portfolios tab");
            mainMenuNavigation.OpenPortfolios();
            var portfoliosForm = new PortfoliosForm();

            LogStep(18, "Remember for imported portfolio quantity of opened positions");
            var orderOfPortfolio = portfoliosForm.GetCountOfPortfolios(AllPortfoliosKinds.Investment);
            var portfoliosColumns = portfoliosForm.GetPortfoliosColumns();
            var portfoliosInfo = portfoliosForm.GetPortfoliosGridRow(orderOfPortfolio, portfoliosColumns);

            LogStep(19, "Click Refresh ");
            portfolioGridsSteps.ClickRefreshPortfolioIdViaSyncFlow(portfolioId);
            portfoliosForm = new PortfoliosForm();

            LogStep(20, "Compare quantity of opened positions in the grid from step 18");
            var portfolioInfoAfterRefresh = portfoliosForm.GetPortfoliosGridRow(orderOfPortfolio, portfoliosColumns);
            Checker.CheckEquals(portfoliosInfo.Positions, portfolioInfoAfterRefresh.Positions,
                "portfolio number of open positions not equals");

            LogStep(21, "Open Positions & Alerts page -> Positions tab Select Portfolio from the dropdown");
            mainMenuNavigation.OpenPositionsGrid();
            positionsTab = new PositionsTabForm();
            positionsAlertsStatisticsPanelForm.SelectPortfolio(portfolioName);

            LogStep(22, "Compare Shares, Entry Date and Entry price for *all* positions in grid with step 19");
            var positionsInformationAfterRefresh = positionsTab.GetPositionDataForAllPositions(Instance.GetListOfDefaultViewPositionsColumnsForPremium());
            Assert.IsTrue(ObjectComparator.AreSharesEntryDatesAndEntryPriceEqualsForPositions(positionsAfterRecognitionAndMerging, positionsInformationAfterRefresh),
                "PositionInfo for all positions not equals");
        }
    }
}