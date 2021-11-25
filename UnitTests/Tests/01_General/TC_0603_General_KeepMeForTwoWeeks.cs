using AutomatedTests.Enums;
using AutomatedTests.Forms;
using AutomatedTests.Enums.User;
using AutomatedTests.Forms.LoginForm;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Linq;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.WebDriver;
using AutomatedTests.Forms.ThirdPartyResourcesForms;
using System;

namespace UnitTests.Tests._01_General
{
    [TestClass]
    public class TC_0603_General_KeepMeForTwoWeeks : BaseTestUnitTests
    {
        private string cookieName;
        private const int TestNumber = 603;
        private const int MaxAttemptQuantity = 3;
        private const int DaysQuantityToKeepMeCookie = 14;

        [TestInitialize]
        public void TestInitialize()
        {
            cookieName = TestContext.DataRow["Cookie"].ToString();

            LogStep(0, "Precondition - Login as registered user");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPremium));
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_603$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("LoginLogout"), TestCategory("Smoke")]
        [Description("The test checks correctness behaviour for 'Keep me for two weeks' function on login page. https://tr.a1qa.com/index.php?/cases/view/19232525")]
        public override void RunTest()
        {
            LogStep(1, 2, "Enter data of existed user (precondition #1): 'Email/Login:' 'Password:' fields. Make sure 'Keep me logged in for 2 weeks' check - box is not checked.");
            var loginForm = new LoginForm();
            loginForm.SetEmail(UserModels.First().Email);
            loginForm.SetPassword(UserModels.First().Password);
            Checker.IsFalse(loginForm.IsKeepMeLoggedInForTwoWeeksChecked(), "Keep Me Logged In For Two Weeks is  Checked");

            LogStep(3, "Click Log In. Make sure expiration date for cookies 'SSO_AuthCookie_1' is correct and equal to 'Session'.");
            loginForm.ClickLogin();
            var mainMenuForm = new MainMenuForm();
            mainMenuForm.ClickMenuItem(MainMenuItems.Dashboard);

            LogStep(4, "In new browser tab open https://qa-auto.sso.tradesmith.com/ " +
                "Make sure expiration date for cookies 'SSO_AuthCookie_1' is correct and equal to 'Session'.");
            var currentDriver = Browser.GetDriver();
            var firstWindow = currentDriver.CurrentWindowHandle;
            var mainMenuNavigation = new MainMenuNavigation();
            mainMenuNavigation.OpenUrlInNewTab($"{AutomatedTests.Configuration.GetSsoUrl}/api/healthcheck/ping");
            try
            {
                new WebDriverWait(Browser.GetDriver(), Timeouts.CookieTimeout()).Until(
                    b => Browser.GetDriver().Manage().Cookies.GetCookieNamed(cookieName) != null);
                Checker.IsTrue(Browser.GetDriver().Manage().Cookies.GetCookieNamed(cookieName).Expiry == null, 
                    $"Expiration date for cookie '{cookieName}' is not 'Session'.");
            }
            catch (WebDriverTimeoutException ex)
            {
                Logger.Instance.Info($"Cookie does not received: {ex}");
            }

            Cookie twoWeekCookie;
            var attempt = 0;
            do
            {
                LogStep(5, "Close tab with https://qa-auto.sso.tradesmith.com/ link.");
                currentDriver.Manage().Cookies.DeleteAllCookies();
                currentDriver.Close();
                currentDriver.SwitchTo().Window(firstWindow);

                LogStep(6, "Click on area with username -> Click 'Log Out'");
                mainMenuForm.ClickUserAccountItem(UserAccountItems.LogOut);
                loginForm.AssertIsOpen();

                LogStep(7, "Enter data of existed user (precondition #1): 'Email/Login:' 'Password:' fields. Check the 'Keep me logged in for 2 weeks' check - box.");
                loginForm.SetEmail(UserModels.First().Email);
                loginForm.SetPassword(UserModels.First().Password);
                loginForm.ClickKeepMeLoggeInForTwoWeeks();

                LogStep(8, "Click 'Log In' button and make sure expiration date for cookie '.ASPXAUTH' is correct.");
                loginForm.ClickLogin();
                mainMenuForm = new MainMenuForm();
                mainMenuForm.ClickMenuItem(MainMenuItems.Dashboard);

                LogStep(9, "Make sure expiration date for cookies 'SSO_AuthCookie_1' is today + 14 days");
                mainMenuNavigation.OpenUrlInNewTab($"{AutomatedTests.Configuration.GetSsoUrl}/api/healthcheck/ping");
                new HealthCheckForm().AssertIsOpen();
                twoWeekCookie = currentDriver.Manage().Cookies.GetCookieNamed(cookieName);
                attempt++;
            } while (twoWeekCookie != null && attempt < MaxAttemptQuantity);
            Checker.IsFalse(twoWeekCookie == null, "Keep me 2 week cookie is NOT available");
            Checker.CheckEquals(Parsing.ConvertToShortDateString(DateTime.Now.AddDays(DaysQuantityToKeepMeCookie).ToShortDateString()), 
                Parsing.ConvertToShortDateString(twoWeekCookie.Expiry.ToString()), "Keep me 2 week cookie has wrong Date for expiration");
        }
    }
}