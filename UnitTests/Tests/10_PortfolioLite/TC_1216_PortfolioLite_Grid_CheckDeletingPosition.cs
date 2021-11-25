using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Positions;
using AutomatedTests.Database.Users;
using AutomatedTests.Enums;
using AutomatedTests.Enums.PortfolioLite;
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
    public class TC_1216_PortfolioLite_Grid_CheckDeletingPosition : BaseTestUnitTests
    {

        private const int TestNumber = 1216;

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
            deleteText = string.Format(GetTestDataAsString(nameof(deleteText)), tickerToDelete);

            LogStep(0, "Preconditions. Create user with subscription to PortfolioLite. Add positions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.PortfolioLite));

            var fakeSnaid = StringUtility.RandomString(Constants.SnaidPattern);
            new UsersQueries().UpdateUserSnaid(fakeSnaid, UserModels.First().TradeSmithUserId);
            new PortfolioLiteNavigation().OpenPortfolioLiteWithUserGuid(UserModels.First());

            var portfolioLiteMainForm = new PortfolioLiteMainForm();
            portfolioLiteMainForm.ClickAddAPosition();
            portfolioLiteMainForm.AddPositions(positionsModels);
            portfolioLiteMainForm.ExitFrame();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1216$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("PortfolioLite")]
        [Description("Test checks position can be deleted from position grid https://tr.a1qa.com/index.php?/cases/view/21116209")]
        public override void RunTest()
        {
            LogStep(1, "Expand action dropdown for defined in test data position and click 'Delete'. Make sure expected text and buttons appear.");
            var portfolioLiteMainForm = new PortfolioLiteMainForm();
            Checker.CheckEquals(positionsQuantity, portfolioLiteMainForm.GetPositionQuantityInGrid(),
                "Grid does not contain all added positions");
            portfolioLiteMainForm.ClickActionPositionInGridByNumber(PortfolioLitePositionActionTypes.Delete, deletedPositionNumber);
            Checker.IsTrue(portfolioLiteMainForm.AreConfirmationDeleteButtonsPresent(deletedPositionNumber),
                "Delete confirmation is not as expected");
            Checker.CheckEquals(deleteText, portfolioLiteMainForm.GetInlineTextFromGrid(deletedPositionNumber),
                "Grid does not contain expected warning before position deleting");

            LogStep(2, "Click Cancel.");
            portfolioLiteMainForm.ConfirmDeletionOfPosition(deletedPositionNumber, false);
            Checker.IsFalse(portfolioLiteMainForm.AreConfirmationDeleteButtonsPresent(deletedPositionNumber),
                "Delete confirmation is not as expected");
            Checker.CheckEquals(string.Empty, portfolioLiteMainForm.GetInlineTextFromGrid(deletedPositionNumber),
                "Grid contains warning after canceling of position deleting");

            LogStep(3, "Repeat step 1");
            portfolioLiteMainForm.ClickActionPositionInGridByNumber(PortfolioLitePositionActionTypes.Delete, deletedPositionNumber);
            Checker.IsTrue(portfolioLiteMainForm.AreConfirmationDeleteButtonsPresent(deletedPositionNumber),
                "Delete confirmation is not as expected");

            LogStep(4, "Click 'Yes'");
            portfolioLiteMainForm.ConfirmDeletionOfPosition(deletedPositionNumber, true);
            Checker.CheckEquals(positionsQuantity - 1, portfolioLiteMainForm.GetPositionQuantityInGrid(),
                "Grid contains unexpected positions quantity after deleting");

            LogStep(5, 6, "Check that empty portfolio message is shown according to test data isLastPosition. In DB: make sure the position is marked as deleted");
            new PortfolioLiteSteps().CheckPositionDeleting(portfolioLiteMainForm, tickerToDelete, isLastPosition, UserModels.First());
        }
    }
}
