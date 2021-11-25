using System;
using System.Linq;
using System.Text.RegularExpressions;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Database.Timings;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.Timing;
using AutomatedTests.Forms;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Extensions;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._05_PositionCard
{
    [TestClass]
    public class TC_0067_PositionCard_PillsHaveExpectedWordingAndBehavior : BaseTestUnitTests
    {
        private const int TestNumber = 67;

        private int positionId;
        private int symbolId;

        private bool isHealthPillsPresent;
        private string healthShort;
        private string healthFull;
        private string holdDurationPattern;
        private string healthColor;
        private string expectedIconClass;

        private bool isTrendPillPresent;
        private string expectedTrendZones;
        private string trendFullText;
        private string trendColor;

        private bool isVqPillsPresent;
        private string vqShortDescription;
        private string vqFullText;
        private string vqColor;

        private bool isTimingPillPresent;

        private bool isLikefolioPillPresent;
        private string likefolioStatus;
        private string likefolioShortDescrition;
        private string likefolioFullDescrition;
        private string likefolioColor;

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
            var positionModel = new PositionsDBModel
            {
                Symbol = GetTestDataAsString("Symbol"),
                TradeType = $"{(int)GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>("TradeType")}",
                PurchaseDate = DateTime.Now.AddDays(-5).ToShortDateString(),
                StatusType = GetTestDataAsString("positionStatus"),
                CurrencyId = GetTestDataAsString("CurrencyId"),
                IgnoreDividend = GetTestDataAsString("adjust")
            };
            if (positionModel.StatusType == $"{(int)AutotestPositionStatusTypes.Close}")
            {
                positionModel.CloseDate = DateTime.Now.ToShortDateString();
                positionModel.ClosePrice = GetTestDataAsString("ClosePrice");
            }

            isHealthPillsPresent = GetTestDataAsBool(nameof(isHealthPillsPresent));
            healthShort = GetTestDataAsString(nameof(healthShort));
            healthFull = GetTestDataAsString(nameof(healthFull));
            holdDurationPattern = GetTestDataAsString(nameof(holdDurationPattern));
            healthColor = GetTestDataAsString(nameof(healthColor));
            expectedIconClass = GetTestDataAsString(nameof(expectedIconClass));

            isTrendPillPresent = GetTestDataAsBool(nameof(isTrendPillPresent));
            expectedTrendZones = GetTestDataAsString(nameof(expectedTrendZones));
            trendFullText = GetTestDataAsString(nameof(trendFullText));
            trendColor = GetTestDataAsString(nameof(trendColor));

            isVqPillsPresent = GetTestDataAsBool(nameof(isVqPillsPresent));
            var positionsQueries = new PositionsQueries();
            symbolId = Parsing.ConvertToInt(positionsQueries.SelectSymbolIdNameUsingSymbol(positionModel.Symbol).SymbolId);
            var vqValue = positionsQueries.SelectCurrentVqBySymbolId(symbolId);
            vqShortDescription = string.Format(GetTestDataAsString(nameof(vqShortDescription)), Parsing.ConvertToDouble(vqValue).ToString("N2"));
            vqFullText = GetTestDataAsString(nameof(vqFullText));
            vqColor = GetTestDataAsString(nameof(vqColor));

            isTimingPillPresent = GetTestDataAsBool(nameof(isTimingPillPresent));

            isLikefolioPillPresent = GetTestDataAsBool(nameof(isLikefolioPillPresent));
            likefolioStatus = GetTestDataAsString(nameof(likefolioStatus));
            likefolioShortDescrition = GetTestDataAsString(nameof(likefolioShortDescrition));
            likefolioFullDescrition = GetTestDataAsString(nameof(likefolioFullDescrition));
            likefolioColor = GetTestDataAsString(nameof(likefolioColor));

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));
            var portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            positionId = PositionsAlertsSetUp.AddPositionViaDB(portfolioId, positionModel);

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_67$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test checks that Health, VQ, Trend, Timing and Likefolio pills have expected appearence, wording and behavior. https://tr.a1qa.com/index.php?/cases/view/19232106")]
        [TestCategory("Smoke"), TestCategory("PositionCard"), TestCategory("PositionCardGeneral"), TestCategory("HealthGroup"), TestCategory("VqGroup"), TestCategory("ClosedPositionCard")]
        public override void RunTest()
        {
            LogStep(1, "Open position card for the position of precondition");
            new MainMenuNavigation().OpenPositionCard(positionId);
            var positionCardForm = new PositionCardForm();

            LogStep(2, "Check that health pill is shown according to isHealthPillsPresent");
            Checker.CheckEquals(isHealthPillsPresent, positionCardForm.IsPillPresent(PillsType.Health), "Health pill displaying is not as expected");

            LogStep(3, "If isHealthPillsPresent = true then -label of the pill is correct(health); - Status of the health is correct; - color of the health pill is correct; " +
                "- short and full wording of the health pill is correct; - there is information about period in the status");
            if (isHealthPillsPresent)
            {
                Checker.CheckEquals(healthShort, positionCardForm.GetPillTextByTypeState(PillsType.Health, PillState.Collapsed), "Health short description is not as expected");
                if (!string.IsNullOrEmpty(expectedIconClass))
                {
                    var expectedSsiZone = Dictionaries.TypeOfHealthTypeReverse[expectedIconClass];
                    Checker.CheckEquals(expectedSsiZone, positionCardForm.GetSsiStatus(), $"Icon Health is not the same as expected '({expectedSsiZone})'");
                }
                var actualColor = positionCardForm.GetPillColorByType(PillsType.Health);
                if (!string.IsNullOrEmpty(holdDurationPattern))
                {
                    Checker.IsTrue(Regex.IsMatch(positionCardForm.GetPillHoldDuration(PillsType.Health), holdDurationPattern), "Hold duration is not matched");
                }

                Checker.CheckContains(healthColor, actualColor, "Color for Health pill does not expected");
                Checker.IsTrue(Regex.IsMatch(positionCardForm.GetPillTextByTypeState(PillsType.Health, PillState.Expanded), healthFull), "Health full description is not as expected");
            }

            LogStep(4, "Check that VQ pill is shown according to isVqPillsPresent");
            Checker.CheckEquals(isVqPillsPresent, positionCardForm.IsPillPresent(PillsType.Vq), "Vq pill displaying is not as expected");

            LogStep(5, "If isVqPillsPresent= true then - label of the pill is correct(RISK(VQ)); - color of the VQ pill is correct; - short and full wording of the VQ pill is correct.");
            if (isVqPillsPresent)
            {
                Checker.CheckEquals(vqShortDescription, positionCardForm.GetPillTextByTypeState(PillsType.Vq, PillState.Collapsed), "Vq short description is not as expected");
                var actualColor = positionCardForm.GetPillColorByType(PillsType.Vq);
                Checker.CheckContains(vqColor, actualColor, "Color for Vq pill does not contains expected");
                Checker.IsTrue(Regex.IsMatch(positionCardForm.GetPillTextByTypeState(PillsType.Vq, PillState.Expanded), vqFullText), $"Vq full description is not as expected: '{vqFullText}'");
            }

            LogStep(6, "Check that Trend pill is shown according to isTrendPillsPresent");
            Checker.CheckEquals(isTrendPillPresent, positionCardForm.IsPillPresent(PillsType.HealthTrend), "Trend pill displaying is not as expected");

            LogStep(7, "if isTrendPillsPresent= true then - label of the pill is correct(TREND); - color of the Trend pill is correct; - short and full wording of the Trend pill is correct.");
            if (isTrendPillPresent)
            {
                Checker.CheckEquals(expectedTrendZones, positionCardForm.GetPillTextByTypeState(PillsType.HealthTrend, PillState.Collapsed), "Trend pill description is not as expected");
                var actualColor = positionCardForm.GetPillColorByType(PillsType.HealthTrend);
                Checker.CheckContains(trendColor, actualColor, "Color for Trend pill does not contains expected");
                Checker.IsTrue(Regex.IsMatch(positionCardForm.GetPillTextByTypeState(PillsType.HealthTrend, PillState.Expanded), trendFullText),
                    $"Trend full description is not as expected: '{trendFullText}'");
            }

            LogStep(8, "Check that Likefolio pill is shown according to isLikefolioPillsPresent");
            Checker.CheckEquals(isLikefolioPillPresent, positionCardForm.IsPillPresent(PillsType.Likefolio), "Likefolio pill displaying is not as expected");

            LogStep(9, "if isLikefolioPillsPresent = true then - ico for the pill is shown; - color of the Likefolio pill is correct; - short and full wording of the Likefolio pill is correct.");
            if (isLikefolioPillPresent)
            {
                var actualColor = positionCardForm.GetPillColorByType(PillsType.Likefolio);
                Checker.CheckContains(likefolioColor, actualColor, "Color for Likefolio pill does not contains expected");
                Checker.CheckEquals(positionCardForm.GetPillTextByTypeState(PillsType.Likefolio, PillState.Expanded), likefolioFullDescrition,
                    $"Likefolio full description is not as expected: '{trendFullText}'");
            }

            LogStep(10, "Check that Timing pill is shown according to isTimingPillsPresent");
            Checker.CheckEquals(isTimingPillPresent, positionCardForm.IsPillPresent(PillsType.Timing), "Timing pill displaying is not as expected");

            LogStep(11, "if isTimingPillsPresent = true then - label of the pill is correct(Peak / Valley); " +
                "- color of the Timing pill is correct; - short and full wording of the Timing pill is correct.");
            if (isTimingPillPresent)
            {
                var timingDbModel = new TimingsQueries().SelectNearestTimingModelBySymbolId(symbolId);
                var actualDirectionType = timingDbModel.GetTimingDirectionTypes();
                var expectedShortDescriptionRegex = actualDirectionType.In(TimingDirectionTypes.UpcomingPeak, TimingDirectionTypes.UpcomingValley)
                    ? Constants.TimingUpcomingTextRegex
                    : Constants.TimingCurrentTextRegex;
                var actualShortDescription = positionCardForm.GetPillTextByTypeState(PillsType.Timing, PillState.Collapsed);
                Checker.IsTrue(expectedShortDescriptionRegex.IsMatch(actualShortDescription),
                    $"Timing Short description {actualShortDescription} is not as expected");
                var actualColor = positionCardForm.GetPillColorByType(PillsType.Timing);
                var expectedTimingColor = actualDirectionType.In(TimingDirectionTypes.Valley, TimingDirectionTypes.UpcomingValley)
                    ? ColorConstants.TimingValleyColor
                    : ColorConstants.TimingPeakColor;
                Checker.CheckContains(expectedTimingColor, actualColor, "Color for Timing pill does not contains expected");
                var actualHint = positionCardForm.GetPillTextByTypeState(PillsType.Timing, PillState.Expanded);
                Checker.IsTrue(Constants.TimingHintTextRegex.IsMatch(actualHint),
                    $"Timing full description {actualHint} is not as expected");
            }
        }
    }
}