using System;
using System.Collections.Generic;
using System.Linq;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Users;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Dashboard;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Forms;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms.Dashboard;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.SetUpsTearDowns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;
using TradeStops.Common.Enums;
using AutomatedTests.Navigation;
using AutomatedTests.Steps.Newsletters;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests.Utils;
using AutomatedTests.Enums.FilterEnums;

namespace UnitTests.Tests._02_Dashboard._01_Statistics
{
    [TestClass]
    public class TC_1303_Dashboard_Statistics_CheckStatisticsValuesDisplayAccordingToSelectedPortfolioWithPublisherPositions : BaseTestUnitTests
    {
        private const int TestNumber = 1303;
        private const string DbParamColumn = "dbParam";
        private const string DbValueColumn = "dbValue";

        private PortfolioDBModel portfolioModel;
        private int portfolioId;

        [TestInitialize]
        public void TestInitialize()
        {
            portfolioModel = new PortfolioDBModel
            {
                Name = "CheckThatStatisticsValuesDisplayAccordingToSelectedPortfolioWithPublisherPositionsInDropdown",
                Type = ((int)GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("portfolioType")).ToString(),
                CurrencyId = ((int)Currency.USD).ToString()
            };

            var dbParamsAndValues = InitializeDbParamsAndValues();
            var dbParamTradesmith = GetTestDataValuesAsListByColumnName("dbTradesmithParam");

            LogStep(0, "Preconditions: Login. Create the portfolio. Open Dashboard page");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPremium));
            var userModel = UserModels.First();
            var usersQueries = new UsersQueries();
            for (int i = 0; i < dbParamTradesmith.Count; i++)
            {
                usersQueries.UpdateUserProductTradesmithFeatures(userModel.TradeSmithUserId, 
                    dbParamTradesmith[i], dbParamsAndValues.ElementAt(i).Value);
            }
            portfolioId = PortfoliosSetUp.AddPortfolioViaDb(userModel, portfolioModel);

            LoginSetUp.LogIn(userModel);
            new MainMenuForm().ClickMenuItem(MainMenuItems.MyGurus);
            var mainMenuNavigation = new MainMenuNavigation();
            mainMenuNavigation.OpenCustomPublisherGrid();
            new NewslettersSteps().AddAllPositionsFromCustomPublishers(portfolioModel.Name);
            mainMenuNavigation.OpenDashboard();
        }

        private Dictionary<string, string> InitializeDbParamsAndValues()
        {
            var testData = new Dictionary<string, string>();
            var tableColumns = TestContext.DataRow.Table.Columns;

            foreach (var column in tableColumns)
            {
                if (column.ToString().Contains(DbParamColumn))
                {
                    var index = column.ToString().Split(new[] { DbParamColumn }, StringSplitOptions.None).Last();
                    var dbParam = GetTestDataAsString($"{DbParamColumn}{index}");
                    var dbValue = GetTestDataAsString($"{DbValueColumn}{index}");
                    testData[dbParam] = dbValue;
                }
            }

            return testData;
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1303$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Dashboard"), TestCategory("DashboardStatistics"), TestCategory("PortfoliosPage"), TestCategory("PortfolioStatistics")]
        [Description("Test for Portfolio Statistics on the Dashboards page the same as on the Portfolio page: https://tr.a1qa.com/index.php?/cases/view/19234296")]
        public override void RunTest()
        {
            LogStep(1, $"Select '{portfolioModel.Name}' from 'Select Portfolio' dropdown in statistics section");
            var dashboardForm = new DashboardForm();
            dashboardForm.SelectPortfolioStatisticsWidgetPortfolio(portfolioModel.Name);
            Checker.CheckEquals(AllPortfoliosKinds.All.GetStringMapping(), dashboardForm.GetSelectedPortfolioStatisticsWidgetPortfolioName(),
                "Selected option from dropdown in statistics section is not as expected");

            LogStep(2, "Remember the Statistics section");
            var diversificationQuotientOnDashboardPage = GetSectionTextByMainValueType(DashboardStatisticSections.DiversificationQuotient);
            var positionsInGreenZoneOnDashboardPage = GetSectionTextByMainValueType(DashboardStatisticSections.PositionsInGreenZone);
            var portfolioRiskPvqOnDashboardPage = GetSectionTextByMainValueType(DashboardStatisticSections.PortfolioRiskPvq);
            var totalGainOnDashboardPage = GetSectionTextByMainValueType(DashboardStatisticSections.TotalGain);
            var dailyGainOnDashboardPage = GetSectionTextByMainValueType(DashboardStatisticSections.DailyGain);
            var tradestopsValueOnDashboardPage = GetSectionTextByMainValueType(DashboardStatisticSections.TradestopsValue);

            LogStep(3, "Open My Portfolios. Check PVQ and DQ value with step 2");
            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);
            var portfoliosForm = new PortfoliosForm();
            var portfolioData = new PortfolioGridsSteps().RememberPortfolioInformationForPortfolioId(portfolioId);
            Checker.CheckEquals(diversificationQuotientOnDashboardPage, portfolioData.Dq,
                "DQ is not as expected on Portfolios page");
            Checker.CheckEquals(portfolioRiskPvqOnDashboardPage, portfolioData.Pvq,
                "PVQ is not as expected on Portfolios page");

            LogStep(4, 5, $"Go to '{portfolioModel.Name}' portfolio page");
            portfoliosForm.ClickOnPortfolioName(portfolioModel.Name);
            var positionsAlertsStatisticsPanelForm = new PositionsAlertsStatisticsPanelForm();
            positionsAlertsStatisticsPanelForm.AssertIsOpen();

            LogStep(6, "Check the Statistics");
            Checker.CheckEquals(positionsAlertsStatisticsPanelForm.GetValue(), tradestopsValueOnDashboardPage,
                $"'{DashboardStatisticSections.TradestopsValue.GetStringMapping()}' is not as expected on Positions&Alerts page" );
            Checker.CheckEquals(positionsAlertsStatisticsPanelForm.GetTotalGain(), totalGainOnDashboardPage,
                $"'{DashboardStatisticSections.TotalGain.GetStringMapping()}' is not as expected on Positions&Alerts page");
            Checker.CheckEquals(positionsAlertsStatisticsPanelForm.GetDailyGain(), dailyGainOnDashboardPage,
                $"'{DashboardStatisticSections.DailyGain.GetStringMapping()}' is not as expected on Positions&Alerts page");

            LogStep(7, "Click Portfolio Summary. Check that Positions in Green Zone in donut chart are same as in dashboard");
            positionsAlertsStatisticsPanelForm.ClickPortfolioSummary();
            positionsAlertsStatisticsPanelForm.PieChart.ClickPieChartSection(Dictionaries.HealthFillColorCodes[HealthStatusFilter.Green]);
            var actualPositionsInGreenZone = positionsAlertsStatisticsPanelForm.PieChart.GetPieChartHoverDistributionPercentText();
            Checker.CheckEquals(positionsInGreenZoneOnDashboardPage, actualPositionsInGreenZone,
                "Positions In Green Zone is not as expected on Positions grid page");
        }

        private string GetSectionTextByMainValueType(DashboardStatisticSections section)
        {
            return new DashboardForm().GetPortfolioStatisticSectionTextByItemType(section, DashboardStatisticSectionItemTypes.MainValue);
        }
    }
}