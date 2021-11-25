using AutomatedTests.Forms.ThirdPartyResourcesForms;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._01_General
{
    [TestClass]
    public class TC_1361_General_AvailabilityOfPingHealthCheck : BaseTestUnitTests
    {

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [TestMethod]
        [TestCategory("Admin")]
        [Description("The test checks ping health check answer. https://tr.a1qa.com/index.php?/cases/view/19232986")]
        public override void RunTest()
        {
            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(1361, ProductSubscriptions.TradeStopsLifetime));

            LogStep(1, "Transition to *Sso/api/healthcheck/ping");
            var mainMenuNavigation = new MainMenuNavigation();
            mainMenuNavigation.OpenSsoHealthCheckPingForm();
            var healthCheckForm = new HealthCheckForm();
            healthCheckForm.AssertIsOpen();
            mainMenuNavigation.OpenLogin();

            LogStep(2, "Transition to *Home/api/healthcheck/ping");
            mainMenuNavigation.OpenHealthCheckPingForm();
            healthCheckForm.AssertIsOpen();
            mainMenuNavigation.OpenLogin();

            LogStep(3, "Transition to *Home/api/healthcheck/ping");
            mainMenuNavigation.OpenAdminHealthCheckPingForm();
            healthCheckForm.AssertIsOpen();
            mainMenuNavigation.OpenLogin();
        }
    }
}