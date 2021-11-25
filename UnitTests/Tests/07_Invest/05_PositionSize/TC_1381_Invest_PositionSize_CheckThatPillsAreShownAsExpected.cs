using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.PositionData;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Dashboard;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.Timing;
using AutomatedTests.Enums;
using AutomatedTests.Forms.Dashboard;
using AutomatedTests.Forms.ResearchPages.PositionSizeForm;
using AutomatedTests.Forms;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using UnitTests.BaseTestsUnitTests;
using AutomatedTests.Enums.Tools;

namespace UnitTests.Tests._07_Invest._05_PositionSize
{
    [TestClass]
    public class TC_1381_Invest_PositionSize_CheckThatPillsAreShownAsExpected : BaseTestUnitTests
    {
        private const int TestNumber = 1381;

        private PortfolioModel portfolioModel;
        private PositionsDBModel positionModel;
        private string expectedPageDescription;
        private string expectedReadMoreLink;
        private string expectedAutocompleteLabel;
        private string expectedAutocompletePlaceHolder;
        private string ticker;
        private PositionAssetTypes tickerAssetType;
        private string expectedHealthLineColor;
        private string expectedTradeTypeLabel;
        private string expectedLatestCloseLabelWording;
        private string expectedCurrentStockLabelWording;
        private HealthZoneTypes expectedHealth;
        private string expectedExchangeName;

        private string expectedHealthFullText;
        private string expectedTrendFullText;
        private string expectedVqShortDescription;
        private string expectedVqLongDescription;

        private bool isTimingPillsPresent;
        private bool isTrendPillsPresent;
        private bool isLikefolioPillsPresent;

        private string expectedTimingLongDescription;
        private HealthTrendZones expectedHealthTrendZones;
        private TimingScheduleTypes expectedTimingSchedule;
        private TimingDirectionTypes expectedTimingDirection;
        private TimingConvictionLevelTypes expectedTimingConvictionLevel;

        private string expectedLikefolioStatus;
        private string expectedLikefolioShortDescription;
        private string expectedLikefolioFullDescription;
        private string expectedLikefolioColor;

        private readonly PositionDataQueries positionDataQueries = new PositionDataQueries();
        private readonly PositionsQueries positionsQueries = new PositionsQueries();

        [TestInitialize]
        public void TestInitialize()
        {
            var userProductSubscriptions = GetUserProductSubscriptions("userSubscription");

            portfolioModel = new PortfolioModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency")
            };
            positionModel = new PositionsDBModel
            {
                StatusType = GetTestDataAsString("DbStatus"),
                Symbol = GetTestDataAsString("SymbolBefore"),
                TradeType = ((int)GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>("TradeType")).ToString(),
                Shares = GetTestDataAsString("Shares")
            };

            expectedPageDescription = GetTestDataAsString(nameof(expectedPageDescription));
            expectedReadMoreLink = GetTestDataAsString(nameof(expectedReadMoreLink));
            expectedAutocompleteLabel = GetTestDataAsString(nameof(expectedAutocompleteLabel));
            expectedAutocompletePlaceHolder = GetTestDataAsString(nameof(expectedAutocompletePlaceHolder));
            ticker = GetTestDataAsString(nameof(ticker));

            tickerAssetType = GetTestDataParsedAsEnumFromStringMapping<PositionAssetTypes>(nameof(tickerAssetType));
            expectedHealthLineColor = GetTestDataAsString(nameof(expectedHealthLineColor));
            expectedTradeTypeLabel = GetTestDataAsString(nameof(expectedTradeTypeLabel));
            expectedLatestCloseLabelWording = GetTestDataAsString(nameof(expectedLatestCloseLabelWording));
            expectedCurrentStockLabelWording = GetTestDataAsString(nameof(expectedCurrentStockLabelWording));

            expectedHealth = (HealthZoneTypes)GetTestDataAsInt(nameof(expectedHealth));
            expectedExchangeName = GetTestDataAsString(nameof(expectedExchangeName));

            expectedHealthTrendZones = GetTestDataParsedAsEnumFromStringMapping<HealthTrendZones>(nameof(expectedHealthTrendZones));
            expectedHealthFullText = GetTestDataAsString(nameof(expectedHealthFullText));
            expectedTrendFullText = GetTestDataAsString(nameof(expectedTrendFullText));

            expectedVqShortDescription = GetTestDataAsString(nameof(expectedVqShortDescription));
            expectedVqLongDescription = GetTestDataAsString(nameof(expectedVqLongDescription));

            isTimingPillsPresent = GetTestDataAsBool(nameof(isTimingPillsPresent));
            isTrendPillsPresent = GetTestDataAsBool(nameof(isTrendPillsPresent));
            isLikefolioPillsPresent = GetTestDataAsBool(nameof(isLikefolioPillsPresent));

            expectedTimingSchedule = GetTestDataParsedAsEnumFromStringMapping<TimingScheduleTypes>(nameof(expectedTimingSchedule));
            expectedTimingDirection = GetTestDataParsedAsEnumFromStringMapping<TimingDirectionTypes>(nameof(expectedTimingDirection));
            expectedTimingConvictionLevel = GetTestDataParsedAsEnumFromStringMapping<TimingConvictionLevelTypes>(nameof(expectedTimingConvictionLevel));
            expectedTimingLongDescription = GetTestDataAsString(nameof(expectedTimingLongDescription));

            expectedLikefolioStatus = GetTestDataAsString(nameof(expectedLikefolioStatus));
            expectedLikefolioShortDescription = GetTestDataAsString(nameof(expectedLikefolioShortDescription));
            expectedLikefolioFullDescription = GetTestDataAsString(nameof(expectedLikefolioFullDescription));
            expectedLikefolioColor = GetTestDataAsString(nameof(expectedLikefolioColor));

            LogStep(0, "Precondition; Login");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));

            var portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            PositionsAlertsSetUp.AddPositionViaDB(portfolioId, positionModel);

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
            new DashboardForm().ClickActionItem(DashboardWidgetActionItems.CalculatePositionSize);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1381$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("ResearchPage"), TestCategory("PositionSize"), TestCategory("SSIGroup"), TestCategory("VQGroup")]
        [Description("Test for header elements on the PositionSizeCalculator page and it's pills. https://tr.a1qa.com/index.php?/cases/view/21127477")]
        [Ignore]
        public override void RunTest()
        {
            LogStep(1, "Click 'Calculate Position Size' button on the dashboard carousel.");
            var positionSizeForm = new PositionSizeCalculatorForm();
            Checker.CheckEquals(expectedPageDescription, positionSizeForm.GetPageDescription(), "Page description is not as expected");
            Checker.CheckEquals(expectedReadMoreLink, positionSizeForm.GetReadMoreLink(), "Read more link is not as expected");

            LogStep(2, "Enter symbol in the 'Search for Ticker' field.");
            positionSizeForm.SelectSource(PositionSizeSourceTypes.IndividualSecurities);
            positionSizeForm.SetSymbol(ticker, Constants.DefaultOrderOfSameItemsToReturn);
            Checker.CheckEquals(ticker,
                positionSizeForm.GetTextFromPositionsAutocompleteDataFields(PositionForManualPortfolioCreateInformation.Ticker, Constants.DefaultOrderOfSameItemsToReturn),
                "Ticker in auto-complete is not as expected");
            Checker.CheckEquals(PositionTradeTypes.Long, positionSizeForm.GetSelectedTradeTypeByOrder(Constants.DefaultOrderOfSameItemsToReturn), "Trade Type:' Long is not shown");
        }
    }
}