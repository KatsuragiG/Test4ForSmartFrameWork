using AutomatedTests.Database.Publications;
using AutomatedTests.Forms.Publications;
using AutomatedTests.Forms;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.WebDriver;
using AutomatedTests.Enums;

namespace UnitTests.Tests._09_UsersMenu._04_Publications
{
    [TestClass]
    public class TC_1385_Publications_PublicationsAreFilteredAsExpected : BaseTestUnitTests
    {
        private const int TestNumber = 1385;
        private const string TypeDropdownMultiselectTextPattern = "{0} types selected";

        private string firstDate;
        private int publicationsQuantityToCheck;
        private int minimalCountOfNews;
        private List<string> expectedTypes;
        private List<string> selectedPublicationTypes;

        [TestInitialize]
        public void TestInitialize()
        {
            var userProductSubscriptions = GetUserProductSubscriptions("userSubscription");
            publicationsQuantityToCheck = GetTestDataAsInt(nameof(publicationsQuantityToCheck));
            minimalCountOfNews = GetTestDataAsInt(nameof(minimalCountOfNews));
            firstDate = GetTestDataAsString(nameof(firstDate));
            expectedTypes = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(expectedTypes)).OrderBy(p => p).ToList();
            selectedPublicationTypes = GetTestDataValuesAsListByColumnNameAndRemoveEmpty(nameof(selectedPublicationTypes));

            LogStep(0, "Preconditions: Login. Open Dashboard page");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Publications);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1385$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("Publications")]
        [Description("Test checks that filtration and all links works as expected https://tr.a1qa.com/index.php?/cases/view/21330331")]
        public override void RunTest()
        {
            LogStep(1, "Check Publications types in Type dropdown");
            var publicationsForm = new PublicationsForm();
            var availablePublicationsTypes = publicationsForm.GetAllPublicationsTypesInDropdown().OrderBy(p => p).ToList();
            Checker.CheckListsEquals(expectedTypes, availablePublicationsTypes, "Not all expected publications types are shown in dropdown");

            LogStep(2, "Select Custom range date and select From date = firstDate and toDate = today");
            var todayDate = Parsing.ConvertToShortDateString(DateTime.Now.AsShortDate());
            publicationsForm.SelectCustomPeriodRangeWithStartEndDate(firstDate, todayDate);
            Checker.IsFalse(publicationsForm.IsNoResultBlockShown(), "No result block is shown");
            Checker.CheckNotEquals(0, publicationsForm.GetNumberOfPublications(), "Unexpected publications quantity");

            LogStep(3, "Select  expected Publication type from dropdown");
            publicationsForm.SelectPublicationTypes(selectedPublicationTypes);
            var allPublicationsModels = publicationsForm.GetAllPublicationsModels();
            var resultsInGrid = allPublicationsModels.Count;
            var expectedDropdownText = selectedPublicationTypes.Count == 1
                ? selectedPublicationTypes.First()
                : string.Format(TypeDropdownMultiselectTextPattern, selectedPublicationTypes.Count);
            Checker.CheckEquals(expectedDropdownText, publicationsForm.GetSelectedPublicationInDropdown(),
                "Publication type dropdown has wrong text");
            Checker.IsTrue(resultsInGrid >= minimalCountOfNews,
                $"Count of news is not as expected: {resultsInGrid} but expected {minimalCountOfNews}");

            LogStep(4, "Click on the random newsQuantityToCheck news in the grid.");
            var publicationOrdersToClick = Randoms.GetRandomNumbersInRange(1, resultsInGrid, publicationsQuantityToCheck);
            var selectedPublications = allPublicationsModels.Select((value, index) => new { value, index })
                .Where(t => publicationOrdersToClick.Contains(t.index + 1))
                .Select(t => t.value).ToList();
            var publicationsQueries = new PublicationsQueries();
            for (int i = 0; i < publicationOrdersToClick.Count; i++)
            {
                var currentPublication = selectedPublications[i];
                Logger.Instance.Info($"Click on the '{currentPublication.PublicationTitle}' pubs for '{currentPublication.PublicationDescription}'");
                publicationsForm.ClickOnPublicationLink(publicationOrdersToClick[i]);
                var topicName = currentPublication.PublicationTitle.Split('\'')[0].Split('£')[0].Trim();
                var currentPublicationsDbModels = publicationsQueries.SelectPublicationsByTitleDate(topicName,
                    Parsing.ConvertToShortDateString(currentPublication.PublicationDate));
                var currentPublicationDbModel = currentPublicationsDbModels.FirstOrDefault();
                if (currentPublicationDbModel == null)
                {
                    Checker.Fail($"No Db models for publication '{currentPublication.PublicationTitle}' in DB for '{currentPublication.PublicationDescription}'");
                }
                else
                {
                    CheckPublicationOpening(currentPublication, currentPublicationDbModel);
                }                
            }
        }
    }
}