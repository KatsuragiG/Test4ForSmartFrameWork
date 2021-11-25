using AutomatedTests;
using AutomatedTests.Enums.Settings;
using AutomatedTests.Forms.Settings;
using AutomatedTests.Forms.Settings.Alerts;
using AutomatedTests.Forms.Settings.Contact;
using AutomatedTests.Forms.Settings.Notification;
using AutomatedTests.Forms.Settings.Position;
using AutomatedTests.Forms.Settings.Subscriptions;
using AutomatedTests.Forms.Settings.Support;
using AutomatedTests.Forms.Settings.Tags;
using AutomatedTests.Models.UserModels;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Settings;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._09_UsersMenu._01_Settings
{
    [TestClass]
    public class TC_0576_Settings_CheckDescriptions : BaseTestUnitTests
    {
        private const int TestNumber = 576;

        private bool isUserFromStansberry;
        private string stansberryEmail;

        private string expectedContactTitle;
        private List<string> expectedContactBottomText;

        private string expectedSubscriptionTitle;
        private List<string> expectedSubscriptionSubNames;
        private List<string> expectedCurrentSubscriptionNames;
        private List<string> expectedOtherProdutsNames;
        private string expectedBillingText;

        private string expectedNotificationTitle;
        private List<string> expectedNotificationSubNames;
        private List<string> expectedNotificationBottomTexts;
        private List<string> expectedNotificationInfoTexts;
        private List<string> expectedNotificationRightTexts;

        private string expectedPositionsTitle;
        private List<string> expectedPositionsSubNames;
        private List<string> expectedPositionsBottomTexts;
        private List<string> expectedPositionsLeftTexts;

        private string expectedAlertTitle;
        private List<string> expectedAlertSubNames;
        private List<string> expectedAlertInfoTexts;
        private List<string> expectedAlertLeftTexts;

        private string expectedTagsTitle;
        private List<string> expectedTagsSubNames;
        private List<string> expectedTagsBottomText;
        private List<string> expectedTagsInfoTexts;

        private string expectedSupportTitle;
        private List<string> expectedSupportSubNames;
        private List<string> expectedSupportAccessTexts;
        private List<string> expectedSupportInfoTexts;

        [TestInitialize]
        public void TestInitialize()
        {
            isUserFromStansberry = GetTestDataAsBool(nameof(isUserFromStansberry));
            stansberryEmail = GetTestDataAsString(nameof(stansberryEmail));
            var userProductSubscriptions = GetUserProductSubscriptions("userSubscription");

            expectedContactTitle = GetTestDataAsString(nameof(expectedContactTitle));
            expectedContactBottomText = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(expectedContactBottomText));

            expectedSubscriptionTitle = GetTestDataAsString(nameof(expectedSubscriptionTitle));
            expectedSubscriptionSubNames = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(expectedSubscriptionSubNames));
            expectedCurrentSubscriptionNames = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(expectedCurrentSubscriptionNames));
            expectedOtherProdutsNames = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(expectedOtherProdutsNames));
            expectedBillingText = GetTestDataAsString(nameof(expectedBillingText));

            expectedNotificationTitle = GetTestDataAsString(nameof(expectedNotificationTitle));
            expectedNotificationSubNames = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(expectedNotificationSubNames));
            expectedNotificationBottomTexts = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(expectedNotificationBottomTexts));
            expectedNotificationInfoTexts = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(expectedNotificationInfoTexts));
            expectedNotificationRightTexts = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(expectedNotificationRightTexts));

            expectedPositionsTitle = GetTestDataAsString(nameof(expectedPositionsTitle));
            expectedPositionsSubNames = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(expectedPositionsSubNames));
            expectedPositionsBottomTexts = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(expectedPositionsBottomTexts));
            expectedPositionsLeftTexts = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(expectedPositionsLeftTexts));

            expectedAlertTitle = GetTestDataAsString(nameof(expectedAlertTitle));
            expectedAlertSubNames = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(expectedAlertSubNames));
            expectedAlertInfoTexts = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(expectedAlertInfoTexts));
            expectedAlertLeftTexts = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(expectedAlertLeftTexts));

            expectedTagsTitle = GetTestDataAsString(nameof(expectedTagsTitle));
            expectedTagsSubNames = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(expectedTagsSubNames));
            expectedTagsBottomText = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(expectedTagsBottomText));
            expectedTagsInfoTexts = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(expectedTagsInfoTexts));

            expectedSupportTitle = GetTestDataAsString(nameof(expectedSupportTitle));
            expectedSupportSubNames = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(expectedSupportSubNames));
            expectedSupportAccessTexts = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(expectedSupportAccessTexts));
            expectedSupportInfoTexts = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(expectedSupportInfoTexts));

            LogStep(0, "Precondition");
            UserModels.Add(isUserFromStansberry
                ? new UserModel{ Email = stansberryEmail, Password = new CustomTestDataReader().GetDefaultUserPassword() }
                : ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));

            new SettingsSteps().LoginNavigateToSettings(UserModels.First());
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_576$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("SettingsPage"), TestCategory("SettingsPageAlertsTab"), TestCategory("SettingsPageContactTab"), TestCategory("SettingsPagePositionsTab"),
            TestCategory("SettingsPageSiteTab"), TestCategory("SettingsPageSubscriptionTab"), TestCategory("SettingsTagsTab"), TestCategory("SettingsPageSupportTab")]
        [Description("The test checks text on all Settings pages. https://tr.a1qa.com/index.php?/cases/view/19232476")]
        public override void RunTest()
        {
            LogStep(1, "Open page 'My Contact Information' Make sure text as expected.");
            var settingsForm = new SettingsMainForm();
            Checker.CheckEquals(expectedContactTitle, settingsForm.GetSectionHeader(), "Contact page title is not as expected");

            var sectionBottomTexts = new ContactForm().GetBottomText();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedContactBottomText, sectionBottomTexts),
                $"Bottom info is not as expected on Contact form: {GetExpectedResultsString(expectedContactBottomText)}\r\n{GetActualResultsString(sectionBottomTexts)}");

            LogStep(2, "Open page 'Subscriptions Settings' Make sure text as expected.");
            settingsForm.ClickSettingsItem(SettingsSectionTypes.Subscriptions);
            var subscriptionForm = new SubscriptionsForm();
            Checker.CheckEquals(expectedSubscriptionTitle, settingsForm.GetSectionHeader(), "Subscription title is not as expected on Subscriptions Settings form");
            var sectionSubNames = subscriptionForm.GetSectionsNames();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedSubscriptionSubNames, sectionSubNames),
                $"Sub section names is not as expected on subscription form: {GetExpectedResultsString(expectedSubscriptionSubNames)}\r\n{GetActualResultsString(sectionSubNames)}");
            var activeSubscriptionsNames = subscriptionForm.GetCurrentProductSubscriptions()
                .Select(t => t.Replace("\r\n", " ").Trim()).ToList();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedCurrentSubscriptionNames, activeSubscriptionsNames),
                $"Active Subscriptions Names is not as expected on subscription form: {GetExpectedResultsString(expectedCurrentSubscriptionNames)}\r\n" +
                $"{GetActualResultsString(activeSubscriptionsNames)}");
            var otherProductsNames = subscriptionForm.GetOtherProductSubscriptions().Select(t => t.Trim()).ToList();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedOtherProdutsNames, otherProductsNames),
                $"Other Products Names is not as expected on subscription form: {GetExpectedResultsString(expectedOtherProdutsNames)}\r\n" +
                $"{GetActualResultsString(otherProductsNames)}");
            Checker.CheckEquals(expectedBillingText, subscriptionForm.GetBillingInformationText(),
                "Billing Information text is not as expected on Subscriptions Settings form");

            LogStep(3, "Open page 'My Notification Settings Make sure text as expected.");
            settingsForm.ClickSettingsItem(SettingsSectionTypes.Notification);
            var notificationsForm = new NotificationForm();
            Checker.CheckEquals(expectedNotificationTitle, settingsForm.GetSectionHeader(), "Notification title is not as expected");
            sectionSubNames = notificationsForm.GetSubSectionsNames().Select(t => t.Replace("\r\n", " ").Trim()).ToList();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedNotificationSubNames, sectionSubNames),
                $"Sub section names is not as expected on Notification form: {GetExpectedResultsString(expectedNotificationSubNames)}\r\n" +
                $"{GetActualResultsString(sectionSubNames)}");
            sectionBottomTexts = notificationsForm.GetBottomText();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedNotificationBottomTexts, sectionBottomTexts),
                $"Bottom info is not as expected on Notification form: {GetExpectedResultsString(expectedNotificationBottomTexts)}\r\n" +
                $"{GetActualResultsString(sectionBottomTexts)}");
            var infoTexts = notificationsForm.GetInfoTexts();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedNotificationInfoTexts, infoTexts),
                $"Info text is not as expected on Notification form: {GetExpectedResultsString(expectedNotificationInfoTexts)}\r\n" +
                $"{GetActualResultsString(infoTexts)}");
            var leftTexts = notificationsForm.GetLeftText();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedNotificationRightTexts, leftTexts),
                $"Left message is not as expected on Notification form: {GetExpectedResultsString(expectedNotificationRightTexts)}\r\n" +
                $"{GetActualResultsString(leftTexts)}");

            LogStep(4, "Open page Positions Make sure text as expected.");
            settingsForm.ClickSettingsItem(SettingsSectionTypes.Positions);
            var positionsForm = new PositionSettingForm();
            Checker.CheckEquals(expectedPositionsTitle, settingsForm.GetSectionHeader(), "Notification title is not as expected on Positions form");
            sectionSubNames = positionsForm.GetSubSectionsNames();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedPositionsSubNames, sectionSubNames),
                $"Sub section names is not as expected on Positions form: {GetExpectedResultsString(expectedPositionsSubNames)}\r\n" +
                $"{GetActualResultsString(sectionSubNames)}");
            sectionBottomTexts = positionsForm.GetBottomText();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedPositionsBottomTexts, sectionBottomTexts),
                $"Bottom info is not as expected on Positions form: {GetExpectedResultsString(expectedPositionsBottomTexts)}\r\n" +
                $"{GetActualResultsString(sectionBottomTexts)}");
            var leftMessages = positionsForm.GetInfoTexts();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedPositionsLeftTexts, leftMessages),
                $"Left message is not as expected on Positions form: {GetExpectedResultsString(expectedPositionsLeftTexts)}\r\n" +
                $"{GetActualResultsString(leftMessages)}");

            LogStep(5, "Open page 'My Alert Settings' Make sure text as expected.");
            settingsForm.ClickSettingsItem(SettingsSectionTypes.Alerts);
            var alertsForm = new AlertsSettingsForm();
            Checker.CheckEquals(expectedAlertTitle, settingsForm.GetSectionHeader(), "Notification title is not as expected on Alert form");
            sectionSubNames = alertsForm.GetSubSectionsNames().Select(t => t.Replace("\r\n", " ").Trim()).ToList();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedAlertSubNames, sectionSubNames),
                $"Sub section names is not as expected on Alert form: {GetExpectedResultsString(expectedAlertSubNames)}\r\n" +
                $"{GetActualResultsString(sectionSubNames)}");
            infoTexts = alertsForm.GetInfoTexts();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedAlertInfoTexts, infoTexts),
                $"Info text is not as expected on Alert form: {GetExpectedResultsString(expectedAlertInfoTexts)}\r\n" +
                $"{GetActualResultsString(infoTexts)}");
            sectionBottomTexts = alertsForm.GetBottomText();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedAlertLeftTexts, sectionBottomTexts),
                $"Left message is not as expected on Alert form: {GetExpectedResultsString(expectedAlertLeftTexts)}\r\n" +
                $"{GetActualResultsString(sectionBottomTexts)}");

            LogStep(6, "Open page 'Manage My Tags' Make sure text as expected.");
            settingsForm.ClickSettingsItem(SettingsSectionTypes.Tags);
            var tagsForm = new TagsForm();
            Checker.CheckEquals(expectedTagsTitle, settingsForm.GetSectionHeader(), "Tags title is not as expected on Tags form");
            sectionSubNames = tagsForm.GetSubSectionsNames();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedTagsSubNames, sectionSubNames),
                $"Sub section names is not as expected on Tags form: {GetExpectedResultsString(expectedTagsSubNames)}\r\n" +
                $"{GetActualResultsString(sectionSubNames)}");
            sectionBottomTexts = tagsForm.GetBottomInfoMessage();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedTagsBottomText, sectionBottomTexts),
                $"Bottom info  is not as expected on Tags form: {GetExpectedResultsString(expectedTagsBottomText)}\r\n" +
                $"{GetActualResultsString(sectionBottomTexts)}");
            infoTexts = tagsForm.GetInfoTexts();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedTagsInfoTexts, infoTexts),
                $"Info text is not as expected on Tags form: {GetExpectedResultsString(expectedTagsInfoTexts)}\r\n" +
                $"{GetActualResultsString(infoTexts)}");

            LogStep(7, "Open page 'Support' Make sure text as expected.");
            settingsForm.ClickSettingsItem(SettingsSectionTypes.Support);
            var supportForm = new SupportForm();
            Checker.CheckEquals(expectedSupportTitle, settingsForm.GetSectionHeader(), "Support title is not as expected on Support form");
            sectionSubNames = supportForm.GetSubSectionsNames();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedSupportSubNames, sectionSubNames),
                $"Sub section names is not as expected on Support form: {GetExpectedResultsString(expectedSupportSubNames)}\r\n" +
                $"{GetActualResultsString(sectionSubNames)}");
            var accessText = supportForm.GetAccessText();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedSupportAccessTexts, accessText),
                $"Access-duration-message is not as expected on Support form: {GetExpectedResultsString(expectedSupportAccessTexts)}\r\n" +
                $"{GetActualResultsString(accessText)}");
            infoTexts = supportForm.GetInfoTexts();
            Checker.IsTrue(ListsComparator.AreTwoListsEqualsNotInOrder(expectedSupportInfoTexts, infoTexts),
                $"Info text is not as expected on Support form: {GetExpectedResultsString(expectedSupportInfoTexts)}\r\n" +
                $"{GetActualResultsString(infoTexts)}");
        }
    }
}