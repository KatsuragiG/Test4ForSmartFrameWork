using AutomatedTests;
using AutomatedTests.Forms.Dashboard;
using AutomatedTests.Forms.ExpiredSubscription;
using AutomatedTests.Forms.LoginForm;
using AutomatedTests.Models.UserModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.BrowserSteps;
using AutomatedTests.Steps.Settings;
using AutomatedTests.Steps.ThirdPartyResourcesSteps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._01_General
{
    [TestClass]
    public class TC_1362_General_UserWithoutSubscriptionCanLoginAndLogoutAsExpected : BaseTestUnitTests
    {
        private UserModel userModel;
        private string pageHeader;
        private string pageWording;

        [TestInitialize]
        public void TestInitialize()
        {
            userModel = new UserModel
            {
                Email = GetTestDataAsString("SaUser"),
                Password = GetTestDataAsString("SaUserPassword")
            };
            pageHeader = GetTestDataAsString("PageHeader");
            pageWording = GetTestDataAsString("PageWording");
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1362$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("LoginLogout")]
        [Description("The test checks S&A user Without Subsciption can be logged in, can not open pages and can logout. https://tr.a1qa.com/index.php?/cases/view/19232987")]
        public override void RunTest()
        {
            LogStep(1, "Login");
            LoginSetUp.LogIn(userModel);

            LogStep(2, "Check that expired page is shown");
            var expiredSubscriptionForm = new ExpiredSubscriptionForm();
            expiredSubscriptionForm.AssertIsOpen();

            LogStep(3, "Check page look");
            Checker.CheckEquals(pageHeader, expiredSubscriptionForm.GetPageHeader(),  "Page Header is not as expected");
            Checker.CheckEquals(pageWording, expiredSubscriptionForm.GetPageWording(),  "Page Wording is not as expected");
            Checker.IsTrue(expiredSubscriptionForm.IsImageExpiredPresent(), "Page does not contain image for expired page");

            LogStep(4, "Click Renew");
            expiredSubscriptionForm.ClickRenewAccount();
            new ThirdPartyResourcesSteps().AssertBlogFormAndCloseNewTabAndSwitchToLastTabByHandle(Browser.GetDriver().CurrentWindowHandle);

            LogStep(5, "Clear cookies");
            new SettingsSteps().LogoutClearCookiesRefresh(userModel.Email);

            LogStep(6, "Login as User with subscription");
            var userWithSubscription = new CustomTestDataReader().GetSpareAccount();
            LoginSetUp.LogIn(userWithSubscription);
            var dashboardForm = new DashboardForm();
            dashboardForm.AssertIsOpen();

            LogStep(7, "Clear cookies and page refresh. Check that login form is shown");
            var browserSteps = new BrowserSteps();
            browserSteps.ClearAllDomainCookiesClearStorages();
            var mainMenuNavigation = new MainMenuNavigation();
            mainMenuNavigation.OpenSsoHealthCheckPingForm();
            browserSteps.ClearAllDomainCookiesClearStorages();
            mainMenuNavigation.OpenDashboard();
            new LoginForm().AssertIsOpen();

            LogStep(8, "Login as User with subscription");
            LoginSetUp.LogIn(userWithSubscription);
            dashboardForm.AssertIsOpen();
        }
    }
}