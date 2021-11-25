using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TradeStops.Common.Enums;
using TradeStops.Common.Extensions;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._04_PositionsAndAlerts._01_Positions
{
    [TestClass]
    public class TC_0115_PositionsGrid_ManagementPositionsEditingPositionWithoutSharesEntryDateEntryPrice : BaseTestUnitTests
    {
        private const int TestNumber = 115;

        private string portfolioName;
        private string optionName;
        private int portfolioId;

        [TestInitialize]
        public void TestInitialize()
        {
            optionName = GetTestDataAsString(nameof(optionName));

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPremium));
            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);

            var portfolioGridsSteps = new PortfolioGridsSteps();
            portfolioId = portfolioGridsSteps.ImportInvestmentPortfolio(new CustomTestDataReader().GetBrokerAccount().BrokerFullName, UserModels.First().Email, true);
            portfolioName = portfolioGridsSteps.RememberPortfolioInformationForPortfolioId(portfolioId).PortfolioName;
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_115$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PositionsGrid"), TestCategory("SyncPositions"), TestCategory("SyncPositionEditing")]
        [Description("https://tr.a1qa.com/index.php?/cases/view/19232906 The test checks editing position with incomplete data from Positions grid")]
        public override void RunTest()
        {
            LogStep(1, "Click on the imported portfolio from Preconditions");
            var portfoliosForm = new PortfoliosForm();
            portfoliosForm.ClickOnPortfolioNameViaId(portfolioId);
            var positionAlertsStatisticsPanelForm = new PositionsAlertsStatisticsPanelForm();
            positionAlertsStatisticsPanelForm.ActivateTab(PositionsTabs.OpenPositions);

            LogStep(3, "Detect all positions (positionIDs) that have 'context' icon in theirs rows");
            var positionsTab = new PositionsTabForm();
            var positionsOrdersWithWarningIcon = positionsTab.GetPositionsOrderWithWarningSign();
            Assert.IsTrue(positionsOrdersWithWarningIcon.Count > 0, "Positions with Warning icon quantity is 0");

            LogStep(4, "Remember Id and name for 1st detected position");
            var positionsIdsAndNames = positionsTab.GetPositionColumnValues(PositionsGridDataField.Ticker);

            LogStep(8, "Repeat 5-7 steps for other detected positions (from step 5)");
            var positionsQueries = new PositionsQueries();
            foreach (var positionOrder in positionsOrdersWithWarningIcon)
            {
                LogStep(5, "Click on pencil for position");
                var tickerName = positionsIdsAndNames[positionOrder - 1].Split('\r');
                positionsTab.ClickOnPositionLink(positionsTab.GetPositionIdFromGridByLineNumber(positionOrder));
                new PositionDetailsTabPositionCardForm().EditPositionCard();

                LogStep(7, "Check that the opened position card has Name(from step 5), portfolio name(the portfolio from preconditions) and url contains positionId(from step 5) in the end ");
                var positionCardForm = new PositionCardForm();
                Checker.CheckEquals(tickerName[0], positionCardForm.GetSymbol(), "Position Tickers are not equal");
                var positionId = positionCardForm.GetPositionIdFromUrl();
                var expectedName = positionsQueries.SelectAssetTypeNameByPositionId(positionId).EqualsIgnoreCase(PositionAssetTypes.Option.GetStringMapping())
                    ? optionName
                    : tickerName[1].Replace("\n", "").ToUpperInvariant();
                Checker.CheckEquals(expectedName, positionCardForm.GetName(), "Position Names are not equal");
                Checker.CheckEquals(portfolioName, positionCardForm.GetPortfolioLinkText(), "Portfolio name is not the same");

                LogStep(8, "Click on Portfolio link");
                positionCardForm.ClickCancel();
                positionCardForm.ClickOnPortfolioLink();
            }
        }
    }
}