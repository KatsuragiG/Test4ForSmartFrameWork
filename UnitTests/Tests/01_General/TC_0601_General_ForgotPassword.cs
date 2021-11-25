using System;
using System.Linq;
using AutomatedTests.Database.Users;
using AutomatedTests.Forms.LoginForm;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Support.UI;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.WebDriver;

namespace UnitTests.Tests._01_General
{
    [TestClass]
    public class TC_0601_General_ForgotPassword : BaseTestUnitTests
    {
        private const int TestNumber = 601;

        private string message;
        private string forgotPasswordLabel;
        private string forgotPasswordText;
        private string passwordLabel;
        private string emailLabel;

        [TestInitialize]
        public void TestInitialize()
        {
            message = GetTestDataAsString(nameof(message));
            forgotPasswordLabel = GetTestDataAsString(nameof(forgotPasswordLabel));
            forgotPasswordText = GetTestDataAsString(nameof(forgotPasswordText));
            emailLabel = GetTestDataAsString(nameof(emailLabel));
            passwordLabel = GetTestDataAsString(nameof(passwordLabel));

            LogStep(0, "Precondition - Create a Premium user");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPremium));
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_601$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("LoginLogout"), TestCategory("Smoke")]
        [Description("The test checks correctness work of Forgot Password function. https://tr.a1qa.com/index.php?/cases/view/19232528")]
        public override void RunTest()
        {
            LogStep(1, "Open Login screen");
            var loginForm = new LoginForm();
            loginForm.AssertIsOpen();

            LogStep(2, "Click link 'Forgot Password?'");
            loginForm.ClickForgotPasswordLink();
            var resetPasswordForm = new ResetPasswordForm();
            Asserts.Batch(
                () =>
                    Assert.AreEqual(forgotPasswordText, resetPasswordForm.GetForgorPasswordText(), "Forgot Password Text is unexpected"),
                () =>
                    Assert.AreEqual(forgotPasswordLabel, resetPasswordForm.GetForgorPasswordLabel(), "Forgot Password Label is unexpected")
            );

            LogStep(3, "Click Back link");
            resetPasswordForm.ClickBack();
            loginForm.AssertIsOpen();
            Asserts.Batch(
                () =>
                    Assert.IsTrue(loginForm.IsEmailLoginFieldPresent(), "Fill in field Email/Login does not present on the page"),
                () =>
                    Assert.AreEqual(string.Empty, loginForm.GetValidationErrorMessage(), "Error about login password flow is shown"),
                () =>
                    Assert.AreEqual(emailLabel, loginForm.GetEmailLabel(), "Error about login password flow is shown"),
                () =>
                    Assert.AreEqual(passwordLabel, loginForm.GetPasswordLabel(), "Error about login password flow is shown"),
                () =>
                    Assert.IsTrue(loginForm.IsPasswordFieldPresent(), "Fill in field Password does not present on the page")
            );

            LogStep(4, "Click link 'Forgot Password?'");
            loginForm.ClickForgotPasswordLink();

            LogStep(5, "Enter any e-mail which *isn't exist on DB* (like test@gmail.com). Click Send button");
            var email = $"{SRandom.Instance.Next()}@gmail.com";
            resetPasswordForm.EnterEmailForgotPasswordAndClickSend(email);

            Asserts.Batch(
                () =>
                    Assert.AreEqual(message, loginForm.GetForgotPasswordMessage(), $"Text {message} is not shown on page"),
                () =>
                    Assert.IsTrue(loginForm.IsEmailLoginFieldPresent(), "Fill in field Email/Login does not present on the page"),
                () =>
                    Assert.IsTrue(loginForm.IsPasswordFieldPresent(), "Fill in field Password does not present on the page")
                );

            LogStep(6, "Make sure that record isn't present on DB");
            var userQueries = new UsersQueries();
            var tempPasswordsDb = userQueries.SelectTempPasswordData();
            var expectedTradeSmithUserId = userQueries.SelectTradeSmithUserFromMasterDBByUserEmail(UserModels.First().Email).TradeSmithUserId;
            var userSetups = new UserSetups();
            Assert.IsFalse(userSetups.GetTradeSmithUserIdsFromTempPasswordModelsList(tempPasswordsDb).Contains(expectedTradeSmithUserId),
                $"There is record with email which corresponded step 3 {expectedTradeSmithUserId}.");

            LogStep(7, 8, "Click link Forgot Password? -> Enter any e-mail *which exist* on DB. Click Send button");
            loginForm.ClickForgotPasswordLink();
            resetPasswordForm.EnterEmailForgotPasswordAndClickSend(UserModels.First().Email);

            Asserts.Batch(
                () =>
                    Assert.AreEqual(message, loginForm.GetForgotPasswordMessage(), $"Text {message} is not shown on page"),
                () =>
                    Assert.IsTrue(loginForm.IsEmailLoginFieldPresent(), "Fill in field Email/Login does not present on the page"),
                () =>
                    Assert.IsTrue(loginForm.IsPasswordFieldPresent(), "Fill in field Password does not present on the page")
                );

            LogStep(9, 10, "Make sure that record is present on DB; Make sure that value on Active column = 1");
            WaitTempPasswordDataForTradeSmithUserId(expectedTradeSmithUserId);
            tempPasswordsDb = userQueries.SelectTempPasswordData();

            Asserts.Batch(
                () =>
                    Assert.IsTrue(userSetups.GetTradeSmithUserIdsFromTempPasswordModelsList(tempPasswordsDb).Contains(expectedTradeSmithUserId),
                    $"There is no record with TradeSmithUserId which corresponded step 6 {expectedTradeSmithUserId}."),
                () =>
                    Assert.IsTrue(userSetups.GetActiveColumnValueInTempPasswordModelsListByTradeSmithUserId(tempPasswordsDb, expectedTradeSmithUserId),
                    "Value on Active column is not 1")
                );
        }

        public static void WaitTempPasswordDataForTradeSmithUserId(string expectedTradeSmithUserId)
        {
            new WebDriverWait(Browser.Instance.GetDriver(), TimeSpan.FromMilliseconds(Parsing.ConvertToInt(Configuration.LoginRequestTimeOut)))
                .Until(d => new UserSetups().GetTradeSmithUserIdsFromTempPasswordModelsList(new UsersQueries().SelectTempPasswordData()).Contains(expectedTradeSmithUserId));
        }
    }
}