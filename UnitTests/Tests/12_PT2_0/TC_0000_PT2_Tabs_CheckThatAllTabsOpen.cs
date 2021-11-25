using AutomatedTests.ConstantVariables;
using AutomatedTests.Enums.Portfolios.CreateManual;
using AutomatedTests.Enums.PositionCard;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Forms.Charts;
using AutomatedTests.Forms.PortfolioTracker;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Forms;
using AutomatedTests.Models.PT2;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.BrowserSteps;
using AutomatedTests.Utils;
using AutomatedTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._12_PT2_0
{
    [TestClass]
    public class TC_0000_PT2_Tabs_CheckThatAllTabsOpen : BaseTestUnitTests
    {
        private const int TestNumber = 0;
        private const int NumberRowPosition = 1;
        private readonly List<PositionCardTabs> tabsOnPositionCardForStock = EnumExtensions.EnumUtility.GetValues<PositionCardTabs>()
            .Except(new List<PositionCardTabs> 
            { 
                PositionCardTabs.Insights, 
                PositionCardTabs.Alerts, 
                PositionCardTabs.TagsAndNotes, 
                PositionCardTabs.Options, 
                PositionCardTabs.CoinProfile 
            }).ToList();

        private PtPortfolioModel portfolioModel;
        private PtPositionAtCreatingPortfolioModel positionsModel;

        [TestInitialize]
        public void TestInitialize()
        {
            portfolioModel = new PtPortfolioModel
            {
                Name = StringUtility.RandomStringOfSize(Constants.DefaultSizeOfDateStringToClearField)
            };

            positionsModel = new PtPositionAtCreatingPortfolioModel
            {
                Ticker = new CustomTestDataReader().GetDefaultTicker(),
                EntryDate = DateTime.Now.AddYears(-3).ToString(Constants.ShortDateFormat),
                Quantity = NumberRowPosition.ToString(),
                TradeType = PositionTradeTypes.Long,
                PositionAssetType = PositionAssetTypes.Stock
            };

            LogStep(0, "Precondition - Login as user with subscription to Portfolio Tracker");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, new List<ProductSubscriptions> {
                ProductSubscriptions.TradeStopsLifetime,
                ProductSubscriptions.TestOrganization
            }));

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [Description("Check that created user in TS with subscription on Portfolio Tracker (TradeSmith Organization) has access to all tabs " +
            "in Pubs https://tr.a1qa.com/index.php?/cases/view/21494846")]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PT20")]
        public override void RunTest()
        {
            LogStep(1, "Check that Add Portfolio form is shown");
            var mainMenuNavigation = new MainMenuNavigation();
            mainMenuNavigation.OpenPtAddPortfolioForm();
            var portfolioCreationForm = new PtPortfolioCreationForm();
            portfolioCreationForm.AssertIsOpen();
            var browserSteps = new BrowserSteps();
            browserSteps.CheckBrowserConsoleForErrors(portfolioCreationForm.GetFormTitle());

            LogStep(2, "Fill portfolio and positions field and click Save");
            portfolioCreationForm.FillPortfolioFields(portfolioModel);
            portfolioCreationForm.FillPositionFields(positionsModel, NumberRowPosition);
            portfolioCreationForm.ClickPortfolioActionsButton(PortfolioManualFlowActionsButton.SavePortfolio);

            LogStep(3, "Check that Pubs tab is shown");
            var manageForm = new ManageForm();
            manageForm.AssertIsOpen();            
            browserSteps.CheckBrowserConsoleForErrors(manageForm.GetFormTitle());
            var portfoliosIds = manageForm.GetPortfoliosIds();

            LogStep(4, "Check that Position tab is shown");
            mainMenuNavigation.OpenPtPositionsGrid();
            var ptPositionsTabForm = new PtPositionsTabForm();
            ptPositionsTabForm.AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(ptPositionsTabForm.GetFormTitle());
            var positionId = ptPositionsTabForm.GetPositionIdFromGridByLineNumber(NumberRowPosition);

            LogStep(5, "Check that Alert tab is shown");
            mainMenuNavigation.OpenPtAlertsGrid();
            var ptAlertsTabForm = new PtAlertsTabForm();
            ptAlertsTabForm.AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(ptAlertsTabForm.GetFormTitle());

            LogStep(6, "Check that Add Position form is shown");
            mainMenuNavigation.OpenPtAddPositionPage();
            var ptAddPositionForm = new PtAddPositionForm();
            ptAddPositionForm.AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(ptAddPositionForm.GetFormTitle());

            LogStep(7, "Position card (AAPL)");
            mainMenuNavigation.OpenPtPositionCard(positionId);
            foreach (var tab in tabsOnPositionCardForStock)
            {
                CheckPositionCardTab(tab);
            }

            LogStep(8, "Check Publish Portfolio Form");
            mainMenuNavigation.OpenPtPublishForm(portfoliosIds.First());
            var ptPublishPortfolioForm = new PtPublishPortfolioForm();
            ptPublishPortfolioForm.AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(ptPublishPortfolioForm.GetFormTitle());

            LogStep(9, "Check Website Portfolio Form");
            mainMenuNavigation.OpenPtWebsitesForm(portfoliosIds.First());
            var ptWebsitesForm = new PtWebsitesForm();
            ptWebsitesForm.AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(ptWebsitesForm.GetFormTitle());

            LogStep(10, "Check Website Portfolio Form");
            ptWebsitesForm.ClickAddWebsite();
            var ptAddWebsiteWidgetForm = new PtAddWebsiteWidgetForm();
            ptAddWebsiteWidgetForm.AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(ptAddWebsiteWidgetForm.GetFormTitle());

            LogStep(11, "Fill required fields and click Generate. Check  Website Get Code Form");
            ptAddWebsiteWidgetForm.FillRequiredFieldsClickGenerate();
            var ptWebsitesGetCodeForm = new PtWebsitesGetCodeForm();
            ptWebsitesGetCodeForm.AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(ptWebsitesGetCodeForm.GetFormTitle());

            LogStep(12, "Check Website Portfolio Form");
            mainMenuNavigation.OpenPtWebsitesForm(portfoliosIds.First());
            ptWebsitesForm.AssertIsOpen();

            TearDowns.DeletePtPortfolios(UserModels.First(), portfoliosIds.First());
        }

        private static void CheckPositionCardTab(PositionCardTabs tab)
        {
            new PtPositionCardForm().ActivateTabWithoutChartWaiting(tab);
            switch (tab)
            {
                case PositionCardTabs.PositionDetails:
                    new PositionDetailsTabPositionCardForm().AssertIsOpen();
                    break;
                case PositionCardTabs.Performance:
                    new PerformanceTabPositionCardForm().AssertIsOpen();
                    break;
                case PositionCardTabs.MyOpportunities:
                    new MyOpportunitiesTabForm().AssertIsOpen();
                    break;
                case PositionCardTabs.Statistics:
                    new StatisticTabForm().AssertIsOpen();
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
                default:
                    throw new ArgumentOutOfRangeException(Constants.UnexpectedTabExceptionText);
            }
            new BrowserSteps().CheckBrowserConsoleForErrors(tab.GetStringMapping());
        }
    }
}