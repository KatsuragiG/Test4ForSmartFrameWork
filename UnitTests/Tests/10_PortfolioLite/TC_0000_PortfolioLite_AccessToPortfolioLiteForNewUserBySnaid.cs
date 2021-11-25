using AutomatedTests;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Users;
using AutomatedTests.Enums.PortfolioLite;
using AutomatedTests.Enums.User;
using AutomatedTests.Forms.PortfolioLite;
using AutomatedTests.Models.UserModels;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Constants;
using TradeStops.Common.Enums;
using TradeStops.Common.Helpers;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._10_PortfolioLite
{
    [TestClass]
    public class TC_0000_PortfolioLite_AccessToPortfolioLiteForNewUserBySnaid : BaseTestUnitTests
    {
        private const int LiteSubscriptionRecordsQuantity = 1;
        private const int QuantityOfAddedPositions = 1;

        private int expectedSumOfUserProductFeatures;
        private string portfolioLiteEmail;
        private string portfolioLiteSnaid;
        private int portfolioLiteSubscriptionExpirationYears;
        private TradeSmithUserDBModel tradeSmithUserDbModel;
        private readonly UsersQueries usersQueries = new UsersQueries();

        [TestInitialize]
        public void TestInitialize()
        {
            portfolioLiteEmail = GetTestDataAsString(nameof(portfolioLiteEmail));
            portfolioLiteSnaid = GetTestDataAsString(nameof(portfolioLiteSnaid));
            expectedSumOfUserProductFeatures = GetTestDataAsInt(nameof(expectedSumOfUserProductFeatures));
            portfolioLiteSubscriptionExpirationYears = GetTestDataAsInt(nameof(portfolioLiteSubscriptionExpirationYears));

            LogStep(0, "Preconditions. Check that TradeSmith DB does not contains user with email from test data. If user exist - delete it. If deleting causes error - stop test via Fail");
            tradeSmithUserDbModel = usersQueries.SelectTradeSmithUserFromMasterDBByUserEmail(portfolioLiteEmail);
            if (tradeSmithUserDbModel != null)
            {
                Log.Info($"User {portfolioLiteEmail} was found in Db. Making attempt to delete");
                var existedUserModel = new UserModel
                {
                    Email = portfolioLiteEmail,
                    TradeSmithUserId = Parsing.ConvertToInt(tradeSmithUserDbModel.TradeSmithUserId),
                    SubscriptionType = new List<ProductSubscriptionTypes> { ProductSubscriptionTypes.PortfolioLiteInvestorPlaceMedia }
                };
                existedUserModel.UserProductSubscriptionsIds = usersQueries.SelectUserProductSubscriptions(existedUserModel.TradeSmithUserId).Select(t => t.UserProductSubscriptionId).ToList();
                TearDowns.DeleteUserViaApi(new UserModel().FillUserModelFromDb(tradeSmithUserDbModel));

                tradeSmithUserDbModel = usersQueries.SelectTradeSmithUserFromMasterDBByUserEmail(portfolioLiteEmail);
                if (tradeSmithUserDbModel != null)
                {
                    Assert.Fail($"Can not delete user {portfolioLiteEmail}");
                }
            }

            new PortfolioLiteNavigation().OpenPortfolioLiteWithSnaidId(portfolioLiteSnaid, EnumDisplayNamesHelper.Get(PublisherTypes.InvestorPlaceMedia));
            var portfolioLiteMainForm = new PortfolioLiteMainForm();
            portfolioLiteMainForm.ExitFrame();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_0000$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("PortfolioLite")]
        [Description("Test checks that all pages are available for the new user from S&A. https://tr.a1qa.com/index.php?/cases/view/21116206")]
        public override void RunTest()
        {
            LogStep(1, "Check that TradeSmith.dbo.TradeSmithUsers contains user record with correct email and snaid");
            tradeSmithUserDbModel = usersQueries.SelectTradeSmithUserFromMasterDBByUserEmail(portfolioLiteEmail);
            Checker.IsTrue(tradeSmithUserDbModel != null, $"User was not created in MasterDb for email {portfolioLiteEmail} and snaid {portfolioLiteSnaid}");
            Checker.CheckEquals(portfolioLiteSnaid, tradeSmithUserDbModel.Snaid, $"User with email {portfolioLiteEmail} does not have correct snaid");
            var tradeSmithUserId = Parsing.ConvertToInt(tradeSmithUserDbModel.TradeSmithUserId);
            UserModels.Add(new UserModel().FillUserModelFromDb(tradeSmithUserDbModel));

            LogStep(2, "Check that for user's tradesmithUserid there is a record in UserProductSubscriptions");
            var userProductSubscriptions = usersQueries.SelectUserProductSubscriptions(tradeSmithUserId);
            Checker.IsTrue(userProductSubscriptions.Any(), "User has null in UserProductSubscriptions");
            Checker.CheckEquals(LiteSubscriptionRecordsQuantity, userProductSubscriptions.Count, "User has unexpected records in UserProductSubscriptions");

            var expectedStartDate = Parsing.ConvertToShortDateString(DateTime.Now.AsShortDate());
            var expectedExpirationDate = Parsing.ConvertToShortDateString(DateTime.Now.AddYears(portfolioLiteSubscriptionExpirationYears).AsShortDate());
            var portfolioLiteUserProductSubscriptions = userProductSubscriptions.First();
            Checker.CheckEquals((int)ProductSubscriptions.PortfolioLiteInvestorPlaceMedia,
                Parsing.ConvertToInt(portfolioLiteUserProductSubscriptions.ProductSubscriptionId),
                "User has unexpected ProductSubscriptionId in UserProductSubscriptions");
            Checker.CheckContains(expectedStartDate, Parsing.ConvertToShortDateString(portfolioLiteUserProductSubscriptions.StartDate),
                "PL subscription start date is not as expected");
            Checker.CheckContains(expectedExpirationDate, Parsing.ConvertToShortDateString(portfolioLiteUserProductSubscriptions.ExpirationDate),
                "PL subscription expiration date is not as expected");
            Checker.CheckEquals((int)SyncronizationSources.TradeStops,
                Parsing.ConvertToInt(portfolioLiteUserProductSubscriptions.SyncSourceId),
                "User has unexpected SyncSourceId in UserProductSubscriptions");
            Checker.CheckEquals((int)SubscriptionStatuses.Active,
                Parsing.ConvertToInt(portfolioLiteUserProductSubscriptions.SubscriptionStatusId),
                "User has unexpected SubscriptionStatusId in UserProductSubscriptions");

            LogStep(3, "Check that user has 1 only in Positions feature in TradeSmit.dbo.UserFeatures");
            var userProductFeatures = usersQueries.GetUserProductFeatures(tradeSmithUserId);
            var sumOfUserProductFeatures = userProductFeatures.GetType().GetProperties()
                .Where(x => x.PropertyType == typeof(int) && x.Name != nameof(userProductFeatures.TradeSmithUserId))
                .Sum(x => (int)x.GetValue(userProductFeatures));

            Checker.CheckEquals(expectedSumOfUserProductFeatures, sumOfUserProductFeatures,
                 "User has unexpected sum Of User Product Features");
            Checker.CheckEquals((int)UserStatuses.Active, userProductFeatures.Positions,
                "User has unexpected Positions Features");

            LogStep(4, "Check that portfolioLiteMainForm is shown");
            var portfolioLiteMainForm = new PortfolioLiteMainForm();

            LogStep(5, "Click Add positions");
            portfolioLiteMainForm.ClickAddAPosition();
            Checker.IsTrue(portfolioLiteMainForm.IsAddPositionBlockPresent(), "Add position Block is NOT shown before adding a position");

            LogStep(6, "Select a ticker from autocomplete and click save");
            var ticker = new CustomTestDataReader().GetDefaultTicker();
            portfolioLiteMainForm.SetSymbol(ticker, QuantityOfAddedPositions);
            portfolioLiteMainForm.ClickSave();
            Checker.CheckEquals(QuantityOfAddedPositions, portfolioLiteMainForm.GetPositionQuantityInGrid(),
                "Grid does not contain only the added position");

            LogStep(7, "Check in DB that created in TS portfolio has correct PortfolioLitePartnerId");
            var portfoliosQueries = new PortfoliosQueries();
            var portfolioId = portfoliosQueries.SelectPortfolioIdForLastPortfolioByUserEmail(UserModels.First().Email);
            Assert.IsTrue(portfolioId != 0, "PortfolioId for the newly created portfolio is 0");
            var portfolioData = portfoliosQueries.SelectPortfolioDataByPortfolioId(portfolioId);
            Checker.IsTrue(portfolioData != null, "Portfolio data for the newly created portfolio does not have values");
            Checker.CheckEquals(PortfolioLitePartnerIds.InvestorPlaceMedia,
                Parsing.ConvertToInt(portfolioData.PortfolioLitePartnerId),
                "Portfolio data for the newly created portfolio does not have values");

            LogStep(8, "Click more details");
            portfolioLiteMainForm.ClickPortfolioSummary();
            var portfolioLiteDetailsForm = new PortfolioLiteDetailsForm();
            portfolioLiteDetailsForm.AssertIsOpen();

            LogStep(9, "Click Less details");
            portfolioLiteMainForm.ClickPortfolioSummary();
            Checker.IsFalse(portfolioLiteDetailsForm.Chart.IsChartAreaExist(), "Chart area is shown after clicking Less details");

            LogStep(10, "Click on link for the created position in grid");
            portfolioLiteMainForm.ClickPositionLinkInGridByNumber(QuantityOfAddedPositions);
            var portfolioLiteCardForm = new PortfolioLiteCardForm();
            portfolioLiteCardForm.AssertIsOpen();

            LogStep(11, "Type in search field a ticker and select it from autocomplete");
            portfolioLiteMainForm.SearhSymbol(ticker);
            var portfolioLiteAnalyzerForm = new PortfolioLiteAnalyzerForm();
            portfolioLiteAnalyzerForm.AssertIsOpen();

            LogStep(12, "Click Add to portfolio");
            portfolioLiteAnalyzerForm.ClickAdditionalActionsButton(PortfolioLiteAdditionalButtons.AddToPortfolio);
            var addToPortfolioForm = new PortfolioLiteAddToPortfolioForm();
            addToPortfolioForm.AssertIsOpen();

            LogStep(13, "Repeat steps 11");
            portfolioLiteMainForm.SearhSymbol(ticker);

            LogStep(14, "Click Position Size");
            portfolioLiteAnalyzerForm.ClickAdditionalActionsButton(PortfolioLiteAdditionalButtons.PositionSize);
            new PortfolioLitePositionSizeForm().AssertIsOpen();
        }
    }
}