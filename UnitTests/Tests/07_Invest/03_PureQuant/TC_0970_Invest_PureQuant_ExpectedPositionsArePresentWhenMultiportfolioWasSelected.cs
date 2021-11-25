using System.Collections.Generic;
using System.Linq;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.FilterEnums;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Enums.Tools;
using AutomatedTests.Forms.ResearchPages.PureQuant;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._07_Invest._03_PureQuant
{
    [TestClass]
    public class TC_0970_Invest_PureQuant_ExpectedPositionsArePresentWhenMultiportfolioWasSelected : BaseTestUnitTests
    {
        private const int TestNumber = 970;
        private const int SeparateTickerListingTimes = 1;
        private const string SharesValueForExcludedTickers = "0.00";

        private int stepNumber;
        private readonly List<PositionsDBModel> listOfPositionsModels = new List<PositionsDBModel>();
        private readonly List<string> listOfSymbolsToAddIndividual = new List<string>();

        [TestInitialize]
        public void TestInitialize()
        {
            var positionsQuantity = GetTestDataAsInt("positionsQuantity");
            for (int i = 1; i <= positionsQuantity; i++)
            {
                listOfPositionsModels.Add(new PositionsDBModel
                {
                    Symbol = GetTestDataAsString($"Symbol{i}"),
                    TradeType = $"{(int)PositionTradeTypes.Long}"
                });
            }

            listOfSymbolsToAddIndividual.AddRange(GetTestDataValuesAsListByColumnName("SymbolsIndividual"));

            LogStep(stepNumber++, "Precondition");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, new List<ProductSubscriptions>
                {
                    ProductSubscriptions.TradeStopsLifetime, ProductSubscriptions.CryptoStopsLifetime, ProductSubscriptions.TradeIdeasLifetime
                }
            ));
            var positionsIds = new List<int>
            {
                PortfoliosSetUp.AddInvestmentPortfoliosDefaultUSD(UserModels.First().Email),
                PortfoliosSetUp.AddInvestmentPortfoliosDefaultUSD(UserModels.First().Email),
                PortfoliosSetUp.AddInvestmentPortfoliosDefaultUSD(UserModels.First().Email)
            };
            PositionsAlertsSetUp.AddPositionViaDB(positionsIds[0], listOfPositionsModels[0]);
            PositionsAlertsSetUp.AddPositionViaDB(positionsIds[0], listOfPositionsModels[1]);
            PositionsAlertsSetUp.AddPositionViaDB(positionsIds[1], listOfPositionsModels[2]);
            PositionsAlertsSetUp.AddPositionViaDB(positionsIds[1], listOfPositionsModels[3]);
            PositionsAlertsSetUp.AddPositionViaDB(positionsIds[1], listOfPositionsModels[4]);
            PositionsAlertsSetUp.AddPositionViaDB(positionsIds[2], listOfPositionsModels[5]);
            PositionsAlertsSetUp.AddPositionViaDB(positionsIds[2], listOfPositionsModels[6]);

            LoginSetUp.LogIn(UserModels.First());
            new MainMenuNavigation().OpenPureQuantRunsGrid();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_970$", DataAccessMethod.Sequential)]
        [TestMethod]
        [TestCategory("Smoke"), TestCategory("PureQuantTool"), TestCategory("MultiportfolioSelect")]
        [Description("The test checks that expected positions are shown on the grid when selected multiple portfolios https://tr.a1qa.com/index.php?/cases/view/19232137")]
        public override void RunTest()
        {
            LogStep(stepNumber++, "Click Build New Pure Quant Portfolio Button");
            new PureQuantRunInitForm().ClickBuildNewPureQuantPortfolio();
            var pureQuantStep1Form = new PureQuantStep1Form();

            LogStep(stepNumber++, "Select 'Multiple Portfolios'. Add MSFT and SAVE to Individual Securities");
            pureQuantStep1Form.SetSourcesCombination(StrategyFilterToggleState.Any);
            pureQuantStep1Form.SelectPortfolioMultipleByText(PureQuantDataSourceTypes.PortfoliosAndWatchlists, AllPortfoliosKinds.All.GetStringMapping());
            pureQuantStep1Form.SelectTickerForSource(PureQuantAutocompleteTypes.IndividualSecurities, listOfSymbolsToAddIndividual[0]);
            pureQuantStep1Form.SelectTickerForSource(PureQuantAutocompleteTypes.IndividualSecurities, listOfSymbolsToAddIndividual[1], false);

            LogStep(stepNumber++, "On Step 2 disable Gains and MaximizeDiversification. Click 'Build Portfolio' button");
            var pureQuantForm = new PureQuantCommonForm();
            pureQuantForm.ClickPureQuantStep(PureQuantInternalSteps.Step2ChangeDefaultSettings);
            var pureQuantStep2Form = new PureQuantStep2Form();
            pureQuantStep2Form.SetSliderFilter(PureQuantFilterTypes.Gains, SliderFilterStates.No);
            pureQuantStep2Form.SetSliderFilter(PureQuantFilterTypes.MaximizeDiversification, SliderFilterStates.No);
            pureQuantForm.ClickPureQuantStep(PureQuantInternalSteps.Step3CustomizePortfolio);
            pureQuantForm.RunResearchOrRecalculate();

            LogStep(stepNumber, "Make sure expected positions present on the grid.");
            new PureQuantProgressForm().WaitTaskFinishingWithClickingViewResult();
            var pureQuantResultsForm = new PureQuantResultsForm();
            var gridDataBeforeUpdating = pureQuantResultsForm.GetPureQuantGrid();
            CheckPositionsPresence(gridDataBeforeUpdating.Select(t => t.Ticker).ToList(), stepNumber);

            LogStep(++stepNumber, "UnCheck positions");
            foreach (var symbol in listOfSymbolsToAddIndividual)
            {
                pureQuantResultsForm.SetPositionCheckboxByTickerState(symbol, false);
            }

            LogStep(++stepNumber, "Click Update results");
            pureQuantForm.ClickUpdateResultsButton();
            pureQuantResultsForm = new PureQuantResultsForm();
            var gridDataAfterUpdating = pureQuantResultsForm.GetPureQuantGrid();
            CheckPositionsPresence(gridDataAfterUpdating.Select(t => t.Ticker).ToList(), stepNumber);

            LogStep(++stepNumber, "Check that unchecked positions have 0.00 in Shares");
            foreach (var model in listOfPositionsModels)
            {
                Checker.CheckNotEquals(gridDataBeforeUpdating.Where(t => t.Ticker == model.Symbol).ToList().First().Shares,
                    gridDataAfterUpdating.Where(t => t.Ticker == model.Symbol).ToList().First().Shares,
                    $"Shares for {model.Symbol} is not updated");
            }
            foreach (var symbol in listOfSymbolsToAddIndividual)
            {
                Checker.CheckEquals(SharesValueForExcludedTickers,
                    Constants.DecimalNumberWithIntegerPossibilityRegex.Match(gridDataAfterUpdating.Where(t => t.Ticker == symbol).ToList().First().Shares).Value,
                    $"Shares for individual Symbol {symbol} is not updated");
            }
        }

        private void CheckPositionsPresence(List<string> symbols, int step)
        {
            Assert.IsTrue(symbols.Count > 0, $"The grid does not contain positions - Step {step}");
            foreach (var model in listOfPositionsModels)
            {
                Checker.CheckEquals(SeparateTickerListingTimes, symbols.Count(u => u.Equals(model.Symbol)),
                    $"Symbol {model.Symbol} is not present or present more than 1 time - step {step}");
            }
            foreach (var symbol in listOfSymbolsToAddIndividual)
            {
                Checker.CheckEquals(SeparateTickerListingTimes, symbols.Count(u => u.Equals(symbol)),
                    $"Individual Symbol {symbol} is not present or present more than 1 time - step {step}");
            }
        }
    }
}