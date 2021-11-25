using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.User;
using AutomatedTests.Enums;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms;
using AutomatedTests.Models.FiltersModels;
using AutomatedTests.Models;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.PositionsGridSteps;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System;
using TradeStops.Common.Enums;
using TradeStops.Common.Extensions;
using TradeStops.Common.Utils;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._04_PositionsAndAlerts._01_Positions
{
    [TestClass]
    public class TC_0228_ClosedPositionsGrid_CorrectPositionCardIsOpen : BaseTestUnitTests
    {
        private const int TestNumber = 228;

        private const string StartDateForCustomDateRangeFilter = "01/01/2014";
        private string optionName;
        private readonly PositionsQueries positionsQueries = new PositionsQueries();
        private List<int> closedPositionIds;
        private readonly PositionIssuesFilterModel positionStatusFilterModel = new PositionIssuesFilterModel();

        [TestInitialize]
        public void TestInitialize()
        {
            var userType = GetTestDataParsedAsEnumFromStringMapping<ProductSubscriptionTypes>("User");
            var portfolioModel = new PortfolioModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = PortfolioType.WatchOnly,
                Currency = GetTestDataAsString("Currency")
            };
            var longTradeType = GetTestDataAsString("TradeTypeLong");
            var shortTradeType = GetTestDataAsString("TradeTypeShort");
            optionName = GetTestDataAsString(nameof(optionName));
            var closedStatusType = $"{(int)AutotestPositionStatusTypes.Close}";
            var positionsModels = new List<PositionsDBModel>
            {
                new PositionsDBModel
                {
                    Symbol = GetTestDataAsString("SymbolStock2"),
                    TradeType = longTradeType,
                    StatusType = closedStatusType
                },
                new PositionsDBModel
                {
                    Symbol = GetTestDataAsString("SymbolStock1"),
                    TradeType = shortTradeType,
                    StatusType = closedStatusType
                },
                new PositionsDBModel
                {
                    Symbol = GetTestDataAsString("SymbolOption1"),
                    TradeType = longTradeType,
                    StatusType = closedStatusType
                },
                new PositionsDBModel
                {
                    Symbol = GetTestDataAsString("SymbolOption2"),
                    TradeType = shortTradeType,
                    StatusType = closedStatusType
                },
                new PositionsDBModel
                {
                    Symbol = GetTestDataAsString("SymbolCrypto"),
                    TradeType = longTradeType,
                    StatusType = closedStatusType
                },
                new PositionsDBModel
                {
                    Symbol = GetTestDataAsString("SymbolCrypto2"),
                    TradeType = shortTradeType,
                    StatusType = closedStatusType
                },
                new PositionsDBModel
                {
                    Symbol = GetTestDataAsString("SymbolIndex"),
                    TradeType = longTradeType,
                    StatusType = closedStatusType,
                },
                new PositionsDBModel
                {
                    Symbol = GetTestDataAsString("SymbolForex"),
                    TradeType = shortTradeType,
                    StatusType = closedStatusType
                }
            };
            foreach (var item in EnumUtils.GetValues<PositionsIssuesTypes>())
            {
                positionStatusFilterModel.IssueNameToState.Add(item, true);
            }

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, (ProductSubscriptions)(int)userType));

            var manualPortfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            foreach (var positionModel in positionsModels)
            {
                PositionsAlertsSetUp.AddPositionViaDB(manualPortfolioId, positionModel);
            }

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);
            PortfoliosSetUp.ImportDagSiteInvestment06(true);

            var mainMenuNavigation = new MainMenuNavigation();
            mainMenuNavigation.OpenPositionsGrid();
            var positionsAlertsStatisticsPanelForm = new PositionsAlertsStatisticsPanelForm();
            positionsAlertsStatisticsPanelForm.SelectPortfolio(AllPortfoliosKinds.AllInvestment.GetStringMapping());
            var positionsTabForm = new PositionsTabForm();
            var importedPositionsIds = positionsTabForm.GetPositionsIds();
            foreach (var importedPositionId in importedPositionsIds)
            {
                positionsQueries.SetPossibilityToCloseSynchPositionByPositionId(importedPositionId);
            }
            Browser.Refresh();

            positionsAlertsStatisticsPanelForm.SelectPortfolio(AllPortfoliosKinds.All.GetStringMapping());
            new PositionsGridSteps().FilterPositionsIssues(positionStatusFilterModel);
            var positionsWithIssuesIds = positionsTabForm.GetPositionsIds();
            var stocksWithIssuesIds = positionsWithIssuesIds.Where(id =>
                    positionsQueries.SelectAssetTypeNameByPositionId(id).In(PositionAssetTypes.Stock.GetStringMapping(), PositionAssetTypes.Crypto.GetStringMapping()))
                .ToList();
            var optionsWithIssuesIds = positionsWithIssuesIds.Where(id =>
                positionsQueries.SelectAssetTypeNameByPositionId(id)
                    .EqualsIgnoreCase(PositionAssetTypes.Option.GetStringMapping())).ToList();

            PositionsAlertsSetUp.ClosePosition(stocksWithIssuesIds[0]);
            PositionsAlertsSetUp.ClosePosition(optionsWithIssuesIds[0]);
            mainMenuNavigation.OpenPositionsGrid(PositionsTabs.ClosedPositions);
            var closedPositionTabForm = new ClosedPositionsTabForm();
            closedPositionTabForm.SelectPeriod(GridFilterPeriods.Last12Months);
            closedPositionIds = closedPositionTabForm.GetPositionsIds();

            Assert.IsTrue(closedPositionIds.Count > 0, "number of closed positions is 0");
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_228$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("ClosedPositionsGrid"), TestCategory("ClosedPositionCard")]
        [Description("https://tr.a1qa.com/index.php?/cases/view/19901593 The test checks correctness of opened Card Position from position grid")]
        public override void RunTest()
        {
            foreach (var positionId in closedPositionIds)
            {
                LogStep(1, "Select All portfolio in dropdowns");
                new PositionsAlertsStatisticsPanelForm().SelectPortfolio(AllPortfoliosKinds.All.GetStringMapping());

                LogStep(2, "Select Previous 6 Months from dropdown Period:");
                var closedPositionTabForm = new ClosedPositionsTabForm();
                closedPositionTabForm.SelectCustomPeriodRangeWithStartEndDates(StartDateForCustomDateRangeFilter, DateTime.Now.ToShortDateString());

                LogStep(3, "Click on Symbol column for *Stock* position (precondition #2-1).");
                var positionInfo = closedPositionTabForm.GetPositionsGridCellValue(
                    new TableCellMetrics { PositionId = positionId, ColumnHeader = ClosedPositionsGridDataField.Ticker.GetStringMapping() })
                    .Split('\r');
                closedPositionTabForm.ClickOnPositionLink(positionId);

                LogStep(4, $"Make sure that PositionID, Symbol and Position Name is corresponded selected stock position. {positionId}");
                var positionCard = new PositionCardForm();
                var id = positionCard.GetPositionIdFromUrl();
                var symbol = positionCard.GetSymbol();
                var name = positionCard.GetName();
                Checker.CheckEquals(positionId, id, "Id is not equals");
                var expectedName = positionsQueries.SelectAssetTypeNameByPositionId(id).EqualsIgnoreCase(PositionAssetTypes.Option.GetStringMapping())
                    ? optionName
                    : positionInfo[1].Replace("\n", "").ToUpperInvariant();
                Checker.CheckEquals(expectedName, name, "Name is not equals");
                Checker.CheckEquals(positionInfo[0], symbol, "Symbol is not equals");

                new MainMenuNavigation().OpenPositionsGrid(PositionsTabs.ClosedPositions);
            }
        }
    }
}