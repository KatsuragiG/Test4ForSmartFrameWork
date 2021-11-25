using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Positions;
using AutomatedTests.Database.Users;
using AutomatedTests.Enums;
using AutomatedTests.Forms.Popups;
using AutomatedTests.Forms.PortfolioLite;
using AutomatedTests.Models.PositionsModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.PortfolioLiteSteps;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._10_PortfolioLite
{
    [TestClass]
    public class TC_1397_PositionCard_CheckDeletingPosition : BaseTestUnitTests
    {

        private const int TestNumber = 1397;

        private int positionsQuantity;
        private int deletedPositionNumber;
        private string deleteText;
        private string tickerToDelete;
        private bool isLastPosition;

        [TestInitialize]
        public void TestInitialize()
        {
            positionsQuantity = GetTestDataAsInt(nameof(positionsQuantity));
            deletedPositionNumber = GetTestDataAsInt(nameof(deletedPositionNumber));
            deleteText = GetTestDataAsString(nameof(deleteText));
            isLastPosition = GetTestDataAsBool(nameof(isLastPosition));

            var positionsQueries = new PositionsQueries();
            var positionsModels = new List<PortfolioLitePositionModel>();
            for (int i = 1; i <= positionsQuantity; i++)
            {
                var entryDate = GetTestDataAsString($"entryDate{i}");
                var entryPrice = GetTestDataAsString($"entryPrice{i}");
                var quantity = GetTestDataAsString($"quantities{i}");
                positionsModels.Add(new PortfolioLitePositionModel
                {
                    Ticker = GetTestDataAsString($"symbolsToAdd{i}"),
                    BuyDate = string.IsNullOrEmpty(entryDate) ? null : entryDate,
                    BuyPrice = string.IsNullOrEmpty(entryPrice) ? null : entryPrice,
                    Qty = string.IsNullOrEmpty(quantity) ? null : quantity,
                    IsLongType = GetTestDataAsBool($"IsLongType{i}")
                });
                positionsModels.Last().Currency = (Currency)positionsQueries.SelectSymbolCurrencyBySymbol(positionsModels.Last().Ticker);
            }
            tickerToDelete = positionsModels[deletedPositionNumber - 1].Ticker;

            LogStep(0, "Preconditions. Create user with subscription to PortfolioLite. Add positions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.PortfolioLite));

            var fakeSnaid = StringUtility.RandomString(Constants.SnaidPattern);
            new UsersQueries().UpdateUserSnaid(fakeSnaid, UserModels.First().TradeSmithUserId);
            new PortfolioLiteNavigation().OpenPortfolioLiteWithUserGuid(UserModels.First());

            var portfolioLiteMainForm = new PortfolioLiteMainForm();
            portfolioLiteMainForm.ClickAddAPosition();
            portfolioLiteMainForm.AddPositions(positionsModels);
            Assert.AreEqual(positionsQuantity, portfolioLiteMainForm.GetPositionQuantityInGrid(), "Positions are not added fully to grid");
            portfolioLiteMainForm.ClickPositionLinkInGridByNumber(deletedPositionNumber);
            portfolioLiteMainForm.ExitFrame();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1397$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("PortfolioLite")]
        [Description("Test checks position can be deleted from position card https://tr.a1qa.com/index.php?/cases/view/22153210")]
        public override void RunTest()
        {
            LogStep(1, "click 'Delete'. Make sure expected popup with text appears.");
            var portfolioLiteMainForm = new PortfolioLiteMainForm();
            var portfolioLiteCardForm = new PortfolioLiteCardForm();
            portfolioLiteCardForm.ClickDelete();
            var popupForm = new ConfirmPopup(PopupNames.Confirm);
            Checker.CheckContains(deleteText, popupForm.GetMessage(), "Popup window text doesn't contain expected text");

            LogStep(2, "Click No.");
            popupForm.ClickNoButton();
            portfolioLiteCardForm.AssertIsOpen();

            LogStep(3, "Click back to positions.");
            portfolioLiteCardForm.ClickBackToPositions();
            Checker.CheckEquals(positionsQuantity, portfolioLiteMainForm.GetPositionQuantityInGrid(), "Positions are not fully shown in grid");

            LogStep(4, "Click on the defined in test data position, click 'Delete'.");
            portfolioLiteMainForm.ClickPositionLinkInGridByNumber(deletedPositionNumber);
            portfolioLiteCardForm.AssertIsOpen();
            portfolioLiteCardForm.ClickDelete();

            LogStep(5, "Click Yes.");
            popupForm.AssertIsOpen();
            popupForm.ClickYesButton();
            portfolioLiteCardForm.AssertIsClosed();
            Checker.CheckEquals(positionsQuantity - 1, portfolioLiteMainForm.GetPositionQuantityInGrid(),
                "Grid contains unexpected positions quantity after deleting");

            LogStep(6, 7, "Check that empty portfolio message is shown according to test data isLastPosition. In DB: make sure the position is marked as deleted");
            new PortfolioLiteSteps().CheckPositionDeleting(portfolioLiteMainForm, tickerToDelete, isLastPosition, UserModels.First());
        }
    }
}
