using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.SyncFlow;
using AutomatedTests.Forms;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests.Utils;
using AutomatedTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._03_MyPortfolios._02_SynchPortfoliosImport
{
    [TestClass]
    public class TC_0145_SynchPortfoliosImport_AuthentificationProcessAtImportingPortfolioWithMultilevelMFA : BaseTestUnitTests
    {
        private const int TestNumber = 145;

        private CustomTestDataReader reader;
        private int countOfProgressBars;
        private int countOfPortfolios;

        [TestInitialize]
        public void TestInitialize()
        {
            reader = new CustomTestDataReader();

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPremium));

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_145$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PortfoliosPage"), TestCategory("SyncPortfolio"), TestCategory("SyncPortfolioImport")]
        [Description("Test checks Authentication Process for broker with multilevel MFA with failing after passing broker's authentication https://tr.a1qa.com/index.php?/cases/view/19232871")]
        public override void RunTest()
        {
            LogStep(1, "Remember numbers of existed portfolios and progress bars");
            new PortfolioGridsSteps().GetCountOfPortfoliosProgressBars(out countOfPortfolios, out countOfProgressBars);

            LogStep(2, "Open Fast Link by direct URL");
            new MainMenuNavigation().OpenSyncFlowFastLinkImport();

            LogStep(3, 4, "Enter in Broker name field text for appropriate Broker; Select broker from autocomplete");
            var fastLinkImportForm = new FastLinkImportForm();
            fastLinkImportForm.ClickGetStarted();
            fastLinkImportForm.SelectBroker(reader.GetBrokerAccountMultilevel().BrokerFullName);

            LogStep(5, 7, "Type Broker credentials. Enter Security Question in field and click Submit");
            fastLinkImportForm.SetLoginPassword(reader.GetBrokerAccountMultilevel().Account01, reader.GetBrokerAccountMultilevel().Password01);
            fastLinkImportForm.ClickSubmit();

            fastLinkImportForm.SetSecureKey(reader.GetBrokerAccountMultilevel().SecurityAnswer01);
            fastLinkImportForm.ClickSubmit();

            LogStep(8, 9, "Enter Security Questions in field and click Submit");
            fastLinkImportForm.EnterFirstSecureQuestion(reader.GetBrokerAccountMultilevel().SecurityAnswer02);
            fastLinkImportForm.EnterSecondSecureQuestionAndSubmit(reader.GetBrokerAccountMultilevel().SecurityAnswer03);
            fastLinkImportForm.WaitUntilSuccessfulImportMessagePresent();

            LogStep(10, "Click Next");
            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);

            LogStep(9, 10, "After delay popup about failed import is appeared; Close the message by clicking Close button");
            var portfoliosForm = new PortfoliosForm();
            var numberOfProgressBarsAfterImport = portfoliosForm.GetNumberOfImportProcessLines();

            Asserts.Batch(
                () =>
                Assert.AreEqual(countOfPortfolios, portfoliosForm.GetCountOfPortfolios(AllPortfoliosKinds.Investment), "Portfolio grid is loaded *with* new portfolio in list"),
                () =>
                Assert.AreEqual(numberOfProgressBarsAfterImport, countOfProgressBars, "In bottom of Import process rows new progress bar is not appeared")
                );
        }
    }
}