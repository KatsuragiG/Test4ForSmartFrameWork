using System;
using AutomatedTests.ConstantVariables;
using AutomatedTests.Database.Portfolios;
using AutomatedTests.Database.Positions;
using AutomatedTests.Database.PositionStats;
using AutomatedTests.Enums.Alerts;
using AutomatedTests.Enums.Portfolios;
using AutomatedTests.Enums.PositionCard;
using AutomatedTests.Forms.PositionsAlertsForm.Forms;
using AutomatedTests.Forms.PositionCard;
using AutomatedTests.Navigation;
using AutomatedTests.SetUpsTearDowns;
using AutomatedTests.Steps.PreconditionsSteps;
using AutomatedTests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.BaseTestsUnitTests;
using AutomatedTests.Models.PositionsModels;
using AutomatedTests.Enums.Positions;
using TradeStops.Common.Enums;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TradeStops.Common.Extensions;
using WebdriverFramework.Framework.Util;

namespace UnitTests.Tests._05_PositionCard
{
    [TestClass]
    public class TC_0484_PositionCard_PositionDetails_MatchingOfDataWithPositionsndAlertsGrids : BaseTestUnitTests
    {
        private const int TestNumber = 484;
        private const int DecimalsQuantityToCompare = 2;
        private const string ViewNameForAddedView = "view";

        private AddPositionAdvancedModel positionModel;
        private PositionsDBModel positionDataFromDb;
        private PositionStatsModel positionStats;
        private List<PositionsGridDataField> positionsViewColumns;
        private List<AlertsGridColumnsDataField> alertsViewColumns;
        private int positionId;
        private string addedSignForShort = "";
        private string daysBeforeExpiration;

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
            var optionContract = GetTestDataAsString("Contract1");
            var optionExpirationDate = GetTestDataAsString("ExpirationDate1");
            var optionStrikePrice = GetTestDataAsString("ExpirationPrice1");
            var optionType = GetTestDataAsString("StrikeType1");
            positionModel = new AddPositionAdvancedModel
            {
                Ticker = GetTestDataAsString("Symbol1"),
                Shares = shares == string.Empty ? null : shares,
                IsLongTradeType = GetTestDataAsBool("LongType1"),
                EntryDate = GetTestDataAsString("EntryDate1"),
                EntryPrice = GetTestDataAsString("EntryPrice1"),
                EntryCommission = GetTestDataAsDecimal("EntryCommission1"),
                AssetType = GetTestDataParsedAsEnumFromStringMapping<PositionAssetTypes>("AssetType"),
                IsAdjustByDividends = GetTestDataAsBool("AdjustAlertByDividends1"),
                Portfolio = portfolioModel.Name,
                Contracts = optionContract == string.Empty ? null : optionContract,
                ExpirationDate = optionExpirationDate == string.Empty ? null : optionExpirationDate,
                StrikePrice = optionStrikePrice == string.Empty ? null : optionStrikePrice,
                OptionType = optionType == string.Empty ? null : optionType
            };
            if (!(bool)positionModel.IsLongTradeType)
            {
                addedSignForShort = Constants.MinusSign;
            }

            positionsViewColumns = new List<PositionsGridDataField>
            {
                PositionsGridDataField.EntryDate, PositionsGridDataField.EntryPrice, PositionsGridDataField.Shares, PositionsGridDataField.Commissions,
                PositionsGridDataField.MaxProfitableClose, PositionsGridDataField.TradeType, PositionsGridDataField.DaysHeld,
                PositionsGridDataField.MaxProfitableCloseDate, PositionsGridDataField.DaysHeld, PositionsGridDataField.DaysBeforeExpiration
            };
            alertsViewColumns = new List<AlertsGridColumnsDataField>
            {
                AlertsGridColumnsDataField.PercentOffHigh, AlertsGridColumnsDataField.EntryDate, AlertsGridColumnsDataField.EntryPrice,
                AlertsGridColumnsDataField.MaxProfitableClose, AlertsGridColumnsDataField.TradeType, AlertsGridColumnsDataField.MaxProfitableCloseDate
            };

            LogStep(0, "Preconditions");
            UserModels.Add(ApiClientSetUp.CreateUserWithSubscriptions(TestNumber, new List<ProductSubscriptions>
            {
                    ProductSubscriptions.TradeStopsPremium, ProductSubscriptions.CryptoStopsPremium
            }));
            new PreconditionsCommonSteps().AddPositionViaAdvancedFormGetPositionDataGetPositionStatisticAddTs25AlertToAllPositionsAddViews(
                UserModels.First(), portfolioModel, positionModel, ViewNameForAddedView, ViewNameForAddedView, out positionDataFromDb, out positionStats,
                out _, out positionId);
        }

        [DeploymentItem("TestEnvironmentConfiguration\\")]
        [DeploymentItem("chromedriver.exe"), DeploymentItem("geckodriver.exe"), DeploymentItem("IEDriverServer.exe")]
        [DeploymentItem("TSPData.xlsx")]
        [DataSource(DataProviderName, DataProviderConnectionStr, "TC_484$", DataAccessMethod.Sequential)]
        [TestMethod]
        [Description("https://tr.a1qa.com/index.php?/cases/view/19232559 The test checks the matching data between position card, the Position grid and the Alerts grid")]
        [TestCategory("Smoke"), TestCategory("PositionCard"), TestCategory("PositionCardPositionDetailsTab"), TestCategory("PositionsGrid"), TestCategory("AlertsGrid")]
        public override void RunTest()
        {
            LogStep(1, "Click on position (precondition #2-1) and open New Position Card -> 'Alerts' tab. ");
            new MainMenuNavigation().OpenPositionCard(positionId);
            var positionCardForm = new PositionCardForm();
            var positionDetailsTabPositionCardForm = positionCardForm.ActivateTabGetForm<PositionDetailsTabPositionCardForm>(PositionCardTabs.PositionDetails);

            LogStep(2, "Make sure that values is shown as expectedly on strings:- Entry Date- Entry Price- Shares / Contracts- Entry Commission- Trade Type" +
                "- Adjust alerts by dividends- Max Profitable Close Date- Max Profitable Close- % Off High- Days Held");
            var entryDate = positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.EntryDate);
            var entryPrice = StringUtility.ReplaceAllCurrencySigns(positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.EntryPrice));
            var shares = positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(positionDetailsTabPositionCardForm.GetSharesTypeFieldWithDetectingType());
            var entryCommission = StringUtility.ReplaceAllCurrencySigns(positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.EntryCommission));
            var isLongTradeType = positionDetailsTabPositionCardForm.IsTradeTypeLong();
            var adjustAlert = positionDetailsTabPositionCardForm.GetAdjustAlertsByDividends();
            if (positionModel.AssetType == PositionAssetTypes.Option)
            {
                daysBeforeExpiration = positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.DaysToExpiration);
            }
            var daysHeld = positionDetailsTabPositionCardForm.GetPositionDetailsFieldValue(PositionDetailsFieldTypes.DaysHeld);
            var performanceTabPositionCardForm = positionCardForm.ActivateTabGetForm<PerformanceTabPositionCardForm>(PositionCardTabs.Performance);
            var maxProfitableCloseDate = performanceTabPositionCardForm.GetPerformanceTabFieldValue(PerformanceTabFieldTypes.MaxProfitableCloseDate);
            var maxProfitableClose = StringUtility.ReplaceAllCurrencySigns(performanceTabPositionCardForm.GetPerformanceTabFieldValue(PerformanceTabFieldTypes.MaxProfitableClose));
            var percentOffHigh = performanceTabPositionCardForm.GetPerformanceTabFieldValue(PerformanceTabFieldTypes.PercentOffHigh).Replace("%", string.Empty);


            Checker.CheckEquals(Parsing.ConvertToShortDateString(entryDate), Parsing.ConvertToShortDateString(positionDataFromDb.PurchaseDate), "2 entry date is not equals");
            Checker.CheckEquals(Math.Round(Parsing.ConvertToDecimal(entryPrice), DecimalsQuantityToCompare),
                Math.Round(Parsing.ConvertToDecimal(positionDataFromDb.PurchasePrice), DecimalsQuantityToCompare), "2 entry price is not equals");
            Checker.CheckEquals(Math.Round(Parsing.ConvertToDecimal(shares), DecimalsQuantityToCompare),
                Math.Round(Parsing.ConvertToDecimal($"{addedSignForShort}{positionDataFromDb.Shares}"), DecimalsQuantityToCompare), "2 shares is not equals");
            Checker.CheckEquals(Math.Round(Parsing.ConvertToDouble(entryCommission), DecimalsQuantityToCompare),
                Math.Round(Parsing.ConvertToDouble(positionDataFromDb.OpenFee), DecimalsQuantityToCompare), "2 entry Commission is not equals");
            Checker.CheckEquals(isLongTradeType, positionDataFromDb.TradeType == $"{(int)PositionTradeTypes.Long}", "2 trade Type is not equals");
            Checker.CheckEquals(adjustAlert, !Parsing.ConvertToBool(positionDataFromDb.IgnoreDividend), "2 adjust Alert by dividends is not equals");
            Checker.CheckEquals(Parsing.ConvertToShortDateString(maxProfitableCloseDate),
                positionModel.IsLongTradeType.HasValue && (bool)positionModel.IsLongTradeType
                    ? Parsing.ConvertToShortDateString(positionStats.HighestCloseDate)
                    : Parsing.ConvertToShortDateString(positionStats.LowestCloseDate),
                "2 max Profitable Close Date is not equals");
            Checker.CheckEquals(Math.Round(Parsing.ConvertToDecimal(maxProfitableClose), DecimalsQuantityToCompare),
                Math.Round(Parsing.ConvertToDecimal((bool)positionModel.IsLongTradeType
                    ? positionStats.HighestClosePrice
                    : positionStats.LowestClosePrice), DecimalsQuantityToCompare),
                "2 maxProfitableClose is not equals");
            Checker.CheckEquals(percentOffHigh, Math.Round(Parsing.ConvertToDecimal(positionDataFromDb.PercentOffHLClose),
                DecimalsQuantityToCompare).ToString("N"), "2 percent Off High is not equals");

            LogStep(3, "Open Positions grid (*cbe.qa-auto.tradestops.com/alerts?tab=1*). Make sure that values are matched with the Position Card.Columns:" +
                "- % Off High- Entry Date- Entry Price- Shares- Commissions- Max Profitable Close- Max Profitable Close Date- L / S- Days Held");
            new MainMenuNavigation().OpenPositionsGrid();
            var positionTabForm = new PositionsTabForm();
            var positionData = positionTabForm.GetPositionDataForAllPositions(positionsViewColumns).First();
            Checker.CheckEquals(percentOffHigh, Math.Round(Parsing.ConvertToDecimal(positionDataFromDb.PercentOffHLClose), DecimalsQuantityToCompare).ToString("N"),
                "3 percent Off High is not equals");
            Checker.CheckEquals(Parsing.ConvertToShortDateString(entryDate), Parsing.ConvertToShortDateString(positionData.EntryDate), "3 entry date is not equals");
            Checker.CheckEquals(Math.Round(Parsing.ConvertToDecimal(entryPrice), DecimalsQuantityToCompare),
                Math.Round(Parsing.ConvertToDecimal(StringUtility.ReplaceAllCurrencySigns(positionData.EntryPrice)), DecimalsQuantityToCompare),
                "3 entry price is not equals");
            Checker.CheckEquals(Math.Round(Parsing.ConvertToDecimal(positionModel.AssetType == PositionAssetTypes.Option
                    ? (Parsing.ConvertToDecimal(shares) * Constants.DefaultContractSize).ToString(CultureInfo.InvariantCulture)
                    : shares), DecimalsQuantityToCompare),
                Math.Round(Parsing.ConvertToDecimal(positionData.Shares), DecimalsQuantityToCompare), "3 shares is not equals");
            Checker.CheckEquals(Math.Round(Parsing.ConvertToDouble(entryCommission), DecimalsQuantityToCompare),
                Math.Round(Parsing.ConvertToDouble(StringUtility.ReplaceAllCurrencySigns(positionData.Commissions)), DecimalsQuantityToCompare),
                "3 entry Commission is not equals");
            Checker.CheckEquals(Math.Round(Parsing.ConvertToDecimal(maxProfitableClose), DecimalsQuantityToCompare),
                Math.Round(Parsing.ConvertToDecimal(StringUtility.ReplaceAllCurrencySigns(positionData.MaxProfitableClose)), DecimalsQuantityToCompare),
                "3 maxProfitableClose is not equals");
            Checker.CheckEquals(isLongTradeType, positionData.TradeType.EqualsIgnoreCase(PositionTradeTypes.Long.GetStringMapping()), "3 trade Type is not equals");
            Checker.CheckEquals(Parsing.ConvertToShortDateString(maxProfitableCloseDate), Parsing.ConvertToShortDateString(positionData.MaxProfitableCloseDate),
                "3 max Profitable Close Date is not equals");
            Checker.CheckEquals(daysHeld, positionData.DaysHeld, "3 Days Held is not equals");
            if (positionModel.AssetType == PositionAssetTypes.Option)
            {
                Checker.CheckEquals(daysBeforeExpiration, positionData.DaysBeforeExpiration, "3 Days Before Expiration is not equals");
            }

            LogStep(4, "Open Alerts grid (*cbe.qa-auto.tradestops.com/alerts?tab=2*). Make sure that values are matched with the Position Card Columns:" +
                "- % Off High- Entry Date- Entry Price- Max Profitable Close- Max Profitable Close Date- L / S");
            new MainMenuNavigation().OpenAlertsGrid();
            var alertsTabForm = new AlertsTabForm();
            var alertData = alertsTabForm.GetAlertInformation(alertsViewColumns, 1);
            Checker.CheckEquals(percentOffHigh, alertData.PercentOffHigh.Replace("%", string.Empty), "3 percent Off High is not equals");
            Checker.CheckEquals(Parsing.ConvertToShortDateString(entryDate), Parsing.ConvertToShortDateString(alertData.EntryDate), "3 entry date is not equals");
            Checker.CheckEquals(Math.Round(Parsing.ConvertToDecimal(entryPrice), DecimalsQuantityToCompare),
                Math.Round(Parsing.ConvertToDecimal(StringUtility.ReplaceAllCurrencySigns(alertData.EntryPrice)), DecimalsQuantityToCompare), "3 entry price is not equals");
            Checker.CheckEquals(Math.Round(Parsing.ConvertToDecimal(maxProfitableClose), DecimalsQuantityToCompare),
                Math.Round(Parsing.ConvertToDecimal(StringUtility.ReplaceAllCurrencySigns(alertData.MaxProfitableClose)), DecimalsQuantityToCompare),
                "3 maxProfitableClose is not equals");
            Checker.CheckEquals(isLongTradeType, alertData.TradeType.EqualsIgnoreCase(PositionTradeTypes.Long.GetStringMapping()), "3 trade Type is not equals");
            Checker.CheckEquals(Parsing.ConvertToShortDateString(maxProfitableCloseDate), Parsing.ConvertToShortDateString(alertData.MaxProfitableCloseDate),
                "3 max Profitable Close Date is not equals");
        }
    }
}