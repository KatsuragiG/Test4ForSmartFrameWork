using System.Collections.Generic;
using System.Linq;
using AutomatedTests.Database.Views;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.User;
using AutomatedTests.Forms;
using AutomatedTests.Forms.MyPortfolios;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._04_PositionsAndAlerts._01_Positions
{
    [TestClass]
    public class TC_0075_PositionsGrid_SelectViewPositionsTab : BaseTestUnitTests
    {
        private const int TestNumber = 75;
        private readonly List<string> viewsNames = new List<string>();

        [TestInitialize]
        public void TestInitialize()
        {
            var userType = GetTestDataParsedAsEnumFromStringMapping<ProductSubscriptionTypes>("User");

            var viewsQuantity = GetTestDataAsInt("viewsQuantity");
            for (int i = 0; i < viewsQuantity; i++)
            {
                viewsNames.Add(StringUtility.RandomString("Test view #########"));
            }

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, (ProductSubscriptions)(int)userType));
            PortfoliosSetUp.AddInvestmentUsdPortfoliosWithOpenPosition(UserModels.First().Email);
            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);
            new MyPortfoliosMenuForm().ClickMyPortfoliosMenuItem(MyPortfoliosMenuItems.PositionsGrid);

            var positionsTab = new PositionsTabForm();
            var numberOfAvailableCheckboxes = positionsTab.GetNumberOfAvailableCheckboxes();
            for (int i = 0; i < viewsQuantity; i++)
            {
                positionsTab.AddNewViewWithRandomCheckboxesMarked(viewsNames[i], SRandom.Instance.Next(numberOfAvailableCheckboxes));
            }
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_75$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PositionsGrid"), TestCategory("View")]
        [Description("The test checks selecting View for Positions grid (also with using Dropdown); https://tr.a1qa.com/index.php?/cases/view/19235127")]
        public override void RunTest()
        {
            DoSteps1To5(viewsNames[0], 1);

            LogStep(6, "Repeat steps #5-9 for:- *second * View;*third * View");
            DoSteps1To5(viewsNames[1], 2);
            DoSteps1To5(viewsNames[2], 3);

            LogStep(7, 8, "Repeat steps #5-9");
            new PositionsTabForm().ClickEditSignForCertainView(viewsNames[3]);
            DoSteps1To5(viewsNames[3], 1);
        }

        private void DoSteps1To5(string viewName, int viewNumber)
        {
            LogStep(1, "Click edit sign for View. Detect and remember UserViewId");
            var positionsTab = new PositionsTabForm();
            positionsTab.SelectView(viewName);
            positionsTab.ClickEditSignForCertainView(viewName);
            var viewId = new ViewsQueries().SelectViewId(UserModels.First().Email, (int)ViewTypes.Position, viewName);

            LogStep(2, 3, "Remember all marked items; Click Cancel");
            var markedItems = positionsTab.GetCheckedColumnCheckboxesLabelNames();
            positionsTab.ClickCancelButton();

            LogStep(4, "Find all selected options in table header of the Alerts Grid");
            positionsTab.CheckColumnNamesInTableHeader(markedItems);

            LogStep(5, "In DB: make sure expected columns are present in view");
            var userViewColumnsNames = new ViewsQueries().SelectUserViewColumns(viewId, (int)UserViewTypes.Positions).Select(col => col.ColumnType)
                .Cast<ViewColumnTypes>().ToList().Select(t => t.ToString()).ToList();

            Checker.CheckEquals(markedItems.Count, userViewColumnsNames.Count, "Columns in DB is not matched by quantity");

            foreach (var userViewColumnsName in userViewColumnsNames)
            {
                if (Dictionaries.PositionDbColumnToGridColumnMapping.ContainsKey(userViewColumnsName))
                {
                    Checker.IsTrue(markedItems.Contains(Dictionaries.PositionDbColumnToGridColumnMapping[userViewColumnsName].GetStringMapping()),
                        $"Column '{userViewColumnsName}' from DB is not present for the current view {viewName} number {viewNumber} " +
                        $"user {UserModels.First().SubscriptionType[0]}");
                }
                else
                {
                    Checker.Fail($"Column {userViewColumnsName} can not be mapped to marked checkboxes");
                }
            }
        }
    }
}