using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums;
using AutomatedTests.Forms.PortfolioTracker;
using AutomatedTests.Forms;
using AutomatedTests.Models.PT2;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;
using TradeStops.Common.Utils;
using AutomatedTests.Enums.PT2;
using AutomatedTests.Enums.Portfolios;

namespace UnitTests.Tests._12_PT2_0
{
    [TestClass]
    public class TC_1412_PT2_PositionCreationAndApplyingView : BaseTestUnitTests
    {
        private const int TestNumber = 1412;

        private const int minPercentColumn = 30;
        private const int maxPercentColumn = 70;

        private string viewName;
        private string fileName;
        private string fileNamePattern;

        private PtPortfolioModel portfolioModel;
        private List<PtPositionAtCreatingPortfolioModel> positionsModel = new List<PtPositionAtCreatingPortfolioModel>();
        private Dictionary<PtPositionTabViewSectionType, int> quantityOfCheckboxesInSections;

        [TestInitialize]
        public void TestInitialize()
        {
            viewName = StringUtility.RandomString(GetTestDataAsString(nameof(viewName)));

            portfolioModel = new PtPortfolioModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Currency = GetTestDataAsString("Currency"),
                PubCode = GetTestDataAsString("PubCode")
            };
            
            var positionsQuantity = GetTestDataAsInt("positionsQuantity");
            var entryDate = GetTestDataAsString("EntryDate");
            var shares = GetTestDataAsString("Shares");
            for (int i = 1; i <= positionsQuantity; i++)
            {                
                var currentSymbol = GetTestDataAsString($"Symbol{i}");
                var currentTradeType = GetTestDataParsedAsEnumFromStringMapping<PositionTradeTypes>($"positionType{i}");
                var currentAssetType = GetTestDataParsedAsEnumFromStringMapping<PositionAssetTypes>($"assetType{i}");
                
                positionsModel.Add(new PtPositionAtCreatingPortfolioModel
                {
                    Ticker = currentSymbol,
                    EntryDate = entryDate,
                    Quantity = shares,
                    TradeType = currentTradeType,
                    PositionAssetType = currentAssetType
                });
            }

            quantityOfCheckboxesInSections = new Dictionary<PtPositionTabViewSectionType, int>
            {
                {PtPositionTabViewSectionType.TradeSmithIndicators, GetTestDataAsInt("quantityColumn1")},
                {PtPositionTabViewSectionType.PositionDetails, GetTestDataAsInt("quantityColumn2")},
                {PtPositionTabViewSectionType.PositionDetailsCont, GetTestDataAsInt("quantityColumn3")},
                {PtPositionTabViewSectionType.Gains, GetTestDataAsInt("quantityColumn4")},
                {PtPositionTabViewSectionType.Summary, GetTestDataAsInt("quantityColumn5")},
                {PtPositionTabViewSectionType.Fundamentals, GetTestDataAsInt("quantityColumn6")},
                {PtPositionTabViewSectionType.AlertDetails, GetTestDataAsInt("quantityColumn7")}
            };

            fileName = GetTestDataAsString("fileName");
            fileNamePattern = GetTestDataAsString("fileName1");

            var userProductSubscriptions = GetUserProductSubscriptions("userSubscription");

            LogStep(0, "Precondition - Login as user with subscription to Portfolio Tracker");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, userProductSubscriptions));

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);            
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [Description("The test checks creation portfolio, positions and publishing position https://tr.a1qa.com/index.php?/cases/edit/22890372")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1412$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PT20")]
        public override void RunTest()
        {
            LogStep(1, "Check that Add Portfolio form is shown");
            var mainMenuNavigation = new MainMenuNavigation();
            mainMenuNavigation.OpenPtAddPortfolioForm();
            var portfolioCreationForm = new PtPortfolioCreationForm();
            portfolioCreationForm.AssertIsOpen();

            LogStep(2, "Fill portfolio and positions field and click Save");
            portfolioCreationForm.CreatePtPortfolioWithPositions(portfolioModel, positionsModel);
            
            LogStep(3, "Click on created portfolio");
            var manageForm = new ManageForm();            
            manageForm.ClickOnPortfolioName(portfolioModel.Name);
            
            LogStep(4, "Click add view button");
            var positionTabForm = new PtPositionsTabForm();
            positionTabForm.ClickAddViewButton();
            
            LogStep(5, "Check that checkboxes for column is shown");
            positionTabForm.TypeNameForView(viewName);
            Checker.IsTrue(positionTabForm.GetColumnCheckboxLabelNames().Any(), "There are no views checkboxes for any section");
            foreach (var item in EnumUtils.GetValues<PtPositionTabViewSectionType>())
            {
                var viewsColumn = positionTabForm.GetColumnCheckboxLabelNamesBySection(item.GetStringMapping()).Count;
                Checker.CheckEquals(quantityOfCheckboxesInSections[item], viewsColumn, "Views column are not shown");
            }
            positionTabForm.CheckRandomCheckboxesWithoutDuplicate(minPercentColumn, maxPercentColumn);
            positionTabForm.ClickSaveViewButton();
            
            LogStep(6, "Check that in grid for selected view {test data: viewName} column are shown");
            positionTabForm.SelectView(viewName);
            var columnHeaders = positionTabForm.GetColumnNamesInTableHeader();
            Checker.IsTrue(columnHeaders.Any(), "There are no columns in table");
            positionTabForm.ClickEditSignForCurrentView();
            var columnCheckBoxes = positionTabForm.GetCheckedColumnCheckboxesLabelNames();
            Checker.IsTrue(columnCheckBoxes.Any(), "There are no checked checkboxes");
            foreach (var item in columnCheckBoxes)
            {
              Checker.IsTrue(columnHeaders.Contains(item), $"Column {item} in table are not present in Position tab");
            }
            positionTabForm.ClickCancelButton();

            LogStep(7, "Click on 'CSV' button and check that columns in 'CSV' file is shown according to columns in table");
            var portfolioNameDropDown = positionTabForm.GetPortfolioNamePt();
            var portfolioName = EnumUtils.GetValues<AllPortfoliosKinds>().Select(t => t.GetStringMapping()).Contains(portfolioNameDropDown)
                ? string.Format(fileNamePattern, portfolioNameDropDown)
                : portfolioModel.Name;
            positionTabForm.ClickGridActionButton(GridActionsButton.Export);
            var path = $"{GetDownloadedFilePathGridDepended()}{string.Format(fileName, portfolioName)}";
            FileUtilsExtension.WaitUntilFileIsDownloaded(path);
            Assert.IsTrue(FileUtilsExtension.IsFileExistGridDepended(path), $"File {path} is not present");

            LogStep(8, "Check that columns in 'CSV' file is shown according to");
            var positionsCsvColumnNames = FileUtilsExtension.GetCsvColumnsNames(path, columnHeaders.Count);
            Checker.IsTrue(positionsCsvColumnNames.Any(), "There are no columns in CSV file");
            foreach (var item in positionsCsvColumnNames)
            {
                Checker.IsTrue(columnHeaders.Contains(item), "Columns in CSV are not fully displayed");
            }

            LogStep(9, "Go to Manage tab. Click on action button for created portfolio according to {test data: PortfolioName} and click to 'Manage Publication'.");
            mainMenuNavigation.OpenPtManageForm();                     
            var managePtForm = new ManageForm();
            var portfolioId = managePtForm.GetPortfolioIdViaName(portfolioModel.Name);
            managePtForm.ClickManagePublicationViaPortfolioId(portfolioId);

            LogStep(10, $"Check that created View {viewName} is shown on drop-down 'View Template'. Select created View {viewName}");
            var ptPortfolioPublishViewForm = new PtPublishViewForm();
            new PtPublishPortfolioForm().SelectCreatedViewWithScrollingThePage(viewName);

            LogStep(11, $"Check that checkboxes for {viewName} are set in Manage Publication form");
            var publishCheckBoxes = ptPortfolioPublishViewForm.GetCheckedColumnCheckboxesLabelNames();
            Checker.IsTrue(publishCheckBoxes.Any(), $"There no checked checkboxes in created {viewName}");
            Checker.CheckListsEquals(columnCheckBoxes, publishCheckBoxes, "Not all views checkboxes are checked");
            
            LogStep(12, $"Verify that columns are shown for {viewName}");
            var publishColumnHeaders = ptPortfolioPublishViewForm.GetColumnNamesInTableHeaderPubs();
            Checker.IsTrue(publishColumnHeaders.Any(), "There are no columns in grid");
            foreach (var item in publishCheckBoxes)
            {
                Checker.IsTrue(publishColumnHeaders.Contains(item), $"Column {item} in table are not present in Publication Manage Tab");
            }

            TearDowns.DeleteCreatedViewInPositionsTabForPubs(viewName);
            TearDowns.DeletePtPortfolios(UserModels.First(), portfolioId);
        }
    }
}