using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Models;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Enums;
using TradeStops.Common.Extensions;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._04_PositionsAndAlerts._01_Positions
{
    [TestClass]
    public class TC_0112_PositionsGrid_CorrectCardPositionOpeningViaClickingOnSymbolColumn : BaseTestUnitTests
    {
        private const int TestNumber = 112;

        private AddPortfolioManualModel portfolioModel;
        private readonly PositionsQueries positionsQueries = new PositionsQueries();
        private List<string> optionNames;
        private IList<int> positionsIds;

        [TestInitialize]
        public void TestInitialize()
        {
            portfolioModel = new AddPortfolioManualModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency")
            };
            optionNames = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(optionNames));

            var positionsQuantity = GetTestDataAsInt("positionsQuantity");
            var positionsModel = new List<PositionAtManualCreatingPortfolioModel>();
            for (int i = 1; i <= positionsQuantity; i++)
            {
                positionsModel.Add(new PositionAtManualCreatingPortfolioModel
                {
                    Ticker = GetTestDataAsString($"Symbol{i}"),
                    PositionAssetType = GetTestDataParsedAsEnumFromStringMapping<PositionAssetTypes>($"StockType{i}"),
                    TradeType = GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>($"tradeType{i}")
                });
            }

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, new List<ProductSubscriptions>
                {
                    ProductSubscriptions.TradeStopsLifetime, ProductSubscriptions.CryptoStopsLifetime
                }
            ));

            LoginSetUp.LogIn(UserModels.First());
            new AddPortfoliosSteps().NavigateToAddManualPortfolioSavePortfolioAfterFillingFields(portfolioModel, new List<PositionAtManualCreatingPortfolioModel>());
            new PositionsTabForm().ClickAddPositionButton();
            var addPositionInFrameForm = new AddPositionInFrameForm();
            addPositionInFrameForm.FillPositionsFields(positionsModel);
            addPositionInFrameForm.ClickSaveAndClose();

            var portfolioId = new PortfoliosQueries().SelectPortfolioIdForLastPortfolioByUserEmail(UserModels.First().Email);

            positionsIds = positionsQueries.SelectPositionIdsForPortfolio(portfolioId);
            positionsQueries.SetStatusTypeAsDelisted(positionsIds[1]);
            positionsQueries.SetStatusTypeAsExpired(positionsIds[3]);
            positionsQueries.SetStatusTypeAsDelisted(positionsIds[5]);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_112$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PositionsGrid"), TestCategory("PositionCard")]
        [Description("The test checks that links for position card leads to the corresponded position card. https://tr.a1qa.com/index.php?/cases/view/19232907")]
        public override void RunTest()
        {
            LogStep(1, "Go to Portfolios page");
            new MainMenuNavigation().OpenPortfolios();

            LogStep(2, "Click on the portfolio from Preconditions");
            var portfoliosForm = new PortfoliosForm();
            portfoliosForm.ClickOnPortfolioName(portfolioModel.Name);
            var positionsAlertsStatisticsPanelForm = new PositionsAlertsStatisticsPanelForm();
            positionsAlertsStatisticsPanelForm.ActivateTab(PositionsTabs.OpenPositions);

            LogStep(8, "Repeat 3-5 steps for other positions.");
            foreach (var positionId in positionsIds)
            {
                Steps3To7(positionId);
            }
        }

        private void Steps3To7(int positionId)
        {
            LogStep(3, 4, "Remember Id and name for 1st position.Click on 'Ticker' column for 1st position.");
            var positionTabForm = new PositionsTabForm();
            var positionTickerAndName = positionTabForm.GetPositionsGridCellValue(
                        new TableCellMetrics { PositionId = positionId, ColumnHeader = PositionsGridDataField.Ticker.GetStringMapping() });
            positionTabForm.ClickOnPositionLink(positionId);

            LogStep(5, "Check that the opened position card- has Name(from step 6)- portfolio name(1st portfolio from preconditions)- URL with positionId(from step 6) in the end ");
            var positionCardForm = new PositionCardForm();
            var actualName = positionCardForm.GetName();
            var expectedName = positionsQueries.SelectAssetTypeNameByPositionId(positionId).EqualsIgnoreCase(PositionAssetTypes.Option.GetStringMapping())
                ? optionNames.FirstOrDefault(t => t.Contains(actualName.Split(' ')[1]))
                : positionTickerAndName.Split('\r')[1].Replace("\n", "").ToUpperInvariant();
            Checker.CheckEquals(expectedName, actualName, "Position names are not the same");
            Checker.CheckEquals(portfolioModel.Name, positionCardForm.GetPortfolioLinkText(), "portfolio name are not the same");
            Checker.IsTrue(Browser.GetDriver().Url.Contains(positionId.ToString()), "URL doesn't contains position id in the end");

            LogStep(6, 7, "Click the tab 'My portfolios' and choose the Investment type. Click on the Name of portfolio from Preconditions.");
            positionCardForm.ClickOnPortfolioLink();
        }
    }
}