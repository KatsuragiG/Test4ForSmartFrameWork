using System;
using System.Collections.Generic;
using System.Linq;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Positions;
using AutomatedTests.Database.Users;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Tools;
using AutomatedTests.Forms;
using AutomatedTests.Forms.BackTester;
using AutomatedTests.Forms.OpportunitiesForm;
using AutomatedTests.Forms.ResearchPages.PositionSizeForm;
using AutomatedTests.Forms.ResearchPages.PureQuant;
using AutomatedTests.Forms.Screener;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.BrowserSteps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using static WebdriverFramework.Framework.Util.EnumExtensions;
using DescriptionAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;

namespace UnitTests.Tests._01_General
{
    [TestClass]
    public class TC_0000_General_Invest_AvailabilityAllPossiblePagesByDirectLinks : BaseTestUnitTests
    {
        private const int CryptoCoinSymbolId = 26567722;
        private readonly List<string> DbParamTradeSmith = new List<string>
        {
            nameof(UserFeaturesCustomizationsModel.Backtester)
        };
        private const string DbValueForPrecondition = "1";

        [TestInitialize]
        public void TestInitialize()
        {
            LogStep(0, "Precondition - Login as registered user");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(0, ProductSubscriptions.TradeStopsPlatinum));

            var userModel = UserModels.First();
            var usersQueries = new UsersQueries();
            foreach (var dbParamTradeSmith in DbParamTradeSmith)
            {
                usersQueries.UpdateUserTradesmithFeaturesCustomizations(userModel.TradeSmithUserId, dbParamTradeSmith, DbValueForPrecondition);
            }

            LoginSetUp.LogIn(userModel);
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [Description("The test checks all possible pages availability and opening using direct links https://tr.a1qa.com/index.php?/cases/view/19235574")]
        [TestMethod]
        [TestCategory("Smoke")]
        public override void RunTest()
        {
            LogStep(1, "Check strategies on Opportunities pages");
            var mainMenuNavigation = new MainMenuNavigation();
            var browserSteps = new BrowserSteps();
            foreach (var strategy in Constants.AvailableStrategyTypes)
            {
                mainMenuNavigation.OpenOpportunitiesWithSelectingStrategy(strategy);
                var opportunitiesForm = new OpportunitiesForm();
                opportunitiesForm.AssertIsOpen();
                Checker.IsTrue(opportunitiesForm.IsResultGridOrNoResultMessagePresent(), $"Result grid or No Result Message are not shown for {strategy.GetStringMapping()}");
                browserSteps.CheckBrowserConsoleForErrors(strategy.GetStringMapping());
            }

            LogStep(2, "Check pureQuant internal forms");
            mainMenuNavigation.OpenPureQuantRunsGrid();
            new PureQuantRunInitForm().AssertIsOpen();
            foreach (var step in EnumUtility.GetValues<PureQuantInternalSteps>())
            {
                mainMenuNavigation.OpenPureQuant(step);
                switch (step)
                {
                    case PureQuantInternalSteps.Step1ChooseSources:
                        new PureQuantStep1Form().AssertIsOpen();
                        break;
                    case PureQuantInternalSteps.Step2ChangeDefaultSettings:
                        new PureQuantStep2Form().AssertIsOpen();
                        break;
                    case PureQuantInternalSteps.Step3CustomizePortfolio:
                        new PureQuantStep3Form().AssertIsOpen();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                browserSteps.CheckBrowserConsoleForErrors(step.GetStringMapping());
            }

            LogStep(3, "Check Screener");
            mainMenuNavigation.OpenSavedScreeners();
            new SavedScreenersForm().AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors($"Saved {InvestMenuItems.Screener.GetStringMapping()}");

            mainMenuNavigation.OpenScreenerWithDefaultSearch();
            new ScreenerFiltersForm().AssertIsOpen();
            new ScreenerGridForm().AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(InvestMenuItems.Screener.GetStringMapping());

            LogStep(4, "Check Position Size");
            mainMenuNavigation.OpenPositionSize();
            var newPositionSize = new PositionSizeCalculatorForm();
            newPositionSize.AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(InvestMenuItems.PositionSize.GetStringMapping());

            mainMenuNavigation.OpenPositionSizeForDefaultTicker();
            newPositionSize.AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors($"{InvestMenuItems.PositionSize.GetStringMapping()} for AAPL");

            var positionsQueries = new PositionsQueries();
            mainMenuNavigation.OpenPositionSizeForTicker(CryptoCoinSymbolId);
            newPositionSize.AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors($"{InvestMenuItems.PositionSize.GetStringMapping()} for crypto {positionsQueries.SelectSymbolBySymbolId(CryptoCoinSymbolId)}");

            LogStep(5, "Check BackTester");
            mainMenuNavigation.OpenBackTesterTasks();
            var backTesterTasksForm = new BackTesterTasksForm();
            backTesterTasksForm.AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors("BackTester tasks");

            backTesterTasksForm.ClickNewBackTester();
            new BackTesterFiltersForm().AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors("BackTester filters");
        }
    }
}