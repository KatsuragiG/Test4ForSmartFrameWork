using System.Linq;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Users;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._03_MyPortfolios._02_SynchPortfoliosImport
{
    [TestClass]
    public class TC_0817_SynchPortfoliosImport_ReEntryPositionsAreNotCreatedAfterClosingSyncPositionsAtTheDisabledSetting : BaseTestUnitTests
    {
        private const int TestNumber = 817;

        [TestInitialize]
        public void TestInitialize()
        {
            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPlus));

            var settingsSteps = new SettingsSteps();
            settingsSteps.LoginNavigateToSettingsPositionGetForm(UserModels.First());
            settingsSteps.ChangeReEntryForSynchronizedPortfolioStateWithSaving(false);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [TestMethod]
        [TestCategory("PortfoliosPage"), TestCategory("SyncPortfolio"), TestCategory("SyncPositions")]
        [Description("Test checks that ReEntry Positions Are Not Created After Closing Sync Positions if corresponded setting is disabled. https://tr.a1qa.com/index.php?/cases/view/22094403")]
        public override void RunTest()
        {
            LogStep(1, "Import portfolio");
            PortfoliosSetUp.ImportDagSiteInvestment15(true);

            LogStep(2, "Remember portfolioId");
            var tradeSmithUserId = UserModels.First().TradeSmithUserId;
            var portfolioId = new UsersQueries().SelectAllUserPortfolioIds(tradeSmithUserId).Last();

            LogStep(3, "Click Edit sign for the portfolio.Click Update Credentials button. Change creds for Investments with creds Account20 / Password20. Close success popup");
            PortfoliosSetUp.UpdatePortfolioWithDagSiteInvestment20(portfolioId);

            LogStep(4, "Open Portfolio grid.Check that 'Re-Entry Watchlist' portfolio is not listed in the grid");
            new MainMenuNavigation().OpenPortfolios(PortfolioType.WatchOnly);
            Checker.IsFalse(new PortfoliosForm().GetPortfolioColumnValues(PortfolioGridColumnTypes.PortfolioName).Contains(AllPortfoliosKinds.ReEntryWatchlist.GetStringMapping()),
                "'Re-Entry Watchlist' portfolio is listed in the grid");

            LogStep(5, "Check in DB that manual portfolio 'Re-Entry Watchlist' does not exist for the user");
            var portfolioIdsDb = new UsersQueries().SelectAllUserPortfolioIds(tradeSmithUserId);
            Assert.IsTrue(portfolioIdsDb.Count > 0, "There are no portfolios for user in DB");

            var portfolioNamesDb = portfolioIdsDb.Select(t => new PortfoliosQueries().SelectPortfolioName(t)).ToList();
            Checker.IsFalse(portfolioNamesDb.Contains(AllPortfoliosKinds.ReEntryWatchlist.GetStringMapping()),
                "Manual portfolio 'Re-Entry Watchlist' does not exist for the user in DB");
        }
    }
}