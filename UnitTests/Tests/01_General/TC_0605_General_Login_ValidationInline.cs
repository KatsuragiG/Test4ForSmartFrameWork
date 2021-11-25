using AutomatedTests.Forms.LoginForm;
using AutomatedTests.Navigation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._01_General
{
    [TestClass]
    public class TC_0605_General_Login_ValidationInline : BaseTestUnitTests
    {
        private List<string> validationMessages = new List<string>();
        private string validEmail;
        private string validPassword;
        private string invalidChars;
        private string validChars;

        [TestInitialize]
        public void TestInitialize()
        {
            validationMessages = GetTestDataValuesAsListByColumnName(nameof(validationMessages));
            validEmail = GetTestDataAsString(nameof(validEmail));
            validPassword = GetTestDataAsString(nameof(validPassword));
            invalidChars = GetTestDataAsString(nameof(invalidChars));
            validChars = GetTestDataAsString(nameof(validChars));
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_605$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("LoginLogout"), TestCategory("Smoke")]
        [Description("The test checks correctness of validation inline for iFrame login page. https://tr.a1qa.com/index.php?/cases/view/19232527")]
        public override void RunTest()
        {
            LogStep(1, "Open iFrame Login screen.");
            new MainMenuNavigation().OpenLogin();

            LogStep(2, "Do not enter data on fields:- 'Email/Login:'- 'Password:' fields.Click 'Log in' button. ");
            var loginForm = new LoginForm();
            loginForm.ClickLogin();
            Checker.CheckEquals(validationMessages[0], loginForm.GetInlineValidationMessageEmail(),
                $"Red warning message '{validationMessages[0]}' did not appear under 'Email/Login:' field.");
            Checker.CheckEquals(validationMessages[1], loginForm.GetInlineValidationMessagePassword(), 
                $"Red warning message '{validationMessages[1]}' did not appear under 'Password:' field.");

            LogStep(3, "Enter any e-mail and make sure inline validation for 'Email/Login:' field disappears.");
            loginForm.SetEmail(validEmail);
            var unexpectedValidation = loginForm.GetInlineValidationMessageEmail();
            Checker.IsTrue(string.IsNullOrEmpty(unexpectedValidation), $"Red warning message appeared under 'Email/Login:' field : {unexpectedValidation} .");

            LogStep(4, "Enter any password and make sure inline validation for 'Password:' field disappears.");
            loginForm.SetPassword(validPassword);
            unexpectedValidation = loginForm.GetInlineValidationMessagePassword();
            Checker.IsTrue(unexpectedValidation == string.Empty, $"Red warning message appeared under 'Password:' field: {unexpectedValidation} .");

            LogStep(5, "Clear field 'Email/Login:' and make sure inline validation for 'Email/Login:' field appears.");
            loginForm.SetEmail("");
            Checker.CheckEquals(validationMessages[0], loginForm.GetInlineValidationMessageEmail(),
                $"Red warning message '{validationMessages[0]}' did not appear under 'Email/Login:' field.");

            LogStep(6, "Clear field 'Password:' and make sure inline validation for 'Password:' field appears.");
            loginForm.SetPassword("");
            Checker.CheckEquals(validationMessages[1], loginForm.GetInlineValidationMessagePassword(),
                $"Red warning message '{validationMessages[1]}' did not appear under 'Password:' field.");

            LogStep(7, "Enter 2 characters for fields:- 'Email/Login:'- 'Password:'Make sure inline validation is correct.");
            loginForm.SetEmail(invalidChars);
            loginForm.SetPassword(invalidChars);
            Checker.CheckEquals(validationMessages[2], loginForm.GetInlineValidationMessageEmail(),
                $"Red warning message '{validationMessages[2]}' did not appear under 'Email/Login:' field.");
            Checker.CheckEquals(validationMessages[2], loginForm.GetInlineValidationMessagePassword(),
                $"Red warning message '{validationMessages[2]}' did not appear under 'Password:' field.");

            LogStep(8, "Enter 4 characters for fields:- 'Email/Login:';- 'Password:'.Make sure inline validation disappears.");
            loginForm.SetEmail(validChars);
            loginForm.SetPassword(validChars);
            Checker.IsTrue(loginForm.GetInlineValidationMessageEmail() == string.Empty, "Red warning message appeared under 'Email/Login:' field.");
            Checker.IsTrue(loginForm.GetInlineValidationMessagePassword() == string.Empty, "Red warning message appeared under 'Password:' field.");
        }
    }
}