using AutomatedTests;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.SyncFlow;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._03_MyPortfolios._02_SynchPortfoliosImport
{
    [TestClass]
    public class TC_0074_SynchPortfoliosImport_UpdateCredentials : BaseTestUnitTests
    {
        private const int TestNumber = 74;
        private int portfolioId;

        [TestInitialize]
        public void TestInitialize()
        {
            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPremium));
            LoginSetUp.LogIn(UserModels.First());

            portfolioId = new PortfolioGridsSteps().ImportInvestmentPortfolio(new CustomTestDataReader().GetBrokerAccount().BrokerFullName, UserModels.First().Email, true);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PortfoliosPage"), TestCategory("SyncPortfolio"), TestCategory("SyncPortfolioUpdate")]
        [Description("Test checks availability of the updating credentials for synch portfolio; https://tr.a1qa.com/index.php?/cases/view/19232940")]
        public override void RunTest()
        {
            LogStep(1, "Open Investment portfolio grid");
            var mainMenuNavigation = new MainMenuNavigation();
            mainMenuNavigation.OpenInvestmentPortfoliosTab();

            LogStep(2, 3, "Check that portfolio from Precondition step 1 is listed in grid. Remember Portfolio Name, Cash, Notes, Created As for portfolio from Precondition step 1");
            var portfolioGridsSteps = new PortfolioGridsSteps();
            var portfolioData = portfolioGridsSteps.RememberPortfolioInformationForPortfolioId(portfolioId);
            var portfoliosForm = new PortfoliosForm();
            Checker.CheckListsEquals(new List<int> { portfolioId }, portfoliosForm.GetPortfoliosIds(), 
                "Portfolio from Precondition step 1 is NOT listed in grid");
            Checker.IsTrue(portfolioData != null, "portfolioData is empty");

            LogStep(4, "Click 'Synchronize' (GridCellSyncIcon in the grid)");
            new MainMenuNavigation().OpenSyncFlowYodleeSynchronize(new PortfoliosQueries().SelectPortfolioDataByPortfolioId(portfolioId).VendorAccountId);
            var syncFlowEditForm = new SyncFlowEditForm();           

            LogStep(5, "Check that hint is appeared at mouse over label for Update Credentials and its not empty");
            var actualHint = syncFlowEditForm.GetHintForSyncItem(SynchFlowEditItems.UpdateCreds);
            Checker.IsTrue(actualHint.Length > 0, "Update hint is empty");

            LogStep(6, "Click Update");
            syncFlowEditForm.ClickUpdateCredentials();

            LogStep(7, 10, "Enter login and password. Click Show my password checkbox. Click Update. Check that Updating Credentials is finished successfully. Click OK in popup");
            new ImportPortfoliosSteps().UpdateCredentialsNonMfaWithShowPassword();
            portfoliosForm.AssertIsOpen();

            LogStep(12, "Check that Portfolio Name, Cash, Notes of portfolio is same as for step 3");
            var portfolioInformation = portfolioGridsSteps.RememberPortfolioInformationForPortfolioId(portfolioId);

            Checker.CheckEquals(portfolioData.PortfolioName, portfolioInformation.PortfolioName, "Name is not the same");
            Checker.CheckEquals(portfolioData.Cash, portfolioInformation.Cash, "Cash is not the same");
            Checker.CheckEquals(portfolioData.Notes, portfolioInformation.Notes, "Notes is not the same");
            Checker.CheckEquals(portfolioData.Currency, portfolioInformation.Currency, "Currency is not the same");
            Checker.CheckEquals(portfolioData.Alerts, portfolioInformation.Alerts, "Alerts is not the same");
            Checker.CheckEquals(portfolioData.TriggeredAlerts, portfolioInformation.TriggeredAlerts, "Triggered Alerts is not the same");
            Checker.CheckEquals(portfolioData.Brokerage, portfolioInformation.Brokerage, "Brokerage is not the same");
            Checker.CheckEquals(portfolioData.BrokerageValue, portfolioInformation.BrokerageValue, "Brokerage Value is not the same");
        }
    }
}