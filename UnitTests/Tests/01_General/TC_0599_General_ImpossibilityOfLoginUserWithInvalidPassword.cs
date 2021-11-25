using AutomatedTests.Forms.LoginForm;
using AutomatedTests.Models.UserModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;


namespace UnitTests.Tests._01_General
{
    [TestClass]
    public class TC_0599_General_ImpossibilityOfLoginUserWithInvalidPassword : BaseTestUnitTests
    {
        private const int TestNumber = 599;

        private string password;
        private string validationMessage;

        [TestInitialize]
        public void TestInitialize()
        {
            password = GetTestDataAsString(nameof(password));
            validationMessage = GetTestDataAsString(nameof(validationMessage));

            LogStep(0, "Precondition - Create a Premium user");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPremium));
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_599$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("LoginLogout")]
        [Description("The test checks impossibility of logged in with invalid password via login page. https://tr.a1qa.com/index.php?/cases/view/19232529")]
        public override void RunTest()
        {
            LogStep(1, "Open iFrame Login screen.");
            new MainMenuNavigation().OpenLogin();

            LogStep(2, "Enter Username and Password 1 with password.Click 'Log in' button.");
            var loginForm = new LoginForm();
            loginForm.LogInWithoutDbWaiting(new UserModel { Email = UserModels.First().Email, Password = password });
            Asserts.Batch(
                () =>
                    Assert.AreEqual(validationMessage, loginForm.GetValidationErrorMessage(), "Error text is not shown on the page"),
                () =>
                    Assert.IsTrue(loginForm.IsClickHereLinkPresentAndEnabled(), "'Click here' is not a link. Link is not clickable.")
                );

            LogStep(3, "Click the 'Click here' link");
            loginForm.ClickHere();
            new ResetPasswordForm().AssertIsOpen();
        }
    }
}