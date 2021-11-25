using System;
using System.Collections.Generic;
using System.Linq;
using AutomatedTests.Database.Alerts;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Alerts;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.Settings;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Extensions;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._04_PositionsAndAlerts._04_AddPositionSimpleMode
{
    [TestClass]
    public class TC_1026_AddPositionSimple_AddPositionWithTrailingStopAlert : BaseTestUnitTests
    {
        private const int TestNumber = 1026;
        
        private readonly List<int> portfoliosIds = new List<int>();
        private readonly List<PositionAtManualCreatingPortfolioModel> addPositionsSimpleModels = new List<PositionAtManualCreatingPortfolioModel>();
        private Dictionary<string, int> alertsInDefaultTemplates;

        [TestInitialize]
        public void TestInitialize()
        {
            var userProductSubscriptions = GetUserProductSubscriptions("User");

            var symbolsQuantity = GetTestDataAsInt("symbolsQuantity");
            var symbols = new List<string>();
            for (int i = 0; i < symbolsQuantity; i++)
            {
                symbols.Add(GetTestDataAsString($"Symbol{i + 1}"));
            }

            var portfolioName = GetTestDataAsString("PortfolioName");
            var portfolioCurrency = $"{(int)GetTestDataParsedAsEnumFromStringMapping<Currency>("Currency")}";
            var portfolioModelInvest = new PortfolioDBModel
            {
                Name = StringUtility.RandomString(portfolioName),
                Type = ((int)PortfolioType.Investment).ToString(),
                CurrencyId = portfolioCurrency,
                EntryCommission = GetTestDataAsString("Commission1")
            };
            var portfolioModelWatch = new PortfolioDBModel
            {
                Name = StringUtility.RandomString(portfolioName),
                Type = ((int)PortfolioType.WatchOnly).ToString(),
                CurrencyId = portfolioCurrency,
                EntryCommission = GetTestDataAsString("Commission2")
            };
            var shares = GetTestDataAsString("Shares");
            addPositionsSimpleModels.Add(new PositionAtManualCreatingPortfolioModel
            {
                Ticker = symbols[1],
                Quantity = shares,
                TradeType = PositionTradeTypes.Long,
                PositionAssetType = GetTestDataAsString("assetType2").ParseAsEnumFromStringMapping<PositionAssetTypes>(),
                EntryDate = DateTimeProvider.GetDate(DateTime.Now, 0, 0, -1).AsShortDate()
            });
            addPositionsSimpleModels.Add(new PositionAtManualCreatingPortfolioModel
            {
                Ticker = symbols[2],
                Quantity = shares,
                TradeType = PositionTradeTypes.Short,
                PositionAssetType = GetTestDataAsString("assetType3").ParseAsEnumFromStringMapping<PositionAssetTypes>(),
                EntryDate = DateTimeProvider.GetDate(DateTime.Now, 0, 0, -1).AsShortDate()
            });
            addPositionsSimpleModels.Add(new PositionAtManualCreatingPortfolioModel
            {
                Ticker = symbols[3],
                Quantity = shares,
                TradeType = PositionTradeTypes.Long,
                PositionAssetType = GetTestDataAsString("assetType4").ParseAsEnumFromStringMapping<PositionAssetTypes>(),
                EntryDate = DateTimeProvider.GetDate(DateTime.Now, 0, 0, -1).AsShortDate()
            });
            alertsInDefaultTemplates = new Dictionary<string, int>
            {
                {DefaultTemplateTypes.TrailingStop15.GetStringMapping(), Parsing.ConvertToInt(DefaultTemplateTypes.TrailingStop15.GetDescription())},
                {DefaultTemplateTypes.TrailingStop20.GetStringMapping(), Parsing.ConvertToInt(DefaultTemplateTypes.TrailingStop20.GetDescription())},
                {DefaultTemplateTypes.TrailingStop25.GetStringMapping(), Parsing.ConvertToInt(DefaultTemplateTypes.TrailingStop25.GetDescription())}
            };

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));
            portfoliosIds.Add(PortfoliosSetUp.AddPortfolioViaDb(UserModels.First(), portfolioModelInvest));
            portfoliosIds.Add(PortfoliosSetUp.AddPortfolioViaDb(UserModels.First(), portfolioModelWatch));
            PositionsAlertsSetUp.AddPositionViaDB(portfoliosIds[0], new PositionsDBModel { Symbol = symbols[0] });

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
            new MainMenuNavigation().OpenPositionsGrid();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1026$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PositionsGrid"), TestCategory("AddPositionPopup"), TestCategory("Alerts"), TestCategory("AlertAdd"), TestCategory("TrailingStopAlertsGroup")]
        [Description("The test ability adding a Stock position (Long and Short) with selecting 'Trailing Stop' alert. https://tr.a1qa.com/index.php?/cases/view/19232075")]
        public override void RunTest()
        {
            DoSteps1To10(portfoliosIds[0], addPositionsSimpleModels[0]);
            DoSteps1To10(portfoliosIds[1], addPositionsSimpleModels[1]);
            DoSteps1To10(portfoliosIds[0], addPositionsSimpleModels[2]);
        }

        private void DoSteps1To10(int portfolioId, PositionAtManualCreatingPortfolioModel addPositionPopupModel)
        {
            var mainMenuNavigation = new MainMenuNavigation();
            var positionsAlertsStatisticsPanelForm = new PositionsAlertsStatisticsPanelForm();
            var alertsQueries = new AlertsQueries();
            var positionsQueries = new PositionsQueries();
            var samePositionsQuantity = 1;
            foreach (var alert in alertsInDefaultTemplates)
            {
                LogStep(1, "Select the Investment portfolio. Click 'Add Position' button.");
                var positionsTab = new PositionsTabForm();
                positionsAlertsStatisticsPanelForm.SelectPortfolioById(portfolioId);
                positionsTab.ClickAddPositionButton();

                LogStep(2, 3, "Enter and select from autocomplete symbol with $ currency");
                var addPositionPopup = new AddPositionInFrameForm();
                addPositionPopup.FillPositionFields(addPositionPopupModel, 1);
                addPositionPopup.SelectTemplate(alert.Key);

                LogStep(4, "Click 'Save and Close' button.");
                addPositionPopup.ClickSaveAndClose();
                var positionId = positionsQueries.SelectLastAddedPositionId(portfolioId);
                var ticker = addPositionPopupModel.PositionAssetType == PositionAssetTypes.Option
                    ? positionsQueries.SelectSymbolByPositionId(positionId)
                    : addPositionPopupModel.Ticker;
                var tickerList = positionsTab.GetPositionColumnValues(PositionsGridDataField.Ticker).Select(t => t.Split('\r')[0]).ToList();
                Checker.IsTrue(tickerList.Contains(ticker), $"Added ticker is not shown in Position grid for {ticker}");
                Checker.CheckEquals(tickerList.Count(t => t.EqualsIgnoreCase(ticker)), samePositionsQuantity, 
                    $"Added ticker is not shown in Position grid for {ticker} {samePositionsQuantity} times");

                LogStep(5, "Open Positions & Alerts -> Alerts tab. Make sure there is no alert for the position. In DB: make sure the alert present for the position and % value match expectation.");
                mainMenuNavigation.OpenAlertsGrid();
                positionsAlertsStatisticsPanelForm.SelectPortfolioById(portfolioId);
                var alertsTabForm = new AlertsTabForm();
                tickerList = alertsTabForm.GetColumnValues(AlertsGridColumnsDataField.Ticker).Select(t => t.Split('\r')[0]).ToList();
                Checker.IsTrue(tickerList.ToList().Contains(ticker), $"Added ticker is not shown in Alerts grid for {ticker}");
                Checker.CheckEquals(tickerList.Count(t => t.EqualsIgnoreCase(ticker)), samePositionsQuantity, 
                    $"Added ticker is not shown in Alert grid for {ticker} {samePositionsQuantity} times");

                var alertsIds = alertsQueries.SelectAllAlertsIdUsingPositionId(positionId);
                var alertData = alertsQueries.SelectAlertDataByAlertId(alertsIds[0]);
                Checker.CheckEquals((int)AlertTypes.PercentageTrailingStop, Parsing.ConvertToInt(alertData.TriggerTypeId),
                    $"Db alert id is not as expected for {ticker}");
                Checker.CheckEquals((double)alert.Value, Parsing.ConvertToDouble(alertData.ThresholdValue),
                    $"Db alert threshold value is not as expected for {ticker}");
                Checker.CheckEquals(1, alertsIds.Count, $"Db there is more than 1 alert for the position for {ticker}");
                samePositionsQuantity++;
                mainMenuNavigation.OpenPositionsGrid();
            }
            new PositionsTabForm().AssertIsOpen();
        }
    }
}