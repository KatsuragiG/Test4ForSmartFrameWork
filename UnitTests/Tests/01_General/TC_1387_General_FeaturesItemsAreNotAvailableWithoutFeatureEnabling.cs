using System;
using AutomatedTests;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Enums.PositionCard;
using AutomatedTests.Enums.Tools;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Other;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Forms.ResearchPages;
using AutomatedTests.Forms.ResearchPages.StockAnalyzer;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.BrowserSteps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using AutomatedTests.Enums.Tools.StockAnalyzer;

namespace UnitTests.Tests._01_General
{
    [TestClass]
    public class TC_1387_General_FeaturesItemsAreNotAvailableWithoutFeatureEnabling : BaseTestUnitTests
    {
        private const int TestNumber = 1387;
        private const int OptionSymbolId = 27334096;
        private readonly PositionsQueries positionsQueries = new PositionsQueries();

        private int stockId;
        private int optionId;
        private string defaultTicker;

        [TestInitialize]
        public void TestInitialize()
        {
            defaultTicker = new CustomTestDataReader().GetDefaultTicker();

            LogStep(0, "Precondition - Login as user with maximum subscriptions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, new List<ProductSubscriptions> {
                ProductSubscriptions.TradeStopsLifetime,
                ProductSubscriptions.CyclesLifetime,
                ProductSubscriptions.ChartAsImage,
                ProductSubscriptions.LikeFolio,
                ProductSubscriptions.CryptoStopsLifetime,
                ProductSubscriptions.TradeIdeasLifetime,
                ProductSubscriptions.PortfolioAnalyzer,
                ProductSubscriptions.CryptoIdeasLifetime,
                ProductSubscriptions.PureQuantPremium,
                ProductSubscriptions.PortfolioLite,
                ProductSubscriptions.TestOrganization,
                ProductSubscriptions.AllNewsletters,
                ProductSubscriptions.DecoderLifetime,
                ProductSubscriptions.Megatrends,
                ProductSubscriptions.EarningsReports,
                ProductSubscriptions.TimingReport,
                ProductSubscriptions.TrendsLifetime,
                ProductSubscriptions.RatingsLifetime
            }));

            var userModel = UserModels.First();
            PortfoliosSetUp.AddInvestmentUsdPortfoliosWithOpenPosition(userModel.Email);
            stockId = PortfoliosSetUp.AddInvestmentUsdPortfoliosWithOpenPosition(userModel.Email);
            optionId = PortfoliosSetUp.AddWatchOnlyUsdPortfoliosWithOpenPosition(userModel.Email, positionsQueries.SelectSymbolBySymbolId(OptionSymbolId));

            LoginSetUp.LogIn(userModel);
            new MainMenuForm().ClickMenuItem(MainMenuItems.Invest);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [TestMethod]
        [TestCategory("Permissions")]
        [Description("The test checks impossibility of opening feature-based items without feature enabling. https://tr.a1qa.com/index.php?/cases/view/21631332")]
        public override void RunTest()
        {
            LogStep(1, "Check that Backtester item is not available in Invest menu");
            Checker.IsFalse(new InvestMenuForm().IsTabPresent(InvestMenuItems.Backtester), "Backtester item is available in menu without feature enabling");
            var mainMenuNavigation = new MainMenuNavigation();
            mainMenuNavigation.OpenBackTesterTasks();
            var browserSteps = new BrowserSteps();
            browserSteps.CheckThatPageOpensGetPageWithSoftAssert<Error404Form>();

            LogStep(2, "Check that Options screener is not available");
            mainMenuNavigation.OpenStockAnalyzerForDefaultTicker();
            Checker.IsFalse(new MainMenuForm().IsMenuItemPresent(MainMenuItems.Trade), "Options screener is available in menu without feature enabling");
            mainMenuNavigation.OpenOptionScreeners();
            browserSteps.CheckThatPageOpensGetPageWithSoftAssert<Error404Form>();

            LogStep(3, "Open Options Opportunities URL and check that 404 error is shown");
            mainMenuNavigation.OpenOptionOpportunities();
            browserSteps.CheckThatPageOpensGetPageWithSoftAssert<Error404Form>();

            LogStep(4, "Check that Stock Analyzer for stock and option does not have Option/Advanced tab");
            mainMenuNavigation.OpenStockAnalyzerForDefaultTicker();
            CheckTabsAbsenceOnStockAnalyzer(defaultTicker);
            CheckDirectUrlOpening();
            mainMenuNavigation.OpenStockAnalyzerForSymbolId(OptionSymbolId);
            CheckTabsAbsenceOnStockAnalyzer(positionsQueries.SelectSymbolBySymbolId(OptionSymbolId));
            CheckDirectUrlOpening();

            LogStep(5, "Check that Position card for stock and option does not have Option/Advanced tab");
            mainMenuNavigation.OpenPositionCard(stockId);
            CheckTabsAbsenceOnPositionCard(defaultTicker);
            CheckDirectUrlOpening();
            mainMenuNavigation.OpenPositionCard(optionId);
            CheckTabsAbsenceOnPositionCard(positionsQueries.SelectSymbolBySymbolId(OptionSymbolId));
            CheckDirectUrlOpening();

            LogStep(6, "Open Copilot Companion URL and check that 404 error is shown");
            mainMenuNavigation.OpenCoPilot();
            browserSteps.CheckThatPageOpensGetPageWithSoftAssert<Error404Form>();

            LogStep(7, "Open Platinum Area URL and check that 404 error is shown");
            mainMenuNavigation.OpenPlatinumArea();
            browserSteps.CheckThatPageOpensGetPageWithSoftAssert<Error404Form>();
        }

        private void CheckTabsAbsenceOnStockAnalyzer(string ticker)
        {
            var stockAnalyzerForm = new StockAnalyzerForm();
            Checker.IsFalse(stockAnalyzerForm.IsTabPresent(StockAnalyzerTabs.Insights), $"Advanced tab is available for {ticker} on Stock Analyzer without feature enabling");
            Checker.IsFalse(stockAnalyzerForm.IsTabPresent(StockAnalyzerTabs.Options), $"Options tab is available for {ticker} on Stock Analyzer without feature enabling");
        }

        private void CheckDirectUrlOpening()
        {
            var currentUrl = Browser.GetDriver().Url;
            var currentUrlWithoutTabname = currentUrl.Substring(0, currentUrl.LastIndexOf("/", StringComparison.Ordinal) + 1);
            var mainMenuNavigation = new MainMenuNavigation();
            mainMenuNavigation.NavigateTo($"{currentUrlWithoutTabname}/options");
            var browserSteps = new BrowserSteps();
            browserSteps.CheckThatPageOpensGetPageWithSoftAssert<Error404Form>();
            mainMenuNavigation.NavigateTo($"{currentUrlWithoutTabname}/advanced");
            browserSteps.CheckThatPageOpensGetPageWithSoftAssert<Error404Form>();
        }

        private void CheckTabsAbsenceOnPositionCard(string ticker)
        {
            var positionCardForm = new PositionCardForm();
            Checker.IsFalse(positionCardForm.IsTabPresent(PositionCardTabs.Insights), $"Advanced tab is available for {ticker} on Position Card without feature enabling");
            Checker.IsFalse(positionCardForm.IsTabPresent(PositionCardTabs.Options), $"Options tab is available for {ticker} on Position Card without feature enabling");
        }
    }
}