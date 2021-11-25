using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.PositionData;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.PositionCard;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.PositionCard;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Enums;
using UnitTests.BaseTestsUnitTests;

namespace UnitTests.Tests._05_PositionCard
{
    [TestClass]
    public class TC_0593_PositionCard_CorrespondenceDataBetweenPositionGridAndStatBlock : BaseTestUnitTests
    {
        private const int TestNumber = 484;

        private List<PositionsGridDataField> positionsViewColumns;
        private readonly List<int> positionsIds = new List<int>();
        private string viewName;
        private string optionExchange;
        private string divYield = Constants.NotAvailableAcronym;
        private string avgVq = Constants.NotAvailableAcronym;

        [TestInitialize]
        public void TestInitialize()
        {
            var portfolioModel = new PortfolioModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency")
            };
            var positionsQuantity = GetTestDataAsInt("positionsQuantity");
            var positionsModels = new List<PositionsDBModel>();
            for (int i = 1; i <= positionsQuantity; i++)
            {
                positionsModels.Add(new PositionsDBModel
                {
                    Symbol = GetTestDataAsString($"SymbolSpa{i}"),
                    TradeType = GetTestDataAsString($"TradeType{i}"),
                    PurchaseDate = GetTestDataAsString($"DateSpa{i}")
                });
            }
            var entryCommission = GetTestDataAsString("entryCommission");
            var exitCommission = GetTestDataAsString("exitCommission");
            optionExchange = GetTestDataAsString(nameof(optionExchange));

            positionsViewColumns = new List<PositionsGridDataField>
            {
                PositionsGridDataField.TotalDividends, PositionsGridDataField.CostBasis, PositionsGridDataField.Value, PositionsGridDataField.GainWithDivPercentage,
                PositionsGridDataField.GainWithDiv, PositionsGridDataField.GainPerShareWithDiv, PositionsGridDataField.LatestClose, PositionsGridDataField.Volume,
                PositionsGridDataField.Exchange, PositionsGridDataField.Average30YearsVolatilityQuotient, PositionsGridDataField.TrailingDividendYield
            };

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, new List<ProductSubscriptions>
            {
                    ProductSubscriptions.TradeStopsPremium, ProductSubscriptions.CryptoStopsPremium
            }));
            var portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);
            new PortfoliosQueries().UpdateCommissionInPortfoliosByPortfolioId(entryCommission, exitCommission, portfolioId);
            foreach (var positionModel in positionsModels)
            {
                positionsIds.Add(PositionsAlertsSetUp.AddPositionViaDB(portfolioId, positionModel));
            }
            Assert.IsTrue(positionsIds.Count > 0, "There are no positions");

            LoginSetUp.LogIn(UserModels.First());

            viewName = ViewSetups.AddNewCustomViewForPositionsTabWithAllColumns();
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_593$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test checks matching data between Position Card and Positions grid. https://tr.a1qa.com/index.php?/cases/view/19232575")]
        [TestCategory("Smoke"), TestCategory("PositionCard"), TestCategory("PositionCardGeneral"), TestCategory("PositionCardStatistics"), TestCategory("PositionsGrid"), TestCategory("Statistics")]
        public override void RunTest()
        {
            foreach (var positionId in positionsIds)
            {
                LogStep(1, "Open position card for the position");
                new PositionCardSteps().ResavePositionCard(positionId);

                LogStep(2, "Go to 'Performance' section'.Remember values:-Cost Basis;- Value;- Total Dividends;- Gain Per Share;- $ Total Gain;- % Total Gain");
                var positionCardForm = new PositionCardForm();
                var performanceTabPositionCardForm = positionCardForm.ActivateTabGetForm<PerformanceTabPositionCardForm>(PositionCardTabs.Performance);
                var costBasis = performanceTabPositionCardForm.GetPerformanceTabFieldValue(PerformanceTabFieldTypes.CostBasis);
                var value = performanceTabPositionCardForm.GetPerformanceTabFieldValue(PerformanceTabFieldTypes.Value);
                var totalDividends = performanceTabPositionCardForm.GetPerformanceTabFieldValue(PerformanceTabFieldTypes.TotalDividends);
                var gainPerShare = performanceTabPositionCardForm.GetPerformanceTabFieldValue(performanceTabPositionCardForm.DetectAndGetSharesTypeField());
                var dollarTotalGain = performanceTabPositionCardForm.GetPerformanceTabFieldValue(PerformanceTabFieldTypes.TotalGainDollar);
                var percentTotalGain = performanceTabPositionCardForm.GetPerformanceTabFieldValue(PerformanceTabFieldTypes.TotalGainPercent);

                LogStep(3, "Go to 'General Statistics' grid.Make sure that 'Latest Close' value is correct.Remember values:-Latest Close;- Volume");
                var generalStatisticTabPositionCardForm = positionCardForm.ActivateTabGetForm<StatisticTabForm>(PositionCardTabs.Statistics);
                generalStatisticTabPositionCardForm.AssertIsOpen();
                var latestCloseFromGeneralStatTabs = generalStatisticTabPositionCardForm.GetGeneralStatisticTabTabFieldValue(GeneralStatisticsFieldTypes.LatestClose);
                var volume = generalStatisticTabPositionCardForm.GetGeneralStatisticTabTabFieldValue(GeneralStatisticsFieldTypes.Volume);
                var ticker = positionCardForm.GetSymbol();
                var positionsQueries = new PositionsQueries();
                var assetType = positionsQueries.SelectAssetTypeNameByPositionId(positionCardForm.GetPositionIdFromUrl());
                if (assetType != PositionAssetTypes.Option.ToString())
                {
                    divYield = generalStatisticTabPositionCardForm.GetGeneralStatisticTabTabFieldValue(GeneralStatisticsFieldTypes.DividendYield);
                    avgVq = generalStatisticTabPositionCardForm.GetGeneralStatisticTabTabFieldValue(GeneralStatisticsFieldTypes.AverageVq);
                }
                var positionDbData = positionsQueries.SelectAllPositionData(positionId);
                var symbolStatistic = new PositionDataQueries().SelectSymbolStatisticsForSymbol(ticker);
                try
                {
                    Assert.AreEqual(symbolStatistic.LatestClose.ToFractionalString(),
                        StringUtility.ReplaceAllCurrencySigns(latestCloseFromGeneralStatTabs).Replace(",", string.Empty),
                        $"'Latest Close' value is not matched with step #2 on 'General Statistics' tab for {ticker}");
                }
                catch (AssertFailedException)
                {
                    Checker.CheckEquals(positionDbData.ClosePrice.ToFractionalString(),
                        StringUtility.ReplaceAllCurrencySigns(latestCloseFromGeneralStatTabs).Replace(",", string.Empty),
                        $"'Latest Close' value is not matched with step #2 on 'General Statistics' tab for {ticker}");
                }

                LogStep(4, "Open Positions grid ");
                new MainMenuNavigation().OpenPositionsGrid();
                var positionsTab = new PositionsTabForm();
                positionsTab.SelectView(viewName);

                LogStep(5, "Make sure that data in columns is correct");
                var positionData = positionsTab.GetPositionDataByPositionId(positionsViewColumns, positionId);
                Checker.CheckEquals(costBasis, positionData.CostBasis, $"Total C/B does not equal to Cost Basis (step #2) for {ticker}");
                Checker.CheckEquals(value, positionData.Value, $"Value does not equal to Value (step #2) for {ticker}");
                Checker.CheckEquals(percentTotalGain, positionData.GainWithDivPercentage, $"Gain % w/ Div != Total Gain, % (step #2) for {ticker}");
                Checker.CheckEquals(dollarTotalGain, positionData.GainWithDiv, $"Gain w/ Div != Total  Gain, $ (step #2) for {ticker}");
                Checker.CheckEquals(StringUtility.ReplaceAllCurrencySigns(latestCloseFromGeneralStatTabs).ToFractionalString(),
                    StringUtility.ReplaceAllCurrencySigns(positionData.LatestClose).Replace(",", string.Empty),
                    $"Latest Close != Latest Close (step #2) for {ticker}");
                Checker.CheckEquals(volume, positionData.Volume, $"Volume != Volume (step #3) for {ticker}");
                if (assetType != PositionAssetTypes.Option.ToString() && assetType != PositionAssetTypes.Crypto.ToString())
                {
                    Checker.CheckEquals(divYield, positionData.TrailingDividendYield, $"Dividend Yield != Dividend Yield (step #2) for {ticker}");
                }
                if (assetType != PositionAssetTypes.Option.ToString())
                {
                    Checker.CheckEquals(totalDividends, positionData.TotalDividends, $"Total Dividends != Total Dividends (step #2) for {ticker}");
                    Checker.CheckEquals(gainPerShare, positionData.GainPerShareWithDiv, $"Gain/Shr w/ Div != Gain Per Share (step #2) for {ticker}");
                    Checker.CheckEquals(avgVq, positionData.Average30YearsVolatilityQuotient, $"Average VQ != Average VQ (step #2) for {ticker}");
                }
            }
        }
    }
}