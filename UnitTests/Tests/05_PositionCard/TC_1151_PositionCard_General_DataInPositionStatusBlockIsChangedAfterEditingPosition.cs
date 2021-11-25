using System;
using System.Collections.Generic;
using System.Linq;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.PositionCard;
using AutomatedTests.Enums.Positions;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Models.PositionsModels;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeStops.Common.Enums;
using TradeStops.Common.Extensions;
using UnitTests.BaseTestsUnitTests;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._05_PositionCard
{
    [TestClass]
    public class TC_1151_PositionCard_General_DataInPositionStatusBlockIsChangedAfterEditingPosition : BaseTestUnitTests
    {
        private const int TestNumber = 1151;
        
        private readonly List<AddPositionAdvancedModel> positionsModels = new List<AddPositionAdvancedModel>();
        private List<string> ssiShortTicker;
        private List<string> ssiLongTicker;
        private List<string> ssiLongAdjStockTicker;
        private string sharesAfterEditing;
        private int monthShift;
        private int portfolioId;

        [TestInitialize]
        public void TestInitialize()
        {
            var portfolioModel = new PortfolioModel
            {
                Name = StringUtility.RandomString(GetTestDataAsString("PortfolioName")),
                Type = GetTestDataParsedAsEnumFromStringMapping<PortfolioType>("PortfolioType"),
                Currency = GetTestDataAsString("Currency")
            };
            var shares = GetTestDataAsString("Shares1");
            var entryDate = GetTestDataAsString("EntryDate");
            positionsModels.Add(new AddPositionAdvancedModel
            {
                Ticker = GetTestDataAsString("StockTicker"),
                IsLongTradeType = true,
                EntryDate = entryDate,
                IsAdjustByDividends = false,
                Shares = shares,
                ExitDate = Parsing.ConvertToShortDateString(DateTime.Now.ToShortDateString()),
                IsOpenStatusType = false,
                AssetType = GetTestDataParsedAsEnumFromStringMapping<PositionAssetTypes>("AssetType1"),
                Portfolio = portfolioModel.Name
            });
            positionsModels.Add(new AddPositionAdvancedModel
            {
                Ticker = GetTestDataAsString("CryptoTicker"),
                IsLongTradeType = true,
                EntryDate = entryDate,
                IsAdjustByDividends = false,
                Shares = shares,
                IsOpenStatusType = true,
                AssetType = GetTestDataParsedAsEnumFromStringMapping<PositionAssetTypes>("AssetType2"),
                Portfolio = portfolioModel.Name
            });
            positionsModels.Add(new AddPositionAdvancedModel
            {
                Ticker = GetTestDataAsString("ForexTicker"),
                IsLongTradeType = true,
                EntryDate = entryDate,
                IsAdjustByDividends = false,
                Shares = shares,
                ExitDate = Parsing.ConvertToShortDateString(DateTime.Now.ToShortDateString()),
                IsOpenStatusType = false,
                AssetType = GetTestDataParsedAsEnumFromStringMapping<PositionAssetTypes>("AssetType3"),
                Portfolio = portfolioModel.Name,
                Notes = GetTestDataAsString("ForexTicker")
            });
            positionsModels.Add(new AddPositionAdvancedModel
            {
                Ticker = GetTestDataAsString("OptionTicker"),
                IsLongTradeType = true,
                EntryDate = DateTime.Now.AddMonths(-1).ToShortDateString(),
                IsAdjustByDividends = false,
                Contracts = shares,
                IsOpenStatusType = true,
                AssetType = GetTestDataParsedAsEnumFromStringMapping<PositionAssetTypes>("AssetType4"),
                Portfolio = portfolioModel.Name
            });
            ssiShortTicker = GetTestDataValuesAsListByColumnName(nameof(ssiShortTicker));
            ssiLongTicker = GetTestDataValuesAsListByColumnName(nameof(ssiLongTicker));
            ssiLongAdjStockTicker = GetTestDataValuesAsListByColumnName(nameof(ssiLongAdjStockTicker));
            sharesAfterEditing = GetTestDataAsString("Shares2");
            monthShift = GetTestDataAsInt("MonthShift");

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, new List<ProductSubscriptions>
            {
                    ProductSubscriptions.TradeStopsPremium, ProductSubscriptions.CryptoStopsPremium
            }));
            portfolioId = PortfoliosSetUp.AddManualPortfolio(UserModels.First().Email, portfolioModel);

            LoginSetUp.LogIn(UserModels.First());
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_1151$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("The test checks Data in Position Status Block is Changed at Position Editing. https://tr.a1qa.com/index.php?/cases/view/19232035")]
        [TestCategory("Smoke"), TestCategory("ClosedPositionCard"), TestCategory("PositionCard"), TestCategory("ClosedPositionCardGeneral"), TestCategory("ClosedPositionCardStatistics"), TestCategory("Statistics")]
        public override void RunTest()
        {
            for (int i = 0; i < positionsModels.Count; i++)
            {
                PositionsAlertsSetUp.AddPositionFromAdvancedForm(portfolioId, positionsModels[i]);

                LogStep(1, "Open Position Card -> Position Details ");
                var positionCardForm = new PositionCardForm();
                var performanceTabPositionCardForm = positionCardForm.ActivateTabGetForm<PerformanceTabPositionCardForm>(PositionCardTabs.Performance);

                LogStep(2, "Remember:- SSI status;-Cost Basis;- Total Dividends;- Gain Per Share;-Total Gain");
                var ssi = positionCardForm.GetSsiStatus().ToString();
                var ticker = positionCardForm.GetSymbol();
                var costBasis = performanceTabPositionCardForm.GetPerformanceTabFieldValue(PerformanceTabFieldTypes.CostBasis);
                var totalDividends = performanceTabPositionCardForm.GetPerformanceTabFieldValue(PerformanceTabFieldTypes.TotalDividends);
                var gainPerShare = performanceTabPositionCardForm.GetPerformanceTabFieldValue(PerformanceTabFieldTypes.GainPerShare);
                var totalGainDollar = performanceTabPositionCardForm.GetPerformanceTabFieldValue(PerformanceTabFieldTypes.TotalGain).Split(' ')[0];
                var totalGainPercent = performanceTabPositionCardForm.GetPerformanceTabFieldValue(PerformanceTabFieldTypes.TotalGain).Split('(')[1].TrimEnd(')');
                var latestClosePrice = positionCardForm.GetPositionCardInfoFieldValue(PositionCardInfoFieldTypes.LatestClose);
                Checker.CheckEquals(ssiLongTicker[i], ssi, $"SSI status is not {ssiLongTicker[i]} for Long  + Non-Adj for {ticker}");

                LogStep(3, "Click Edit.Change Trade Type from 'Long' to 'Short'.Click 'Save'.");
                var positionDetailsTabPositionCardForm = positionCardForm.ActivateTabGetForm<PositionDetailsTabPositionCardForm>(PositionCardTabs.PositionDetails);
                positionDetailsTabPositionCardForm.EditPositionCard();
                positionDetailsTabPositionCardForm.SelectTradeType(PositionTradeTypes.Short);
                positionCardForm.ClickSave();

                LogStep(4, "Make sure value are changed:- SSI Status;");
                ssi = positionCardForm.GetSsiStatus().ToString();
                Checker.CheckEquals(ssiShortTicker[i], ssi, $"SSI status is not {ssiShortTicker[i]} for Short for {ticker}");

                LogStep(5, "Click Edit.Change Trade Type from 'Short' to 'Long'.Adjust alerts by dividends = Yes.Click 'Save'.");
                positionDetailsTabPositionCardForm.EditPositionCard();
                positionDetailsTabPositionCardForm.SelectTradeType(PositionTradeTypes.Long);
                positionCardForm.ClickSave();
                var id = positionCardForm.GetPositionIdFromUrl();
                var isDividendsAvailableForPositionId = new PositionsQueries().SelectAssetTypeNameByPositionId(id).In(PositionAssetTypes.Stock.GetStringMapping(), SymbolTypes.Fund.ToString());
                if (isDividendsAvailableForPositionId)
                {
                    positionDetailsTabPositionCardForm.EditPositionCard();
                    positionDetailsTabPositionCardForm.AdjustAlertsByDividends(true);
                    positionCardForm.ClickSave();

                    LogStep(6, "Make sure value are changed:- SSI Status");
                    ssi = positionCardForm.GetSsiStatus().ToString();
                    Checker.CheckEquals(ssiLongAdjStockTicker[i], ssi, $"SSI status is not {ssiLongAdjStockTicker} for Long  + Adj for {ticker}");
                }

                LogStep(7, "Click Edit.Change 'Entry Date' to 01/01/2017.Click 'Get Quote' button.Click 'Save'.");
                positionDetailsTabPositionCardForm.EditPositionCard();
                positionDetailsTabPositionCardForm.SetValueInTextBoxField(PositionDetailsFieldTypes.EntryDate, Parsing.ConvertToShortDateString(DateTime.Now.AddMonths(-monthShift).ToShortDateString()));
                positionDetailsTabPositionCardForm.ClickGetQuote();
                positionDetailsTabPositionCardForm.SetValueInTextBoxField(positionDetailsTabPositionCardForm.GetSharesTypeFieldWithDetectingType(), sharesAfterEditing);
                positionCardForm.ClickSave();

                LogStep(8, "Make sure value is changed:- Cost Basis;- Value;- Total Dividends;- Gain Per Share;- Total Gain");
                performanceTabPositionCardForm = positionCardForm.ActivateTabGetForm<PerformanceTabPositionCardForm>(PositionCardTabs.Performance);
                Checker.CheckNotEquals(performanceTabPositionCardForm.GetPerformanceTabFieldValue(PerformanceTabFieldTypes.CostBasis), costBasis,
                    $"Cost Basis was not changed after changing Entry Date for {ticker}");
                if (isDividendsAvailableForPositionId)
                {
                    Checker.CheckNotEquals(performanceTabPositionCardForm.GetPerformanceTabFieldValue(PerformanceTabFieldTypes.TotalDividends), totalDividends,
                    $"Total Dividends was not changed after changing Entry Date for {ticker}");
                }
                if (string.IsNullOrEmpty(positionsModels[i].Notes) && positionsModels[i].AssetType != PositionAssetTypes.Option)
                {
                    Checker.CheckNotEquals(performanceTabPositionCardForm.GetPerformanceTabFieldValue(performanceTabPositionCardForm.DetectAndGetSharesTypeField()), gainPerShare,
                    $"Gain Per Share was not changed after changing Entry Date for {ticker}");
                }
                Checker.CheckNotEquals(performanceTabPositionCardForm.GetPerformanceTabFieldValue(PerformanceTabFieldTypes.TotalGain).Split(' ')[0], totalGainDollar,
                    $"Total Gain Money was not changed after changing Entry Date for {ticker}");
                Checker.CheckNotEquals(performanceTabPositionCardForm.GetPerformanceTabFieldValue(PerformanceTabFieldTypes.TotalGain).Split('(')[1].TrimEnd(')'), totalGainPercent,
                    $"Total Gain Percent was not changed after changing Entry Date for {ticker}");
                Checker.CheckEquals(positionCardForm.GetPositionCardInfoFieldValue(PositionCardInfoFieldTypes.LatestClose), latestClosePrice,
                    $"Latest Close Price was changed after changing Entry Date for {ticker}");
            }
        }
    }
}