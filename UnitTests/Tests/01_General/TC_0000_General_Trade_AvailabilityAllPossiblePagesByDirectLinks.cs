using System.Collections.Generic;
using System.Linq;
using AutomatedTests.Database.Users;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Opportunities;
using AutomatedTests.Forms;
using AutomatedTests.Forms.OpportunitiesForm;
using AutomatedTests.Forms.Screener;
using AutomatedTests.Forms.Trade;
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
    public class TC_0000_General_Trade_AvailabilityAllPossiblePagesByDirectLinks : BaseTestUnitTests
    {
        private readonly List<string> DbParamTradeSmith = new List<string>
        {
            nameof(UserFeaturesCustomizationsModel.OptionChain),
            nameof(UserFeaturesCustomizationsModel.PopCalculator),
            nameof(UserFeaturesCustomizationsModel.CoPilotCompanion)
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
        [Description("The test checks all possible pages availability and opening using direct links https://tr.a1qa.com/index.php?/cases/view/22957655")]
        [TestMethod]
        [TestCategory("Smoke")]
        public override void RunTest()
        {
            LogStep(1, "Check Option Screener");
            var mainMenuNavigation = new MainMenuNavigation();
            var browserSteps = new BrowserSteps();
            mainMenuNavigation.OpenSavedOptionScreeners();
            var savedOptionScreenersForm = new SavedOptionScreenersForm();
            savedOptionScreenersForm.AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors("Option Saved Screener");

            savedOptionScreenersForm.ClickScreenerByName(PreSavedOptionScreenerTypes.TradeSmithSellCalls.GetStringMapping());
            new OptionScreenerForm().AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors("Option Screener Form");

            LogStep(2, "Check Copilot Companion");
            mainMenuNavigation.OpenCoPilot();
            var coPilotTradableAssetsForm = new CoPilotWatchlistForm();
            coPilotTradableAssetsForm.AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(coPilotTradableAssetsForm.GetFormTitle());

            LogStep(3, "Check Option Opportunities");
            mainMenuNavigation.OpenOptionOpportunities();
            new OpportunitiesOptionsForm().AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors("Option Opportunities");

            LogStep(4, "Check Pop calculator");
            mainMenuNavigation.OpenPopCalculator();
            new PopCalculatorForm().AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors("Pop Calculator");

            LogStep(5, "Check Monthly Opportunities");
            mainMenuNavigation.OpenMonthlyOpportunities();
            new MonthlyOpportunitiesForm().AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors("Monthly Opportunities");
        }
    }
}