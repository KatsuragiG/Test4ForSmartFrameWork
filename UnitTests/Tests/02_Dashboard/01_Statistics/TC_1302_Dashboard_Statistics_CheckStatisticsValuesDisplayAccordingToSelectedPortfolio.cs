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
using AutomatedTests.Steps;
using AutomatedTests.Steps.AddPositionAdvanced;
using AutomatedTests.Steps.AddPositionInlineForm;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;
using TradeStops.Common.Enums;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Models.PositionsModels;

namespace UnitTests.Tests._02_Dashboard._01_Statistics
{
    [TestClass]
    public class TC_1302_Dashboard_Statistics_CheckStatisticsValuesDisplayAccordingToSelectedPortfolio : BaseTestUnitTests
    {
        private const int TestNumber = 1302;
        private const string DbParamColumn = "dbParam";
        private const string DbValueColumn = "dbValue";
        private const string TickerColumn = "ticker";

        private PortfolioModel portfolioModel;
        private string expPositionsInGreenZoneOnDashboardPage;
        private string expPortfolioRiskPvqOnDashboardPage;
        private string expDiversificationQuotientOnDashboardPage;

        [TestInitialize]
        public void TestInitialize()
        {
            expPositionsInGreenZoneOnDashboardPage = GetTestDataAsString("expPositionsInGreenZoneOnDashboardPage");
            expPortfolioRiskPvqOnDashboardPage = GetTestDataAsString("expPortfolioRiskPvqOnDashboardPage");
            expDiversificationQuotientOnDashboardPage = GetTestDataAsString("expDiversificationQuotientOnDashboardPage");

            portfolioModel = new PortfolioModel
            {
                Name = "CheckThatStatisticsValuesDisplayAccordingToSelectedPortfolioWithOptionsInDropdown",
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("portfolioType"),
                Currency = Currency.USD.GetStringMapping()
            };

            var positionsModels = InitializePositionModels(portfolioModel.Name);
            var dbParamsAndValues = InitializeDbParamsAndValues();
            var dbParamTradesmith = GetTestDataValuesAsListByColumnName("dbTradesmithParam");

            LogStep(0, "Precondition - Login as registered user");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPremium));

            var usersQueries = new UsersQueries();
            for (int i = 0; i < dbParamTradesmith.Count; i++)
            {
                usersQueries.UpdateUserProductTradesmithFeatures(UserModels.First().TradeSmithUserId, dbParamTradesmith[i], dbParamsAndValues.ElementAt(i).Value);
            }
            PortfoliosSetUp.AddInvestmentPortfoliosDefaultUSD(UserModels.First().Email);

            LoginSetUp.LogIn(UserModels.First());

            PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            AddPositions(positionsModels);

            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
        }

        private List<AddPositionAdvancedModel> InitializePositionModels(string portfolioName)
        {
            var positions = new List<AddPositionAdvancedModel>();
            var tableColumns = TestContext.DataRow.Table.Columns;
            var entryDate = GetTestDataAsString("entryDate");
            var contracts = GetTestDataAsString("contracts");
            foreach (var column in tableColumns)
            {
                if (column.ToString().Contains(TickerColumn))
                {
                    positions.Add(new AddPositionAdvancedModel
                    {
                        Ticker = GetTestDataAsString(column.ToString()),
                        EntryDate = entryDate,
                        Contracts = contracts,
                        AssetType = PositionAssetTypes.Option,
                        Portfolio = portfolioName
                    });
                }
            }

            return positions;
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

        private void AddPositions(IEnumerable<AddPositionAdvancedModel> positions)
        {
            var mainMenuSteps = new MainMenuSteps();
            var addPositionPopupSteps = new AddPositionInlineFormSteps();
            var addPositionAdvancedSteps = new AddPositionAdvancedSteps();

            foreach (var positionModel in positions)
            {
                mainMenuSteps.OpenPositionGridViaPortfolioGridLink(new PortfolioModel { Name = positionModel.Portfolio });
                addPositionPopupSteps.OpenAddPositionAdvancedViaAddPositionInlineFormGetAddPositionAdvancedForm();
                addPositionAdvancedSteps.FillFieldsClickSave(positionModel);
            }
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1302$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Dashboard"), TestCategory("DashboardStatistics"), TestCategory("AddPortfolio"), TestCategory("PortfoliosPage"), TestCategory("PortfolioStatistics")]
        [Description("Test for Portfolio Statistics on the Dashboards page the same as on the Portfolio page: https://tr.a1qa.com/index.php?/cases/view/19234295")]
        public override void RunTest()
        {
            LogStep(1, $"Select '{portfolioModel.Name}' from 'Select Portfolio' dropdown in statistics section");
            var dashboardForm = new DashboardForm();
            dashboardForm.SelectPortfolioStatisticsWidgetPortfolio(portfolioModel.Name);
            Checker.IsTrue(dashboardForm.IsSelectedPortfolioPresent(portfolioModel.Name),
                "Selected option from dropdown in statistics section is not as expected");

            LogStep(2, "Check Statistics section");
            var tradestopsValueOnDashboardPage = GetSectionTextByMainValueType(DashboardStatisticSections.TradestopsValue);
            var totalGainOnDashboardPage = GetSectionTextByMainValueType(DashboardStatisticSections.TotalGain);
            var dailyGainOnDashboardPage = GetSectionTextByMainValueType(DashboardStatisticSections.DailyGain);
            CheckSectionTextByMainValueType(DashboardStatisticSections.PositionsInGreenZone, expPositionsInGreenZoneOnDashboardPage);
            CheckSectionTextByMainValueType(DashboardStatisticSections.PortfolioRiskPvq, expPortfolioRiskPvqOnDashboardPage);
            CheckSectionTextByMainValueType(DashboardStatisticSections.DiversificationQuotient, expDiversificationQuotientOnDashboardPage);

            LogStep(3, "Go to Portfolios page");
            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);
            var portfoliosForm = new PortfoliosForm();
            portfoliosForm.AssertIsOpen();

            LogStep(4, $"Go to '{portfolioModel.Name}' portfolio page");
            portfoliosForm.ClickOnPortfolioName(portfolioModel.Name);
            var positionsAlertsStatisticsPanelForm = new PositionsAlertsStatisticsPanelForm();
            positionsAlertsStatisticsPanelForm.AssertIsOpen();

            LogStep(5, "Check the Statistics on portfolio page");
            Checker.CheckEquals(tradestopsValueOnDashboardPage, positionsAlertsStatisticsPanelForm.GetValue(), 
                $"'{DashboardStatisticSections.TradestopsValue.GetStringMapping()}' is not as expected on Positions&Alerts page");
            Checker.CheckEquals(dailyGainOnDashboardPage, positionsAlertsStatisticsPanelForm.GetDailyGain(),
                $"'{DashboardStatisticSections.DailyGain.GetStringMapping()}' is not as expected on Positions&Alerts page");
            Checker.CheckEquals(totalGainOnDashboardPage, positionsAlertsStatisticsPanelForm.GetTotalGain(),
                $"'{DashboardStatisticSections.TotalGain.GetStringMapping()}' is not as expected on Positions&Alerts page");
        }

        private string GetSectionTextByMainValueType(DashboardStatisticSections section)
        {
            return new DashboardForm().GetPortfolioStatisticSectionTextByItemType(section, DashboardStatisticSectionItemTypes.MainValue);
        }

        private void CheckSectionTextByMainValueType(DashboardStatisticSections section, string expectedValue)
        {
            Checker.CheckEquals(expectedValue, GetSectionTextByMainValueType(section),
                $"'{section.GetStringMapping()}' main value is not as expected on Dashboard page");
        }
    }
}