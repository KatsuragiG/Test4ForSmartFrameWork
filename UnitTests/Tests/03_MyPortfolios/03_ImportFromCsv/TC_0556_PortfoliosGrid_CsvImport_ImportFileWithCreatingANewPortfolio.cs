using System;
using System.Collections.Generic;
using System.Linq;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.PositionsAlertsForm.Models;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._03_MyPortfolios._03_ImportFromCsv
{
    [TestClass]
    public class TC_0556_PortfoliosGrid_CsvImport_ImportFileWithCreatingANewPortfolio : BaseTestUnitTests
    {
        private const int TestNumber = 556;
        private const string FileNameForValidImportedFile = "valid_TC_554_and_556.csv";
        private const string ViewNameForAddedView = "view";
        private const string PortfolioName = "portfolio";

        private List<PositionsGridDataField> columns;
        private readonly List<PositionGridModel> expectedPositions = new List<PositionGridModel>();
        private int positionsQuantity;

        [TestInitialize]
        public void TestInitialize()
        {
            columns = new List<PositionsGridDataField>
            {
                PositionsGridDataField.Ticker,
                PositionsGridDataField.EntryDate,
                PositionsGridDataField.TradeType,
                PositionsGridDataField.Shares,
                PositionsGridDataField.EntryPrice,
                PositionsGridDataField.Commissions
            };
            positionsQuantity = GetTestDataAsInt(nameof(positionsQuantity));
            for (int i = 1; i <= positionsQuantity; i++)
            {
                expectedPositions.Add(new PositionGridModel
                {
                    Ticker = TestContext.DataRow[$"Symbol{i}"].ToString(),
                    EntryDate = TestContext.DataRow[$"EntryDate{i}"].ToString(),
                    EntryPrice = TestContext.DataRow[$"EntryPrice{i}"].ToString(),
                    TradeType = TestContext.DataRow[$"LS{i}"].ToString(),
                    Shares = TestContext.DataRow[$"Shares{i}"].ToString(),
                    Commissions = TestContext.DataRow[$"Commissions{i}"].ToString()
                });
            }
            var positionModel = new PositionsDBModel
            {
                Symbol = GetTestDataAsString("additionalTicker"),
                TradeType = $"{(int)PositionTradeTypes.Short}"
            };

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPlatinum));
            var portfolio = PortfoliosSetUp.AddInvestmentPortfoliosDefaultUSD(UserModels.First().Email);
            PositionsAlertsSetUp.AddPositionViaDB(portfolio, positionModel);
            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);

            new MainMenuNavigation().OpenPositionsGrid();
            new PositionsTabForm().AddNewViewWithAllCheckboxesMarked(ViewNameForAddedView);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DeploymentItem("TestEnvData\\")]
        [DeploymentItem("valid_TC_554_and_556.csv")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_556$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("https://tr.a1qa.com/index.php?/cases/view/19232501 The test checks possibility to import file with creating a new portfolio.")]
        [TestCategory("Smoke"), TestCategory("PositionsGrid"), TestCategory("CSVImport"), TestCategory("AddPortfolio")]
        public override void RunTest()
        {
            LogStep(1, 2, "Open 'Import Portfolio from CSV' page. Click link 'Click here");
            var addPortfoliosSteps = new AddPortfoliosSteps();
            var importPortfolioFromCsvForm = addPortfoliosSteps.NavigateToImportPortfolioFromCsvFormSelectFileClickImport(FileNameForValidImportedFile);

            LogStep(3, 4, "Assign columns:- First column: 'Symbol'- Second column: 'Entry Date'- Third column: 'Trade Type'- Fourth column: 'Shares'- Fifth column: 'Entry Price'" +
                "- Sixth column: 'Entry Commission'Remember data for all columns and strings.Remember count of positions.");
            addPortfoliosSteps.MadeMappingColumnsToHeaderForValidCsvFile();
            Assert.IsFalse(importPortfolioFromCsvForm.IsErrorMessagePresent(), "Error message is present");

            LogStep(5, "Click 'Next' button.");
            importPortfolioFromCsvForm.ClickNext();

            LogStep(6, "Click button 'Create a new portfolio'.Enter portfolio name(ex, 'test').");
            importPortfolioFromCsvForm.AddToANewPortfolio(PortfolioName);

            LogStep(7, "Click button 'Finish and import data'");
            importPortfolioFromCsvForm.ClickFinish();
            var createdDate = Parsing.ConvertToShortDateString(DateTime.Now.ToShortDateString());

            LogStep(8, "Make sure data is as expected. Check number as expected. Note: check data for all strings and columns.");
            var positionsTab = new PositionsTabForm();
            positionsTab.SelectView(ViewNameForAddedView);
            var positionsData = positionsTab.GetPositionDataForAllPositions(columns);
            Assert.AreEqual(positionsData.Count, positionsQuantity, "Count of positions is not equals");

            foreach (var positionGridModel in expectedPositions)
            {
                var appropriateRow = positionsData.FirstOrDefault(t => t.Ticker.Contains(positionGridModel.Ticker));
                if (appropriateRow == null)
                {
                    Checker.Fail($"No records in grid for ticker {positionGridModel.Ticker}");
                }
                else
                {
                    var inequalNotNullProperties = ObjectComparator.CompareTwoModelsGetUnequalProperties<PositionGridModel>(appropriateRow, positionGridModel);
                    Checker.CheckEquals(inequalNotNullProperties.Count, 0,
                        $"Next columns are not equal {inequalNotNullProperties.Aggregate("", (current, positionInformation) => current + positionInformation.ToString())} " +
                        $"for {positionGridModel.Ticker}");
                }
            }

            LogStep(9, "Open portfolio grid and check created date");
            new MainMenuNavigation().OpenPortfolios(PortfolioType.Investment);
            var portfoliosForm = new PortfoliosForm();
            var portfolioId = portfoliosForm.GetPortfolioIdViaName(PortfolioName);
            Checker.CheckEquals(createdDate,
                new PortfolioGridsSteps().RememberPortfolioInformationForPortfolioId(portfolioId).CreatedDate,
                "Created Date for created portfolio is not as expected");
        }
    }
}