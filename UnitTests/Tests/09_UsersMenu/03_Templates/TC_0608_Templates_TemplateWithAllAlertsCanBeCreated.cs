using System;
using System.Collections.Generic;
using System.Linq;
using AutomatedTests.Database.AlertTemplates;
using AutomatedTests.Enums.Alerts;
using AutomatedTests.Enums.User;
using AutomatedTests.Forms.Templates;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.PreconditionsSteps;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._09_UsersMenu._03_Templates
{
    [TestClass]
    public class TC_0608_Templates_TemplateWithAllAlertsCanBeCreated : BaseTestUnitTests
    {
        private const int TestNumber = 608;

        private string templateName;
        private List<string> expectedAlerts;
        private List<int> expectedAlertsIds;
        private int targetAlertQuantity;

        [TestInitialize]
        public void TestInitialize()
        {
            var userType = GetTestDataParsedAsEnumFromStringMapping<ProductSubscriptionTypes>("user");
            var alertsQuantity = GetTestDataAsInt("alertsQuantity");
            expectedAlerts = new ReadTestDataFromDataSourceSteps().GetAlertsListFromDataSourceByColumnPatternAlertsQuantity(alertsQuantity, "AddedAlerts", TestContext);

            expectedAlertsIds = new List<int>
            {
                (int)AlertTypes.PercentOfAverageVolume, (int)AlertTypes.AboveBelowMovingAverage, (int)AlertTypes.MovingAverageCrosses,
                (int)AlertTypes.CalendarDaysAfterEntry, (int)AlertTypes.TradingDaysAfterEntry, (int)AlertTypes.ProfitableClosesAfterEntry,
                (int)AlertTypes.SpecificDate, (int)AlertTypes.PercentageTrailingStop, (int)AlertTypes.PercentageGainLoss, (int)AlertTypes.DollarGainLoss,
                (int)AlertTypes.FixedPriceAboveBelow, (int)AlertTypes.Breakout, (int)AlertTypes.Target, (int)AlertTypes.Target, (int)AlertTypes.Target,
                (int)AlertTypes.Target, (int)AlertTypes.Target, (int)AlertTypes.Target, (int)AlertTypes.Target, (int)AlertTypes.VqTrailingStop,
                (int)AlertTypes.TwoVq
            };
            targetAlertQuantity = expectedAlertsIds.Count(t => t == (int)AlertTypes.Target);
            templateName = "template";

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, (ProductSubscriptions)(int)userType));
            LoginSetUp.LogIn(UserModels.First());
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_608$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("TemplatesPage"), TestCategory("TemplateAdd"), TestCategory("Alerts"), TestCategory("AlertAdd")]
        [Description("The test checks possibility of creating template with adding all alerts. https://tr.a1qa.com/index.php?/cases/view/19232456")]
        public override void RunTest()
        {
            LogStep(1, 6, "Click 'Add Alert' button for the alert with default settings.");
            var templateSetUps = new TemplateSetUps();
            templateSetUps.CreateTemplateWithAllTypesOfAlertsWithDefaultSettings(templateName);

            LogStep(7, "Click on name for the added template. button");
            new TemplatesForm().ClickOnTemplateName(templateName);

            LogStep(8, "Make sure all added alert present on 'Current Alerts' section.");
            var editTemplateForm = new EditTemplateForm();
            var allAlerts = editTemplateForm.GetAllAlerts();
            Assert.IsTrue(allAlerts.Any(), "All Alerts quantity is less than 1");

            var items = BaseObjectComparator.FindItemsNotPresentIn2Lists(allAlerts, expectedAlerts);
            Checker.IsFalse(items.Any(), $"There are some alerts, wasn't created: {items.Aggregate("", (current, item) => current + item + " ")}");

            LogStep(9, "In DB:make sure all alerts added correctlyTables:dbo.AlertTemplates");
            var templatesQueries = new AlertTemplatesQueries();
            var templateFromDb = templatesQueries.SelectUserTemplates(UserModels.First().TradeSmithUserId).FirstOrDefault(s => s.Title == templateName);
            Assert.IsNotNull(templateFromDb, "Template from db is null");
            Assert.AreEqual(Parsing.ConvertToShortDateString(templateFromDb.DateCreated), Parsing.ConvertToShortDateString(DateTime.Now.ToShortDateString()), "Date time is not equals");
            Assert.IsTrue(Parsing.ConvertToBool(templateFromDb.IsDefault), "Template is NOT default");
            var templateAlerts = templatesQueries.SelectTemplateAlerts(Parsing.ConvertToInt(templateFromDb.AlertTemplateId));
            foreach (var alertId in expectedAlertsIds)
            {
                if (alertId != (int)AlertTypes.Target)
                {
                    Checker.IsTrue(templateAlerts.FirstOrDefault(t => Parsing.ConvertToInt(t.TriggerTypeId) == alertId) != null,
                        $"Alert with TriggerTypeId {alertId} wasn't created");
                }
                else
                {
                    targetAlertQuantity--;
                }
            }
            Checker.IsTrue(targetAlertQuantity == 0, "Unexpected target alerts in DB");
        }
    }
}