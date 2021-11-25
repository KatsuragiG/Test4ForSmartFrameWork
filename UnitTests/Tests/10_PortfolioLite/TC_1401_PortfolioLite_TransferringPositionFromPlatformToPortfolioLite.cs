using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Database.Users;
using AutomatedTests.Enums.PortfolioLite;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms.Dashboard;
using AutomatedTests.Forms.PortfolioLite;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Models.PositionsModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.BrowserSteps;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._10_PortfolioLite
{
    [TestClass]
    public class TC_1401_PortfolioLite_TransferringPositionFromPlatformToPortfolioLite : BaseTestUnitTests
    {
        private const int TestNumber = 1401;
        private const int PositionsQuantityToMakeCombined = 2;

        private int finalPositionsQuantity;
        private readonly List<PositionAtManualCreatingPortfolioModel> positionsModels = new List<PositionAtManualCreatingPortfolioModel>();
        private PortfolioLitePositionModel portfolioLitePositionModel;
        private readonly UsersQueries usersQueries = new UsersQueries();

        [TestInitialize]
        public void TestInitialize()
        {
            var userProductSubscriptions = GetUserProductSubscriptions("userSubscription");

            finalPositionsQuantity = GetTestDataAsInt(nameof(finalPositionsQuantity));
            var quantity = GetTestDataAsString("quantity");
            var isTradeTypeLong = GetTestDataAsBool("isTradeTypeLong");
            var entryDate = GetTestDataAsString("entryDate");
            portfolioLitePositionModel = new PortfolioLitePositionModel
            {
                Ticker = GetTestDataAsString("stockPl"),
                BuyDate = entryDate,
                Qty = quantity,
                IsLongType = isTradeTypeLong
            };

            var positionsQuantity = GetTestDataAsInt("positionsQuantity");
            for (int i = 1; i <= positionsQuantity; i++)
            {
                positionsModels.Add(new PositionAtManualCreatingPortfolioModel
                {
                    Ticker = GetTestDataAsString($"symbolsToAdd{i}"),
                    EntryDate = entryDate,
                    Quantity = quantity,
                    TradeType = isTradeTypeLong ? PositionTradeTypes.Long : PositionTradeTypes.Short,
                    PositionAssetType = GetTestDataParsedAsEnumFromStringMapping<PositionAssetTypes>($"assetType{i}")
                });
            }

            LogStep(0, "Preconditions. Create user with subscription to PortfolioLite");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));
            var userModel = UserModels.First();
            var fakeSnaid = StringUtility.RandomString(Constants.SnaidPattern);

            UpdateUserFakeSnaidAndOpenPortfolioLite(userModel.TradeSmithUserId, fakeSnaid);

            var portfolioLiteMainForm = new PortfolioLiteMainForm();
            portfolioLiteMainForm.ClickAddAPosition();
            portfolioLiteMainForm.AddPosition(portfolioLitePositionModel);
            portfolioLiteMainForm.ExitFrame();

            new BrowserSteps().ClearAllDomainCookiesClearStorages();
            usersQueries.ResetUserSnaid(userModel.TradeSmithUserId);
            var mainMenuNavigation = new MainMenuNavigation();
            mainMenuNavigation.OpenLogin();
            LoginSetUp.LogIn(userModel);
            new DashboardForm().AssertIsOpen();

            mainMenuNavigation.OpenPositionsGrid();
            var positionsTab = new PositionsTabForm();
            positionsTab.ClickAddPositionButton();
            var addPositionPopup = new AddPositionInFrameForm();
            var portfolioData = new PortfoliosQueries().SelectPortfoliosDataByUserId(userModel).First();
            addPositionPopup.SelectPortfolio(portfolioData.Name);
            addPositionPopup.FillPositionsFields(positionsModels);
            addPositionPopup.ClickSaveAndClose();

            var positionsQueries = new PositionsQueries();
            var subtradeIds = positionsQueries.SelectPositionIdsForPortfolio(Parsing.ConvertToInt(portfolioData.PortfolioId))
                .Reverse().Take(PositionsQuantityToMakeCombined).Reverse().ToList();
            var positionIdToClose = positionsQueries.SelectPositionIdByUserEmailWithSymbol(userModel.Email, portfolioLitePositionModel.Ticker).First();
            PositionsAlertsSetUp.PartialClosePositionInHalf(positionIdToClose);

            positionsQueries.MakeCombinedPosition(subtradeIds.First(), subtradeIds.Last());
            Browser.Refresh();
            positionsTab.AssertIsOpen();

            TearDowns.LogOut();

            UpdateUserFakeSnaidAndOpenPortfolioLite(userModel.TradeSmithUserId, fakeSnaid);
        }

        private void UpdateUserFakeSnaidAndOpenPortfolioLite(int tradeSmithUserId, string fakeSnaid)
        {
            usersQueries.UpdateUserSnaid(fakeSnaid, tradeSmithUserId);
            new PortfolioLiteNavigation().OpenPortfolioLiteWithUserGuid(UserModels.First());
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1401$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("PortfolioLite")]
        [Description("Test checks that only proper positions (options, combined, closed etc.) created in TSP are shown in PL https://tr.a1qa.com/index.php?/cases/view/21116198")]
        public override void RunTest()
        {
            LogStep(1, "Check that grid contains expected positions");
            var portfolioLiteMainForm = new PortfolioLiteMainForm();
            var actualPositionsQuantity = portfolioLiteMainForm.GetPositionQuantityInGrid();
            Assert.AreEqual(finalPositionsQuantity, actualPositionsQuantity, "Grid does not contain expected added positions");

            LogStep(2, "Check that position cards are openable");
            for (int i = 1; i <= actualPositionsQuantity; i++)
            {
                portfolioLiteMainForm.ClickPositionLinkInGridByNumber(i);
                var portfolioLiteCardForm = new PortfolioLiteCardForm();
                var portfolioLiteChartTabForm = portfolioLiteCardForm.ActivateTabGetForm<PortfolioLiteChartTabForm>(PortfolioLiteCardTabs.Chart);
                portfolioLiteChartTabForm.AssertIsOpen();

                portfolioLiteCardForm.ClickBackToPositions();
            }
        }
    }
}