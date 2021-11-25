using System;
using System.Collections.Generic;
using System.Linq;
using AutomatedTests.Database.Currency;
using AutomatedTests.Database.Dividends;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Database.Users;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.User;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Models.PositionsModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._04_PositionsAndAlerts._03_PortfolioSummary
{
    [TestClass]
    public class TC_1047_PortfolioSummary_StatisticsForPortfolioWithDifferentPositionsCurrenciesMatchesTheExpectation : BaseTestUnitTests
    {
        private const int TestNumber = 1047;
        private const int DaysShiftForEntryDate = -1;

        private readonly List<PortfolioModel> portfoliosModels = new List<PortfolioModel>();
        private readonly List<string> exchangesIds = new List<string>();
        private readonly List<string> limitsOfDividendsQuantity = new List<string>();
        private readonly List<string> limitsOfDividendsDate = new List<string>();
        private List<int> currenciesIds;
        private readonly List<CrossCourseModel> positionsWithCrossCourses = new List<CrossCourseModel>();
        private string viewNameForAddedView;
        private bool isLongTradeType;
        private bool adjustAlertsByDividends;
        private Currency userDefaultCurrency;
        private int numberOfPositions;
        private string shares;

        [TestInitialize]
        public void TestInitialize()
        {
            var userType = GetTestDataParsedAsEnumFromStringMapping<ProductSubscriptionTypes>("User");
            userDefaultCurrency = GetTestDataParsedAsEnumFromStringMapping<Currency>("UserCurrency");
            var portfolioName = GetTestDataAsString("PortfolioName");
            portfoliosModels.Add(new PortfolioModel
            {
                Name = StringUtility.RandomString(portfolioName),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType1"),
                Currency = GetTestDataAsString("PortfolioCurrency1")
            });
            portfoliosModels.Add(new PortfolioModel
            {
                Name = StringUtility.RandomString(portfolioName),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType2"),
                Currency = GetTestDataAsString("PortfolioCurrency2")
            });
            numberOfPositions = GetTestDataAsInt("PositionQuantityAll");

            limitsOfDividendsQuantity.Add(GetTestDataAsString("LowerLimitofDividendQuantityStep4"));
            limitsOfDividendsQuantity.Add(GetTestDataAsString("LowerLimitofDividendQuantityStep5"));
            limitsOfDividendsDate.Add(GetTestDataAsString("LowerLimitOfFirstDividendDateStep4"));
            limitsOfDividendsDate.Add(GetTestDataAsString("LowerLimitOfFirstDividendDateStep5"));
            exchangesIds.Add(GetTestDataAsString("SystemCategoryIdsStep4"));
            exchangesIds.Add(GetTestDataAsString("SystemCategoryIdsStep5"));

            isLongTradeType = GetTestDataAsBool("TradeType");
            adjustAlertsByDividends = GetTestDataAsBool("AdjustAlertsByDividends");
            shares = GetTestDataAsString("Shares");
            currenciesIds = GetTestDataValuesAsListByColumnName("CurrencyId").Select(Parsing.ConvertToInt).ToList();
            viewNameForAddedView = StringUtility.RandomString("$$$$$$$###");

            MakePreconditions(userType);
        }

        private void MakePreconditions(ProductSubscriptionTypes userType)
        {
            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, (ProductSubscriptions)(int)userType));

            new UsersQueries().SetDefaultCurrencyForUser((int)userDefaultCurrency, UserModels.First().TradeSmithUserId);
            var portfoliosIds = portfoliosModels.Select(portfolioModel => PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel)).ToList();

            var dividendQueries = new DividendsQueries();
            var dividendsModels = new List<List<DividendsQuantitySumModel>>();
            for (int i = 0; i < exchangesIds.Count; i++)
            {
                dividendsModels.Add(dividendQueries.SelectSeveralPositionsWithDividendsDefinedCurrencyWithDividendLimitationsWithoutSplit(numberOfPositions,
                exchangesIds[i], limitsOfDividendsDate[i], limitsOfDividendsQuantity[i], currenciesIds[i]));
            }
            LoginSetUp.LogIn(UserModels.First());

            var currencyQueries = new CurrencyQueries();
            var portfoliosQueries = new PortfoliosQueries();
            for (int i = 0; i < limitsOfDividendsDate.Count; i++)
            {
                var currentCrossCourseModels = dividendsModels[i].Select(model => new CrossCourseModel
                {
                    PositionId = PositionsAlertsSetUp.AddPositionFromAdvancedForm(portfoliosIds[i], new AddPositionAdvancedModel
                    {
                        Ticker = new PositionsQueries().SelectSymbolBySymbolId(model.SymbolId),
                        IsLongTradeType = isLongTradeType,
                        IsAdjustByDividends = adjustAlertsByDividends,
                        EntryDate = DateTime.Parse(limitsOfDividendsDate[i]).AddDays(DaysShiftForEntryDate).ToShortDateString(),
                        Shares = shares,
                        Portfolio = portfoliosQueries.SelectPortfolioName(portfoliosIds[i])
                    }),
                    EntryCrossCoursePositionToPortfolio = currencyQueries.SelectEntryCrossCourse(DateTime.Parse(limitsOfDividendsDate[i]).AddDays(DaysShiftForEntryDate).ToShortDateString(),
                         $"{(Currency)currenciesIds[i]}{portfoliosModels[i].Currency}"),
                    EntryCrossCoursePortfolioToSystem = currencyQueries.SelectEntryCrossCourse(DateTime.Parse(limitsOfDividendsDate[i]).AddDays(DaysShiftForEntryDate).ToShortDateString(),
                         $"{portfoliosModels[i].Currency}{userDefaultCurrency}"),
                    ExitCrossCoursePositionToPortfolio = currencyQueries.SelectExitCrossCourse($"{(Currency)currenciesIds[i]}{portfoliosModels[i].Currency}"),
                    ExitCrossCoursePortfolioToSystem = currencyQueries.SelectExitCrossCourse($"{portfoliosModels[i].Currency}{userDefaultCurrency}"),
                    EntryCrossCoursePositionToSystem = currencyQueries.SelectEntryCrossCourse(DateTime.Parse(limitsOfDividendsDate[i]).AddDays(DaysShiftForEntryDate).ToShortDateString(),
                         $"{(Currency)currenciesIds[i]}{userDefaultCurrency}"),
                    ExitCrossCoursePositionToSystem = currencyQueries.SelectExitCrossCourse($"{(Currency)currenciesIds[i]}{userDefaultCurrency}")
                })
                .ToList();
                positionsWithCrossCourses.AddRange(currentCrossCourseModels);
            }

            new MainMenuNavigation().OpenPositionsGrid();
            new PositionsAlertsStatisticsPanelForm().SelectPortfolio(AllPortfoliosKinds.All.GetStringMapping());
            new PositionsTabForm().AddNewViewWithAllCheckboxesMarked(viewNameForAddedView);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1047$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PositionsGrid"), TestCategory("PortfolioSummary")]
        [Description("The test checks that Statistics for portfolio with different positions currencies matches the expectation. https://tr.a1qa.com/index.php?/cases/view/19232077")]
        public override void RunTest()
        {
            LogStep(1, "Remember value for Total C/B, Total Dividends, Total Dividends, Value, Daily Gain Total, Gain ex Div, Gain w/ Div, Entry Price ");
            var positionsTabForm = new PositionsTabForm();
            var positionsInformation = positionsWithCrossCourses.Select(positionWithCrossCources =>
                positionsTabForm.GetPositionDataByPositionId(new List<PositionsGridDataField>
                    {
                        PositionsGridDataField.TotalDividends,
                        PositionsGridDataField.CostBasis,
                        PositionsGridDataField.Value,
                        PositionsGridDataField.DailyGainTotal,
                        PositionsGridDataField.GainExDiv,
                        PositionsGridDataField.GainWithDiv
                    }, positionWithCrossCources.PositionId)).ToList();
            var positionsAlertsStatisticsPanelForm = new PositionsAlertsStatisticsPanelForm();
            positionsAlertsStatisticsPanelForm.ClickPortfolioSummary();

            LogStep(2, "Check that Value in wrapDetailedPortfolioStats block is calculated as Sum " +
                "(Position Value * ExitCrossCoursePositionToPortfolio * ExitCrossCoursePortfolioToSystem)");
            Checker.CheckEquals(GetValueWithExitCourse(positionsWithCrossCourses, positionsInformation.Select(pos => pos.Value).ToList()).ToString("N1"),
                Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(
                    positionsAlertsStatisticsPanelForm.GetValueFromStatisticsSummaryBlock(PortfolioSummaryStatisticValueTypes.TradeSmithValue).Replace(",", string.Empty))).ToString("N1"),
                "Value in wrapDetailedPortfolioStats block is not calculated as Sum (Position Value * ExitCrossCoursePositionToSystem)");

            LogStep(3, "Check that Dividends Total in wrapDetailedPortfolioStats block is calculated as Sum " +
                "(Position Total Dividends * ExitCrossCoursePositionToPortfolio * ExitCrossCoursePortfolioToSystem)");
            Checker.CheckEquals(GetValueWithExitCourse(positionsWithCrossCourses, positionsInformation.Select(pos => pos.TotalDividends).ToList()).ToString("N1"),
                Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(
                    positionsAlertsStatisticsPanelForm.GetValueFromStatisticsSummaryBlock(PortfolioSummaryStatisticValueTypes.DividendTotal).Replace(",", string.Empty))).ToString("N1"),
                "Dividends Total in wrapDetailedPortfolioStats block is not calculated as Sum (Position Total Dividends * ExitCrossCoursePositionToSystem)");

            LogStep(4, "Check that Cost Basis in wrapDetailedPortfolioStats block is calculated as Sum " +
                "(Position Total C/B * EnterCrossCoursePositionToPortfolio * EnterCrossCoursePortfolioToSystem)");
            Checker.CheckEquals(GetValueWithEntryCourse(positionsWithCrossCourses, positionsInformation.Select(pos => pos.CostBasis).ToList()).ToString("N1"),
                Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(
                    positionsAlertsStatisticsPanelForm.GetValueFromStatisticsSummaryBlock(PortfolioSummaryStatisticValueTypes.CostBasis).Replace(",", string.Empty))).ToString("N1"),
                "Cost Basis in wrapDetailedPortfolioStats block is not calculated as Sum (Position Total C/B * EntryCrossCoursePositionToSystem)");

            LogStep(5, "Check that Today in wrapDetailedPortfolioStats block is calculated as Sum " +
                "(Position Daily Gain Total * ExitCrossCoursePositionToPortfolio * ExitCrossCoursePortfolioToSystem)");
            Checker.CheckEquals(GetValueWithExitCourse(positionsWithCrossCourses, positionsInformation.Select(pos => pos.DailyGainTotal).ToList()).ToString("N1"),
                Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(
                    positionsAlertsStatisticsPanelForm.GetValueFromStatisticsSummaryBlock(PortfolioSummaryStatisticValueTypes.DailyGain).Split(' ')[0].Replace(",", string.Empty))).ToString("N1"),
                "Today in wrapDetailedPortfolioStats block is calculated as Sum (Position Daily Gain Total * EntryCrossCoursePositionToSystem)");

            LogStep(6, "Check that Total Gain E/Div  block is calculated as Sum (Position Value * EntryCrossCoursePositionToSystem-" +
                " Position Total C/B * EnterCrossCoursePositionToPortfolio * EnterCrossCoursePortfolioToSystem)");
            Checker.CheckEquals((GetValueWithExitCourse(positionsWithCrossCourses, positionsInformation.Select(pos => pos.Value).ToList()) -
                    GetValueWithEntryCourse(positionsWithCrossCourses, positionsInformation.Select(pos => pos.CostBasis).ToList())).ToString("N1"),
                Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(
                    positionsAlertsStatisticsPanelForm.GetValueFromStatisticsSummaryBlock(PortfolioSummaryStatisticValueTypes.TotalGainExcludeDiv).Replace(",", string.Empty))).ToString("N1"),
                "Total Gain E/Div  block is not calculated as Sum (Position Value * EntryCrossCoursePositionToSystem - Position Total C/B * EntryCrossCoursePositionToSystem)");

            LogStep(7, "Check that Total Gain W/Div  block is calculated as Sum (PositionGainE/DivInSystemCurrency + " +
                "Position Total Dividends * EntryCrossCoursePositionToSystem)");
            Checker.CheckEquals((GetValueWithExitCourse(positionsWithCrossCourses, positionsInformation.Select(pos => pos.Value).ToList()) -
                    GetValueWithEntryCourse(positionsWithCrossCourses, positionsInformation.Select(pos => pos.CostBasis).ToList()) +
                    GetValueWithExitCourse(positionsWithCrossCourses, positionsInformation.Select(pos => pos.TotalDividends).ToList())).ToString("N1"),
                Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(
                    positionsAlertsStatisticsPanelForm.GetValueFromStatisticsSummaryBlock(PortfolioSummaryStatisticValueTypes.TotalGainWithDiv)
                        .Split(' ')[0].Replace(",", string.Empty))).ToString("N1"),
                "Total Gain W/Div  block is not calculated as Sum (PositionGainE/DivInSystemCurrency + Position Total Dividends * EntryCrossCoursePositionToSystem)");
        }

        private static double GetValueWithExitCourse(List<CrossCourseModel> positionsValuesWithCrossCourses, List<string> positionsGridData)
        {
            return positionsGridData.Select(
                (t, i) =>
                    Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(t.Replace(",", string.Empty))) * positionsValuesWithCrossCourses[i].ExitCrossCoursePositionToSystem)
                .Sum();
        }

        private static double GetValueWithEntryCourse(List<CrossCourseModel> positionsValuesWithCrossCourses, List<string> positionsGridData)
        {
            return positionsGridData.Select(
                (t, i) =>
                    Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(t.Replace(",", string.Empty))) * positionsValuesWithCrossCourses[i].EntryCrossCoursePositionToSystem)
                .Sum();
        }
    }
}