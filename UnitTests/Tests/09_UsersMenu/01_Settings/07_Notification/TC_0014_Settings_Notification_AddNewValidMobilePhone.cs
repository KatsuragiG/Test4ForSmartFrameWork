using System;
using System.Collections.Generic;
using System.Linq;
using AutomatedTests.Database.Settings.Notification;
using AutomatedTests.Forms.Settings.Notification;
using AutomatedTests.Forms.Settings;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Settings;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.BaseTestsUnitTests;
using TradeStops.Common.Enums;
using AutomatedTests.Enums.User;

namespace UnitTests.Tests._09_UsersMenu._01_Settings._07_Notification
{
    [TestClass]
    public class TC_0014_Settings_Notification_AddNewValidMobilePhone : BaseTestUnitTests
    {
        private const int TestNumber = 14;

        private NotificationSettingsModel userNewMobilePhone;
        private DbNotificationModel expectedDbNewPhone1Email;

        [TestInitialize]
        public void TestInitialize()
        {
            var userType = GetTestDataParsedAsEnumFromStringMapping<ProductSubscriptionTypes>("user");
            userNewMobilePhone = new NotificationSettingsModel
            {
                MobileOperator1 = GetTestDataAsString("NewMobileOperator1"),
                Phone1 = GetTestDataAsString("NewPhone1")
            };
            expectedDbNewPhone1Email = new DbNotificationModel
            {
                Emails = new List<string>
                {
                    GetTestDataAsString("ExpectedNewPhone1Email")
                }
            };

            LogStep(0, "Preconditions. Go to Settings. Go to Notification");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, (ProductSubscriptions)(int)userType));
            new SettingsSteps().LoginNavigateToSettingsNotificationGetForm(UserModels.First());
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_14$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test checks Adding New Valid Mobile Phone https://tr.a1qa.com/index.php?/cases/view/20339732")]
        [TestCategory("Smoke"), TestCategory("SettingsPage"), TestCategory("SettingsPageNotificationTab")]
        public override void RunTest()
        {
            LogStep(1, "Change Mobile Phone 1");
            var notificationForm = new NotificationForm();
            notificationForm.FillPhoneFields(userNewMobilePhone);
            Checker.CheckEquals(userNewMobilePhone.MobileOperator1, notificationForm.GetPhoneDropdownCurrentItem(1), "Mobile operator is not as expected");
            Checker.CheckEquals(userNewMobilePhone.Phone1, notificationForm.GetMobileNumber(1), "Phone number is not as expected");

            LogStep(2, "Save new Info");
            var settingsForm = new SettingsMainForm();
            settingsForm.SaveSettings();
            Checker.IsTrue(settingsForm.IsSuccessSavedMessagePresent(), "Successful saving is not shown");

            LogStep(3, "Compare the actual and the expected result");
            var actualUserMobilePhone = notificationForm.GetDisplayedValues(userNewMobilePhone);
            var notFoundValues = ObjectComparator.FindInequalNotNullProperties(userNewMobilePhone, actualUserMobilePhone);

            var notificationQueries = new NotificationQueries();
            var dataFromDb = notificationQueries.SelectNotificationPhoneEmails(UserModels.First().TradeSmithUserId);
            var notFoundValuesInDb = ObjectComparator.CompareArrays(expectedDbNewPhone1Email.Emails, dataFromDb);

            Asserts.Batch(
               () => Assert.IsFalse(notFoundValues.Any(),
                   $"The data is not equal to the expected result :\r\n{string.Join(Environment.NewLine, notFoundValues.GetFormattedNotFoundValues())}"),
               () => Assert.IsFalse(notFoundValuesInDb.Any(),
                   $"Next values not found for the user with ID {UserModels.First().TradeSmithUserId} in database:\r\n" +
                   $"{string.Join(Environment.NewLine, notFoundValuesInDb.GetFormattedNotFoundValues())}"));
        }
    }
}