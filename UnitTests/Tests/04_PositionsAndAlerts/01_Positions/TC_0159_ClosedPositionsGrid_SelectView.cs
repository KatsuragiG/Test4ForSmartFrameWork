using System;
using System.Collections.Generic;
using System.Linq;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Database.Views;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.User;
using AutomatedTests.Forms;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
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
    public class TC_0159_ClosedPositionsGrid_SelectView : BaseTestUnitTests
    {
        private const int TestNumber = 159;
        private readonly List<string> viewsNames = new List<string>();

        [TestInitialize]
        public void TestInitialize()
        {
            var userType = GetTestDataParsedAsEnumFromStringMapping<ProductSubscriptionTypes>("User");
            var portfolioModel = new PortfolioModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency")
            };
            var positionModelStock = new PositionsDBModel
            {
                Symbol = GetTestDataAsString("SymbolStock"),
            };
            var viewsQuantity = GetTestDataAsInt("viewsQuantity");
            for (int i = 0; i < viewsQuantity; i++)
            {
                viewsNames.Add(StringUtility.RandomString("Test view #########"));
            }

            LogStep(0, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, (ProductSubscriptions)(int)userType));
            var portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            var stockId = PositionsAlertsSetUp.AddPositionViaDB(portfolioId, positionModelStock);

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
            PositionsAlertsSetUp.ClosePosition(stockId);

            new MainMenuNavigation().OpenPositionsGrid(PositionsTabs.ClosedPositions);
            var closedPositionsTab = new ClosedPositionsTabForm();
            closedPositionsTab.SelectCustomPeriodRangeWithStartEndDates(DateTime.Now.AddYears(-Constants.DefaultSizeOfDateStringToClearField).ToShortDateString(), 
                DateTime.Now.ToShortDateString());
            var numberOfAvailableCheckboxes = closedPositionsTab.GetNumberOfAvailableCheckboxes();
            for (int i = 0; i < viewsQuantity; i++)
            {
                closedPositionsTab.AddNewViewWithRandomCheckboxesMarked(viewsNames[i], SRandom.Instance.Next(numberOfAvailableCheckboxes));
            }
            new PositionsAlertsStatisticsPanelForm().SelectPortfolio(AllPortfoliosKinds.All.GetStringMapping());
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_159$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("ClosedPositionsGrid"), TestCategory("View")]
        [Description("The test checks selecting View for Closed Position grid (also with using Dropdown) https://tr.a1qa.com/index.php?/cases/view/19232863")]
        public override void RunTest()
        {
            LogStep(1, "Select All portfolios in dropdowns");
            DoSteps1To5(viewsNames[0]);

            LogStep(6, "Repeat steps #1-5 for - *second * View; -*third * View ");
            DoSteps1To5(viewsNames[1]);
            DoSteps1To5(viewsNames[2]);

            LogStep(7, 8, "Select the *fourth* View via dropdown");
            new ClosedPositionsTabForm().ClickEditSignForCertainView(viewsNames[3]);

            LogStep(9, "Repeat steps #2-6");
            DoSteps1To5(viewsNames[3]);
        }

        private void DoSteps1To5(string view)
        {
            LogStep(1, "Click edit sign for View. Detect and remember UserViewId");
            var closedPositionsTab = new ClosedPositionsTabForm();
            closedPositionsTab.SelectView(view);
            closedPositionsTab.ClickEditSignForCertainView(view);
            var viewsQueries = new ViewsQueries();
            var viewId = viewsQueries.SelectViewId(UserModels.First().Email, (int)ViewTypes.ClosedPosition, view);

            LogStep(2, 3, "Remember all marked items; Click Cancel");
            var markedItems = closedPositionsTab.GetCheckedColumnCheckboxesLabelNames();
            closedPositionsTab.ClickCancelButton();

            LogStep(4, "Find all selected options in table header of the Alerts Grid");
            closedPositionsTab.CheckColumnNamesInTableHeader(markedItems);

            LogStep(5, "In DB: make sure expected columns are present in view");
            var userViewColumnTypes = viewsQueries.SelectUserViewColumns(viewId, (int)UserViewTypes.ClosedPositions).Select(col => col.ColumnType);

            var userViewColumnsNames = userViewColumnTypes.Cast<ViewColumnTypes>().ToList().Select(t => t.ToString()).ToList();

            foreach (string markedItem in markedItems)
            {
                if (Dictionaries.ClosedPositionsColumnsAndObjProperties.ContainsKey(markedItem.Trim()))
                {
                    Checker.IsTrue(userViewColumnsNames.Contains(Dictionaries.ClosedPositionsColumnsAndObjProperties[markedItem.Trim()]),
                    $"Column '{markedItem}' is not present in DB for the view");
                }
                else
                {
                    Checker.Fail($"Column {markedItem} can not be mapped to DB");
                }
            }
        }
    }
}