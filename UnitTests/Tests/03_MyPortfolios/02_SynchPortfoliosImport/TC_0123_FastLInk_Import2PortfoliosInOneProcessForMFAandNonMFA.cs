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
using System.Collections.Generic;
using System.Linq;
using System;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._03_MyPortfolios._02_SynchPortfoliosImport
{
    [TestClass]
    public class TC_0123_FastLink_Import2PortfoliosInOneProcessForMfaAndNonMfa : BaseTestUnitTests
    {
        private const int TestNumber = 116;

        private CustomTestDataReader reader;
        private readonly List<string> importedPortfoliosIdsNonMfa = new List<string>();
        private readonly List<string> importedPortfoliosIdsMfa = new List<string>();
        private string expectedStatus;
        private string currentDate;
        private string descriptionBlockText;
        private string searchBrokerText;
        private string brokerNameToClick;
        private string searchBrokerMfaText;
        private string brokerNameMfaToClick;
        private int numberOfPortfolios;
        private int numberOfProgressBars;
        private int numberOfPortfoliosAfterImport;
        private int numberOfProgressBarsAfterImport;
        private int brokerCardsQuantity;
        private int portfoliosQuantity;

        [TestInitialize]
        public void TestInitialize()
        {
            reader = new CustomTestDataReader();
            expectedStatus = GetTestDataAsString(nameof(expectedStatus));
            currentDate = Parsing.ConvertToShortDateString(DateTime.Now.ToShortDateString());
            descriptionBlockText = GetTestDataAsString(nameof(descriptionBlockText));
            searchBrokerText = GetTestDataAsString(nameof(searchBrokerText));
            brokerNameToClick = GetTestDataAsString(nameof(brokerNameToClick));
            searchBrokerMfaText = GetTestDataAsString(nameof(searchBrokerMfaText));
            brokerNameMfaToClick = GetTestDataAsString(nameof(brokerNameMfaToClick));            
            brokerCardsQuantity = GetTestDataAsInt(nameof(brokerCardsQuantity));
            portfoliosQuantity = GetTestDataAsInt(nameof(portfoliosQuantity));

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsLifetime));
            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_123$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("PortfoliosPage"), TestCategory("SyncPortfolio"), TestCategory("SyncPortfolioImport")]
        [Description("Test checks importing process 2 portfolios in one process from FastLink for broker with MFA (token) and non-MFA https://tr.a1qa.com/index.php?/cases/view/19232891")]
        public override void RunTest()
        {
            LogStep(1, "Remember numbers of existed portfolios");
            var portfolioGridsSteps = new PortfolioGridsSteps();
            portfolioGridsSteps.GetCountOfPortfoliosProgressBars(out numberOfPortfolios, out numberOfProgressBars);
            var portfoliosForm = new PortfoliosForm();
            var alreadyExistedPortfolios = portfoliosForm.GetPortfoliosData(PortfolioType.Investment);

            LogStep(2, "Open Fast Link widget");
            new MainMenuNavigation().OpenSyncFlowFastLinkImport();
            var fastLinkImportForm = new FastLinkImportForm();

            LogStep(3, "Click Get Started");
            Checker.IsTrue(fastLinkImportForm.IsGetStartedPresent(), "Get Started button is not shown");
            var actualText = string.Join(string.Empty, fastLinkImportForm.GetDescriptionBlockText().ToArray()).Replace("\r\n", "");
            Checker.CheckContains(descriptionBlockText, actualText, "Description Block Text is not as expected");
            fastLinkImportForm.ClickGetStarted();

            LogStep(4, "Check Broker Card Page");
            Checker.IsTrue(fastLinkImportForm.IsPrivacyPolicyExists(), "Privacy Policy is not shown");
            Checker.IsTrue(fastLinkImportForm.IsBrokerAutocompletePresent(), "Broker Autocomplete is not shown");
            Checker.CheckEquals(brokerCardsQuantity, fastLinkImportForm.GetAllAvailableBrokers().Count, "Brokers card quantity is not as expected");

            LogStep(5, "Type in search field short name for broker and check that credentials fields are shown");
            fastLinkImportForm.SelectBroker(searchBrokerText, brokerNameToClick);
            Checker.IsTrue(fastLinkImportForm.IsCredentialsFiledsShown(), "Credentials fields are not shown");
            Checker.IsTrue(fastLinkImportForm.IsResetPasswordExists(), "Reset Password link is not shown");
            Checker.IsTrue(fastLinkImportForm.IsTermsOfUseExists(), "Terms Of Use link is not shown");
            Checker.IsTrue(fastLinkImportForm.IsSubmitButtonPresent(), "Submit Button is not shown");

            LogStep(6, "Type credentials");
            var expectedLogin = reader.GetBrokerAccount().Account05;
            var expectedPassword = reader.GetBrokerAccount().Password05;
            fastLinkImportForm.SetLoginPassword(expectedLogin, expectedPassword);
            Checker.CheckEquals(expectedLogin, fastLinkImportForm.GetLogin(), "Login is not as expected");
            Checker.IsFalse(string.IsNullOrEmpty(fastLinkImportForm.GetPassword()), "Password is empty");

            LogStep(7, "Click Show Password");
            fastLinkImportForm.ClickOnShowPassword();
            Checker.CheckEquals(expectedPassword, fastLinkImportForm.GetPassword(), "Password is not as expected");

            LogStep(8, "Click Submit");
            fastLinkImportForm.ClickSubmitAndWaitImportFinishing();
            portfoliosForm.AssertIsOpen();

            LogStep(9, "Check number of imported portfolios");
            importedPortfoliosIdsNonMfa.AddRange(portfoliosForm.FindCreatedPortfolioUsingUi(alreadyExistedPortfolios));
            CheckInfoAfterImportAndGetNumberOfPortfoliosAfterImport(numberOfPortfolios, numberOfProgressBars, brokerNameToClick);

            LogStep(10, "Update existed portfolio information");
            portfolioGridsSteps.GetCountOfPortfoliosProgressBars(out numberOfPortfoliosAfterImport, out numberOfProgressBarsAfterImport);
            var alreadyExistedPortfolios2 = portfoliosForm.GetPortfoliosData(PortfolioType.Investment);

            LogStep(11, "Import the Dag Site TokenPMPS - Investments portfolio with Account02 and Password02 as creds");
            portfolioGridsSteps.ImportFastLinkPortfolioUsingCusmomCredentialsWithSecurityKey(brokerNameMfaToClick, 
                reader.GetBrokerAccountToken().Account02, reader.GetBrokerAccountToken().Password02, reader.GetBrokerAccountToken().SecurityKey);

            LogStep(12, "Check number of imported MFA portfolios");
            importedPortfoliosIdsMfa.AddRange(portfoliosForm.FindCreatedPortfolioUsingUi(alreadyExistedPortfolios2));
            CheckInfoAfterImportAndGetNumberOfPortfoliosAfterImport(numberOfPortfoliosAfterImport, numberOfProgressBarsAfterImport, brokerNameMfaToClick);

            LogStep(13, "Check that new 4 portfolios has non-zero values in Positions column");
            CheckImportedPortfolios(importedPortfoliosIdsNonMfa);
            CheckImportedPortfolios(importedPortfoliosIdsMfa);
        }

        private void CheckImportedPortfolios(IEnumerable<string> importedPortfoliosIds)
        {
            var portfolioGridsSteps = new PortfolioGridsSteps();
            foreach (var importedPortfolioId in importedPortfoliosIds)
            {
                Checker.IsTrue(portfolioGridsSteps.GetNumberOfPositionsViaPortfolioId(Parsing.ConvertToInt(importedPortfolioId)) > 0,
                    $"Portfolio with id {importedPortfolioId} has zero value in Positions column");
            }
        }

        private void CheckInfoAfterImportAndGetNumberOfPortfoliosAfterImport(int numberOfPortfolios, int numberOfProgressBars, string brokerName)
        {
            var portfoliosForm = new PortfoliosForm();
            var countOfPortfoliosAfterImport = portfoliosForm.GetCountOfPortfolios(AllPortfoliosKinds.All);
            var countOfProgressBarsAfterImport = portfoliosForm.GetNumberOfImportProcessLines();
            var importProgressBar = portfoliosForm.GetInformationOfImportBarByNumber(countOfProgressBarsAfterImport - numberOfProgressBars);

            Asserts.Batch(
                () =>
                    Assert.IsTrue(countOfPortfoliosAfterImport == numberOfPortfolios + portfoliosQuantity, 
                        "Portfolio grid is not loaded with new portfolios in list"),
                () =>
                    Assert.IsTrue(countOfProgressBarsAfterImport == numberOfProgressBars + 1, 
                        "In bottom of Import process rows not only one progress bar is appeared"),
                () =>
                    Assert.AreEqual(importProgressBar[PortfolioProgressBarNotification.Name], brokerName, 
                        "Brokers names are not equals"),
                () =>
                    Assert.AreEqual(importProgressBar[PortfolioProgressBarNotification.Status], expectedStatus, 
                        "Expected status of Progress Bar is not equals with current status"),
                () =>
                    Assert.AreEqual(importProgressBar[PortfolioProgressBarNotification.Date], currentDate, 
                        "Date is not the same as current date")
            );
        }
    }
}