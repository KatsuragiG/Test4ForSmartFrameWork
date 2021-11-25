using System.Collections.Generic;
using System.Linq;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Dashboard;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.Sorting;
using AutomatedTests.Enums;
using AutomatedTests.Forms.Dashboard;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests.Steps.TableElementSteps;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Models.PositionsModels;
using AutomatedTests.Navigation;

namespace UnitTests.Tests._02_Dashboard._07_WinnerAndLosers
{
    [TestClass]
    public class TC_1345_Dashboard_WinnerAndLosers_CheckExpectedBehaviorForWinnersAndLosersTabsTabsWithData : BaseTestUnitTests
    {
        private const int TestNumber = 1345;
        private const WidgetTypes Widget = WidgetTypes.WinnersAndLosers;
        private const string TickerColumn = "Ticker";
        private const string CompanyColumn = "Company";
        private const string EntryDateColumn = "EntryDate";
        private const string TotalGainColumn = "TotalGain";
        private const string TotalGainPercentColumn = "TotalPercent";
        private const string Winner = "winner";
        private const string Losers = "loser";

        private AddPortfolioManualModel portfolioModel;
        private List<PositionAtManualCreatingPortfolioModel> positionsModels;
        private List<AddPositionAdvancedModel> optionModels;
        private int step = 1;
        private Dictionary<string, List<string>> winnerTableValuesDictionary;
        private Dictionary<string, List<string>> loserTableValuesDictionary;
        private Dictionary<DashboardTopStatisticsTab, string> colors;

        [TestInitialize]
        public void TestInitialize()
        {
            var entryDate = GetTestDataAsString("entryDate");
            var shares = GetTestDataAsString("shares");
            var contracts = GetTestDataAsString("contracts");

            portfolioModel = new AddPortfolioManualModel
            {
                Name = "CheckSortingInGrids"
            };

            positionsModels = new List<PositionAtManualCreatingPortfolioModel>
            {
                new PositionAtManualCreatingPortfolioModel
                {
                    Ticker = GetTestDataAsString("ticker1"),
                    PositionAssetType = PositionAssetTypes.Stock,
                    EntryDate = entryDate,
                    Quantity = shares
                },
                new PositionAtManualCreatingPortfolioModel
                {
                    Ticker = GetTestDataAsString("ticker2"),
                    PositionAssetType = PositionAssetTypes.Stock,
                    EntryDate = entryDate,
                    Quantity = shares
                },
                new PositionAtManualCreatingPortfolioModel
                {
                    Ticker = GetTestDataAsString("crypto1"),
                    PositionAssetType = PositionAssetTypes.Crypto,
                    EntryDate = entryDate,
                    Quantity = shares
                },
                new PositionAtManualCreatingPortfolioModel
                {
                    Ticker = GetTestDataAsString("crypto2"),
                    PositionAssetType = PositionAssetTypes.Crypto,
                    EntryDate = entryDate,
                    Quantity = shares
                }
            };
            var expirationDate = GetTestDataAsString("ExpirationDate");
            var strikePrice = GetTestDataAsString("StrikePrice");
            optionModels = new List<AddPositionAdvancedModel>
            {
                new AddPositionAdvancedModel
                {
                    Portfolio = portfolioModel.Name,
                    Ticker = GetTestDataAsString("option1"),
                    AssetType = PositionAssetTypes.Option,
                    EntryDate = entryDate,
                    EntryPrice = GetTestDataAsString("entryPrice1"),
                    Contracts = contracts,
                    EntryCommission = GetTestDataAsDecimal("commission1"),
                    ExpirationDate = expirationDate,
                    StrikePrice = strikePrice,
                    OptionType = GetTestDataAsString("StrikeType1")
                },
                new AddPositionAdvancedModel
                {
                    Portfolio = portfolioModel.Name,
                    Ticker = GetTestDataAsString("option2"),
                    AssetType = PositionAssetTypes.Option,
                    EntryDate = entryDate,
                    EntryPrice = GetTestDataAsString("entryPrice2"),
                    Contracts = contracts,
                    ExpirationDate = expirationDate,
                    StrikePrice = strikePrice,
                    OptionType = GetTestDataAsString("StrikeType2")
                }
            };

            winnerTableValuesDictionary = InitializeDictionaryData(Winner);
            loserTableValuesDictionary = InitializeDictionaryData(Losers);

            colors = new Dictionary<DashboardTopStatisticsTab, string>
            {
                {DashboardTopStatisticsTab.TopWinners, GetTestDataAsString("winnerTextColor") },
                {DashboardTopStatisticsTab.TopLosers, GetTestDataAsString("loserTextColor") }
            };

            LogStep(0, "Preconditions: Login. Create the portfolios. Open Dashboard page");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPlatinum));

            LoginSetUp.LogIn(UserModels.First());
            new AddPortfoliosSteps().NavigateToAddManualPortfolioSavePortfolioAfterFillingFields(portfolioModel, positionsModels);
            var portfolioId = new PortfoliosQueries().SelectPortfolioDataByPortfolioNameUserModel(portfolioModel.Name, UserModels.First()).PortfolioId;

            CorrectOptionsDataForActual(PositionsAlertsSetUp.AddPositionsFromAdvancedForm(Parsing.ConvertToInt(portfolioId), optionModels));
            new MainMenuForm().ClickMenuItem(MainMenuItems.MyPortfolios);
            new MainMenuNavigation().OpenDashboard();
        }

        private Dictionary<string, List<string>> InitializeDictionaryData(string type)
        {
            return new Dictionary<string, List<string>>
            {
                {TickerColumn, GetTestDataValuesAsListByColumnName($"{type}{TickerColumn}")},
                {TotalGainColumn, GetTestDataValuesAsListByColumnName($"{type}{TotalGainColumn}")},
                {TotalGainPercentColumn, GetTestDataValuesAsListByColumnName($"{type}{TotalGainPercentColumn}")},
                {CompanyColumn, GetTestDataValuesAsListByColumnName($"{type}{CompanyColumn}")},
                {EntryDateColumn, GetTestDataValuesAsListByColumnName($"{type}{EntryDateColumn}")}
            };
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1345$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Dashboard"), TestCategory("DashboardTopWinnersLosers"), TestCategory("PositionCard"), TestCategory("Sorting")]
        [Description("Test checks the expected behavior for winners and losers tabs: https://tr.a1qa.com/index.php?/cases/view/19234313")]
        public override void RunTest()
        {
            LogStep(step++, "Select Portfolio - All Investment");
            var dashboardForm = new DashboardForm();
            dashboardForm.SelectPortfolioStatisticsWidgetPortfolio(portfolioModel.Name);

            LogStep(step++, "Check that name of the widget is displayed");
            var dashboardWidget = new WidgetForm(Widget);
            Checker.CheckEquals(Widget.GetStringMapping(), dashboardWidget.GetWidgetHeaderText(),
                "Dashboard widget name is not as expected");

            SelectContentTabAndCheckSorting(DashboardTopStatisticsTab.TopWinners, winnerTableValuesDictionary);

            SelectContentTabAndCheckSorting(DashboardTopStatisticsTab.TopLosers, loserTableValuesDictionary);
        }

        private void CorrectOptionsDataForActual(List<int> optionsIds)
        {
            var positionsQueries = new PositionsQueries();
            winnerTableValuesDictionary[TickerColumn][1] = positionsQueries.SelectSymbolByPositionId(optionsIds[0]);
            winnerTableValuesDictionary[CompanyColumn][1] = positionsQueries.SelectPositionName(optionsIds[0]);
            loserTableValuesDictionary[TickerColumn][1] = positionsQueries.SelectSymbolByPositionId(optionsIds[1]);
            loserTableValuesDictionary[CompanyColumn][1] = positionsQueries.SelectPositionName(optionsIds[1]);
        }

        private void SelectContentTabAndCheckSorting(DashboardTopStatisticsTab contentTab, IReadOnlyDictionary<string, List<string>> data)
        {
            LogStep(step++, $"Open '{contentTab.GetStringMapping()}' tab");
            var dashboardWidget = new WidgetForm(Widget);
            dashboardWidget.ClickWidgetContentTab(contentTab);

            LogStep(step++, $"'{contentTab.GetStringMapping()}': Sort data in the table by {GeneralTablesHeaders.Ticker.GetStringMapping()} ASC");
            var embeddedTableElementSteps = new EmbeddedTableElementSteps(dashboardWidget.WidgetTable);
            embeddedTableElementSteps.ClickColumnHeaderCheckSortInColumnBySortingStatus(GeneralTablesHeaders.Ticker, SortingStatus.Asc);

            Checker.CheckListsEquals(data[TickerColumn],
                dashboardWidget.WidgetTable.GetTableColumnSymbolTextValuesByColumnName(GeneralTablesHeaders.Ticker),
                "Ticker values are not as expected");
            Checker.CheckListsEquals(data[CompanyColumn],
                dashboardWidget.WidgetTable.GetTableColumnSymbolTitleValuesByColumnName(GeneralTablesHeaders.Ticker),
                "Company values are not as expected");
            CheckTableValuesByHeader(GeneralTablesHeaders.TotalGain, data[TotalGainColumn], colors[contentTab]);
            CheckTableValuesByHeader(GeneralTablesHeaders.TotalGainPercent, data[TotalGainPercentColumn], colors[contentTab]);
            CheckTableValuesByHeader(GeneralTablesHeaders.EntryDate, data[EntryDateColumn], colors[contentTab]);

            ClickTickerAndCheckNavigation(contentTab, data[TickerColumn]);
        }

        private void ClickTickerAndCheckNavigation(DashboardTopStatisticsTab contentTab, IEnumerable<string> tickerNames)
        {
            foreach (var tickerName in tickerNames)
            {
                LogStep(step++, $"'{contentTab.GetStringMapping()}': Click {tickerName}");
                var dashboardWidget = new WidgetForm(Widget);
                dashboardWidget.ClickWidgetContentTab(contentTab);
                dashboardWidget.WidgetTable.ClickTableValueByLinkText(tickerName);

                var positionCardForm = new PositionCardForm();
                Checker.CheckEquals(tickerName, positionCardForm.GetSelectedPosition(), "Expected ticker is selected on PositionCard page");

                LogStep(step++, "Go back to the previous page");
                Browser.GetDriver().Navigate().Back();
                new DashboardForm().AssertIsOpen();
            }
        }

        private void CheckTableValuesByHeader(GeneralTablesHeaders header, List<string> expectedValues, string expectedColor)
        {
            var widgetTable = new WidgetForm(Widget).WidgetTable;

            var actualValues = widgetTable.GetTableColumnTextValuesWithoutLinksByColumnName(header);
            Checker.CheckListsEquals(expectedValues, actualValues, $"{header.GetStringMapping()} values are not as expected");

            if (header == GeneralTablesHeaders.TotalGain || header == GeneralTablesHeaders.TotalGainPercent)
            {
                var actualColors = widgetTable.GetTableColumnTextColorsByColumnName(header);
                for (var i = 0; i < actualValues.Count; i++)
                {
                    Checker.CheckEquals(expectedColor.GetColorFromHexString(),
                        actualColors[i].GetColorFromRgbString(), 
                        $"{header.GetStringMapping()} header: {actualValues[i]} value: color is not as expected");
                }
            }
        }
    }
}