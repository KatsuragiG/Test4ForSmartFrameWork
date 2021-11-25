using AutomatedTests.Database.Portfolios;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.BaseTestsUnitTests;
using System.Collections.Generic;
using TradeStops.Common.Enums;
using AutomatedTests.Navigation;
using AutomatedTests.Enums.Positions;
using System.Linq;

namespace UnitTests.Tests._03_MyPortfolios._04_PortfolioGrid
{
    [TestClass]
    public class TC_0110_PortfoliosGrid_TransitionToPortfolioOnCurrentPositionsAlertsPageByClickingOnIt : BaseTestUnitTests
    {
        private const int TestNumber = 110;
        
        private int addedPortfolioId;
        private readonly List<PortfolioModel> portfoliosModels = new List<PortfolioModel>();

        [TestInitialize]
        public void TestInitialize()
        {
            portfoliosModels.Add(new PortfolioModel
            {
                Name = GetTestDataAsString("PortfolioName1"),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType1"),
                Currency = GetTestDataAsString("Currency")
            });

            portfoliosModels.Add(new PortfolioModel()
            {
                Name = GetTestDataAsString("PortfolioName2")
            });

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsLifetime));

            PortfoliosSetUp.AddInvestmentPortfoliosDefaultUSD(UserModels.First().Email);

            LoginSetUp.LogIn(UserModels.First());
            addedPortfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfoliosModels[0]);

            new MainMenuNavigation().OpenPortfolios();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_110$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("Test checks loading corresponded positions grid from portfolio grid; https://tr.a1qa.com/index.php?/cases/view/19232920")]
        [TestCategory("Smoke"), TestCategory("PortfoliosPage"), TestCategory("EditPortfolio")]
        public override void RunTest()
        {
            LogStep(1, "Click the tab 'My Portfolio' and choose the Watch Only type.");
            var portfoliosForm = new PortfoliosForm();
            portfoliosForm.ClickPortfolioTypeTab(portfoliosModels[0].Type);

            LogStep(2, "Choose 'Edit Portfolio' in the drop-down menu of the newly created portfolio.");
            var portfolioGridsSteps = new PortfolioGridsSteps();
            var editPortfolio = portfolioGridsSteps.ClickEditPortfolioByIdGetEditPopupForm(addedPortfolioId);

            LogStep(3, "Change Name for {noformat}<img style= '_-+:,/[{}[%^&*()_'> noformat");
            editPortfolio.SetPortfolioName(portfoliosModels[1].Name);

            LogStep(4, "Click Save in popup");
            editPortfolio.ClickSave();

            LogStep(5, "Click Name of portfolio from step 3");
            portfoliosForm.ClickOnPortfolioName(portfoliosModels[1].Name);
            var positionsAlertsStatisticsPanelForm = new PositionsAlertsStatisticsPanelForm();

            LogStep(6, "Open the drop-down menu of yours portfolio.");            
            positionsAlertsStatisticsPanelForm.ActivateTab(PositionsTabs.OpenPositions);

            LogStep(7, "Open the drop-down menu of yours portfolio.");
            Checker.CheckEquals(portfoliosModels[1].Name, positionsAlertsStatisticsPanelForm.GetPortfolioName(),
                "Portfolio name is not same as expected");

            LogStep(8, "Click More Details link (More Details are collapsed).");
            positionsAlertsStatisticsPanelForm.ClickPortfolioSummary();
            Checker.CheckEquals(portfoliosModels[1].Name,
                positionsAlertsStatisticsPanelForm.GetValueFromBasicSummaryBlock(PortfolioSummaryBasicValueTypes.Name),
                "'Name' from basic block is not as expected");
        }
    }
}