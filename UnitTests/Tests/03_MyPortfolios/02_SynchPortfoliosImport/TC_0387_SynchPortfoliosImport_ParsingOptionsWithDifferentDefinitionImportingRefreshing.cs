using System.Collections.Generic;
using System.Linq;
using AutomatedTests;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms;
using AutomatedTests.Forms.MyPortfolios;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._03_MyPortfolios._02_SynchPortfoliosImport
{
    [TestClass]
    public class TC_0387_SynchPortfoliosImport_ParsingOptionsWithDifferentDefinitionImportingRefreshing : BaseTestUnitTests
    {
        private const int TestNumber = 387;

        private CustomTestDataReader reader;
        private int numberOfPortfolios;
        private int numberOfPositions;
        private List<string> positionsTicker;
        private List<int> portfoliosIds;

        [TestInitialize]
        public void TestInitialize()
        {
            numberOfPositions = GetTestDataAsInt(nameof(numberOfPositions));
            numberOfPortfolios = GetTestDataAsInt(nameof(numberOfPortfolios));
            positionsTicker = GetTestDataValuesAsListByColumnName(nameof(positionsTicker));

            reader = new CustomTestDataReader();

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPremium));
            LoginSetUp.LogIn(UserModels.First());

            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_387$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PortfoliosPage"), TestCategory("SyncPortfolio"), TestCategory("SyncPortfolioImport"), TestCategory("SyncPortfolioUpdate"), TestCategory("SyncPositions")]
        [Description("Test checks Positions recognizing option with different definition at importing and refreshing portfolio; https://tr.a1qa.com/index.php?/cases/view/19232714")]
        public override void RunTest()
        {
            LogStep(1, 3, "Click on Add Portfolio. Import portfolio with Token. Close the message by clicking Close button");
            var portfolioGridStes = new PortfolioGridsSteps();
            portfolioGridStes.ImportInvestmentPortfolioUsingCusmomCredentialsWithSecurityKey(reader.GetBrokerAccountToken().BrokerFullName, reader.GetBrokerAccountToken().Account04,
                reader.GetBrokerAccountToken().Password04, reader.GetBrokerAccountToken().SecurityKey, true);
            var portfoliosData = new PortfoliosQueries().SelectPortfoliosDataByUserId(UserModels.First());

            portfoliosIds = portfoliosData.Select(t => Parsing.ConvertToInt(t.PortfolioId)).ToList();
            var positionsInPortfolios = new PortfoliosForm().GetValuesOfPortfolioColumn(PortfolioType.Investment, PortfolioGridColumnTypes.Positions)
                .Select(Parsing.ConvertToInt).ToList();

            LogStep(4, "Open Portfolios tab");
            Checker.CheckEquals(numberOfPortfolios, portfoliosIds.Count, "Unexpected portfolios quantity after import");
            Checker.CheckListsEquals(Enumerable.Repeat(numberOfPositions, numberOfPortfolios).ToList(), positionsInPortfolios, "Unexpected positions quantity on portfolio grid");

            DoSteps5To8();

            LogStep(9, "Open Portfolios tab. Click refresh for portfolio");
            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);
            portfolioGridStes.ClickRefreshPortfolioIdViaSyncFlow(portfoliosIds.First());

            LogStep(10, "Repeat steps 5-8");
            DoSteps5To8();
        }

        private void DoSteps5To8()
        {
            LogStep(5, "Open Positions & Alerts page -> Positions tab.Select Portfolio from the dropdown");
            new MyPortfoliosMenuForm().ClickMyPortfoliosMenuItem(MyPortfoliosMenuItems.PositionsGrid);
            var positionsAlertsStatisticsPanelForm = new PositionsAlertsStatisticsPanelForm();
            positionsAlertsStatisticsPanelForm.ActivateTab(PositionsTabs.OpenPositions);

            LogStep(8, "Repeat steps 6-7 for all portfolios");
            var positionsTab = new PositionsTabForm();
            var portfoliosQueries = new PortfoliosQueries();
            foreach (var portfolioId in portfoliosIds)
            {
                LogStep(6, "Check that there are follow positions in grid");
                var portfolioname = portfoliosQueries.SelectPortfolioName(portfolioId);
                positionsAlertsStatisticsPanelForm.SelectPortfolio(portfolioname);
                var allPositions = positionsTab.GetPositionColumnValues(PositionsGridDataField.Ticker).Select(t => t.Split('\r')[0]).ToList();
                Checker.IsTrue(ListsComparator.AreTwoListsEqualsInOrder(positionsTicker, allPositions),
                    $"There are no following positions in grid\n {GetExpectedResultsString(positionsTicker)}\r\n" +
                    $"{GetActualResultsString(allPositions)}");

                LogStep(7, "Check that All position has non-N/A values in Entry Date, Entry price and Shares column");
                var entryDates = positionsTab.GetPositionColumnValues(PositionsGridDataField.EntryDate);
                var entryPrice = positionsTab.GetPositionColumnValues(PositionsGridDataField.EntryPrice);
                var shares = positionsTab.GetPositionColumnValues(PositionsGridDataField.Shares);
                for (var i = 0; i < allPositions.Count; i++)
                {
                    Checker.CheckNotEquals(Constants.NotAvailableAcronym, entryDates[i], $"Entry Date is N/A for position # {i} in portfolio {portfolioname}");
                    Checker.CheckNotEquals(Constants.NotAvailableAcronym, entryPrice[i], $"Entry Price is N/A for position # {i} in portfolio {portfolioname}");
                    Checker.CheckNotEquals(Constants.NotAvailableAcronym, shares[i], $"Shares is N/A for position # {i} in portfolio {portfolioname}");
                }
            }
        }
    }
}