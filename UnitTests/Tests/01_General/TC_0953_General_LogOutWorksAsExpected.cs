using System.Linq;
using AutomatedTests.Enums.User;
using AutomatedTests.Forms;
using AutomatedTests.Forms.LoginForm;
using AutomatedTests.SetUpsTearDowns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;
using Configuration = WebdriverFramework.Framework.WebDriver.Configuration;

namespace UnitTests.Tests._01_General
{
    [TestClass]
    public class TC_0953_General_LogOutWorksAsExpected : BaseTestUnitTests
    {
        private const int TestNumber = 953;

        private string loginUrl;
        private string loginUrlIframe;
        private string loginUrlSelenoid;
        private string pageUrl;

        [TestInitialize]
        public void TestInitialize()
        {
            loginUrl = $"{Configuration.LoginUrl}{GetTestDataAsString("LoginUrl")}";
            var loginInframe = GetTestDataAsString("LoginUrl2");
            loginUrlIframe = $"{Configuration.LoginUrl}{string.Format(loginInframe, string.Empty)}";
            loginUrlSelenoid = $"{Configuration.LoginUrl}{string.Format(loginInframe.Replace("http://", "https://"), Configuration.LoginUrl)}".Replace("http://", "https://");
            pageUrl = $"{Configuration.LoginUrl}{GetTestDataAsString("Url")}";

            LogStep(0, "Precondition - Login as registered user");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPremium));
            LoginSetUp.LogIn(UserModels.First());
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_953$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("LoginLogout"), TestCategory("Smoke")]
        [Description("The test checks behaviour Logout function matched expectations. https://tr.a1qa.com/index.php?/cases/view/19232183")]
        public override void RunTest()
        {
            LogStep(1, "Click on area with username -> Click 'Log Out'");
            new MainMenuForm().ClickUserAccountItem(UserAccountItems.LogOut);

            LogStep(2, "Click 'Yes' Make sure there is not any browser errors.");
            new LoginForm().AssertIsOpen();
            Checker.CheckEquals($"{loginUrl}", Browser.GetDriver().Url, $"URL of basic login page is not {loginUrl}");
            var browserErrors = ErrorsCollector.GetBrowserErrors(Browser.GetDriver());
            Checker.CheckEquals(0, browserErrors.Count, $"There are browser errors after Log Out\n {browserErrors.Aggregate("", (current, error) => current + error.ToString())}");

            LogStep(3, "Enter direct link to TradeStops page.Make sure user still on login page.");
            Browser.NavigateTo(pageUrl);
            new LoginForm().AssertIsOpen();
            var currentUrl = Browser.GetDriver().Url;
            Checker.IsTrue(DecodeUrl(Browser.GetDriver().Url) == $"{loginUrlIframe}" || DecodeUrl(currentUrl) == $"{loginUrlSelenoid}", 
                $"URL of redirected login page {currentUrl} is not {loginUrlIframe} or {loginUrlSelenoid}");

            LogStep(4, "Make sure there is not any browser errors.");
            browserErrors = ErrorsCollector.GetBrowserErrors(Browser.GetDriver());
            Checker.CheckEquals(0, browserErrors.Count, $"There are browser errors after changing Url\n {browserErrors.Aggregate("", (current, error) => current + error.ToString())}\n");
        }

        private static string DecodeUrl(string url)
        {
            return url.Replace("%2f", "/").Replace("%3a", ":").Replace("%2F", "/").Replace("%3A", ":");
        }
    }
}