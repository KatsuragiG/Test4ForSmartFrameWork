using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Newsletter;
using AutomatedTests.Enums;
using AutomatedTests.Forms.Charts;
using AutomatedTests.Forms.Gurus;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Forms.ResearchPages.StockAnalyzer;
using AutomatedTests.Forms.TimingCalendar;
using AutomatedTests.Forms;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.BrowserSteps;
using AutomatedTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;
using AutomatedTests.Enums.Tools.StockAnalyzer;

namespace UnitTests.Tests._01_General
{
    [TestClass]
    public class TC_0000_General_Newsletters_StockAnalyzer_AvailabilityAllPossiblePagesByDirectLinks : BaseTestUnitTests
    {
        private string defaultTicker;
        private const int CryptoCoinSymbolId = 23727169;
        private const int OptionSymbolId = 27334096;
        private const string StockAnalyzerPageDescription = "Stock Analyzer";
        private const string StockRatingPageDescription = "Stock Rating overview";
        private readonly List<StockAnalyzerTabs> tabsOnStockAnalyzerForStock = EnumExtensions.EnumUtility.GetValues<StockAnalyzerTabs>()
            .Except(new List<StockAnalyzerTabs> { StockAnalyzerTabs.Insights, StockAnalyzerTabs.CoinProfile, StockAnalyzerTabs.Distributions }).ToList();
        private static readonly List<StockAnalyzerTabs> tabsOnStockAnalyzerForOption = new List<StockAnalyzerTabs>
        {
            StockAnalyzerTabs.StopLossAnalysis,
            StockAnalyzerTabs.Statistics,
            StockAnalyzerTabs.Insights,
            StockAnalyzerTabs.ChartSettings
        };
        private readonly List<StockAnalyzerTabs> tabsOnStockAnalyzerForCrypto = tabsOnStockAnalyzerForOption
            .Except(new List<StockAnalyzerTabs> { StockAnalyzerTabs.Insights })
            .Concat(new List<StockAnalyzerTabs> { StockAnalyzerTabs.News, StockAnalyzerTabs.MyOpportunities, StockAnalyzerTabs.CoinProfile }).ToList();

        [TestInitialize]
        public void TestInitialize()
        {
            defaultTicker = new CustomTestDataReader().GetDefaultTicker();

            LogStep(0, "Precondition - Login as registered user");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(0, new List<ProductSubscriptions> {
                ProductSubscriptions.TradeStopsPlatinum
            }));

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [Description("The test checks all possible pages availability and opening using direct links https://tr.a1qa.com/index.php?/cases/view/19235575")]
        [TestMethod]
        [TestCategory("Smoke")]
        public override void RunTest()
        {
            LogStep(1, "Check Investment and Watch Only portfolios page is opened. No errors in console.");
            var mainMenuNavigation = new MainMenuNavigation();
            mainMenuNavigation.OpenNewsletters();
            var mainMenuPublisher = new MainPublishersForm();
            mainMenuPublisher.AssertIsOpen();
            var browserSteps = new BrowserSteps();
            browserSteps.CheckBrowserConsoleForErrors(GurusMenuItems.Newsletters.ToString());

            LogStep(2, "Open all publishers pages");
            mainMenuNavigation.OpenCustomPublisherGrid();
            new SelectedPublisherForm().AssertIsOpen();
            Checker.CheckEquals(Constants.CustomPublisherPortfolioName, new SelectedPublisherForm().GetPortfolioName(), "Portfolio Name is not as expected");
            browserSteps.CheckBrowserConsoleForErrors($"{GurusMenuItems.Newsletters} for custom publisher");

            mainMenuNavigation.OpenGurusOverview();
            new GurusOverviewForm().AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(GurusMenuItems.Overview.ToString());

            mainMenuNavigation.OpenGurusTopRecommendations();
            new GurusTopRecommendationsForm().AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(GurusMenuItems.TopRecomendations.GetStringMapping());

            mainMenuNavigation.OpenGurusBillionairesClub();
            new SelectedPublisherForm().AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(GurusMenuItems.BillionairesClub.GetStringMapping());

            mainMenuNavigation.OpenGurusDecoder();
            mainMenuPublisher.AssertIsOpen();
            Checker.IsTrue(mainMenuPublisher.IsNoPortfoliosMessagePresent(), "Decoder portfolio is not empty");
            browserSteps.CheckBrowserConsoleForErrors(GurusMenuItems.Decoder.ToString());

            LogStep(3, "Stock Analyzer (AAPL)");
            mainMenuNavigation.OpenStockAnalyzerForDefaultTicker();
            var stockAnalyzerForm = new StockAnalyzerForm();
            stockAnalyzerForm.AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors($"{StockAnalyzerPageDescription} for ticker {defaultTicker}");

            foreach (var tab in tabsOnStockAnalyzerForStock)
            {
                CheckStockAnalyzerTab(tab);
            }

            LogStep(4, "Stock Analyzer (BTC/USD)");
            mainMenuNavigation.OpenStockAnalyzerForSymbolId(CryptoCoinSymbolId);
            stockAnalyzerForm.AssertIsOpen();
            var positionsQueries = new PositionsQueries();
            var cryptoTicker = positionsQueries.SelectSymbolBySymbolId(CryptoCoinSymbolId);
            browserSteps.CheckBrowserConsoleForErrors($"{StockAnalyzerPageDescription} for ticker {cryptoTicker}");

            foreach (var tab in tabsOnStockAnalyzerForCrypto)
            {
                CheckStockAnalyzerTab(tab);
            }

            LogStep(5, "Stock Analyzer (Option)");
            mainMenuNavigation.OpenStockAnalyzerForSymbolId(OptionSymbolId);
            stockAnalyzerForm.AssertIsOpen();
            var optionTicker = positionsQueries.SelectSymbolBySymbolId(OptionSymbolId);
            browserSteps.CheckBrowserConsoleForErrors($"{StockAnalyzerPageDescription} for ticker {optionTicker}");

            foreach (var tab in tabsOnStockAnalyzerForOption)
            {
                CheckStockAnalyzerTab(tab);
            }

            LogStep(6, "Open Timing Calendar");
            mainMenuNavigation.OpenTimingCalendar();
            var timingCalendarForm = new TimingCalendarForm();
            timingCalendarForm.AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors("Timing Calendar Form");

            LogStep(7, "Stock Rating (AAPL)");
            mainMenuNavigation.OpenStockRatingForDefaultTicker();
            var stockRatingForm = new StockRatingForm();
            stockRatingForm.AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors($"{StockRatingPageDescription} for ticker {defaultTicker}");
        }

        private static void CheckStockAnalyzerTab(StockAnalyzerTabs tab)
        {
            new StockAnalyzerForm().ActivateTabWithoutChartWaiting(tab);
            switch (tab)
            {
                case StockAnalyzerTabs.StopLossAnalysis:
                    new StopLossAnalysisTabForm().AssertIsOpen();
                    break;
                case StockAnalyzerTabs.Statistics:
                    new StatisticTabForm().AssertIsOpen();
                    break;
                case StockAnalyzerTabs.MyOpportunities:
                    new MyOpportunitiesTabForm().AssertIsOpen();
                    break;
                case StockAnalyzerTabs.News:
                    new NewsTabForm().AssertIsOpen();
                    break;
                case StockAnalyzerTabs.CompanyProfile:
                    new CompanyProfileTabForm().AssertIsOpen();
                    break;
                case StockAnalyzerTabs.ChartSettings:
                    new ChartSettingsTabForm().AssertIsOpen();
                    break;
                case StockAnalyzerTabs.Options:
                    new OptionsTabCommonForm().AssertIsOpen();
                    break;
                case StockAnalyzerTabs.Insights:
                    new InsightsTabForm().AssertIsOpen();
                    break;
                case StockAnalyzerTabs.Financial:
                    new FinancialsTabForm().AssertIsOpen();
                    break;
                case StockAnalyzerTabs.CoinProfile:
                    new CoinProfileTabForm().AssertIsOpen();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(Constants.UnexpectedTabExceptionText);
            }
            new BrowserSteps().CheckBrowserConsoleForErrors(tab.GetStringMapping());
        }
    }
}