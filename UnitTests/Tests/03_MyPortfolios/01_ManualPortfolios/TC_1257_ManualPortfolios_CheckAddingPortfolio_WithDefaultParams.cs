using System;
using System.Linq;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Portfolios.CreateManual;
using AutomatedTests.Forms.Portfolios;
using AutomatedTests.Forms.PositionsAlertsForm;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.PositionsAlertsForm.Models;
using AutomatedTests.Forms.Settings;
using AutomatedTests.Forms.Settings.Position;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.Portfolios;
using AutomatedTests.Steps.Settings;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._03_MyPortfolios._01_ManualPortfolios
{
    [TestClass]
    public class TC_1257_ManualPortfolios_CheckAddingPortfolio_WithDefaultParams : BaseTestUnitTests
    {
        private const int TestNumber = 1257;
        private const string ZeroPercentValue = "(0.00%)";

        private AddPortfolioManualModel portfolioModel;
        private AddPortfolioManualModel expectedPortfolioModel;
        private PositionsAlertsBasicBlockModel expectedPositionsModel;
        private string currencySign;
        private string portfolioCash;
        private string noPositionWording;

        [TestInitialize]
        public void TestInitialize()
        {
            var portfolioCurrency = GetTestDataAsString("PortfolioCurrency");
            noPositionWording = GetTestDataAsString(nameof(noPositionWording));
            var portfolioEntryCommission = "800";
            var portfolioExitCommission = "900";
            portfolioCash = Constants.DefaultStringZeroIntValue;

            currencySign = GetTestDataAsString("CurrencySign");
            portfolioModel = new AddPortfolioManualModel
            {
                Name = "CheckAddingPortfolio_WithDefaultParams"
            };

            expectedPortfolioModel = new AddPortfolioManualModel
            {
                Name = portfolioModel.Name,
                Currency = portfolioCurrency,
                EntryCommission = $"{currencySign}{portfolioEntryCommission}",
                ExitCommission = $"{currencySign}{portfolioExitCommission}",
                Cash = string.Empty,
                Notes = string.Empty
            };

            expectedPositionsModel = new PositionsAlertsBasicBlockModel
            {
                Name = portfolioModel.Name,
                Type = PortfolioType.Investment.GetStringMapping(),
                Currency = expectedPortfolioModel.Currency,
                EntryCommission = $"{currencySign}{portfolioEntryCommission.ToFractionalString()}",
                ExitCommission = $"{currencySign}{portfolioExitCommission.ToFractionalString()}",
                Cash = $"{currencySign}{portfolioCash.ToFractionalString()}",
                Notes = expectedPortfolioModel.Notes
            };

            LogStep(0, "Preconditions: Login as Premium. User setup. Open Portfolios page -> Click 'Add Portfolio' -> Click 'Add Manually'");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscription(TestNumber, ProductSubscriptions.TradeStopsPremium));

            PortfoliosSetUp.AddInvestmentPortfoliosDefaultUSD(UserModels.First().Email);

            var settingsSetup = new SettingsSteps();
            settingsSetup.LoginNavigateToSettingsPositionGetForm(UserModels.First());
            settingsSetup.SetDefaultPositionCommissions(portfolioEntryCommission, portfolioExitCommission);
            new PositionSettingForm().SetCurrency(portfolioCurrency);
            new SettingsMainForm().SaveSettings();

            new AddPortfoliosSteps().OpenPortfolioCreationFormViaSelectionFlowPage(AddPortfolioTypes.Manual);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1257$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PortfoliosPage"), TestCategory("AddPortfolio")]
        [Description("Test for adding Portfolio with default Currency, Entry Commission and Exit Commission: https://tr.a1qa.com/index.php?/cases/view/19234166")]
        public override void RunTest()
        {
            LogStep(1, 3, "Enter Portfolio Name and Click Portfolio Settings link");
            new AddPortfoliosSteps().FillCheckPortfolioFields(portfolioModel, expectedPortfolioModel);

            LogStep(4, "Click 'Save Portfolio'");
            new ManualPortfolioCreationForm().ClickPortfolioManualFlowActionsButton(PortfolioManualFlowActionsButton.SavePortfolio);
            var createdDate = Parsing.ConvertToShortDateString(DateTime.Now.ToShortDateString());
            var positionsAlertsStatisticsPanelForm = new PositionsAlertsStatisticsPanelForm();
            var positionsTabForm = new PositionsTabForm();
            Checker.CheckEquals(noPositionWording, positionsTabForm.GetNoPositionText(),
                "Text 'There are no positions in this portfolio.' is NOT  displayed.");
            Checker.IsTrue(positionsTabForm.IsAddPositionButtonPresent(),
                "Add Position Button is NOT displayed.");
            Checker.CheckEquals(expectedPositionsModel.Name, positionsAlertsStatisticsPanelForm.GetPortfolioName(),
                "'Portfolio Selection Dropdown' is not as expected");
            Checker.CheckEquals(expectedPositionsModel.Cash, positionsAlertsStatisticsPanelForm.GetValue(),
                "'Value' is not as expected");
            Checker.CheckEquals(expectedPositionsModel.Cash, positionsAlertsStatisticsPanelForm.GetPortfolioCash(),
                "'Portfolio Cash' is not as expected");
            Checker.CheckEquals(expectedPositionsModel.Cash, positionsAlertsStatisticsPanelForm.GetDailyGain(),
                "'Daily Gain Dollar' is not as expected");
            Checker.CheckEquals(ZeroPercentValue, positionsAlertsStatisticsPanelForm.GetDailyGainPercent(),
                "'Daily Gain Percent' is not as expected");
            Checker.CheckEquals(expectedPositionsModel.Cash, positionsAlertsStatisticsPanelForm.GetTotalGain(),
                "'Daily Gain Dollar' is not as expected");
            Checker.CheckEquals(ZeroPercentValue, positionsAlertsStatisticsPanelForm.GetTotalGainPercent(),
                "'Total Gain Percent' is not as expected");

            LogStep(5, "Click 'Show Portfolio Summary'");
            positionsAlertsStatisticsPanelForm.ClickPortfolioSummary();

            Checker.CheckEquals(expectedPositionsModel.Name, 
                positionsAlertsStatisticsPanelForm.GetValueFromBasicSummaryBlock(PortfolioSummaryBasicValueTypes.Name),
                "'Name' from basic block is not as expected" );
            Checker.CheckEquals(expectedPositionsModel.Type,
                positionsAlertsStatisticsPanelForm.GetValueFromBasicSummaryBlock(PortfolioSummaryBasicValueTypes.Type),
                "'Type' from basic block is not as expected");
            Checker.CheckEquals(expectedPositionsModel.Notes.SetUnixNewLines(),
                positionsAlertsStatisticsPanelForm.GetValueFromBasicSummaryBlock(PortfolioSummaryBasicValueTypes.Notes),
                "'Notes' from basic block is not as expected");
            Checker.CheckEquals(expectedPositionsModel.Cash,
                positionsAlertsStatisticsPanelForm.GetValueFromBasicSummaryBlock(PortfolioSummaryBasicValueTypes.Cash),
                "'Cash' from basic block is not as expected");
            Checker.CheckEquals(expectedPositionsModel.EntryCommission,
                positionsAlertsStatisticsPanelForm.GetValueFromBasicSummaryBlock(PortfolioSummaryBasicValueTypes.EntryCommission),
                "'Entry Commission' from basic block is not as expected");
            Checker.CheckEquals(expectedPositionsModel.ExitCommission,
                positionsAlertsStatisticsPanelForm.GetValueFromBasicSummaryBlock(PortfolioSummaryBasicValueTypes.ExitCommission),
                "'Exit Commission' from basic block is not as expected");
            Checker.CheckEquals(expectedPositionsModel.Currency, positionsAlertsStatisticsPanelForm.GetSelectedCurrencyFromBasicBlock(),
                "'Currency' from basic block is not as expected");
            Checker.CheckEquals(expectedPositionsModel.Cash,
                positionsAlertsStatisticsPanelForm.GetValueFromBasicSummaryBlock(PortfolioSummaryBasicValueTypes.Cash),
                "'Cash' from statistic block is not as expected");

            Checker.CheckEquals(expectedPositionsModel.Cash,
                positionsAlertsStatisticsPanelForm.GetValueFromStatisticsSummaryBlock(PortfolioSummaryStatisticValueTypes.TradeSmithValue),
                "'TradeStops Value' from statistic block is not as expected");
            Checker.CheckEquals(expectedPositionsModel.Cash,
                positionsAlertsStatisticsPanelForm.GetValueFromStatisticsSummaryBlock(PortfolioSummaryStatisticValueTypes.Cash),
                "'Cash' from statistic block is not as expected");
            Checker.CheckEquals(expectedPositionsModel.Cash,
                positionsAlertsStatisticsPanelForm.GetValueFromStatisticsSummaryBlock(PortfolioSummaryStatisticValueTypes.CostBasis),
                "'CostBasis' from statistic block is not as expected");
            Checker.CheckEquals($"{expectedPositionsModel.Cash} {ZeroPercentValue}",
                positionsAlertsStatisticsPanelForm.GetValueFromStatisticsSummaryBlock(PortfolioSummaryStatisticValueTypes.DailyGain),
                "'Today' from statistic block is not as expected");
            Checker.CheckEquals(expectedPositionsModel.Cash,
                positionsAlertsStatisticsPanelForm.GetValueFromStatisticsSummaryBlock(PortfolioSummaryStatisticValueTypes.DividendTotal),
                "'Dividend Total' from statistic block is not as expected");
            Checker.CheckEquals($"{expectedPositionsModel.Cash} {ZeroPercentValue}",
                positionsAlertsStatisticsPanelForm.GetValueFromStatisticsSummaryBlock(PortfolioSummaryStatisticValueTypes.TotalGainWithDiv),
                "'Total Gain W/Div' from statistic block is not as expected");
            Checker.CheckEquals(expectedPositionsModel.Cash,
                positionsAlertsStatisticsPanelForm.GetValueFromStatisticsSummaryBlock(PortfolioSummaryStatisticValueTypes.TotalGainExcludeDiv),
                "'Total Gain E/Div' from statistic block is not as expected");
            Checker.CheckEquals(portfolioCash,
                positionsAlertsStatisticsPanelForm.GetValueFromStatisticsSummaryBlock(PortfolioSummaryStatisticValueTypes.Positions),
                "'Positions' from statistic block is not as expected");
            Checker.CheckEquals(portfolioCash,
                positionsAlertsStatisticsPanelForm.GetValueFromStatisticsSummaryBlock(PortfolioSummaryStatisticValueTypes.AverageDaysHeld),
                "'AverageDaysHeld' from statistic block is not as expected");

            LogStep(6, "Open portfolio grid and check created date");
            new MainMenuNavigation().OpenPortfolios(portfolioModel.Type ?? PortfolioType.Investment);
            var portfoliosForm = new PortfoliosForm();
            var portfolioId = portfoliosForm.GetPortfolioIdViaName(portfolioModel.Name);
            Checker.CheckEquals(createdDate,
                new PortfolioGridsSteps().RememberPortfolioInformationForPortfolioId(portfolioId).CreatedDate,
                "Created Date for created portfolio is not as expected");
        }
    }
}