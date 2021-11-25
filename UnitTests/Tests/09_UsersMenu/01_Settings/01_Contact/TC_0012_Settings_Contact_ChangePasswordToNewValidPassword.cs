using AutomatedTests.Enums.Settings;
using AutomatedTests.Enums.User;
using AutomatedTests.Forms.LoginForm;
using AutomatedTests.Forms.Settings.Contact;
using AutomatedTests.Forms.Settings;
using AutomatedTests.Forms;
using AutomatedTests.Models.UserModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using AutomatedTests.Forms.ThirdPartyResourcesForms;

namespace UnitTests.Tests._09_UsersMenu._01_Settings._01_Contact
{
    [TestClass]
    public class TC_0012_Settings_Contact_ChangePasswordToNewValidPassword : BaseTestUnitTests
    {
        private const int TestNumber = 12;

        private UserModel userWithNewPassword;
        private UserModel userExternal;
        private ContactModel userNewPassword;
        private string newPassword;
        private string validationMessage;
        private string urlSA;

        [TestInitialize]
        public void TestInitialize()
        {
            var userType = GetTestDataParsedAsEnumFromStringMapping<ProductSubscriptionTypes>("user");

            newPassword = GetTestDataAsString(nameof(newPassword));
            validationMessage = GetTestDataAsString(nameof(validationMessage));
            userNewPassword = new ContactModel
            {
                Password = newPassword,
                ConfirmPassword = GetTestDataAsString("ConfirmNewPassword")
            };
            userExternal = new UserModel
            {
                Email = GetTestDataAsString("emailSandA"),
                Password = GetTestDataAsString("passwordSandA")
            };
            urlSA = GetTestDataAsString(nameof(urlSA));

            LogStep(0, 2, "Preconditions. Go to Settings Change Password");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, (ProductSubscriptions)(int)userType));
            userWithNewPassword = new UserModel
            {
                Email = UserModels.First().Email,
                Password = newPassword
            };
            new SettingsSteps().LoginNavigateToSettingsContactFillContactFields(UserModels.First(), userNewPassword);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_12$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test possibility to change contact information. https://tr.a1qa.com/index.php?/cases/view/20339808")]
        [TestCategory("Smoke"), TestCategory("SettingsPage"), TestCategory("SettingsPageContactTab")]
        public override void RunTest()
        {
            LogStep(1, "Type new valid password for TSP user and click Save");
            new SettingsMainForm().SaveSettings();
            Assert.IsTrue(new SettingsMainForm().IsSuccessSavedMessagePresent(), "Success message is not present");

            LogStep(2, "Log Out");
            TearDowns.LogOut();
            var loginForm = new LoginForm();
            loginForm.AssertIsOpen();

            LogStep(3, "Try to login with old password");
            loginForm.LogInWithoutDbWaiting(UserModels.First());
            Checker.CheckEquals(validationMessage, loginForm.GetValidationErrorMessage(), "Login with old password seems to be workable");

            LogStep(4, "Login with new password");
            LoginSetUp.LogIn(userWithNewPassword);
            new MainMenuForm().AssertIsOpen();

            LogStep(5, "Logout");
            TearDowns.LogOut();
            loginForm.AssertIsOpen();

            LogStep(6, "Login as S&A user");
            LoginSetUp.LogIn(userExternal);
            new MainMenuForm().AssertIsOpen();

            LogStep(7, "Open Settings -> Contact page");
            new MainMenuNavigation().OpenSettings();
            var contactForm = new ContactForm();
            Checker.IsFalse(contactForm.IsValueTextBoxShown(ContactFieldTypes.Password), "Password field is shown for S&A user");
            Checker.IsFalse(contactForm.IsValueTextBoxShown(ContactFieldTypes.ConfirmPassword), "Password field is shown for S&A user");
            contactForm.ClickManagePassword();
            var driver = Browser.GetDriver();
            var newTabWindowHandle = driver.WindowHandles.Last();
            driver.SwitchTo().Window(newTabWindowHandle);
            new StansberryResetPasswordFrom().AssertIsOpen();
            Checker.CheckEquals(urlSA, driver.Url, "Manage password page has unexpexted URL");
        }
    }
}
