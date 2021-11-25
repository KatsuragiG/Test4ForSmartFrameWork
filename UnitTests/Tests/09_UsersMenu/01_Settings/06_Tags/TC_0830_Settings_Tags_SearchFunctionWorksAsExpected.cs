using System.Collections.Generic;
using System.Linq;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Positions;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._09_UsersMenu._01_Settings._06_Tags
{
    [TestClass]
    public class TC_0830_Settings_Tags_SearchFunctionWorksAsExpected : BaseTestUnitTests
    {
        private const int TestNumber = 830;

        private readonly List<string> allTagsList = new List<string>();
        private List<string> specialCharacterTagsList;
        private List<string> numberCharacterTagsList;
        private List<string> uppercaseCharacterTagsList;
        private List<string> lowercaseCharacterTagsList;


        [TestInitialize]
        public void TestInitialize()
        {
            var tagsQuantity = GetTestDataAsInt("tagsQuantity");

            var positionsModels = new List<PositionsDBModel>
            {
                new PositionsDBModel
                {
                    Symbol = GetTestDataAsString("Stock"),
                    TradeType = $"{(int)PositionTradeTypes.Long}"
                },
                new PositionsDBModel
                {
                    Symbol = GetTestDataAsString("Option"),
                    TradeType = $"{(int)PositionTradeTypes.Short}"
                },
                new PositionsDBModel
                {
                    Symbol = GetTestDataAsString("Stock"),
                    TradeType = $"{(int)PositionTradeTypes.Short}"
                },
                new PositionsDBModel
                {
                    Symbol = GetTestDataAsString("Option"),
                    TradeType = $"{(int)PositionTradeTypes.Long}"
                }
            };
            for (int i = 1; i <= tagsQuantity; i++)
            {
                allTagsList.Add(TestContext.DataRow[$"Tag{i}"].ToString());
            }
            specialCharacterTagsList = new List<string> { allTagsList[1] };
            numberCharacterTagsList = new List<string> { allTagsList[4] };
            uppercaseCharacterTagsList = new List<string>
            {
                allTagsList[0],
                allTagsList[1],
                allTagsList[2],
                allTagsList[3],
                allTagsList[4],
                allTagsList[5],
                allTagsList[6]
            };
            lowercaseCharacterTagsList = new List<string>
            {
                allTagsList[0],
                allTagsList[1],
                allTagsList[2],
                allTagsList[3],
                allTagsList[4],
                allTagsList[5],
                allTagsList[6]
            };

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPlatinum));
            var portfolioManualInvestment = PortfoliosSetUp.AddInvestmentPortfoliosDefaultUSD(UserModels.First().Email);
            var positionsQueries = new PositionsQueries();
            foreach (var currentTag in allTagsList)
            {
                positionsQueries.AddNewRowIntoUserTags(UserModels.First().TradeSmithUserId, currentTag);
            }

            var position1 = PositionsAlertsSetUp.AddPositionViaDB(portfolioManualInvestment, positionsModels[0]);
            positionsQueries.AddNewRowIntoUserPositionTags(UserModels.First().TradeSmithUserId, allTagsList[0], position1);
            positionsQueries.AddNewRowIntoUserPositionTags(UserModels.First().TradeSmithUserId, allTagsList[1], position1);

            var position2 = PositionsAlertsSetUp.AddPositionViaDB(portfolioManualInvestment, positionsModels[1]);
            positionsQueries.AddNewRowIntoUserPositionTags(UserModels.First().TradeSmithUserId, allTagsList[0], position2);
            positionsQueries.AddNewRowIntoUserPositionTags(UserModels.First().TradeSmithUserId, allTagsList[2], position2);
            positionsQueries.AddNewRowIntoUserPositionTags(UserModels.First().TradeSmithUserId, allTagsList[3], position2);
            positionsQueries.AddNewRowIntoUserPositionTags(UserModels.First().TradeSmithUserId, allTagsList[4], position2);

            var position3 = PositionsAlertsSetUp.AddPositionViaDB(portfolioManualInvestment, positionsModels[2]);
            positionsQueries.AddNewRowIntoUserPositionTags(UserModels.First().TradeSmithUserId, allTagsList[5], position3);
            positionsQueries.AddNewRowIntoUserPositionTags(UserModels.First().TradeSmithUserId, allTagsList[6], position3);

            var position4 = PositionsAlertsSetUp.AddPositionViaDB(portfolioManualInvestment, positionsModels[3]);
            positionsQueries.AddNewRowIntoUserPositionTags(UserModels.First().TradeSmithUserId, allTagsList[0], position4);
            positionsQueries.AddNewRowIntoUserPositionTags(UserModels.First().TradeSmithUserId, allTagsList[2], position4);

            new SettingsSteps().LoginNavigateToSettingsTagsGetForm(UserModels.First());
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_830$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test checks search function works as expected for Tags field. https://tr.a1qa.com/index.php?/cases/view/19232294")]
        [TestCategory("Smoke"), TestCategory("SettingsPage"), TestCategory("SettingsPageTagsTab"), TestCategory("TagsNotes")]
        public override void RunTest()
        {
            LogStep(1, 2, "Put cursor in the search field. Click Enter. Start typing name of the tag which started with special characters.");
            SearchForTagsAndAssertTagsAreAsExpected(allTagsList[1].Substring(0, 1), specialCharacterTagsList);

            LogStep(3, "Start typing name of the tag which started with number.");
            SearchForTagsAndAssertTagsAreAsExpected(allTagsList[4].Substring(0, 1), numberCharacterTagsList);

            LogStep(4, "Start typing name of the tag which started with Uppercase letter (Ex, Tag).");
            SearchForTagsAndAssertTagsAreAsExpected("Tag", uppercaseCharacterTagsList);

            LogStep(5, "Start typing name of the tag which started with Lowercase letter (Ex, tag).");
            SearchForTagsAndAssertTagsAreAsExpected("tag", lowercaseCharacterTagsList);
        }

        private void SearchForTagsAndAssertTagsAreAsExpected(string tagForSearch, List<string> expectedTags)
        {
            var actualTagsAfterSearching = new SettingsSteps().TypeATagGetAllListedTags(tagForSearch);
            Checker.IsTrue(actualTagsAfterSearching.Count > 0, $"Tags Count is equals 0 at search {tagForSearch}");
            foreach (var expectedTag in expectedTags)
            {
                Checker.IsTrue(actualTagsAfterSearching.Any(x => x.Contains(expectedTag)), $"{expectedTag} is not present");
            }
        }
    }
}