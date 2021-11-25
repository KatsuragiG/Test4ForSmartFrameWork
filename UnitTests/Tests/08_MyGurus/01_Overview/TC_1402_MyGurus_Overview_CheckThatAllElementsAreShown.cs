using AutomatedTests.ConstantVariables;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Dashboard;
using AutomatedTests.Enums.Newsletter;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Gurus;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Enums;
using TradeStops.Common.Extensions;
using TradeStops.Common.Helpers;
using TradeStops.Common.Utils;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._08_MyGurus._01_Overview
{
    [TestClass]
    public class TC_1402_MyGurus_Overview_CheckThatAllElementsAreShown : BaseTestUnitTests
    {
        private const int TestNumber = 1402;

        private string expectedDropdownWording;
        private string expectedPositionsInGreenZoneWording;
        private string firstPortfolioDropdownText;
        private string secondPortfolioDropdownText;
        private int expectedYearsQuantityAgo;
        private bool isPortfoilioPerfomanceShown;
        private bool isPortfolioDistributionShown;
        private List<string> equityDropdownWording;
        private readonly Dictionary<WidgetPortfolioDistributionTabs, bool> areTabsShownInDistributionWidget = new Dictionary<WidgetPortfolioDistributionTabs, bool>();
        private readonly Dictionary<string, bool> arePublishersShownInDropdown = new Dictionary<string, bool>();
        private WidgetForm gurusOverviewWidget;

        [TestInitialize]
        public void TestInitialize()
        {
            var userProductSubscriptions = GetUserProductSubscriptions("userSubscription");

            expectedDropdownWording = GetTestDataAsString(nameof(expectedDropdownWording));
            expectedPositionsInGreenZoneWording = GetTestDataAsString(nameof(expectedPositionsInGreenZoneWording));
            firstPortfolioDropdownText = GetTestDataAsString(nameof(firstPortfolioDropdownText));
            secondPortfolioDropdownText = GetTestDataAsString(nameof(secondPortfolioDropdownText));

            isPortfoilioPerfomanceShown = GetTestDataAsBool(nameof(isPortfoilioPerfomanceShown));
            isPortfolioDistributionShown = GetTestDataAsBool(nameof(isPortfolioDistributionShown));

            expectedYearsQuantityAgo = GetTestDataAsInt(nameof(expectedYearsQuantityAgo));
            equityDropdownWording = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(equityDropdownWording));

            var availalePublisherNames = EnumUtils.GetValues<PublisherTypes>()
                .Where(t => !t.In(PublisherTypes.Pt2Test, PublisherTypes.Pt2TradeSmith, PublisherTypes.Pt2Stansberry, PublisherTypes.Pt2TsOptions, PublisherTypes.Pt2TsPlatinum));
            foreach (var publisherType in availalePublisherNames)
            {
                arePublishersShownInDropdown.Add(EnumDisplayNamesHelper.Get(publisherType), GetTestDataAsBool($"is{publisherType}Shown"));
            }

            foreach (var tabType in EnumUtils.GetValues<WidgetPortfolioDistributionTabs>())
            {
                areTabsShownInDistributionWidget.Add(tabType, GetTestDataAsBool($"is{tabType}TabShown"));
            }

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));

            LoginSetUp.LogIn(UserModels.First());

            new MainMenuForm().ClickMenuItem(MainMenuItems.MyGurus);
            new GurusMenuForm().ClickGurusMenuItem(GurusMenuItems.Overview);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1402$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("https://tr.a1qa.com/index.php?/cases/view/22298208 The test checks that all elements are displayed on Overview page.")]
        [TestCategory("OverviewPage"), TestCategory("NewsLettersPage")]
        public override void RunTest()
        {
            LogStep(1, "Check that Overview tab is displayed");
            var gurusOverviewForm = new GurusOverviewForm();
            gurusOverviewForm.AssertIsOpen();
            Checker.IsTrue(new GurusMenuForm().IsGurusTabSelected(GurusMenuItems.Overview), "Overview tab is not active in the Gurus menu");

            LogStep(2, "Check that drop-down list is displayed");
            Checker.IsTrue(gurusOverviewForm.IsPortfolioDrodownShown(), "Portfolio dropdown is not exist");

            LogStep(3, "Check that drop-down list contain text");
            Checker.CheckContains(expectedDropdownWording, gurusOverviewForm.GetPortfoliosDropdownText(),
                "Portfolio dropdown default text is not as expected");

            LogStep(4, "Check that drop-down list is expanded after clicking and shows the next lists");
            var portfolioNames = gurusOverviewForm.GetAllPortfoliosNames();
            var expectedNames = new List<string>(arePublishersShownInDropdown.Keys);
            foreach (var portfolioName in expectedNames)
            {
                Checker.CheckEquals(arePublishersShownInDropdown[portfolioName], portfolioNames.Contains(portfolioName),
                    $"Portfolio dropdown has unexpected state of containing {portfolioName}\n{GetActualResultsString(portfolioNames)}");
            }

            LogStep(5, "Check that information about Positions in Green zone is displayed");
            CheckPositionsStatisticPanel(gurusOverviewForm);

            LogStep(6, "Check that widget Portfolio Performance (% change) is displayed if user has {test data: isPortfoilioPerfomanceShown}");
            var widgetsNames = gurusOverviewForm.GetWidgetNames();
            CheckPortfolioPerformanceWidget(widgetsNames);

            LogStep(7, "Check that widget Portfolio Distribution is displayed if user has {test data: isPortfolioDistributionShown}");
            CheckPortfolioDistributionWidget(widgetsNames);

            LogStep(9, "Check that widget Ranked Recommendations is displayed");
            Checker.IsTrue(widgetsNames.Contains(WidgetTypes.RankedRecommendations.GetStringMapping()),
                $"Ranked Recommendations widget has unexpected state\n {GetActualResultsString(widgetsNames)}");

            LogStep(10, "Check that widget Best Performing Recommendations is displayed");
            Checker.IsTrue(widgetsNames.Contains(WidgetTypes.BestPerformingRecommendations.GetStringMapping()),
                $"Best Performing Recommendations has unexpected state\n {GetActualResultsString(widgetsNames)}");

            LogStep(11, "Check that widget Recent Events is displayed");
            Checker.IsTrue(widgetsNames.Contains(WidgetTypes.NewslettersRecentEvents.GetStringMapping()),
                $"Recent Events has unexpected state\n {GetActualResultsString(widgetsNames)}");

            LogStep(12, "Check that widget Recent Events contain tabs.");
            gurusOverviewWidget = new WidgetForm(WidgetTypes.NewslettersRecentEvents, true);
            foreach (var tabType in EnumUtils.GetValues<WidgetRecentEventsTabs>())
            {
                Checker.IsTrue(gurusOverviewWidget.IsWidgetContentTabExist(tabType),
                    $"Portfolio Distribution tab {tabType.GetStringMapping()} has unexpected visibility");
            }
        }

        private void CheckPortfolioDistributionWidget(List<string> widgetsNames)
        {
            var actualWidgetVisibilityState = widgetsNames.Contains(WidgetTypes.PortfolioDistribution.GetStringMapping());
            Checker.CheckEquals(isPortfolioDistributionShown, actualWidgetVisibilityState,
                $"Portfolio Distribution widget has unexpected state\n {GetActualResultsString(widgetsNames)}");
            if (actualWidgetVisibilityState && isPortfolioDistributionShown)
            {
                LogStep(8, "Check that widget Portfolio Distribution contain tabs.");
                gurusOverviewWidget = new WidgetForm(WidgetTypes.PortfolioDistribution, false);
                foreach (var tabType in EnumUtils.GetValues<WidgetPortfolioDistributionTabs>())
                {
                    Checker.CheckEquals(areTabsShownInDistributionWidget[tabType], gurusOverviewWidget.IsWidgetContentTabExist(tabType),
                        $"Portfolio Distribution tab {tabType.GetStringMapping()} has unexpected visibility");
                }
            }
        }

        private void CheckPortfolioPerformanceWidget(List<string> widgetsNames)
        {
            var actualWidgetVisibilityState = widgetsNames.Contains(WidgetTypes.PortfolioEquityPerformance.GetStringMapping());
            Checker.CheckEquals(isPortfoilioPerfomanceShown, actualWidgetVisibilityState,
                $"Portfolio Performance (% change) widget has unexpected state\n {GetActualResultsString(widgetsNames)}");
            if (actualWidgetVisibilityState && isPortfoilioPerfomanceShown)
            {
                gurusOverviewWidget = new WidgetForm(WidgetTypes.PortfolioEquityPerformance);
                var actualEquityDropdownWording = gurusOverviewWidget.GetEquityWidgetDropDownWording();
                Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(equityDropdownWording, actualEquityDropdownWording),
                    $"Portfolio Performance (% change) dropdown has unexpected wording:\n{GetExpectedResultsString(equityDropdownWording)}\n" +
                    $"{GetActualResultsString(actualEquityDropdownWording)}");

                Checker.CheckEquals(firstPortfolioDropdownText, gurusOverviewWidget.GetWidgetFirstComparedPortfolioName(),
                    "Portfolio Performance (% change) widget has unexpected first portfolio");
                Checker.CheckEquals(secondPortfolioDropdownText, gurusOverviewWidget.GetSelectedValueFromWidgetDropDown(EquityPerformanceDropDownTypes.ComparePortfolios),
                    "Portfolio Performance (% change) widget has unexpected second portfolio");

                var expectedDate = DateTime.Now.AddYears(-expectedYearsQuantityAgo).ToString("MMMM d, yyyy");
                Checker.CheckEquals(expectedDate, gurusOverviewWidget.GetSelectedValueFromWidgetDropDown(EquityPerformanceDropDownTypes.FromDate),
                    "From Date has unexpected date");
                Checker.CheckEquals(GridFilterPeriods.Today.ToString(), gurusOverviewWidget.GetSelectedValueFromWidgetDropDown(EquityPerformanceDropDownTypes.ToDate),
                    "To Date has unexpected date");
            }
        }

        private void CheckPositionsStatisticPanel(GurusOverviewForm gurusOverviewForm)
        {
            Checker.CheckEquals(expectedPositionsInGreenZoneWording, gurusOverviewForm.GetStatisticTextByType(StatisticAttributeTypes.Description),
                "Positions in Green zone unexpected label");
            var healthPercentValue = gurusOverviewForm.GetStatisticTextByType(StatisticAttributeTypes.Value);
            var healthPercentValueColor = gurusOverviewForm.GetStatisticColorByType(StatisticAttributeTypes.Value);
            if (!healthPercentValue.Equals(Constants.NotAvailableAcronym))
            {
                Checker.IsTrue(Constants.PercentValuesRegex.IsMatch(healthPercentValue),
                    $"Positions in Green zone value is not as expected: {healthPercentValue}");
                var expectedHealthPercentValueColorValue = Parsing.ConvertToDecimal(Constants.DecimalNumberRegex.Match(healthPercentValue).Value) > 0
                    ? ColorConstants.BackgroundColorForActiveCheckbox
                    : ColorConstants.BackgroundColorForGrayText;
                Checker.CheckContains(expectedHealthPercentValueColorValue, healthPercentValueColor,
                    "Positions in Green zone value unexpected color for percent value");
            }
            else
            {
                Checker.CheckContains(ColorConstants.BackgroundColorForGrayText, healthPercentValueColor,
                    "Positions in Green zone value unexpected color for N/A");
            }
        }
    }
}