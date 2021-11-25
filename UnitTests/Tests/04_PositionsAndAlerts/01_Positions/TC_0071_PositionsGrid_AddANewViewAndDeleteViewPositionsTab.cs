using System.Linq;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Views;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Dashboard;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.User;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Popups;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using TradeStops.Common.Extensions;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._04_PositionsAndAlerts._01_Positions
{
    [TestClass]
    public class TC_0071_PositionsGrid_AddANewViewAndDeleteViewPositionsTab : BaseTestUnitTests
    {
        private const int TestNumber = 71;

        private string portfolioName;
        private string viewNameForAddedView;
        private string popupText;

        [TestInitialize]
        public void TestInitialize()
        {
            var userType = GetTestDataParsedAsEnumFromStringMapping<ProductSubscriptionTypes>("User");

            portfolioName = GetTestDataAsString(nameof(portfolioName));
            viewNameForAddedView = StringUtility.RandomString(GetTestDataAsString(nameof(viewNameForAddedView)));
            popupText = string.Format(GetTestDataAsString(nameof(popupText)), viewNameForAddedView);

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, (ProductSubscriptions)(int)userType));
            PortfoliosSetUp.AddInvestmentUsdPortfoliosWithOpenPosition(UserModels.First().Email);
            LoginSetUp.LogIn(UserModels.First());

            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);
            new MainMenuNavigation().OpenPositionsGrid();
            new PositionsAlertsStatisticsPanelForm().SelectPortfolio(portfolioName);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_71$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PositionsGrid"), TestCategory("View"), TestCategory("ViewAdd"), TestCategory("ViewDelete")]
        [Description("The test checks adding and deleting view for positions grid; https://tr.a1qa.com/index.php?/cases/view/19235128")]
        public override void RunTest()
        {
            LogStep(1, "Click '+Add view'");
            var positionsTabForm = new PositionsTabForm();
            positionsTabForm.ClickAddViewButton();

            LogStep(2, "Type any unique name for View");
            positionsTabForm.TypeNameForView(viewNameForAddedView);

            LogStep(3, 4, "Select all options + Save; Find all selected options in table header of the Positions Grid");
            positionsTabForm.ChooseAllCheckboxAndSave();
            var viewsQueries = new ViewsQueries();
            var viewId = viewsQueries.SelectViewId(UserModels.First().Email, (int)UserViewTypes.Positions, viewNameForAddedView);
            positionsTabForm.ClickEditSignForCurrentView();
            var checkedOptions = positionsTabForm.GetColumnCheckboxLabelNames().Select(t => t.ParseAsEnumFromStringMapping<PositionsGridDataField>()).ToList();
            positionsTabForm.ClickCancelButton();

            LogStep(5, "In DB: make sure expected columns are present in view");
            var userViewColumnTypes = viewsQueries.SelectUserViewColumns(viewId, (int)UserViewTypes.Positions).Select(col => col.ColumnType);

            var userViewColumnsNames = userViewColumnTypes.Cast<ViewColumnTypes>().ToList().Select(t => t.ToString())
                .Where(c => !c.EqualsIgnoreCase(GeneralTablesHeaders.Ticker.GetStringMapping())).ToList();

            foreach (string dbColumnName in userViewColumnsNames)
            {
                Checker.IsTrue(checkedOptions.Contains(Dictionaries.PositionDbColumnToGridColumnMapping[dbColumnName.Trim()]), 
                    $"Column in DB {dbColumnName} does not mapped to any frontcheckbox");
                if (checkedOptions.Contains(Dictionaries.PositionDbColumnToGridColumnMapping[dbColumnName.Trim()]))
                {
                    checkedOptions.Remove(Dictionaries.PositionDbColumnToGridColumnMapping[dbColumnName.Trim()]);
                }
            }
            Checker.IsFalse(checkedOptions.Any(), $"Unexpected columns in front \n {checkedOptions.Aggregate("", (current, column) => current + column.GetStringMapping())}");

            LogStep(6, 7, "Click edit sign for created View; Click delete button for deleting created view");
            positionsTabForm.ClickEditSignForCurrentView();
            positionsTabForm.ClickDeleteView();

            LogStep(8, "Confirm deleting by Clicking Yes");
            var deleteViewPopup = new ConfirmPopup(PopupNames.Confirm);
            Checker.IsTrue(deleteViewPopup.GetMessage().Contains(popupText), $"Popup window text doesn't contain {popupText}");
            deleteViewPopup.ClickYesButton();

            LogStep(9, "In DB: make sure the view is deleted");
            Checker.CheckEquals(0, new ViewsQueries().SelectUserViewColumns(viewId, (int)UserViewTypes.Positions).Count, "The view was not deleted in DB");
        }
    }
}