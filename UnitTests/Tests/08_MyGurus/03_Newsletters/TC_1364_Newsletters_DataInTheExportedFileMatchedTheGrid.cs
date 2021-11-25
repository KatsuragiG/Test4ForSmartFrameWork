using AutomatedTests.Enums;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Gurus;
using AutomatedTests.Models;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._08_MyGurus._03_Newsletters
{
    [TestClass]
    public class TC_1364_Newsletters_DataInTheExportedFileMatchedTheGrid : BaseTestUnitTests
    {
        private const int TestNumber = 1364;

        private int expectedNumberOfColumns;
        private string fileName;
        private bool isWithSsi;
        private List<string> propertiesToCompare = new List<string>();

        [TestInitialize]
        public void TestInitialize()
        {
            var userProductSubscriptions = GetUserProductSubscriptions("userSubscription");

            expectedNumberOfColumns = GetTestDataAsInt(nameof(expectedNumberOfColumns));
            fileName = GetTestDataAsString(nameof(fileName));
            isWithSsi = GetTestDataAsBool(nameof(isWithSsi));

            propertiesToCompare = Dictionaries.NewslettersGridColumnsNamesObjProperties.Values.ToList();

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));
            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.MyGurus);
            new MainMenuNavigation().OpenCustomPublisherGrid();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1364$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("NewsLettersPage"), TestCategory("Export")]
        [Description("The test checks matching data between the grid and exported csv file. https://tr.a1qa.com/index.php?/cases/view/19234264")]
        public override void RunTest()
        {
            LogStep(1, "Remember grid");
            var selectedPublisherForm = new SelectedPublisherForm();
            var newslettersResultsGrid = selectedPublisherForm.GetAllPositionsDataWithOrWithoutSsi(isWithSsi);
            Assert.IsTrue(newslettersResultsGrid.Any(), "Grid does not contain data");

            LogStep(2, "Click Export. Wait until file is downloaded");
            selectedPublisherForm.ClickActionButton(GridActionsButton.Export);
            var path = $"{GetDownloadedFilePathGridDepended()}{fileName}";
            FileUtilsExtension.WaitUntilFileIsDownloaded(path);
            Assert.IsTrue(FileUtilsExtension.IsFileExistGridDepended(path), $"File {path} is not present");

            LogStep(3, "Open csv. file and compare data in exported file and in the grid. Check that CSV file contains the same quantity of records like grid. Adjust csv to grid (there are maybe some difference for tickers with same names but different exchanges)");
            var positionsCsvData = FileUtilsExtension.ParseCsvIntoObjects<SelectedPublisheGridRowModel>(path, expectedNumberOfColumns);
            FileUtilsExtension.DeleteFileGridDepended(path);
            Assert.AreEqual(newslettersResultsGrid.Count, positionsCsvData.Count, "Newsletters results counts are not equal");
            foreach (var gridRowModel in positionsCsvData)
            {
                var correspondedRowInGrid = newslettersResultsGrid.FirstOrDefault(u => u.Ticker == gridRowModel.Ticker && u.Advice == gridRowModel.Advice.Replace("\r\n", " ").Replace("  ", " ").Trim());
                if (correspondedRowInGrid == null)
                {
                    Checker.Fail($"No correspondence for position with ticker = '{gridRowModel.Ticker}' and advice = '{gridRowModel.Advice}'");
                }
                else
                {
                    var unequalValues = new SelectedPublisheGridRowModel().AreFieldsValuesEquals(propertiesToCompare, gridRowModel, correspondedRowInGrid);
                    Checker.IsFalse(unequalValues.Any(), unequalValues.Aggregate("", (current, unequalValue) => current + unequalValue + "\n"));
                }
            }
        }
    }
}