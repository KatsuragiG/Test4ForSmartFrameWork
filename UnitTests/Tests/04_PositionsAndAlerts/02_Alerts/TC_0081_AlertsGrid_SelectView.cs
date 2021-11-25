using System.Collections.Generic;
using System.Linq;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Database.Views;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.User;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._04_PositionsAndAlerts._02_Alerts
{
    [TestClass]
    public class TC_0081_AlertsGrid_SelectView : BaseTestUnitTests
    {
        private const int TestNumber = 81;
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
                Symbol = GetTestDataAsString("SymbolStock")
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

            PositionsAlertsSetUp.AddTS12PercentAlert(stockId);
            new MainMenuNavigation().OpenAlertsGrid();

            var alertsTabForm = new AlertsTabForm();
            var numberOfAvailableCheckboxes = alertsTabForm.GetNumberOfAvailableCheckboxes();
            for (int i = 0; i < viewsQuantity; i++)
            {
                alertsTabForm.AddNewViewWithRandomCheckboxesMarked(viewsNames[i], SRandom.Instance.Next(numberOfAvailableCheckboxes));
            }
            new PositionsAlertsStatisticsPanelForm().SelectPortfolio(AllPortfoliosKinds.All.GetStringMapping());
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_81$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("AlertsGrid"), TestCategory("View")]
        [Description("The test checks selecting View for Alerts grid (also with using Dropdown); https://tr.a1qa.com/index.php?/cases/view/19232914")]
        public override void RunTest()
        {
            DoSteps1To5(viewsNames[0]);

            LogStep(6, "Repeat steps #5-9 for - *second * View; -*third * View ");
            DoSteps1To5(viewsNames[1]);
            DoSteps1To5(viewsNames[2]);

            LogStep(7, 8, "Select the *fourth* View via dropdown");
            new AlertsTabForm().ClickEditSignForCertainView(viewsNames[3]);
            DoSteps1To5(viewsNames[3]);
        }

        private void DoSteps1To5(string view)
        {
            LogStep(1, "Click edit sign for View. Detect and remember UserViewId");
            var alertsTabForm = new AlertsTabForm();
            alertsTabForm.SelectView(view);
            alertsTabForm.ClickEditSignForCertainView(view);
            var viewId = new ViewsQueries().SelectViewId(UserModels.First().Email, (int)ViewTypes.Alert, view);

            LogStep(2, 3, "Remember all marked items; Click Cancel");
            var markedItems = alertsTabForm.GetCheckedColumnCheckboxesLabelNames();
            alertsTabForm.ClickCancelButton();

            LogStep(4, "Find all selected options in table header of the Alerts Grid");
            alertsTabForm.CheckColumnNamesInTableHeader(markedItems);

            LogStep(5, "In DB: make sure expected columns are present in view");
            var userViewColumnsNames = new ViewsQueries().SelectUserViewColumns(viewId, (int)UserViewTypes.Alerts).Select(col => col.ColumnType)
                .Cast<ViewColumnTypes>().ToList().Select(t => t.ToString()).ToList();

            Checker.CheckEquals(markedItems.Count, userViewColumnsNames.Count, "Columns in DB is not matched by quantity");

            foreach (string markedItem in markedItems)
            {
                if (Dictionaries.AlertsColumnsMappingForView.ContainsKey(markedItem.Trim()))
                {
                    Checker.IsTrue(userViewColumnsNames.Contains(Dictionaries.AlertsColumnsMappingForView[markedItem.Trim()]),
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