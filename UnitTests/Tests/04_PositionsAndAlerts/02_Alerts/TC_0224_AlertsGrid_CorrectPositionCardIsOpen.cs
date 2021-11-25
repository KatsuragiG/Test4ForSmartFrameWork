using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Alerts;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Settings;
using AutomatedTests.Enums;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.PositionsGridSteps;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Models;
using AutomatedTests.Enums.Positions;
using TradeStops.Common.Extensions;

namespace UnitTests.Tests._04_PositionsAndAlerts._02_Alerts
{
    [TestClass]
    public class TC_0224_AlertsGrid_CorrectPositionCardIsOpen : BaseTestUnitTests
    {
        private const int TestNumber = 224;

        private readonly PositionsQueries positionsQueries = new PositionsQueries();
        private List<string> optionNames;
        private List<int> alertsIds;

        [TestInitialize]
        public void TestInitialize()
        {
            var userProductSubscriptions = GetUserProductSubscriptions("User");

            var longTradeType = GetTestDataAsString("TradeTypeLong");
            var shortTradeType = GetTestDataAsString("TradeTypeShort");
            optionNames = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(optionNames));
            var portfolioModel = new PortfolioModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency")
            };
            var positionModelStockLong = new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolStock2"),
                TradeType = longTradeType
            };
            var positionModelStockShort = new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolStock1"),
                TradeType = shortTradeType
            };
            var positionModelStockLongForDelisting = new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolStock4"),
                TradeType = longTradeType
            };
            var positionModelStockShortForDelisting = new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolStock3"),
                TradeType = shortTradeType
            };
            var positionModelOptionLong = new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolOption1"),
                TradeType = longTradeType
            };
            var positionModelOptionShort = new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolOption2"),
                TradeType = shortTradeType
            };
            var positionModelOptionLongForExpiring = new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolOption4"),
                TradeType = longTradeType
            };
            var positionModelOptionShortForExpiring = new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolOption3"),
                TradeType = shortTradeType
            };
            var positionsIds = new List<int>();

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));

            var manualPortfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            PositionsAlertsSetUp.AddPositionViaDB(manualPortfolioId, positionModelStockLong);
            PositionsAlertsSetUp.AddPositionViaDB(manualPortfolioId, positionModelStockShort);
            positionsIds.Add(PositionsAlertsSetUp.AddPositionViaDB(manualPortfolioId, positionModelStockLongForDelisting));
            positionsIds.Add(PositionsAlertsSetUp.AddPositionViaDB(manualPortfolioId, positionModelStockShortForDelisting));
            positionsQueries.SetStatusTypeAsDelisted(positionsIds[0]);
            positionsQueries.SetStatusTypeAsDelisted(positionsIds[1]);
            PositionsAlertsSetUp.AddPositionViaDB(manualPortfolioId, positionModelOptionLong);
            PositionsAlertsSetUp.AddPositionViaDB(manualPortfolioId, positionModelOptionShort);
            positionsIds.Add(PositionsAlertsSetUp.AddPositionViaDB(manualPortfolioId, positionModelOptionLongForExpiring));
            positionsIds.Add(PositionsAlertsSetUp.AddPositionViaDB(manualPortfolioId, positionModelOptionShortForExpiring));
            positionsQueries.SetStatusTypeAsExpired(positionsIds[2]);
            positionsQueries.SetStatusTypeAsExpired(positionsIds[3]);

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
            new PositionsGridSteps().OpenPositionGridSelectAllPositionApplyTemplateCloseSuccessPopup(DefaultTemplateTypes.TrailingStop15.GetStringMapping());

            PortfoliosSetUp.ImportDagSiteInvestment(true);
            new MainMenuNavigation().OpenAlertsGrid();
            new PositionsAlertsStatisticsPanelForm().SelectPortfolio(AllPortfoliosKinds.All.GetStringMapping());
            alertsIds = new AlertsTabForm().GetActiveAlertsIds();
            Assert.IsTrue(alertsIds.Count > 0, "Number of alerts is 0");
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_224$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("AlertsGrid"), TestCategory("PositionCard")]
        [Description("The test checks correctness of opened Card Position from Alerts grid https://tr.a1qa.com/index.php?/cases/view/19232802")]
        public override void RunTest()
        {
            LogStep(1, "Select Manual portfolio in dropdowns");
            var positionsAlertsStatisticsPanelForm = new PositionsAlertsStatisticsPanelForm();
            var alertsTabForm = new AlertsTabForm();
            var mainMenuNavigation = new MainMenuNavigation();
            positionsAlertsStatisticsPanelForm.SelectPortfolio(AllPortfoliosKinds.All.GetStringMapping());

            foreach (int alertId in alertsIds)
            {
                LogStep(2, 3, "Select All from status dropdown. Click on Symbol column for *Stock* position (precondition #2-1).");
                var alertOrder = alertsTabForm.GetAlertOrderInGridByAlertId(alertId);
                if (alertOrder != Constants.ItemNotFoundInCollection)
                {
                    var alertTickerName = alertsTabForm.GetPositionsGridCellValue(new TableCellMetrics { PositionOrder = alertOrder, ColumnHeader = AlertsGridColumnsDataField.Ticker.GetStringMapping() })
                        .Split('\r');
                    var positionId = alertsTabForm.GetPositionIdFromGridByLineNumber(alertOrder);
                    alertsTabForm.ClickOnAlertViaOrder(alertOrder);

                    LogStep(4, $"Make sure that PositionID, Symbol and Position Name is corresponded selected stock position. {alertId}");
                    var positionCard = new PositionCardForm();
                    var id = positionCard.GetPositionIdFromUrl();
                    var name = positionCard.GetName();
                    var symbol = positionCard.GetSymbol();

                    Checker.CheckEquals(positionId, id, "Id is not equals");
                    var expectedName = positionsQueries.SelectAssetTypeNameByPositionId(positionId).EqualsIgnoreCase(PositionAssetTypes.Option.GetStringMapping())
                        ? optionNames.FirstOrDefault(t => t.Contains(symbol.Split('2')[0]))
                        : alertTickerName[1].Replace("\n", "").ToUpperInvariant();
                    Checker.CheckEquals(expectedName, name, "Name is not equals");
                    Checker.CheckEquals(alertTickerName[0], symbol, "Symbol is not equals");

                    mainMenuNavigation.OpenAlertsGrid();
                    positionsAlertsStatisticsPanelForm.SelectPortfolio(AllPortfoliosKinds.All.GetStringMapping());
                }
                else
                {
                    Checker.Fail($"Alert with id {alertId} was not found in grid");
                }
            }
        }
    }
}