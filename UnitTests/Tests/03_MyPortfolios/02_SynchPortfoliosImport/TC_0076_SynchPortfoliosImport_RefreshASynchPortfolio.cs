using System;
using System.Linq;
using AutomatedTests;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.SyncFlow;
using AutomatedTests.Models.PortfoliosModels;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.BrowserSteps;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._03_MyPortfolios._02_SynchPortfoliosImport
{
    [TestClass]
    public class TC_0076_SynchPortfoliosImport_RefreshASynchPortfolio : BaseTestUnitTests
    {
        private const int TestNumber = 76;

        private int portfolioId;
        private PortfoliosListRowModel portfolioDataBeforeUpdate;

        [TestInitialize]
        public void TestInitialize()
        {
            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPremium));
            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);

            var portfolioGridsSteps = new PortfolioGridsSteps();
            portfolioId = portfolioGridsSteps.ImportInvestmentPortfolio(new CustomTestDataReader().GetBrokerAccount().BrokerFullName, UserModels.First().Email, true);
            portfolioDataBeforeUpdate = portfolioGridsSteps.RememberPortfolioInformationForPortfolioId(portfolioId);
            new PortfoliosQueries().UpdateSynchDateInPortfoliosByPortfolioId(Parsing.ConvertToShortDateString(DateTime.Now.AddMonths(-1).ToShortDateString()), portfolioId);
            new BrowserSteps().LogoutClearCookiesLogin(UserModels.First());
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PortfoliosPage"), TestCategory("SyncPortfolio"), TestCategory("SyncPortfolioUpdate")]
        [Description("Test checks availability of the manual refresh of synch portfolio; https://tr.a1qa.com/index.php?/cases/view/19232941")]
        public override void RunTest()
        {
            LogStep(1, 2, "Login. Click on Portfolios link");
            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);            

            LogStep(3, "Check that portfolio from Precondition step 1 is listed in grid");
            var portfoliosForm = new PortfoliosForm();
            Assert.IsTrue(portfolioDataBeforeUpdate.Id != null, "There are no imported portfolio data");
            Checker.IsTrue(portfoliosForm.GetAllPortfoliosIdsByType(AllPortfoliosKinds.Investment).Count > 0, "There are no imported portfolio data");

            LogStep(4, "Click Refresh button for portfolio from Precondition step 1");
            portfoliosForm.SelectPortfolioContextMenuOption(portfolioId, PortfolioContextNavigation.Synchronize);
            var syncFlowEditForm = new SyncFlowEditForm();

            LogStep(5, "Check that hint appears at mouse over label for Refresh Portfolio option");
            var actualHint = syncFlowEditForm.GetHintForSyncItem(SynchFlowEditItems.Refresh);
            Checker.IsTrue(actualHint.Length > 0, "Update hint is empty");

            LogStep(6, "Click Refresh Portfolio option -> Synchronize button");
            syncFlowEditForm.ClickRefreshPortfolio();

            LogStep(7, "Check that Refreshing is finished successfully");
            syncFlowEditForm.ClickNextIfPresent();

            LogStep(8, "Check Synch Date for the Portfolio on the Portfolios page.");
            var portfolioDataAfterUpdate = new PortfolioGridsSteps().RememberPortfolioInformationForPortfolioId(portfolioId);
            var createdDate = Parsing.ConvertToShortDateString(DateTime.Now.ToShortDateString());
            Checker.CheckEquals(createdDate, portfolioDataAfterUpdate.SynchDate, 
                "Synch date is not the same with current date");
            Checker.CheckEquals(createdDate, portfolioDataAfterUpdate.CreatedDate,
                "Created date is not the same with current date");
            Checker.CheckEquals(portfolioDataBeforeUpdate.PortfolioName, portfolioDataAfterUpdate.PortfolioName, "Name is not the same");
            Checker.CheckEquals(portfolioDataBeforeUpdate.Cash, portfolioDataAfterUpdate.Cash, "Cash is not the same");
            Checker.CheckEquals(portfolioDataBeforeUpdate.Notes, portfolioDataAfterUpdate.Notes, "Notes is not the same");
            Checker.CheckEquals(portfolioDataBeforeUpdate.Currency, portfolioDataAfterUpdate.Currency, "Currency is not the same");
            Checker.CheckEquals(portfolioDataBeforeUpdate.Alerts, portfolioDataAfterUpdate.Alerts, "Alerts is not the same");
            Checker.CheckEquals(portfolioDataBeforeUpdate.TriggeredAlerts, portfolioDataAfterUpdate.TriggeredAlerts, "Triggered Alerts is not the same");
            Checker.CheckEquals(portfolioDataBeforeUpdate.Brokerage, portfolioDataAfterUpdate.Brokerage, "Brokerage is not the same");
            Checker.CheckEquals(portfolioDataBeforeUpdate.BrokerageValue, portfolioDataAfterUpdate.BrokerageValue, "Brokerage Value is not the same");
        }
    }
}