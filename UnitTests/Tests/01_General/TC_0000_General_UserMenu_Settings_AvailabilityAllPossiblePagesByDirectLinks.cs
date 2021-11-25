using AutomatedTests.ConstantVariables;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Dashboard;
using AutomatedTests.Enums.Settings;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Events;
using AutomatedTests.Forms.Publications;
using AutomatedTests.Forms.Settings.Alerts;
using AutomatedTests.Forms.Settings.Contact;
using AutomatedTests.Forms.Settings.Notification;
using AutomatedTests.Forms.Settings.Position;
using AutomatedTests.Forms.Settings.Subscriptions;
using AutomatedTests.Forms.Settings.Support;
using AutomatedTests.Forms.Settings.Tags;
using AutomatedTests.Forms.Templates;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.BrowserSteps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._01_General
{
    [TestClass]
    public class TC_0000_General_UserMenu_Settings_AvailabilityAllPossiblePagesByDirectLinks : BaseTestUnitTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            LogStep(0, "Precondition - Login as registered user");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(0, ProductSubscriptions.TradeStopsPlatinum));
            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [Description("The test checks all possible pages availability and opening using direct links https://tr.a1qa.com/index.php?/cases/view/19235577")]
        [TestMethod]
        [TestCategory("Smoke")]
        public override void RunTest()
        {
            LogStep(1, "Check Settings");
            var mainMenuNavigation = new MainMenuNavigation();
            mainMenuNavigation.OpenSettings();
            var contactForm = new ContactForm();
            contactForm.AssertIsOpen();
            var browserSteps = new BrowserSteps();
            browserSteps.CheckBrowserConsoleForErrors(SettingsSectionTypes.Contact.ToString());

            mainMenuNavigation.OpenSettingsSubscriptions();
            new SubscriptionsForm().AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(SettingsSectionTypes.Subscriptions.ToString());

            mainMenuNavigation.OpenSettingsNotification();
            new NotificationForm().AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(SettingsSectionTypes.Notification.ToString());

            mainMenuNavigation.OpenSettingsPositions();
            new PositionSettingForm().AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(SettingsSectionTypes.Positions.ToString());

            mainMenuNavigation.OpenSettingsAlerts();
            new AlertsSettingsForm().AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(SettingsSectionTypes.Alerts.ToString());

            mainMenuNavigation.OpenSettingsTags();
            new TagsForm().AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(SettingsSectionTypes.Tags.ToString());

            mainMenuNavigation.OpenSettingsSupport();
            new SupportForm().AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(SettingsSectionTypes.Support.ToString());

            LogStep(2, "Check Events");
            mainMenuNavigation.OpenEvents();
            new EventsForm().AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(MainMenuItems.Events.ToString());

            LogStep(3, "Check Publications");
            mainMenuNavigation.OpenPublications();
            var publicationsForm = new PublicationsForm();
            publicationsForm.AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(MainMenuItems.AlertTemplates.ToString());

            LogStep(4, "Check Templates");
            mainMenuNavigation.OpenTemplates();
            var templatesForm = new TemplatesForm();
            templatesForm.AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(MainMenuItems.AlertTemplates.ToString());

            LogStep(5, "Check Creation Templates");
            mainMenuNavigation.OpenCreationTemplatePage();
            new AddTemplateForm().AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(DashboardWidgetActionItems.CreateAnAlertTemplate.GetStringMapping());

            LogStep(6, "Check CheckList");
            mainMenuNavigation.OpenCheckList();
            var checkListTemplateForm = new CheckListTemplateForm();
            checkListTemplateForm.AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(checkListTemplateForm.GetFormTitle());

            LogStep(7, "Check Creation CheckList page");
            mainMenuNavigation.OpenCheckListAdding();
            var createCheckListForm = new CreateCheckListForm();
            createCheckListForm.AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(createCheckListForm.GetFormTitle());

            LogStep(8, "Check Baskets");
            mainMenuNavigation.OpenBaskets();
            var basketsManagementForm = new BasketsManagementForm();
            basketsManagementForm.AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(basketsManagementForm.GetFormTitle());

            LogStep(9, "Edit CoPilot Baskets");
            basketsManagementForm.ClickOnBasketName(Constants.DefaultPositionsBasketName);
            var addEditBasketForm = new AddEditBasketForm();
            addEditBasketForm.AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(Constants.DefaultPositionsBasketName);

            LogStep(10, "Check Creation Baskets");
            mainMenuNavigation.OpenBasketsCreating();
            addEditBasketForm.AssertIsOpen();
            browserSteps.CheckBrowserConsoleForErrors(addEditBasketForm.GetFormTitle());
        }
    }
}