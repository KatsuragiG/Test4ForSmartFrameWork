using AutomatedTests;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Enums;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.SyncFlow;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._03_MyPortfolios._04_PortfolioGrid
{
    [TestClass]
    public class TC_0156_PortfoliosGrid_ResolveButtonForOnePortfolioWithErrorNonMFA : BaseTestUnitTests
    {
        private const int TestNumber = 156;

        private CustomTestDataReader reader;
        private int portfolioId;

        [TestInitialize]
        public void TestInitialize()
        {
            reader = new CustomTestDataReader();
            var isRequiredNewCredentialsNumber = GetTestDataAsInt("isRequiredNewCredentialsNumber");

            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPlus));
            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);

            new PortfolioGridsSteps().ImportInvestmentPortfolioUsingCusmomCredentials(reader.GetBrokerAccount().BrokerFullName,
                reader.GetBrokerAccount().Account, reader.GetBrokerAccount().Password, true);
            var portfoliosQueries = new PortfoliosQueries();
            portfolioId = portfoliosQueries.SelectPortfolioIdForLastPortfolioByUserEmail(UserModels.First().Email);
            Assert.IsTrue(portfolioId > 0, "No one portfolio was imported");

            portfoliosQueries.MakeIsRequiredNewCredentialsParameterTo(isRequiredNewCredentialsNumber, portfolioId);
            new MainMenuNavigation().OpenDashboard();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_154$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("PortfoliosPage"), TestCategory("SyncPortfolio"), TestCategory("SyncPortfolioUpdate")]
        [Description("Test checks that Resolve button is solved by correct credentials entering for non-MFA broker https://tr.a1qa.com/index.php?/cases/view/19232869")]
        public override void RunTest()
        {
            LogStep(1, "Reload portfolio grid");
            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);

            LogStep(2, "Check that all imported portfolios has active Resolve button");
            var portfoliosForm = new PortfoliosForm();
            Checker.IsTrue(portfoliosForm.IsResolveButtonExistsForPortfolio(portfolioId), $"Resolve button does not exist for {portfolioId}");

            LogStep(3, "Click Resolved button for portfolio (see precondition #1)");
            portfoliosForm.ClickResolve(portfolioId);

            LogStep(4, 7, "Enter *correct* login and password, Click Update ");
            new ImportPortfoliosSteps().SetLoginPasswordClickSyncBrokerage(reader.GetBrokerAccount().Account, reader.GetBrokerAccount().Password);
            new SyncFlowImportForm().ClickNextIfPresent();

            LogStep(8, "Check that for refreshed portfolio Resolve button is not displayed");
            Checker.IsFalse(portfoliosForm.IsResolveButtonExistsForPortfolio(portfolioId), $"Resolve button exist for {portfolioId}");

            LogStep(9, 11, "Click Refresh button for the Dag Site – Investments portfolio");
            new PortfolioGridsSteps().ClickRefreshPortfolioIdViaSyncFlow(portfolioId);

            Checker.IsFalse(portfoliosForm.IsResolveButtonExistsForPortfolio(portfolioId), $"Resolve button exist for {portfolioId} after refresh");
        }
    }
}