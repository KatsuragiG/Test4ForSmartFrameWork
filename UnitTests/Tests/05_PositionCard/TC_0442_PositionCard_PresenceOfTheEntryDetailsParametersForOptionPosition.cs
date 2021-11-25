using AutomatedTests;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Enums.PositionCard;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.PositionCard;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._05_PositionCard
{
    [TestClass]
    public class TC_0442_PositionCard_PresenceOfTheEntryDetailsParametersForOptionPosition : BaseTestUnitTests
    {
        private const int TestNumber = 442;

        private readonly List<int> optionsIds = new List<int>();

        [TestInitialize]
        public void TestInitialize()
        {
            var optionModelShort = new PositionsDBModel
            {
                Symbol = GetTestDataAsString("Symbol"),
                TradeType = $"{(int)PositionTradeTypes.Short}"
            };

            var optionModelLong = new PositionsDBModel
            {
                Symbol = GetTestDataAsString("Symbol"),
                TradeType = $"{(int)PositionTradeTypes.Long}"
            };

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPremium));

            var portfolioWatch = PortfoliosSetUp.AddWatchOnlyPortfolio(UserModels.First().Email);
            var portfolioManual = PortfoliosSetUp.AddInvestmentPortfoliosDefaultUSD(UserModels.First().Email);
            var portfolioSynch = PortfoliosSetUp.ImportSynchronizedPortfolio06ViaDb(UserModels.First());
            optionsIds.Add(new PositionsQueries().SelectPositionIdUsingPortfolioIdPositionSymbol(portfolioSynch, new CustomTestDataReader().GetSynchronized06Portfolio()[9]));
            optionsIds.Add(PositionsAlertsSetUp.AddPositionViaDB(portfolioManual, optionModelShort));
            optionsIds.Add(PositionsAlertsSetUp.AddPositionViaDB(portfolioWatch, optionModelLong));

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_442$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("https://tr.a1qa.com/index.php?/cases/view/19232593 The test checks that required data is present for Options position of Manual and Synch portfolios.")]
        [TestCategory("Smoke"), TestCategory("PositionCard"), TestCategory("PositionCardPositionDetailsTab")]
        public override void RunTest()
        {
            LogStep(1, 3, "Repeat steps for all options");
            foreach (var positionId in optionsIds)
            {
                LogStep(1, "Open Position Card -> Position Details  via direct link for position ");
                new MainMenuNavigation().OpenPositionCard(positionId);

                LogStep(2, "Make sure that there is date");
                var positionCardForm = new PositionCardForm();
                var positionDetailsTabPositionCardForm = positionCardForm.ActivateTabGetForm<PositionDetailsTabPositionCardForm>(PositionCardTabs.PositionDetails);
                Checker.CheckNotEquals(positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.EntryDate),
                    string.Empty, "Entry date is not present");
                Checker.CheckNotEquals(positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.EntryPrice),
                    string.Empty, "Entry price is not present");
                Checker.CheckNotEquals(positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.Contracts),
                    string.Empty, "Contracts is not present");
                Checker.CheckNotEquals(positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.EntryCommission),
                    string.Empty, "Entry commission is not present");
                Checker.CheckEquals(positionDetailsTabPositionCardForm.IsTradeTypeLong(),
                    new PositionsQueries().SelectPositionData(positionId).TradeType == (int)PositionTradeTypes.Long,
                    "Trade Type is not present");
                Checker.CheckNotEquals(positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.OptionType),
                    string.Empty, "Option Type is not present");
                Checker.CheckNotEquals(positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.DaysHeld),
                    string.Empty, "Days Held is not present");
                Checker.CheckNotEquals(positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.DaysToExpiration),
                    string.Empty, "Days To Expiration is not present");
                new PerformanceTabPositionCardSteps().AssertDataPresenceOnPerformanceTab();
            }
        }
    }
}