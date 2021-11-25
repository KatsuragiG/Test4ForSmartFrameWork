using AutomatedTests.Database.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Enums.PositionCard;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Charts;
using AutomatedTests.Forms.Dashboard;
using AutomatedTests.Forms.Other;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.BrowserSteps;
using AutomatedTests.Steps.PositionCard;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using AutomatedTests.ConstantVariables;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;
using static WebdriverFramework.Framework.Util.EnumExtensions;

namespace UnitTests.Tests._01_General
{
    [TestClass]
    public class TC_0000_General_Dashboard_AvailabilityAllPossiblePagesByDirectLinks : BaseTestUnitTests
    {
        private const string Error404Description = "randomlink123";
        private const string Error500Description = "500 error page";
        private const int CryptoCoinSymbolId = 23727169;
        private const int OptionSymbolId = 27326263;
        private int stockId;
        private int cryptoId;
        private int optionId;
        private const string PositionCardPageDescription = "Position Card";
        private readonly PositionsQueries positionsQueries = new PositionsQueries();
        private readonly List<PositionCardTabs> tabsOnPositionCardForStock = EnumUtility.GetValues<PositionCardTabs>()
            .Except(new List<PositionCardTabs> { PositionCardTabs.Insights, PositionCardTabs.CoinProfile }).ToList();
        private static readonly List<PositionCardTabs> tabsOnPositionCardForOption = new List<PositionCardTabs>
        {
            PositionCardTabs.PositionDetails,
            PositionCardTabs.Performance,
            PositionCardTabs.Alerts,            
            PositionCardTabs.Statistics,
            PositionCardTabs.TagsAndNotes,
            PositionCardTabs.Insights,
            PositionCardTabs.ChartSettings
        };
        private readonly List<PositionCardTabs> tabsOnPositionCardForCrypto = tabsOnPositionCardForOption
            .Except(new List<PositionCardTabs> { PositionCardTabs.Insights })
            .Concat(new List<PositionCardTabs> { PositionCardTabs.News, PositionCardTabs.CoinProfile, PositionCardTabs.MyOpportunities }).ToList();

        [TestInitialize]
        public void TestInitialize()
        {
            LogStep(0, "Precondition - Login as registered user");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(0, ProductSubscriptions.TradeStopsPlatinum));

            stockId = PortfoliosSetUp.AddInvestmentUsdPortfoliosWithOpenPosition(UserModels.First().Email);
            cryptoId = PortfoliosSetUp.AddWatchOnlyUsdPortfoliosWithOpenPosition(UserModels.First().Email, positionsQueries.SelectSymbolBySymbolId(CryptoCoinSymbolId));
            optionId = PortfoliosSetUp.AddWatchOnlyUsdPortfoliosWithOpenPosition(UserModels.First().Email, positionsQueries.SelectSymbolBySymbolId(OptionSymbolId));

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.Description("The test checks all possible pages availability and opening using direct links https://tr.a1qa.com/index.php?/cases/view/19241886")]
        [TestMethod]
        [TestCategory("Smoke")]
        public override void RunTest()
        {
            LogStep(1, "404 error page: { Home Url}/ random123");
            Browser.NavigateTo($"{HomeUrl}/randomlink123");
            new Error404Form().AssertIsOpen();
            var browserSteps = new BrowserSteps();
            browserSteps.CheckBrowserConsoleForErrors(Error404Description);

            LogStep(2, "500 error: { Home Url}/ error / 500");
            Browser.NavigateTo($"{HomeUrl}/error/500");
            new Error500Form().AssertIsOpen();            
            browserSteps.CheckBrowserConsoleForErrors(Error500Description);

            LogStep(3, "Check Dashboard");
            var mainMenuNavigation = new MainMenuNavigation();
            mainMenuNavigation.OpenDashboard();
            new DashboardForm().AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(MainMenuItems.Dashboard.ToString());

            LogStep(4, "Position card (AAPL)");
            mainMenuNavigation.OpenPositionCard(stockId);
            var positionCardForm = new PositionCardForm();
            positionCardForm.AssertIsOpen();

            browserSteps.CheckBrowserConsoleForErrors($"{PositionCardPageDescription} for ticker {positionsQueries.SelectSymbolByPositionId(stockId)}");

            foreach (var tab in tabsOnPositionCardForStock)
            {
                CheckPositionCardTab(tab);
            }

            LogStep(5, "Position card (BTC/USD)");
            mainMenuNavigation.OpenPositionCard(cryptoId);
            positionCardForm.AssertIsOpen();

            browserSteps.CheckBrowserConsoleForErrors($"{PositionCardPageDescription} for ticker {positionsQueries.SelectSymbolBySymbolId(CryptoCoinSymbolId)}");

            foreach (var tab in tabsOnPositionCardForCrypto)
            {
                CheckPositionCardTab(tab);
            }

            LogStep(6, "Position card (Option)");
            new PositionCardSteps().ResavePositionCard(optionId);
            positionCardForm.AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors($"{PositionCardPageDescription} for ticker {positionsQueries.SelectSymbolByPositionId(optionId)}");

            foreach (var tab in tabsOnPositionCardForOption)
            {
                CheckPositionCardTab(tab);
            }

            LogStep(7, "Position card-Company documents (AAPL)");
            mainMenuNavigation.OpenPositionCard(stockId);
            positionCardForm.ActivateTabWithoutChartWaiting(PositionCardTabs.Financials);
            var financialsTabForm = new FinancialsTabForm();
            foreach (var type in EnumUtility.GetValues<FinancialTabCompanyDocumentsTypes>())
            {
                financialsTabForm.SelectDocumentType(type);
                financialsTabForm.AssertIsOpen();
                browserSteps.CheckBrowserConsoleForErrors($"Company documents tab {type.GetStringMapping()} " +
                    $"for ticker {positionsQueries.SelectSymbolByPositionId(stockId)}");
            }

            LogStep(8, "Platinum Area");
            mainMenuNavigation.OpenPlatinumArea();
            new PlatinumAreaDashboardForm().AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors("Platinum Area Dashboard Form");

        }

        private static void CheckPositionCardTab(PositionCardTabs tab)
        {
            new PositionCardForm().ActivateTabWithoutChartWaiting(tab);
            switch (tab)
            {
                case PositionCardTabs.PositionDetails:
                    new PositionDetailsTabPositionCardForm().AssertIsOpen();
                    break;
                case PositionCardTabs.Performance:
                    new PerformanceTabPositionCardForm().AssertIsOpen();
                    break;
                case PositionCardTabs.Alerts:
                    new AlertsTabPositionCardForm().AssertIsOpen();
                    break;
                case PositionCardTabs.MyOpportunities:
                    new MyOpportunitiesTabForm().AssertIsOpen();
                    break;
                case PositionCardTabs.Statistics:
                    new StatisticTabForm().AssertIsOpen();
                    break;
                case PositionCardTabs.TagsAndNotes:
                    new TagsNotesTabPositionCardForm().AssertIsOpen();
                    break;
                case PositionCardTabs.News:
                    new NewsTabForm().AssertIsOpen();
                    break;
                case PositionCardTabs.CompanyProfile:
                    new CompanyProfileTabForm().AssertIsOpen();
                    break;
                case PositionCardTabs.Financials:
                    new FinancialsTabForm().AssertIsOpen();
                    break;
                case PositionCardTabs.ChartSettings:
                    new ChartSettingsTabForm().AssertIsOpen();
                    break;
                case PositionCardTabs.Options:
                    new OptionsTabCommonForm().AssertIsOpen();
                    break;
                case PositionCardTabs.Insights:
                    new InsightsTabForm().AssertIsOpen();
                    break;
                case PositionCardTabs.CoinProfile:
                    new CoinProfileTabForm().AssertIsOpen();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(Constants.UnexpectedTabExceptionText);
            }
            new BrowserSteps().CheckBrowserConsoleForErrors(tab.GetStringMapping());
        }

        protected string HomeUrl => WebdriverFramework.Framework.WebDriver.Configuration.LoginUrl;
    }
}