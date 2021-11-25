using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.PositionData;
using AutomatedTests.Database.Positions;
using AutomatedTests.Database.Users;
using AutomatedTests.Enums.PortfolioLite;
using AutomatedTests.Enums;
using AutomatedTests.Forms.PortfolioLite;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.PortfolioLiteSteps;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System;
using TradeStops.Common.Enums;
using TradeStops.Common.Utils;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._10_PortfolioLite
{
    [TestClass]
    public class TC_1395_PortfolioLite_Analyzer_TabsContainExpectedData : BaseTestUnitTests
    {
        private const int TestNumber = 1395;

        private List<string> statisticSectionsTitles;
        private readonly PositionsQueries positionsQueries = new PositionsQueries();
        private readonly PositionDataQueries positionDataQueries = new PositionDataQueries();

        private int symbolId;
        private bool isCorporateShown;
        private string yearAgoDate;
        private string symbol;
        private string corporateDescription;
        private string statisticsDescription;
        private string currencySign;

        [TestInitialize]
        public void TestInitialize()
        {
            symbol = GetTestDataAsString(nameof(symbol));
            symbolId = Parsing.ConvertToInt(positionsQueries.SelectSymbolIdNameUsingSymbol(symbol).SymbolId);
            currencySign = ((Currency)positionsQueries.SelectSymbolCurrencyBySymbol(symbol)).GetDescription();

            isCorporateShown = GetTestDataAsBool(nameof(isCorporateShown));
            corporateDescription = GetTestDataAsString(nameof(corporateDescription));
            statisticsDescription = string.Format(GetTestDataAsString(nameof(statisticsDescription)),
                Parsing.ConvertToShortDateString(positionDataQueries.SelectLastTradeDate(symbol)));
            statisticSectionsTitles = GetTestDataValuesAsListByColumnName(nameof(statisticSectionsTitles));
            yearAgoDate = DateTimeProvider.GetDate(DateTime.Now, 0, 0, -1).AsShortDate();

            LogStep(0, "Preconditions. Create user with subscription to PortfolioLite");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.PortfolioLite));
            var fakeSnaid = StringUtility.RandomString(Constants.SnaidPattern);
            new UsersQueries().UpdateUserSnaid(fakeSnaid, UserModels.First().TradeSmithUserId);
            new PortfolioLiteNavigation().OpenPortfolioLiteWithUserGuid(UserModels.First());
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1395$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("PortfolioLite"), TestCategory("StockAnalyzer")]
        [Description("Test checks that tabs contain expected data. https://tr.a1qa.com/index.php?/cases/view/22045841")]
        public override void RunTest()
        {
            LogStep(1, "Select ticker in search field");
            var portfolioLiteMainForm = new PortfolioLiteMainForm();
            portfolioLiteMainForm.SearhSymbol(symbol);
            var portfolioLiteAnalyzerForm = new PortfolioLiteAnalyzerForm();
            portfolioLiteAnalyzerForm.AssertIsOpen();
            Checker.CheckEquals(symbol, portfolioLiteAnalyzerForm.GetSymbol(),
                "Symbol is not expected");

            LogStep(2, "Check that three tabs are available");
            foreach (var tab in EnumUtils.GetValues<PortfolioLiteAnalyzerTabs>())
            {
                Checker.IsTrue(portfolioLiteAnalyzerForm.IsTabPresent(tab), $"{tab} tab is not present");
            }

            LogStep(3, "Click Chart tab");
            var portfolioLiteChartTabForm = portfolioLiteAnalyzerForm.ActivateTabAndGetForm<PortfolioLiteChartTabForm>(PortfolioLiteAnalyzerTabs.Chart);
            portfolioLiteChartTabForm.AssertIsOpen();

            LogStep(4, "Check that Chart tab contains expected data");
            var portfolioLiteSteps = new PortfolioLiteSteps();
            portfolioLiteSteps.CheckChartTabData(portfolioLiteAnalyzerForm.Chart);

            LogStep(5, "Click Statistics tab and check sections headers");
            var statisticTabForm = portfolioLiteAnalyzerForm.ActivateTabAndGetForm<StatisticTabForm>(PortfolioLiteAnalyzerTabs.Statistics);
            statisticTabForm.AssertIsOpen();
            var tabTitles = statisticTabForm.GetSectionTitles();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(statisticSectionsTitles, tabTitles),
                $"Statistics tab headers are not as expected: {GetExpectedResultsString(statisticSectionsTitles)}\r\n{ GetActualResultsString(tabTitles)}");

            LogStep(6, "Check that Statistics tab contains expected Corporate data");
            portfolioLiteSteps.CheckCorporateActionsData(statisticTabForm, yearAgoDate, symbolId, corporateDescription, isCorporateShown);

            LogStep(7, "Check that Statistics tab contains expected general statistics data");
            portfolioLiteSteps.CheckGeneralStatisticData(statisticTabForm, symbol, currencySign, portfolioLiteAnalyzerForm.GetLatestClose(), statisticsDescription);
        }
    }
}