using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Forms.ResearchPages.PureQuant;
using AutomatedTests.Forms;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.ResearchPages;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._07_Invest._03_PureQuant
{
    [TestClass]
    public class TC_0930_Invest_PureQuant_DataInTheExportedFileMatchedTheGrid : BaseTestUnitTests
    {
        private const int TestNumber = 930;

        private PortfolioModel portfolioModel;
        private int expectedNumberOfColumns;
        private string fileName;
        private List<string> propertiesToCompare = new List<string>();

        [TestInitialize]
        public void TestInitialize()
        {
            portfolioModel = new PortfolioModel
            {
                Name = GetTestDataAsString("PortfolioName1"),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType1"),
                Currency = Currency.USD.ToString()
            };

            var positionsQuantity = GetTestDataAsInt("positionsQuantity");
            var positionsModels = new List<PositionsDBModel>();
            for (int i = 1; i <= positionsQuantity; i++)
            {
                positionsModels.Add(new PositionsDBModel
                {
                    Symbol = GetTestDataAsString($"Symbol{i}"),
                    TradeType = $"{(int)PositionTradeTypes.Long}",
                    CurrencyId = $"{(int)GetTestDataParsedAsEnumFromStringMapping<Currency>($"Currency{i}")}"
                });
            }
            expectedNumberOfColumns = GetTestDataAsInt(nameof(expectedNumberOfColumns));
            fileName = GetTestDataAsString(nameof(fileName));
            propertiesToCompare = Dictionaries.PureQuantColumnsNamesAndObjProperties.Values.Select(s => s.Replace("SsiStatusString", "SsiStatus")).ToList();

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsLifetime));
            var portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            foreach (var positionModel in positionsModels)
            {
                PositionsAlertsSetUp.AddPositionViaDB(portfolioId, positionModel);
            }
            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Invest);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_930$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("ResearchPage"), TestCategory("PureQuantTool"), TestCategory("Export")]
        [Description("The test checks matching data between the grid and exported csv file. https://tr.a1qa.com/index.php?/cases/view/19232167")]
        public override void RunTest()
        {
            LogStep(1, "Select the manual portfolio on the dropdown 'Select a portfolio'. Click 'Run Research' button.");
            new PureQuantSteps().OpenPureQuantSelectPortfolioClickRunResearchGetPureQuantForm(portfolioModel.Name);
            var pureQuantResultsForm = new PureQuantResultsForm();
            Checker.IsTrue(pureQuantResultsForm.IsGridTablePresent(), "Grid does not shown");

            LogStep(2, "Remember data for Pure Quant grid.");
            var pureQuantResultsGrid = pureQuantResultsForm.GetPureQuantGrid();
            Assert.IsTrue(pureQuantResultsGrid.Any(), "Grid does not contain data");

            LogStep(3, "Click export");
            pureQuantResultsForm.ClickExport();

            LogStep(4, "Make sure data in the exported file matched the Pure Quant grid.");
            var path = $"{GetDownloadedFilePathGridDepended()}{fileName}";
            FileUtilsExtension.WaitUntilFileIsDownloaded(path);
            Assert.IsTrue(FileUtilsExtension.IsFileExistGridDepended(path), $"File {path} is not present");

            var positionsCsvData = FileUtilsExtension.ParseCsvIntoObjects<PureQuantGridModel>(path, expectedNumberOfColumns);
            FileUtilsExtension.DeleteFileGridDepended(path);
            Assert.AreEqual(pureQuantResultsGrid.Count, positionsCsvData.Count, "Pure Quant results counts are not equal");
            foreach (var model in positionsCsvData)
            {
                var correspondedModel = pureQuantResultsGrid.FirstOrDefault(u => u.Ticker == model.Ticker);
                if (correspondedModel == null)
                {
                    Checker.Fail($"No records in CSV with ticker = {model.Ticker}");
                }
                else
                {
                    var unequalValues = new PureQuantGridModel().GetModelsDifferenceDescription(propertiesToCompare, model, pureQuantResultsGrid.FirstOrDefault(u => u.Ticker == model.Ticker));
                    Checker.CheckEquals(0, unequalValues.Count, unequalValues.Aggregate("", (current, unequalValue) => current + unequalValue + "\n"));
                }
            }
        }
    }
}