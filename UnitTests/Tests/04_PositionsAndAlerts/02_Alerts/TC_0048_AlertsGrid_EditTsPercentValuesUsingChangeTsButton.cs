using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Alerts;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Alerts;
using AutomatedTests.Enums.Portfolios.CreateManual;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms.Popups;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests.Steps.PositionCard;
using AutomatedTests.Steps.PositionsGridSteps;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Enums;
using TradeStops.Common.Extensions;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._04_PositionsAndAlerts._02_Alerts
{
    [TestClass]
    public class TC_0048_AlertsGrid_EditTsPercentValuesUsingChangeTsButton : BaseTestUnitTests
    {
        private const int TestNumber = 48;

        private AddPortfolioManualModel portfolioModel;
        private readonly List<PositionAtManualCreatingPortfolioModel> positionsModels = new List<PositionAtManualCreatingPortfolioModel>();
        private string alertDescription;
        private int newTrailingStopValue;
        private int portfolioId;

        [TestInitialize]
        public void TestInitialize()
        {
            portfolioModel = new AddPortfolioManualModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency"),
            };

            var positionsQuantity = GetTestDataAsInt("positionsQuantity");
            var shares = GetTestDataAsString("shares");
            for (int i = 1; i <= positionsQuantity; i++)
            {
                positionsModels.Add(new PositionAtManualCreatingPortfolioModel
                {
                    Ticker = GetTestDataAsString($"ticker{i}"),
                    EntryDate = GetTestDataAsString($"entryDate{i}"),
                    Quantity = shares,
                    TradeType = GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>($"tradeType{i}"),
                    PositionAssetType = GetTestDataParsedAsEnumFromStringMapping<PositionAssetTypes>($"positionType{i}")
                });
            }

            alertDescription = GetTestDataAsString(nameof(alertDescription));
            newTrailingStopValue = GetTestDataAsInt(nameof(newTrailingStopValue));

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, new List<ProductSubscriptions> {
                ProductSubscriptions.TradeStopsBasic,
                ProductSubscriptions.CryptoStopsBasic
            }));

            LoginSetUp.LogIn(UserModels.First());

            new AddPortfoliosSteps().NavigateToAddManualPortfolioSavePortfolioAfterFillingFields(portfolioModel, positionsModels);
            var addAlertsAtCreatingPortfolioSteps = new AddAlertsAtCreatingPortfolioSteps();
            addAlertsAtCreatingPortfolioSteps.SetAlertSliderWithChecking(AlertsToPositionsAtPortfolioCreation.TrailingStop, AlertsToPositionsStates.On);
            addAlertsAtCreatingPortfolioSteps.SetAlertSliderWithChecking(AlertsToPositionsAtPortfolioCreation.VqTrailingStop, AlertsToPositionsStates.On);
            new AddAlertsAtCreatingPortfolioForm().ClickActionButton(AddAlertsAtCreatingPortfolioButtons.AddAlerts);

            portfolioId = new PortfoliosQueries().SelectPortfolioIdForLastPortfolioByUserEmail(UserModels.First().Email);
            var positionsQueries = new PositionsQueries();
            var positionsIds = positionsQueries.SelectPositionIdsForPortfolio(portfolioId);
            var cryptoIds = positionsIds.Where(id => positionsQueries.SelectAssetTypeNameByPositionId(id).EqualsIgnoreCase(PositionAssetTypes.Crypto.GetStringMapping())).ToList();

            Checker.IsTrue(cryptoIds.Any(), "No crypto positions were added");
            var alertTabPositionCardSteps = new AlertTabPositionCardSteps();
            foreach (var cryptoId in cryptoIds)
            {
                alertTabPositionCardSteps.GetAddAlertsFormByAlertCategory(cryptoId, AlertCategoryTypes.TrailingStop);
                alertTabPositionCardSteps.AddIntradayTrailingStopAlert(TrailingStopAlertTypes.Ts);
                alertTabPositionCardSteps.AddIntradayTrailingStopAlert(TrailingStopAlertTypes.Vq);
            }

            new MainMenuNavigation().OpenAlertsGrid();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_48$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("AlertsGrid"), TestCategory("AlertsGridBulkActionButtons"), TestCategory("Alerts"), TestCategory("AlertEdit"), TestCategory("TrailingStopAlertsGroup")]
        [Description("The test checks bulk changing Trailing Stops percent alerts at Alerts Grid https://tr.a1qa.com/index.php?/cases/view/20746650")]

        public override void RunTest()
        {
            LogStep(1, "Remember AlertsIds and Intraday AlertsIds. Select added portfolio in Portfolio dropdown. Select all listed alerts");
            new PositionsAlertsStatisticsPanelForm().SelectPortfolio(portfolioModel.Name);
            var alertsTabForm = new AlertsTabForm();
            var alertsIds = alertsTabForm.GetAllAlertsId(UserModels.First().TradeSmithUserId);
            Assert.IsTrue(alertsIds.Any(), "No alert Ids in DB before changing TS percent");
            var alertsQueries = new AlertsQueries();
            var intradayAlertsIds = alertsIds.Where(a => Parsing.ConvertToBool(alertsQueries.SelectAlertDataByAlertId(a).UseIntraday)).ToList();
            Checker.IsTrue(intradayAlertsIds.Any(), "No intraday alert Ids in DB before changing TS percent");

            alertsTabForm.SelectAllItemsInGrid();
            Checker.IsTrue(alertsTabForm.IsSelectAllChecked(), "All alerts are not checked");

            LogStep(2, "Click On Change TS Buttom");
            alertsTabForm.ClickGroupActionButton(AlertsGroupAction.ChangeTsPercent);
            var changeTsPopup = new ChangeTsPopup();

            LogStep(3, "Set new TS% value");
            changeTsPopup.SetNewTrailingStopPercent(newTrailingStopValue);
            Checker.CheckEquals(newTrailingStopValue.ToString(), changeTsPopup.GetTrailingStopPercent(), "New Ts value is NOT shown in popup");

            LogStep(4, "Click Ok Button");
            changeTsPopup.ClickOkButton();
            new ConfirmPopup(PopupNames.Success).ClickOkButton();
            alertsTabForm.AssertIsOpen();

            LogStep(5, "Check Alert descriptions");
            var newAlertDescriptions = new AlertsGridSteps().GetAllAlertsDescriptionFromAlertGrid();
            Assert.IsTrue(newAlertDescriptions.Any(), "No alerts descriptions after changing TS percent");
            foreach (var newAlertDescription in newAlertDescriptions)
            {
                if (!newAlertDescription.ToLower().Contains(TrailingStopAlertTypes.Vq.GetStringMapping().ToLower()))
                {
                    Checker.CheckContains(alertDescription, newAlertDescription, "TS%  Alerts description is not as expected");
                }
                else
                {
                    Checker.CheckContains(AlertTypes.VqTrailingStop.GetStringMapping(), newAlertDescription, "Vq Alerts description is not as expected");
                }
            }

            LogStep(6, "Check Alerts in DB");
            var alertsIdsAfterChanging = alertsTabForm.GetAllAlertsId(UserModels.First().TradeSmithUserId);
            Checker.CheckEquals(alertsIds.Count, alertsIdsAfterChanging.Count, "Count of alerts in DB is not as expected");
            foreach (var alertId in alertsIdsAfterChanging)
            {
                var alertDbModel = alertsQueries.SelectAlertDataByAlertId(alertId);
                if (Parsing.ConvertToInt(alertDbModel.TriggerTypeId) == (int)AlertTypes.PercentageTrailingStop)
                {
                    Checker.CheckContains(newTrailingStopValue.ToString(), alertDbModel.ThresholdValue, "Alert TS% value in DB is not as expected");
                }

                if (intradayAlertsIds.Contains(alertId))
                {
                    Checker.IsTrue(Parsing.ConvertToBool(alertDbModel.UseIntraday), "Alert intraday type is not as expected");
                }
                else
                {
                    Checker.IsFalse(Parsing.ConvertToBool(alertDbModel.UseIntraday), "Alert intraday type is not as expected");
                }
            }
        }
    }
}