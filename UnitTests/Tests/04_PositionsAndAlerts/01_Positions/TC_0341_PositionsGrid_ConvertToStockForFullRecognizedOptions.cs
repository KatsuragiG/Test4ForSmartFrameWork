using System;
using System.Linq;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Events;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.PositionCard;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.Sorting;
using AutomatedTests.Enums.User;
using AutomatedTests.Enums;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.AddPositionAdvanced;
using AutomatedTests.Steps.Events;
using AutomatedTests.Steps.PositionCard;
using AutomatedTests.Steps.PositionsGridSteps;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;
using System.Collections.Generic;
using System.Globalization;
using AutomatedTests.Models.EventsModels;
using AutomatedTests.Models;

namespace UnitTests.Tests._04_PositionsAndAlerts._01_Positions
{
    [TestClass]
    public class TC_0341_PositionsGrid_ConvertToStockForFullRecognizedOptions : BaseTestUnitTests
    {
        private const int TestNumber = 341;

        private ProductSubscriptionTypes userType;
        private PortfolioModel portfolioModel;
        private readonly List<PositionsDBModel> positionsModelsOption = new List<PositionsDBModel>();
        private int optionSymbolCategoryId;
        private List<int> optionsIds = new List<int>();
        private int optionQuantity;
        private int portfolioId;
        private int countOfClosedPositions;
        private string optionEventDescription;
        private string stockDescription;
        private string expectedClosePrice;

        [TestInitialize]
        public void TestInitialize()
        {
            userType = GetTestDataParsedAsEnumFromStringMapping<ProductSubscriptionTypes>("User");
            portfolioModel = new PortfolioModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency")
            };
            optionQuantity = GetTestDataAsInt(nameof(optionQuantity));
            for (int i = 1; i <= optionQuantity; i++)
            {
                positionsModelsOption.Add(new PositionsDBModel
                {
                    Symbol = GetTestDataAsString($"Symbol{i}"),
                    TradeType = $"{(int)GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>($"TradeType{i}")}",
                    PurchasePriceAdj = GetTestDataAsString($"PurchasePrice{i}"),
                    PurchaseDate = GetTestDataAsString($"PurchaseDate{i}"),
                    Shares = GetTestDataAsString($"Qty{i}")
                });
            }
            positionsModelsOption.Last().StatusType = $"{(int)AutotestPositionStatusTypes.Expired}";

            stockDescription = "Add Position on {0} ({1} shares at ${2} on {3})";
            optionEventDescription = "Close of {0} ({1} contracts at {2} on {3})";
            optionSymbolCategoryId = GetTestDataAsInt(nameof(optionSymbolCategoryId));
            expectedClosePrice = GetTestDataAsString(nameof(expectedClosePrice));

            LogStep(0, "Precondition - Login as registered user");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, (ProductSubscriptions)(int)userType));

            portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            optionsIds = PositionsAlertsSetUp.AddPositionsViaDB(portfolioId, positionsModelsOption);

            LoginSetUp.LogIn(UserModels.First());

            countOfClosedPositions = new PositionsGridSteps().GetAllPositionsIdsByTab(PositionsTabs.ClosedPositions).Count;
            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_341$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PositionsGrid"), TestCategory("PositionsGridPositionActionsPopup"), TestCategory("EventHistoryPage")]
        [Description("The test checks correctness of opened Convert to Stock operation for Full recognized options. https://tr.a1qa.com/index.php?/cases/view/19232756")]
        public override void RunTest()
        {
            Assert.IsTrue(optionsIds.Count > 0, "options are not added for test");

            var mainMenuNavigation = new MainMenuNavigation();
            foreach (var optionId in optionsIds)
            {
                LogStep(1, "Open 'MyPortfolio'");
                mainMenuNavigation.OpenPortfolios(portfolioModel.Type);
                var portfoliosForm = new PortfoliosForm();
                portfoliosForm.ClickOnPortfolioName(portfolioModel.Name);
                var positionTabForm = new PositionsTabForm();

                LogStep(2, "Order positions grid by Ticker field DESC. Remember Ticker and L/S for the first position in the grid.");
                positionTabForm.ClickOnPositionColumnToSort(PositionsGridDataField.Ticker, SortingStatus.Desc);

                LogStep(3, "Select in the drop down menu for the first position 'Convert to Stock'.");
                positionTabForm.SelectPositionContextMenuOption(optionId, PositionContextNavigation.ConvertToStock);

                LogStep(4, "Select in the drop down menu for the first position 'Convert to Stock'.");
                var addPositionAdvancedSteps = new AddPositionAdvancedSteps();
                var addPositionAdvancedModel = addPositionAdvancedSteps.GetCurrentAddPositionAdvancedOpenStockModel();
                var positionsQueries = new PositionsQueries();
                var strikePrice = positionsQueries.SelectStrikePriceForOption(optionId);
                var optionInfo = positionsQueries.SelectPositionData(optionId);
                var expectedShares = Parsing.ConvertToDouble(optionInfo.Shares) * Constants.DefaultContractSize;
                var expectedConversionDate = Parsing.ConvertToShortDateString(DateTime.Now.AsShortDate());

                Checker.CheckEquals(portfolioModel.Name, addPositionAdvancedModel.Portfolio, "Portfolio is not as expected");
                Checker.CheckEquals(Constants.OptionSymbolRegex.Match(optionInfo.Symbol).Value, addPositionAdvancedModel.Ticker,
                    "Symbol is not as expected");
                Checker.CheckEquals(StringUtility.SetFormatFromSample(strikePrice, StringUtility.ReplaceAllCurrencySigns(addPositionAdvancedModel.EntryPrice.Replace(",", string.Empty))),
                    StringUtility.ReplaceAllCurrencySigns(addPositionAdvancedModel.EntryPrice.Replace(",", string.Empty)),
                    $"Entry Price is not as expected for {addPositionAdvancedModel.Ticker}");
                Checker.CheckEquals(Currency.USD.GetDescription(), Constants.AllCurrenciesRegex.Match(addPositionAdvancedModel.EntryPrice).Value,
                    $"Entry Price currency sign is not as expected for {addPositionAdvancedModel.Ticker}");
                Checker.CheckEquals(Parsing.ConvertToShortDateString(expectedConversionDate), addPositionAdvancedModel.EntryDate, "Entry Date is not as expected");
                if (optionInfo.TradeType.HasValue && addPositionAdvancedModel.IsLongTradeType.HasValue)
                {
                    Checker.CheckEquals((int)PositionTradeTypes.Short - (int)optionInfo.TradeType + 1, (int)PositionTradeTypes.Short - ((bool)addPositionAdvancedModel.IsLongTradeType ? 1 : 0),
                        $"Trade Type is not as expected for {addPositionAdvancedModel.Ticker}");
                }
                Checker.CheckEquals(expectedShares.ToString(CultureInfo.InvariantCulture), addPositionAdvancedModel.Shares, "Shares is not as expected on Convert to stock form");

                LogStep(5, "Click Save button.");
                addPositionAdvancedSteps.ClickSaveGetPositionId(portfolioId);

                LogStep(6, 7, "Open 'Position Details' tab.Make sure date match expectation for:-Symbol;- Entry Price;- Trade Type;- Entry Date");
                var positionCard = new PositionCardForm();
                var positionDetailsTabPositionCardForm = new PositionDetailsTabPositionCardForm();
                var symbol = positionCard.GetSymbol();
                Checker.CheckEquals(addPositionAdvancedModel.Ticker, positionCard.GetSymbol(), "Symbol is not as expected on Position Card");
                Checker.CheckEquals(StringUtility.SetFormatFromSample(positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.EntryPrice),
                    StringUtility.ReplaceAllCurrencySigns(addPositionAdvancedModel.EntryPrice.Replace(",", string.Empty))),
                        StringUtility.ReplaceAllCurrencySigns(addPositionAdvancedModel.EntryPrice.Replace(",", string.Empty)),
                    $"Entry Price is not as expected on Position card for {symbol}");
                Checker.CheckEquals(Currency.USD.GetDescription(),
                    Constants.AllCurrenciesRegex.Match(positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.EntryPrice)).Value,
                    $"Entry Price currency sign is not as expected on Position card for {symbol}");
                Checker.CheckEquals(addPositionAdvancedModel.EntryDate, positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.EntryDate),
                    $"Entry Date is not as expected on Position Card on Position card for {symbol}");
                Checker.CheckEquals(addPositionAdvancedModel.IsLongTradeType, positionDetailsTabPositionCardForm.IsTradeTypeLong(),
                    $"Trade Type is not as expected on Position Card on Position card for {symbol}");
                Checker.CheckEquals(expectedShares.ToString(CultureInfo.InvariantCulture).ToFractionalString(), 
                    positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.Shares).Replace("-", string.Empty),
                    $"Shares is not as expected on Position card for {symbol}");

                LogStep(8, "Open 'Alerts' tab.Make sure that Stock position has no alert.");
                var alertsPositionCard = new AlertTabPositionCardSteps().GetAllAddedAlerts();
                Checker.CheckEquals(0, alertsPositionCard.Count,
                    $"Stock position has some alerts\n{alertsPositionCard.Aggregate("", (current, alert) => current + "\n" + alert.ToString())}");

                LogStep(9, "Open My Portfolio tab-> Positions tab -> Closed positions tab. Select portfolio from the preconditions. Select Period Today.");
                mainMenuNavigation.OpenPositionsGrid(PositionsTabs.ClosedPositions);
                new PositionsAlertsStatisticsPanelForm().SelectPortfolio(portfolioModel.Name);
                var closedPositionsTabForm = new ClosedPositionsTabForm();
                closedPositionsTabForm.SelectPeriod(GridFilterPeriods.Last7Days);
                Checker.CheckEquals(++countOfClosedPositions, new PositionsGridSteps().GetAllPositionsIdsByTab(PositionsTabs.ClosedPositions).Count,
                    "Count of positions is not as expected");

                LogStep(10, "Make sure that the option position (step #1) is present on Closed Positions grid.");
                Checker.IsTrue(closedPositionsTabForm.IsPositionPresentInGridById(optionId), "Option position is not present on Closed Positions grid");

                LogStep(11, "For the closed Option (step #1) check:- Exit Price;-Exit Date on columns Exit Price, Exit Date.");
                Checker.CheckEquals(expectedClosePrice,
                    closedPositionsTabForm.GetPositionsGridCellValue(new TableCellMetrics { PositionId = optionId, ColumnHeader = ClosedPositionsGridDataField.ExitPrice.GetStringMapping() }),
                    "Exit Price ids not $0.00");
                Checker.CheckEquals(Parsing.ConvertToShortDateString(expectedConversionDate),
                    closedPositionsTabForm.GetPositionsGridCellValue(new TableCellMetrics { PositionId = optionId, ColumnHeader = ClosedPositionsGridDataField.ExitDate.GetStringMapping() }),
                    "Exit Date is not current");

                LogStep(12, "Open Events History page. Select All Portfolios. Select All Positions.Select All Event Types.Select Previous 12 Previous 12 Months.");
                var eventsForm = new EventsSteps().OpenEventsSelectPortfolioByNameSelectEventTypeGetEventsForm(portfolioModel.Name, EventTypes.All);
                eventsForm.SelectPosition(EventTypes.All.ToString());
                eventsForm.SelectPeriod(GridFilterPeriods.Last7Days);
                var eventsModels = new List<EventsModel>
                {
                    new EventsModel
                    {
                        EventType = EventTypes.Sell.GetStringMapping(),
                        Description = string.Format(optionEventDescription, optionInfo.Symbol, optionInfo.Shares.ToFractionalString(),
                            expectedClosePrice, expectedConversionDate)
                    },
                    new EventsModel
                    {
                        EventType = EventTypes.PurchaseOpen.GetStringMapping(),
                        Description = string.Format(stockDescription, addPositionAdvancedModel.Ticker, expectedShares.ToString(CultureInfo.InvariantCulture).ToFractionalString(),
                            $"{double.Parse(strikePrice):0,0.00}", expectedConversionDate)
                    }
                };
                foreach (var eventModel in eventsModels)
                {
                    Checker.IsTrue(eventsForm.IsEventGridContainsCertainEventDescriptionType(eventModel),
                        $"Expected event '{eventModel.Description}' is not present");
                }
            }        
        }
    }
}