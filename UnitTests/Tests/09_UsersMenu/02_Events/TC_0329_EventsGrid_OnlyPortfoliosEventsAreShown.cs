using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Events;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Alerts;
using AutomatedTests.Enums.Events;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.User;
using AutomatedTests.Enums;
using AutomatedTests.Forms.Events;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Events;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests.Steps.Settings;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;
using TradeStops.Common.Extensions;

namespace UnitTests.Tests._09_UsersMenu._02_Events
{
    [TestClass]
    public class TC_0329_EventsGrid_OnlyPortfoliosEventsAreShown : BaseTestUnitTests
    {
        private const int TestNumber = 329;

        private readonly List<int> portfolioIds = new List<int>();
        private List<int> eventsQuantities = new List<int>();
        private int step;
        private int eventsInvestQuantity;
        private int eventsWatchQuantity;
        private bool isCreatedCorporatePortfolio;
        private bool isCreatedImportedPortfolio;
        private string alertType;
        private string portfolioNameToUpdateCreds;
        private string startDateForCustomDateRangeFilter;

        [TestInitialize]
        public void TestInitialize()
        {
            var userType = GetTestDataParsedAsEnumFromStringMapping<ProductSubscriptionTypes>("userSubscription");

            eventsInvestQuantity = GetTestDataAsInt(nameof(eventsInvestQuantity));
            eventsWatchQuantity = GetTestDataAsInt(nameof(eventsWatchQuantity));
            isCreatedCorporatePortfolio = GetTestDataAsBool(nameof(isCreatedCorporatePortfolio));
            isCreatedImportedPortfolio = GetTestDataAsBool(nameof(isCreatedImportedPortfolio));
            alertType = GetTestDataAsString(nameof(alertType));
            portfolioNameToUpdateCreds = GetTestDataAsString(nameof(portfolioNameToUpdateCreds));
            startDateForCustomDateRangeFilter = GetTestDataAsString(nameof(startDateForCustomDateRangeFilter));
            eventsQuantities = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(eventsQuantities))
                .Select(Parsing.ConvertToInt).ToList();

            var portfolioName = GetTestDataAsString("PortfolioName1");
            var portfolioCurrency = GetTestDataAsString("Currency");
            var portfoliosDbModels = new List<PortfolioModel>
            {
                new PortfolioModel
                {
                    Name = StringUtility.RandomString(portfolioName),
                    Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType1"),
                    Currency = portfolioCurrency
                },
                new PortfolioModel
                {
                    Name = StringUtility.RandomString(portfolioName),
                    Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType2"),
                    Currency = portfolioCurrency
                }
            };
            var portfolioManualModel = new AddPortfolioManualModel
            {
                Name = StringUtility.RandomString(portfolioName),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType3"),
            };

            var positionsDbModels = new List<PositionsDBModel>
            {
                new PositionsDBModel
                {
                    Symbol = GetTestDataAsString("Symbol1"),
                    TradeType = $"{(int)GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>("TradeType1")}",
                    PurchaseDate = GetTestDataAsString("EntryDate1"),
                    Shares = GetTestDataAsString("Shares1"),
                    StatusType = $"{(int)GetTestDataParsedAsEnumFromStringMapping<AutotestPositionStatusTypes>("PositionType1")}"
                },
                new PositionsDBModel
                {
                    Symbol = GetTestDataAsString("Symbol2"),
                    TradeType = $"{(int)GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>("TradeType2")}",
                    PurchaseDate = GetTestDataAsString("EntryDate2"),
                    Shares = GetTestDataAsString("Shares2"),
                    StatusType = $"{(int)GetTestDataParsedAsEnumFromStringMapping<AutotestPositionStatusTypes>("PositionType2")}",
                    CloseDate = DateTime.Now.AsShortDate()
                }
            };
            var positionsManualModels = new List<PositionAtManualCreatingPortfolioModel>
            {
                new PositionAtManualCreatingPortfolioModel
                {
                    Ticker = GetTestDataAsString("Symbol3"),
                    EntryDate = GetTestDataAsString("EntryDate3"),
                    Quantity = GetTestDataAsString("Shares3"),
                    TradeType = GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>("TradeType3"),
                    PositionAssetType = GetTestDataParsedAsEnumFromStringMapping<PositionAssetTypes>("assetType3")
                },
                new PositionAtManualCreatingPortfolioModel
                {
                    Ticker = GetTestDataAsString("Symbol4"),
                    EntryDate = GetTestDataAsString("EntryDate4"),
                    Quantity = GetTestDataAsString("Shares4"),
                    TradeType = GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>("TradeType4"),
                    PositionAssetType = GetTestDataParsedAsEnumFromStringMapping<PositionAssetTypes>("assetType4")
                }
            };

            LogStep(step, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, (ProductSubscriptions)(int)userType));

            portfolioIds.Add(PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfoliosDbModels.First()));
            portfolioIds.Add(PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfoliosDbModels.Last()));

            PositionsAlertsSetUp.AddPositionViaDB(portfolioIds.First(), positionsDbModels.First());
            PositionsAlertsSetUp.AddPositionViaDB(portfolioIds.First(), positionsDbModels.Last());
            PositionsAlertsSetUp.AddPositionViaDB(portfolioIds.Last(), positionsDbModels.First());
            PositionsAlertsSetUp.AddPositionViaDB(portfolioIds.Last(), positionsDbModels.Last());

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);

            CreateManualPortfolioWithEvents(portfolioManualModel, positionsManualModels);

            GeneratePortfolioEventsViaImport();

            var positionsQueries = new PositionsQueries();
            var positionsIds = new List<int>();
            foreach (var portfolioId in portfolioIds)
            {
                positionsIds.AddRange(positionsQueries.SelectPositionIdsForPortfolio(portfolioId));
            }

            InsertAdditionalEvents(positionsDbModels, positionsIds);

            new MainMenuNavigation().OpenEvents();
            var eventsForm = new EventsForm();
            eventsForm.AssertIsOpen();

            new EventsSteps().SelectCustomPeriodRangeWithStartEndDate(startDateForCustomDateRangeFilter, Parsing.ConvertToShortDateString(DateTime.Now.AsShortDate()));
            eventsForm.SelectPosition(AllPortfoliosKinds.All.GetStringMapping());
            eventsForm.SelectEventType(EventTypes.All);
        }

        private void CreateManualPortfolioWithEvents(AddPortfolioManualModel portfolioManualModel, List<PositionAtManualCreatingPortfolioModel> positionsManualModels)
        {
            if (isCreatedCorporatePortfolio)
            {
                new AddPortfoliosSteps().NavigateToAddManualPortfolioSavePortfolioAfterFillingFields(portfolioManualModel, positionsManualModels);
                portfolioIds.Add(new PortfoliosQueries().SelectPortfolioIdForLastPortfolioByUserEmail(UserModels.First().Email));

                new MainMenuNavigation().OpenPortfolios(portfolioManualModel.Type);
                new PortfoliosForm().SelectPortfolioContextMenuOption(portfolioIds.Last(), PortfolioContextNavigation.DeletePortfolio);
                new PortfolioGridsSteps().ConfirmDeletingPortfoliosCloseSuccessPopup();
            }
        }

        private void GeneratePortfolioEventsViaImport()
        {
            if (isCreatedImportedPortfolio)
            {
                var settingsSteps = new SettingsSteps();
                settingsSteps.NavigateToSettingsAlertsGetForm();
                settingsSteps.SetAlertsForStockOptionWithSaving(true, true, alertType, alertType);

                PortfoliosSetUp.ImportDagSiteInvestment15(true);
                var importedPortfolioId = new PortfoliosQueries().SelectPortfoliosDataByUserId(UserModels.First())
                    .Select(t => Parsing.ConvertToInt(t.PortfolioId))
                    .Except(portfolioIds);
                PortfoliosSetUp.UpdatePortfolioWithDagSiteInvestment20(importedPortfolioId.First());

                portfolioIds.AddRange(importedPortfolioId);
            }
        }

        private void InsertAdditionalEvents(List<PositionsDBModel> positionsDbModels, List<int> positionsIds)
        {
            var eventModelBase = new EventsDBModel
            {
                UserId = UserModels.First().TradeSmithUserId.ToString(),
                ItemName = positionsDbModels.First().Symbol,
                Currency = Currency.USD.GetDescription(),
                ThresholdValue = SRandom.Instance.Next(0, Constants.DefaultContractSize).ToString(),
                ExtremumPrice = SRandom.Instance.Next(0, Constants.DefaultContractSize).ToString(),
                PriceType = ((int)PriceType.Close).ToString(),
                TradeType = SRandom.Instance.Next((int)PositionTradeTypes.Long, (int)PositionTradeTypes.Short).ToString(),
                StopPrice = SRandom.Instance.Next(0, Constants.DefaultContractSize).ToString(),
                Date1 = DateTime.Now.AddDays(-1).AsShortDate(),
                ExtremumDate = DateTime.Now.AddDays(-1).AsShortDate(),
                CurrentValue = SRandom.Instance.Next(0, Constants.DefaultContractSize).ToString(),
                Text1 = StringUtility.RandomStringOfSize(Constants.DefaultSizeOfDateStringToClearField),
                Text2 = StringUtility.RandomStringOfSize(Constants.DefaultSizeOfDateStringToClearField),
                Decimal1 = SRandom.Instance.Next(0, Constants.DefaultContractSize).ToString(),
                Decimal2 = SRandom.Instance.Next(0, Constants.DefaultContractSize).ToString(),
                TinyInt1 = Constants.OrderForStatisticOnPositionCard,
                UseIntraday = Constants.DefaultStringZeroIntValue,
                ItemType = Constants.DefaultStringZeroIntValue,
                IssueDate = DateTime.Now.AddMonths(-1).AsShortDate()
            };

            for (int i = 1; i <= eventsInvestQuantity; i++)
            {
                var currentEvent = eventModelBase.DeepCopy();
                var currentSystemEventCategoryIdInvest = GetTestDataAsString($"systemEventCategoryIdInvest{i}");
                currentEvent.SystemEventCategoryId = currentSystemEventCategoryIdInvest;
                currentEvent.PortfolioId = portfolioIds.First().ToString();
                currentEvent.PositionId = positionsIds.First().ToString();
                if (Parsing.ConvertToInt(currentSystemEventCategoryIdInvest)
                        .In((int)SystemEventCategories.AlertTriggered, (int)SystemEventCategories.AlertCancelled, (int)SystemEventCategories.AlertEdited))
                {
                    currentEvent.IsTriggered = Constants.OrderForStatisticOnPositionCard;
                    currentEvent.ItemType = ((int)AlertTypes.PercentageTrailingStop).ToString();
                }
                EventsSetUp.AddNewRowIntoSystemEvents(currentEvent);
            }
            for (int i = 1; i <= eventsWatchQuantity; i++)
            {
                var currentEvent = eventModelBase.DeepCopy();
                currentEvent.SystemEventCategoryId = GetTestDataAsString($"systemEventCategoryIdWatch{i}");
                currentEvent.PortfolioId = portfolioIds[1].ToString();
                currentEvent.PositionId = positionsIds[2].ToString();
                EventsSetUp.AddNewRowIntoSystemEvents(currentEvent);
            }
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_329$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("EventHistoryPage"), TestCategory("SyncPortfolio")]
        [Description("The test checks that events are shown only for selected portfolio. https://tr.a1qa.com/index.php?/cases/view/22301733")]
        public override void RunTest()
        {
            LogStep(++step, "Select the Investment portfolio {step 1 of precondition} from the portfolios dropdown and check displaying expected quantity of Events.");
            SelectPortfolioAndCheckEventsQuantity(step, portfolioIds.First(), eventsQuantities.First());

            LogStep(++step, "Select the Watch Only portfolio {step 2 of precondition} from the portfolios dropdown and check displaying expected quantity of Events.");
            SelectPortfolioAndCheckEventsQuantity(step, portfolioIds[1], eventsQuantities[1]);

            LogStep(++step, "Select removed portfolio and check displaying expected quantity of Events if isCreatedCorporatePortfolio");
            if (isCreatedCorporatePortfolio)
            {
                SelectPortfolioAndCheckEventsQuantity(step, portfolioIds[2], eventsQuantities[2]);
            }

            LogStep(++step, "Select Import portfolio  and check displaying expected quantity of Events if isCreatedImportedPortfolio");
            if (isCreatedImportedPortfolio)
            {
                var portoflioId = new PortfoliosQueries().SelectPortfolioDataByPortfolioNameUserModel(portfolioNameToUpdateCreds, UserModels.First()).PortfolioId;
                SelectPortfolioAndCheckEventsQuantity(step, Parsing.ConvertToInt(portoflioId), eventsQuantities[3]);
            }
        }

        private void SelectPortfolioAndCheckEventsQuantity(int step, int portfolioId, int expectedEventsQuantity)
        {
            var eventsForm = new EventsForm();
            eventsForm.SelectPortfolioById(portfolioId);
            Checker.CheckEquals(expectedEventsQuantity, eventsForm.GetNumberOfEvents(), $"Step {step} portfolio has unexpected Events Quantity");
        }
    }
}