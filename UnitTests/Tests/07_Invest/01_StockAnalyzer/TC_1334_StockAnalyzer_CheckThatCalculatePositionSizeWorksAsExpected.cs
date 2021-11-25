using AutomatedTests.ConstantVariables;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.Tools.StockAnalyzer;
using AutomatedTests.Enums.Tools;
using AutomatedTests.Enums;
using AutomatedTests.Forms.ResearchPages.PositionSizeForm;
using AutomatedTests.Forms.ResearchPages.StockAnalyzer;
using AutomatedTests.Forms;
using AutomatedTests.SetUpsTearDowns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._07_Invest._01_StockAnalyzer
{
    [TestClass]
    public class TC_1334_StockAnalyzer_CheckThatCalculatePositionSizeWorksAsExpected : BaseTestUnitTests
    {
        private const int TestNumber = 1334;

        private string ticker;
        private string positionSizeDescription;

        [TestInitialize]
        public void TestInitialize()
        {
            ticker = GetTestDataAsString(nameof(ticker));
            positionSizeDescription = GetTestDataAsString(nameof(positionSizeDescription));

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsLifetime));

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1334$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("Test checks that reselection of the ticker works as expected: https://tr.a1qa.com/index.php?/cases/view/19234185")]
        [TestCategory("Smoke"), TestCategory("StockAnalyzer"), TestCategory("PositionSize")]
        public override void RunTest()
        {
            LogStep(1, $"Enter '{ticker}' in Search for Ticker field");
            new MainMenuForm().SetSymbol(ticker);
            var stockAnalyzerForm = new StockAnalyzerForm();
            stockAnalyzerForm.AssertIsOpen();

            LogStep(2, "Click 'Calculate Position Size' button");
            stockAnalyzerForm.ClickAdditionalActionsButton(StockAnalyzerAdditionalButtonTypes.CalculatePositionSize);

            LogStep(3, "Compare values ​​with values ​​from steps 2-4");
            var positionSizeForm = new PositionSizeCalculatorForm();
            Checker.CheckEquals(positionSizeDescription, positionSizeForm.GetPageDescription(), "Page Description is not as expected");
            Checker.CheckEquals(PositionSizeSourceTypes.IndividualSecurities, positionSizeForm.GetSelectedSource(), "Position Size Source is not as expected");
            Checker.CheckEquals(ticker, 
                positionSizeForm.GetTextFromPositionsAutocompleteDataFields(PositionForManualPortfolioCreateInformation.Ticker, Constants.DefaultOrderOfSameItemsToReturn), 
                "Ticker is not as expected");
        }
    }
}