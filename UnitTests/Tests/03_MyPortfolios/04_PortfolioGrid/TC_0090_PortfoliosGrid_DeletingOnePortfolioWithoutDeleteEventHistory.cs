using AutomatedTests.Database.Portfolios;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Events;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._03_MyPortfolios._04_PortfolioGrid
{
    [TestClass]
    public class TC_0090_PortfoliosGrid_DeletingOnePortfolioWithoutDeleteEventHistory : BaseTestUnitTests
    {
        private const int TestNumber = 90;

        private int portfolioId;
        private readonly List<PositionAtManualCreatingPortfolioModel> positionsModels = new List<PositionAtManualCreatingPortfolioModel>();

        [TestInitialize]
        public void TestInitialize()
        {
            var portfolioModel = new AddPortfolioManualModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency")
            };
            positionsModels.Add(new PositionAtManualCreatingPortfolioModel
            {
                TradeType = GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>("StockType1"),
                Ticker = GetTestDataAsString("Symbol1"),
                EntryDate = GetTestDataAsString("EntryDate"),
                Quantity = GetTestDataAsString("Shares")
            });

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPro));
            LoginSetUp.LogIn(UserModels.First());
            new AddPortfoliosSteps().NavigateToAddManualPortfolioSavePortfolioAfterFillingFields(portfolioModel, positionsModels);
            portfolioId = new PortfoliosQueries().SelectPortfolioIdForLastPortfolioByUserEmail(UserModels.First().Email);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_90$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("Test checks possibility to delete portfolio without all history of corresponded events https://tr.a1qa.com/index.php?/cases/view/19232938")]
        [TestCategory("Smoke"), TestCategory("PortfoliosPage"), TestCategory("PortfoliosPageActionsPopup"), TestCategory("EventHistoryPage")]
        public override void RunTest()
        {
            LogStep(1, 4, "Go to Events");
            new EventsSteps().OpenEventsCheckAllPortfoliosForNonEmptyGridClickPortfolios(new List<int> { portfolioId });

            LogStep(5, "Click Delete button for portfolio from precondition step 1");
            new PortfoliosForm().ClickDeletePortfolioViaId(portfolioId);

            LogStep(6, 7, "Check that checkbox 'Delete event history for selected portfolios.' is unmarked, Click yes");
            Checker.IsFalse(new DeletePortfolioPopup().IsDeleteHistoryChecked(), "Default state for Delete History with portfolio is not as expected");

            LogStep(8, 9, "Check that Deleting is finished successfully. Click OK");
            new PortfolioGridsSteps().ConfirmDeletingPortfoliosCloseSuccessPopup();

            LogStep(10, 12, "Click menu in right upper corner, Click Events. Select Portfolio Name from precondition step 1 in dropdown");
            var eventsForm = new EventsSteps().OpenEventsSelectPortfolioByIdGetEventsForm(portfolioId);
            eventsForm.SelectPeriod(GridFilterPeriods.Last7Days);

            LogStep(13, "Check that table with Events is not empty");
            Assert.IsTrue(eventsForm.GetNumberOfEvents() > 0, "13 events quantity is 0");

            LogStep(14, "Check portfolio name and Delisted status for the deleted portfolio in DB");
            var portfolioData = new PortfoliosQueries().SelectPortfolioDataByPortfolioId(portfolioId);
            Assert.IsTrue(Parsing.ConvertToBool(portfolioData.Delisted), "portfolio is not delisted");
        }
    }
}