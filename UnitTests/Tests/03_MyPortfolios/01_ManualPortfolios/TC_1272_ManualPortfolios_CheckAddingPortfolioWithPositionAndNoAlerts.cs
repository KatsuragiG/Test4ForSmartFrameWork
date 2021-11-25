using System;
using System.Collections.Generic;
using System.Linq;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Portfolios.CreateManual;
using AutomatedTests.Enums.PositionCard;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms.MyPortfolios;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Models;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._03_MyPortfolios._01_ManualPortfolios
{
    [TestClass]
    public class TC_1272_ManualPortfolios_CheckAddingPortfolioWithPositionAndNoAlerts : BaseTestUnitTests
    {
        private const int TestNumber = 1272;
        private const int NumberRowPosition = 1;
        private const string Today = "today";

        private AddPortfolioManualModel portfolioModel;
        private PositionAtManualCreatingPortfolioModel positionsModel;
        private PositionAtManualCreatingPortfolioModel expectedPosition;
        private PositionAtManualCreatingPortfolioModel expectedPositionOnPositionsPage;

        private string positionEntryCommission;
        private string portfolioCashStatistic;
        private string portfolioDisplayedCommission;
        private string portfolioNotes;
        private string sharesCard;
        private string entryDateCard;
        private string entryPriceCard;
        private string entryPriceGrid;
        private string positionNameOnCard;

        [TestInitialize]
        public void TestInitialize()
        {
            var positionCurrencySign = GetTestDataAsString("positionCurrencySign");
            var portfolioCurrencySign = GetTestDataAsString("portfolioCurrencySign");
            entryDateCard = GetEntryDate("entryDateGridCard");
            entryPriceCard = GetPriceIfNotAcronym(GetTestDataAsString(nameof(entryPriceCard)), positionCurrencySign);
            entryPriceGrid = GetPriceIfNotAcronym(GetTestDataAsString(nameof(entryPriceGrid)), positionCurrencySign); 
            sharesCard = GetTestDataAsString(nameof(sharesCard));
            positionEntryCommission = GetPriceIfNotAcronym(GetTestDataAsString(nameof(positionEntryCommission)), positionCurrencySign);
            portfolioNotes = GetTestDataAsString(nameof(portfolioNotes));
            positionNameOnCard = GetTestDataAsString(nameof(positionNameOnCard));
            portfolioDisplayedCommission = GetPriceIfNotAcronym(GetTestDataAsString(nameof(portfolioDisplayedCommission)), portfolioCurrencySign);

            portfolioModel = new AddPortfolioManualModel
            {
                Name = "CheckAddingPortfolioWithPositionAndNoAlerts", 
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("portfolioType"),
                Notes = GetTestDataAsString("portfolioNotes"),
                EntryCommission = GetPriceAsFractionalIfNotAcronym(GetTestDataAsString("portfolioEntryCommission"), portfolioCurrencySign), 
                ExitCommission = GetPriceAsFractionalIfNotAcronym(GetTestDataAsString("portfolioExitCommission"), portfolioCurrencySign),
                Cash = GetTestDataAsString("portfolioCash"),
                Currency = GetTestDataAsString("portfolioCurrency"),
            };

            portfolioCashStatistic = GetPriceIfNotAcronym(GetTestDataAsString(nameof(portfolioCashStatistic)), portfolioCurrencySign);

            positionsModel = new PositionAtManualCreatingPortfolioModel
            {
                Ticker = GetTestDataAsString("ticker"),
                EntryDate = GetEntryDate("entryDate"),
                EntryPrice = GetTestDataAsString("entryPrice"),
                Quantity = GetTestDataAsString("sharesAddManually"),
                TradeType = GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>("tradeType"),
                PositionAssetType = GetTestDataParsedAsEnumFromStringMapping<PositionAssetTypes>("positionType")
            };

            expectedPosition = new PositionAtManualCreatingPortfolioModel
            {
                Ticker = GetTestDataAsString("tickerShown"),
                EntryDate = positionsModel.EntryDate,
                EntryPrice = $"{positionCurrencySign}{positionsModel.EntryPrice}",
                Quantity = positionsModel.Quantity,
                TradeType = positionsModel.TradeType
            };

            expectedPositionOnPositionsPage = new PositionAtManualCreatingPortfolioModel
            {
                Ticker = expectedPosition.Ticker,
                EntryDate = GetEntryDate("entryDateGridCard"),
                EntryPrice = entryPriceCard,
                Quantity = GetToFractionalIfNotAcronym(GetTestDataAsString("sharesGrid")),
                TradeType = expectedPosition.TradeType
            };

            LogStep(0, "Preconditions: Login as Premium. Open Portfolios page -> Click 'Add Portfolio' -> Click 'Add Manually'");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, new List<ProductSubscriptions>
                {
                    ProductSubscriptions.TradeStopsLifetime, ProductSubscriptions.CryptoStopsLifetime
                }
            ));

            LoginSetUp.LogIn(UserModels.First());
            new AddPortfoliosSteps().GetManualPortfolioCreateFormViaPortfolioGridAddButton();
        }

        private string GetEntryDate(string propertyName)
        {
            var entryDate = GetTestDataAsString(propertyName);
            return entryDate.Equals(Today, StringComparison.OrdinalIgnoreCase) ? DateTime.Now.ToString(Constants.ShortDateFormat) : entryDate;
        }

        private string GetToFractionalIfNotAcronym(string value)
        {
            if (value.Equals(Constants.NotAvailableAcronym, StringComparison.OrdinalIgnoreCase))
            {
                return value;
            }

            var isNegative = value.Contains(Constants.MinusSign);
            var valueAsFractionalString = value.ToFractionalString();

            if (isNegative && !valueAsFractionalString.Contains(Constants.MinusSign))
            {
                valueAsFractionalString = $"{Constants.MinusSign}{valueAsFractionalString}";
            }
            return valueAsFractionalString;
        }

        private string GetPriceAsFractionalIfNotAcronym(string priceValue, string currencySign)
        {
            return priceValue.Equals(Constants.NotAvailableAcronym, StringComparison.OrdinalIgnoreCase) 
                ? priceValue 
                : $"{currencySign}{priceValue.ToFractionalString()}";
        }

        private string GetPriceIfNotAcronym(string priceValue, string currencySign)
        {
            return priceValue.Equals(Constants.NotAvailableAcronym, StringComparison.OrdinalIgnoreCase) 
                ? priceValue 
                : $"{currencySign}{priceValue}";
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1272$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("AddPortfolio")]
        [Description("Test for adding Portfolio with all combinations of Entry Date, Entry Price, Shares, Trade Type: https://tr.a1qa.com/index.php?/cases/view/19234167")]
        public override void RunTest()
        {
            LogStep(1, 7, "Enter and Check Portfolio Data");
            var addPortfoliosSteps = new AddPortfoliosSteps();
            addPortfoliosSteps.FillCheckPortfolioFields(portfolioModel);

            LogStep(8, "Enter Positions fields");
            addPortfoliosSteps.FillCheckFilledPositionFieldsByNumberRowPosition(positionsModel, expectedPosition, NumberRowPosition);

            LogStep(9, "Click Save Portfolio");
            new ManualPortfolioCreationForm().ClickPortfolioManualFlowActionsButton(PortfolioManualFlowActionsButton.SavePortfolio);
            var addAlertsAtCreatingPortfolioForm = new AddAlertsAtCreatingPortfolioForm();
            addAlertsAtCreatingPortfolioForm.AssertIsOpen();

            LogStep(10, "Click I'll add alerts later button");
            addAlertsAtCreatingPortfolioForm.ClickActionButton(AddAlertsAtCreatingPortfolioButtons.AddAlertsLater);
            var positionsAlertsStatisticsPanelForm = new PositionsAlertsStatisticsPanelForm();
            var myPortfoliosMenuForm = new MyPortfoliosMenuForm();
            Checker.IsTrue(myPortfoliosMenuForm.IsMyPortfoliosTabSelected(MyPortfoliosMenuItems.PositionsGrid), 
                "Positions tab in Main Menu is not active");
            Checker.IsTrue(positionsAlertsStatisticsPanelForm.IsTabActive(PositionsTabs.OpenPositions),
                "Open Positions tab is not active");

            LogStep(11, "Check value for the following columns in the grid");
            var positionsTabForm = new PositionsTabForm();
            positionsTabForm.ClickEditSignForCurrentView();
            positionsTabForm.InvertColumnCheckboxByLabel(PositionsGridDataField.Commissions.GetStringMapping());
            positionsTabForm.ClickSaveViewButton();
            var positionsIds = positionsTabForm.GetPositionsIdsByTicker(expectedPositionOnPositionsPage.Ticker);
            Assert.IsTrue(positionsIds.Any(), "Position Id is not found");

            var expectedPositionId = positionsIds.First();
            var positionName = new PositionsQueries().SelectPositionName(expectedPositionId);
            Checker.CheckEquals(expectedPositionOnPositionsPage.Ticker,
                positionsTabForm.GetSymbol(expectedPositionId),
                "Ticker is not as expected in the grid");
            Checker.CheckEquals(positionName, 
                positionsTabForm.GetPositionsGridCellValue(new TableCellMetrics { PositionId = expectedPositionId, ColumnHeader = PositionsGridDataField.Ticker.GetStringMapping() })
                    .Split('\r').Last().Replace("\n", ""),
                "Name is not as expected in the grid");
            Checker.CheckEquals(expectedPositionOnPositionsPage.EntryDate, 
                positionsTabForm.GetPositionsGridCellValue(new TableCellMetrics { PositionId = expectedPositionId, ColumnHeader = PositionsGridDataField.EntryDate.GetStringMapping() }),
                "EntryDate is not as expected in the grid");
            Checker.CheckEquals(entryPriceGrid,
                positionsTabForm.GetPositionsGridCellValue(new TableCellMetrics { PositionId = expectedPositionId, ColumnHeader = PositionsGridDataField.EntryPrice.GetStringMapping() }),
                "EntryPrice is not as expected in the grid");
            Checker.CheckEquals(expectedPositionOnPositionsPage.Quantity,
                GetToFractionalIfNotAcronym(positionsTabForm.GetPositionsGridCellValue(
                    new TableCellMetrics { PositionId = expectedPositionId, ColumnHeader = PositionsGridDataField.Shares.GetStringMapping() }).Replace("-", string.Empty)), 
                "Shares is not as expected in the grid");
            Checker.CheckEquals(expectedPositionOnPositionsPage.TradeType.GetStringMapping(),
                positionsTabForm.GetPositionsGridCellValue(new TableCellMetrics { PositionId = expectedPositionId, ColumnHeader = PositionsGridDataField.TradeType.GetStringMapping() }),
                "LS is not as expected in the grid");
            Checker.CheckEquals(positionEntryCommission,
                positionsTabForm.GetPositionsGridCellValue(new TableCellMetrics { PositionId = expectedPositionId, ColumnHeader = PositionsGridDataField.Commissions.GetStringMapping() }),
                "Position Entry Commission is not as expected in the grid");

            LogStep(12, "Expand the portfolio details - click More Details. Compare the displayed values with values from test data");
            positionsAlertsStatisticsPanelForm.ClickPortfolioSummary();
            Checker.CheckEquals(portfolioCashStatistic,
                positionsAlertsStatisticsPanelForm.GetValueFromBasicSummaryBlock(PortfolioSummaryBasicValueTypes.Cash),
                "'Cash' from basic block is not as expected");
            Checker.CheckEquals(portfolioCashStatistic,
                positionsAlertsStatisticsPanelForm.GetValueFromBasicSummaryBlock(PortfolioSummaryBasicValueTypes.Cash),
                "'Cash' from statistic block is not as expected");
            Checker.CheckEquals(portfolioModel.Name,
                positionsAlertsStatisticsPanelForm.GetValueFromBasicSummaryBlock(PortfolioSummaryBasicValueTypes.Name),
                "'Name' from basic block is not as expected");
            Checker.CheckEquals(portfolioModel.Type.GetStringMapping(),
                positionsAlertsStatisticsPanelForm.GetValueFromBasicSummaryBlock(PortfolioSummaryBasicValueTypes.Type),
                "'Type' from basic block is not as expected");
            Checker.CheckEquals(portfolioNotes,
                positionsAlertsStatisticsPanelForm.GetValueFromBasicSummaryBlock(PortfolioSummaryBasicValueTypes.Notes),
                "'Notes' from basic block is not as expected");
            Checker.CheckEquals(portfolioDisplayedCommission,
                positionsAlertsStatisticsPanelForm.GetValueFromBasicSummaryBlock(PortfolioSummaryBasicValueTypes.EntryCommission),
                "'Entry Commission' from basic block is not as expected");
            Checker.CheckEquals(portfolioDisplayedCommission,
                positionsAlertsStatisticsPanelForm.GetValueFromBasicSummaryBlock(PortfolioSummaryBasicValueTypes.ExitCommission),
                "'Exit Commission' from basic block is not as expected");
            Checker.CheckEquals(portfolioModel.Currency, positionsAlertsStatisticsPanelForm.GetSelectedCurrencyFromBasicBlock(),
                "'Currency' from basic block is not as expected");
            positionsAlertsStatisticsPanelForm.ClickPortfolioSummary();

            LogStep(13, "Go to Alerts tab");
            myPortfoliosMenuForm.ClickMyPortfoliosMenuItem(MyPortfoliosMenuItems.AlertsGrid);
            var alertsTabForm = new AlertsTabForm();
            Checker.IsTrue(alertsTabForm.IsAlertsEmptyIconPresent(), "Empty icon is not present");
            Checker.IsTrue(alertsTabForm.IsNoAlertTextPresent(), "No Alert Text is not present");
            Checker.IsTrue(alertsTabForm.IsAddAlertsButtonPresent(), "Add Alerts button is not present");

            LogStep(14, "Go to Positions tab");
            new MainMenuNavigation().OpenPositionsGrid();
            positionsTabForm.AssertIsOpen();

            LogStep(15, "Click the link in the Symbol column");
            positionsTabForm.ClickOnPositionLink(expectedPositionId);
            var positionCardForm = new PositionCardForm();

            var positionDetailsTabPositionCardForm = new PositionDetailsTabPositionCardForm();
            var currentSharesField = positionDetailsTabPositionCardForm.GetSharesTypeFieldWithDetectingType();

            Checker.CheckEquals(expectedPositionOnPositionsPage.Ticker, positionCardForm.GetSymbol(),
                "Ticker is not as expected on Position Card page");
            Checker.CheckEquals(positionNameOnCard.ToUpperInvariant(), positionCardForm.GetName(),
                "Name is not as expected on Position Card page");
            Checker.CheckEquals(entryDateCard, positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.EntryDate), 
                "Entry Date is not as expected on Position Card page");
            Checker.CheckEquals(entryPriceCard,
                positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.EntryPrice).RemoveNumberSeparator(), 
                "Entry Price is not as expected on Position Card page");
            Checker.CheckEquals(sharesCard,
                positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(currentSharesField).RemoveNumberSeparator().Replace("-", string.Empty), 
                "Shares is not as expected on Position Card page");
            Checker.CheckEquals(positionEntryCommission.Replace(",", string.Empty),
                positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.EntryCommission).RemoveNumberSeparator(), 
                "Entry Commission is not as expected on Position Card page");
            Checker.CheckEquals(expectedPosition.TradeType.GetStringMapping(),                
                positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.TradeType),
                "Trade Type is not as expected on Position Card page");
        }
    }
}