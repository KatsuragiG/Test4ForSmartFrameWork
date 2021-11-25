using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Events;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Alerts;
using AutomatedTests.Enums.Events;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Models.EventsModels;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Events;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._09_UsersMenu._02_Events
{
    [TestClass]
    public class TC_0918_EventsGrid_DataInTheExportedFileMatchedTheGrid : BaseTestUnitTests
    {
        private const int TestNumber = 918;
        private const string StartDate = "01/01/2010";

        private PortfolioModel portfolioModel;
        private PositionsDBModel positionModelHpq;
        private int expectedNumberOfColumns;
        private string fileName;

        [TestInitialize]
        public void TestInitialize()
        {
            portfolioModel = new PortfolioModel
            {
                Name = GetTestDataAsString("PortfolioName1"),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType1"),
                Currency = GetTestDataAsString("PortfolioCurrency1")
            };
            positionModelHpq = new PositionsDBModel
            {
                Symbol = GetTestDataAsString("Symbol1"),
                TradeType = GetTestDataAsString("TradeType1"),
                PurchaseDate = DateTime.Now.ToShortDateString()
            };
            expectedNumberOfColumns = GetTestDataAsInt(nameof(expectedNumberOfColumns));
            fileName = GetTestDataAsString(nameof(fileName));

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPremium));
            AddPortfolioPositionsAndEvents();
            LoginSetUp.LogIn(UserModels.First());
        }

        private void AddPortfolioPositionsAndEvents()
        {
            var portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            var position1Id = PositionsAlertsSetUp.AddPositionViaDB(portfolioId, positionModelHpq);

            var listOfEventsModels = new List<EventsDBModel>
            {
                new EventsDBModel
                {
                    UserId = UserModels.First().TradeSmithUserId.ToString(),
                    ItemType = $"{(int)AlertTypes.PercentageTrailingStop}",
                    SystemEventCategoryId = $"{(int)EventTypes.AlertCancelled}",
                    PositionId = position1Id.ToString(),
                    ItemName = positionModelHpq.Symbol,
                    IssueDate = "01/01/2017 05:38:45",
                    ThresholdValue = SRandom.Instance.NextDouble().ToString(CultureInfo.InvariantCulture)
                },
                new EventsDBModel
                {
                    UserId = UserModels.First().TradeSmithUserId.ToString(),
                    ItemType = $"{(int)AlertTypes.PercentageTrailingStop}",
                    SystemEventCategoryId = $"{(int)EventTypes.AlertEdited}",
                    PositionId = position1Id.ToString(),
                    ItemName = positionModelHpq.Symbol,
                    IssueDate = DateTimeProvider.GetDate(DateTime.Now,-1).ToShortDateString(),
                    ThresholdValue = SRandom.Instance.NextDouble().ToString(CultureInfo.InvariantCulture)
                },
                new EventsDBModel
                {
                    UserId = UserModels.First().TradeSmithUserId.ToString(),
                    ItemType = $"{(int)AlertTypes.PercentageTrailingStop}",
                    SystemEventCategoryId = $"{(int)EventTypes.AlertCreated}",
                    PositionId = position1Id.ToString(),
                    ItemName = positionModelHpq.Symbol,
                    IssueDate = DateTimeProvider.GetDate(DateTime.Now,-5).ToShortDateString(),
                    ThresholdValue = SRandom.Instance.NextDouble().ToString(CultureInfo.InvariantCulture)
                },
                new EventsDBModel
                {
                    UserId = UserModels.First().TradeSmithUserId.ToString(),
                    ItemType = $"{(int)AlertTypes.PercentageTrailingStop}",
                    SystemEventCategoryId = $"{(int)EventTypes.AlertTriggered}",
                    PositionId = position1Id.ToString(),
                    ItemName = positionModelHpq.Symbol,
                    IssueDate = DateTimeProvider.GetDate(DateTime.Now).ToShortDateString(),
                    ThresholdValue = SRandom.Instance.NextDouble().ToString(CultureInfo.InvariantCulture)
                },
                new EventsDBModel
                {
                    UserId = UserModels.First().TradeSmithUserId.ToString(),
                    ItemType = ((int)PositionAssetTypes.Stock).ToString(),
                    SystemEventCategoryId = $"{(int)EventTypes.DeletePosition}",
                    PositionId = position1Id.ToString(),
                    ItemName = positionModelHpq.Symbol,
                    IssueDate = DateTimeProvider.GetDate(DateTime.Now,0,-1).ToShortDateString()
                },
                new EventsDBModel
                {
                    UserId = UserModels.First().TradeSmithUserId.ToString(),
                    ItemType = ((int)PositionAssetTypes.Stock).ToString(),
                    SystemEventCategoryId = $"{(int)EventTypes.CopyPosition}",
                    PositionId = position1Id.ToString(),
                    ItemName = positionModelHpq.Symbol,
                    IssueDate = "05/01/2017 10:38:45",
                    Text1 = StringUtility.RandomStringOfSize(Constants.DefaultSizeOfDateStringToClearField),
                    Text2 = StringUtility.RandomStringOfSize(Constants.DefaultSizeOfDateStringToClearField)
                },
                new EventsDBModel
                {
                    UserId = UserModels.First().TradeSmithUserId.ToString(),
                    ItemType = ((int)PositionAssetTypes.Stock).ToString(),
                    SystemEventCategoryId = $"{(int)EventTypes.PositionEdited}",
                    PositionId = position1Id.ToString(),
                    ItemName = positionModelHpq.Symbol,
                    IssueDate = "05/01/2018 10:38:45"
                },
                new EventsDBModel
                {
                    UserId = UserModels.First().TradeSmithUserId.ToString(),
                    ItemType = ((int)PositionAssetTypes.Stock).ToString(),
                    SystemEventCategoryId = $"{(int)EventTypes.MovePosition}",
                    PositionId = position1Id.ToString(),
                    ItemName = positionModelHpq.Symbol,
                    IssueDate = DateTimeProvider.GetDate(DateTime.Now,0,-11).ToShortDateString(),
                    Text1 = StringUtility.RandomStringOfSize(Constants.DefaultSizeOfDateStringToClearField),
                    Text2 = StringUtility.RandomStringOfSize(Constants.DefaultSizeOfDateStringToClearField)
                },
                new EventsDBModel
                {
                    UserId = UserModels.First().TradeSmithUserId.ToString(),
                    ItemType = ((int)PositionAssetTypes.Stock).ToString(),
                    SystemEventCategoryId = $"{(int)EventTypes.PartialSell}",
                    PositionId = position1Id.ToString(),
                    ItemName = positionModelHpq.Symbol,
                    IssueDate = DateTime.Parse($"01/01/{DateTime.Now.Year}").ToShortDateString()
                },
                new EventsDBModel
                {
                    UserId = UserModels.First().TradeSmithUserId.ToString(),
                    ItemType = ((int)PositionAssetTypes.Stock).ToString(),
                    SystemEventCategoryId = $"{(int)EventTypes.PurchaseOpen}",
                    PositionId = position1Id.ToString(),
                    ItemName = positionModelHpq.Symbol,
                    IssueDate = DateTimeProvider.GetDate(DateTime.Now,0,-1,-1).ToShortDateString()
                },
                new EventsDBModel
                {
                    UserId = UserModels.First().TradeSmithUserId.ToString(),
                    ItemType = ((int)PositionAssetTypes.Stock).ToString(),
                    SystemEventCategoryId = $"{(int)EventTypes.Sell}",
                    PositionId = position1Id.ToString(),
                    ItemName = positionModelHpq.Symbol,
                    IssueDate = DateTime.Now.ToShortDateString()
                },
                new EventsDBModel
                {
                    UserId = UserModels.First().TradeSmithUserId.ToString(),
                    SystemEventCategoryId = $"{(int)EventTypes.PortfolioCreated}",
                    PortfolioId = portfolioId.ToString(),
                    IssueDate = DateTime.Now.AddMonths(-1).ToShortDateString(),
                    ItemName = StringUtility.RandomStringOfSize(Constants.DefaultSizeOfDateStringToClearField),
                    Text1 = Currency.BTC.ToString(),
                    Decimal1 = SRandom.Instance.NextDouble().ToString(CultureInfo.InvariantCulture)
                },
                new EventsDBModel
                {
                    UserId = UserModels.First().TradeSmithUserId.ToString(),
                    SystemEventCategoryId = $"{(int)EventTypes.PortfolioDeleted}",
                    PortfolioId = portfolioId.ToString(),
                    IssueDate = DateTime.Now.AddDays(-Constants.DigitsQuantityToFillNumericInStringToCompare).ToShortDateString(),
                    ItemName = StringUtility.RandomStringOfSize(Constants.DefaultSizeOfDateStringToClearField),
                    Text1 = Currency.USD.ToString(),
                    Decimal1 = SRandom.Instance.NextDouble().ToString(CultureInfo.InvariantCulture)
                },
                new EventsDBModel
                {
                    UserId = UserModels.First().TradeSmithUserId.ToString(),
                    SystemEventCategoryId = $"{(int)EventTypes.PortfolioEdited}",
                    PortfolioId = portfolioId.ToString(),
                    IssueDate = DateTime.Now.AddDays(-Constants.DefaultSizeOfDateStringToClearField).ToShortDateString(),
                    ItemName = StringUtility.RandomStringOfSize(Constants.DefaultSizeOfDateStringToClearField),
                    Text1 = Currency.HKD.ToString(),
                    Decimal1 = SRandom.Instance.NextDouble().ToString(CultureInfo.InvariantCulture)
                }
             };

            Assert.IsTrue(listOfEventsModels.Count > 0, "Events models list does not contain a event");
            foreach (var model in listOfEventsModels)
            {
                model.PortfolioId = portfolioId.ToString();
                EventsSetUp.AddNewRowIntoSystemEvents(model);
            }
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_918$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("EventHistoryPage"), TestCategory("Export")]
        [Description("The test checks matching data between Events grid and exported csv file. https://tr.a1qa.com/index.php?/cases/view/19232184")]
        public override void RunTest()
        {
            LogStep(1, "Select 'All' in drop-down 'Select a portfolio'.Select 'All' in drop - down 'Position'." +
                "Select 'All' in drop - down 'Event Types'. Select 'Custom Date Range' in drop - down 'Period'." +
                "Select date From '01/01/2015' and date To 'today'");
            var eventsForm = new EventsSteps().OpenEventsSelectPortfolioSetCustomDateRangeFilterFromDateGetEventsForm(portfolioModel.Name, StartDate);

            LogStep(2, "Remember data for 'EVENT HISTORY' grid.");
            var gridEvents = eventsForm.GetAllEvents();
            Assert.IsTrue(gridEvents.Count > 0, "Events grid does not contain a event");

            LogStep(3, "Click export");
            eventsForm.ClickExportButton();

            LogStep(4, "Make sure data in the exported file matched the grid.");
            var path = $"{GetDownloadedFilePathGridDepended()}{fileName}";
            FileUtilsExtension.WaitUntilFileIsDownloaded(path);
            Assert.IsTrue(FileUtilsExtension.IsFileExistGridDepended(path), $"File {path} is not present");
            var eventsCsvData = FileUtilsExtension.ParseCsvIntoObjects<EventsModel>(path, expectedNumberOfColumns);
            FileUtilsExtension.DeleteFileGridDepended(path);
            Assert.AreEqual(gridEvents.Count, eventsCsvData.Count, "Events quantity are not equal");
            foreach (var model in eventsCsvData)
            {
                var unequalValues = new EventsModel().AreFieldsValuesEquals(Dictionaries.EventsColumnsNamesAndObjProperties.Values.ToList(), 
                    model, gridEvents.FirstOrDefault(u => u.EventType == model.EventType));
                Checker.IsTrue(unequalValues.Count == 0, unequalValues.Aggregate("", (current, unequalValue) => current + unequalValue + "\n"));
            }
        }
    }
}