using TradeStops.Common.Extensions;
using System.Collections.Generic;
using System.Linq;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Dashboard;
using AutomatedTests.Forms;
using AutomatedTests.Forms.Dashboard;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._02_Dashboard
{
    [TestClass]
    public class TC_1291_Dashboard_CheckThePortfolioRemembrance_GeneralDropdown_Pep : BaseTestUnitTests
    {
        private const int TestNumber = 1291;
        private const string SelectPortfolioDropDown = "Select Portfolio";
        private const string PortfolioEquityPerformanceDropDown = "Portfolio Equity Performance";

        private AddPortfolioManualModel portfolioModel;

        [TestInitialize]
        public void TestInitialize()
        {
            portfolioModel = new AddPortfolioManualModel
            {
                Name = "Dashboard_CheckThePortfolioRemembrance_GeneralDropdown_Pep"
            };

            var positionsModels = new List<PositionAtManualCreatingPortfolioModel>
            {
                new PositionAtManualCreatingPortfolioModel
                {
                    Ticker = GetTestDataAsString("ticker"),
                    EntryDate = GetTestDataAsString("entryDate"),
                    Quantity = GetTestDataAsString("shares")
                }
            };

            LogStep(0, "Precondition - Login as registered user");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsLifetime));
            PortfoliosSetUp.AddInvestmentUsdPortfoliosWithOpenPosition(UserModels.First().Email);
            LoginSetUp.LogIn(UserModels.First());
            new AddPortfoliosSteps().NavigateToAddManualPortfolioSavePortfolioAfterFillingFields(portfolioModel, positionsModels);
            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1291$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("Test checks portfolio memorization in both drop downs if leave the page: https://tr.a1qa.com/index.php?/cases/view/19234222")]
        [TestCategory("DashboardStatistics"), TestCategory("DashboardPortfolioEquityPerformance"), TestCategory("Dashboard")]
        public override void RunTest()
        {
            Step1To4(portfolioModel, SelectPortfolioDropDown);

            Step1To4(portfolioModel, PortfolioEquityPerformanceDropDown);
        }

        private void Step1To4(AddPortfolioManualModel portfolio, string dropDownName)
        {
            LogStep(1, $"Select portfolio in '{dropDownName}' drop down");
            SelectAndCheckPortfolioInDropDown(portfolio, dropDownName);

            LogStep(2, "Open Portfolios page");
            var mainMenuForm = new MainMenuForm();
            mainMenuForm.ClickMenuItem(MainMenuItems.MyPortfolios);

            LogStep(3, "Open Dashboard page");
            mainMenuForm.ClickMenuItem(MainMenuItems.Dashboard);

            LogStep(4, $"Check that selected earlier portfolio is displayed in '{dropDownName}' drop down");
            CheckSelectedPortfolioInDropDown(portfolio, dropDownName);
        }

        private void SelectAndCheckPortfolioInDropDown(AddPortfolioManualModel portfolio, string dropDownName)
        {
            if (SelectPortfolioDropDown.EqualsIgnoreCase(dropDownName))
            {
                new DashboardForm().SelectPortfolioStatisticsWidgetPortfolio(portfolio.Name);
            }
            else
            {
                new WidgetForm(WidgetTypes.PortfolioEquityPerformance)
                    .SelectValueInWidgetDropDownInTreeSelectByText(EquityPerformanceDropDownTypes.ComparePortfolios, portfolio.Name);
            }

            CheckSelectedPortfolioInDropDown(portfolio, dropDownName);
        }

        private void CheckSelectedPortfolioInDropDown(AddPortfolioManualModel portfolio, string dropDownName)
        {
            var dashboardForm = new DashboardForm();
            var dashboardWidgetForm = new WidgetForm(WidgetTypes.PortfolioEquityPerformance);
            var expectedPortfolioName = SelectPortfolioDropDown.EqualsIgnoreCase(dropDownName)
                ? dashboardForm.GetSelectedPortfolioStatisticsWidgetPortfolioName()
                : dashboardWidgetForm.GetSelectedValueFromWidgetDropDown(EquityPerformanceDropDownTypes.ComparePortfolios);

            Checker.CheckEquals(portfolio.Name, expectedPortfolioName,
                $"Created portfolio is not selected in '{dropDownName}' drop down");
        }
    }
}