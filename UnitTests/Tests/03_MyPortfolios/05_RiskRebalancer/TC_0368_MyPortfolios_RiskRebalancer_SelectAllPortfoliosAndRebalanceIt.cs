using System;
using System.Collections.Generic;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Tools;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.ResearchPages.RiskRebalancer;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Settings;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;
using AutomatedTests.Enums.Positions;
using TradeStops.Common.Enums;
using System.Linq;

namespace UnitTests.Tests._03_MyPortfolios._05_RiskRebalancer
{
    [TestClass]
    public class TC_0368_MyPortfolios_RiskRebalancer_SelectAllPortfoliosAndRebalanceIt : BaseTestUnitTests
    {
        private const int TestNumber = 368;

        private readonly List<int> portfoliosIds = new List<int>();
        private readonly List<int> portfoliosManualIds = new List<int>();
        private PortfolioModel portfolioModelInvest;
        private PortfolioModel portfolioModelWatch;
        private readonly List<PositionsDBModel> stocksModels = new List<PositionsDBModel>();
        private int step = 1;

        [TestInitialize]
        public void TestInitialize()
        {
            var portfolioName = GetTestDataAsString("PortfolioName");
            var portfolioCurrency = GetTestDataAsString("Currency");
            portfolioModelInvest = new PortfolioModel
            {
                Name = StringUtility.RandomString(portfolioName),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = portfolioCurrency
            };
            portfolioModelWatch = new PortfolioModel
            {
                Name = StringUtility.RandomString(portfolioName),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType2"),
                Currency = portfolioCurrency
            };

            var positionEntryDate = GetTestDataAsString("EntryDate1");
            stocksModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("Stock1"),
                TradeType = $"{(int)PositionTradeTypes.Long}",
                PurchaseDate = positionEntryDate,
            });
            stocksModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("Stock2"),
                TradeType = $"{(int)PositionTradeTypes.Short}",
                PurchaseDate = positionEntryDate,
            });
            stocksModels.Add(new PositionsDBModel
            {
                Symbol = GetTestDataAsString("Stock3"),
                TradeType = $"{(int)PositionTradeTypes.Short}",
                PurchaseDate = positionEntryDate,
                StatusType = $"{(int)AutotestPositionStatusTypes.Close}",
                CloseDate = $"{DateTime.Now}"
            });

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, new List<ProductSubscriptions>
                {
                    ProductSubscriptions.TradeStopsLifetime, ProductSubscriptions.CryptoStopsPremium
                }
            ));

            portfoliosManualIds.Add(PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModelInvest));
            portfoliosManualIds.Add(PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModelWatch));
            PositionsAlertsSetUp.AddPositionViaDB(portfoliosManualIds[0], stocksModels[0]);
            PositionsAlertsSetUp.AddPositionViaDB(portfoliosManualIds[0], stocksModels[1]);
            PositionsAlertsSetUp.AddPositionViaDB(portfoliosManualIds[1], stocksModels[0]);
            PositionsAlertsSetUp.AddPositionViaDB(portfoliosManualIds[1], stocksModels[2]);

            LoginSetUp.LogIn(UserModels.First());
            new SettingsSteps().SetDefaultPortfolioCurrencyTo(Currency.CAD);
            var portfoliosQueries = new PortfoliosQueries();
            PortfoliosSetUp.ImportDagSiteInvestment06(true);
            portfoliosIds.Add(portfoliosQueries.SelectPortfolioIdForLastPortfolioByUserEmail(UserModels.First().Email));
            PortfoliosSetUp.ImportDagSiteInvestment03(false);
            portfoliosIds.Add(portfoliosQueries.SelectPortfolioIdForLastPortfolioByUserEmail(UserModels.First().Email));
            portfoliosIds.Add(portfoliosQueries.SelectPortfolioIdForLastPortfolioByUserEmail(UserModels.First().Email));
            new PortfoliosForm().ClickOnPortfolioNameViaId(portfoliosIds[1]);
            var positionsTab = new PositionsTabForm();
            var importedPositionsIds = positionsTab.GetPositionsIds();
            new PositionsQueries().SetPossibilityToCloseSynchPositionByPositionId(importedPositionsIds[0]);

            Browser.GetDriver().Navigate().Refresh();
            PositionsAlertsSetUp.ClosePosition(importedPositionsIds[0]);

            PortfoliosSetUp.DuplicatePortfolioAsWatchOnly(portfoliosIds[0]);
            PortfoliosSetUp.DuplicatePortfolioAsWatchOnly(portfoliosIds[1]);

            new MainMenuNavigation().OpenRiskRebalancer();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_368$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("RiskRebalancer"), TestCategory("MultiPortfolioSelect")]
        [Description("The test checks matching of statistics for All positions (in investment and in watch portfolio) with sums of statistics in " +
            "All investment and in All Watch mode https://tr.a1qa.com/index.php?/cases/view/19232682")]
        public override void RunTest()
        {
            LogStep(step++, "Select All portfolios from in the dropdown Select a portfolio");
            var riskRebalancer = new RiskRebalancerForm();
            riskRebalancer.SelectPortfolioMultipleByText(AllPortfoliosKinds.All.ToString());

            LogStep(2, 4, "Check Rebalancer tabs for result");
            DoStep2To4();

            LogStep(step++, "Select All Investment from in the dropdown Select a portfolio");
            Browser.Refresh();
            riskRebalancer.SelectPortfolioMultipleByText(AllPortfoliosKinds.AllInvestment.GetStringMapping());

            LogStep(step++, "Click Show advanced options Check check - box 'Reallocate funds from positions that are Stopped Out according to SSI'");
            riskRebalancer.SetAdvancedOptionsState(true);
            riskRebalancer.SetReallocateFundsState(true);

            LogStep(step++, "Uncheck cash check-box ");
            riskRebalancer.SetCheckboxForStatisticInState(RiskRebalancerStatisticTypes.IncludeCash, false);

            LogStep(step++, "Check Dividends check-box ");
            riskRebalancer.SetCheckboxForStatisticInState(RiskRebalancerStatisticTypes.IncludeCash, false);

            LogStep(9, 11, "Check Rebalancer tabs for result");
            DoStep2To4();

            LogStep(step++, "Select All Watch from in the dropdown Select a portfolio");
            Browser.Refresh();
            riskRebalancer.SelectPortfolioMultipleByText(AllPortfoliosKinds.AllWatch.GetStringMapping());

            LogStep(step++, "Check cash check-box ");
            riskRebalancer.SetCheckboxForStatisticInState(RiskRebalancerStatisticTypes.IncludeCash, true);

            LogStep(14, 17, "Check Rebalancer tabs for result");
            DoStep2To4();
        }

        private void DoStep2To4()
        {
            LogStep(step++, "Click Rebalance button");
            var riskRebalancer = new RiskRebalancerForm();
            riskRebalancer.ClickRebalance();
            Checker.IsTrue(riskRebalancer.IsTabPresent(RiskRebalancerTabs.RebalanceOverview), $"{RiskRebalancerTabs.RebalanceOverview} is not present");
            Checker.IsTrue(riskRebalancer.IsTabPresent(RiskRebalancerTabs.RebalancedResults), $"{RiskRebalancerTabs.RebalancedResults} is not present");
            Checker.IsTrue(riskRebalancer.IsTabPresent(RiskRebalancerTabs.ChangeInHoldings), $"{RiskRebalancerTabs.ChangeInHoldings} is not present");

            Checker.IsTrue(riskRebalancer.IsTabActive(RiskRebalancerTabs.RebalanceOverview), "rebalance overview is not active");

            Checker.IsFalse(riskRebalancer.GetRebalancedValueBySection(RiskRebalancerOverviewSectionTypes.PortfolioVolatilityQuotient).Equals(string.Empty),
                "Risk rebalancer pvq is not empty");
            Checker.IsFalse(riskRebalancer.GetRebalancedValueBySection(RiskRebalancerOverviewSectionTypes.VolatilityRiskPerPosition).Equals(string.Empty),
                "Volatility risk per position is not empty");
            Checker.IsFalse(riskRebalancer.GetRiskAllocationValuesByType(RiskRebalancerRiskAllocationsTypes.Current).Contains(string.Empty),
                "Risk allocation values is not empty");

            LogStep(step++, "Click on Rebalanced Results tab");
            riskRebalancer.SelectTab(RiskRebalancerTabs.RebalancedResults);
            var rebalancedResultsData = riskRebalancer.GetRebalancedResultRow((int)RiskRebalancerTabs.RebalancedResults);
            Checker.IsFalse(rebalancedResultsData.Ticker == null, "Rebalanced results grid is empty");

            LogStep(step++, "Click Steps to take tab");
            riskRebalancer.SelectTab(RiskRebalancerTabs.ChangeInHoldings);
            var valuesOfStepsToTakeGrid = riskRebalancer.GetChangeInHoldingsRow((int)RiskRebalancerTabs.RebalancedResults);
            Checker.IsFalse(valuesOfStepsToTakeGrid.Ticker == null, "Steps to take grid is empty");
        }
    }
}