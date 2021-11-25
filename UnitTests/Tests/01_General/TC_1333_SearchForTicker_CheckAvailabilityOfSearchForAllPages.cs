using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Database.Users;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.Tools;
using AutomatedTests.Enums;
using AutomatedTests.Forms.AddPositionAdvanced;
using AutomatedTests.Forms.BackTester;
using AutomatedTests.Forms.Charts;
using AutomatedTests.Forms.Dashboard;
using AutomatedTests.Forms.Events;
using AutomatedTests.Forms.Gurus;
using AutomatedTests.Forms.Markets;
using AutomatedTests.Forms.OpportunitiesForm;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.PortfolioTracker;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.Publications;
using AutomatedTests.Forms.ResearchPages.AssetAllocation;
using AutomatedTests.Forms.ResearchPages.PositionSizeForm;
using AutomatedTests.Forms.ResearchPages.PureQuant;
using AutomatedTests.Forms.ResearchPages.PVQAnalyzer;
using AutomatedTests.Forms.ResearchPages.RiskRebalancer;
using AutomatedTests.Forms.Screener;
using AutomatedTests.Forms.Settings.Alerts;
using AutomatedTests.Forms.Settings.Contact;
using AutomatedTests.Forms.Settings.Notification;
using AutomatedTests.Forms.Settings.Position;
using AutomatedTests.Forms.Settings.Support;
using AutomatedTests.Forms.Settings.Tags;
using AutomatedTests.Forms.Templates;
using AutomatedTests.Forms.TimingCalendar;
using AutomatedTests.Forms;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.BrowserSteps;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;
using WebdriverFramework.Framework.WebDriver;

namespace UnitTests.Tests._01_General
{
    [TestClass]
    public class TC_1333_SearchForTicker_CheckAvailabilityOfSearchForAllPages : BaseTestUnitTests
    {
        private const int TestNumber = 1333;
        private const string DbValueForPrecondition = "1";

        private PortfolioModel portfolioModel;
        private PositionsDBModel closedPosition;
        private PositionsDBModel openPosition;
        private string validTickerName;
        private string validTickerNasdaq;
        private string validTickerCompany;
        private int countOfTreeSelectItems;
        private int openPositionId;
        private int closedPositionId;
        private string itemPattern;

        private readonly List<string> DbParamTradeSmith = new List<string> { "BackTester" };

        [TestInitialize]
        public void TestInitialize()
        {
            validTickerName = GetTestDataAsString(nameof(validTickerName));
            validTickerNasdaq = GetTestDataAsString(nameof(validTickerNasdaq));
            validTickerCompany = GetTestDataAsString(nameof(validTickerCompany));
            itemPattern = GetTestDataAsString(nameof(itemPattern));
            countOfTreeSelectItems = GetTestDataAsInt(nameof(countOfTreeSelectItems));

            portfolioModel = new PortfolioModel
            {
                Name = "CheckAvailabilityOfSearchForAllPages",
                Type = PortfolioType.Investment,
                Currency = Currency.USD.GetStringMapping(),
                Notes = "Portfolio: Notes",
                EntryCommission = Constants.DefaultStringZeroIntValue,
                ExitCommission = Constants.DefaultStringZeroIntValue,
                Cash = "100"
            };

            closedPosition = new PositionsDBModel
            {
                Symbol = GetTestDataAsString("closedPositionTicker"),
                PurchaseDate = GetTestDataAsString("entryDate"),
                PurchasePrice = GetTestDataAsString("entryPrice"),
                Shares = GetTestDataAsString("shares"),
                StatusType = $"{(int)AutotestPositionStatusTypes.Close}"
            };

            openPosition = new PositionsDBModel
            {
                Symbol = GetTestDataAsString("openPositionTicker"),
                PurchaseDate = closedPosition.PurchaseDate,
                PurchasePrice = closedPosition.PurchasePrice,
                Shares = closedPosition.Shares
            };

            LogStep(0, "Precondition - Login as registered user");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, new List<ProductSubscriptions> {
                ProductSubscriptions.TradeStopsPlatinum,
                ProductSubscriptions.TestOrganization
            }));
            var userModel = UserModels.First();
            var usersQueries = new UsersQueries();
            foreach (var dbParamTradeSmith in DbParamTradeSmith)
            {
                usersQueries.UpdateUserTradesmithFeaturesCustomizations(userModel.TradeSmithUserId, dbParamTradeSmith, DbValueForPrecondition);
            }

            var portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            closedPositionId = PositionsAlertsSetUp.AddPositionViaDB(portfolioId, closedPosition);
            openPositionId = PositionsAlertsSetUp.AddPositionViaDB(portfolioId, openPosition);

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1333$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("Test for Search for Ticker functionality on different pages: https://tr.a1qa.com/index.php?/cases/view/19234218")]
        [TestCategory("Smoke"), TestCategory("SearchForTicker")]
        public override void RunTest()
        {
            LogStep(1, $"Dashboard page: Enter '{validTickerName}' in Search for Ticker field");
            var mainMenuNavigation = new MainMenuNavigation();
            mainMenuNavigation.OpenDashboard();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<DashboardForm>("Dashboard");

            LogStep(2, $"Positions -> Alerts tab: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenAlertsGrid();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<AlertsTabForm>("AlertsTab");

            LogStep(3, $"Positions -> Positions tab:: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenPositionsGrid();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<PositionsTabForm>("PositionsTab");

            LogStep(4, $"Positions -> ClosedPositions tab:: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenPositionsGrid(PositionsTabs.ClosedPositions);
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<ClosedPositionsTabForm>("ClosedPositionsTab");

            LogStep(5, $"InvestmentPortfolios page: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenInvestmentPortfoliosTab();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<PortfoliosForm>("InvestmentPortfolios");

            LogStep(6, $"ImportPortfolioFromCsv page: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenImportPortfolioFromCsv();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<ImportPortolioFromCsvForm>("ImportPortfolioFromCsv");

            LogStep(7, $"WatchOnlyPortfoliosType page: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenWatchOnlyPortfoliosTab();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<PortfoliosForm>("WatchOnlyPortfoliosType");

            LogStep(8, $"StockAnalyzer page: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenStockAnalyzerForDefaultTicker();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<ChartSettingsTabForm>("ChartSettingsTabForm");

            LogStep(9, $"PositionSize page: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenPositionSize();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<PositionSizeCalculatorForm>("NewPositionSize");

            LogStep(10, $"PureQuant page: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenPureQuantRunsGrid();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<PureQuantRunInitForm>("PureQuantInit");

            mainMenuNavigation.OpenPureQuant(PureQuantInternalSteps.Step1ChooseSources);
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<PureQuantCommonForm>(PureQuantInternalSteps.Step1ChooseSources.GetStringMapping());

            mainMenuNavigation.OpenPureQuant(PureQuantInternalSteps.Step2ChangeDefaultSettings);
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<PureQuantCommonForm>(PureQuantInternalSteps.Step2ChangeDefaultSettings.GetStringMapping());

            mainMenuNavigation.OpenPureQuant(PureQuantInternalSteps.Step3CustomizePortfolio);
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<PureQuantCommonForm>(PureQuantInternalSteps.Step3CustomizePortfolio.GetStringMapping());

            LogStep(11, $"AssetAllocation page: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenAssetAllocation();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<AssetAllocationForm>("AssetAllocation");

            LogStep(12, $"PVQAnalyzer page: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenPvqAnalyzer();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<PvqAnalyzerForm>("PVQAnalyzer");

            LogStep(13, $"RiskRebalancer page: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenRiskRebalancer();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<RiskRebalancerForm>("RiskRebalancer");

            LogStep(14, $"MainPublishers page: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenNewsletters();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<MainPublishersForm>("MainPublishers");
            mainMenuNavigation.OpenCustomPublisherGrid();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<SelectedPublisherForm>("SelectedPublisherForm");
            mainMenuNavigation.OpenGurusOverview();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<GurusOverviewForm>("GurusOverviewForm");
            mainMenuNavigation.OpenGurusTopRecommendations();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<GurusTopRecommendationsForm>("GurusTopRecommendationsForm");

            LogStep(15, $"Settings -> Contact: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenSettings();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<ContactForm>("Contact");

            LogStep(16, $"CryptoMarketOutlookForm: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenCryptoMarketOutlook();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<CryptoMarketOutlookForm>("CryptoMarketOutlookForm");

            LogStep(17, $"Screener: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenSavedScreeners();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<SavedScreenersForm>("SavedScreenersForm");

            LogStep(18, $"Settings -> Notification: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenSettingsNotification();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<NotificationForm>("Notification");

            LogStep(19, $"Settings -> PositionSetting: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenSettingsPositions();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<PositionSettingForm>("PositionSetting");

            LogStep(20, $"Settings -> AlertsSettings: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenSettingsAlerts();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<AlertsSettingsForm>("AlertsSettings");

            LogStep(21, $"Settings -> Tags: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenSettingsTags();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<TagsForm>("Tags");

            LogStep(22, $"Settings -> Support: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenSettingsSupport();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<SupportForm>("Support");

            LogStep(23, $"Events page: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenEvents();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<EventsForm>("Events");

            LogStep(24, $"Templates: Dashboard page: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenTemplates();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<TemplatesForm>("Templates");
            mainMenuNavigation.OpenCreationTemplatePage();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<AddTemplateForm>("AddTemplateForm");

            LogStep(25, $"Screener Filters: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenScreenerFilters();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<ScreenerFiltersForm>("ScreenerFiltersForm");

            LogStep(26, $"AddPositionAdvanced: Dashboard page: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenAddPositionAdvanced();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<AddPositionAdvancedForm>("AddPositionAdvanced");

            LogStep(27, $"Opened PositionDetailsTabPositionCard: Dashboard page: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenPositionCard(openPositionId);
            new BrowserSteps().AcceptAlert();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<PositionDetailsTabPositionCardForm>("Opened PositionDetailsTabPositionCard");

            LogStep(28, $"SelectPortfolioFlow: Dashboard page: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenCreatePortfolioPage();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<SelectPortfolioFlowForm>("SelectPortfolioFlow");

            LogStep(29, $"Closed PositionDetailsTabPositionCard: Dashboard page: Enter '{validTickerName}' in Search for Ticker field");
            Browser.Refresh();
            mainMenuNavigation.OpenPositionCard(closedPositionId);
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<PositionDetailsTabPositionCardForm>("Closed PositionDetailsTabPositionCard");

            LogStep(30, $"ManualPortfolioCreation: Dashboard page: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenAddManualPortfolio();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<ManualPortfolioCreationForm>("ManualPortfolioCreation");

            LogStep(31, $"Market Outlook tab: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenMarketOutlook();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<MarketsHealthCommonForm>("Market Outlook tab");

            LogStep(32, $"S&P Sectors tab: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenSpSectors();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<MarketsHealthCommonForm>("S&P Sectors tab");

            LogStep(33, $"Commodities tab: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenCommodities();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<MarketsHealthCommonForm>("Commodities tab");

            LogStep(34, $"Opportunities: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenOpportunities();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<OpportunitiesForm>(InvestMenuItems.Opportunities.GetStringMapping());

            LogStep(35, $"Publications: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenPublications();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<PublicationsForm>("PublicationsForm");

            LogStep(36, $"Timing calendar: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenTimingCalendar();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<TimingCalendarForm>("TimingCalendarForm");

            LogStep(37, $"Option Opportunities: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenOptionOpportunities();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<OpportunitiesOptionsForm>("OpportunitiesOptionsForm");

            LogStep(38, $"Option Opportunities: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenSavedOptionScreeners();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<SavedOptionScreenersForm>("OptionSavedScreenerForm");
            mainMenuNavigation.OpenOptionScreeners();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<OptionScreenerForm>("OptionScreenerForm");

            LogStep(39, $"PT manage: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenPtManageForm();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<ManageForm>("ManageForm");
            mainMenuNavigation.OpenPtPositionsGrid();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<PtPositionsTabForm>("PtPositionsTabForm");

            LogStep(40, $"Back Tester: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenBackTesterTasks();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<BackTesterTasksForm>("BackTesterTasksForm");

            LogStep(41, $"Platinum area: Enter '{validTickerName}' in Search for Ticker field");
            mainMenuNavigation.OpenPlatinumArea();
            CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<PlatinumAreaDashboardForm>("PlatinumAreaDashboardForm");
        }

        private void CheckThatPageOpenedAndEnterSymbolToSearchFieldAndCheck<T>(string pageName) where T : BaseForm, new()
        {
            if (CheckThatPageOpens<T>(pageName))
            {
                CheckItemsFromSymbolTreeSelectAutocomplete(new MainMenuForm().GetItemsInSymbolTreeSelectAutocomplete(validTickerName), pageName);
            }
        }

        private bool CheckThatPageOpens<T>(string pageName) where T : BaseForm, new()
        {
            var baseAppForm = new BrowserSteps().CheckThatPageOpensGetPageWithSoftAssert<T>(pageName);
            if (baseAppForm == null)
            {
                Checker.Fail($"{pageName} page is not opened");
                return false;
            }

            return true;
        }

        private void CheckItemsFromSymbolTreeSelectAutocomplete(IReadOnlyCollection<string> treeSelectItems, string pageName)
        {
            var actualTreeSelectItems = treeSelectItems.Select(e => e.ReplaceNewLineWithTrim()).ToList();
            var expectedItemName = string.Format(itemPattern, validTickerName, validTickerNasdaq, validTickerCompany);

            Checker.CheckEquals(countOfTreeSelectItems, treeSelectItems.Count,
                $"Count of tree select items is not as expected on page {pageName}.\nFound items:\n{string.Join("\n", actualTreeSelectItems)}");
            Checker.IsTrue(actualTreeSelectItems.Contains(expectedItemName),
                $"List of tree select items on page {pageName} doesn't contain expected item [{expectedItemName}]\nFound items:\n{string.Join("\n", actualTreeSelectItems)}");
        }
    }
}