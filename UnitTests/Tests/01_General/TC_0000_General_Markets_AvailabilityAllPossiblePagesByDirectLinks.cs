using AutomatedTests.Enums.Markets;
using AutomatedTests.Enums;
using AutomatedTests.Forms.Markets;
using AutomatedTests.Forms;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.BrowserSteps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._01_General
{
    [TestClass]
    public class TC_0000_General_Markets_AvailabilityAllPossiblePagesByDirectLinks : BaseTestUnitTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            LogStep(0, "Precondition - Login as registered user");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(0, ProductSubscriptions.TradeStopsPlatinum));

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [Description("The test checks all possible pages availability and opening using direct links https://tr.a1qa.com/index.php?/cases/view/19235573")]
        [TestMethod]
        [TestCategory("Smoke")]
        public override void RunTest()
        {
            LogStep(1, "Check Market Outlook tab. No errors in console.");
            var mainMenuNavigation = new MainMenuNavigation();
            mainMenuNavigation.OpenMarketOutlook();
            var marketsMenuForm = new MarketsHealthCommonForm();
            marketsMenuForm.AssertIsOpen();
            Checker.IsTrue(marketsMenuForm.IsMarketsTabSelected(MarketsMenuItems.MarketOutlook), $"Tab {MarketsMenuItems.MarketOutlook} is not active");
            var browserSteps = new BrowserSteps();
            browserSteps.CheckBrowserConsoleForErrors(MarketsMenuItems.MarketOutlook.GetStringMapping());

            LogStep(2, "Check S&P Sectors tab. No errors in console");
            mainMenuNavigation.OpenSpSectors();
            marketsMenuForm.AssertIsOpen();
            Checker.IsTrue(marketsMenuForm.IsMarketsTabSelected(MarketsMenuItems.SandPSectors), $"Tab {MarketsMenuItems.SandPSectors} is not active");
            browserSteps.CheckBrowserConsoleForErrors(MarketsMenuItems.SandPSectors.GetStringMapping());

            LogStep(3, "Check Commodities tab is opened. No errors in console");
            mainMenuNavigation.OpenCommodities();
            marketsMenuForm.AssertIsOpen();
            Checker.IsTrue(marketsMenuForm.IsMarketsTabSelected(MarketsMenuItems.Commodities), $"Tab {MarketsMenuItems.Commodities} is not active");
            browserSteps.CheckBrowserConsoleForErrors(MarketsMenuItems.Commodities.GetStringMapping());

            LogStep(4, "Check Crypto Market Outlook tab is opened. No errors in console");
            mainMenuNavigation.OpenCryptoMarketOutlook();
            marketsMenuForm.AssertIsOpen();
            Checker.IsTrue(marketsMenuForm.IsMarketsTabSelected(MarketsMenuItems.Crypto), $"Tab {MarketsMenuItems.Crypto} is not active");
            browserSteps.CheckBrowserConsoleForErrors(MarketsMenuItems.Crypto.GetStringMapping());
        }
    }
}