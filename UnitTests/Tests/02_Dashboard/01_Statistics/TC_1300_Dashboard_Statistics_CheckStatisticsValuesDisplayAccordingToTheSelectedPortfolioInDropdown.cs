using System;
using System.Collections.Generic;
using System.Linq;
using AutomatedTests.Database.Users;
using AutomatedTests.Enums;
using AutomatedTests.Enums.Dashboard;
using AutomatedTests.Forms;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms.Dashboard;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;
using AutomatedTests.Enums.Portfolios.CreateManual;
using AutomatedTests.Navigation;
using TradeStops.Common.Enums;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.Portfolios;

namespace UnitTests.Tests._02_Dashboard._01_Statistics
{
    [TestClass]
    public class TC_1300_Dashboard_Statistics_CheckStatisticsValuesDisplayAccordingToTheSelectedPortfolioInDropdown : BaseTestUnitTests
    {
        private const int TestNumber = 1300;
        private const string DbParamColumn = "dbParam";
        private const string DbValueColumn = "dbValue";
        private const string TickerColumn = "ticker";

        private AddPortfolioManualModel portfolioModel;
        private string expPositionsInGreenZoneOnDashboardPage;
        private string expPortfolioRiskPvqOnDashboardPage;
        private string expDiversificationQuotientOnDashboardPage;
        private string expValueOnDashboardPage;
        private string expTotalGainOnDashboardPage;
        private string expTotalGainPercentOnDashboardPage;
        private string expDailyGainOnDashboardPage;
        private string expDailyGainPercentOnDashboardPage;

        [TestInitialize]
        public void TestInitialize()
        {
            expPositionsInGreenZoneOnDashboardPage = GetTestDataAsString(nameof(expPositionsInGreenZoneOnDashboardPage));
            expPortfolioRiskPvqOnDashboardPage = GetTestDataAsString(nameof(expPortfolioRiskPvqOnDashboardPage));
            expDiversificationQuotientOnDashboardPage = GetTestDataAsString(nameof(expDiversificationQuotientOnDashboardPage));

            expValueOnDashboardPage = GetTestDataAsString(nameof(expValueOnDashboardPage));
            expTotalGainOnDashboardPage = GetTestDataAsString(nameof(expTotalGainOnDashboardPage));
            expTotalGainPercentOnDashboardPage = GetTestDataAsString(nameof(expTotalGainPercentOnDashboardPage));
            expDailyGainOnDashboardPage = GetTestDataAsString(nameof(expDailyGainOnDashboardPage));
            expDailyGainPercentOnDashboardPage = GetTestDataAsString(nameof(expDailyGainPercentOnDashboardPage));

            portfolioModel = new AddPortfolioManualModel
            {
                Name = "CheckThatStatisticsValuesDisplayAccordingToTheSelectedPortfolioInDropdown",
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("portfolioType")
            };

            var positionsModels = InitializePositionModels();
            var dbParamsAndValues = InitializeDbParamsAndValues();
            var dbParamTradesmith = GetTestDataValuesAsListByColumnName("dbTradesmithParam");

            LogStep(0, "Preconditions: Login. Create the portfolio. Open Dashboard page");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, new List<ProductSubscriptions>
                {
                    ProductSubscriptions.TradeStopsPremium, ProductSubscriptions.CryptoStopsPremium
                }
            ));

            var usersQueries = new UsersQueries();
            for (int i = 0; i < dbParamTradesmith.Count; i++)
            {
                usersQueries.UpdateUserProductTradesmithFeatures(UserModels.First().TradeSmithUserId,
                    dbParamTradesmith[i], dbParamsAndValues.ElementAt(i).Value);
            }
            PortfoliosSetUp.AddInvestmentPortfoliosDefaultUSD(UserModels.First().Email);
            LoginSetUp.LogIn(UserModels.First());

            new AddPortfoliosSteps().NavigateToAddManualPortfolioSavePortfolioAfterFillingFields(portfolioModel, positionsModels);
            new AddAlertsAtCreatingPortfolioForm().ClickActionButton(AddAlertsAtCreatingPortfolioButtons.AddAlertsLater);

            new MainMenuForm().ClickMenuItem(MainMenuItems.Dashboard);
        }

        private List<PositionAtManualCreatingPortfolioModel> InitializePositionModels()
        {
            var positions = new List<PositionAtManualCreatingPortfolioModel>();
            var tableColumns = TestContext.DataRow.Table.Columns;
            var entryDate = GetTestDataAsString("entryDate");
            var shares = GetTestDataAsString("shares");
            foreach (var column in tableColumns)
            {
                if (column.ToString().Contains(TickerColumn))
                {
                    var ticker = GetTestDataAsString(column.ToString());
                    if (!string.IsNullOrEmpty(ticker))
                    {
                        positions.Add(new PositionAtManualCreatingPortfolioModel
                        {
                            Ticker = ticker,
                            EntryDate = entryDate,
                            Quantity = shares,
                            PositionAssetType = ticker.Contains("/")
                            ? PositionAssetTypes.Crypto
                            : PositionAssetTypes.Stock
                        });
                    }
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

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1300$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Dashboard"), TestCategory("DashboardStatistics"), TestCategory("AddPortfolio"), TestCategory("PortfoliosPage"), TestCategory("PortfolioStatistics")]
        [Description("Test for Portfolio Statistics on the Dashboards page the same as on the Portfolio page: https://tr.a1qa.com/index.php?/cases/view/19234297")]
        public override void RunTest()
        {
            LogStep(1, $"Select '{portfolioModel.Name}' from 'Select Portfolio' dropdown in statistics section");
            var dashboardForm = new DashboardForm();
            dashboardForm.SelectPortfolioStatisticsWidgetPortfolio(portfolioModel.Name);
            Checker.CheckEquals(portfolioModel.Name, dashboardForm.GetSelectedPortfolioStatisticsWidgetPortfolioName(), 
                "Selected option from dropdown in statistics section is not as expected");

            LogStep(2, "Check Statistics section");
            CheckIfNotEmptyAndGetSectionTextByValueType(DashboardStatisticSections.PositionsInGreenZone, 
                expPositionsInGreenZoneOnDashboardPage, DashboardStatisticSectionItemTypes.MainValue);
            CheckIfNotEmptyAndGetSectionTextByValueType(DashboardStatisticSections.PortfolioRiskPvq, 
                expPortfolioRiskPvqOnDashboardPage, DashboardStatisticSectionItemTypes.MainValue);
            CheckIfNotEmptyAndGetSectionTextByValueType(DashboardStatisticSections.DiversificationQuotient,
                expDiversificationQuotientOnDashboardPage, DashboardStatisticSectionItemTypes.MainValue);

            var tradestopsValueOnDashboardPage = CheckIfNotEmptyAndGetSectionTextByValueType(DashboardStatisticSections.TradestopsValue, 
                expValueOnDashboardPage, DashboardStatisticSectionItemTypes.MainValue);
            var totalGainOnDashboardPage = CheckIfNotEmptyAndGetSectionTextByValueType(DashboardStatisticSections.TotalGain, 
                expTotalGainOnDashboardPage, DashboardStatisticSectionItemTypes.MainValue);
            var dailyGainOnDashboardPage = CheckIfNotEmptyAndGetSectionTextByValueType(DashboardStatisticSections.DailyGain, 
                expDailyGainOnDashboardPage, DashboardStatisticSectionItemTypes.MainValue);

            var totalGainPercentOnDashboardPage = CheckIfNotEmptyAndGetSectionTextByValueType(DashboardStatisticSections.TotalGain, 
                expTotalGainPercentOnDashboardPage, DashboardStatisticSectionItemTypes.AdditionalValue);
            var dailyGainPercentOnDashboardPage = CheckIfNotEmptyAndGetSectionTextByValueType(DashboardStatisticSections.DailyGain, 
                expDailyGainPercentOnDashboardPage, DashboardStatisticSectionItemTypes.AdditionalValue);

            LogStep(3, "Go to Portfolios page");
            new MainMenuNavigation().OpenPortfolios();
            var portfoliosForm = new PortfoliosForm();
            portfoliosForm.AssertIsOpen();

            LogStep(4, $"Go to '{portfolioModel.Name}' portfolio page");
            portfoliosForm.ClickOnPortfolioName(portfolioModel.Name);
            var positionsAndAlertsForm = new PositionsAlertsStatisticsPanelForm();
            positionsAndAlertsForm.AssertIsOpen();

            LogStep(5, "Check the Statistics");
            Checker.CheckEquals(tradestopsValueOnDashboardPage, positionsAndAlertsForm.GetValue(),
                $"'{DashboardStatisticSections.TradestopsValue.GetStringMapping()}' is not as expected on Positions&Alerts page");
            Checker.CheckEquals(totalGainOnDashboardPage, positionsAndAlertsForm.GetTotalGain(), 
                $"'{DashboardStatisticSections.TotalGain.GetStringMapping()}' is not as expected on Positions&Alerts page");
            Checker.CheckEquals(dailyGainOnDashboardPage, positionsAndAlertsForm.GetDailyGain(), 
                $"'{DashboardStatisticSections.DailyGain.GetStringMapping()}' is not as expected on Positions&Alerts page");

            if (!string.IsNullOrEmpty(expDailyGainPercentOnDashboardPage))
            {
                Checker.CheckEquals(dailyGainPercentOnDashboardPage, positionsAndAlertsForm.GetDailyGainPercent(),
                    $"'{DashboardStatisticSections.DailyGain.GetStringMapping()}' Percent is not as expected on Positions&Alerts page");
                Checker.CheckEquals(totalGainPercentOnDashboardPage, positionsAndAlertsForm.GetTotalGainPercent(),
                    $"'{DashboardStatisticSections.TotalGain.GetStringMapping()}' Percent is not as expected on Positions&Alerts page");
            }
        }

        private string GetSectionTextByValueType(DashboardStatisticSections section, DashboardStatisticSectionItemTypes itemType)
        {
            return new DashboardForm().GetPortfolioStatisticSectionTextByItemType(section, itemType);
        }

        private string CheckIfNotEmptyAndGetSectionTextByValueType(DashboardStatisticSections section, string expectedValue, 
            DashboardStatisticSectionItemTypes itemType)
        {
            var actualValue = GetSectionTextByValueType(section, itemType);
            if (!string.IsNullOrEmpty(expectedValue))
            {
                Checker.CheckEquals(expectedValue, actualValue, 
                    $"'{section.GetStringMapping()}' in {itemType} is not as expected on Dashboard page");
            }
            return actualValue;
        }
    }
}