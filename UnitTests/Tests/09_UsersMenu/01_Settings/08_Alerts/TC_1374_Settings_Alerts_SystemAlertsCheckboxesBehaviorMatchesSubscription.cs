using AutomatedTests.Enums.Settings;
using AutomatedTests.Enums.User;
using AutomatedTests.Enums;
using AutomatedTests.Forms.Settings.Alerts;
using AutomatedTests.Forms.Settings;
using AutomatedTests.Forms;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Enums;
using TradeStops.Common.Extensions;
using TradeStops.Common.Utils;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._09_UsersMenu._01_Settings._08_Alerts
{
    [TestClass]
    public class TC_1374_Settings_Alerts_SystemAlertsCheckboxesBehaviorMatchesSubscription : BaseTestUnitTests
    {
        private const int TestNumber = 1374;

        private List<SystemAlertsTypes> actualSystemAlerts;
        private ProductSubscriptionTypes userType;
        private readonly Dictionary<SystemAlertsTypes, bool> expectedDefaultStateSystemAlerts = new Dictionary<SystemAlertsTypes, bool>();
        private int amountOfAlerts;
        private string newHighProfitHint;

        [TestInitialize]
        public void TestInitialize()
        {
            userType = GetTestDataParsedAsEnumFromStringMapping<ProductSubscriptionTypes>("Subscription");
            var allSystemAlerts = EnumUtils.GetValues<SystemAlertsTypes>().ToList();
            if (userType == ProductSubscriptionTypes.TSBasic)
            {
                actualSystemAlerts = new List<SystemAlertsTypes> { SystemAlertsTypes.NewHighProfit };
            }
            else if (userType != ProductSubscriptionTypes.TSPlatinum)
            {
                actualSystemAlerts = allSystemAlerts.Except(new List<SystemAlertsTypes> { SystemAlertsTypes.StockRatingUpdates }).ToList();
            }
            else
            {
                actualSystemAlerts = allSystemAlerts;
            }

            amountOfAlerts = GetTestDataAsInt(nameof(amountOfAlerts));
            newHighProfitHint = GetTestDataAsString(nameof(newHighProfitHint));

            foreach (var systemAlert in allSystemAlerts)
            {
                expectedDefaultStateSystemAlerts.Add(systemAlert, !systemAlert.In(SystemAlertsTypes.NewHighProfit, SystemAlertsTypes.StockRatingUpdates));
            }


            LogStep(0, "Preconditions: Login. Create the portfolio. Change DB param. Open Dashboard page");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, (ProductSubscriptions)(int)userType));
            new SettingsSteps().LoginNavigateToSettingsAlertsGetForm(UserModels.First());
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1374$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test checks that System Alerts can be checked and unchecked for users with different subscriptions https://tr.a1qa.com/index.php?/cases/view/19539235")]
        [TestCategory("SettingsPage"), TestCategory("SettingsPageAlertsTab")]
        public override void RunTest()
        {
            LogStep(1, "Check that amount of system alerts");
            var alertSettingsForm = new AlertsSettingsForm();
            Checker.CheckEquals(amountOfAlerts * EnumUtils.GetValues<SystemAlertsCategoryTypes>().Count(),
               alertSettingsForm.GetAvailableSystemAlertsQuantity(),
               "Quantity of available system alerts is not as expected");

            LogStep(2, "Uncheck all alerts");
            foreach (var systemAlert in actualSystemAlerts)
            {
                foreach (var category in EnumUtils.GetValues<SystemAlertsCategoryTypes>())
                {
                    var expectedDefaultStateOfCurrentSystemAlert = category != SystemAlertsCategoryTypes.Newsletters
                                                                            && expectedDefaultStateSystemAlerts[systemAlert];
                    Checker.CheckEquals(expectedDefaultStateOfCurrentSystemAlert, alertSettingsForm.IsSystemAlertCheckboxActive(category, systemAlert),
                        $"Default state of {category.GetStringMapping()} for {systemAlert.GetStringMapping()} is not as expected");

                    alertSettingsForm.SetSystemAlertCheckboxByNameCategoryState(category, systemAlert, false);
                    Checker.IsFalse(alertSettingsForm.IsSystemAlertCheckboxActive(category, systemAlert),
                        $"Disabling of {category.GetStringMapping()} for {systemAlert.GetStringMapping()} is not as expected");
                }
            }

            LogStep(3, "Check all alerts");
            foreach (var systemAlert in actualSystemAlerts)
            {
                foreach (var category in EnumUtils.GetValues<SystemAlertsCategoryTypes>())
                {
                    alertSettingsForm.SetSystemAlertCheckboxByNameCategoryState(category, systemAlert, true);
                    Checker.IsTrue(alertSettingsForm.IsSystemAlertCheckboxActive(category, systemAlert),
                        $"Enabling of {category.GetStringMapping()} for {systemAlert.GetStringMapping()} is not as expected");
                }
            }

            LogStep(4, "Click Deselect All button'");
            if (userType == ProductSubscriptionTypes.TSPlatinum)
            {
                CheckClickingGroupSelectingLink(CheckAllSystemAlertsLinkType.DeselectAll);
            }

            LogStep(5, "Click Select All button'");
            if (userType == ProductSubscriptionTypes.TSPlatinum)
            {
                CheckClickingGroupSelectingLink(CheckAllSystemAlertsLinkType.SelectAll);
            }

            LogStep(6, "Check New High Profit alert hint");
            Checker.CheckEquals(newHighProfitHint, alertSettingsForm.GetHighProfitHint(), "Hint for high profit is not as expected");

            LogStep(7, "Uncheck New High Profit for watch. Click Save for setting");
            alertSettingsForm.SetSystemAlertCheckboxByNameCategoryState(SystemAlertsCategoryTypes.WatchOnlyPorfolios, SystemAlertsTypes.NewHighProfit, false);
            var settingsMainForm = new SettingsMainForm();
            settingsMainForm.SaveSettings();

            LogStep(8, "Leave page and return");
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
            new MainMenuNavigation().OpenSettingsAlerts();
            alertSettingsForm.AssertIsOpen();
            Checker.IsFalse(alertSettingsForm.IsSystemAlertCheckboxActive(SystemAlertsCategoryTypes.WatchOnlyPorfolios, SystemAlertsTypes.NewHighProfit),
                "Disabling of WatchOnlyPorfolios for NewHighProfit is not saved");

            if (userType == ProductSubscriptionTypes.TSPlatinum)
            {
                LogStep(9, "Uncheck New Stock Rating Updates for newsletters. Click Save for setting");
                alertSettingsForm.SetSystemAlertCheckboxByNameCategoryState(SystemAlertsCategoryTypes.Newsletters, SystemAlertsTypes.StockRatingUpdates, false);
                settingsMainForm.SaveSettings();

                LogStep(10, "Leave page and return");
                settingsMainForm.ClickSettingsItem(SettingsSectionTypes.Contact);
                settingsMainForm.ClickSettingsItem(SettingsSectionTypes.Alerts);
                alertSettingsForm.AssertIsOpen();
                Checker.IsFalse(alertSettingsForm.IsSystemAlertCheckboxActive(SystemAlertsCategoryTypes.Newsletters, SystemAlertsTypes.StockRatingUpdates),
                    "Disabling of WatchOnlyPorfolios for NewHighProfit is not saved");
            }
        }

        private void CheckClickingGroupSelectingLink(CheckAllSystemAlertsLinkType link)
        {
            var alertSettingsForm = new AlertsSettingsForm();
            alertSettingsForm.SetAllSystemAlertsLink(link);
            foreach (var systemAlert in actualSystemAlerts)
            {
                foreach (var category in EnumUtils.GetValues<SystemAlertsCategoryTypes>())
                {
                    Checker.CheckEquals(link == CheckAllSystemAlertsLinkType.SelectAll, alertSettingsForm.IsSystemAlertCheckboxActive(category, systemAlert),
                        $"Clicking {link.GetStringMapping()} at {category.GetStringMapping()} for {systemAlert.GetStringMapping()} is not as expected");
                }
            }
        }
    }
}