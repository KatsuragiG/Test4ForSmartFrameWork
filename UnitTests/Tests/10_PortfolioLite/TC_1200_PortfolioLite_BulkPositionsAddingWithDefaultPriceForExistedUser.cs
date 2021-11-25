using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.PositionData;
using AutomatedTests.Database.Positions;
using AutomatedTests.Database.Users;
using AutomatedTests.Enums.PortfolioLite;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Forms.Dashboard;
using AutomatedTests.Forms.PortfolioLite;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms;
using AutomatedTests.Models.PositionsModels;
using AutomatedTests.Models.UserModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.BrowserSteps;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;
using System.Globalization;

namespace UnitTests.Tests._10_PortfolioLite
{
    [TestClass]
    public class TC_1200_PortfolioLite_BulkPositionsAddingWithDefaultPriceForExistedUser : BaseTestUnitTests
    {
        private const int TestNumber = 1200;
        private const string ViewNameForAddedView = "view";

        private int positionsQuantity;
        private readonly List<PortfolioLitePositionModel> positionsModels = new List<PortfolioLitePositionModel>();
        private readonly List<PortfolioLitePositionModel> expectedPositionsModels = new List<PortfolioLitePositionModel>();
        private readonly List<PositionsGridDataField> columnsToCollectDataOnGrid = new List<PositionsGridDataField>();
        private UserModel userModel;

        [TestInitialize]
        public void TestInitialize()
        {
            var userProductSubscriptions = GetUserProductSubscriptions("userSubscription");

            positionsQuantity = GetTestDataAsInt(nameof(positionsQuantity));

            var columnsInPositionsGrid = new List<PositionsGridDataField>
            {
                PositionsGridDataField.EntryDate,
                PositionsGridDataField.TradeType,
                PositionsGridDataField.Shares,
                PositionsGridDataField.EntryPrice,
                PositionsGridDataField.Commissions
            };

            columnsToCollectDataOnGrid.AddRange(columnsInPositionsGrid);
            columnsToCollectDataOnGrid.AddRange(new List<PositionsGridDataField>
            {
                PositionsGridDataField.Ticker, PositionsGridDataField.Name
            });

            var positionDataQueries = new PositionDataQueries();
            var positionsQueries = new PositionsQueries();
            for (int i = 1; i <= positionsQuantity; i++)
            {
                var entryDate = GetTestDataAsString($"entryDate{i}");
                var quantity = GetTestDataAsString($"quantities{i}");
                positionsModels.Add(new PortfolioLitePositionModel
                {
                    Ticker = GetTestDataAsString($"symbolsToAdd{i}"),
                    BuyDate = string.IsNullOrEmpty(entryDate) ? null : entryDate,
                    Qty = string.IsNullOrEmpty(quantity) ? null : quantity,
                    IsLongType = GetTestDataAsBool($"IsLongType{i}")
                });
                var currentPositionModel = positionsModels.Last();
                currentPositionModel.Currency = (Currency)positionsQueries.SelectSymbolCurrencyBySymbol(positionsModels.Last().Ticker);

                var expectedSharesSign = currentPositionModel.IsLongType.HasValue && (bool)currentPositionModel.IsLongType || currentPositionModel.Qty == Constants.DefaultStringZeroIntValue
                    ? string.Empty
                    : Constants.MinusSign;
                expectedPositionsModels.Add(new PortfolioLitePositionModel
                {
                    Ticker = currentPositionModel.Ticker,
                    BuyDate = string.IsNullOrEmpty(entryDate) ? Constants.NotAvailableAcronym : entryDate,
                    BuyPrice = string.IsNullOrEmpty(entryDate)
                        ? Constants.NotAvailableAcronym
                        : positionDataQueries.SelectHdAdjPriceForSymbolIdDate(currentPositionModel.Ticker, entryDate)
                            .GetTradeSplitOnlyAdjClose()
                            .ToString(CultureInfo.InvariantCulture),
                    Qty = string.IsNullOrEmpty(quantity) ? Constants.DefaultStringZeroDecimalValue : $"{expectedSharesSign}{quantity.ToFractionalString()}",
                    IsLongType = currentPositionModel.IsLongType,
                    Currency = currentPositionModel.Currency,
                    Name = positionsQueries.SelectSymbolIdNameUsingSymbol(currentPositionModel.Ticker).SymbolName
                });
            }

            LogStep(0, "Preconditions. Create user with subscription to PortfolioLite");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));
            userModel = UserModels.First();
            PortfoliosSetUp.AddInvestmentAudPortfoliosWithOpenPosition(userModel.Email);

            LoginSetUp.LogIn(userModel);
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
            new MainMenuNavigation().OpenPositionsGrid();
            new PositionsTabForm().AddANewViewWithCheckboxesMarked(ViewNameForAddedView, columnsInPositionsGrid.Select(t => t.GetStringMapping()).ToList());
            TearDowns.LogOut();

            var fakeSnaid = StringUtility.RandomString(Constants.SnaidPattern);
            new UsersQueries().UpdateUserSnaid(fakeSnaid, userModel.TradeSmithUserId);
            new PortfolioLiteNavigation().OpenPortfolioLiteWithUserGuid(UserModels.First());
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1200$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("PortfolioLite")]
        [Description("Test checks that existed user in TS can add positions with default price from the Portfolio Lite and open it in TSP https://tr.a1qa.com/index.php?/cases/view/21116214")]
        public override void RunTest()
        {
            LogStep(1, "Click Add a Position");
            var portfolioLiteMainForm = new PortfolioLiteMainForm();
            Checker.IsTrue(portfolioLiteMainForm.IsInfoAboutEmptyPortfolioPresent(), "Info about Empty Portfolio is NOT shown");
            portfolioLiteMainForm.ClickAddAPosition();
            Checker.IsTrue(portfolioLiteMainForm.IsAddPositionBlockPresent(), "Add position Block is NOT shown before adding a position");

            LogStep(2, "Select tickers from autocomplete for defined rows");

            for (int i = 1; i <= positionsQuantity; i++)
            {
                portfolioLiteMainForm.SetSymbol(positionsModels[i - 1].Ticker, i);
            }
            for (int i = 1; i <= positionsQuantity; i++)
            {
                Checker.CheckEquals(positionsModels[i - 1].Ticker, portfolioLiteMainForm.GetAddedSymbol(i),
                    $"Ticker in row {i} is not as expected");
            }

            LogStep(3, "Type Entry Date (leave empty if cell is empty)");
            for (int i = 1; i <= positionsQuantity; i++)
            {
                if (!string.IsNullOrEmpty(positionsModels[i - 1].BuyDate))
                {
                    portfolioLiteMainForm.SetEntryDate(positionsModels[i - 1].BuyDate, i);
                }
            }
            for (int i = 1; i <= positionsQuantity; i++)
            {
                var expectedEntryDate = string.IsNullOrEmpty(positionsModels[i - 1].BuyDate) ? string.Empty : positionsModels[i - 1].BuyDate;
                Checker.CheckEquals(expectedEntryDate, portfolioLiteMainForm.GetPortfolioLiteFieldValue(PortfolioLiteAddPositionFields.BuyDate, i),
                    $"Entry Date in row {i} is not as expected");
            }

            LogStep(4, "Check that Entry Price has correct prefilled value");
            for (int i = 1; i <= positionsQuantity; i++)
            {
                var actualPrice = portfolioLiteMainForm.GetPortfolioLiteFieldValue(PortfolioLiteAddPositionFields.BuyPrice, i);
                var expectedPrice = string.IsNullOrEmpty(positionsModels[i - 1].BuyDate)
                    ? string.Empty
                    : $"{positionsModels[i - 1].Currency.GetDescription()}{StringUtility.SetFormatFromSample(expectedPositionsModels[i - 1].BuyPrice, actualPrice)}";
                Checker.CheckEquals(expectedPrice, actualPrice,
                    $"Entry Price value in row {i} is prefilled by wrong value in comparison with DB value");
            }

            LogStep(5, "Select position type");
            for (int i = 1; i <= positionsQuantity; i++)
            {
                portfolioLiteMainForm.SelectTradeType(positionsModels[i - 1].IsLongType, i);
            }
            for (int i = 1; i <= positionsQuantity; i++)
            {
                var expectedTradeType = positionsModels[i - 1].IsLongType.HasValue && (bool)positionsModels[i - 1].IsLongType 
                    ? PositionTradeTypes.Long 
                    : PositionTradeTypes.Short;
                Checker.IsTrue(portfolioLiteMainForm.IsBtnTradeTypeActive(expectedTradeType, i), $"TradeType in row {i} is not as expected");
            }

            LogStep(6, "Type Qty (leave empty if cell is empty)");
            for (int i = 1; i <= positionsQuantity; i++)
            {
                if (!string.IsNullOrEmpty(positionsModels[i - 1].Qty))
                {
                    portfolioLiteMainForm.SetTextInPositionsDataFields(PortfolioLiteAddPositionFields.Qty, i, positionsModels[i - 1].Qty);
                }
            }
            for (int i = 1; i <= positionsQuantity; i++)
            {
                var expectedShares = string.IsNullOrEmpty(positionsModels[i - 1].Qty) ? string.Empty : positionsModels[i - 1].Qty;
                Checker.CheckEquals(expectedShares, portfolioLiteMainForm.GetPortfolioLiteFieldValue(PortfolioLiteAddPositionFields.Qty, i),
                    $"Quantity in row {i} is not as expected");
            }

            LogStep(7, "Click Save");
            portfolioLiteMainForm.ClickSave();
            var actualPositionsQuantity = portfolioLiteMainForm.GetPositionQuantityInGrid();
            Checker.CheckEquals(positionsQuantity, actualPositionsQuantity, "Grid does not contain expected added positions");

            LogStep(8, "Check in the grid that created position has expected values");
            for (int i = 1; i <= actualPositionsQuantity; i++)
            {
                var actualDataForPositionPortfolioLite = portfolioLiteMainForm.GetPositionGridDataByOrder(i);
                var symbolAndName = actualDataForPositionPortfolioLite.Ticker.Split('\r');
                var mappedModel = expectedPositionsModels.FirstOrDefault(u => u.Ticker == symbolAndName[0]);
                if (mappedModel == null)
                {
                    Checker.Fail($"Position {actualDataForPositionPortfolioLite.Ticker} not found in grid");
                }
                else
                {
                    var actualPositionName = actualDataForPositionPortfolioLite.Ticker.Split('\r')[1].Replace("\n", string.Empty);
                    Checker.CheckEquals(mappedModel.Name, actualPositionName, $"Ticker name for {symbolAndName[0]} in the grid is not matched expectation");
                    Checker.CheckEquals(mappedModel.BuyDate, actualDataForPositionPortfolioLite.EntryDate,
                        $"Entry Date for {symbolAndName[0]} in the grid is not matched expectation");
                    var expectedPrice = mappedModel.BuyDate == Constants.NotAvailableAcronym
                        ? Constants.NotAvailableAcronym
                        : $"{mappedModel.Currency.GetDescription()}{StringUtility.SetFormatFromSample(mappedModel.BuyPrice, actualDataForPositionPortfolioLite.EntryPrice)}";
                    Checker.CheckEquals(expectedPrice, actualDataForPositionPortfolioLite.EntryPrice,
                        $"Entry Price for {symbolAndName[0]} in the grid is not matched expectation");
                    Checker.CheckEquals(mappedModel.Qty, actualDataForPositionPortfolioLite.Shares.Replace(",", ""),
                        $"Shares for {symbolAndName[0]} in the grid is not matched expectation");
                }
            }

            LogStep(9, "Remember values for Value, Cost Basis, Total Gain, Daily Gain");
            var portfolioLiteValue = portfolioLiteMainForm.GetStatisticValue(PortfolioLiteStatisticTypes.Value, StatisticAttributeTypes.Value);
            var portfolioLiteCostBasis = portfolioLiteMainForm.GetStatisticValue(PortfolioLiteStatisticTypes.CostBasis, StatisticAttributeTypes.Value);
            var portfolioLiteTotalGainMoney = portfolioLiteMainForm.GetStatisticValue(PortfolioLiteStatisticTypes.TotalGain, StatisticAttributeTypes.Value);
            var portfolioLiteTotalGainPercent = portfolioLiteMainForm.GetStatisticValue(PortfolioLiteStatisticTypes.TotalGain, StatisticAttributeTypes.Percent);
            var portfolioLiteDailyGainMoney = portfolioLiteMainForm.GetStatisticValue(PortfolioLiteStatisticTypes.DailyGain, StatisticAttributeTypes.Value);
            var portfolioLiteDailyGainPercent = portfolioLiteMainForm.GetStatisticValue(PortfolioLiteStatisticTypes.DailyGain, StatisticAttributeTypes.Percent);

            LogStep(10, "Logout and login into Platform");
            new BrowserSteps().ClearAllDomainCookiesClearStorages();
            new UsersQueries().ResetUserSnaid(userModel.TradeSmithUserId);
            var mainMenuNavigation = new MainMenuNavigation();
            mainMenuNavigation.OpenLogin();
            LoginSetUp.LogIn(userModel);
            new DashboardForm().AssertIsOpen();
            mainMenuNavigation.OpenPositionsGrid();

            LogStep(11, "Open the new portfolio in Positions grid");
            var portfolioId = new PortfoliosQueries().SelectPortfolioIdForLastPortfolioByUserEmail(userModel.Email);
            var positionsAlertsStatisticsPanelForm = new PositionsAlertsStatisticsPanelForm();
            positionsAlertsStatisticsPanelForm.SelectPortfolioById(portfolioId);
            var positionsTabForm = new PositionsTabForm();
            positionsTabForm.SelectView(ViewNameForAddedView);
            var gridPositionsQuantity = positionsTabForm.GetNumberOfRowsInGrid();
            Checker.CheckEquals(positionsQuantity, gridPositionsQuantity, "Positions grid does not contain expected added positions");

            LogStep(12, "Check in the grid that created position has expected values");
            for (int i = 1; i <= gridPositionsQuantity; i++)
            {
                var actualDataForPositionGrid = positionsTabForm.GetPositionDataByRowOrder(columnsToCollectDataOnGrid, i);
                var symbolAndName = actualDataForPositionGrid.Ticker.Split('\r');
                var mappedModel = expectedPositionsModels.FirstOrDefault(u => u.Ticker == symbolAndName[0]);
                if (mappedModel == null)
                {
                    Checker.Fail($"Position {actualDataForPositionGrid} not found in Positions grid");
                }
                else
                {
                    Checker.CheckEquals(mappedModel.Name, actualDataForPositionGrid.Name, $"Ticker name for {symbolAndName[0]} in the grid is not matched expectation");
                    Checker.CheckEquals(mappedModel.BuyDate, actualDataForPositionGrid.EntryDate,
                        $"Entry Date for {symbolAndName[0]} in the Positions grid is not matched expectation");
                    var expectedPrice = mappedModel.BuyDate == Constants.NotAvailableAcronym
                        ? Constants.NotAvailableAcronym
                        : $"{mappedModel.Currency.GetDescription()}{StringUtility.SetFormatFromSample(mappedModel.BuyPrice, actualDataForPositionGrid.EntryPrice)}";
                    Checker.CheckEquals(expectedPrice, actualDataForPositionGrid.EntryPrice,
                        $"Entry Price for {symbolAndName[0]} in the Positions grid is not matched expectation");
                    Checker.CheckEquals(mappedModel.Qty, actualDataForPositionGrid.Shares.Replace(",", ""),
                        $"Shares for {symbolAndName[0]} in the Positions grid is not matched expectation");
                    Checker.CheckEquals($"{mappedModel.Currency.GetDescription()}{Constants.DefaultStringZeroDecimalValue}", actualDataForPositionGrid.Commissions,
                      $"Commissions for {symbolAndName[0]} in the Positions grid is not matched expectation");
                    var expectedTradeType = mappedModel.IsLongType.HasValue && (bool)mappedModel.IsLongType 
                        ? PositionTradeTypes.Long 
                        : PositionTradeTypes.Short;
                    Checker.CheckEquals(expectedTradeType.ToString(), actualDataForPositionGrid.TradeType,
                        $"TradeType for {symbolAndName[0]} in the Positions grid is not matched expectation");
                }
            }

            LogStep(13, "Check that statistic values are equal to step 9");
            var positionsGridValue = positionsAlertsStatisticsPanelForm.GetValue();
            var positionsGridTotalGainMoney = positionsAlertsStatisticsPanelForm.GetTotalGain();
            var positionsGridTotalGainPercent = positionsAlertsStatisticsPanelForm.GetTotalGainPercent();
            var positionsGridDailyGainMoney = positionsAlertsStatisticsPanelForm.GetDailyGain();
            var positionsGridDailyGainPercent = positionsAlertsStatisticsPanelForm.GetDailyGainPercent();
            positionsAlertsStatisticsPanelForm.ClickPortfolioSummary();
            var positionsGridCostBasis = positionsAlertsStatisticsPanelForm.GetValueFromStatisticsSummaryBlock(PortfolioSummaryStatisticValueTypes.CostBasis);
            positionsAlertsStatisticsPanelForm.ClickPortfolioSummary();

            Checker.CheckEquals(portfolioLiteValue, positionsGridValue, "Value is not matched expectation");
            Checker.CheckEquals(portfolioLiteCostBasis, positionsGridCostBasis, "Cost Basis is not matched expectation");
            Checker.CheckEquals(portfolioLiteTotalGainMoney, positionsGridTotalGainMoney, "Total Gain Money is not matched expectation");
            Checker.CheckEquals(portfolioLiteTotalGainPercent, positionsGridTotalGainPercent, "Total Gain Percent is not matched expectation");
            Checker.CheckEquals(portfolioLiteDailyGainMoney, positionsGridDailyGainMoney, "Daily Gain Money is not matched expectation");
            Checker.CheckEquals(portfolioLiteDailyGainPercent, positionsGridDailyGainPercent, "Daily Gain Percent is not matched expectation");
        }
    }
}