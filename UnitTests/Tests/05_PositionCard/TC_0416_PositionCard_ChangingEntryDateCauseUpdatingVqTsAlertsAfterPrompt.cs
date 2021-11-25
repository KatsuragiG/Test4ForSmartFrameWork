using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Alerts;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Alerts;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.PositionCard;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms.Popups;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms.Templates;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.PositionCard;
using AutomatedTests.Utils;
using AutomatedTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;
using AutomatedTests.Steps.PositionsGridSteps;
using AutomatedTests.Models.FiltersModels;

namespace UnitTests.Tests._05_PositionCard
{
    [TestClass]
    public class TC_0416_PositionCard_ChangingEntryDateCauseUpdatingVqTsAlertsAfterPrompt : BaseTestUnitTests
    {
        private const int TestNumber = 416;

        private CustomTestDataReader reader;
        private readonly TypeFilterModel manualFilterModel = new TypeFilterModel { IsManual = true, IsSync = false};
        private readonly TypeFilterModel syncFilterModel = new TypeFilterModel { IsManual = false, IsSync = true };
        private const string TemplateName = "template";
        private readonly List<int> addedPositionsIds = new List<int>();

        [TestInitialize]
        public void TestInitialize()
        {
            reader = new CustomTestDataReader();

            var userProductSubscriptions = GetUserProductSubscriptions("userSubscription");

            var cryptoTicker = GetTestDataAsString("cryptoTicker");

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));
            addedPositionsIds.Add(PortfoliosSetUp.AddWatchOnlyUsdPortfoliosWithOpenPosition(UserModels.First().Email, cryptoTicker));
            var portfolio = PortfoliosSetUp.ImportSynchronizedPortfolio06ViaDb(UserModels.First());
            LoginSetUp.LogIn(UserModels.First());

            var templateSetUps = new TemplateSetUps();
            templateSetUps.NavigateToTemplatesClickAddEnterTemplateName(TemplateName);
            templateSetUps.AddAllTypesOfTrailingStopAlertsDefault();
            templateSetUps.AddAllTypesOfUnderlyingStockAlertsDefault();
            new AddTemplateForm().ClickSaveTemplate();

            PortfoliosSetUp.DuplicatePortfolioAsInvestment(portfolio);

            var mainMenuNavigation = new MainMenuNavigation();
            mainMenuNavigation.OpenPositionCard(addedPositionsIds.First());
            var percentageTrailingStopForm = new AlertTabPositionCardSteps().GetAlertPercentageTrailingStopForm(TrailingStopAlertTypes.Ts);
            percentageTrailingStopForm.SelectIntradayAlertType(AlertTypes.PercentageTrailingStop, IntradayAlertTypes.LatestPrice);
            new AddAlertsPositionCardForm().ClickOnAddAlertButton(AlertTypes.PercentageTrailingStop.GetStringMapping());

            mainMenuNavigation.OpenPositionsGrid();
            new PositionsAlertsStatisticsPanelForm().SelectPortfolio(AllPortfoliosKinds.All.GetStringMapping());
            var positionsTabForm = new PositionsTabForm();
            positionsTabForm.SelectAllItemsInGrid();
            positionsTabForm.ClickGroupActionButton(PositionsGroupAction.AddAlert);
            var addAlert = new AddAlertPopup();
            addAlert.SelectAlertTemplate(TemplateName);
            addAlert.ClickAddButton();
            new ConfirmPopup(PopupNames.Warning).ClickOkButton();

            var positionsGridSteps = new PositionsGridSteps();
            positionsGridSteps.FilterTypeOfPositions(syncFilterModel);
            addedPositionsIds.AddRange(positionsTabForm.GetPositionsIdsByTicker(reader.GetSynchronized06Portfolio()[3]));
            addedPositionsIds.AddRange(positionsTabForm.GetPositionsIdsByTicker(reader.GetSynchronized06Portfolio()[4]));
            addedPositionsIds.AddRange(positionsTabForm.GetPositionsIdsByTicker(reader.GetSynchronized06Portfolio()[9]));
            addedPositionsIds.AddRange(positionsTabForm.GetPositionsIdsByTicker(reader.GetSynchronized06Portfolio()[10]));

            positionsGridSteps.FilterTypeOfPositions(manualFilterModel);
            addedPositionsIds.AddRange(positionsTabForm.GetPositionsIdsByTicker(reader.GetSynchronized06Portfolio()[6]));
            addedPositionsIds.AddRange(positionsTabForm.GetPositionsIdsByTicker(reader.GetSynchronized06Portfolio()[7]));
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_416$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("https://tr.a1qa.com/index.php?/cases/view/19232626 The test checks correctness of updating alerts after changing entry date from position card for Premium users")]
        [TestCategory("Smoke"), TestCategory("PositionCard"), TestCategory("Alerts"), TestCategory("PositionCardPositionDetailsTab"), TestCategory("TrailingStopAlertsGroup")]
        public override void RunTest()
        {
            LogStep(11, "Repeat steps 1-10 for positions 5.2 - 5.12");
            foreach (var positionId in addedPositionsIds)
            {
                LogStep(1, "Click on Symbol link for position from step 5.1 from precondition");
                var positionsTab = new PositionsTabForm();
                new PositionsGridSteps().ResetPositionsFilters();
                positionsTab.ClickOnPositionLink(positionId);

                LogStep(2, "Click Position Details tab Click Edit sign");
                var positionsCard = new PositionCardForm();
                var positionDetailsTabPositionCardForm = positionsCard.ActivateTabGetForm<PositionDetailsTabPositionCardForm>(PositionCardTabs.PositionDetails);
                positionDetailsTabPositionCardForm.EditPositionCard();

                LogStep(3, "Select from datapicker entrydate = Today - 3 days Click Get quote Click Save");
                var entryDate = Parsing.ConvertToShortDateString(DateTimeProvider.GetDate(DateTime.Now, -3).ToShortDateString());
                positionDetailsTabPositionCardForm.SetValueInTextBoxField(PositionDetailsFieldTypes.EntryDate, entryDate);
                positionDetailsTabPositionCardForm.ClickGetQuote();
                positionsCard.ClickSave();

                LogStep(4, "Click Yes");
                new ConfirmPopup(PopupNames.Warning).ClickYesButton();
                var symbol = new PositionsQueries().SelectSymbolByPositionId(positionId);
                Checker.CheckEquals(entryDate, positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.EntryDate),
                    $"entry Date not equals for {symbol}");

                LogStep(5, 8, "Go to Alerts tab on position card");
                var alertsTabPositionCardForm = positionsCard.ActivateTabGetForm<AlertsTabPositionCardForm>(PositionCardTabs.Alerts);
                var allAlerts = alertsTabPositionCardForm.GetAllAlertsDescriptions();

                Checker.IsTrue(allAlerts.Any(), $"number of alerts is 0 for {symbol}");

                for (int i = 0; i < allAlerts.Count; i++)
                {
                    LogStep(6, "Click Edit sign for first alert Check that Start date of alert is matched with selected on step 3");
                    alertsTabPositionCardForm.EditAlert(allAlerts[i]);
                    Checker.CheckEquals(entryDate,
                        Parsing.ConvertToShortDateString(DateTime.Parse(alertsTabPositionCardForm.GetAlertStartDateByOrderOfAlert(i + 1)).ToShortDateString()),
                        $"5 start date not equals for {allAlerts[i]} on {symbol}");

                    LogStep(7, "Click Cancel");
                    alertsTabPositionCardForm.ClickCancelEditAlertByNumberOfAlert(i + 1);
                }

                LogStep(9, "Check that start date for position's alerts in db is matched with selected on step 3");
                var alertsQueries = new AlertsQueries();
                var allAlertsDb = alertsQueries.SelectAllActiveAlertsIdUsingPositionId(positionId);
                Checker.IsTrue(allAlertsDb.Any(), $"count of alerts in DB is 0 for  {symbol}");
                foreach (var alert in allAlertsDb)
                {
                    Checker.CheckEquals(DateTime.Parse(entryDate), DateTime.Parse(alertsQueries.SelectAlertsStartDateTsPercent(alert).StartDate),
                        $"9 start date not equals for {alert} on {symbol}");
                }

                LogStep(10, "Go to Positions&Alerts -> Positions tab. Select All in portfolio dropdown.");
                new MainMenuNavigation().OpenPositionsGrid();
                new PositionsAlertsStatisticsPanelForm().SelectPortfolio(AllPortfoliosKinds.All.GetStringMapping());
            }
        }
    }
}