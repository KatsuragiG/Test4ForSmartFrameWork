using System.Linq;
using AutomatedTests.Enums;
using AutomatedTests.Forms;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._01_General
{
    [TestClass]
    public class TC_1332_General_SearchForTicker_SearchForTickerCarriedOnlyAmongActiveStocks : BaseTestUnitTests
    {
        private const int TestNumber = 1332;

        private string validTickerName;
        private string validTickerNasdaq;
        private string validTickerCompany;
        private string invalidTickerName;
        private int countOfTreeSelectItems;
        private string itemPattern;

        [TestInitialize]
        public void TestInitialize()
        {
            validTickerName = GetTestDataAsString(nameof(validTickerName));
            validTickerNasdaq = GetTestDataAsString(nameof(validTickerNasdaq));
            validTickerCompany = GetTestDataAsString(nameof(validTickerCompany));
            invalidTickerName = GetTestDataAsString(nameof(invalidTickerName));
            itemPattern = GetTestDataAsString(nameof(itemPattern));
            countOfTreeSelectItems = GetTestDataAsInt(nameof(countOfTreeSelectItems));

            LogStep(0, "Precondition - Login as registered user");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPlatinum));

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1332$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("Test for Search for ticker carried only among active stocks: https://tr.a1qa.com/index.php?/cases/view/19234216")]
        [TestCategory("Smoke"), TestCategory("SearchForTicker"), TestCategory("Dashboard")]
        public override void RunTest()
        {
            LogStep(1, $"Enter '{validTickerName}' in Search for Ticker field");
            var mainMenuForm = new MainMenuForm();
            var treeSelectItems = mainMenuForm.GetItemsInSymbolTreeSelectAutocomplete(validTickerName);
            var actualTreeSelectItems = treeSelectItems.Select(e => e.ReplaceNewLineWithTrim()).ToList();
            var expectedItemName = string.Format(itemPattern, validTickerName, validTickerNasdaq, validTickerCompany);

            Checker.CheckEquals(countOfTreeSelectItems, treeSelectItems.Count, 
                $"Count of tree select items is not as expected.\nFound items:\n{string.Join("\n", actualTreeSelectItems)}");
            Checker.IsTrue(actualTreeSelectItems.Contains(expectedItemName),
                $"List of tree select items doesn't contain expected item [{expectedItemName}]\nFound items:\n{string.Join("\n", actualTreeSelectItems)}");

            LogStep(2, $"Enter '{invalidTickerName}' in Search for Ticker field");
            Checker.IsTrue(mainMenuForm.IsNonResultsTipTreeSelectAutocompletePresent(invalidTickerName),
                $"Non results tip is not present after entering '{invalidTickerName}' symbol");
        }
    }
}