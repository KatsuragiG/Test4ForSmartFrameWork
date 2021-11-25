using System;
using System.Collections.Generic;
using System.Linq;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.PositionCard;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._05_PositionCard
{
    [TestClass]
    public class TC_1141_ClosedPositionCard_SwitchingBetweenClosedPositionsWithinOnePortfolioUsingChevron : BaseTestUnitTests
    {
        private const int TestNumber = 1141;
        private const int DaysShiftForEntryDate = -365;

        private PortfolioModel portfolioModel;
        private int symbolsQuantity;
        private int portfolioId;
        private readonly List<PositionsDBModel> positionsModels = new List<PositionsDBModel>();
        private List<string> symbolsSortedList;

        [TestInitialize]
        public void TestInitialize()
        {
            portfolioModel = new PortfolioModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency")
            };
            symbolsQuantity = GetTestDataAsInt(nameof(symbolsQuantity));
            for (int i = 1; i <= symbolsQuantity; i++)
            {
                positionsModels.Add(new PositionsDBModel
                {
                    Symbol = GetTestDataAsString($"Symbol{i}"),
                    TradeType = GetTestDataAsString($"tradeType{i}"),
                    PurchaseDate = DateTime.Now.AddDays(DaysShiftForEntryDate).ToShortDateString(),
                    StatusType = $"{(int)AutotestPositionStatusTypes.Close}",
                    CloseDate = DateTime.Now.ToShortDateString()
                });
            }

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPremium));
            portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            foreach (var positionModel in positionsModels)
            {
                PositionsAlertsSetUp.AddPositionViaDB(portfolioId, positionModel);
            }
            symbolsSortedList = positionsModels.Select(positionModel => positionModel.Symbol).ToList().OrderBy(p => p).ToList();

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1141$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test checks possibility of switching between closed positions within one portfolio. https://tr.a1qa.com/index.php?/cases/view/19232040")]
        [TestCategory("Smoke"), TestCategory("ClosedPositionCard"), TestCategory("PositionCard"), TestCategory("PositionCardChevrons")]
        public override void RunTest()
        {
            LogStep(1, "Open Closed Position Card -> Position Details  via direct link for position");
            new MainMenuNavigation().OpenPositionCard(new PositionsQueries().SelectPositionIdUsingPortfolioIdPositionSymbol(portfolioId, symbolsSortedList[0]));
            var positionCardForm = new PositionCardForm();
            Checker.CheckEquals(symbolsSortedList[0], positionCardForm.GetSymbol(), "Position Card for the 1st position is not shown");

            LogStep(2, "Make sure there is expected text above 'chevron-right' button.");
            Checker.CheckEquals(ChevronTypes.Next.ToString().ToUpper(), positionCardForm.GetChevronText(ChevronTypes.Next), 
                "Text above 'chevron-right' button is not as expected");

            LogStep(3, "Make sure there is expected ticker name near from 'chevron-right' button.");
            Checker.CheckEquals(symbolsSortedList[1], positionCardForm.GetChervonSymbol(ChevronTypes.Next), "Next ticker name is not as expected when 1st position open");

            LogStep(4, "Make sure there is no 'chevron-left' button.");
            Checker.IsFalse(positionCardForm.IsChevronPresent(ChevronTypes.Prev), "Left chevron button is present when 1st position open");

            LogStep(5, "Click 'chevron-right' button.");
            positionCardForm.ClickChevron(ChevronTypes.Next);
            Checker.CheckEquals(symbolsSortedList[1], positionCardForm.GetSymbol(), "Position Card for the 2nd position is not shown (after clicking right chevron)");

            LogStep(6, "Make sure there is expected ticker name for:- 'chevron-right' button.- 'chevron-left' button.");
            Checker.CheckEquals(symbolsSortedList[2], positionCardForm.GetChervonSymbol(ChevronTypes.Next), 
                "Next ticker name is not as expected when 2nd position open (after clicking right chevron)");
            Checker.CheckEquals(symbolsSortedList[0], positionCardForm.GetChervonSymbol(ChevronTypes.Prev),
                "Prev ticker name is not as expected when 2nd position open (after clicking right chevron)");

            LogStep(7, "Click 'chevron-right' button.");
            positionCardForm.ClickChevron(ChevronTypes.Next);
            Checker.CheckEquals(symbolsSortedList[2], positionCardForm.GetSymbol(), "Position Card for the 3rd position is not shown (after clicking right chevron)");

            LogStep(8, "Repeat step #6.");
            Checker.CheckEquals(symbolsSortedList[3], positionCardForm.GetChervonSymbol(ChevronTypes.Next),
                "Next ticker name is not as expected when 3rd position open (after clicking right chevron)");
            Checker.CheckEquals(symbolsSortedList[1], positionCardForm.GetChervonSymbol(ChevronTypes.Prev),
                "Prev ticker name is not as expected when 3rd position open (after clicking right chevron)");

            LogStep(9, "Click 'chevron-right' button.");
            positionCardForm.ClickChevron(ChevronTypes.Next);
            Checker.CheckEquals(positionCardForm.GetSymbol(), symbolsSortedList[3], "Position Card for the last position is not shown (after clicking right chevron)");

            LogStep(10, "Make sure there is no 'chevron-right' button.");
            Checker.IsFalse(positionCardForm.IsChevronPresent(ChevronTypes.Next), "Right chevron button is present when last position open");

            LogStep(11, "Make sure there is expected text above 'chevron-left' button.");
            Checker.CheckEquals(ChevronTypes.Prev.ToString().ToUpper(), positionCardForm.GetChevronText(ChevronTypes.Prev), 
                "Text above 'chevron-left' button is not as expected");

            LogStep(12, "Click 'chevron-left' button.");
            positionCardForm.ClickChevron(ChevronTypes.Prev);
            Checker.CheckEquals(symbolsSortedList[2], positionCardForm.GetSymbol(), "Position Card for the 3rd position is not shown (after clicking left chevron)");

            LogStep(13, "Repeat step #6");
            Checker.CheckEquals(symbolsSortedList[3], positionCardForm.GetChervonSymbol(ChevronTypes.Next), 
                "Next ticker name is not as expected when 3rd position open (after clicking left chevron)");
            Checker.CheckEquals(symbolsSortedList[1], positionCardForm.GetChervonSymbol(ChevronTypes.Prev), 
                "Prev ticker name is not as expected when 3rd position open (after clicking left chevron)");

            LogStep(14, "Repeat steps #12, 13 for the second and third position.");
            positionCardForm.ClickChevron(ChevronTypes.Prev);
            Checker.CheckEquals(symbolsSortedList[2], positionCardForm.GetChervonSymbol(ChevronTypes.Next), 
                "Next ticker name is not as expected when 2nd position open (after clicking left chevron)");
            Checker.CheckEquals(symbolsSortedList[0], positionCardForm.GetChervonSymbol(ChevronTypes.Prev), 
                "Prev ticker name is not as expected when 2nd position open (after clicking left chevron)");
        }
    }
}