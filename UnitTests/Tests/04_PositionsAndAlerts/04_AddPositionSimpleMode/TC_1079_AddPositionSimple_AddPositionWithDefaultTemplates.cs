using AutomatedTests.Database.Alerts;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Alerts;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.Settings;
using AutomatedTests.Enums.Sorting;
using AutomatedTests.Enums.User;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using TradeStops.Common.Enums;
using TradeStops.Common.Extensions;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._04_PositionsAndAlerts._04_AddPositionSimpleMode
{
    [TestClass]
    public class TC_1079_AddPositionSimple_AddPositionWithDefaultTemplates : BaseTestUnitTests
    {
        private const int TestNumber = 1079;
        private const int RowNumberToAddPosition = 1;

        private PortfolioModel portfolioModel;
        private PositionAtManualCreatingPortfolioModel positionModel;
        private string alertTemplate;
        private string alertDescription;
        private int portfolioId;
        private int alertsQuantity;
        private int expectedTriggerTypeId;

        [TestInitialize]
        public void TestInitialize()
        {
            var userType = GetTestDataParsedAsEnumFromStringMapping<ProductSubscriptionTypes>("User");
            portfolioModel = new PortfolioModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency")
            };
            positionModel = new PositionAtManualCreatingPortfolioModel
            {
                PositionAssetType = GetTestDataParsedAsEnumFromStringMapping<PositionAssetTypes>("positionType"),
                Ticker = GetTestDataAsString("Symbol"),
                TradeType = GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>("TradeType"),
                Quantity = GetTestDataAsString("Shares"),
                EntryDate = DateTimeProvider.GetDate(DateTime.Now, -1).ToShortDateString(),
                EntryPrice = StringUtility.ConvertEmptyStringToNull(GetTestDataAsString("EntryPrice"))
            };
            alertTemplate = GetTestDataAsString(nameof(alertTemplate));
            alertDescription = GetTestDataAsString(nameof(alertDescription));
            expectedTriggerTypeId = alertTemplate.ParseAsEnumFromStringMapping<DefaultTemplateTypes>() == DefaultTemplateTypes.VqTrailingStop
                ? (int)AlertTypes.VqTrailingStop
                : (int)AlertTypes.PercentageTrailingStop;
            alertsQuantity = GetTestDataAsInt(nameof(alertsQuantity));

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, (ProductSubscriptions)(int)userType));
            PortfoliosSetUp.AddInvestmentUsdPortfoliosWithOpenPosition(UserModels.First().Email);
            portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            LoginSetUp.LogIn(UserModels.First());

            new MainMenuNavigation().OpenPositionsGrid();
            new PositionsAlertsStatisticsPanelForm().SelectPortfolio(AllPortfoliosKinds.All.GetStringMapping());
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1079$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test ability adding a position with adding alert from default template. https://tr.a1qa.com/index.php?/cases/view/19232074")]
        [TestCategory("Smoke"), TestCategory("PositionsGrid"), TestCategory("AddPositionPopup"), TestCategory("Alerts"), TestCategory("AlertAdd"), TestCategory("AlertTemplatesGroup")]
        public override void RunTest()
        {
            LogStep(1, "Click 'Add Position' button.");
            var positionsTabForm = new PositionsTabForm();
            positionsTabForm.ClickAddPositionButton();

            LogStep(2, "Select / Enter:- Symbol;-Entry Date: by default;-Entry Price: click 'Get Quote';-Shares: any positive value;-Trade Type: Long;-Entry Commission: by deault");
            var addPositionPopup = new AddPositionInFrameForm();
            addPositionPopup.FillPositionFields(positionModel, RowNumberToAddPosition);
            addPositionPopup.SelectPortfolio(portfolioModel.Name);

            LogStep(3, "Select Alert template.");
            addPositionPopup.SelectTemplate(alertTemplate);
            Checker.CheckEquals(portfolioModel.Name, addPositionPopup.GetSelectedPortfolio(), "Unexpected portfolio is selected");
            Checker.CheckEquals(positionModel.Ticker, addPositionPopup.GetTextFromPositionsAutocompleteDataFields(PositionForManualPortfolioCreateInformation.Ticker, RowNumberToAddPosition), 
                "Unexpected ticker is selected");
            Checker.CheckEquals(positionModel.PositionAssetType, addPositionPopup.GetPositionType(RowNumberToAddPosition), "Asset type is not as expected");
            Checker.CheckEquals(positionModel.TradeType, addPositionPopup.GetSelectedTradeTypeByOrder(RowNumberToAddPosition).ParseAsEnumFromStringMapping<PositionTradeTypes>(),
                "Trade type is not as expected");
            Checker.CheckEquals(positionModel.Quantity, 
                addPositionPopup.GetTextFromPositionsDataFields(PositionForManualPortfolioCreateInformation.Quantity, RowNumberToAddPosition).Replace(",",""),
                "Quantity is not as expected");

            LogStep(4, "Click 'Save and Close' button. Check that added positions shown in grid and has triggered or untriggered alert sign.");
            addPositionPopup.ClickSaveAndClose();
            var positionId = new PositionsQueries().SelectLastAddedPositionId(portfolioId);
            Assert.IsTrue(positionsTabForm.IsPositionPresentInGridById(positionId), "There is no addded position in grid");
            var positionStatus = positionsTabForm.GetValueOfStatusColumnByPositionId(positionId);
            Checker.IsTrue(positionStatus.Alerts.In(StatusOfAlertOnPositionGridStates.UntriggeredAlert, StatusOfAlertOnPositionGridStates.TriggeredAlert), 
                $"Position's alert sign is not as expected: {positionStatus.Alerts}");

            LogStep(5, "Open Positions & Alerts->Alerts tab.Make sure there is expected alert for the position.");
            new MainMenuNavigation().OpenAlertsGrid();
            new PositionsAlertsStatisticsPanelForm().SelectPortfolio(new PortfoliosQueries().SelectPortfolioName(portfolioId));
            var alertsTab = new AlertsTabForm();
            var alertsIds = alertsTab.GetAlertsIdsUsingPositionId(positionId);
            Checker.CheckEquals(alertsQuantity, alertsIds.Count, "There is not only one alert for the position selected on step #3.");
            Checker.CheckEquals(alertDescription, alertsTab.GetColumnValuesByPositionId(AlertsGridColumnsDataField.AlertDescription, positionId).First(), 
                "Alert description not as expected");

            LogStep(6, "In DB: make sure the alert present for the position only one with corresponded TriggerTypeId.");
            var alertsQueries = new AlertsQueries();
            Checker.CheckEquals(alertsQuantity, alertsQueries.SelectAllAlertsIdUsingPositionId(positionId).Count, "There is not only one alert in DB");
            Checker.CheckEquals(expectedTriggerTypeId, Parsing.ConvertToInt(alertsQueries.SelectAlertDataByAlertId(alertsIds[0]).TriggerTypeId), 
                "Alert in DB is not as expected");
        }
    }
}