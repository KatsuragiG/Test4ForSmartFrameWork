using System;
using System.Linq;
using AutomatedTests;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._03_MyPortfolios._02_SynchPortfoliosImport
{
    [TestClass]
    public class TC_0116_SynchPortfoliosImport_ImportPortfolioWithToken : BaseTestUnitTests
    {
        private const int TestNumber = 116;

        private CustomTestDataReader reader;
        private int portfolioId;
        private string currentDate;
        private int countOfProgressBars;
        private int countOfPortfolios;
        private int countOfProgressBarsAfterImport;
        private int countOfPortfoliosAfterImport;

        [TestInitialize]
        public void TestInitialize()
        {
            reader = new CustomTestDataReader();
            currentDate = Parsing.ConvertToShortDateString(DateTime.Now.ToShortDateString());

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPlus));
            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PortfoliosPage"), TestCategory("SyncPortfolio"), TestCategory("SyncPortfolioImport")]
        [Description("https://tr.a1qa.com/index.php?/cases/view/19232892 Test checks importing process for broker with MFA (token)")]
        public override void RunTest()
        {
            LogStep(1, "Remember numbers of existed portfolios");            
            var portfolioGridsSteps = new PortfolioGridsSteps();
            portfolioGridsSteps.GetCountOfPortfoliosProgressBars(out countOfPortfolios, out countOfProgressBars);

            LogStep(2, 8, "Import portfolio with custom credentials");
            portfolioGridsSteps.ImportInvestmentPortfolioUsingCusmomCredentialsWithSecurityKey(reader.GetBrokerAccountToken().BrokerFullName, 
                reader.GetBrokerAccountToken().Account01, reader.GetBrokerAccountToken().Password01, reader.GetBrokerAccountToken().SecurityKey, true);
            var portfoliosForm = new PortfoliosForm();

            LogStep(9, "Check imported portfolio data");
            portfolioId = portfoliosForm.GetLastImportedPortfolioId(UserModels.First().Email);
            portfolioGridsSteps.GetCountOfPortfoliosProgressBars(out countOfPortfoliosAfterImport, out countOfProgressBarsAfterImport);
            var importProgressBar = portfoliosForm.GetInformationOfImportBarByNumber(countOfProgressBarsAfterImport);

            Asserts.Batch(
                    () =>
                    Assert.IsTrue(countOfPortfoliosAfterImport == countOfPortfolios + 1, $"Portfolio with id {portfolioId} is not On the grid"),
                    () =>
                    Assert.IsTrue(countOfProgressBarsAfterImport == countOfProgressBars + 1, "Progress bar for last imported portfolio is not on the grid"),
                    () =>
                    Assert.AreEqual(importProgressBar[PortfolioProgressBarNotification.Name], reader.GetBrokerAccountToken().BrokerFullName, "Brokers names are not equals"),
                    () =>
                    Assert.AreEqual(importProgressBar[PortfolioProgressBarNotification.Status], AutoTestsImportProcessStatusTypes.Success.GetStringMapping(), "Expected status of Progress Bar is not equals with current status"),
                    () =>
                    Assert.AreEqual(importProgressBar[PortfolioProgressBarNotification.Date], currentDate, "Date is not the same as current date")
                );
        }
    }
}