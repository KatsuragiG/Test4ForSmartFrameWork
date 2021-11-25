using AutomatedTests;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.SyncFlow;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;
using WebdriverFramework.Framework.WebDriver;

namespace UnitTests.Tests._03_MyPortfolios._02_SynchPortfoliosImport
{
    [TestClass]
    public class TC_0002_SynchPortfoliosImport_ImportPortfolioFromBrokerage : BaseTestUnitTests
    {
        private const int TestNumber = 2;
        private const int CharQuantityForTyping = 5;
        private const int PortfolioQuantityInDagAccount = 1;
        private const int ImportProgressBarsQuantityAfterImport = 1;

        private CustomTestDataReader reader;

        [TestInitialize]
        public void TestInitialize()
        {
            reader = new CustomTestDataReader();

            LogStep(0, "Preconditions: Login");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPlus));

            LoginSetUp.LogIn(UserModels.First());

            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PortfoliosPage"), TestCategory("SyncPortfolio"), TestCategory("SyncPortfolioImport")]
        [Description("Test checks importing process for broker without MFA https://tr.a1qa.com/index.php?/cases/view/20745889")]
        public override void RunTest()
        {
            LogStep(1, "Remember numbers of existed portfolios and progress bars");
            var portfoliosForm = new PortfoliosForm();
            portfoliosForm.ClickPortfolioTypeTab(PortfolioType.Investment);
            var portfoliosQuantityBefore = portfoliosForm.GetNumberOfRowsInPortfoliosGrid();
            var importBarsQuantityBefore = portfoliosForm.GetNumberOfImportProcessLines();

            LogStep(2, "Click on Add Portfolio -> Link to Brokerage");
            portfoliosForm.ClickAddPortfolio();
            new SelectPortfolioFlowForm().SelectPortfolioCreationFlowWithoutSwitchingToYodlee(AddPortfolioTypes.Synchronized);
            try
            {
                new PlaidImportForm().AssertIsOpen();
            }
            catch (AssertFailedException e)
            {
                Logger.Instance.Warn($"Plaid popup is not shown: '{e}'");
            }

            LogStep(3, "Open https://qa-auto.finance.tradesmith.com/sync-flow/alternative-import by direct URL");
            new MainMenuNavigation().OpenSyncFlowYodleeImport();
            var syncFlowImportForm = new SyncFlowImportForm();

            LogStep(4, 5, "Enter in Broker name field text for appropriate Broker. Select broker from autocomplete");
            Assert.IsTrue(syncFlowImportForm.IsBrokerAutocompletePresent(), "Broker autocomplete is not shown");
            var brokerName = reader.GetBrokerAccount().BrokerFullName;
            syncFlowImportForm.SelectBroker(brokerName.Substring(0, CharQuantityForTyping), brokerName);

            LogStep(6, "Type Broker credentials");
            var brokerLogin = reader.GetBrokerAccount().Account;
            var brokerPass = reader.GetBrokerAccount().Password;
            syncFlowImportForm.SetAccount(brokerLogin);
            Checker.CheckContains(brokerLogin, syncFlowImportForm.GetLogin(), "Login is not shown for Broker");
            syncFlowImportForm.SetPassword(brokerPass);
            syncFlowImportForm.ClickOnShowPassword();
            Checker.CheckContains(brokerPass, syncFlowImportForm.GetPassword(), "Password is not shown for Broker");

            LogStep(7, "Check that default agreement statement is non-agree (not marked)");
            Checker.IsFalse(syncFlowImportForm.GetAgreeTermsBoxState(), "Default agreement statement is agree");
            Checker.IsFalse(syncFlowImportForm.IsSyncBrokerageButtonEnabled(), "Sync Brokerage button is active");

            LogStep(8, "Check that default agreement statement is non-agree (not marked)");
            syncFlowImportForm.SetAgreeTermsBoxInState(true);
            Checker.IsTrue(syncFlowImportForm.GetAgreeTermsBoxState(), "Default agreement statement is NOT agree");
            Checker.IsTrue(syncFlowImportForm.IsSyncBrokerageButtonEnabled(), "Sync Brokerage button is NOT active");

            LogStep(9, "Click 'Sync Brokerage'");
            syncFlowImportForm.ClickSyncBrokerage();
            Checker.IsTrue(syncFlowImportForm.IsImportStepsPresent(), "Import Steps is NOT shown");
            Checker.IsTrue(syncFlowImportForm.IsCurrentImportProgressBarPresent(), "Import Progress Bar is NOT shown");
            syncFlowImportForm.WaitUntilSuccessfulImportMessagePresent();

            LogStep(10, "Click Next");
            syncFlowImportForm.ClickNextIfPresent();
            portfoliosForm.AssertIsOpen();
            Checker.CheckEquals(portfoliosQuantityBefore + PortfolioQuantityInDagAccount, portfoliosForm.GetNumberOfRowsInPortfoliosGrid(), 
                "Count of Portfolios was not changed");
            Checker.CheckEquals(importBarsQuantityBefore + ImportProgressBarsQuantityAfterImport, portfoliosForm.GetNumberOfImportProcessLines(), 
                "Count of Import Progress Bars was not changed");

            LogStep(11, "Check that Last Import process bar has correct name, date and status");
            var progressBar = portfoliosForm.GetInformationOfImportBarByNumber(ImportProgressBarsQuantityAfterImport);
            Assert.IsNotNull(progressBar, "Progress bars are empty");
            Checker.CheckContains(brokerName, progressBar[PortfolioProgressBarNotification.Name], "Name of Broker in Progress bar is not as expected");
            Checker.CheckEquals(Parsing.ConvertToShortDateString(DateTime.Now.ToShortDateString()), 
                progressBar[PortfolioProgressBarNotification.Date], "Date of Import in Progress bar is not as expected");
            Checker.CheckEquals(AutoTestsImportProcessStatusTypes.Success.GetStringMapping(),
                progressBar[PortfolioProgressBarNotification.Status], "Status of Import in Progress bar is not as expected");
        }
    }
}