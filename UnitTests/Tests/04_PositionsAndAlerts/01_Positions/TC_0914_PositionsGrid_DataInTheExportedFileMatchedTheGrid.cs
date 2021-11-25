using System.Collections.Generic;
using System.Linq;
using AutomatedTests;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.PositionsAlertsForm.Models;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._04_PositionsAndAlerts._01_Positions
{
    [TestClass]
    public class TC_0914_PositionsGrid_DataInTheExportedFileMatchedTheGrid : BaseTestUnitTests
    {
        private const int TestNumber = 914;

        private readonly List<PositionsDBModel> positionsModels = new List<PositionsDBModel>();
        private int expectedNumberOfColumns;
        private int positionsQuantity;
        private string fileName;
        private string viewNameForAddedView;
        private List<PositionsGridDataField> columnsToExport;

        [TestInitialize]
        public void TestInitialize()
        {
            viewNameForAddedView = StringUtility.RandomString("########");
            positionsQuantity = GetTestDataAsInt(nameof(positionsQuantity));
            for (int i = 1; i <= positionsQuantity; i++)
            {
                positionsModels.Add(new PositionsDBModel
                {
                    Symbol = GetTestDataAsString($"Symbol{i}"),
                    TradeType = ((int)GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>($"TradeType{i}")).ToString(),
                    CurrencyId = $"{(int)GetTestDataParsedAsEnumFromStringMapping<Currency>($"Currency{i}")}",
                    Notes = StringUtility.RandomStringOfSize(Constants.DefaultSizeOfDateStringToClearField)
                });
            }
            expectedNumberOfColumns = GetTestDataAsInt(nameof(expectedNumberOfColumns));

            var userProductSubscriptions = GetUserProductSubscriptions("user");

            columnsToExport = userProductSubscriptions.First() == ProductSubscriptions.TradeStopsBasic 
                ? Instance.GetListOfAllPositionsBasicColumns()
                : Instance.GetListOfAllPositionsColumns();
            fileName = GetTestDataAsString(nameof(fileName));

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));
            var portfolioId = PortfoliosSetUp.AddInvestmentPortfoliosDefaultUSD(UserModels.First().Email);
            foreach (var positionModel in positionsModels)
            {
                PositionsAlertsSetUp.AddPositionViaDB(portfolioId, positionModel);
            }
            LoginSetUp.LogIn(UserModels.First());

            PortfoliosSetUp.ImportDagSiteInvestment06(true);
            new MainMenuNavigation().OpenPositionsGrid();
            new PositionsTabForm().AddNewViewWithAllCheckboxesMarked(viewNameForAddedView);
            new PositionsAlertsStatisticsPanelForm().SelectPortfolio(AllPortfoliosKinds.All.GetStringMapping());
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_914$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test checks matching data between Positions grid and exported csv file. https://tr.a1qa.com/index.php?/cases/view/19232191")]
        [TestCategory("Smoke"), TestCategory("PositionsGrid"), TestCategory("Export")]
        public override void RunTest()
        {
            LogStep(1, "Remember data for Positions grid");
            var positionsTabForm = new PositionsTabForm();
            var positionsData = positionsTabForm.GetPositionDataForAllPositions(columnsToExport);
            Assert.IsTrue(positionsData.Count > 0, "There are no data in the grid");

            LogStep(2, "Click export");
            positionsTabForm.ClickGridActionButton(GridActionsButton.Export);
            var path = $"{GetDownloadedFilePathGridDepended()}{fileName}";
            FileUtilsExtension.WaitUntilFileIsDownloaded(path);
            Assert.IsTrue(FileUtilsExtension.IsFileExistGridDepended(path), $"File {path} is not present");

            LogStep(3, "Make sure data in the exported file matched the Positions grid");
            var positionsCsvData = FileUtilsExtension.ParseCsvIntoObjects<PositionGridModel>(path, expectedNumberOfColumns);
            FileUtilsExtension.DeleteFileGridDepended(path);
            Assert.AreEqual(positionsCsvData.Count, positionsData.Count, "Quantity of records in grid and CSV are not matched");
            foreach (PositionGridModel model in positionsCsvData)
            {
                var mappedModel = positionsData.FirstOrDefault(u => u.Ticker == model.Ticker);
                if (mappedModel == null)
                {
                    Checker.Fail($"Position {model.Ticker} from CSV not found in grid");
                }
                else
                {
                    var unequalValues = new PositionGridModel().GetModelsDifferenceDescription(Dictionaries.PositionsColumnsNamesAndObjProperties.Values.ToList(),
                        model, mappedModel);
                    Checker.IsTrue(unequalValues.Count == 0, unequalValues.Aggregate("", (current, unequalValue) => current + unequalValue + "\n"));
                }
            }
        }
    }
}