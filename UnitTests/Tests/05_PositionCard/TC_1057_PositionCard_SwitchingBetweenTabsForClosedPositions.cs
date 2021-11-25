using System;
using System.Collections.Generic;
using System.Linq;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Database.Users;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Chart;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.PositionCard;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Charts;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._05_PositionCard
{
    [TestClass]
    public class TC_1057_PositionCard_SwitchingBetweenTabsForClosedPositions : BaseTestUnitTests
    {
        private const int TestNumber = 1057;
        private const string DbParamTradeSmith = "OptionChain";
        private const string DbValueForPrecondition = "1";

        private int positionsQuantity;
        private bool isEventSectionOnChartShown;
        private List<bool> isIdeasTabShown;
        private List<bool> isNewsTabShown;
        private List<bool> isCompanyTabShown;
        private List<bool> isFinancialTabShown;
        private List<bool> isOptionsTabShown;
        private List<bool> isOptionsSubTabShown;
        private List<bool> isCryptoTabShown;
        private List<bool> isAdvancedTabShown;
        private List<bool> isEarningBlockShown;
        private readonly List<int> positionsIds = new List<int>();
        private readonly List<PositionsDBModel> positionsModels = new List<PositionsDBModel>();
        private string positionDetailsDescription;
        private string statisticsDescription;
        private string tagsAndNotesDescription;
        private string corporateActionsDescription;
        private string chartSettingsDescription;

        [TestInitialize]
        public void TestInitialize()
        {
            var userProductSubscriptions = GetUserProductSubscriptions("userSubscription");

            var portfolioModel = new PortfolioModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency")
            };

            positionsQuantity = GetTestDataAsInt(nameof(positionsQuantity));
            for (int i = 1; i <= positionsQuantity; i++)
            {
                positionsModels.Add(new PositionsDBModel
                {
                    Symbol = GetTestDataAsString($"Symbol{i}"),
                    TradeType = $"{(i % 2) + 1}",
                    StatusType = $"{(int)PositionStatusTypes.Close}",
                    CloseDate = DateTime.Now.AddDays(Constants.DaysQuantityForPreviousClose).ToShortDateString()
                });
            }

            isIdeasTabShown = GetTestDataValuesAsListByColumnName(nameof(isIdeasTabShown)).Select(Parsing.ConvertToBool).ToList();
            isNewsTabShown = GetTestDataValuesAsListByColumnName(nameof(isNewsTabShown)).Select(Parsing.ConvertToBool).ToList();
            isCompanyTabShown = GetTestDataValuesAsListByColumnName(nameof(isCompanyTabShown)).Select(Parsing.ConvertToBool).ToList();
            isFinancialTabShown = GetTestDataValuesAsListByColumnName(nameof(isFinancialTabShown)).Select(Parsing.ConvertToBool).ToList();
            isOptionsTabShown = GetTestDataValuesAsListByColumnName(nameof(isOptionsTabShown)).Select(Parsing.ConvertToBool).ToList();
            isOptionsSubTabShown = GetTestDataValuesAsListByColumnName(nameof(isOptionsSubTabShown)).Select(Parsing.ConvertToBool).ToList();
            isAdvancedTabShown = GetTestDataValuesAsListByColumnName(nameof(isAdvancedTabShown)).Select(Parsing.ConvertToBool).ToList();
            isCryptoTabShown = GetTestDataValuesAsListByColumnName(nameof(isCryptoTabShown)).Select(Parsing.ConvertToBool).ToList();
            isEarningBlockShown = GetTestDataValuesAsListByColumnName(nameof(isEarningBlockShown)).Select(Parsing.ConvertToBool).ToList();

            positionDetailsDescription = GetTestDataAsString(nameof(positionDetailsDescription));
            statisticsDescription = GetTestDataAsString(nameof(statisticsDescription));
            tagsAndNotesDescription = GetTestDataAsString(nameof(tagsAndNotesDescription));
            corporateActionsDescription = GetTestDataAsString(nameof(corporateActionsDescription));
            chartSettingsDescription = GetTestDataAsString(nameof(chartSettingsDescription));
            isEventSectionOnChartShown = GetTestDataAsBool(nameof(isEventSectionOnChartShown));

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));
            new UsersQueries().UpdateUserTradesmithFeaturesCustomizations(UserModels.First().TradeSmithUserId, DbParamTradeSmith, DbValueForPrecondition);

            var portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            foreach (var positionModel in positionsModels)
            {
                positionsIds.Add(PositionsAlertsSetUp.AddPositionViaDB(portfolioId, positionModel));
            }

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1057$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test checks Possibility opening of tabs without errors. https://tr.a1qa.com/index.php?/cases/view/19232180")]
        [TestCategory("Smoke"), TestCategory("ClosedPositionCard"), TestCategory("PositionCard"), TestCategory("ClosedPositionCardGeneral")]
        public override void RunTest()
        {
            LogStep(14, "Repeat all steps for all positions");
            for (int i = 0; i < positionsQuantity; i++)
            {
                LogStep(1, $"Open Closed Position Card for position for {positionsModels[i].Symbol}");
                new MainMenuNavigation().OpenPositionCard(positionsIds[i]);

                LogStep(2, $"Make sure tab not present:- Alerts for {positionsModels[i].Symbol}");
                var positionCardForm = new PositionCardForm();
                Checker.IsFalse(positionCardForm.IsTabPresent(PositionCardTabs.Alerts), $"Tab 'Alerts' is present for {positionsModels[i].Symbol}");

                LogStep(3, "Click 'Position Details' tab.Make sure:-grid isn't empty;- there is text on the table 'View your position details';" +
                    $"- Chart is shown on the right part of the screen for {positionsModels[i].Symbol}");
                var positionDetailsTabPositionCardForm = positionCardForm.ActivateTabWithoutChartWaitingGetForm<PositionDetailsTabPositionCardForm>(PositionCardTabs.PositionDetails);
                Checker.IsTrue(positionCardForm.IsTabActive(PositionCardTabs.PositionDetails), $"'Positions Details' tab is not active for {positionsModels[i].Symbol}");
                Checker.IsTrue(positionDetailsTabPositionCardForm.IsPositionDetailsBlockPresent(), $"Position Details block is not present for {positionsModels[i].Symbol}");
                Checker.CheckEquals(positionDetailsDescription, positionDetailsTabPositionCardForm.GetTabDescription(),
                    $"View and edit your position details is not present for {positionsModels[i].Symbol}");
                Checker.IsTrue(positionCardForm.IsChartPresent(), $"Chart is not present on Position Details for {positionsModels[i].Symbol}");

                LogStep(4, $"Click 'Performance' tab. Make sure: Performance block is present. Chart on the right part of the screen is shown for {positionsModels[i].Symbol}.");
                var performanceTabPositionCardForm = positionCardForm.ActivateTabGetForm<PerformanceTabPositionCardForm>(PositionCardTabs.Performance);
                Checker.IsTrue(positionCardForm.IsTabActive(PositionCardTabs.Performance), $"'Performance' tab is not active for {positionsModels[i].Symbol}");
                Checker.IsTrue(performanceTabPositionCardForm.IsPerformanceBlockPresent(), $"Performance block is not present for {positionsModels[i].Symbol}");
                Checker.IsTrue(positionCardForm.IsChartPresent(), $"Chart is not present on Performance for {positionsModels[i].Symbol}");

                LogStep(5, "Check that Opportunities tab is shown/not shown in accordance to isIdeasTabShown. If Indicator tab shown - " +
                    $"Click Opportunities tab and check that Opportunities block is present + Chart is shown for {positionsModels[i].Symbol}");
                var isTabPresent = positionCardForm.IsTabPresent(PositionCardTabs.MyOpportunities);
                Checker.CheckEquals(isIdeasTabShown[i], isTabPresent, $"'My Opportunities' tab presence is not as expected for {positionsModels[i].Symbol}");
                if (isIdeasTabShown[i] && isTabPresent)
                {
                    var ideasTabForm = positionCardForm.ActivateTabGetForm<MyOpportunitiesTabForm>(PositionCardTabs.MyOpportunities);
                    Checker.IsTrue(positionCardForm.IsTabActive(PositionCardTabs.MyOpportunities), $"'Indicator' tab is not active for {positionsModels[i].Symbol}");
                    Checker.IsTrue(ideasTabForm.IsOpportunitiesBlockPresent(), $"My Opportunities block is not present for {positionsModels[i].Symbol}");
                }

                LogStep(6, "Click 'Statistics' tab.Make sure:-Statistics block is present;- text present on the page " +
                    $"'Information as of ... ';- chart on the right part of the screen is shown for {positionsModels[i].Symbol}.");
                var generalStatisticTabPositionCardForm = positionCardForm.ActivateTabWithoutChartWaitingGetForm<StatisticTabForm>(PositionCardTabs.Statistics);
                Checker.IsTrue(positionCardForm.IsTabActive(PositionCardTabs.Statistics), $"'Statistics' tab is not active for {positionsModels[i].Symbol}");
                Checker.IsTrue(generalStatisticTabPositionCardForm.IsGeneralStatisticBlockPresent(), $"General Statistics block is not present for {positionsModels[i].Symbol}");
                Checker.IsTrue(generalStatisticTabPositionCardForm.GetStatisticTabDescription().Contains(statisticsDescription),
                    $"Information as of is not present for {positionsModels[i].Symbol}");
                if (isAdvancedTabShown[i])
                {
                    Checker.IsTrue(generalStatisticTabPositionCardForm.IsGeneralStatisticBlockPresent(), $"General Statistics block is not present for {positionsModels[i].Symbol}");
                }
                if (isEarningBlockShown[i])
                {
                    Checker.IsTrue(generalStatisticTabPositionCardForm.IsEarningBlockPresent(), $"Earning Statistics block is not present for {positionsModels[i].Symbol}");
                }
                Checker.IsFalse(positionCardForm.IsChartPresent(), $"Chart is present on 'Statistics' for {positionsModels[i].Symbol}");

                LogStep(7, "Click 'Tags & Notes' tab.Make sure:-text present on the page 'Keep track of your thoughts about this position'." +
                    $"- words 'Tags:' and 'Notes:' present on the page.- chart on the right part of the screen is shown for {positionsModels[i].Symbol}.");
                var tagsNotesTabPositionCardForm = positionCardForm.ActivateTabWithoutChartWaitingGetForm<TagsNotesTabPositionCardForm>(PositionCardTabs.TagsAndNotes);
                Checker.IsTrue(positionCardForm.IsTabActive(PositionCardTabs.TagsAndNotes), $"'Tag and Notes' tab is not active for {positionsModels[i].Symbol}");
                Checker.CheckEquals(tagsAndNotesDescription, tagsNotesTabPositionCardForm.GetTabDescription(),
                    $"Keep track of your thoughts about this position is not present for {positionsModels[i].Symbol}");
                Checker.IsTrue(tagsNotesTabPositionCardForm.IsTagBlockPresent(), $"Tags block is not present on the page for {positionsModels[i].Symbol}");
                Checker.IsTrue(tagsNotesTabPositionCardForm.IsNotesBlockPresent(), $"Notes block is not present on the page for {positionsModels[i].Symbol}");
                Checker.IsTrue(positionCardForm.IsChartPresent(), $"Chart is not present on 'Tags And Notes' for {positionsModels[i].Symbol}");

                LogStep(8, "Check that News tab is shown/not shown in accordance to isNewsTabShown. If News tab shown - Click 'News' tab." +
                    $"Make sure: News block is shown for {positionsModels[i].Symbol}");
                isTabPresent = positionCardForm.IsTabPresent(PositionCardTabs.News);
                Checker.CheckEquals(isNewsTabShown[i], isTabPresent, $"'News' tab presence is not as expected for {positionsModels[i].Symbol}");
                if (isNewsTabShown[i] && isTabPresent)
                {
                    var newsTabForm = positionCardForm.ActivateTabWithoutChartWaitingGetForm<NewsTabForm>(PositionCardTabs.News);
                    Checker.IsTrue(positionCardForm.IsTabActive(PositionCardTabs.News), $"'News' tab is not active for {positionsModels[i].Symbol}");
                    Checker.IsTrue(newsTabForm.IsNewsBlockPresent(), $"News block is not present for {positionsModels[i].Symbol}");
                    Checker.IsFalse(positionCardForm.IsChartPresent(), $"Chart is present on News for {positionsModels[i].Symbol}");
                }

                LogStep(9, "Check that Options tab is shown/not shown in accordance to isOptionsTabShown. If Options tab shown - " +
                    $"Click 'Options' tab. Make sure: Options table block is shown for {positionsModels[i].Symbol}");
                isTabPresent = positionCardForm.IsTabPresent(PositionCardTabs.Options);
                Checker.CheckEquals(isOptionsTabShown[i], isTabPresent, $"'Options' tab presence is not as expected for {positionsModels[i].Symbol}");
                if (isOptionsTabShown[i] && isTabPresent)
                {
                    var optionsTabForm = positionCardForm.ActivateTabWithoutChartWaitingGetForm<OptionsTabCommonForm>(PositionCardTabs.Options);
                    optionsTabForm.AssertIsOpen();
                    Checker.IsTrue(positionCardForm.IsTabActive(PositionCardTabs.Options), $"'Options' tab is not active for {positionsModels[i].Symbol} for List");
                    Checker.IsFalse(positionCardForm.IsChartPresent(), $"Chart is present on Options tab for {positionsModels[i].Symbol} for List");

                    if (isOptionsSubTabShown[i])
                    {
                        var optionsTabCalendarForm = new OptionsTabCalendarForm();
                        Checker.IsTrue(positionCardForm.IsTabActive(PositionCardTabs.Options), $"'Options' tab is not active for {positionsModels[i].Symbol} for Calendar");
                        Checker.IsTrue(optionsTabCalendarForm.IsOptionsTableBlockPresent(), $"Options Calendar block is not present for {positionsModels[i].Symbol}");
                        Checker.IsFalse(positionCardForm.IsChartPresent(), $"Chart is present on Options tab for {positionsModels[i].Symbol} for Calendar");

                        optionsTabForm.ActivateTab(OptionsTabSectionTypes.List);
                        var optionsTabListForm = new OptionsTabListForm();
                        Checker.IsTrue(optionsTabListForm.IsOptionsTableBlockPresent(), $"Options-List block is not present for {positionsModels[i].Symbol}");

                        optionsTabForm.ActivateTab(OptionsTabSectionTypes.Straddle);
                        var optionsTabStraddleForm = new OptionsTabStraddleForm();
                        Checker.IsTrue(positionCardForm.IsTabActive(PositionCardTabs.Options), $"'Options' tab is not active for {positionsModels[i].Symbol} for Straddle");
                        Checker.IsTrue(optionsTabStraddleForm.IsOptionsTableBlockPresent(), $"Options Straddle block is not present for {positionsModels[i].Symbol}");
                        Checker.IsFalse(positionCardForm.IsChartPresent(), $"Chart is present on Options tab for {positionsModels[i].Symbol} for Straddle");                        
                    }
                }

                LogStep(10, "Check that Company Profile tab is shown/not shown in accordance to isCompanyTabShown. If Company Profile tab shown - " +
                    $"Click 'Company Profile' tab. Make sure: Company Profile block is shown for {positionsModels[i].Symbol}");
                isTabPresent = positionCardForm.IsTabPresent(PositionCardTabs.CompanyProfile);
                Checker.CheckEquals(isCompanyTabShown[i], isTabPresent, $"'Company Profile' tab presence is not as expected for {positionsModels[i].Symbol}");
                if (isCompanyTabShown[i] && isTabPresent)
                {
                    var companyProfileTabForm = positionCardForm.ActivateTabWithoutChartWaitingGetForm<CompanyProfileTabForm>(PositionCardTabs.CompanyProfile);
                    Checker.IsTrue(positionCardForm.IsTabActive(PositionCardTabs.CompanyProfile), $"'Company Profile' tab is not active for {positionsModels[i].Symbol}");
                    Checker.IsTrue(companyProfileTabForm.IsCompanyDetailsBlockPresent(), $"Company Profile block is not present for {positionsModels[i].Symbol}");
                    Checker.IsFalse(positionCardForm.IsChartPresent(), $"Chart is present on Company Profile for {positionsModels[i].Symbol}");
                }

                LogStep(11, "Check that Financials tab is shown/not shown in accordance to isFinancialTabShown. If Financials tab shown - " +
                    $"Click 'Financials ' tab. Make sure: Financials block is shown for {positionsModels[i].Symbol}");
                isTabPresent = positionCardForm.IsTabPresent(PositionCardTabs.Financials);
                Checker.CheckEquals(isFinancialTabShown[i], isTabPresent, $"'Company Profile' tab presence is not as expected for {positionsModels[i].Symbol}");
                if (isFinancialTabShown[i] && isTabPresent)
                {
                    var financialsTabForm = positionCardForm.ActivateTabWithoutChartWaitingGetForm<FinancialsTabForm>(PositionCardTabs.Financials);
                    Checker.IsTrue(positionCardForm.IsTabActive(PositionCardTabs.Financials), $"'Financials' tab is not active for {positionsModels[i].Symbol}");
                    Checker.IsTrue(financialsTabForm.IsFinancialBlockPresent(), $"Financials block is not present for {positionsModels[i].Symbol}");
                    Checker.IsFalse(positionCardForm.IsChartPresent(), $"Chart is present on Financials for {positionsModels[i].Symbol}");
                }

                LogStep(11, "Check that Advanced tab is shown/not shown in accordance to isAdvancedTabShown. If Advanced tab shown - " +
                    $"Click 'Advanced ' tab. Make sure: option block is shown for {positionsModels[i].Symbol}");
                isTabPresent = positionCardForm.IsTabPresent(PositionCardTabs.Insights);
                Checker.CheckEquals(isAdvancedTabShown[i], isTabPresent, $"'Advanced' tab presence is not as expected for {positionsModels[i].Symbol}");
                if (isAdvancedTabShown[i] && isTabPresent)
                {
                    var advancedTabForm = positionCardForm.ActivateTabWithoutChartWaitingGetForm<InsightsTabForm>(PositionCardTabs.Insights);
                    Checker.IsTrue(positionCardForm.IsTabActive(PositionCardTabs.Insights), $"'Advanced' tab is not active for {positionsModels[i].Symbol}");
                    Checker.IsTrue(advancedTabForm.IsOptionsDataBlockPresent(), $"Option block is not present for {positionsModels[i].Symbol}");
                }

                LogStep(12, "Click 'Chart Settings' tab.Make sure:-text present on the tab 'Toggle the chart settings on and off';- There are sections:--Lines;" +
                    $"-- Events;-- Charts;- chart on the right part of the screen is shown for {positionsModels[i].Symbol}.");
                var chartSettingsTabPositionCardForm = positionCardForm.ActivateTabWithoutChartWaitingGetForm<ChartSettingsTabForm>(PositionCardTabs.ChartSettings);
                chartSettingsTabPositionCardForm.OpenChartSettings();
                Checker.IsTrue(positionCardForm.IsTabActive(PositionCardTabs.ChartSettings), $"'Charts settings' tab. not active for {positionsModels[i].Symbol}");
                Checker.CheckEquals(chartSettingsTabPositionCardForm.GetTabDescription(), chartSettingsDescription,
                    $"Toggle the chart settings on and off is not present for {positionsModels[i].Symbol}");
                Checker.IsTrue(chartSettingsTabPositionCardForm.IsSectionLabelPresent(ChartCheckboxCategoryTypes.Lines.GetStringMapping()),
                    $"Lines block is not present on the page for {positionsModels[i].Symbol}");
                Checker.CheckEquals(isEventSectionOnChartShown, 
                    chartSettingsTabPositionCardForm.IsSectionLabelPresent(ChartCheckboxCategoryTypes.Events.GetStringMapping()),
                    $"Events block is unexpected on the page for {positionsModels[i].Symbol}");
                Checker.IsTrue(chartSettingsTabPositionCardForm.IsSectionLabelPresent(ChartCheckboxCategoryTypes.Charts.GetStringMapping()),
                    $"Charts block is not present on the page for {positionsModels[i].Symbol}");
                Checker.IsTrue(positionCardForm.IsChartPresent(), $"Chart is not present 'Chart Settings' for {positionsModels[i].Symbol}");

                LogStep(13, $"Check that Coin profile tab is shown/not shown in accordance to isCryptoTabShown for {positionsModels[i].Symbol}");
                isTabPresent = positionCardForm.IsTabPresent(PositionCardTabs.CoinProfile);
                Checker.CheckEquals(isCryptoTabShown[i], isTabPresent, $"'Coin profile' tab presence is not as expected for {positionsModels[i].Symbol}");
                if (isCryptoTabShown[i] && isTabPresent)
                {
                    var coinProfileTabForm = positionCardForm.ActivateTabWithoutChartWaitingGetForm<CoinProfileTabForm>(PositionCardTabs.CoinProfile);
                    coinProfileTabForm.AssertIsOpen();
                    Checker.IsTrue(positionCardForm.IsTabActive(PositionCardTabs.CoinProfile), $"'Coin profile' tab is not active for {positionsModels[i].Symbol} for List");
                    Checker.IsFalse(positionCardForm.IsChartPresent(), $"Chart is present on Options tab for {positionsModels[i].Symbol} for List");
                }
            }
        }
    }
}